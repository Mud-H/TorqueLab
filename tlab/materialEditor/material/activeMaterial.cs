//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$MatEdDblUpdate = false;

//==============================================================================
// Helper functions to help load and update the preview and active material
//------------------------------------------------------------------------------
//==============================================================================
// Finds the selected line in the material list, then makes it active in the editor.
function MaterialEditorTools::prepareActiveMaterial(%this, %material, %override) {
	// If were not valid, grab the first valid material out of the materialSet
	if( !isObject(%material) ) {
		%material = MaterialSet.getObject(0);
		%isNew = true;
	}

	// Check made in order to avoid loading the same material. Overriding
	// made in special cases
	if(%material $= MaterialEditorTools.lastMaterial && !%override) {
		return;
	} else {
		if(MaterialEditorTools.materialDirty ) {
			MaterialEditorTools.showSaveDialog( %material );
			return;
		}

		MaterialEditorTools.setActiveMaterial(%material,%isNew);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Updates the preview material to use the same properties as the selected material,
// and makes that material active in the editor.
//MaterialEditorTools.setActiveMaterial(test_cuboidTestF);
function MaterialEditorTools::setActiveMaterial( %this, %material,%isNew ) {
	// Warn if selecting a CustomMaterial (they can't be properly previewed or edited)
	if ( isObject( %material ) && %material.isMemberOfClass( "CustomMaterial" ) ) {
		LabMsgOK( "Warning", "The selected Material (" @ %material.getName() @
					 ") is a CustomMaterial, and cannot be edited using the Material Editor." );
		return;
	}

	if (strFind(%material.getFilename(),"tlab/materialEditor") || %material.getFilename() $= "") {
		%material.setFilename($Pref::MaterialEditorTools::DefaultMaterialFile);
	}

	MaterialEditorTools.currentMaterial = %material;
	MaterialEditorTools.lastMaterial = %material;
	%this.setActiveMaterialFile(%material);
	// we create or recreate a material to hold in a pristine state
	singleton Material(notDirtyMaterial) {
		mapTo = "matEd_previewMat";
		diffuseMap[0] = "tlab/materialEditor/assets/matEd_mappedMat";
	};
	// Converts the texture files into absolute paths.
	MaterialEditorTools.convertTextureFields();
	// If we're allowing for name changes, make sure to save the name seperately
	%this.originalName = MaterialEditorTools.currentMaterial.name;
	// Copy materials over to other references
	MaterialEditorTools.copyMaterials( MaterialEditorTools.currentMaterial, materialEd_previewMaterial );
	MaterialEditorTools.copyMaterials( MaterialEditorTools.currentMaterial, notDirtyMaterial );
	MaterialEditorTools.guiSync( materialEd_previewMaterial );
	// Necessary functionality in order to render correctly
	materialEd_previewMaterial.flush();
	materialEd_previewMaterial.reload();
	MaterialEditorTools.currentMaterial.flush();
	MaterialEditorTools.currentMaterial.reload();
	MaterialEditorTools.setMaterialNotDirty();

	if (%isNew)
		MaterialEditorTools.currentMaterial.metalness[0] = "0";
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::setActiveMaterialFile( %this, %material ) {
	MatEdTargetMode-->selMaterialFile.setText(%material.getFilename());
	MatEdMaterialMode-->selMaterialFile.setText(%material.getFilename());
}
//------------------------------------------------------------------------------
// Accumulation
function MaterialEditorTools::updateAccuCheckbox(%this, %value) {
	MaterialEditorTools.updateActiveMaterial("accuEnabled[" @ MaterialEditorTools.currentLayer @ "]", %value);
	%mat = MaterialEditorTools.currentMaterial;
	MaterialEditorTools.guiSync( materialEd_previewMaterial );
}

//==============================================================================
function MaterialEditorTools::updateMaterialPBR(%this, %propertyField,%value, %isSlider, %onMouseUp) {
	devLog(" MaterialEditorTools::updateMaterialPBR(%this, %propertyField, %value, %isSlider, %onMouseUp)", %this, %propertyField, %value, %isSlider, %onMouseUp);
	%mat = MaterialEditorTools.currentMaterial;
	%layer = MaterialEditorTools.currentLayer;
	%isDirty = LabObj.set(%mat,%propertyField,%value,%layer);

	if (%isDirty)
		MaterialEditorTools.setMaterialDirty();

	%field = %propertyField@"[" @%layer @ "]";
	eval("materialEd_previewMaterial." @ %field @ " = " @ %value @ ";");
	materialEd_previewMaterial.flush();
	materialEd_previewMaterial.reload();

	if ($MatEdDblUpdate)
		%this.updateActiveMaterial(%field,%value, %isSlider, %onMouseUp);
}
//==============================================================================
function MaterialEditorTools::updateActiveMaterial(%this, %propertyField, %value, %isSlider, %onMouseUp) {
	logc(" MaterialEditorTools::updateActiveMaterial(%this, %propertyField, %value, %isSlider, %onMouseUp)", %this, %propertyField, %value, %isSlider, %onMouseUp);
	MaterialEditorTools.setMaterialDirty();

	if(%value $= "")
		%value = "\"\"";

	// Here is where we handle undo actions with slider controls. We want to be able to
	// undo every onMouseUp; so we overrite the same undo action when necessary in order
	// to achieve this desired effect.
	%last = Editor.getUndoManager().getUndoAction(Editor.getUndoManager().getUndoCount() - 1);

	if((%last != -1) && (%last.isSlider) && (!%last.onMouseUp)) {
		%last.field = %propertyField;
		%last.isSlider = %isSlider;
		%last.onMouseUp = %onMouseUp;
		%last.newValue = %value;
	} else {
		%action = %this.createUndo(ActionUpdateActiveMaterial, "Update Active Material");
		%action.material = MaterialEditorTools.currentMaterial;
		%action.object = MaterialEditorTools.currentObject;
		%action.field = %propertyField;
		%action.isSlider = %isSlider;
		%action.onMouseUp = %onMouseUp;
		%action.newValue = %value;
		eval( "%action.oldValue = " @ MaterialEditorTools.currentMaterial @ "." @ %propertyField @ ";");
		%action.oldValue = "\"" @ %action.oldValue @ "\"";
		MaterialEditorTools.submitUndo( %action );
	}

	if (!isObject(materialEd_previewMaterial))
		MaterialEditorTools.establishMaterials();

	eval("materialEd_previewMaterial." @ %propertyField @ " = " @ %value @ ";");
	materialEd_previewMaterial.flush();
	materialEd_previewMaterial.reload();

	if (MaterialEditorTools.livePreview == true) {
		eval("MaterialEditorTools.currentMaterial." @ %propertyField @ " = " @ %value @ ";");
		MaterialEditorTools.currentMaterial.flush();
		MaterialEditorTools.currentMaterial.reload();
	}

	if(strFind(%propertyField,"diffuseMap") && %autoUpdateNormal) {
		%diffuseSuffix = MaterialEditorPlugin.getCfg("DiffuseSuffix");
		%cleanValue = strreplace(%value,%diffuseSuffix,"");
		%autoUpdateNormal = MaterialEditorPlugin.getCfg("AutoAddNormal");

		if (%autoUpdateNormal) {
			%normalMap = strreplace(%propertyField,"diffuse","normal");
			%normalFile = MaterialEditorTools.currentMaterial.getFieldValue(%normalMap);
			%isFileNormal = isImageFile(%normalFile);

			if (!%isFileNormal) {
				%normalSuffix = MaterialEditorPlugin.getCfg("NormalSuffix");
				%fileBase = fileBase(%cleanValue);
				%filePath = filePath(%cleanValue);
				%testNormal = %filePath@"/"@%fileBase@%normalSuffix;
				%isFileNew = isImageFile(%testNormal);

				if (%isFileNew)
					%this.updateTextureMapImage("normal",%testNormal,0);
			}
		}

		%autoUpdateSpecular = MaterialEditorPlugin.getCfg("AutoAddSpecular");

		if (%autoUpdateSpecular) {
			%specMap = strreplace(%propertyField,"diffuse","specular");
			%specFile = MaterialEditorTools.currentMaterial.getFieldValue(%specMap);
			%isFileSpec = isImageFile(%specFile);

			if (!%isFileSpec) {
				%specSuffix = MaterialEditorPlugin.getCfg("SpecularSuffix");
				%fileBase = fileBase(%cleanValue);
				%filePath = filePath(%cleanValue);
				%testSpec = %filePath@"/"@%fileBase@%specSuffix;
				//%testSpec = strreplace(%testSpec,"\"","");
				%isFileNew = isImageFile(%testSpec);

				if (%isFileNew)
					%this.updateTextureMapImage("spec",%testSpec,0);
			}
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateActiveMaterialName(%this, %name) {
	%action = %this.createUndo(ActionUpdateActiveMaterialName, "Update Active Material Name");
	%action.material =  MaterialEditorTools.currentMaterial;
	%action.object = MaterialEditorTools.currentObject;
	%action.oldName = MaterialEditorTools.currentMaterial.getName();
	%action.newName = %name;
	MaterialEditorTools.submitUndo( %action );
	MaterialEditorTools.currentMaterial.setName(%name);
	// Some objects (ConvexShape, DecalRoad etc) reference Materials by name => need
	// to find and update all these references so they don't break when we rename the
	// Material.
	MaterialEditorTools.updateMaterialReferences( MissionGroup, %action.oldName, %action.newName );
}


