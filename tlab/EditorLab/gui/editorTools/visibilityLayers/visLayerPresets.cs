//==============================================================================
// TorqueLab -> Editor Gui Open and Closing States
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//EVisibilityLayers.updatePresetMenu
function EVisibilityLayers::updatePresetMenu( %this,%noCallback ) {
	%searchFolder = "tlab/EditorLab/gui/editorTools/visibilityLayers/presets/*.layers.ucs";
	//Now go through each files again to add a brush with latest items
	%menus = EVisibilityLayers_PresetMenu SPC SEP_CreatorTools-->VisibilityPreset  SPC SideBarVIS_PresetMenu;
	%selected = 0;

	foreach$(%menu in %menus) {
		%pid = 0;
		%menu.clear();
		%menu.add("Select a preset",0);

		for(%presetFile = findFirstFile(%searchFolder); %presetFile !$= ""; %presetFile = findNextFile(%searchFolder)) {
			%presetName = strreplace(fileBase(%presetFile),".layers","");
			%menu.add(%presetName,%pid++);

			if (EVisibilityLayers.currentPresetFile $= %presetName)
				%selected = %pid;
		}

		%menu.setSelected(%selected,!%noCallback);
	}
}
function EVisibilityLayers::toggleNewPresetCtrl( %this,%state ) {
	if (%state $= "")
		%state = !EVisibilityLayers_NewPreset.visible;

	if (%state) {
		hide(%this-->newPresetButton);
		EVisibilityLayers_NewPreset.visible = 1;
		return;
	}

	show(%this-->newPresetButton);
	EVisibilityLayers_NewPreset.visible = 0;
}

//EVisibilityLayers.exportPresetSample
function EVisibilityLayers::savePresetToFile( %this,%isNew ) {
	if (%isNew)
		%name = EVisibilityLayers_NewPreset-->presetName.getText();
	else
		%name = strreplace(EVisibilityLayers.activePreset,"*","");
	
	%this.savePreset(%name);
}
function EVisibilityLayers::savePreset( %this,%name ) {

	//devLog("Saving Preset:",%name,"IsNew",	%isNew);
	if (strFind(%name,"[")) {
		return;
	}

	%layerExampleObj = newScriptObject();
	%layerExampleObj.internalName = %name;

	for ( %i = 0; %i < %this.classArray.count(); %i++ ) {
		%class = %this.classArray.getKey( %i );
		eval("%selectable = $" @ %class @ "::isSelectable;");
		eval("%renderable = $" @ %class @ "::isRenderable;");
		%layerExampleObj.selectable[%class] = %selectable;
		%layerExampleObj.visible[%class] = %renderable;
	}

	for ( %i = 0; %i < %this.array.count(); %i++ ) {
		%text = "  " @ %this.array.getValue( %i );
		%val = %this.array.getKey( %i );
		%var = getWord( %val, 0 );
		eval("%value = "@%var@";");
		%formatVar = strreplace(%var,"$","");
		%formatVar = strreplace(%formatVar,"::","__");
		%formatVar = strreplace(%formatVar,".","_dot_");
		%layerExampleObj.visOptions[%formatVar] = %value;
	}

	%layerExampleObj.save("tlab/EditorLab/gui/editorTools/visibilityLayers/presets/"@%name@".layers.ucs",0,"%presetLayers = ");
	%this.toggleNewPresetCtrl(false);
	EVisibilityLayers.loadPresetFile(%name);
	%this.updatePresetMenu(true);
	
	return %name;
	
}
//EVisibilityLayers.exportPresetSample
function EVisibilityLayers::exportPresetSample( %this ) {
	%layerExampleObj = newScriptObject("EVisibilityLayerPresetExample");

	for ( %i = 0; %i < %this.classArray.count(); %i++ ) {
		%class = %this.classArray.getKey( %i );
		%layerExampleObj.selectable[%class] = 1;
		%layerExampleObj.visible[%class] = 1;
	}

	%layerExampleObj.save("tlab/EditorLab/gui/editorTools/visibilityLayers/presetExample.layers.ucs",0,"%presetLayers = ");
}



//EVisibilityLayers.loadPresetFile("visBuilder");
function EVisibilityLayers::loadPresetFile( %this,%filename,%noStore ) {
	
	if (!isObject(%this.classArray))
		EVisibilityLayers.init();
	
	%file = "tlab/EditorLab/gui/editorTools/visibilityLayers/presets/"@%filename@".layers.ucs";
	if (!isFile(%file))
		return;

	exec(%file);

	for ( %i = 0; %i < %this.classArray.count(); %i++ ) {
		%class = %this.classArray.getKey( %i );
		%selectable = %presetLayers.selectable[%class];
		%renderable = %presetLayers.visible[%class];

		if (%selectable !$= "") {
			eval("$" @ %class @ "::isSelectable = \""@%selectable@"\";");
			//info("Class:",%class,"isSelectable set to:",%selectable);
		}

		if (%renderable !$= "") {
			eval("$" @ %class @ "::isRenderable = \""@%renderable@"\";");
			//info("Class:",%class,"isRenderable set to:",%renderable);
		}
	}

	for ( %i = 0; %i < %this.array.count(); %i++ ) {
		%text = "  " @ %this.array.getValue( %i );
		%val = %this.array.getKey( %i );
		%var = getWord( %val, 0 );
		%formatVar = strreplace(%var,"$","");
		%formatVar = strreplace(%formatVar,"::","__");
		%formatVar = strreplace(%formatVar,".","_dot_");
		%value = %presetLayers.visOptions[%formatVar];

		if (%value $= "")
			continue;

		eval(%var@" = %value;");
		//info("Variable:",%var," set to:",%value);
	}

	%presetName = %filename;

	if (!%noStore) {
		EVisibilityLayers.currentPresetFile = %filename;
	} else {
		%presetName = "*"@%presetName;
	}

	EVisibilityLayers.activePreset = %presetName;
	return %presetName;
}

function EVisibilityLayers_Preset::onSelect( %this,%id,%text ) {
	if (	%id $= "0")
		return;

	EVisibilityLayers.loadPresetFile(%text);
}
