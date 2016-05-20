//==============================================================================
// TorqueLab -> Datablock Editor Persistence Manager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function DatablockEditorPlugin::setDatablockDirty(%this, %datablock, %dirty ) {
	if (%dirty $= "")
		%dirty = true;
	
	%tree = DatablockEditorTree;
	%id = %tree.findItemByValue( %datablock.getId() );

	if( %id != 0 ){
		if( %dirty ) {
			DatablockEditorTree.editItem( %id, %datablock.getName() @ " *", %datablock.getId() );		
		} else {
			DatablockEditorTree.editItem( %id, %datablock.getName(), %datablock.getId() );		
		}
	}
	
	DbEd_ActiveDbIcons-->saveButton.visible = %dirty;	
	
	//Hack: For unknown reason the icon visibility need to be set twice using a schedule for the second
	if (%dirty)
		DbEd_ActiveIconSave.schedule(100,"setVisible","1");
}
//------------------------------------------------------------------------------


//==============================================================================
//- Return true if there is any datablock with unsaved changes.
function DatablockEditorPlugin::isDirty( %this ) {
	return %this.PM.hasDirty();
}
//------------------------------------------------------------------------------
//==============================================================================
//- Return true if any of the currently selected datablocks has unsaved changes.
function DatablockEditorPlugin::selectedDatablockIsDirty( %this ) {
	%tree = DatablockEditorTree;
	%count = %tree.getSelectedItemsCount();
	%selected = %tree.getSelectedItemList();

	foreach$( %id in %selected ) {
		%db = %tree.getItemValue( %id );

		if( %this.PM.isDirty( %db ) )
			return true;
	}

	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
function DatablockEditorPlugin::syncDirtyState( %this ) {
	%tree = DatablockEditorTree;
	%count = %tree.getSelectedItemsCount();
	%selected = %tree.getSelectedItemList();
	%haveDirty = false;

	foreach$( %id in %selected ) {
		%db = %tree.getItemValue( %id );

		if( %this.PM.isDirty( %db ) ) {
			%this.flagDatablockAsDirty( %db, true );
			%haveDirty = true;
		} else
			%this.flagDatablockAsDirty( %db, false );
	}

	%this.flagInspectorAsDirty( %haveDirty );
}
//------------------------------------------------------------------------------
//==============================================================================
//-
function DatablockEditorPlugin::flagInspectorAsDirty( %this, %dirty ) {
	
	if( %dirty ) {
		DatablockEditorInspectorWindow.text = "Datablock *";
		show(DbEd_ReloadDataButton);
	} else {
		DatablockEditorInspectorWindow.text = "Datablock";
		hide(DbEd_ReloadDataButton);
	}	
	
}
//------------------------------------------------------------------------------
//==============================================================================
function DatablockEditorPlugin::flagDatablockAsDirty(%this, %datablock, %dirty ) {
	//%tree = DatablockEditorTree;
	//%id = %tree.findItemByValue( %datablock.getId() );

	if( !isObject(%datablock))
		return;

	// Tag the item caption and sync the persistence manager.

	if( %dirty ) {
		//DatablockEditorTree.editItem( %id, %datablock.getName() @ " *", %datablock.getId() );
		%this.PM.setDirty( %datablock );
	} else {
		//DatablockEditorTree.editItem( %id, %datablock.getName(), %datablock.getId() );
		%this.PM.removeDirty( %datablock );
	}
	%this.setDatablockDirty(%datablock, %dirty);
	// Sync the inspector dirty state.
	%this.flagInspectorAsDirty( %this.PM.hasDirty() );
}
//------------------------------------------------------------------------------
//==============================================================================
function DatablockEditorPlugin::showSaveNewFileDialog(%this) {
	%currentFile = %this.getSelectedDatablock().getFilename();
	getSaveFilename( "TorqueScript Files|*.cs|All Files|*.*", %this @ ".saveNewFileFinish", %currentFile, false );
}
//------------------------------------------------------------------------------
//==============================================================================
function DatablockEditorPlugin::saveNewFileFinish( %this, %newFileName ) {
	// Clear the first responder to capture any inspector changes
	%ctrl = canvas.getFirstResponder();
	%newFileName = makeRelativePath(%newFileName);

	if( isObject(%ctrl) )
		%ctrl.clearFirstResponder();

	%tree = DatablockEditorTree;
	%count = %tree.getSelectedItemsCount();
	%selected = %tree.getSelectedItemList();

	foreach$( %id in %selected ) {
		%db = %tree.getItemValue( %id );
		%db = %this.getSelectedDatablock();
		// Remove from current file.
		%oldFileName = %db.getFileName();

		if( %oldFileName !$= "" )
			%this.PM.removeObjectFromFile( %db, %oldFileName );

		// Save to new file.
		%this.PM.setDirty( %db, %newFileName );

		if( %this.PM.saveDirtyObject( %db ) ) {
			// Clear dirty state.
			%this.flagDatablockAsDirty( %db, false );
		}
	}

	DatablockEditorInspectorWindow-->DatablockFile.setText( %newFileName );
}
//------------------------------------------------------------------------------
//==============================================================================
function DatablockEditorPlugin::save( %this ) {
	// Clear the first responder to capture any inspector changes
	%ctrl = canvas.getFirstResponder();

	if( isObject(%ctrl) )
		%ctrl.clearFirstResponder();

	%tree = DatablockEditorTree;
	%count = %tree.getSelectedItemsCount();
	%selected = %tree.getSelectedItemList();

	if (%count == 1) {
		%this.saveSingleData(DbEd.activeDatablock);
		return;
	}

	for( %i = 0; %i < %count; %i ++ ) {
		%id = getWord( %selected, %i );
		%db = %tree.getItemValue( %id );

		if( %this.PM.isDirty( %db ) ) {
			%this.PM.saveDirtyObject( %db );
			%this.flagDatablockAsDirty( %db, false );
		}
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function DatablockEditorPlugin::saveSingleData( %this,%db ) {
	// Clear the first responder to capture any inspector changes
	if (DbEd_DatablockNameEdit.getText() !$= %db.getFileName()) {
		%oldFileName = %db.getFileName();

		if( %oldFileName !$= "" )
			%this.PM.removeObjectFromFile( %db, %oldFileName );

		// Save to new file.
		%this.PM.setDirty( %db, DbEd_DatablockNameEdit.getText() );
	}

	if( %this.PM.isDirty( %db ) ) {
		%this.PM.saveDirtyObject( %db );
		
	}
	%this.flagDatablockAsDirty( %db, %this.PM.isDirty( %db ) );
}
//------------------------------------------------------------------------------
