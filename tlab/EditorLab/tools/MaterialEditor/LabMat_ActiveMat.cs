//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Helper functions to help load and update the preview and active material
//------------------------------------------------------------------------------
//==============================================================================
// Finds the selected line in the material list, then makes it active in the editor.
function LabMat::prepareActiveMaterial(%this, %material, %override) {
	// If were not valid, grab the first valid material out of the materialSet
	if( !isObject(%material) ) {
		%material = MaterialSet.getObject(0);
		%isNew = true;
	}

	// Check made in order to avoid loading the same material. Overriding
	// made in special cases
	if(%material $= LabMat.lastMaterial && !%override) {
		return;
	} else {
		if(LabMat.materialDirty ) {
			LabMat.showSaveDialog( %material );
			return;
		}

		LabMat.setActiveMaterial(%material,%isNew);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Updates the preview material to use the same properties as the selected material,
// and makes that material active in the editor.
//LabMat.setActiveMaterial(test_cuboidTestF);
function LabMat::setActiveMaterial( %this, %material,%isNew ) {
	// Warn if selecting a CustomMaterial (they can't be properly previewed or edited)
	if ( isObject( %material ) && %material.isMemberOfClass( "CustomMaterial" ) ) {
		LabMsgOK( "Warning", "The selected Material (" @ %material.getName() @
					 ") is a CustomMaterial, and cannot be edited using the Material Editor." );
		return;
	}

	if (strFind(%material.getFilename(),"tlab/materialEditor") || %material.getFilename() $= "") {
		%material.setFilename($Pref::LabMat::DefaultMaterialFile);
	}

	LabMat.currentMaterial = %material;
	LabMat.lastMaterial = %material;
	%this.setActiveMaterialFile(%material);
	// we create or recreate a material to hold in a pristine state
	
	// Converts the texture files into absolute paths.
	LabMat.convertTextureFields();
	// If we're allowing for name changes, make sure to save the name seperately
	%this.originalName = LabMat.currentMaterial.name;
	
	
	// Copy materials over to other references
	LabMat.copyMaterials( LabMat.currentMaterial, %this.getPreviewMat() );
	LabMat.copyMaterials( LabMat.currentMaterial, %this.getNotDirtyMat() );
	LabMat.guiSync(LabMat_previewMaterial );
	// Necessary functionality in order to render correctly
	LabMat_previewMaterial.flush();
	LabMat_previewMaterial.reload();
	LabMat.currentMaterial.flush();
	LabMat.currentMaterial.reload();
	LabMat.setMaterialNotDirty();

	if (%isNew)
		LabMat.currentMaterial.metalness[0] = "0";
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::setActiveMaterialFile( %this, %material ) {
	LabMatModeTarget-->selMaterialFile.setText(%material.getFilename());
	LabMatModeMaterial-->selMaterialFile.setText(%material.getFilename());
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::copyMaterials( %this, %copyFrom, %copyTo) {
	// Make sure we copy and restore the map to.
	if (!isObject(%copyFrom) || !isObject(%copyTo))
		return;

	%mapTo = %copyTo.mapTo;
	%copyTo.assignFieldsFrom( %copyFrom );
	%copyTo.mapTo = %mapTo;
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::getPreviewMat( %this) {
	//------------------------------------------------------------------------------
   //Material used to preview other materials in the editor.
	if (!isObject(LabMat_previewMaterial))
		singleton Material(LabMat_previewMaterial) {
		mapTo = "LabMat_mappedMat";
		diffuseMap[0] = "tlab/materialEditor/assets/matEd_mappedMat";
	};
	return LabMat_previewMaterial;
}
function LabMat::getNotDirtyMat( %this) {
   	if (!isObject(LabMat_NotDirtyMaterial))
	singleton Material(LabMat_NotDirtyMaterial) {
		mapTo = "LabMat_previewMat";
		diffuseMap[0] = "tlab/materialEditor/assets/matEd_mappedMat";
	};
	return LabMat_NotDirtyMaterial;
}

//==============================================================================
function LabMat::updateMaterialPBR(%this, %propertyField,%value, %isSlider, %onMouseUp) {
	devLog(" LabMat::updateMaterialPBR(%this, %propertyField, %value, %isSlider, %onMouseUp)", %this, %propertyField, %value, %isSlider, %onMouseUp);
	%mat = LabMat.currentMaterial;
	%layer = LabMat.currentLayer;
	%isDirty = LabObj.set(%mat,%propertyField,%value,%layer);

	if (%isDirty)
		LabMat.setMaterialDirty();

	%field = %propertyField@"[" @%layer @ "]";
	eval("LabMat_previewMaterial." @ %field @ " = " @ %value @ ";");
	LabMat_previewMaterial.flush();
	LabMat_previewMaterial.reload();

	if ($LabMatDblUpdate)
		%this.updateActiveMaterial(%field,%value, %isSlider, %onMouseUp);
}
//==============================================================================
function LabMat::updateActiveMaterial(%this, %propertyField, %value, %isSlider, %onMouseUp) {
	logc(" LabMat::updateActiveMaterial(%this, %propertyField, %value, %isSlider, %onMouseUp)", %this, %propertyField, %value, %isSlider, %onMouseUp);
	LabMat.setMaterialDirty();

	if(%value $= "")
		%value = "\"\"";

/*
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
		%action.material = LabMat.currentMaterial;
		%action.object = LabMat.currentObject;
		%action.field = %propertyField;
		%action.isSlider = %isSlider;
		%action.onMouseUp = %onMouseUp;
		%action.newValue = %value;
		eval( "%action.oldValue = " @ LabMat.currentMaterial @ "." @ %propertyField @ ";");
		%action.oldValue = "\"" @ %action.oldValue @ "\"";
		//LabMat.submitUndo( %action );
	}
*/
	if (!isObject(LabMat_previewMaterial))
		LabMat.establishMaterials();

	eval("LabMat_previewMaterial." @ %propertyField @ " = " @ %value @ ";");
	LabMat_previewMaterial.flush();
	LabMat_previewMaterial.reload();

	if (LabMat.livePreview == true) {
		eval("LabMat.currentMaterial." @ %propertyField @ " = " @ %value @ ";");
		LabMat.currentMaterial.flush();
		LabMat.currentMaterial.reload();
	}

	if(strFind(%propertyField,"diffuseMap") && %autoUpdateNormal) {
		%diffuseSuffix = MaterialEditorPlugin.getCfg("DiffuseSuffix");
		%cleanValue = strreplace(%value,%diffuseSuffix,"");
		%autoUpdateNormal = MaterialEditorPlugin.getCfg("AutoAddNormal");

		if (%autoUpdateNormal) {
			%normalMap = strreplace(%propertyField,"diffuse","normal");
			%normalFile = LabMat.currentMaterial.getFieldValue(%normalMap);
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
			%specFile = LabMat.currentMaterial.getFieldValue(%specMap);
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
function LabMat::updateActiveMaterialName(%this, %name) {
	%action = %this.createUndo(ActionUpdateActiveMaterialName, "Update Active Material Name");
	%action.material =  LabMat.currentMaterial;
	%action.object = LabMat.currentObject;
	%action.oldName = LabMat.currentMaterial.getName();
	%action.newName = %name;
	LabMat.submitUndo( %action );
	LabMat.currentMaterial.setName(%name);
	// Some objects (ConvexShape, DecalRoad etc) reference Materials by name => need
	// to find and update all these references so they don't break when we rename the
	// Material.
	LabMat.updateMaterialReferences( MissionGroup, %action.oldName, %action.newName );
}

