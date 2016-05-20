//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Editor Open/Close Callbacks
//==============================================================================
function Lab::onEditorOpen(%this,%type) {
	logd("Lab::onEditorOpen",%type);
	%this.initialBindCmd["enter"] = 	globalActionMap.getCommand( keyboard, enter);
	 globalActionMap.unbind( keyboard, enter);
	 
	%this.initialBindCmd["button1"] = 	globalActionMap.getCommand( mouse0, button1);
	 globalActionMap.unbind( mouse0, button1);
	  $LabEditorActiveType = %type;
	  $LabEditorActive = true;
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::onEditorClose(%this,%type) {
	logd("Lab::onEditorClose",%type);
	if ( %this.initialBindCmd["enter"] !$= "")
	 globalActionMap.bind( keyboard, enter, %this.initialBindCmd["enter"]);
	 if ( %this.initialBindCmd["button1"] !$= "")
	 globalActionMap.bind( mouse0, button1, %this.initialBindCmd["button1"]);
	 $LabEditorActiveType = "";
	$LabEditorActive = false;
	
}
//------------------------------------------------------------------------------

//==============================================================================
// Editor onWake/OnSleep Callbacks
//==============================================================================
//==============================================================================
function Lab::onEditorWake(%this,%type) {	 
	logd("Lab::onEditorWake",%type);
	 $LabEditorAwake = true;
	 $LabEditorAwakeType = %type;
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::onEditorSleep(%this,%type) {
	logd("Lab::onEditorSleep",%type);
	 $LabEditorAwake = false;
	 $LabEditorAwakeType = "";
}
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function Lab::onWorldEditorOpen( %this ) {
	Lab.onEditorOpen("World");
	//Check for methods in each plugin objects
	foreach(%pluginObj in EditorPluginSet) {
		if (%pluginObj.isMethod("onEditorOpen"))
			%pluginObj.onEditorOpen();
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::onWorldEditorWake( %this ) {
	Lab.onEditorWake("World");
	EditorMap.push();
	
	//Check for methods in each plugin objects
	foreach(%pluginObj in EditorPluginSet) {
		if (%pluginObj.isMethod("onEditorWake"))
			%pluginObj.onEditorWake();
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::onWorldEditorClose( %this ) {
	Lab.onEditorClose("World");
	if(isObject(HudChatEdit))
		HudChatEdit.close();

	//Restore the Client COntrolling Object
	if (isObject( Lab.clientWasControlling))
		LocalClientConnection.setControlObject( Lab.clientWasControlling );
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::onWorldEditorSleep( %this ) {
	Lab.onEditorSleep("World");
	
	// Remove the editor's ActionMaps.
	EditorMap.pop();
	MoveMap.pop();
	

	// Notify the editor plugins that the editor will be closing.
	foreach( %plugin in EditorPluginSet )
		%plugin.onEditorSleep();
}
//------------------------------------------------------------------------------

//==============================================================================
// GUI EDITOR CALLBACKS
//==============================================================================

//==============================================================================
function Lab::onGuiEditorOpen( %this ) {
	logd("Lab::onGuiEditorOpen");
	Lab.onEditorOpen("Gui");
	GuiEdMap.push();	
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::onGuiEditorWake( %this ) {
	logd("Lab::onGuiEditorWake");
	Lab.onEditorWake("Gui");
	
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::onGuiEditorClose( %this ) {
	logd("Lab::onGuiEditorClose");
	Lab.onEditorClose("Gui");
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::onGuiEditorSleep( %this ) {
	logd("Lab::onGuiEditorSleep");
	Lab.onEditorSleep("Gui");
	//GuiEdMap.pop();	
	GuiEdMap.pop();
}
//------------------------------------------------------------------------------