//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_ScatterSkyManager.buildParams();
function SEP_AmbientManager::buildCloudLayerParams( %this ) {
	%arCfg = Lab.createBaseParamsArray("SEP_CloudLayer",SEP_CloudLayer);
	%arCfg.updateFunc = "SEP_AmbientManager.updateCloudLayerParam";
	%arCfg.style = "StyleA";
	%arCfg.useNewSystem = true;
	%arCfg.noDirectSync = true;
	%arCfg.group[%gid++] = "CloudLayer Overall" TAB "Stack StackA";
	%arCfg.setVal("texture",       "" TAB "texture" TAB "FileSelect" TAB "callback>>SEP_AmbientManager.getCloudLayerTexture(0);" TAB "SEP_AmbientManager.selectedCloudLayer" TAB %gid);
	%arCfg.setVal("baseColor",       "" TAB "baseColor" TAB "ColorSlider" TAB "" TAB "SEP_AmbientManager.selectedCloudLayer" TAB %gid);
	%arCfg.setVal("exposure",       "" TAB "exposure" TAB "SliderEdit" TAB "range>>0 10" TAB "SEP_AmbientManager.selectedCloudLayer" TAB %gid);
	%arCfg.setVal("coverage",       "" TAB "coverage" TAB "SliderEdit" TAB "range>>0 10" TAB "SEP_AmbientManager.selectedCloudLayer" TAB %gid);
	%arCfg.setVal("windSpeed",       "" TAB "windSpeed" TAB "SliderEdit" TAB "range>>0 10" TAB "SEP_AmbientManager.selectedCloudLayer" TAB %gid);
	%arCfg.setVal("height",       "" TAB "height" TAB "SliderEdit" TAB "range>>0 10" TAB "SEP_AmbientManager.selectedCloudLayer" TAB %gid);
	%arCfg.group[%gid++] = "Cloud Layer #1" TAB "Stack StackB";
	%arCfg.setVal("texScale[0]",       "" TAB "texScale" TAB "SliderEdit" TAB "range>>0 10" TAB "SEP_AmbientManager.selectedCloudLayer" TAB %gid);
	%arCfg.setVal("texDirection[0]",       "" TAB "texDirection" TAB "TextEdit" TAB "" TAB "SEP_AmbientManager.selectedCloudLayer" TAB %gid);
	%arCfg.setVal("texSpeed[0]",       "" TAB "texSpeed" TAB "SliderEdit" TAB "range>>0 1" TAB "SEP_AmbientManager.selectedCloudLayer" TAB %gid);
	%arCfg.group[%gid++] = "Cloud Layer #2" TAB "Stack StackB";
	%arCfg.setVal("texScale[1]",       "" TAB "texScale" TAB "SliderEdit" TAB "range>>0 10" TAB "SEP_AmbientManager.selectedCloudLayer" TAB %gid);
	%arCfg.setVal("texDirection[1]",       "" TAB "texDirection" TAB "TextEdit" TAB "" TAB "SEP_AmbientManager.selectedCloudLayer" TAB %gid);
	%arCfg.setVal("texSpeed[1]",       "" TAB "texSpeed" TAB "SliderEdit" TAB "range>>0 1" TAB "SEP_AmbientManager.selectedCloudLayer" TAB %gid);
	%arCfg.group[%gid++] = "Cloud Layer #3" TAB "Stack StackB";
	%arCfg.setVal("texScale[2]",       "" TAB "texScale" TAB "SliderEdit" TAB "range>>0 10" TAB "SEP_AmbientManager.selectedCloudLayer" TAB %gid);
	%arCfg.setVal("texDirection[2]",       "" TAB "texDirection"  TAB "TextEdit" TAB "" TAB "SEP_AmbientManager.selectedCloudLayer" TAB %gid);
	%arCfg.setVal("texSpeed[2]",       "" TAB "texSpeed" TAB "SliderEdit" TAB "range>>0 1" TAB "SEP_AmbientManager.selectedCloudLayer" TAB %gid);
	buildParamsArray(%arCfg,false);
	%this.CloudLayerParamArray = %arCfg;
}
//------------------------------------------------------------------------------

