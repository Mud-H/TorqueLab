//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
//Close editor call
function Editor::close(%this, %gui) {
	logd("Editor::close");
	EWorldEditor.clearSelection();
	Lab.onWorldEditorClose();	
	
	if (!isObject(%gui))
		%gui = EditorGui.previousGui;

	Canvas.setContent(%gui);	
	
}
//------------------------------------------------------------------------------
//==============================================================================
// EditorGui onSleep -> When the EditorGui is hidded
function EditorGui::onSleep( %this ) {
	logd("EditorGui::onSleep");
	deactivatePackage( TorqueLabPackage );
	// Deactivate the current editor plugin.
	Lab.onWorldEditorSleep();
	if (!$pref::Misc::AlwaysNotifyFileChange)
		stopFileChangeNotifications();

	if( Lab.currentEditor.isActivated )
		Lab.currentEditor.onDeactivated();


   Lab.storePluginsToolbarState();

	//Get the current Plugin Icon order
	Lab.updatePluginIconOrder();
	//Lab.saveAllPluginData();
	LabCfg.writeBaseConfig();
	Lab.onWorldEditorSleep();

	if(isObject($Server::CurrentScene))
		$Server::CurrentScene.open();

	//Set the game camera (Will load the same camera object as before entering editor)
	Lab.restoreClientCameraState();
	//Lab.setGameCamera();
}
//------------------------------------------------------------------------------
//==============================================================================
// Called before onSleep when the canvas content is changed
function EditorGui::onUnsetContent(%this, %newContent) {
	Lab.detachMenus();
}
//------------------------------------------------------------------------------
//==============================================================================
// Shutdown the EditorGui-> Called from the onExit function
function EditorGui::shutdown( %this ) {
	logd("EditorGui::shutdown");
	if (isObject(Lab.editCamera)) {
		Lab.editCamera.delete();
	}

	if (isObject(Lab.initialCamera)) {
		Lab.initialCamera.delete();
	}

	// Store settings.
	LabCfg.writeBaseConfig();

	// Deactivate current editor.
	if ( isObject( Lab.currentEditor ) && Lab.currentEditor.isActivated)
		Lab.currentEditor.onDeactivated();

	// Call the shutdown callback on the editor plugins.
	foreach( %plugin in EditorPluginSet )
		%plugin.onWorldEditorShutdown();
}
//------------------------------------------------------------------------------

//==============================================================================
// Called when a mission is ended-> used to cleanup Plugins objects
function EditorMissionCleanup::onRemove( %this ) {
	Lab.levelName = "";

	foreach( %plugin in EditorPluginSet )
		%plugin.onExitMission();
}
//------------------------------------------------------------------------------
//==============================================================================
// TorqueLab Quitting Binds/Menu Call
//==============================================================================

//==============================================================================
// Handle the escape bind
function EditorGui::handleEscape( %this ) {
	%result = false;

	//Check if the current Plugin accept the request
	if ( isObject( Lab.currentEditor ) )
		%result = Lab.currentEditor.handleEscape();

	if ( !%result ) {
		LabMsgYesNo( "Leaving the game?", "Are you sure you want to exit the level and go back to main menu? If you want to leave editor and test your level press NO?" SPC
						 "If you don't know what you are doing, hit CANCEL...", "disconnect();", "Editor.close();","" );
		//Editor.close($HudCtrl);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::QuitGame(%this) {
	if( EditorIsDirty() && !isWebDemo()) {
		LabMsgYesNoCancel("Level Modified", "Would you like to save your changes before quitting?", "Lab.SaveCurrentMission(); quit();", "quit();", "" );
	} else
		quit();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::ExitMission(%this) {
	if( EditorIsDirty() && !isWebDemo() ) {
		LabMsgYesNoCancel("Level Modified", "Would you like to save your changes before exiting?", "Lab.DoExitMission(true);", "Lab.DoExitMission(false);", "");
	} else
		Lab.DoExitMission(false);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::DoExitMission(%this,%saveFirst) {
	%this.DoExitMissionCleanup();
	
	if(%saveFirst ) {
		Lab.SaveCurrentMission();
	} else {
		EditorClearDirty();
	}

	if (isObject( $Cfg_TLab_defaultGui ))
		Editor.close("$Cfg_TLab_defaultGui");

	disconnect();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::DoExitMissionCleanup(%this) {
	foreach( %plugin in EditorPluginSet )
      %plugin.onExitMission();	
}
//------------------------------------------------------------------------------