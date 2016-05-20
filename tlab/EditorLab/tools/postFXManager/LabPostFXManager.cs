//==============================================================================
// TorqueLab -> Lab PostFX Manager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
$EPostFxManager::PostFxTypes = "SSAO HDR LightRay DOF Vignette CA VolFogGlow";
function EPostFxManager::onWake(%this) {
	//Check if the UI is all there (Might be moved and forgotten)
	%this.onShow();
}
//------------------------------------------------------------------------------
//==============================================================================
// New function to avoid initial onShow/onWake call for init
function EPostFxManager::toggleState(%this) {
	if (%this.isVisible())
	{
		hide(%this);
		return;
	}
	
	
	//Check if the UI is all there (Might be moved and forgotten)
	if (!isObject(%this-->MainContainer)) {
		EPostFxManager.add(EPostFxManager_Main);
		EPostFxManager.schedule(1000,"init","1");
		return;
	}

	if (!Lab.postFxInitialized)
		EPostFxManager.init();

	$EPostFxManagerActive = true;

	if (isObject(EditorGuiToolbarStack-->PostFXManager))
		EditorGuiToolbarStack-->PostFXManager.setStateOn(true);
		
	show(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
function EPostFxManager::onShow(%this) {
	if( !LabEditor.isInitialized )
	{
		warnLog("Premature onWake for EPostFxManager, wait for LabEditor initialized at least...");
		return;
	}
	warnLog("EPostFxManager::onShow does nothing nothing since it's called from somewhere which should not.");
}
//------------------------------------------------------------------------------
//==============================================================================
function EPostFxManager::onHide(%this) {
	$EPostFxManagerActive = false;

	if (isObject(EditorGuiToolbarStack-->PostFXManager))
		EditorGuiToolbarStack-->PostFXManager.setStateOn(false);
}
//------------------------------------------------------------------------------

//==============================================================================
function EPostFxManager::onSleep(%this) {
	if (isObject(TMG_GroundCoverClone-->MainContainer))
		SEP_GroundCover.add(TMG_GroundCoverClone-->MainContainer);
}
//------------------------------------------------------------------------------
//EPostFxManager.init(true);
//==============================================================================
function EPostFxManager::init(%this,%forceInit,%notInitialized) {
	//devLog("EPostFxManager::init(%this,%forceInit,%notInitialized)",%forceInit,%notInitialized);
	if (Lab.postFxInitialized && !%forceInit)
		return;

	EPostFxMiscStack.clear();
	%this.buildParamsHDR();
	%this.buildParamsSSAO();
	%this.buildParamsLightRays();
	%this.buildParamsDOF();
	%this.buildParamsVignette();
	%this.buildParamsCA();
	%this.buildParamsVolFogGlow();
	EPostFxManager.initPresets();
	Lab.postFxInitialized = true;
}
//------------------------------------------------------------------------------
//==============================================================================
function EPostFxManager::moveToGui(%this,%gui) {
	%gui.add(EPostFxManager_Main);
	hide(EPostFxManager);
	//Schedule a new init to adapt the params to new host stack
	EPostFxManager.schedule(1000,"init","1");
}
//------------------------------------------------------------------------------
//==============================================================================
function EPostFxManager::moveFromGui(%this,%gui) {
	%this.add(EPostFxManager_Main);
	EPostFxManager.schedule(1000,"init");
}
//------------------------------------------------------------------------------
//==============================================================================
function EPostFxManager::onPreEditorSave(%this) {
	%this-->StackBloom.clear();
	%this-->StackBrightness.clear();
	EPostFxPage_SSAOStack.clear();
}
//------------------------------------------------------------------------------
//==============================================================================
function EPostFxManager::onPostEditorSave(%this) {
	EPostFxManager.init();
}
//------------------------------------------------------------------------------


$PostFXList = "SSAOPostFx HDRPostFX LightRayPostFX DOFPostEffect VignettePostEffect VolFogGlowPostFx";
function skipPostFx(%skip,%list)
{
	if (%skip $= "")
		%skip = true;
	if (%list $="")
		%list = $PostFXList;
	foreach$(%postFx in %list)
		%postFx.skip = %skip;
}