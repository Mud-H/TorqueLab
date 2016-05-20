//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$SEP_AmbientBook_PageId = 0;
$SEP_CloudsBook_PageId = 0;

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::checkState( %this ) {
	logd("SEP_AmbientManager::checkState");
	if (!$SEP_AmbientManager_Loaded)	
		%this.initDialog();	
	
	return $SEP_AmbientManager_Loaded;
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::onShow( %this ) {
	logd("SEP_AmbientManager::onShow");
	if (!$SEP_AmbientManager_Loaded)
		hide(%this);
	SEP_SkySystemCreator-->newScatterSkyName.setText("envScatterSky");
	SEP_SkySystemCreator-->newSunName.setText("envSun");
	SEP_SkySystemCreator-->newSkyName.setText("envSkyBox");
	
	EPostFxManager.moveToGui(SEP_PostFXManager_Clone);
	hide(SEP_SkySystemCreator);
	SEP_AmbientManager.updateSkySystemData(true);

	if ($LabData_CubemapList $= "")
		$LabData_CubemapList = getRootClassList("CubemapData");
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::onHide( %this ) {
	EPostFxManager.moveFromGui();
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_AmbientManager::onPreEditorSave(%this) {
	logd("SEP_AmbientManager::onPreEditorSave");

	if (isObject(SEP_PostFXManager_Clone-->MainContainer))
		EPostFxManager.moveFromGui();
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_AmbientManager::onPostEditorSave(%this) {
	logd("SEP_AmbientManager::onPostEditorSave");
	EPostFxManager.moveToGui(SEP_PostFXManager_Clone);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::initDialog( %this ) {
	if ($SEP_AmbientManager_Loaded)
	{
		warnLog("Trying to init AmbientManager again");
		return;
	}
	SEP_AmbientBook.selectPage($SEP_AmbientBook_PageId);
	SEP_CloudsBook.selectPage($SEP_CloudsBook_PageId);
	SEP_ScatterSkySystemMenu.clear();
	SEP_ScatterSkySystemMenu.add("New Scatter Sky",0);
	SEP_ScatterSkySystemMenu.add("New Sun (+SkyBox)",1);
	SEP_ScatterSkySystemMenu.setSelected(0);
	%basicCloudsList = Lab.getMissionObjectClassList("BasicClouds");
	SEP_AmbientManager.buildBasicCloudsParams();
	SEP_AmbientManager.initBasicCloudsData();
	SEP_AmbientManager.buildCloudLayerParams();
	SEP_AmbientManager.initCloudLayerData();
	SEP_ScatterSkyManager.initData();
	
	if (!isObject(SEP_AmbientManager_PM))
		new PersistenceManager(SEP_AmbientManager_PM);

	SEP_AmbientBook.selectPage($SEP_AmbientBook_PageId);

	if (!isObject(MissionGroup))
		return;

	%this.getSkySystemObject();
	SEP_AmbientManager.initSkySystemData();
	$SEP_AmbientManager_Loaded = true;
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::onActivated( %this ) {
	logd("SEP_AmbientManager::onActivated(%this)");
	return; //WIP Fast - Only load whe needed
	
	SEP_AmbientBook.selectPage($SEP_AmbientBook_PageId);
	SEP_CloudsBook.selectPage($SEP_CloudsBook_PageId);
	SEP_ScatterSkySystemMenu.clear();
	SEP_ScatterSkySystemMenu.add("New Scatter Sky",0);
	SEP_ScatterSkySystemMenu.add("New Sun (+SkyBox)",1);
	SEP_ScatterSkySystemMenu.setSelected(0);
	%basicCloudsList = Lab.getMissionObjectClassList("BasicClouds");
	SEP_AmbientManager.buildBasicCloudsParams();
	SEP_AmbientManager.initBasicCloudsData();
	SEP_AmbientManager.buildCloudLayerParams();
	SEP_AmbientManager.initCloudLayerData();
	SEP_ScatterSkyManager.initData();
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::onWake( %this ) {
	SEP_PrecipitationManager.initData();
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::onSleep( %this ) {
	logd("SEP_AmbientManager::onSleep( %this )");
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientBook::onTabSelected( %this,%text,%index ) {
	logd("SEP_AmbientManager::onTabSelected( %this,%text,%index )");
	$SEP_AmbientBook_PageId = %index;
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_CloudsBook::onTabSelected( %this,%text,%index ) {
	$SEP_CloudsBook_PageId = %index;
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::setDirtyObject( %this,%obj,%isDirty ) {
	logd("SEP_AmbientManager::setDirtyObject( %this,%obj,%isDirty )");

	if (!isObject(%obj))
		return "-1";

	if (%isDirty !$= "") {
		LabObj.setDirty(%obj,%isDirty);
	}

	%objIsDirty = LabObj.isDirty(%obj);

	if (%objIsDirty) {
		EWorldEditor.isDirty = true;
		return "1";
	}

	return "0";
}
//------------------------------------------------------------------------------

