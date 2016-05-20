//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function Lab::showLights( %this,%visible,%group ) {
	if (%visible $= "")
		%visible = true;
	
	EVisibilityLayers.setLightsVisible(%visible,%group);
}
//------------------------------------------------------------------------------



//==============================================================================
function EVisibilityLayers::showLights( %this,%group ) {
	%this.setLightsVisible(true,%group);
}
//------------------------------------------------------------------------------
//==============================================================================
//EVisibilityLayers.hideLights();
function EVisibilityLayers::hideLights( %this,%group ) {
	%this.setLightsVisible(false,%group);
	
}
//------------------------------------------------------------------------------

//==============================================================================
function EVisibilityLayers::setLightsVisible( %this,%visible,%group ) {
	if (!isObject(%group)){
		if (isObject(SceneEditorCfg.lightsGroup))
			%group = SceneEditorCfg.lightsGroup;
		else
			%group = MissionGroup;
	}
	%this.lightFound = 0;
	%this.setLightGroupVisible(%visible,%group);
	
}
//------------------------------------------------------------------------------

//==============================================================================
function EVisibilityLayers::setLightGroupVisible( %this,%visible,%group,%level ) {
	if (!isObject(%group) || %level > 20)
		return;
		
	foreach(%obj in %group){
		if (%obj.isMemberOfClass("SimSet")){
			%this.setLightGroupVisible(%visible,%obj,%level++);
			continue;
		}
		if (%obj.isMemberOfClass("PointLight")){
			%obj.hidden = !%visible;
			%this.lightFound++;
		}
	}
	
}
//------------------------------------------------------------------------------