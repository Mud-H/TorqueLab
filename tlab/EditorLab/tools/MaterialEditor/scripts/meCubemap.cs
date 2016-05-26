//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Commands for the Cubemap Editor

//------------------------------------------------------------------------------
//==============================================================================
function LabMat::selectCubemap(%this) {
	%cubemap = LabMat.currentCubemap;

	if(!isObject(%cubemap))
		return;

	LabMat.updateActiveMaterial( "cubemap", %cubemap.name );
	LabMat.hideCubemapEditor();
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::cancelCubemap(%this) {
	%cubemap = LabMat.currentCubemap;
	%idx = LabMat_cubemapEd_availableCubemapList.findItemText( %cubemap.getName() );
	LabMat_cubemapEd_availableCubemapList.setItemText( %idx, notDirtyCubemap.originalName );
	%cubemap.setName( notDirtyCubemap.originalName );
	LabMat.copyCubemaps( notDirtyCubemap, %cubemap );
	LabMat.copyCubemaps( notDirtyCubemap, matEdCubeMapPreviewMat);
	%cubemap.updateFaces();
	matEdCubeMapPreviewMat.updateFaces();
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::showCubemapEditor(%this) {
	if (LabCubemapEditor.isVisible())
		return;

	LabMat.currentCubemap = "";
	pushDlg(LabCubemapEditor);
	LabCubemapEditor.setVisible(1);

	if (!isObject(LabMat_cubemapEdPerMan))
		new PersistenceManager(LabMat_cubemapEdPerMan);

	LabMat.setCubemapNotDirty();

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

		LabMat_cubemapEd_availableCubemapList.addItem( RootGroup.getObject(%i).name );
	}

	singleton CubemapData(notDirtyCubemap);

	// if there was no cubemap, pick the first, select, and bail, these are going to take
	// care of themselves in the selected function
	if( !isObject( LabMat.currentMaterial.cubemap ) ) {
		if( LabMat_cubemapEd_availableCubemapList.getItemCount() > 0 ) {
			LabMat_cubemapEd_availableCubemapList.setSelected(0, true);
			return;
		} else {
			// if there are no cubemaps, then create one, select, and bail
			%cubemap = LabMat.createNewCubemap();
			LabMat_cubemapEd_availableCubemapList.addItem( %cubemap.name );
			LabMat_cubemapEd_availableCubemapList.setSelected(0, true);
			return;
		}
	}

	// do not directly change activeMat!
	LabMat.currentCubemap = LabMat.currentMaterial.cubemap.getId();
	%cubemap = LabMat.currentCubemap;
	notDirtyCubemap.originalName = %cubemap.getName();
	LabMat.copyCubemaps( %cubemap, notDirtyCubemap);
	LabMat.copyCubemaps( %cubemap, matEdCubeMapPreviewMat);
	LabMat.syncCubemap( %cubemap );
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::hideCubemapEditor(%this,%cancel) {
	if(%cancel)
		LabMat.cancelCubemap();

   popDlg(LabCubemapEditor);
	LabMat_cubemapEd_availableCubemapList.clearItems();
	LabMat_cubemapEdPerMan.delete();
	LabCubemapEditor.setVisible(0);
}
//------------------------------------------------------------------------------
//==============================================================================
// create category and update current material if there is one
function LabMat::addCubemap( %this,%cubemapName ) {
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
	%cubemap = LabMat.createNewCubemap( %cubemapName );
	%idx = LabMat_cubemapEd_availableCubemapList.addItem( %cubemap.name );
	LabMat_cubemapEd_availableCubemapList.setSelected( %idx, true );
	// material category text field to blank
	matEd_addCubemapWindow-->cubemapName.setText("");
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::createNewCubemap( %this, %cubemap ) {
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
	LabMat_cubemapEdPerMan.setDirty( %cubemap, "art/materials.cs" );
	LabMat_cubemapEdPerMan.saveDirty();
	return %cubemap;
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::setCubemapDirty(%this) {
	%propertyText = "Create Cubemap *";
	LabCubemapEditor.text = %propertyText;
	LabCubemapEditor.dirty = true;
	LabCubemapEditor-->saveCubemap.setActive(true);
	%cubemap = LabMat.currentCubemap;

	// materials created in the materail selector are given that as its filename, so we run another check
	if( LabMat.isLabMatitorMaterial( %cubemap ) )
		LabMat_cubemapEdPerMan.setDirty(%cubemap, "art/materials.cs");
	else
		LabMat_cubemapEdPerMan.setDirty(%cubemap);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::setCubemapNotDirty(%this) {
	%propertyText= strreplace("Create Cubemap" , "*" , "");
	LabCubemapEditor.text = %propertyText;
	LabCubemapEditor.dirty = false;
	LabCubemapEditor-->saveCubemap.setActive(false);
	%cubemap = LabMat.currentCubemap;
	LabMat_cubemapEdPerMan.removeDirty(%cubemap);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::showDeleteCubemapDialog(%this) {
	%idx = LabMat_cubemapEd_availableCubemapList.getSelectedItem();
	%cubemap = LabMat_cubemapEd_availableCubemapList.getItemText( %idx );
	%cubemap = %cubemap.getId();

	if( %cubemap == -1 || !isObject(%cubemap) )
		return;

	if( isObject( %cubemap ) ) {
		LabMsgYesNoCancel("Delete Cubemap?",
								"Are you sure you want to delete<br><br>" @ %cubemap.getName() @ "<br><br> Cubemap deletion won't take affect until the engine is quit.",
								"LabMat.deleteCubemap( " @ %cubemap @ ", " @ %idx @ " );",
								"",
								"" );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::deleteCubemap( %this, %cubemap, %idx ) {
	LabMat_cubemapEd_availableCubemapList.deleteItem( %idx );
	UnlistedCubemaps.add( "unlistedCubemaps", %cubemap.getName() );

	if( !LabMat.isLabMatitorMaterial( %cubemap ) ) {
		LabMat_cubemapEdPerMan.removeDirty( %cubemap );
		LabMat_cubemapEdPerMan.removeObjectFromFile( %cubemap );
	}

	if( LabMat_cubemapEd_availableCubemapList.getItemCount() > 0 ) {
		LabMat_cubemapEd_availableCubemapList.setSelected(0, true);
	} else {
		// if there are no cubemaps, then create one, select, and bail
		%cubemap = LabMat.createNewCubemap();
		LabMat_cubemapEd_availableCubemapList.addItem( %cubemap.getName() );
		LabMat_cubemapEd_availableCubemapList.setSelected(0, true);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat_cubemapEd_availableCubemapList::onSelect( %this, %id, %cubemap ) {
	%cubemap = %cubemap.getId();

	if( LabMat.currentCubemap $= %cubemap )
		return;

	if( LabCubemapEditor.dirty ) {
		%savedCubemap = LabMat.currentCubemap;
		LabMsgYesNoCancel("Save Existing Cubemap?",
								"Do you want to save changes to <br><br>" @ %savedCubemap.getName(),
								"LabMat.saveCubemap(" @ true @ ");",
								"LabMat.saveCubemapDialogDontSave(" @ %cubemap @ ");",
								"LabMat.saveCubemapDialogCancel();" );
	} else
		LabMat.changeCubemap( %cubemap );
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::showSaveCubemapDialog( %this ) {
	%cubemap = LabMat.currentCubemap;

	if( !isObject(%cubemap) )
		return;

	LabMsgYesNoCancel("Save Cubemap?",
							"Do you want to save changes to <br><br>" @ %cubemap.getName(),
							"LabMat.saveCubemap( " @ %cubemap @ " );",
							"",
							"" );
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::saveCubemap( %this, %cubemap ) {
	notDirtyCubemap.originalName = %cubemap.getName();
	LabMat.copyCubemaps( %cubemap, notDirtyCubemap );
	LabMat.copyCubemaps( %cubemap, matEdCubeMapPreviewMat);
	LabMat_cubemapEdPerMan.saveDirty();
	LabMat.setCubemapNotDirty();
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::saveCubemapDialogDontSave( %this, %newCubemap) {
	//deal with old cubemap first
	%oldCubemap = LabMat.currentCubemap;
	%idx = LabMat_cubemapEd_availableCubemapList.findItemText( %oldCubemap.getName() );
	LabMat_cubemapEd_availableCubemapList.setItemText( %idx, notDirtyCubemap.originalName );
	%oldCubemap.setName( notDirtyCubemap.originalName );
	LabMat.copyCubemaps( notDirtyCubemap, %oldCubemap);
	LabMat.copyCubemaps( notDirtyCubemap, matEdCubeMapPreviewMat);
	LabMat.syncCubemap( %oldCubemap );
	LabMat.changeCubemap( %newCubemap );
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::saveCubemapDialogCancel( %this ) {
	%cubemap = LabMat.currentCubemap;
	%idx = LabMat_cubemapEd_availableCubemapList.findItemText( %cubemap.getName() );
	LabMat_cubemapEd_availableCubemapList.clearSelection();
	LabMat_cubemapEd_availableCubemapList.setSelected( %idx, true );
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::changeCubemap( %this, %cubemap ) {
	LabMat.setCubemapNotDirty();
	LabMat.currentCubemap = %cubemap;
	notDirtyCubemap.originalName = %cubemap.getName();
	LabMat.copyCubemaps( %cubemap, notDirtyCubemap);
	LabMat.copyCubemaps( %cubemap, matEdCubeMapPreviewMat);
	LabMat.syncCubemap( %cubemap );
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::editCubemapImage( %this, %face ) {
	LabMat.setCubemapDirty();
	%cubemap = LabMat.currentCubemap;
	%bitmap = LabMat.openFile("texture");

	if( %bitmap !$= "" && %bitmap !$= "tlab/materialEditor/assets/cubemapBtnBorder" ) {
		%cubemap.cubeFace[%face] = %bitmap;
		LabMat.copyCubemaps( %cubemap, matEdCubeMapPreviewMat);
		LabMat.syncCubemap( %cubemap );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::editCubemapName( %this, %newName ) {
	LabMat.setCubemapDirty();
	%cubemap = LabMat.currentCubemap;
	%idx = LabMat_cubemapEd_availableCubemapList.findItemText( %cubemap.getName() );
	LabMat_cubemapEd_availableCubemapList.setItemText( %idx, %newName );
	%cubemap.setName(%newName);
	LabMat.syncCubemap( %cubemap );
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::syncCubemap( %this, %cubemap ) {
	%xpos = searchForTexture(%cubemap.getName(), %cubemap.cubeFace[0]);

	if( %xpos !$= "" )
		LabMat_cubemapEd_XPos.setBitmap( %xpos );

	%xneg = searchForTexture(%cubemap.getName(), %cubemap.cubeFace[1]);

	if( %xneg !$= "" )
		LabMat_cubemapEd_XNeg.setBitmap( %xneg );

	%yneg = searchForTexture(%cubemap.getName(), %cubemap.cubeFace[2]);

	if( %yneg !$= "" )
		LabMat_cubemapEd_YNeG.setBitmap( %yneg );

	%ypos = searchForTexture(%cubemap.getName(), %cubemap.cubeFace[3]);

	if( %ypos !$= "" )
		LabMat_cubemapEd_YPos.setBitmap( %ypos );

	%zpos = searchForTexture(%cubemap.getName(), %cubemap.cubeFace[4]);

	if( %zpos !$= "" )
		LabMat_cubemapEd_ZPos.setBitmap( %zpos );

	%zneg = searchForTexture(%cubemap.getName(), %cubemap.cubeFace[5]);

	if( %zneg !$= "" )
		LabMat_cubemapEd_ZNeg.setBitmap( %zneg );

	LabMat_cubemapEd_activeCubemapNameTxt.setText(%cubemap.getName());
	%cubemap.updateFaces();
	matEdCubeMapPreviewMat.updateFaces();
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::copyCubemaps( %this, %copyFrom, %copyTo) {
	%copyTo.cubeFace[0] = %copyFrom.cubeFace[0];
	%copyTo.cubeFace[1] = %copyFrom.cubeFace[1];
	%copyTo.cubeFace[2] = %copyFrom.cubeFace[2];
	%copyTo.cubeFace[3] = %copyFrom.cubeFace[3];
	%copyTo.cubeFace[4] = %copyFrom.cubeFace[4];
	%copyTo.cubeFace[5] = %copyFrom.cubeFace[5];
}
//------------------------------------------------------------------------------


