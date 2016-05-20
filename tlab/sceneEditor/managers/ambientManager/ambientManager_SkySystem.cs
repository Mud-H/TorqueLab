//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::initSkySystemData( %this ) {
	SEP_ScatterSkyManager.buildParams();
	SEP_LegacySkyManager.buildParams();
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_ScatterSkySystemMenu::OnSelect(%this,%id,%text) {
	logd("SEP_ScatterSkySystemMenu::OnSelect(%this,%id,%text)",%this,%id,%text);
	SEP_SkySystemCreator-->ScatterSky.visible = 0;
	SEP_SkySystemCreator-->Legacy.visible = 0;

	if (%id $= "0") {
		SEP_SkySystemCreator-->ScatterSky.visible = 1;
		SEP_AmbientManager.createSkyType = "ScatterSky";
	} else if (%id $= "1") {
		SEP_SkySystemCreator-->Legacy.visible = 1;
		SEP_AmbientManager.createSkyType = "Legacy";
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_SkySelectMenu::OnSelect(%this,%id,%text) {
	logd("SEP_SkySelectMenu::OnSelect(%this,%id,%obj)",%this,%id,%text);
	%obj = %id;

	if (!isObject(%obj))
		return;

	SEP_AmbientManager.hideAllSkyObjects(true);
	%obj.hidden = false;

	if (isObject(%obj.mySkyBox))
		%obj.mySkyBox.hidden = false;

	SEP_AmbientManager.getSkySystemObject();

	if (%obj.getClassName() $= "Sun")
		SEP_LegacySkyManager.selectSky(%obj);
	else if (%obj.getClassName() $= "ScatterSky")
		SEP_ScatterSkyManager.selectScatterSky(%obj);
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_AmbientManager::activateSkyObj(%this,%obj) {
	logd("SEP_AmbientManager::activateSkyObj(%this,%obj)",%this,%obj);

	if (isObject(%obj)) {
		//if (%text.getClassName() $= "ScatterSky")
		%hideSkyBox = true;

		if (%obj.getClassName() $= "Sun")
			SEP_LegacySkyManager.selectSky(%obj);
		else if (%obj.getClassName() $= "ScatterSky")
			SEP_ScatterSkyManager.selectScatterSky(%obj);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_AmbientManager.updateSkySystemData
function SEP_AmbientManager::updateSkySystemData( %this,%build ) {
	logd("SEP_AmbientManager::updateSkySystemData(%this)");

	if (%build) {
		SEP_ScatterSkyManager.buildParams();
		SEP_LegacySkyManager.buildParams();
	}

	SEP_AmbientManager.getSkySystemObject();
	SEP_SkySelectMenu.clear();
	%id = 0;
	%scatterSkyList = Lab.getMissionObjectClassList("ScatterSky");
	%sunList = Lab.getMissionObjectClassList("Sun");
	%list = %scatterSkyList SPC %sunList;

	foreach$(%obj in %list) {
		//if (!%obj.isBackup)
		//continue;
		if (%obj $= SEP_AmbientManager.skySystemObj)
			%select = %obj.getId();

		%name = %obj.getName();

		if (%name $= "")
			%name = %obj.getClassName()@"\c2-\c1"@%obj.getId();

		SEP_SkySelectMenu.add(%name,%obj.getId());

		if (%select $= "")
			%select = %obj.getId();

		%id++;
	}

	//if (%id $= "1")
	//	hide(AMD_SelectSkyContainer);
	//else
	//show(AMD_SelectSkyContainer);
	%update = false;

	if (!isObject(%this.selectedSunObj))
		%update = true;

	SEP_SkySelectMenu.setSelected(%select,%update);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::getSkySystemObject( %this ) {
	%this.skySystemObj = "";
	%this.scatterSkyObj = "";
	%this.sunObj = "";
	%this.skyBoxObj = "";
	SEP_SkySystem-->ScatterSky.visible = 0;
	SEP_SkySystem-->Lecacy.visible = 0;
	%scatterSkyList = Lab.getMissionObjectClassList("ScatterSky");

	foreach$(%obj in %scatterSkyList) {
		if (%obj.hidden)
			continue;

		if (isObject(%this.scatterSkyObj)) {
			warnLog("Multiple active ScatterSky detected which might cause unexpected result. The first object found is set as SkySystemObj:",%this.scatterSkyObj);
			continue;
		}

		%this.scatterSkyObj = %obj;
		%this.skySystem = "ScatterSky";
		%this.skySystemObj = %obj;
		SEP_SkySystem-->ScatterSky.visible = 1;
	}

	%sunList = Lab.getMissionObjectClassList("Sun");

	foreach$(%obj in %sunList) {
		if (%obj.hidden)
			continue;

		if (isObject(%this.scatterSkyObj)) {
			warnLog("There's already an active ScatterSky and an active sun as been found which cause conflict. The Sun has been disabled");
			%obj.hidden = true;
			continue;
		}

		if (isObject(%this.sunObj)) {
			warnLog("Multiple active suns detected which might cause unexpected result. The first object found is set as SkySystemObj:",%this.sunObj);
			continue;
		}

		%this.sunObj = %obj;
		%this.skySystem = "Sun";
		%this.skySystemObj = %obj;
		SEP_SkySystem-->Lecacy.visible = 1;
		%skyBoxList = Lab.getMissionObjectClassList("SkyBox");

		foreach$(%skyBox in %skyBoxList) {
			if (%skyBox.hidden)
				continue;

			if (isObject(%this.skyBoxObj)) {
				warnLog("Multiple active SkyBoxes detected which might cause unexpected result. The first object found is set as Main Skybox:",%this.skyBoxObj);
				continue;
			}

			%this.skyBoxObj = %skyBox;
			%this.sunObj.setFieldValue("mySkyBox",%skyBox);
		}
	}

	//%this.activateSkyObj(%this.skySystemObj);
	return %this.skySystemObj;
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_AmbientManager.createNewSkySystem();
function SEP_AmbientManager::createNewSkySystem( %this ) {
	logd("SEP_AmbientManager::createNewSkySystem(%this)");
	%type = SEP_AmbientManager.createSkyType;

	if (%type $= "ScatterSky")
		%this.createNewScatterSky();
	else if (%type $= "Legacy")
		%this.createNewLecacySky();

	//Rebuild Fog Params since they depend of type of sky system
	SEP_AmbientManager.updateSkySystemData();
	//%this.buildFogParams();
	hide(SEP_SkySystemCreator);
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::createNewScatterSky( %this ) {
	%backup = SEP_SkySystemCreator-->backupCurrentSystem.isStateOn();

	if (%backup)
		%this.backupCurrentSky();

	%this.clearSkySystem();
	%name = SEP_SkySystemCreator-->newScatterSkyName.getText();
	%unique = getUniqueName(%name);
	%skyObj =  new ScatterSky(%unique) {
		skyBrightness = "25";
		sunSize = "1";
		colorizeAmount = "0";
		colorize = "0 0 0 1";
		rayleighScattering = "0.0035";
		sunScale = "1 1 1 1";
		ambientScale = "1 1 1 1";
		fogScale = "1 1 1 1";
		exposure = "1";
		zOffset = "0";
		azimuth = "0";
		elevation = "35";
		moonAzimuth = "0";
		moonElevation = "45";
		castShadows = "1";
		brightness = "1";
		flareScale = "1";
		nightColor = "0.0196078 0.0117647 0.109804 1";
		nightFogColor = "0.0196078 0.0117647 0.109804 1";
		moonEnabled = "1";
		moonMat = "Moon_Glow_Mat";
		moonScale = "0.2";
		moonLightColor = "0.192157 0.192157 0.192157 1";
		useNightCubemap = "1";
		nightCubemap = "NightCubemap";
		attenuationRatio = "0 1 1";
		shadowType = "PSSM";
		texSize = "512";
		overDarkFactor = "2000 1000 500 100";
		shadowDistance = "400";
		shadowSoftness = "0.15";
		numSplits = "4";
		logWeight = "0.91";
		fadeStartDistance = "0";
		lastSplitTerrainOnly = "0";
		representedInLightmap = "0";
		shadowDarkenColor = "0 0 0 -1";
		includeLightmappedGeometryInShadow = "0";
		position = "-54.9302 383.812 45.9985";
		rotation = "1 0 0 0";
		scale = "1 1 1";
		canSave = "1";
		canSaveDynamicFields = "1";
		byGroup = "0";
		mieScattering = "0.0045";
	};
	MissionGroup.add(%skyObj);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::createNewLecacySky( %this ) {
	%backup = SEP_SkySystemCreator-->backupCurrentSystem.isStateOn();

	if (%backup)
		%this.backupCurrentSky();

	%this.clearSkySystem();
	%name = SEP_SkySystemCreator-->newSunName.getText();
	%unique = getUniqueName(%name);
	%newSun = new Sun(%unique) {
		azimuth = "57.2958";
		elevation = "36";
		color = "0.8 0.8 0.8 1";
		ambient = "0.2 0.2 0.2 1";
		brightness = "1";
		castShadows = "1";
		coronaEnabled = "1";
		coronaMaterial = "Corona_Mat";
		coronaScale = "0.5";
		coronaTint = "1 1 1 1";
		coronaUseLightColor = "1";
		flareType = "SunFlareExample";
		flareScale = "1";
		attenuationRatio = "0 1 1";
		shadowType = "PSSM";
		texSize = "512";
		overDarkFactor = "2000 1000 500 100";
		shadowDistance = "400";
		shadowSoftness = "0.15";
		numSplits = "4";
		logWeight = "0.91";
		fadeStartDistance = "0";
		lastSplitTerrainOnly = "0";
		representedInLightmap = "0";
		shadowDarkenColor = "0 0 0 -1";
		includeLightmappedGeometryInShadow = "0";
		position = "-54.9302 383.812 45.9985";
		rotation = "1 0 0 0";
		scale = "1 1 1";
		canSave = "1";
		canSaveDynamicFields = "1";
		byGroup = "0";
		Direction = "1 1 -1";
	};
	MissionGroup.add(%newSun);

	if (SEP_SkySystemCreator-->createSkyBoxObject.isStateOn()) {
		%name = SEP_SkySystemCreator-->newSkyName.getText();
		%unique = getUniqueName(%name);
		%newSky = new SkyBox(%unique) {
			Material = "Mat_DefaultSky";
			drawBottom = "1";
			fogBandHeight = "0";
			position = "-58.4185 402.875 41.1485";
			rotation = "1 0 0 0";
			scale = "1 1 1";
			canSave = "1";
			canSaveDynamicFields = "1";
			byGroup = "0";
		};
		MissionGroup.add(%newSky);
		%newSun.mySkyBox = %newSky;
		%this.skyBoxObj = %obj;
		%this.SunSkyBox[%newSun.getName()] = %newSky;
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::backupCurrentSky( %this ) {
	devLog("SEP_AmbientManager::backupCurrentSky(%this)");
	%currentObj = %this.getSkySystemObject();

	if (isObject(%currentObj)) {
		//%name = %currentObj.getName()@"_Backup";
		//delObj(%name);
		%backupObj = %currentObj;
		//%backupObj = %currentObj.deepClone();
		//%backupObj.setName(%name);
		%backupObj.hidden = true;
		//%backupObj = newScriptObject("ScatterSkyBackup",MissionGroup);
		%backupObj.isBackup = true;
		//MissionGroup.add(%backupObj);
		//%backupObj.assignFieldsFrom(%scatterSkyObj);
	}

	if (%currentObj.getClassName() $= "Sun") {
		//Look for a skybox
		%skyBoxList = Lab.getMissionObjectClassList("SkyBox");

		foreach$(%obj in %skyBoxList) {
			if (%obj.hidden || %obj.isBackup) {
				%obj.hidden = true;
				continue;
			}

			//%name = %obj.getName()@"_Backup";
			//delObj(%name);
			%backupSkyBox = %obj;
			//%backupSkyBox = %obj.deepClone();
			//%backupSkyBox.setName(%name);
			%backupSkyBox.hidden = true;
			%backupSkyBox.isBackup = true;
			//MissionGroup.add(%backupSkyBox);
			%backupObj.mySkyBox = %backupSkyBox;
			break;
		}
	}
}
//------------------------------------------------------------------------------
function SEP_AmbientManager::clearSkySystem( %this ) {
	%scatterSkyList = Lab.getMissionObjectClassList("ScatterSky");

	foreach$(%sky in %scatterSkyList)
		if (!%sky.isBackup)
			delObj(%sky);

	%sunList = Lab.getMissionObjectClassList("Sun");

	foreach$(%sun in %sunList)
		if (!%sun.isBackup)
			delObj(%sun);

	%skyList = Lab.getMissionObjectClassList("SkyBox");

	foreach$(%sky in %skyList)
		if (!%sky.isBackup)
			delObj(%sky);
}

function SEP_AmbientManager::hideAllSkyObjects( %this,%hideSkyBox ) {
	%scatterSkyList = Lab.getMissionObjectClassList("ScatterSky");
	%sunList = Lab.getMissionObjectClassList("Sun");
	%skyList = Lab.getMissionObjectClassList("SkyBox");
	%list = %scatterSkyList SPC %sunList;// SPC %skyList;

	if (%hideSkyBox)
		%list = %list SPC %skyList;

	foreach$(%obj in %list)
		%obj.hidden = true;
}
