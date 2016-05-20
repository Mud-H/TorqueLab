//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Commands for the Cubemap Editor

//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::selectCubemap(%this) {
	%cubemap = MaterialEditorTools.currentCubemap;

	if(!isObject(%cubemap))
		return;

	MaterialEditorTools.updateActiveMaterial( "cubemap", %cubemap.name );
	MaterialEditorTools.hideCubemapEditor();
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::cancelCubemap(%this) {
	%cubemap = MaterialEditorTools.currentCubemap;
	%idx = matEd_cubemapEd_availableCubemapList.findItemText( %cubemap.getName() );
	matEd_cubemapEd_availableCubemapList.setItemText( %idx, notDirtyCubemap.originalName );
	%cubemap.setName( notDirtyCubemap.originalName );
	MaterialEditorTools.copyCubemaps( notDirtyCubemap, %cubemap );
	MaterialEditorTools.copyCubemaps( notDirtyCubemap, matEdCubeMapPreviewMat);
	%cubemap.updateFaces();
	matEdCubeMapPreviewMat.updateFaces();
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::showCubemapEditor(%this) {
	if (matEd_cubemapEditor.isVisible())
		return;

	MaterialEditorTools.currentCubemap = "";
	matEd_cubemapEditor.setVisible(1);

	if (!isObject(matEd_cubemapEdPerMan))
		new PersistenceManager(matEd_cubemapEdPerMan);

	MaterialEditorTools.setCubemapNotDirty();

	for( %i = 0; %i < RootGroup.getCount(); %i++ ) {
		if( RootGroup.getObject(%i).getClassName()!$= "CubemapData" )
			continue;

		for( %k = 0; %k < UnlistedCubemaps.count(); %k++ ) {
			%unlistedFound = 0;

			if( UnlistedCubemaps.getValue(%k) $= RootGroup.getObject(%i).name ) {
				%unlistedFound = 1;
				break;
			}
		}

		if( %unlistedFound )
			continue;

		matEd_cubemapEd_availableCubemapList.addItem( RootGroup.getObject(%i).name );
	}

	singleton CubemapData(notDirtyCubemap);

	// if there was no cubemap, pick the first, select, and bail, these are going to take
	// care of themselves in the selected function
	if( !isObject( MaterialEditorTools.currentMaterial.cubemap ) ) {
		if( matEd_cubemapEd_availableCubemapList.getItemCount() > 0 ) {
			matEd_cubemapEd_availableCubemapList.setSelected(0, true);
			return;
		} else {
			// if there are no cubemaps, then create one, select, and bail
			%cubemap = MaterialEditorTools.createNewCubemap();
			matEd_cubemapEd_availableCubemapList.addItem( %cubemap.name );
			matEd_cubemapEd_availableCubemapList.setSelected(0, true);
			return;
		}
	}

	// do not directly change activeMat!
	MaterialEditorTools.currentCubemap = MaterialEditorTools.currentMaterial.cubemap.getId();
	%cubemap = MaterialEditorTools.currentCubemap;
	notDirtyCubemap.originalName = %cubemap.getName();
	MaterialEditorTools.copyCubemaps( %cubemap, notDirtyCubemap);
	MaterialEditorTools.copyCubemaps( %cubemap, matEdCubeMapPreviewMat);
	MaterialEditorTools.syncCubemap( %cubemap );
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::hideCubemapEditor(%this,%cancel) {
	if(%cancel)
		MaterialEditorTools.cancelCubemap();

	matEd_cubemapEd_availableCubemapList.clearItems();
	matEd_cubemapEdPerMan.delete();
	matEd_cubemapEditor.setVisible(0);
}
//------------------------------------------------------------------------------
//==============================================================================
// create category and update current material if there is one
function MaterialEditorTools::addCubemap( %this,%cubemapName ) {
	if( %cubemapName $= "" ) {
		LabMsgOK( "Error", "Can not create a cubemap without a valid name.");
		return;
	}

	for(%i = 0; %i < RootGroup.getCount(); %i++) {
		if( %cubemapName $= RootGroup.getObject(%i).getName() ) {
			LabMsgOK( "Error", "There is already an object with the same name.");
			return;
		}
	}

	// Create and select a new cubemap
	%cubemap = MaterialEditorTools.createNewCubemap( %cubemapName );
	%idx = matEd_cubemapEd_availableCubemapList.addItem( %cubemap.name );
	matEd_cubemapEd_availableCubemapList.setSelected( %idx, true );
	// material category text field to blank
	matEd_addCubemapWindow-->cubemapName.setText("");
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::createNewCubemap( %this, %cubemap ) {
	if( %cubemap $= "" ) {
		for(%i = 0; ; %i++) {
			%cubemap = "newCubemap_" @ %i;

			if( !isObject(%cubemap) )
				break;
		}
	}

	new CubemapData(%cubemap) {
		cubeFace[0] = "tlab/materialEditor/assets/cube_xNeg";
		cubeFace[1] = "tlab/materialEditor/assets/cube_xPos";
		cubeFace[2] = "tlab/materialEditor/assets/cube_ZNeg";
		cubeFace[3] = "tlab/materialEditor/assets/cube_ZPos";
		cubeFace[4] = "tlab/materialEditor/assets/cube_YNeg";
		cubeFace[5] = "tlab/materialEditor/assets/cube_YPos";
		parentGroup = RootGroup;
	};
	matEd_cubemapEdPerMan.setDirty( %cubemap, "art/materials.cs" );
	matEd_cubemapEdPerMan.saveDirty();
	return %cubemap;
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::setCubemapDirty(%this) {
	%propertyText = "Create Cubemap *";
	matEd_cubemapEditor.text = %propertyText;
	matEd_cubemapEditor.dirty = true;
	matEd_cubemapEditor-->saveCubemap.setActive(true);
	%cubemap = MaterialEditorTools.currentCubemap;

	// materials created in the materail selector are given that as its filename, so we run another check
	if( MaterialEditorTools.isMatEditorMaterial( %cubemap ) )
		matEd_cubemapEdPerMan.setDirty(%cubemap, "art/materials.cs");
	else
		matEd_cubemapEdPerMan.setDirty(%cubemap);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::setCubemapNotDirty(%this) {
	%propertyText= strreplace("Create Cubemap" , "*" , "");
	matEd_cubemapEditor.text = %propertyText;
	matEd_cubemapEditor.dirty = false;
	matEd_cubemapEditor-->saveCubemap.setActive(false);
	%cubemap = MaterialEditorTools.currentCubemap;
	matEd_cubemapEdPerMan.removeDirty(%cubemap);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::showDeleteCubemapDialog(%this) {
	%idx = matEd_cubemapEd_availableCubemapList.getSelectedItem();
	%cubemap = matEd_cubemapEd_availableCubemapList.getItemText( %idx );
	%cubemap = %cubemap.getId();

	if( %cubemap == -1 || !isObject(%cubemap) )
		return;

	if( isObject( %cubemap ) ) {
		LabMsgYesNoCancel("Delete Cubemap?",
								"Are you sure you want to delete<br><br>" @ %cubemap.getName() @ "<br><br> Cubemap deletion won't take affect until the engine is quit.",
								"MaterialEditorTools.deleteCubemap( " @ %cubemap @ ", " @ %idx @ " );",
								"",
								"" );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::deleteCubemap( %this, %cubemap, %idx ) {
	matEd_cubemapEd_availableCubemapList.deleteItem( %idx );
	UnlistedCubemaps.add( "unlistedCubemaps", %cubemap.getName() );

	if( !MaterialEditorTools.isMatEditorMaterial( %cubemap ) ) {
		matEd_cubemapEdPerMan.removeDirty( %cubemap );
		matEd_cubemapEdPerMan.removeObjectFromFile( %cubemap );
	}

	if( matEd_cubemapEd_availableCubemapList.getItemCount() > 0 ) {
		matEd_cubemapEd_availableCubemapList.setSelected(0, true);
	} else {
		// if there are no cubemaps, then create one, select, and bail
		%cubemap = MaterialEditorTools.createNewCubemap();
		matEd_cubemapEd_availableCubemapList.addItem( %cubemap.getName() );
		matEd_cubemapEd_availableCubemapList.setSelected(0, true);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function matEd_cubemapEd_availableCubemapList::onSelect( %this, %id, %cubemap ) {
	%cubemap = %cubemap.getId();

	if( MaterialEditorTools.currentCubemap $= %cubemap )
		return;

	if( matEd_cubemapEditor.dirty ) {
		%savedCubemap = MaterialEditorTools.currentCubemap;
		LabMsgYesNoCancel("Save Existing Cubemap?",
								"Do you want to save changes to <br><br>" @ %savedCubemap.getName(),
								"MaterialEditorTools.saveCubemap(" @ true @ ");",
								"MaterialEditorTools.saveCubemapDialogDontSave(" @ %cubemap @ ");",
								"MaterialEditorTools.saveCubemapDialogCancel();" );
	} else
		MaterialEditorTools.changeCubemap( %cubemap );
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::showSaveCubemapDialog( %this ) {
	%cubemap = MaterialEditorTools.currentCubemap;

	if( !isObject(%cubemap) )
		return;

	LabMsgYesNoCancel("Save Cubemap?",
							"Do you want to save changes to <br><br>" @ %cubemap.getName(),
							"MaterialEditorTools.saveCubemap( " @ %cubemap @ " );",
							"",
							"" );
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::saveCubemap( %this, %cubemap ) {
	notDirtyCubemap.originalName = %cubemap.getName();
	MaterialEditorTools.copyCubemaps( %cubemap, notDirtyCubemap );
	MaterialEditorTools.copyCubemaps( %cubemap, matEdCubeMapPreviewMat);
	matEd_cubemapEdPerMan.saveDirty();
	MaterialEditorTools.setCubemapNotDirty();
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::saveCubemapDialogDontSave( %this, %newCubemap) {
	//deal with old cubemap first
	%oldCubemap = MaterialEditorTools.currentCubemap;
	%idx = matEd_cubemapEd_availableCubemapList.findItemText( %oldCubemap.getName() );
	matEd_cubemapEd_availableCubemapList.setItemText( %idx, notDirtyCubemap.originalName );
	%oldCubemap.setName( notDirtyCubemap.originalName );
	MaterialEditorTools.copyCubemaps( notDirtyCubemap, %oldCubemap);
	MaterialEditorTools.copyCubemaps( notDirtyCubemap, matEdCubeMapPreviewMat);
	MaterialEditorTools.syncCubemap( %oldCubemap );
	MaterialEditorTools.changeCubemap( %newCubemap );
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::saveCubemapDialogCancel( %this ) {
	%cubemap = MaterialEditorTools.currentCubemap;
	%idx = matEd_cubemapEd_availableCubemapList.findItemText( %cubemap.getName() );
	matEd_cubemapEd_availableCubemapList.clearSelection();
	matEd_cubemapEd_availableCubemapList.setSelected( %idx, true );
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::changeCubemap( %this, %cubemap ) {
	MaterialEditorTools.setCubemapNotDirty();
	MaterialEditorTools.currentCubemap = %cubemap;
	notDirtyCubemap.originalName = %cubemap.getName();
	MaterialEditorTools.copyCubemaps( %cubemap, notDirtyCubemap);
	MaterialEditorTools.copyCubemaps( %cubemap, matEdCubeMapPreviewMat);
	MaterialEditorTools.syncCubemap( %cubemap );
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::editCubemapImage( %this, %face ) {
	MaterialEditorTools.setCubemapDirty();
	%cubemap = MaterialEditorTools.currentCubemap;
	%bitmap = MaterialEditorTools.openFile("texture");

	if( %bitmap !$= "" && %bitmap !$= "tlab/materialEditor/assets/cubemapBtnBorder" ) {
		%cubemap.cubeFace[%face] = %bitmap;
		MaterialEditorTools.copyCubemaps( %cubemap, matEdCubeMapPreviewMat);
		MaterialEditorTools.syncCubemap( %cubemap );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::editCubemapName( %this, %newName ) {
	MaterialEditorTools.setCubemapDirty();
	%cubemap = MaterialEditorTools.currentCubemap;
	%idx = matEd_cubemapEd_availableCubemapList.findItemText( %cubemap.getName() );
	matEd_cubemapEd_availableCubemapList.setItemText( %idx, %newName );
	%cubemap.setName(%newName);
	MaterialEditorTools.syncCubemap( %cubemap );
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::syncCubemap( %this, %cubemap ) {
	%xpos = searchForTexture(%cubemap.getName(), %cubemap.cubeFace[0]);

	if( %xpos !$= "" )
		matEd_cubemapEd_XPos.setBitmap( %xpos );

	%xneg = searchForTexture(%cubemap.getName(), %cubemap.cubeFace[1]);

	if( %xneg !$= "" )
		matEd_cubemapEd_XNeg.setBitmap( %xneg );

	%yneg = searchForTexture(%cubemap.getName(), %cubemap.cubeFace[2]);

	if( %yneg !$= "" )
		matEd_cubemapEd_YNeG.setBitmap( %yneg );

	%ypos = searchForTexture(%cubemap.getName(), %cubemap.cubeFace[3]);

	if( %ypos !$= "" )
		matEd_cubemapEd_YPos.setBitmap( %ypos );

	%zpos = searchForTexture(%cubemap.getName(), %cubemap.cubeFace[4]);

	if( %zpos !$= "" )
		matEd_cubemapEd_ZPos.setBitmap( %zpos );

	%zneg = searchForTexture(%cubemap.getName(), %cubemap.cubeFace[5]);

	if( %zneg !$= "" )
		matEd_cubemapEd_ZNeg.setBitmap( %zneg );

	matEd_cubemapEd_activeCubemapNameTxt.setText(%cubemap.getName());
	%cubemap.updateFaces();
	matEdCubeMapPreviewMat.updateFaces();
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::copyCubemaps( %this, %copyFrom, %copyTo) {
	%copyTo.cubeFace[0] = %copyFrom.cubeFace[0];
	%copyTo.cubeFace[1] = %copyFrom.cubeFace[1];
	%copyTo.cubeFace[2] = %copyFrom.cubeFace[2];
	%copyTo.cubeFace[3] = %copyFrom.cubeFace[3];
	%copyTo.cubeFace[4] = %copyFrom.cubeFace[4];
	%copyTo.cubeFace[5] = %copyFrom.cubeFace[5];
}
//------------------------------------------------------------------------------

new CubemapData(dsdad)
{
   cubeFace[0] = "art/Packs/SkyBoxes/DefaultDay/skybox_1.png";
   cubeFace[1] = "art/Packs/SkyBoxes/DefaultDay/skybox_2.png";
   cubeFace[2] = "art/Packs/SkyBoxes/DefaultDay/skybox_3.png";
   cubeFace[3] = "art/Packs/SkyBoxes/DefaultDay/skybox_4.png";
   cubeFace[4] = "art/Packs/SkyBoxes/DefaultDay/skybox_5.png";
   cubeFace[5] = "art/Packs/SkyBoxes/DefaultDay/skybox_6.png";
};
