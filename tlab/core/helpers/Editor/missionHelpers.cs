//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
/*
 LabMenu.addMenu("MyFirstMenu",0);
   LabMenu.addMenuItem(0,"MyFirstItem",0,"Ctrl numpad0",-1);
   LabMenu.addSubmenuItem("MyFirstMenu","MyFirstItem","MyFirstSubItem",0,"",-1);
   */


//==============================================================================
function Lab::setCurrentViewAsPreview(%this) {
	setGui(HudlessPlayGui);
	%name = expandFileName( filePath(MissionGroup.getFilename())@"/"@fileBase(MissionGroup.getFilename()));
	takeLevelScreenShot(%name);
	schedule(500,0,"setGui",EditorGui);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::setNextScreenShotPreview(%this) {
	$LabNextScreenshotIsPreview = true;
}
//------------------------------------------------------------------------------


//==============================================================================
// SEP_GroundCover.getMissionGroundCover();
function Lab::getMissionObjectClassList( %this,%class ) {
	if (!isObject(MissionGroup))
		return "-1";

	%this.missionObjectClassList = "";
	%this.checkMissionSimGroupForClass(MissionGroup,%class);
	%list = %this.missionObjectClassList;
	%this.missionObjectClassList = "";
	return %list;
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function Lab::checkMissionSimGroupForClass( %this,%group,%class ) {
	foreach(%obj in %group) {
		if (%obj.getClassname() $= %class) {
			%this.missionObjectClassList = strAddWord(%this.missionObjectClassList,%obj.getId());
		} else if (%obj.getClassname() $= "SimGroup") {
			%this.checkMissionSimGroupForClass(%obj,%class);
		}
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// SEP_GroundCover.getMissionGroundCover();
function Lab::getDatablockClassList( %this,%class,%fieldValidators ) {
	%dataList = "";

	foreach(%obj in DataBlockSet) {
		if (%obj.getClassname() !$= %class)
			continue;

		if (%fieldValidators !$= "") {
			%skipThis = false;
			%records = strReplace(%fieldValidators,";;","\n");
			%count = getRecordCount(%records);

			for (%i = 0; %i<%count; %i++) {
				%fieldsData = getRecord(%fieldValidators,%i);
				%fields = strReplace(%fieldsData,">>","\t");
				%field = getField(%fields,0);
				%value = getField(%fields,1);
				%thisValue = %obj.getFieldValue(%field);

				if (%thisValue !$= %value) {
					%skipThis = true;
					continue;
				}
			}
		}

		if (%skipThis)
			continue;

		%dataList = strAddWord(%dataList,%obj.getId());
	}

	return %dataList;
}
//------------------------------------------------------------------------------
