//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Create New and Delete Material
//==============================================================================

//==============================================================================
function MaterialEditorTools::createNewMaterial( %this ) {
	%action = %this.createUndo(ActionCreateNewMaterial, "Create New Material");
	%action.object = "";
	%material = getUniqueName( "newMaterial" );
	new Material(%material) {
		diffuseMap[0] = "art/textures/core/warnMat";
		mapTo = "unmapped_mat";
		parentGroup = RootGroup;
	};
	%defaultFile = "art/materials.cs";
	//%material.setFileName(MaterialEditorTools-->selMaterialFile.getText());
	%material.setFileName(%defaultFile);
	%action.newMaterial = %material.getId();
	%action.oldMaterial = MaterialEditorTools.currentMaterial;
	MaterialEditorTools.submitUndo( %action );
	MaterialEditorTools.currentObject = "";
	MaterialEditorTools.setMode();
	MaterialEditorTools.prepareActiveMaterial( %material.getId(), true );
	MaterialEditorTools.setMaterialDirty();
}
//------------------------------------------------------------------------------
//==============================================================================
// Clone selected Material
function MaterialEditorTools::cloneMaterial( %this ) {
	%srcMat = MaterialEditorTools.currentMaterial;

	if (!isObject(%srcMat))
		return;

	%newMat = %srcMat.deepClone();
	%newName = getUniqueName(%srcMat.getName());
	%newMat.setName(%newName);
	%newMat.setFilename(%srcMat.getFilename());
	MaterialEditorTools.currentObject = "";
	MaterialEditorTools.setMode();
	MaterialEditorTools.prepareActiveMaterial( %newMat.getId(), true );
	MaterialEditorTools.setMaterialDirty();
	MaterialSelector.buildPreviewArray(%newMat);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::deleteMaterial( %this ) {
	%action = %this.createUndo(ActionDeleteMaterial, "Delete Material");
	%action.object = MaterialEditorTools.currentObject;
	%action.currentMode = MaterialEditorTools.currentMode;
	/*
	if( MaterialEditorTools.currentMode $= "Mesh" )
	{
	   %materialTarget = SubMaterialSelector.text;
	   %action.materialTarget = %materialTarget;

	   //create the stub material
	   %toMaterial = getUniqueName( "newMaterial" );
	   new Material(%toMaterial)
	   {
	      diffuseMap[0] = "art/textures/core/warnMat";
	      mapTo = "unmapped_mat";
	      parentGroup = RootGroup;
	   };

	   %action.toMaterial = %toMaterial.getId();
	   %action.fromMaterial = MaterialEditorTools.currentMaterial;
	   %action.fromMaterialOldFname = MaterialEditorTools.currentMaterial.getFilename();
	}
	else
	{
	   // Grab first material we see; if theres not one, create one
	   %toMaterial = MaterialSet.getObject(0);
	   if( !isObject( %toMaterial ) )
	   {
	      %toMaterial = getUniqueName( "newMaterial" );
	      new Material(%toMaterial)
	      {
	         diffuseMap[0] = "art/textures/core/warnMat";
	         mapTo = "unmapped_mat";
	         parentGroup = RootGroup;
	      };
	   }

	   %action.toMaterial = %toMaterial.getId();
	   %action.fromMaterial = MaterialEditorTools.currentMaterial;
	}
	*/
	// Grab first material we see; if theres not one, create one
	%newMaterial = getUniqueName( "newMaterial" );
	new Material(%newMaterial) {
		diffuseMap[0] = "art/textures/core/warnMat";
		mapTo = "unmapped_mat";
		parentGroup = RootGroup;
	};
	// Setup vars
	%action.newMaterial = %newMaterial.getId();
	%action.oldMaterial = MaterialEditorTools.currentMaterial;
	%action.oldMaterialFname = MaterialEditorTools.currentMaterial.getFilename();
	// Submit undo
	MaterialEditorTools.submitUndo( %action );

	// Delete the material from file
	if( !MaterialEditorTools.isMatEditorMaterial( MaterialEditorTools.currentMaterial ) ) {
		matEd_PersistMan.removeObjectFromFile(MaterialEditorTools.currentMaterial);
		matEd_PersistMan.removeDirty(MaterialEditorTools.currentMaterial);
	}

	// Delete the material as seen through the material selector.
	UnlistedMaterials.add( "unlistedMaterials", MaterialEditorTools.currentMaterial.getName() );
	// Loadup another material
	MaterialEditorTools.currentObject = "";
	MaterialEditorTools.setMode();
	MaterialEditorTools.prepareActiveMaterial( %newMaterial.getId(), true );
}
//------------------------------------------------------------------------------