//==============================================================================
function SEP_AmbientManager::updateCloudLayerParam(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3) {
	logd("SEP_AmbientManager::updateCloudLayerParam(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3)",%this,%field,%value,%ctrl,%arg1,%arg2,%arg3);
	%fieldData = strreplace(%field,"["," ");
	%fieldData = strreplace(%fieldData,"]","");
	%this.updateCloudLayerField(getWord(%fieldData,0),%value, getWord(%fieldData,1));
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::initCloudLayerData( %this ) {
	SEP_AmbientManager.buildCloudLayerParams();
	%CloudLayerList = Lab.getMissionObjectClassList("CloudLayer");
	%first = getWord(%CloudLayerList,0);

	if (isObject(%first))
		%selected = %first.getId();

	if (isObject(%this.selectedCloudLayer))
		%currentId = %this.selectedCloudLayer.getId();

	SEP_CloudLayerMenu.clear();
	SEP_CloudLayerMenu.add("None",0);

	foreach$(%layer in %CloudLayerList) {
		SEP_CloudLayerMenu.add(%layer.getName(),%layer.getId());

		if (%currentId $= %layer.getId())
			%selected = %layer.getId();
	}

	if (%selected $= "")
		%selected = 0;

	SEP_CloudLayerMenu.setSelected(%selected);
	//%this.selectCloudLayer(getWord(%CloudLayerList,0));
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the current profile values into the params objects
function SEP_AmbientManager::getCloudLayerTexture( %this,%layer ) {
	%this.currentCloudLayer = %layer;
	%currentFile = $GLab_SelectedObject.bitmap;
	getLoadFilename("*.*|*.*", "SEP_AmbientManager.setCloudLayerTexture", %currentFile);
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the current profile values into the params objects
function SEP_AmbientManager::setCloudLayerTexture( %this,%file ) {
	%filename = makeRelativePath( %file, getMainDotCsDir() );
	%layer = %this.currentCloudLayer;
	%this.updateCloudLayerField("texture",%filename,%layer);
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the current profile values into the params objects
function SEP_AmbientManager::updateCloudLayerField( %this,%field, %value,%layerId ) {
	logd("SEP_AmbientManager::updateCloudLayerField( %this,%field, %value,%layerId )",%this,%field, %value,%layerId );
	%obj = %this.selectedCloudLayer;

	if (!isObject(%obj)) {
		warnLog("Can't update ground cover value because none is selected. Tried wth:",%obj);
		return;
	}

	LabObj.set(%obj,%field,%value,%layerId);
	//eval("%obj."@%checkField@" = %value;");
	//%obj.setFieldValue(%field,%value,%layerId);
	%this.setCloudLayerDirty();
	//CloudLayerInspector.refresh();
	//CloudLayerInspector.apply();
	syncParamArray(SEP_AmbientManager.CloudLayerParamArray);
}
//------------------------------------------------------------------------------

//==============================================================================
function SEP_AmbientManager::selectCloudLayer(%this,%obj) {
	logd("SEP_AmbientManager::selectCloudLayer(%this,%obj)",%this,%obj);

	if (!isObject(%obj)) {
		%this.selectedCloudLayer = "";
		%this.selectedCloudLayerName = "";
		syncParamArray(%this.CloudLayerParamArray);
		return;
	}

	%this.selectedCloudLayer = %obj;
	%this.selectedCloudLayerName = %obj.getName();
	%this.setCloudLayerDirty();
	LabObj.inspect(%obj);
	//CloudLayerInspector.inspect(	%obj);
	syncParamArray(%this.CloudLayerParamArray);
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_AmbientManager::saveCloudLayerObject(%this) {
	logd("SEP_AmbientManager::saveCloudLayerObject(%this)",%this);
	%obj = %this.selectedCloudLayer;

	if (!isObject(%obj)) {
		warnLog("Can't save CloudLayer because none is selected. Tried wth:",%obj);
		return;
	}

	LabObj.save(%obj);
	%this.setCloudLayerDirty(false);
	return;

	if (!SEP_AmbientManager_PM.isDirty(%obj)) {
		warnLog("Object is not dirty, nothing to save");
		return;
	}

	//SEP_AmbientManager_PM.setDirty(%obj);
	//SEP_AmbientManager_PM.saveDirtyObject(%obj);
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_AmbientManager::setCloudLayerDirty(%this,%isDirty) {
	logd("SEP_AmbientManager::setCloudLayerDirty(%this,%isDirty)",%this,%isDirty);
	%obj = %this.selectedCloudLayer;
	%labIsDirty = LabObj.isDirty(%obj);

	if (%isDirty !$= "") {
		if (%isDirty && !%labIsDirty) {
			warnLog("Trying to set CloudLayer different dirty state");
			LabObj.setDirty(%obj,%isDirty);
		}

		%labIsDirty = LabObj.isDirty(%obj);
	}

	if (%labIsDirty)
		EWorldEditor.isDirty = true;

	%this.isDirty = %labIsDirty;
	SEP_CloudLayerSaveButton.active = %labIsDirty;
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_AmbientManager::deleteCloudLayer(%this) {
	delObj(%this.selectedCloudLayer);
	%this.initCloudLayerData();
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_AmbientManager::createCloudLayer(%this) {
	logd("SEP_AmbientManager::setCloudLayerDirty(%this)",%this);
	%obj = %this.selectedCloudLayer;
	%name = getUniqueName("envCloudLayer");
	%obj = new CloudLayer(%name) {
		texture = "art/gfx/skies/clouds/cloud3.png";
		texScale[0] = "1";
		texScale[1] = "1";
		texScale[2] = "1";
		texDirection[0] = "1 0";
		texDirection[1] = "0 1";
		texDirection[2] = "0.5 0";
		texSpeed[0] = "0.005";
		texSpeed[1] = "0.005";
		texSpeed[2] = "0.005";
		baseColor = "0.345098 0.345098 0.345098 1";
		exposure = "4";
		coverage = "1";
		windSpeed = "3";
		height = "6";
	};
	%group = Scene.getActiveSimGroup();
	%group.add(%obj);
	%obj.setFileName(MissionGroup.getFileName());
	SEP_CloudLayerMenu.add(%obj.getName(),%obj.getId());
	SEP_CloudLayerMenu.setSelected(%obj.getId());
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_CloudLayerMenu::onSelect(%this,%id,%text) {
	SEP_AmbientManager.selectCloudLayer(%text);
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
