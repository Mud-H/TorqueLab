//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$BasicClouds::Default_["texture0"] = "art/gfx/skies/clouds/cloud1.png";
$BasicClouds::Default_["texture1"] = "art/gfx/skies/clouds/cloud2.png";
$BasicClouds::Default_["texture2"] = "art/gfx/skies/clouds/cloud3.png";
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_AmbientManager.buildBasicCloudsParams();
function SEP_AmbientManager::buildBasicCloudsParams( %this ) {
	%arCfg = Lab.createBaseParamsArray("SEP_BasicClouds",SEP_BasicClouds_StockParam);
	%arCfg.updateFunc = "SEP_AmbientManager.updateBasicCloudsParam";
	%arCfg.useNewSystem = true;
	//%arCfg.arrayOnly = true;
	//%arCfg.group[%gid++] = "Cloud Layer #1" TAB "Stack StackA";
	%arCfg.setVal("layerEnabled[0]",       "" TAB "Enabled" TAB "Checkbox" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texture[0]",       "" TAB "Texture" TAB "FileSelect" TAB "callback>>SEP_AmbientManager.getBasicCloudTexture(0);" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texScale[0]",       "" TAB "Layer scale" TAB "SliderEdit" TAB "range>>0 5" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texDirection[0]",       "" TAB "Layer direction" TAB "TextEdit" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texSpeed[0]",       "" TAB "Layer speed" TAB "SliderEdit" TAB "range>>0 5" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texOffset[0]",       "" TAB "Layer offset" TAB "TextEdit" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("height[0]",       "" TAB "Layer heigth0" TAB "SliderEdit" TAB "range>>0 10;;tickAt 1" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	//%arCfg.group[%gid++] = "Cloud Layer #2" TAB "Stack StackA";
	%arCfg.setVal("layerEnabled[1]",       "" TAB "Enabled" TAB "Checkbox" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texture[1]",       "" TAB "Texture" TAB "FileSelect" TAB "callback>>SEP_AmbientManager.getBasicCloudTexture(1);" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texScale[1]",       "" TAB "Layer scale" TAB "SliderEdit" TAB "range>>0 5" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texDirection[1]",       "" TAB "Layer direction" TAB "TextEdit" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texSpeed[1]",       "" TAB "Layer speed" TAB "SliderEdit" TAB "range>>0 5" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texOffset[1]",       "" TAB "Layer offset" TAB "TextEdit" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("height[1]",       "" TAB "Layer heigth1" TAB "SliderEdit" TAB "range>>0 10;;tickAt 1" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	//%arCfg.group[%gid++] = "Cloud Layer #3" TAB "Stack StackA";
	%arCfg.setVal("layerEnabled[2]",       "" TAB "Enabled" TAB "Checkbox" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texture[2]",       "" TAB "Texture" TAB "FileSelect" TAB "callback>>SEP_AmbientManager.getBasicCloudTexture(2);" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texScale[2]",       "" TAB "Layer scale" TAB "SliderEdit" TAB "range>>0 5" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texDirection[2]",       "" TAB "Layer direction" TAB "TextEdit" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texSpeed[2]",       "" TAB "Layer speed" TAB "SliderEdit" TAB "range>>0 5" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texOffset[2]",       "" TAB "Layer offset" TAB "TextEdit" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("height[2]",       "" TAB "Layer heigth2" TAB "SliderEdit" TAB "range>>0 10;;tickAt 1" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	//buildParamsArray(%arCfg,false);
	%this.BasicCloudsParamArray = %arCfg;
}
//------------------------------------------------------------------------------

//==============================================================================
function SEP_AmbientManager::updateBasicCloudsParam(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3) {
	logd("SEP_AmbientManager::updateBasicCloudsParam(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3)",%this,%field,%value,%ctrl,%arg1,%arg2,%arg3);
	%fieldData = strreplace(%field,"["," ");
	%fieldData = strreplace(%fieldData,"]","");
	%this.updateBasicCloudField(getWord(%fieldData,0),%value, getWord(%fieldData,1));
}
//------------------------------------------------------------------------------


//==============================================================================
// Sync the current profile values into the params objects
function SEP_AmbientManager::getBasicCloudTexture( %this,%layer ) {
	/*if(Canvas.isCursorOn())
	{
		devLog("Cursor is on");
		Canvas.cursorOff();
	//	TLabGameGui.noCursor = 0;
	}*/
	%this.currentBasicClouds = %layer;
	%currentFile = $GLab_SelectedObject.bitmap;
	//Canvas.cursorOff();
	getLoadFilename("*.*|*.*", "SEP_AmbientManager.setBasicCloudTexture", %currentFile);
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the current profile values into the params objects
function SEP_AmbientManager::setBasicCloudTexture( %this,%file ) {
	%filename = makeRelativePath( %file, getMainDotCsDir() );
	%layer = %this.currentBasicClouds;
	%this.updateBasicCloudField("texture",%filename,%layer);
	
	%textEdit = SEP_BasicClouds_StockParam.findObjectByInternalName("texture["@%layer@"]",true);
	%textEdit.setText(%filename);
	
	
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the current profile values into the params objects
function SEP_AmbientManager::updateBasicCloudField( %this,%field, %value,%layerId ) {
	devLog("SEP_AmbientManager::updateBasicCloudField( %this,%field, %value,%layerId )",%this,%field, %value,%layerId );
	//SEP_AmbientManager.buildBasicCloudsParams();
	%obj = %this.selectedBasicClouds;

	if (!isObject(%obj)) {
		warnLog("Can't update ground cover value because none is selected. Tried wth:",%obj);
		return;
	}

	LabObj.set(%obj,%field,%value,%layerId);
	//eval("%obj."@%checkField@" = %value;");
	//%obj.setFieldValue(%field,%value,%layerId);
	EWorldEditor.isDirty = true;
	%this.setBasicCloudsDirty();
	//BasicCloudsInspector.refresh();
	//BasicCloudsInspector.apply();
	syncParamArray(SEP_AmbientManager.BasicCloudsParamArray);
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor PluginSEP_AmbientManager.initBasicCloudsData
function SEP_AmbientManager::initBasicCloudsData( %this ) {
	%basicCloudsList = Lab.getMissionObjectClassList("BasicClouds");
	%first = getWord(%basicCloudsList,0);

	if (isObject(%first))
		%selected = %first.getId();

	%currentId = "-1";

	if (isObject(%this.selectedBasicClouds))
		%currentId = %this.selectedBasicClouds.getId();

	SEP_BasicCloudsMenu.clear();
	SEP_BasicCloudsMenu.add("None",0);

	foreach$(%layer in %basicCloudsList) {
		%added = true;
		SEP_BasicCloudsMenu.add(%layer.getName(),%layer.getId());

		if (%currentId $= %layer.getId())
			%selected = %layer.getId();
	}

	if (%selected $= "")
		%selected = 0;

	SEP_BasicCloudsMenu.setSelected(%selected);
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_AmbientManager::selectBasicClouds(%this,%obj) {
	logd("SEP_AmbientManager::selectBasicClouds(%this,%obj)",%this,%obj);

	if (isObject(%obj)) {
		%name = %obj.getName();
		%id = %obj;
		%inspectThis = %obj;
		Lab.inspect(%inspectThis);
	}

	SEP_AmbientManager.selectedBasicClouds = %id;
	%this.selectedBasicCloudsName = %name;
	%this.setBasicCloudsDirty();
	//BasicCloudsInspector.inspect(	%inspectThis);
	syncParamArray(SEP_AmbientManager.BasicCloudsParamArray,true);
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_AmbientManager::saveBasicCloudsObject(%this) {
	devLog("SEP_AmbientManager::saveBasicCloudsObject(%this)",%this);
	%obj = %this.selectedBasicClouds;

	if (!isObject(%obj)) {
		warnLog("Can't save BasicClouds because none is selected. Tried wth:",%obj);
		return;
	}

	LabObj.save(%obj);
	%this.setCloudLayerDirty(false);
	return;

	if (SEP_AmbientManager.missionIsDirty) {
		SEP_AmbientManager_PM.setDirty(MissionGroup);
		SEP_AmbientManager_PM.saveDirtyObject(MissionGroup);
		SEP_AmbientManager.missionIsDirty = false;
		%this.setBasicCloudsDirty(false);
		return;
	}

	if (!SEP_AmbientManager_PM.isDirty(%obj)) {
		warnLog("Object is not dirty, nothing to save");
		return;
	}

	SEP_AmbientManager_PM.setDirty(%obj);
	SEP_AmbientManager_PM.saveDirtyObject(%obj);
	%this.setBasicCloudsDirty(false);
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_AmbientManager::setBasicCloudsDirty(%this) {
	logd("SEP_AmbientManager::setBasicCloudsDirty(%this)",%this);
	%obj = %this.selectedBasicClouds;
	%isDirty = LabObj.isDirty(%obj);
	%this.isDirty = %isDirty;
	SEP_BasicCloudsSaveButton.active = %isDirty;
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_AmbientManager::deleteBasicClouds(%this) {
	delObj(%this.selectedBasicClouds);
	//%this.selectBasicClouds();
	SEP_AmbientManager.initBasicCloudsData();
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_AmbientManager::createBasicClouds(%this) {
	logd("SEP_AmbientManager::createBasicClouds(%this)",%this);
	%obj = %this.selectedBasicClouds;
	%name = getUniqueName("envBasicClouds");
	%obj = new BasicClouds(%name) {
		layerEnabled[0] = "1";
		layerEnabled[1] = "1";
		layerEnabled[2] = "1";
		texture[0] = $BasicClouds::Default_["texture0"];
		texture[1] = $BasicClouds::Default_["texture1"];
		texture[2] = $BasicClouds::Default_["texture2"];
		texScale[0] = "1";
		texScale[1] = "0.857143";
		texScale[2] = "1";
		texDirection[0] = "1 0";
		texDirection[1] = "1 0";
		texDirection[2] = "1 0";
		texSpeed[0] = "0.001";
		texSpeed[1] = "0.450549";
		texSpeed[2] = "0.002";
		texOffset[0] = "0.5 0.5";
		texOffset[1] = "0.2 0.2";
		texOffset[2] = "0.2 0.2";
		height[0] = "8";
		height[1] = "5";
		height[2] = "3";
		byGroup = "0";
	};
	%group = Scene.getActiveSimGroup();
	%group.add(%obj);
	%obj.setFileName(MissionGroup.getFileName());
	SEP_BasicCloudsMenu.add(%obj.getName(),%obj.getId());
	SEP_BasicCloudsMenu.setSelected(%obj.getId());
	SEP_AmbientManager.missionIsDirty = true;
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_BasicCloudsMenu::onSelect(%this,%id,%text) {
	SEP_AmbientManager.selectBasicClouds(%text);
}
//------------------------------------------------------------------------------

//==============================================================================
function SEP_BasicClouds::toggleInspectorMode(%this) {
	logd("SEP_BasicClouds::toggleInspectorMode(%this)",%this);
	SEP_BasicClouds.inspectorMode = !SEP_BasicClouds.inspectorMode;

	if (SEP_BasicClouds.inspectorMode) {
		SEP_BasicCloudsInspectButton.text = "Custom mode";
		SEP_BasicClouds_Custom.visible = 0;
		SEP_BasicClouds_Inspector.visible = 1;
	} else {
		SEP_BasicCloudsInspectButton.text = "Inspector mode";
		SEP_BasicClouds_Inspector.visible = 0;
		SEP_BasicClouds_Custom.visible = 1;
	}
}
//------------------------------------------------------------------------------
//==============================================================================
/*
layerEnabled
texture
texScale
texDirection
texSpeed
texOffset
height

*/

