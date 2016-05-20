//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$Precipitation::Default_["numDrops"] = "4096";
$Precipitation::Default_["boxWidth"] = "400";
$Precipitation::Default_["boxHeight"] = "200";
$Precipitation::Default_["dropSize"] = "1";
$Precipitation::Default_["splashSize"] = "1";
$Precipitation::Default_["splashMS"] = "400";

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_PrecipitationManager::initData( %this ) {
	%objList = Lab.getMissionObjectClassList("Precipitation");
	SEP_PrecipitationMenu.clear();

	foreach$(%obj in %objList) {
		SEP_PrecipitationMenu.add(%obj.getName(),%obj.getId());
	}

	SEP_PrecipitationMenu.setSelected(getWord(%objList,0));
	SEP_PrecipitationDataMenu.clear();
	%dataList = Lab.getDatablockClassList("PrecipitationData");

	foreach$(%data in %dataList) {
		SEP_PrecipitationDataMenu.add(%data.getName(),%data.getId());
	}	

	%firstData = getWord(%dataList,0);
	if (isObject(%firstData))
	{
		SEP_PrecipitationDataMenu.setSelected(%firstData);
		%this.defaultDatablock = %firstData.getName();
	}
	SEP_SoundProfileMenu.clear();
	%dataList = Lab.getDatablockClassList("SFXProfile","description>>AudioLoop2d");

	foreach$(%data in %dataList) {
		SEP_SoundProfileMenu.add(%data.getName(),%data.getId());
	}

	SEP_DropShaderMenu.clear();
	SEP_DropShaderMenu.add("None",0);
	SEP_DropShaderMenu.setSelected(0);
	SEP_SplashShaderMenu.clear();
	SEP_SplashShaderMenu.add("None",0);
	SEP_SplashShaderMenu.setSelected(0);
}
//------------------------------------------------------------------------------
//==============================================================================
// PRECIPITATION OBJECT CREATOR
//==============================================================================
//==============================================================================

function SEP_PrecipitationManager::createPrecipitation(%this) {
	logd("SEP_PrecipitationManager::createPrecipitation(%this)",%this);
	%name = getUniqueName("envPrecipitation");
	%precipitation = new Precipitation(%name) {
		numDrops = $Precipitation::Default_["numDrops"];
		boxWidth = $Precipitation::Default_["boxWidth"];
		boxHeight = $Precipitation::Default_["boxHeight"];
		dropSize = $Precipitation::Default_["dropSize"];
		splashSize = $Precipitation::Default_["splashSize"];
		splashMS = $Precipitation::Default_["splashMS"];
		dataBlock = %this.defaultDatablock;
	};
	%precipitation.setFilename(MissionGroup.getFilename());
	%group = Scene.getActiveSimGroup();
	%group.add(%precipitation);
	SEP_PrecipitationMenu.add(%name,%precipitation.getId());
	SEP_PrecipitationMenu.setSelected(%precipitation.getId());
}
//------------------------------------------------------------------------------
function SEP_PrecipitationManager::deletePrecipitation(%this) {
	logd("SEP_PrecipitationManager::deletePrecipitation(%this)",%this);
	delObj(%this.selectedPrecipitation);
	%this.initData();
}
//------------------------------------------------------------------------------
//==============================================================================
// PRECIPITATION OBJECT MANAGEMENT
//==============================================================================
%f1 = "boxWidth boxHeight fadeDist fadeDistEnd useTrueBillboards useLighting glowIntensity reflect";
%f2 = "numDrops dropSize splashSize splashMS animateSplashes dropAnimateMS minSpeed maxSpeed minMass maxMass";
%f3 = "useWind useTurbulence maxTurbulence turbulenceSpeed rotateWithCamVel doCollision hitPlayers hitVehicles followCam";
SEP_PrecipitationManager.fieldList = %f1 SPC %f2 SPC %f3;

