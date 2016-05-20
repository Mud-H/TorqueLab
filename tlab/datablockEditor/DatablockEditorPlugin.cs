//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
// Scene Editor Params - Used set default settings and build plugins options GUI
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function DatablockEditorPlugin::initParamsArray( %this,%array ) {
	%array.group[%groupId++] = "General settings";
	%array.setVal("excludeClientOnlyDatablocks",       "1" TAB "excludeClientOnlyDatablocks" TAB "Checkbox"  TAB "" TAB "DatablockEditorPlugin" TAB %groupId);
}
//------------------------------------------------------------------------------

//==============================================================================
// Plugin Object Callbacks - Called from TLab plugin management scripts
//==============================================================================

//==============================================================================
// Called when TorqueLab is launched for first time
function DatablockEditorPlugin::onPluginLoaded( %this ) {	
	DatablockEditorTreeTabBook.selectPage( 0 );
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when the Plugin is activated (Active TorqueLab plugin)
function DatablockEditorPlugin::onActivated( %this ) {
	DatablockEditorInspectorWindow.makeFirstResponder( true );
	DbEd.setSelectedDatablock(DbEd.activeDatablock);

	if (DbEd.allClasses $= "")
		DatablockEditorPlugin.buildClassList();

	if (DbEd.activeClasses $= "")
		DbEd.selectAllClasses();

	// Set the status bar here until all tool have been hooked up
	EditorGuiStatusBar.setInfo( "Datablock editor." );
	%numSelected = %this.getNumSelectedDatablocks();

	if( !%numSelected )
		EditorGuiStatusBar.setSelection( "" );
	else
		EditorGuiStatusBar.setSelection( %numSelected @ " datablocks selected" );

	if( !DatablockEditorTree.getItemCount() )
		%this.populateTrees();

	if( EWorldEditor.getSelectionSize() == 1 )
		%this.onSelectObject( EWorldEditor.getSelectedObject( 0 ) );

	DbEd.initGui();
	Parent::onActivated( %this );
	DbEd_SelectionTabBook.selectPage(0);
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when the Plugin is deactivated (active to inactive transition)
//function DatablockEditorPlugin::onDeactivated( %this ) {}
//------------------------------------------------------------------------------
//==============================================================================
// Called from TorqueLab after plugin is initialize to set needed settings
//function DatablockEditorPlugin::onPluginCreated( %this ) {}
//------------------------------------------------------------------------------

//==============================================================================
// Called when the mission file has been saved
//function DatablockEditorPlugin::onSaveMission( %this, %file ) {}
//------------------------------------------------------------------------------
//==============================================================================
// Called when the mission file has been saved
function DatablockEditorPlugin::onExitMission( %this ) {
	DatablockEditorTree.clear();
	DatablockEditorInspector.inspect( "" );
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when TorqueLab is closed
//function DatablockEditorPlugin::onEditorSleep( %this ) {}
//------------------------------------------------------------------------------
//==============================================================================
//Called when editor is selected from menu
//function SceneEditorPlugin::onEditMenuSelect( %this, %editMenu ) {}
//------------------------------------------------------------------------------
