//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function EPostFxManager::syncAll(%this) {
	foreach$(%type in $EPostFxManager::PostFxTypes ) {
		syncParamArray("arEPostFx_"@%type@"Param");

		if (%this.isMethod("customSync"@%type))
			eval("%this.customSync"@%type@"();");
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function EPostFxManager::applyPostFXSettings(%this) {
	PostFXManager.settingsSetEnabled( $PostFXManager::PostFX::Enabled);	
	
	ppOptionsUpdateDOFSettings();
	EPostFxManager.syncAll();
}
//------------------------------------------------------------------------------
function EPostFxManager::settingsRefreshAll(%this) {
	PostFXManager.settingsRefreshAll();
	
	%this.syncAll();
}
//==============================================================================
function EPostFxManager_EnableCheck::onAction(%this) {
	PostFxManager.settingsSetEnabled(%this.isStateOn());
}
//------------------------------------------------------------------------------

/*
$PostFxList = "HDR DOF SSAO LightRays Vignette CA VolFogGlow";
$PostFxManagerList = "PostFX Settings SSAO LightRays Vignette CA VolFogGlow";
$PostFxManagerSettingList = "HDR DOF SSAO LightRays";



$PostFXManager = "defaultPreset fileExtension fileFilter forceEnableFromPresets vebose";

$PostFXManager_PostFX = "Enabled EnableDOF EnableHDR EnableLightRays EnableSSAO EnableVignette";
$PostFXManager_Settings = "ColorCorrectionRamp EnableDOF EnabledSSAO EnableHDR EnableLightRays EnablePostFX EnableSSAO EnableVignette";
$PostFXManager_Settings_DOF = "BlurCurveFar BlurCurveNear BlurMax BlurMin EnableAutoFocus EnableDOF FocusRangeMax FocusRangeMin";
$PostFXManager_Settings_HDR = "adaptRate blueShiftColor brightPassThreshold enableBloom enableBlueShift enableToneMapping gaussMean gaussMultiplier gaussStdDev keyValue minLuminace whiteCutoff";
$PostFXManager_Settings_LightRays = "brightScalar decay density numSamples weight";
$PostFXManager_Settings_SSAO = "blurDepthTol blurNormalTol lDepthMax lDepthMin lDepthPow lNormalPow lNormalTol lRadius lStrength overallStrength quality sDepthMax sDepthMin sDepthPow sNormalPow sNormalTol sRadius sStrength targetScale";
$PostFXManager_Settings_Vignette = "VMin VMax";
$PostFXManager_Settings_VolFogGlow = "glowStrength";
$PostFXManager_Settings_CA = "colorDistortionFactor cubeDistortionFactor enabled";

$EPostFx_SSAO_QualityList = "Low Medium High";
$EPostFx_DOF_QualityList = "Low Medium High";

$EPostFx_Fields_CA = "colorDistortionFactor cubeDistortionFactor enabled";
$EPostFx_Fields_DOF = "BlurCurveFar BlurCurveNear BlurMax BlurMin EnableAutoFocus FocusRangeMax FocusRangeMin";
$EPostFx_Fields_SSAO = "blurDepthTol blurNormalTol lDepthMax lDepthMin lDepthPow lNormalPow lNormalTol lRadius lStrength overallStrength quality sDepthMax sDepthMin sDepthPow sNormalPow sNormalTol sRadius sStrength targetScale";
$EPostFx_Fields_HDR = "adaptRate blueShiftColor brightPassThreshold colorCorrectionRamp enableBloom enableBlueShift enableToneMapping gaussMean gaussMultiplier gaussStdDev keyValue minLuminace whiteCutoff";
$EPostFx_Fields_LightRays = "brightScalar decay density exposure numSamples weight resolutionScale";
$EPostFx_Fields_VolFogGlow = "glowStrength";
$EPostFx_Fields_Vignette = "VMin VMax";
*/