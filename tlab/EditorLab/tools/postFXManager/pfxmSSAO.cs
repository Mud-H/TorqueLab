//==============================================================================
// TorqueLab -> Lab PostFX Manager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$EPostFx_SSAO_QualityList = "Low Medium High";
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_ScatterSkyManager.buildParams();
function EPostFxManager::buildParamsSSAO( %this ) {
   
	%arCfg = Lab.createBaseParamsArray("EPostFx_SSAO",EPostFxPage_SSAOStack);
	%arCfg.updateFunc = "EPostFxManager.updateParamSSAO";
	
	%arCfg.useNewSystem = true;
	%arCfg.prefGroup = "$PostFXManager::Settings::SSAO::";
	%arCfg.autoSyncPref = "1";
	%arCfg.group[%gid++] = "General Settings";

	%arCfg.setVal("enableSSAO",       "" TAB "Enable SSAO" TAB "Checkbox" TAB "superClass>>EPostFx_EnableSSAOCheckbox" TAB "" TAB %gid);
	%arCfg.setVal("quality",       "" TAB "Quality" TAB "dropdown_25" TAB "itemList>>$EPostFx_SSAO_QualityList;;syncId>>1" TAB "" TAB %gid);
	%arCfg.setVal("overallStrength",       "" TAB "Overall strength" TAB "SliderEdit" TAB "range>>0 50" TAB "" TAB %gid);
	%arCfg.setVal("blurDepthTol",       "" TAB "Blur (Softness)" TAB "SliderEdit" TAB "range>>0 0.3" TAB "" TAB %gid);
	%arCfg.setVal("blurNormalTol",       "" TAB "Blur (Normal Maps)" TAB "SliderEdit" TAB "range>>0 1" TAB "" TAB %gid);
	%arCfg.setVal("targetScale",       "" TAB "Target Scale" TAB "SliderEdit" TAB "range>>0 1" TAB "" TAB %gid);
	%arCfg.group[%gid++] = "Near Settings";
	%arCfg.setVal("sRadius",       "" TAB "Radius" TAB "SliderEdit" TAB "range>>0.001 5" TAB "" TAB %gid);
	%arCfg.setVal("sStrength",       "" TAB "Strength" TAB "SliderEdit" TAB "range>>0 20" TAB "" TAB %gid);
	%arCfg.setVal("sDepthMin",       "" TAB "Depth min." TAB "SliderEdit" TAB "range>>0 5" TAB "" TAB %gid);
	%arCfg.setVal("sDepthMax",       "" TAB "Depth max." TAB "SliderEdit" TAB "range>>0 50" TAB "" TAB %gid);
	%arCfg.setVal("sNormalTol",       "" TAB "Normal Maps" TAB "SliderEdit" TAB "range>>0 2" TAB "" TAB %gid);
	%arCfg.setVal("sNormalPow",       "" TAB "Tolerance / Power" TAB "SliderEdit" TAB "range>>0 2" TAB "" TAB %gid);
	%arCfg.group[%gid++] = "Far Settings";
	%arCfg.setVal("lRadius",       "" TAB "Radius" TAB "SliderEdit" TAB "range>>0.001 5" TAB "" TAB %gid);
	%arCfg.setVal("lStrength",       "" TAB "Strength" TAB "SliderEdit" TAB "range>>0 20" TAB "" TAB %gid);
	%arCfg.setVal("lDepthMin",       "" TAB "Depth min." TAB "SliderEdit" TAB "range>>0 5" TAB "" TAB %gid);
	%arCfg.setVal("lDepthMax",       "" TAB "Depth max." TAB "SliderEdit" TAB "range>>0 5" TAB "" TAB %gid);
	%arCfg.setVal("lNormalTol",       "" TAB "Normal Maps" TAB "SliderEdit" TAB "range>>0 2" TAB "" TAB %gid);
	%arCfg.setVal("lNormalPow",       "" TAB "Tolerance / Power" TAB "SliderEdit" TAB "range>>0 2" TAB "" TAB %gid);
	buildParamsArray(%arCfg,false);
	%this.SSAOParamArray = %arCfg;
}
//------------------------------------------------------------------------------
//$SSAOPostFX::minLuminace;
//==============================================================================
function EPostFxManager::updateParamSSAO(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3) {
	logd("EPostFxManager::updateParamSSAO(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3)",%this,%field,%value,%ctrl,%arg1,%arg2,%arg3);
	if (!$EPostFxManager_LiveUpdate)
		return;
	eval("$SSAOPostFx::"@%field@" = %value;");
	//eval("$PostFXManager::Settings::SSAO::"@%field@" = %value;");
	PostFXManager.settingsEffectSetEnabled("Vignette",$PostFXManager::PostFX::EnabledSSAO);
}
//------------------------------------------------------------------------------


//==============================================================================
// Common SSAO Gui COntrol Callbacks
//==============================================================================

//==============================================================================
// Enable/Disable SSAO
function EPostFx_EnableSSAOCheckbox::onClick(%this) {
	PostFXManager.settingsEffectSetEnabled("SSAO", %this.isStateOn());
}
//------------------------------------------------------------------------------


//==============================================================================
// ColorShift SSAO Gui Control Callbacks
//==============================================================================
//==============================================================================
// ColorShift Color Picked
function EPostFx_SSAOQuality::onSelect(%this,%id,%text) {
	if(%id > -1 && %id < 3) {
		//$SSAOPostFx::quality = %id;
		$PostFXManager::Settings::SSAO::quality = %id;
	}
}
//------------------------------------------------------------------------------
