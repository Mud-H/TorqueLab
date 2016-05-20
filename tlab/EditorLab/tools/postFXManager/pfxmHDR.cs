//==============================================================================
// TorqueLab -> Lab PostFX Manager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_ScatterSkyManager.buildParams();
function EPostFxManager::buildParamsHDR( %this ) {
	%arCfg = Lab.createBaseParamsArray("EPostFx_HDR",EPostFxPage_HDR);
	%arCfg.updateFunc = "EPostFxManager.updateParamHDR";
	%arCfg.style = "StyleA";
	%arCfg.useNewSystem = true;
	%arCfg.prefGroup = "$PostFXManager::Settings::HDR::";
	%arCfg.autoSyncPref = "1";
	%arCfg.group[%gid++] = "HDR Brightness Settings" TAB "StackType Header;;Stack StackBrightness";
	%arCfg.setVal("enableToneMapping",       "" TAB "Tone mapping" TAB "SliderEdit" TAB "range>>0 1" TAB "" TAB %gid);
	%arCfg.setVal("keyValue",       "" TAB "Key value" TAB "SliderEdit" TAB "range>>0 1" TAB "" TAB %gid);
	%arCfg.setVal("minLuminace",       "" TAB "Liminance min." TAB "SliderEdit" TAB "range>>0 1" TAB "" TAB %gid);
	%arCfg.setVal("whiteCutoff",       "" TAB "White cut-off" TAB "SliderEdit" TAB "range>>0 1" TAB "" TAB %gid);
	%arCfg.setVal("adaptRate",       "" TAB "Adapt rate" TAB "SliderEdit" TAB "range>>0.1 10" TAB "" TAB %gid);
	%arCfg.group[%gid++] = "HDR Bloom Settings" TAB "StackType Header;;Stack StackBloom";
	%arCfg.setVal("enableBloom",       "" TAB "Enable bloom" TAB "checkbox_32" TAB "superClass>>EPostFx_HDRCheckbox" TAB "" TAB %gid);
	%arCfg.setVal("brightPassThreshold",       "" TAB "Bright pass threshold" TAB "SliderEdit" TAB "range>>0 5" TAB "" TAB %gid);
	%arCfg.setVal("gaussMean",       "" TAB "Blur mean" TAB "SliderEdit" TAB "range>>0 1" TAB "" TAB %gid);
	%arCfg.setVal("gaussStdDev",       "" TAB "Blur Std Dev" TAB "SliderEdit" TAB "range>>0 3" TAB "" TAB %gid);
	%arCfg.setVal("gaussMultiplier",       "" TAB "Blur Multiplier" TAB "SliderEdit" TAB "range>>0 5" TAB "" TAB %gid);
	//%arCfg.group[%gid++] = "ColorShift Settings" TAB "StackType Header;;Stack StackColor";
	//%arCfg.setVal("enableBlueShift",       "" TAB "Color shift amount (0 = disabled)" TAB "SliderEdit" TAB "range>>0 2" TAB "" TAB %gid);
	//%arCfg.setVal("EPostFxPage_ColorShiftSelect",       "" TAB "" TAB "CloneCtrl" TAB "" TAB "" TAB %gid);
	buildParamsArray(%arCfg,false);
	%this.HDRParamArray = %arCfg;
	//%colorShiftClone = EPostFxPage_ColorShiftSelect.deepClone();
	//EPostFxPage_ColorShiftSelect.visible = 0;
}
//syncParamArray(arEPostFx_HDRParam);
//------------------------------------------------------------------------------
//$HDRPostFX::minLuminace;
//==============================================================================
function EPostFxManager::updateParamHDR(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3) {
	logd("EPostFxManager::updateParamHDR(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3)",%this,%field,%value,%ctrl,%arg1,%arg2,%arg3);
	if (!$EPostFxManager_LiveUpdate)
		return;
	eval("$HDRPostFx::"@%field@" = %value;");
	//eval("$PostFXManager::Settings::HDR::"@%field@" = %value;");
	PostFXManager.settingsEffectSetEnabled("HDR",$PostFXManager::PostFX::EnableHDR);
	
}
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
function EPostFxManager::getColorCorrectionFile(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3) {
	logd("EPostFxManager::updateParamHDR(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3)",%this,%field,%value,%ctrl,%arg1,%arg2,%arg3);
	%filter = "Image Files (*.png, *.jpg, *.dds, *.bmp, *.gif, *.jng. *.tga)|*.png;*.jpg;*.dds;*.bmp;*.gif;*.jng;*.tga|All Files (*.*)|*.*|";
	getLoadFilename(%filter,"EPostFxManager.setColorCorrectionFile",$PostFXManager::Settings::ColorCorrectionRamp);
}
//------------------------------------------------------------------------------
//==============================================================================
function EPostFxManager::setColorCorrectionFile(%this,%filename,%reset) {
	logd("EPostFxManager::updateParamHDR(%this,%filename)",%this,%filename);	

	if ( %filename $= "" || !isFile( %filename ) || %reset )
	{
	   %filename = "core/scripts/client/postFx/null_color_ramp.png";		
	}

	%filename = makeRelativePath( %filename, getMainDotCsDir() );
	$PostFXManager::Settings::ColorCorrectionRamp = %filename;
	%this-->ColorCorrectionFileName.Text = %filename;
}
//------------------------------------------------------------------------------
//==============================================================================
// Common HDR Gui COntrol Callbacks
//==============================================================================

