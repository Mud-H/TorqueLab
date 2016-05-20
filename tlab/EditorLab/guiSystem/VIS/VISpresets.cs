//==============================================================================
// TorqueLab -> Editor Gui Open and Closing States
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//SideBarVIS.updatePresetMenu
function SideBarVIS::updatePresetMenu( %this,%noCallback ) {
	%searchFolder = "tlab/EditorLab/gui/editorTools/visibilityLayers/presets/*.layers.ucs";
	//Now go through each files again to add a brush with latest items
	%menus = SideBarVIS_PresetMenu;
	%selected = 0;

	foreach$(%menu in %menus) {
		%pid = 0;
		%menu.clear();
		%menu.add("Select a preset",0);

		for(%presetFile = findFirstFile(%searchFolder); %presetFile !$= ""; %presetFile = findNextFile(%searchFolder)) {
			%presetName = strreplace(fileBase(%presetFile),".layers","");
			%menu.add(%presetName,%pid++);

			if (SideBarVIS.currentPresetFile $= %presetName)
				%selected = %pid;
		}

		%menu.setSelected(%selected,!%noCallback);
	}
}
function SideBarVIS::toggleNewPresetCtrl( %this,%state ) {
	if (%state $= "")
		%state = !SideBarVIS_NewPreset.visible;

	if (%state) {
		hide(%this-->newPresetButton);
		SideBarVIS_NewPreset.visible = 1;
		return;
	}

	show(%this-->newPresetButton);
	SideBarVIS_NewPreset.visible = 0;
}

//SideBarVIS.exportPresetSample
function SideBarVIS::savePresetToFile( %this,%isNew ) {
	if (%isNew)
		%name = SideBarVIS_NewPreset-->presetName.getText();
	else
		%name = strreplace(SideBarVIS.activePreset,"*","");

	%savedPreset = EVisibilityLayers.savePreset(%name);
	
	if (%savedPreset $= "")
		return;
	SideBarVIS.activePreset = %savedPreset;

}



//SideBarVIS.loadPresetFile("visBuilder");
function SideBarVIS::loadPresetFile( %this,%filename,%noStore ) {
	
	%activePreset = EVisibilityLayers.loadPresetFile(%filename,%noStore);
	
	SideBarVIS.activePreset = %activePreset;
}

function SideBarVIS_Preset::onSelect( %this,%id,%text ) {
	if (	%id $= "0")
		return;

	SideBarVIS.loadPresetFile(%text);
}
