//==============================================================================
// TorqueLab -> SceneEditor Inspector script
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SceneEditorDialogs::onActivated( %this ) {
	if (SceneEditorDialogs.selectedPage $= "")
		SceneEditorDialogs.selectedPage = "0";

	
}
//------------------------------------------------------------------------------


//==============================================================================
// Hack to force LevelInfo update after Cubemap change...
//==============================================================================
//==============================================================================
function SceneEditorDialogs::onPreEditorSave( %this ) {
	%c1 = SEP_ScatterSky_Custom-->StackA SPC SEP_ScatterSky_Custom-->StackB;
	%c1 = %c1 SPC SEP_LegacySkyProperties-->StackA SPC SEP_LegacySkyProperties-->StackB;
	%c1 = %c1 SPC SEP_CloudLayer-->StackA SPC SEP_CloudLayer-->StackB;

	foreach$(%ctrl in %c1)
		%ctrl.clear();

	if (isObject(SEP_PostFXManager_Clone-->MainContainer))
		EPostFxManager.moveFromGui();

	SEP_GroundCoverLayerArray.clear();
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEditorDialogs::onPostEditorSave( %this ) {
	//EPostFxManager.moveToGui(SEP_PostFXManager_Clone);
}
//------------------------------------------------------------------------------