//==============================================================================
function SEP_PrecipitationMenu::OnSelect(%this,%id,%text) {
	logd("SEP_PrecipitationMenu::OnSelect(%this,%id,%text)",%this,%id,%text);
	SEP_PrecipitationManager.selectPrecipitation(%id);
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_PrecipitationManager::selectPrecipitation(%this,%objId) {
	logd("SEP_PrecipitationManager::selectPrecipitation(%this,%objId)",%this,%objId);
	%this.selectedPrecipitation = %objId;
	%name = %objId.getName();

	if (%name $= "")
		%name = "Precipitation \c2-\c1 " @ %objId.getId();

	%this.selectedPrecipitationName = %name;
	//PrecipitationInspector.inspect(%objId);
	LabObj.inspect(%objId);
	//Lab.inspect(%objId);
	%this.setDirty();
	%datablock = %objId.dataBlock;
	SEP_PrecipitationDataMenu.setSelected(%datablock.getId());
	%this-->activePrecipitationTitle.text = "Precipitation ->\c2" SPC %name;

	foreach$(%field in SEP_PrecipitationManager.fieldList) {
		%ctrl = SEP_PrecipitationProperties.findObjectByInternalName(%field,true);

		if (!isObject(%ctrl)) {
			warnLog("No Gui Control found for setting:",%field);
			continue;
		}

		%value = %objId.getFieldValue(%field);
		%ctrl.setTypeValue(%value);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_PrecipitationManager::saveObject(%this) {
	logd("SEP_PrecipitationManager::saveObject(%this)",%this);
	%obj = %this.selectedPrecipitation;

	if (!isObject(%obj)) {
		warnLog("Can't save precipitation because none is selected. Tried wth:",%obj);
		return;
	}

	LabObj.save(%obj);
	%this.setDirty();
	return;

	if (!SEP_AmbientManager_PM.isDirty(%obj)) {
		warnLog("Object is not dirty, nothing to save");
		return;
	}

	//SEP_AmbientManager_PM.setDirty(%obj);
	SEP_AmbientManager_PM.saveDirtyObject(%obj);
	%this.setDirty(false);
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_PrecipitationManager::setDirty(%this,%isDirty) {
	logd("SEP_PrecipitationManager::setDirty(%this,%isDirty)",%this,%isDirty);
	%obj = %this.selectedPrecipitation;

	if (%isDirty !$= "") {
		LabObj.setDirty(%obj,%isDirty);
	}

	%isDirty = LabObj.isDirty(%obj);
	%this.isDirty = %isDirty;
	SEP_PrecipitationSaveButton.active = %isDirty;
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_PrecipitationManager::updateFieldValue(%this,%field,%value) {
	logd("SEP_PrecipitationManager::updateFieldValue(%this,%field,%value)",%this,%field,%value);
	%obj = %this.selectedPrecipitation;

	if (!isObject(%obj)) {
		warnLog("Can't update ground cover value because none is selected. Tried wth:",%obj);
		return;
	}

	%currentValue = %obj.getFieldValue(%field);

	if (%currentValue $= %value) {
		return;
	}

	LabObj.set(%obj,%field,%value);
	//PrecipitationInspector.apply();
	//eval("%obj."@%checkField@" = %value;");
	//%obj.setFieldValue(%field,%value);
	EWorldEditor.isDirty = true;
	%this.setDirty(true);
}
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_PrecipitationEdit::onValidate( %this ) {
	logd("SEP_PrecipitationEdit::onValidate( %this )",%this.internalName,"LayerId=",%this.layerId);
	SEP_PrecipitationManager.updateFieldValue(%this.internalName,%this.getValue());
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_PrecipitationCheck::onClick( %this ) {
	logd("SEP_PrecipitationCheck::onClick( %this )",%this.internalName);
}
//------------------------------------------------------------------------------

//==============================================================================
// PRECIPITATION DATABLOCK MANAGEMENT
//==============================================================================
%f1 = "soundProfile dropTexture dropsPerSide dropShader splashTexture splashesPerSide splashShader";

SEP_PrecipitationManager.dataFieldList = %f1;

//==============================================================================
function SEP_PrecipitationManager::saveDatablock(%this) {
	logd("SEP_PrecipitationManager::updateDataFieldValue(%this,%field,%value)",%this,%field,%value);
	%data = %this.selectedPrecipitationData;
	LabObj.save(%data);
	%this.setDataDirty();
	return;

	if ( !SEP_AmbientManager_PM.isDirty(%data)) {
		warnLog("Datablock is not dirty");
		return;
	}

	SEP_AmbientManager_PM.saveDirtyObject( %data );
	%this.setDataDirty(false);
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_PrecipitationManager::updateDataFieldValue(%this,%field,%value) {
	logd("SEP_PrecipitationManager::updateDataFieldValue(%this,%field,%value)",%this,%field,%value);
	%data = %this.selectedPrecipitationData;

	if (!isObject(%data)) {
		warnLog("Can't update ground cover value because none is selected. Tried wth:",%data);
		return;
	}

	%currentValue = %data.getFieldValue(%field);

	if (%currentValue $= %value) {
		return;
	}

	LabObj.set(%data,%field,%value);
	%this.setDataDirty();
	%ctrl = SEP_PrecipitationDataProperties.findObjectByInternalName(%field,true);

	if (isObject(%ctrl))
		%ctrl.setTypeValue(%value);

	//PrecipitationDataInspector.apply();
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_PrecipitationManager::setDataDirty(%this,%isDirty) {
	logd("SEP_PrecipitationManager::setDataDirty(%this,%isDirty)",%this,%isDirty);
	%data = %this.selectedPrecipitationData;

	if (%isDirty !$= "") {
		LabObj.setDirty(%data,%isDirty);
	}

	%isDirty = LabObj.isDirty(%data);
	%this.dataIsDirty = %isDirty;
	SEP_PrecipitationDataSaveButton.active = %isDirty;
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_PrecipitationDataMenu::OnSelect(%this,%id,%text) {
	logd("SEP_PrecipitationDataMenu::OnSelect(%this,%id,%text)",%this,%id,%text);
	SEP_PrecipitationManager.selectPrecipitationData(%id);
}
//------------------------------------------------------------------------------

//==============================================================================
function SEP_PrecipitationManager::selectPrecipitationData(%this,%dataId) {
	logd("SEP_PrecipitationManager::selectPrecipitation(%this,%dataId)",%this,%dataId);
	%this.selectedPrecipitationData = %dataId;
	%this.selectedPrecipitationDataName = %dataId.getName();
	//%this.setDataDirty();
	//PrecipitationDataInspector.inspect(%dataId);
	LabObj.inspect(%dataId);
	%this.setDataDirty();

	foreach$(%field in SEP_PrecipitationManager.dataFieldList) {
		%ctrl = SEP_PrecipitationDataProperties.findObjectByInternalName(%field,true);

		if (!isObject(%ctrl)) {
			warnLog("No Gui Control found for setting:",%field);
			continue;
		}

		%value = %dataId.getFieldValue(%field);
		%ctrl.setTypeValue(%value);
	}

	%this.updateFieldValue("data"@"Block",%this.selectedPrecipitationDataName);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_PrecipitationDataEdit::onValidate( %this ) {
	logd("SEP_PrecipitationDataEdit::onValidate( %this )",%this.internalName,"LayerId=",%this.layerId);
	SEP_PrecipitationManager.updateDataFieldValue(%this.internalName,%this.getValue());
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_SoundProfileMenu::OnSelect(%this,%id,%text) {
	logd("SEP_SoundProfileMenu::OnSelect(%this,%id,%text)",%this,%id,%text);
	SEP_PrecipitationManager.updateDataFieldValue("soundProfile",%text);
}
//------------------------------------------------------------------------------

//==============================================================================
function SEP_DropShaderMenu::OnSelect(%this,%id,%text) {
	logd("SEP_DropShaderMenu::OnSelect(%this,%id,%text)",%this,%id,%text);
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_SplashShaderMenu::OnSelect(%this,%id,%text) {
	logd("SEP_SplashShaderMenu::OnSelect(%this,%id,%text)",%this,%id,%text);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_PrecipitationManager::selectDataFile( %this,%field ) {
	%current = %this.selectedPrecipitationData.getFieldValue(%field);
	%this.currentFileField = %field;
	getLoadFilename("*.*|*.*", "SEP_PrecipitationManager.applyDataFile", %current);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_PrecipitationManager::applyDataFile( %this,%file ) {
	%field = %this.currentFileField ;
	SEP_PrecipitationManager.updateDataFieldValue(%field,%file);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_PrecipitationBook::onTabSelected( %this,%text,%index ) {
	$SEP_PrecipitationBook_PageId = %index;
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_PrecipitationManager::toggleInspectorMode(%this) {
	logd("SEP_PrecipitationManager::toggleInspectorMode(%this)",%this);
	SEP_PrecipitationManager.inspectorMode = !SEP_PrecipitationManager.inspectorMode;

	if (SEP_PrecipitationManager.inspectorMode) {
		SEP_PrecipitationInspectButton.text = "Custom mode";
		SEP_Precipitation_Custom.visible = 0;
		SEP_Precipitation_Inspector.visible = 1;
	} else {
		SEP_PrecipitationInspectButton.text = "Inspector mode";
		SEP_Precipitation_Inspector.visible = 0;
		SEP_Precipitation_Custom.visible = 1;
	}
}
//------------------------------------------------------------------------------
/*
General:
	dataBlock = "HeavyRain";
  	boxWidth = "200";
	boxHeight = "100";
Misc:

	rotateWithCamVel = "1";
	doCollision = "1";
	hitPlayers = "0";
	hitVehicles = "0";
	followCam = "1";

Rendering:
   fadeDist = "0";
	fadeDistEnd = "0";
	useTrueBillboards = "0";
	useLighting = "0";
	glowIntensity = "0 0 0 0";
	reflect = "0";;

Drops:
 	numDrops = "8096";
	dropSize = "0.6";
	splashSize = "0.3";
	splashMS = "250";
	animateSplashes = "1";
	dropAnimateMS = "0";
		minSpeed = "1.5";
	maxSpeed = "2";
	minMass = "0.75";
	maxMass = "0.85";

Wind:
 	useWind = "0";

	useTurbulence = "0";
	maxTurbulence = "0.1";
	turbulenceSpeed = "0.2";

DATABLOCK
	soundProfile = "Sound_RainscapeA";
	dropTexture = "art/textures/Environment/Rain/DropletB_2_4";
	dropsPerSide = "4";
	//dropShader = "";
	splashTexture = "art/textures/Environment/Rain/Splash_RainBase";
	splashesPerSide = "1";
	//splashShader = "";
   */