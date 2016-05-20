//==============================================================================
// TorqueLab -> Lab PostFX Manager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_ScatterSkyManager.buildParams();
function EPostFxManager::buildParamsLightRays( %this ) {
	%arCfg = Lab.createBaseParamsArray("EPostFx_LightRays",EPostFxMiscStack);
	%arCfg.updateFunc = "EPostFxManager.updateParamLightRays";
	%arCfg.style = "StyleA";
	%arCfg.useNewSystem = true;
	%arCfg.noContainerClear = true;
	%arCfg.prefGroup = "$PostFXManager::Settings::LightRays::";
	%arCfg.autoSyncPref = "1";
	%arCfg.group[%gid++] = "LightRays Settings" TAB "expanded 1;;autoCollapse 1";
	%arCfg.setVal("enableLightRays",       "" TAB "Enable LightRays" TAB "Checkbox" TAB "superClass>>EPostFx_EnableLightRaysCheckbox" TAB "" TAB %gid);
	%arCfg.setVal("brightScalar",       "" TAB "Brightness" TAB "SliderEdit" TAB "range>>0 5" TAB "" TAB %gid);
	%arCfg.setVal("numSamples",       "" TAB "Samples" TAB "SliderEdit" TAB "range>>20 512" TAB "" TAB %gid);
	%arCfg.setVal("density",       "" TAB "Density" TAB "SliderEdit" TAB "range>>0.01 1" TAB "" TAB %gid);
	%arCfg.setVal("weight",       "" TAB "Weight" TAB "SliderEdit" TAB "range>>0.1 10" TAB "" TAB %gid);
	%arCfg.setVal("decay",       "" TAB "Decay (0 to 1)" TAB "SliderEdit" TAB "range>>0.01 1;;linkSet>>decay_hi" TAB "" TAB %gid);
	%arCfg.setVal("decay_hi",       "" TAB "Decay (0.9 to 1)" TAB "SliderEdit" TAB "range>>0.9 1;;linkSet>>decay" TAB "" TAB %gid);
	%arCfg.setVal("exposure",       "" TAB "Exposure" TAB "SliderEdit" TAB "range>>0.01 1" TAB "" TAB %gid);
	%arCfg.setVal("resolutionScale",       "" TAB "ResolutionScale" TAB "SliderEdit" TAB "range>>0.1 10" TAB "" TAB %gid);

	buildParamsArray(%arCfg,false);
	%this.LightRaysParamArray = %arCfg;
}
//------------------------------------------------------------------------------
//$LightRaysPostFX::minLuminace;
//==============================================================================
function EPostFxManager::updateParamLightRays(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3) {
	logd("EPostFxManager::updateParamLightRays(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3)",%this,%field,%value,%ctrl,%arg1,%arg2,%arg3);
		if (!$EPostFxManager_LiveUpdate)
		return;
	//eval("$LightRayPostFx::"@%field@" = %value;");
	eval("$PostFXManager::Settings::LightRays::"@%field@" = %value;");
	PostFXManager.settingsEffectSetEnabled("LightRays",$PostFXManager::PostFX::EnableLightRays);
}
//------------------------------------------------------------------------------


//==============================================================================
// Common LightRays Gui COntrol Callbacks
//==============================================================================

