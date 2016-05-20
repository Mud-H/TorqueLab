//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$DBSimpleSel = true;
//==============================================================================
function DbEd::setSelectedDatablock( %this,%datablock ) {
	if ($DBSimpleSel)
		return;	
	if (!isObject(%datablock)) {
		DbEd_DatablockNameEdit.setText("");
		hide(DbEd_ActiveDbIcons);
		DbEd_EditorStack.clear();
		hide(DbEd_EditorScroll);
		show(DbEd_NoConfigPill);
		DbEd_NoConfigPill-->titleText.setText("There's no selected datablock");
		DbEd_NoConfigPill-->infoText.setText("Select a datablock from the Datablocks tree above to see the custom editor which do the same as inspector" SPC
														 "but with a selection of settings and GuiControls adapted to the important datablock settings.");
		return;
	}
	%pm = DatablockEditorPlugin.PM;
	%dirty = %pm.isDirty(%datablock);
	DatablockEditorPlugin.setDatablockDirty(%datablock,%dirty);
	show(DbEd_ActiveDbIcons);
	DbEd.activeDatablock = %datablock;

	if (DbEd.isMethod("build"@%datablock.getClassName()@"Params")) {
		show(DbEd_EditorScroll);
		eval("DbEd.build"@%datablock.getClassName()@"Params();");
		hide(DbEd_NoConfigPill);
	} else {
		hide(DbEd_EditorScroll);
		DbEd_EditorStack.clear();
		show(DbEd_NoConfigPill);
		DbEd_NoConfigPill-->titleText.setText("No predefined settings for:" SPC %datablock.getClassName());
		%text = "There's no predefined settings for datablock of this class:\c1" SPC
				  %datablock.getClassName() @". You can create one by using one of the current as reference." SPC
				  "You can find those predefined datablocks in tlab> DatablockEditor> Editor> ClassParams> folder.";
		DbEd_NoConfigPill-->infoText.setText(%text);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//Called from EditorInspectorBase to open selected datablock
function DatablockEditorPlugin::openDatablock( %this, %datablock ) {
	// EditorGui.setEditor( DatablockEditorPlugin );
	%this.selectDatablock( %datablock );
	DatablockEditorTreeTabBook.selectedPage = 0;
}
//------------------------------------------------------------------------------
//==============================================================================
function DatablockEditorPlugin::onSelectObject( %this, %object ) {
	// Select datablock of object if this is a GameBase object.
	if( %object.isMemberOfClass( "GameBase" ) )
		%this.selectDatablock( %object.getDatablock() );
	else if( %object.isMemberOfClass( "SFXEmitter" ) && isObject( %object.track ) )
		%this.selectDatablock( %object.track );
	else if( %object.isMemberOfClass( "LightBase" ) && isObject( %object.animationType ) )
		%this.selectDatablock( %object.animationType );
}
//------------------------------------------------------------------------------
//==============================================================================
function DatablockEditorPlugin::getNumSelectedDatablocks( %this ) {
	return DatablockEditorTree.getSelectedItemsCount();
}
//------------------------------------------------------------------------------
//==============================================================================
function DatablockEditorPlugin::getSelectedDatablock( %this, %index ) {
	%tree = DatablockEditorTree;

	if( !%tree.getSelectedItemsCount() )
		return 0;

	if( !%index )
		%id = %tree.getSelectedItem();
	else
		%id = getWord( %tree.getSelectedItemList(), %index );

	return %tree.getItemValue( %id );
}
//------------------------------------------------------------------------------

//==============================================================================
function DatablockEditorPlugin::selectDatablockCheck( %this, %datablock ) {
	if( %this.selectedDatablockIsDirty() )
		%this.showSaveDialog( %datablock );
	else
		%this.selectDatablock( %datablock );
}
//------------------------------------------------------------------------------
//==============================================================================
function DatablockEditorPlugin::selectDatablock( %this, %datablock, %add, %dontSyncTree ) {
	trace();
	//if( %add )
		//DatablockEditorInspector.addInspect( %datablock );
	//else
		DatablockEditorInspector.inspect( %datablock );

	if( !%dontSyncTree ) {
		%id = DatablockEditorTree.findItemByValue( %datablock.getId() );

		if( !%add )
			DatablockEditorTree.clearSelection();

		DatablockEditorTree.selectItem( %id, true );
		DatablockEditorTree.scrollVisible( %id );
	}

//	%this.syncDirtyState();
	// Update the filename text field.
	//%numSelected = %this.getNumSelectedDatablocks();
	//%fileNameField = DatablockEditorInspectorWindow-->DatablockFile;

	if( %numSelected == 1 ) {
		%fileName = %datablock.getFilename();

		if( %fileName !$= "" )
			%fileNameField.setText( %fileName );
		else
			%fileNameField.setText( $DATABLOCK_EDITOR_DEFAULT_FILENAME );
	} else {
		%fileNameField.setText( "" );
	}

	DbEd.setSelectedDatablock(%datablock);
	//EditorGuiStatusBar.setSelection( %this.getNumSelectedDatablocks() @ " Datablocks Selected" );
	show(DbEd_ActiveDbIcons);
	trace(0);
}
//------------------------------------------------------------------------------


//==============================================================================
function DatablockEditorPlugin::unselectDatablock( %this, %datablock, %dontSyncTree ) {
	DatablockEditorInspector.removeInspect( %datablock );

	if( !%dontSyncTree ) {
		%id = DatablockEditorTree.findItemByValue( %datablock.getId() );
		DatablockEditorTree.selectItem( %id, false );
	}

	%this.syncDirtyState();
	// If we have exactly one selected datablock remaining, re-enable
	// the save-as button.
	%numSelected = %this.getNumSelectedDatablocks();

	if( %numSelected == 1 ) {
		DatablockEditorInspectorWindow-->saveAsButton.setActive( true );
		%fileNameField = DatablockEditorInspectorWindow-->DatablockFile;
		%fileNameField.setText( %this.getSelectedDatablock().getFilename() );
		%fileNameField.setActive( true );
	}

	EditorGuiStatusBar.setSelection( %this.getNumSelectedDatablocks() @ " Datablocks Selected" );
}
//------------------------------------------------------------------------------