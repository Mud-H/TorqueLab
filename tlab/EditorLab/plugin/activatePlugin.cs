//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
/// Callback when the tool is 'activated' by the WorldEditor
/// Push Gui's, stuff like that
function EditorPlugin::onActivated( %this ) {
	
	Lab.fitCameraGui = ""; //Used by GuiShapeLabPreview to Fit camera on object

	if (isObject(%this.editorGui))
		%this.editorGui.fitIntoParents();
		
	if (isObject(ECamViewGui))
	{		
		if (%this.no3D)
			ECamViewGui.setState(false,true);
		else
			ECamViewGui.setState($Cfg_Common_Camera_CamViewEnabled);
	}

	
		//Hide all the Guis for all plugins
		foreach(%gui in LabPluginGuiSet) {
			%gui.setVisible(false);
		}
	

	//Show only the Gui related to actiavted plugin
	if (%this.plugin !$= "") {
		%pluginGuiSet = %this.plugin@"_GuiSet";
		if (!isObject(%pluginGuiSet))
			warnLog("Invalid Plugin GuiSet for update in EditorPlugin::onActivated, tried:",%pluginGuiSet);
		else
			foreach(%gui in %pluginGuiSet) {
				//Don't show dialogs
				if (%gui.isDlg)
					continue;

				%gui.setVisible(true);
			}
	}

	%this.isActivated = true;

	if(isObject(%this.map))
		%this.map.push();

	if( isObject( %this.editorGui ) ) {
		show(%this.editorGui);
		%this.editorGui.setDisplayType( Lab.cameraDisplayType );
		%this.editorGui.setOrthoFOV( Lab.orthoFOV );
		// Lab.syncCameraGui();
	} else {
		warnLog("The plugin",%this.displayName,"have no editor GUI assigned. Using default World Editor GUI");
	}
    Lab.schedule(200,"checkPluginTools");

	
	
}
//------------------------------------------------------------------------------
//==============================================================================
/// Callback when the tool is 'deactivated' / closed by the WorldEditor
/// Pop Gui's, stuff like that
function EditorPlugin::onDeactivated( %this,%newEditor ) {
	stopToolTime(%this.getName());

	if(isObject(%this.map))
		%this.map.pop();

	hide(%this.editorGui);
	%this.isActivated = false;

}
//------------------------------------------------------------------------------
