//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
//Called from EditorInspectorBase to open selected datablock
function SceneEd::openDatablock( %this, %datablock ) {
	// EditorGui.setEditor( SceneEd );
	%this.selectDatablock( %datablock );
	SceneDatablockTreeTabBook.selectedPage = 0;
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEd::onSelectObject( %this, %object ) {
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
function SceneEd::getNumSelectedDatablocks( %this ) {
	return SceneDatablockTree.getSelectedItemsCount();
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEd::getSelectedDatablock( %this, %index ) {
	%tree = SceneDatablockTree;

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
function SceneEd::selectDatablockCheck( %this, %datablock ) {
	if( %this.selectedDatablockIsDirty() )
		%this.showSaveDialog( %datablock );
	else
		%this.selectDatablock( %datablock );
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEd::selectDatablock( %this, %datablock, %add, %dontSyncTree ) {
	//SceneInspector.inspect(%obj);
	//if( %add )
		//DatablockEditorInspector.addInspect( %datablock );
	//else
	//	DatablockEditorInspector.inspect( %datablock );
		Scene.doInspect(%datablock);

	
}
//------------------------------------------------------------------------------


//==============================================================================
function SceneEd::unselectDatablock( %this, %datablock, %dontSyncTree ) {
	Scene.doRemoveInspect( %datablock );

	//if( !%dontSyncTree ) {
	//	%id = SceneDatablockTree.findItemByValue( %datablock.getId() );
//		SceneDatablockTree.selectItem( %id, false );
//	}

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