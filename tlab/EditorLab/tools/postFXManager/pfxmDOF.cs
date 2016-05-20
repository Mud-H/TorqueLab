//==============================================================================
// TorqueLab -> Lab PostFX Manager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$EPostFx_DOF_QualityList = "Low Medium High";
$EPostFxManager_LiveUpdate = true;
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_ScatterSkyManager.buildParams();
function EPostFxManager::buildParamsDOF( %this ) {
	%arCfg = Lab.createBaseParamsArray("EPostFx_DOF",EPostFxPage_DOFStack);
	%arCfg.updateFunc = "EPostFxManager.updateParamDOF";
	%arCfg.style = "StyleA";
	%arCfg.useNewSystem = true;
	%arCfg.prefGroup = "$PostFXManager::Settings::DOF::";
	%arCfg.autoSyncPref = "1";
	%arCfg.group[%gid++] = "General Settings";
	%arCfg.setVal("enableDOF",       "" TAB "Enable Depth of field" TAB "Checkbox" TAB "superClass>>EPostFx_EnableDOFCheckbox" TAB "" TAB %gid);
	%arCfg.setVal("enableAutoFocus",       "" TAB "Enable DOF AutoFocus" TAB "Checkbox" TAB "superClass>>EPostFx_EnableDOFAutoFocus" TAB "" TAB %gid);
	%arCfg.setVal("BlurMin",       "" TAB "Far Blur Min." TAB "SliderEdit" TAB "range>>0 1" TAB "" TAB %gid);
	%arCfg.setVal("BlurMax",       "" TAB "Far Blur Max." TAB "SliderEdit" TAB "range>>0 1" TAB "" TAB %gid);
	%arCfg.setVal("FocusRangeMin",       "" TAB "Focus Range Min." TAB "SliderEdit" TAB "range>>0 100" TAB "" TAB %gid);
	%arCfg.setVal("FocusRangeMax",       "" TAB "Focus Range Max." TAB "SliderEdit" TAB "range>>0 200" TAB "" TAB %gid);
	%arCfg.setVal("BlurCurveNear",       "" TAB "Blur Curve Near" TAB "SliderEdit" TAB "range>>-1 0" TAB "" TAB %gid);
	%arCfg.setVal("BlurCurveFar",       "" TAB "Blur Curve Far" TAB "SliderEdit" TAB "range>>0 1" TAB "" TAB %gid);
	buildParamsArray(%arCfg,false);
	%this.DOFParamArray = %arCfg;
}
//------------------------------------------------------------------------------
//$DOFPostFX::minLuminace;
//==============================================================================
function EPostFxManager::updateParamDOF(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3) {
	logd("EPostFxManager::updateParamDOF(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3)",%this,%field,%value,%ctrl,%arg1,%arg2,%arg3);
	if (!$EPostFxManager_LiveUpdate)
		return;
	eval("$DOFPostFx::"@%field@" = %value;");
	//eval("$PostFXManager::Settings::DOF::"@%field@" = %value;");
	
	ppOptionsUpdateDOFSettings();
}
//------------------------------------------------------------------------------

//==============================================================================
// Common DOF Gui COntrol Callbacks
//==============================================================================

//==============================================================================
// Enable/Disable DOF
function EPostFx_EnableDOFCheckbox::onClick(%this) {
	PostFXManager.settingsEffectSetEnabled("DOF", %this.isStateOn());
}
//-
function EPostFx_EnableDOFAutoFocus::onClick(%this) {
   $PostFXManager::Settings::DOF::EnableAutoFocus = %this.getValue();
	$DOFPostFx::EnableAutoFocus = %this.getValue();
	DOFPostEffect.setAutoFocus( %this.getValue() );
}
//-----------------------------------------------------------------------------
