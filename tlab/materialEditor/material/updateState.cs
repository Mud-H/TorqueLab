//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Clear and Refresh Material
function MaterialEditorTools::clearMaterial(%this) {
	%action = %this.createUndo(ActionClearMaterial, "Clear Material");
	%action.material = MaterialEditorTools.currentMaterial;
	%action.object = MaterialEditorTools.currentObject;
	pushInstantGroup();
	%action.oldMaterial = new Material();
	%action.newMaterial = new Material();
	popInstantGroup();
	MaterialEditorTools.submitUndo( %action );
	MaterialEditorTools.copyMaterials( MaterialEditorTools.currentMaterial, %action.oldMaterial );
	%tempMat = new Material() {
		name = "tempMaterial";
		mapTo = "unmapped_mat";
		parentGroup = RootGroup;
	};
	MaterialEditorTools.copyMaterials( %tempMat, materialEd_previewMaterial );
	MaterialEditorTools.guiSync( materialEd_previewMaterial );
	materialEd_previewMaterial.flush();
	materialEd_previewMaterial.reload();

	if (MaterialEditorTools.livePreview == true) {
		MaterialEditorTools.copyMaterials( %tempMat, MaterialEditorTools.currentMaterial );
		MaterialEditorTools.currentMaterial.flush();
		MaterialEditorTools.currentMaterial.reload();
	}

	MaterialEditorTools.setMaterialDirty();
	%tempMat.delete();
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::refreshMaterial(%this) {
	%action = %this.createUndo(ActionRefreshMaterial, "Refresh Material");
	%action.material = MaterialEditorTools.currentMaterial;
	%action.object = MaterialEditorTools.currentObject;
	pushInstantGroup();
	%action.oldMaterial = new Material();
	%action.newMaterial = new Material();
	popInstantGroup();
	MaterialEditorTools.copyMaterials( MaterialEditorTools.currentMaterial, %action.oldMaterial );
	MaterialEditorTools.copyMaterials( notDirtyMaterial, %action.newMaterial );
	%action.oldName = MaterialEditorTools.currentMaterial.getName();
	%action.newName = %this.originalName;
	MaterialEditorTools.submitUndo( %action );
	MaterialEditorTools.currentMaterial.setName( %this.originalName );
	MaterialEditorTools.copyMaterials( notDirtyMaterial, materialEd_previewMaterial );
	MaterialEditorTools.guiSync( materialEd_previewMaterial );
	materialEd_previewMaterial.flush();
	materialEd_previewMaterial.reload();

	if (MaterialEditorTools.livePreview == true) {
		MaterialEditorTools.copyMaterials( notDirtyMaterial, MaterialEditorTools.currentMaterial );
		MaterialEditorTools.currentMaterial.flush();
		MaterialEditorTools.currentMaterial.reload();
	}

	MaterialEditorTools.setMaterialNotDirty();
}
//------------------------------------------------------------------------------
//==============================================================================
// Switching and Changing Materials
function MaterialEditorTools::switchMaterial( %this, %material ) {
	//MaterialEditorTools.currentMaterial = %material.getId();
	MaterialEditorTools.currentObject = "";
	MaterialEditorTools.setMode();
	MaterialEditorTools.prepareActiveMaterial( %material.getId(), true );
}
/*------------------------------------------------------------------------------
 This changes the map to's of possibly two materials (%fromMaterial, %toMaterial)
 and updates the engines libraries accordingly in order to make this change per
 object/per objects instances/per target. Before this functionality is enacted,
 there is a popup beforehand that will ask if you are sure if you want to make
 this change. Making this change will physically alter possibly two materials.cs
 files in order to move the (%fromMaterial, %toMaterial), replacing the
 (%fromMaterials)'s mapTo to "unmapped_mat".
-------------------------------------------------------------------------------*/

//==============================================================================
function MaterialEditorTools::changeMaterial(%this, %fromMaterial, %toMaterial) {
	%action = %this.createUndo(ActionChangeMaterial, "Change Material");
	%action.object = MaterialEditorTools.currentObject;
	%materialTarget = SubMaterialSelector.text;
	%action.materialTarget = %materialTarget;
	%action.fromMaterial = %fromMaterial;
	%action.toMaterial = %toMaterial;
	%action.toMaterialOldFname = %toMaterial.getFilename();
	%action.object =  MaterialEditorTools.currentObject;

	if( MaterialEditorTools.currentMeshMode $= "Model" ) { // Models
		%action.mode = "model";
		MaterialEditorTools.currentObject.changeMaterial( %materialTarget, %fromMaterial.getName(), %toMaterial.getName() );

		if( MaterialEditorTools.currentObject.shapeName !$= "" )
			%sourcePath = MaterialEditorTools.currentObject.shapeName;
		else if( MaterialEditorTools.currentObject.isMethod("getDatablock") ) {
			if( MaterialEditorTools.currentObject.getDatablock().shapeFile !$= "" )
				%sourcePath = MaterialEditorTools.currentObject.getDatablock().shapeFile;
		}

		// Creating "to" path
		%k = 0;

		while( strpos( %sourcePath, "/", %k ) != -1 ) {
			%count = strpos( %sourcePath, "/", %k );
			%k = %count + 1;
		}

		%fileName = getSubStr( %sourcePath , 0 , %k );
		%fileName = %fileName @ "materials.cs";
		%action.toMaterialNewFname = %fileName;
		MaterialEditorTools.prepareActiveMaterial( %toMaterial, true );

		if( !MaterialEditorTools.isMatEditorMaterial( %toMaterial ) ) {
			matEd_PersistMan.removeObjectFromFile(%toMaterial);
		}

		matEd_PersistMan.setDirty(%fromMaterial);
		matEd_PersistMan.setDirty(%toMaterial, %fileName);
		matEd_PersistMan.saveDirty();
		matEd_PersistMan.removeDirty(%fromMaterial);
		matEd_PersistMan.removeDirty(%toMaterial);
	} else { // EditorShapes
		%action.mode = "editorShapes";
		eval("MaterialEditorTools.currentObject." @ SubMaterialSelector.getText() @ " = " @ %toMaterial.getName() @ ";");

		if( MaterialEditorTools.currentObject.isMethod("postApply") )
			MaterialEditorTools.currentObject.postApply();

		MaterialEditorTools.prepareActiveMaterial( %toMaterial, true );
	}

	MaterialEditorTools.submitUndo( %action );
}
//------------------------------------------------------------------------------
