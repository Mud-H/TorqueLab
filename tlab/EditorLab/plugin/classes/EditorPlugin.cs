//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
//==============================================================================
function EditorPlugin::onAdd( %this ) {
	EditorPluginSet.add( %this );
}
//------------------------------------------------------------------------------

//==============================================================================
function EditorPlugin::setEditorMode( %this,%mode ) {
	%editorGui = "E"@%mode@"Editor";

	if (!isObject(%editorGui)) {
		warnLog("Invalid editor mode called for plugin:",%this.displayName,"Mode attempted:",%mode);
		return;
	}

	%this.editorGui = %editorGui;
}
//------------------------------------------------------------------------------

//==============================================================================
// Initial editor initialization and shutdown callbacks
//==============================================================================

//==============================================================================
function EditorPlugin::onWorldEditorShutdown( %this ) {
}
//------------------------------------------------------------------------------
//==============================================================================
function EditorPlugin::onNewLevelLoaded( %this,%levelName ) {
}
//------------------------------------------------------------------------------
//==============================================================================
/// Callback right before the editor is opened.
function EditorPlugin::onEditorWake( %this ) {
}
//------------------------------------------------------------------------------
//==============================================================================
/// Callback right before the editor is closed.
function EditorPlugin::onEditorSleep( %this ) {
}
//------------------------------------------------------------------------------

//==============================================================================
/// Callback when tab is pressed.
/// Used by the WorldEditor to toggle between inspector/creator, for example.
function EditorPlugin::onToggleToolWindows( %this ) {
}
//------------------------------------------------------------------------------
//==============================================================================
/// Callback when the edit menu is clicked or prior to handling an accelerator
/// key event mapped to an edit menu item.
/// It is up to the active editor to determine if these actions are
/// appropriate in the current state.
function EditorPlugin::onEditMenuSelect( %this, %editMenu ) {
	%editMenu.enableItem( 3, false ); // Cut
	%editMenu.enableItem( 4, false ); // Copy
	%editMenu.enableItem( 5, false ); // Paste
	%editMenu.enableItem( 6, false ); // Delete
	%editMenu.enableItem( 8, false ); // Deselect
}
//------------------------------------------------------------------------------
//==============================================================================
/// If this tool keeps track of changes that necessitate resaving the mission
/// return true in that case.
function EditorPlugin::isDirty( %this ) {
	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
/// This gives tools a chance to clear whatever internal variables keep track of changes
/// since the last save.
function EditorPlugin::clearDirty( %this ) {
}
//------------------------------------------------------------------------------
//==============================================================================
/// This gives tools chance to save data out when the mission is being saved.
/// This will only be called if the tool says it is dirty.
function EditorPlugin::onSaveMission( %this, %missionFile ) {
}
//------------------------------------------------------------------------------
//==============================================================================
/// Called when during mission cleanup to notify plugins.
function EditorPlugin::onExitMission( %this ) {
}
//------------------------------------------------------------------------------


//==============================================================================
/// Callback when the the delete item of the edit menu is selected or its
/// accelerator is pressed.
function EditorPlugin::handleDelete( %this ) {
	warn( "EditorPlugin::handleDelete( " @ %this.getName() @ " )" NL
			"Was not implemented in child namespace, yet menu item was enabled." );
}
//------------------------------------------------------------------------------
//==============================================================================
/// Callback when the the deselect item of the edit menu is selected or its
/// accelerator is pressed.
function EditorPlugin::handleDeselect( %this ) {
	warn( "EditorPlugin::handleDeselect( " @ %this.getName() @ " )" NL
			"Was not implemented in child namespace, yet menu item was enabled." );
}
//------------------------------------------------------------------------------
//==============================================================================
/// Callback when the the cut item of the edit menu is selected or its
/// accelerator is pressed.
function EditorPlugin::handleCut( %this ) {
	warn( "EditorPlugin::handleCut( " @ %this.getName() @ " )" NL
			"Was not implemented in child namespace, yet menu item was enabled." );
}
//------------------------------------------------------------------------------
//==============================================================================
/// Callback when the the copy item of the edit menu is selected or its
/// accelerator is pressed.
function EditorPlugin::handleCopy( %this ) {
	warn( "EditorPlugin::handleCopy( " @ %this.getName() @ " )" NL
			"Was not implemented in child namespace, yet menu item was enabled." );
}
//------------------------------------------------------------------------------
//==============================================================================
/// Callback when the the paste item of the edit menu is selected or its
/// accelerator is pressed.
function EditorPlugin::handlePaste( %this ) {
	warn( "EditorPlugin::handlePaste( " @ %this.getName() @ " )" NL
			"Was not implemented in child namespace, yet menu item was enabled." );
}
//------------------------------------------------------------------------------
//==============================================================================
/// Callback when the escape key is pressed.
/// Return true if this tool has handled the key event in a custom way.
/// If false is returned the WorldEditor default behavior is to return
/// to the ObjectEditor.
function EditorPlugin::handleEscape( %this ) {
	return false;
}
//------------------------------------------------------------------------------

function EditorPlugin::setParam( %this,%field,%value ) {
	LabParams.updateParamSyncData(%field,%value,%this.paramArray);
	LabParams.setParamPillValue(%field,%value,%this.paramArray);
}
function EditorPlugin::setCtrlParam( %this,%field,%ctrl ) {
	%value = %ctrl.getTypeValue();
	LabParams.updateParamFromCtrl(%ctrl,%field,%value,%this.paramArray);
}

//==============================================================================





//==============================================================================
//MaterialEditorPlugin.toggleSettings();
function EditorPlugin::toggleSettings( %this) {
	LabParamsDlg.toggleSettings(%this.displayName);
}
//------------------------------------------------------------------------------