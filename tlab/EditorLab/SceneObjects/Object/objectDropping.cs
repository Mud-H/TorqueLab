//==============================================================================
// TorqueLab -> Editor Gui General Scripts
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$WorldEditor_DropTypes = "toTerrain atOrigin atCamera atCameraRot belowCamera screenCenter atCentroid belowSelection";
$Scene_DropTypes = "currentSel currentSelZ";
$Scene_AllDropTypes = $WorldEditor_DropTypes SPC $Scene_DropTypes;
$Scene_DropTypeDisplay["atOrigin"] = "at origin";
$Scene_DropTypeDisplay["atCamera"] = "at camera";
$Scene_DropTypeDisplay["atCameraRot"] = "at camera+Rot.";
$Scene_DropTypeDisplay["belowCamera"] = "below camera";
$Scene_DropTypeDisplay["atCentroid"] = "at centroid";
$Scene_DropTypeDisplay["toTerrain"] = "to terrain";
$Scene_DropTypeDisplay["belowSelection"] = "below sel.";
$Scene_DropTypeDisplay["currentSel"] = "at current sel.";
$Scene_DropTypeDisplay["currentSelZ"] = "at current sel. Z";
//==============================================================================
function SceneDropTypeMenu::onSelect(%this, %id,%text) {
	%dropType = %this.typeId[%id];
	Scene.setDropType(%dropType);
}
//------------------------------------------------------------------------------
//==============================================================================
function Scene::setDropType(%this, %dropType) {
	foreach$(%menu in Scene.dropTypeMenus) {
		%menu.setText($Scene_DropTypeDisplay[%dropType]);
	}

	if (strFind($WorldEditor_DropTypes,%dropType))
		EWorldEditor.dropType = %dropType;

	Scene.dropMode = %dropType;
}
//------------------------------------------------------------------------------
