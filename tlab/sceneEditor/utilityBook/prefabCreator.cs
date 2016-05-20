//==============================================================================
// TorqueLab -> SceneEditor Inspector script
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================

//==============================================================================
// Hack to force LevelInfo update after Cubemap change...
//==============================================================================
$SceneEd_PrefabCreatorModeId = "0";
//==============================================================================
function SceneEd::initPrefabCreator( %this ) {
	SceneEd_PrefabModeMenu.clear();
	SceneEd_PrefabModeMenu.add("Object Prefab",0);
	SceneEd_PrefabModeMenu.add("Level Prefab",1);
	SceneEd_PrefabModeMenu.add("Generic Prefab",2);
	SceneEd_PrefabModeMenu.setSelected($SceneEd_PrefabCreatorMode);
}
function SceneEd::createObjectPrefab( %this ) {
	%file = ObjectPrefabCreatorFolder.getText() @"/"@ObjectPrefabCreatorName.getText()@".prefab";
	%saveFile = strReplace(%file,"//","/");
	devLog("Creating prefab from selection file:",%saveFile);
	Lab.CreatePrefab(%saveFile);
}

function SceneEd::getCurrentPrefabFolder( %this ) {
	%firstObj = EWorldEditor.getSelectedObject(0);
	%file = %firstObj.shapeName;
	%folder = filePath(%file);
	ObjectPrefabCreatorFolder.setText(%folder);
}

function SceneEd::getCurrentPrefabName( %this ) {
	%firstObj = EWorldEditor.getSelectedObject(0);
	%file = %firstObj.shapeName;
	%fileName = fileBase(%file);
	ObjectPrefabCreatorName.setText(%fileName);
}
function ObjectPrefabCreatorName::onValidate( %this ) {
}

function ObjectPrefabCreatorFolder::onValidate( %this ) {
}


function SceneEd_PrefabModeMenu::onSelect( %this,%id,%text ) {
	%mode = getWord(%text,0);
	$SceneEd_PrefabCreatorModeId = %id;
	$SceneEd_PrefabCreatorMode = %mode;
	SceneEd.setPrefabMode(%mode);
}


function SceneEd::setPrefabMode( %this, %mode) {
	foreach(%gui in SceneEd_PrefabModeStack)
		hide(%gui);

	%modeGui = SceneEd_PrefabModeStack.findObjectByInternalName(%mode);

	if (!isObject(%modeGui)) {
		warnLog("Invalid prefab mode selected:",%mode);
		return;
	}

	show(%modeGui);
}