//==============================================================================
// Enable/Disable LightRays
function EPostFx_EnableLightRaysCheckbox::onClick(%this) {
   
	PostFXManager.settingsEffectSetEnabled("LightRays", %this.isStateOn());
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_ScatterSkyManager.buildParams();
function EPostFxManager::buildParamsVignette( %this ) {
	%arCfg = Lab.createBaseParamsArray("EPostFx_Vignette",EPostFxMiscStack);
	%arCfg.updateFunc = "EPostFxManager.updateParamVignette";
	%arCfg.style = "StyleA";
	%arCfg.noContainerClear = true;
	%arCfg.useNewSystem = true;
	%arCfg.prefGroup = "$PostFXManager::Settings::Vignette::";
	%arCfg.autoSyncPref = "1";
	%arCfg.group[%gid++] = "Vignette Settings" TAB "expanded 0;;autoCollapse 1";
	%arCfg.setVal("enableVignette",       "" TAB "Enable Vignette" TAB "Checkbox" TAB "superClass>>EPostFx_EnableVignetteCheckbox" TAB "" TAB %gid);
	%arCfg.setVal("VMin",       "" TAB "VMin" TAB "SliderEdit" TAB "range>>0.1 10" TAB "" TAB %gid);
	%arCfg.setVal("VMax",       "" TAB "VMax" TAB "SliderEdit" TAB "range>>0.1 10" TAB "" TAB %gid);
	buildParamsArray(%arCfg,false);
	%this.VignetteParamArray = %arCfg;
}
//------------------------------------------------------------------------------

//==============================================================================
function EPostFxManager::updateParamVignette(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3) {
	logd("EPostFxManager::updateParamVignette(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3)",%this,%field,%value,%ctrl,%arg1,%arg2,%arg3);
	
		if (!$EPostFxManager_LiveUpdate)
		return;
	eval("$VignettePostFx::"@%field@" = %value;");
	//eval("$PostFXManager::Settings::Vignette::"@%field@" = %value;");
	PostFXManager.settingsEffectSetEnabled("Vignette",$PostFXManager::PostFX::EnableVignette);
}
//------------------------------------------------------------------------------

function EPostFx_EnableVignetteCheckbox::onClick(%this) {
	PostFXManager.settingsEffectSetEnabled("Vignette",  %this.isStateOn());
}

//==============================================================================
// CHromaticLens
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function EPostFxManager::buildParamsCA( %this ) {
	%arCfg = Lab.createBaseParamsArray("EPostFx_CA",EPostFxMiscStack);
	%arCfg.updateFunc = "EPostFxManager.updateParamCA";
	%arCfg.style = "StyleA";
	%arCfg.useNewSystem = true;
	%arCfg.noContainerClear = true;
	%arCfg.prefGroup = "$PostFXManager::Settings::CA::";
	%arCfg.autoSyncPref = "1";
	%arCfg.group[%gid++] = "CA Settings" TAB"expanded 0;;autoCollapse 1";
	%arCfg.setVal("enable",       "" TAB "Enable CA" TAB "Checkbox" TAB "superClass>>EPostFx_EnableCACheckbox" TAB "" TAB %gid);
	%arCfg.setVal("colorDistortionFactor",       "" TAB "colorDistortionFactor" TAB "SliderEdit" TAB "range>>0.1 10" TAB "" TAB %gid);
	%arCfg.setVal("cubeDistortionFactor",       "" TAB "cubeDistortionFactor" TAB "SliderEdit" TAB "range>>0.1 10" TAB "" TAB %gid);
	%arCfg.setVal("distCoeffecient",       "" TAB "distCoeffecient" TAB "SliderEdit" TAB "range>>0.1 10" TAB "" TAB %gid);
	buildParamsArray(%arCfg,false);
	%this.CAParamArray = %arCfg;
	
}
//------------------------------------------------------------------------------

//==============================================================================
function EPostFxManager::updateParamCA(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3) {
	logd("EPostFxManager::updateParamCA(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3)",%this,%field,%value,%ctrl,%arg1,%arg2,%arg3);
		if (!$EPostFxManager_LiveUpdate)
		return;
	eval("$CAPostFx::"@%field@" = %value;");
	//eval("$PostFXManager::Settings::CA::"@%field@" = %value;");

	PostFXManager.settingsEffectSetEnabled("CA",$PostFXManager::PostFX::EnableCA);
}
//------------------------------------------------------------------------------

function EPostFx_EnableCACheckbox::onClick(%this) {
	PostFXManager.settingsEffectSetEnabled("CA",  %this.isStateOn());
}

//==============================================================================
// Volumetric Fog Glow
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function EPostFxManager::buildParamsVolFogGlow( %this ) {
	%arCfg = Lab.createBaseParamsArray("EPostFx_VolFogGlow",EPostFxMiscStack);
	%arCfg.updateFunc = "EPostFxManager.updateParamVolFogGlow";
	%arCfg.style = "StyleA";
	%arCfg.useNewSystem = true;
	%arCfg.noContainerClear = true;
	%arCfg.prefGroup = "$PostFXManager::Settings::VolFogGlow::";
	%arCfg.autoSyncPref = "1";
	%arCfg.group[%gid++] = "VolFogGlow Settings" TAB "expanded 0;;autoCollapse 1";
	%arCfg.setVal("enable",       "" TAB "Enable VolFogGlow" TAB "Checkbox" TAB "superClass>>EPostFx_EnableVolFogGlowCheckbox" TAB "" TAB %gid);
	%arCfg.setVal("glowStrength",       "" TAB "glowStrength" TAB "SliderEdit" TAB "range>>0.1 10" TAB "" TAB %gid);

	buildParamsArray(%arCfg,false);
	%this.VolFogGlowParamArray = %arCfg;
}
//------------------------------------------------------------------------------

//==============================================================================
function EPostFxManager::updateParamVolFogGlow(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3) {
	logd("EPostFxManager::updateParamVolFogGlow(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3)",%this,%field,%value,%ctrl,%arg1,%arg2,%arg3);
		if (!$EPostFxManager_LiveUpdate)
		return;
	eval("$VolFogGlowPostFx::"@%field@" = %value;");
	//eval("$PostFXManager::Settings::VolFogGlow::"@%field@" = %value;");
	PostFXManager.settingsEffectSetEnabled("VolFogGlow",$PostFXManager::PostFX::EnableVolFogGlow);
}
//------------------------------------------------------------------------------

function EPostFx_EnableVolFogGlowCheckbox::onClick(%this) {
	PostFXManager.settingsEffectSetEnabled("VolFogGlow",  %this.isStateOn());
}