//==============================================================================
// Enable/Disable HDR
function EPostFx_EnableHDRCheckbox::onClick(%this) {
	PostFXManager.settingsEffectSetEnabled("HDR", %this.isStateOn());
}
//------------------------------------------------------------------------------
//==============================================================================
// Enable/Disable HDR Debug
function EPostFx_DebugHDRCheckbox::onClick(%this) {
	if ( %this.getValue() )
		LuminanceVisPostFX.enable();
	else
		LuminanceVisPostFX.disable();

	$HDRPostFX::DebugEnabled = %this.getValue();
}
//------------------------------------------------------------------------------
//==============================================================================
// General Checkbox Settings
function EPostFx_HDRCheckbox::onAction(%this) {
	eval("$HDRPostFX::"@%this.internalName@" = %this.getValue();");
}
//------------------------------------------------------------------------------
//==============================================================================
// ColorShift HDR Gui Control Callbacks
//==============================================================================
//==============================================================================
// ColorShift Color Picked
function EPostFx_ColorShiftPicker::onAction(%this) {
	$PostFXManager::Settings::HDR::blueShiftColor = %this.PickColor;
	if ($EPostFxManager_LiveUpdate)
	   $HDRPostFX::blueShiftColor = %this.PickColor;
	%this.ToolTip = "Color Values : " @ %this.PickColor;
}
//------------------------------------------------------------------------------
//==============================================================================
// ColorShift Base Color Picked
function EPostFx_ColorShiftBasePicker::onAction(%this) {
	%this.parentGroup-->ShiftPicker.baseColor = %this.PickColor;
	%this.parentGroup-->ShiftPicker.updateCOlor();
	$PostFXManager::Settings::HDR::blueShiftColor = %this.parentGroup-->ShiftPicker.PickColor;
	if ($EPostFxManager_LiveUpdate)
	   $HDRPostFX::blueShiftColor = %this.parentGroup-->ShiftPicker.PickColor;
	//EPostFx_ColorShiftPicker.baseColor = %this.PickColor;
	%this.ToolTip = "Color Values : " @ %this.PickColor;
}
//------------------------------------------------------------------------------
function EPostFxManager::updateColorShiftSlider(%this,%slider) {
	//$PostFXManager::Settings::HDR::blueShiftColor = %slider.getValue();
}
//==============================================================================
function EPostFxManager::customSyncHDR(%this) {
	logd("EPostFxManager::customSyncHDR(%this)",%this);
	%debug = $HDRPostFX::DebugEnabled;
	EPostFxManager_Main-->enableDebugMode.setStateOn(%debug);
	EPostFx_EnableHDRCheckbox.setStateOn($PostFXManager::PostFX::EnableHDR);
	%this-->ColorCorrectionFileName.Text = $PostFXManager::Settings::ColorCorrectionRamp;
}
//------------------------------------------------------------------------------
