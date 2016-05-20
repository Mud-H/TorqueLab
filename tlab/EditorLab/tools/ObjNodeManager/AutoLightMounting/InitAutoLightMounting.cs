//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function EAutoLightManager::onWake( %this ) {
	EAutoLightManager.initSystem();
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_ScatterSkyManager.buildParams();
function EAutoLightManager::initSystem(%this) {
	if (isObject(AutoMountLights)){
		AutoLightObjTree.open(AutoMountLights);
		AutoLightObjTree.buildVisibleTree();
	}
	if (isObject(mgLightShapes)){
		AutoLightShapeTree.open(mgLightShapes);
		AutoLightShapeTree.buildVisibleTree();
	}
}

//------------------------------------------------------------------------------
function Lab::setAutoLight(%this,%obj) {
	if (!isObject(%obj))
		return;
	Lab.activeAutoLightObj = %obj;
	EAutoLightManager.activeLight = %obj;
}

