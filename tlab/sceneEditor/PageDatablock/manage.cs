//==============================================================================
// TorqueLab -> Datablock Editor - Creation and Deletion
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Datablock Creation and Cloning
//==============================================================================
//==============================================================================
function SceneEd::createDatablock(%this) {
	%class = DatablockEditorTypeTree.getItemText(DatablockEditorTypeTree.getSelectedItem());

	if( %class !$= "" ) {
		// Need to prompt for a name.
		DatablockEditorCreatePrompt-->CreateDatablockName.setText("Name");
		DatablockEditorCreatePrompt-->CreateDatablockName.selectAllText();
		// Populate the copy source dropdown.
		%list = DatablockEditorCreatePrompt-->CopySourceDropdown;
		%list.clear();
		%list.add( "", 0 );
		%set = DataBlockSet;
		%count = %set.getCount();

		for( %i = 0; %i < %count; %i ++ ) {
			%datablock = %set.getObject( %i );
			%datablockClass = %datablock.getClassName();

			if( !isMemberOfClass( %datablockClass, %class ) )
				continue;

			%list.add( %datablock.getName(), %i + 1 );
		}

		// Set up state of client-side checkbox.
		%clientSideCheckBox = DatablockEditorCreatePrompt-->ClientSideCheckBox;
		%canBeClientSide = SceneEd::canBeClientSideDatablock( %class );
		%clientSideCheckBox.setStateOn( %canBeClientSide );
		%clientSideCheckBox.setActive( %canBeClientSide );
		// Show the dialog.
		canvas.pushDialog( DatablockEditorCreatePrompt, 0, true );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEd::createPromptNameCheck(%this) {
	%name = DatablockEditorCreatePrompt-->CreateDatablockName.getText();

	if( !Lab::validateObjectName( %name, true ) )
		return;

	// Fetch the copy source and clear the list.
	%copySource = DatablockEditorCreatePrompt-->copySourceDropdown.getText();
	DatablockEditorCreatePrompt-->copySourceDropdown.clear();
	// Remove the dialog and create the datablock.
	canvas.popDialog( DatablockEditorCreatePrompt );
	%this.createDatablockFinish( %name, %copySource,$DbEd_SaveInSourceFile,!$DbEd_CreateAsParent,$DbEd_CreateAndSelect );
}
//------------------------------------------------------------------------------
//==============================================================================
//Clone the selected datablock
function SceneEd::cloneDatablock( %this,%deepCloning ) {
	if (!isObject(DbEd.activeDatablock)) {
		LabMsgOk("No datablock selected","You need to select a datablock first!");
		return;
	}

	%copySource = DbEd.activeDatablock.getName();
	%name = getUniqueName(%copySource);
	%this.createDatablockFinish( %name, %copySource,true,%deepCloning,true );
}
//------------------------------------------------------------------------------


//==============================================================================
function SceneEd::createDatablockFinish( %this, %name, %copySource,%useSrcFile,%deepCloning,%autoSelect ) {
	%class = %copySource.getClassName();

	if (%class $= "")
		%class = DatablockEditorTypeTree.getItemText(DatablockEditorTypeTree.getSelectedItem());

	if( %class !$= "" ) {
		%action = %this.createUndo( ActionCreateDatablock, "Create New Datablock" );

		if( DatablockEditorCreatePrompt-->ClientSideCheckBox.isStateOn() )
			%dbType = "singleton ";
		else
			%dbType = "datablock ";

		if( %copySource !$= "" && !%deepCloning )
			%eval = %dbType @ %class @ "(" @ %name @ " : " @ %copySource @ ") { canSaveDynamicFields = \"1\"; };";
		else
			%eval = %dbType @ %class @ "(" @ %name @ ") { canSaveDynamicFields = \"1\"; };";

		%res = eval( %eval );

		//Using deepclone create a singleton so we simply assign fields from source
		if (%deepCloning && isObject(%copySource))
			%name.assignFieldsFrom(%copySource);

		if (%useSrcFile)
			%file = %copySource.getFileName();
		else
			%file = $DATABLOCK_EDITOR_DEFAULT_FILENAME;

		%name.setFileName(%file);
		%action.db = %name.getId();
		%action.dbName = %name;
		%action.fname = %file;
		%this.submitUndo( %action );
		%action.redo();
		info("New datablock created:",%name,"Of class:",%class);

		if (%autoSelect)
			%this.selectDatablock( %name );

		return %name;
	}

	info("Couldn't find a valid class for the new datablock:",%name,". The creation have been aborted!");
	return "";
}
//------------------------------------------------------------------------------
//==============================================================================
// Datablock Deletion
//==============================================================================
//==============================================================================
function SceneEd::deleteDatablock( %this ) {
	%tree = SceneDatablockTree;
	// If we have more than single datablock selected,
	// turn our undos into a compound undo.
	%numSelected = %tree.getSelectedItemsCount();

	if( %numSelected > 1 )
		Editor.getUndoManager().pushCompound( "Delete Multiple Datablocks" );

	for( %i = 0; %i < %numSelected; %i ++ ) {
		%id = %tree.getSelectedItem( %i );
		%db = %tree.getItemValue( %id );
		%fileName = %db.getFileName();
		// Remove the datablock from the tree.
		SceneDatablockTree.removeItem( %id );
		// Create undo.
		%action = %this.createUndo( ActionDeleteDatablock, "Delete Datablock" );
		%action.db = %db;
		%action.dbName = %db.getName();
		%action.fname = %fileName;
		%this.submitUndo( %action );

		// Kill the datablock in the file.

		if( %fileName !$= "" )
			%this.PM.removeObjectFromFile( %db );

		UnlistedDatablocks.add( %db );

		// Show some confirmation.

		if( %numSelected == 1 )
			LabMsgOK( "Datablock Deleted", "The datablock (" @ %db.getName() @ ") has been removed from " @
						 "it's file (" @ %db.getFilename() @ ") and upon restart will cease to exist" );
	}

	// Close compound, if we were deleting multiple datablocks.

	if( %numSelected > 1 )
		Editor.getUndoManager().popCompound();

	// Show confirmation for multiple datablocks.

	if( %numSelected > 1 )
		LabMsgOK( "Datablocks Deleted", "The datablocks have been deleted and upon restart will cease to exist." );

	// Clear selection.
	SceneEd.resetSelectedDatablock();
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEd::resetSelectedDatablock( %this ) {
	SceneDatablockTree.clearSelection();
	DatablockEditorInspector.inspect(0);
	DatablockEditorInspectorWindow-->DatablockFile.setText("");
	EditorGuiStatusBar.setSelection( "" );
}
//------------------------------------------------------------------------------

