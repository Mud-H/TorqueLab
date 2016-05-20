//==============================================================================
// TorqueLab -> Lab PostFX Manager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$EPostFx_PresetFolder = "tlab/EditorLab/gui/editorDialogs/postFXManager/presets";
$EPostFx_PresetFileFilter = "Post Effect Presets|*.pfx.cs|*.postfxpreset.cs";


//==============================================================================
function EPostFxManager::initPresets(%this) {
	EPostFx_Presets-->presetName.text = "[Preset Name]";
	EPostFx_Presets-->saveActivePreset.active = 0;
	%this.updatePresetMenu();
	%this.activatePresetFile($EPostFx_PresetFolder@"/default.pfx.cs");
}
//------------------------------------------------------------------------------
//==============================================================================
//EPostFxManager.updatePresetMenu();
function EPostFxManager::updatePresetMenu(%this) {
	%searchFolder = $EPostFx_PresetFolder@"/*.pfx.cs";
	PFXM_PresetMenu.clear();
	PFXM_PresetMenu.add("[Preset Name]",0);
	%menuId = 0;

	for(%file = findFirstFile(%searchFolder); %file !$= ""; %file = findNextFile(%searchFolder)) {
		%fileName = fileBase(%file);
		%fileNameClean = strreplace(%fileName,".pfx","");
		PFXM_PresetMenu.add(%fileNameClean,%menuId++);
	}

	PFXM_PresetMenu.setSelected(0,false);
}
//------------------------------------------------------------------------------
//==============================================================================
function PFXM_PresetMenu::onSelect(%this,%id,%text) {
	%file = $EPostFx_PresetFolder@"/"@%text@".pfx.cs";
	EPostFxManager.activatePresetFile(%file);
}
//------------------------------------------------------------------------------
//==============================================================================
function EPostFxManager::activatePresetFile(%this,%file) {
	if (!isFile(%file))
		return;
	exec(%file);

	PostFXManager.settingsApplyFromPreset();      
	//%result = EPostFxManager.loadPresetsFromFile(%file);

	if (%failed) {
		EPostFx_Presets-->saveActivePreset.active = 0;
		EPostFx_Presets-->presetName.text = "[Invalid Preset]";
		return;
	}

	%fileName = fileBase(%file);
	%fileName = strreplace(%fileName,".pfx","");
	EPostFx_Presets-->saveActivePreset.active = 1;
	EPostFx_Presets-->presetName.text = %fileName;
}
//------------------------------------------------------------------------------
//==============================================================================
// Save PostFx Presets
//==============================================================================
//==============================================================================

function EPostFxManager::saveActivePreset(%this) {
	%name = %this.validatePresetName();

	if (%name $= "")
		return;

	%file = $EPostFx_PresetFolder@"/"@%name@".pfx.cs";
	%this.savePresetsToFile(%file);
}
//------------------------------------------------------------------------------
//==============================================================================
function EPostFxManager::validatePresetName(%this) {
	%name = EPostFx_Presets-->presetName.getText();

	if (strFindWords(%name,"[ ]") || %name $= "")
		%invalidName = true;

	if(%invalidName) {
		warnLog("Preset name is invalid:",%name);
		EPostFx_Presets-->saveActivePreset.active = 0;
		return "";
	}

	EPostFx_Presets-->saveActivePreset.active = 1;
	return %name;
}
//------------------------------------------------------------------------------
//==============================================================================

function EPostFxManager::selectPresetFileSave(%this) {
	%default = $EPostFx_PresetFolder@"/default.pfx.cs";
	getSaveFilename($EPostFx_PresetFileFilter, "EPostFxManager.savePresetsToFile", %defaultFile);
}
//------------------------------------------------------------------------------
//==============================================================================
function EPostFxManager::savePresetsToFile(%this,%file) {
	
	if (%file $= "")
		return;
		
		 //Apply the current settings to the preset
   PostFXManager.settingsApplyAll();
   
   export("$PostFXManager::Settings::*", %file, false);

		info("PostFX Presets exported to file:",%file);
	%this.updatePresetMenu();
	
	/*
	if (%useStockFormat){
		EPostFxManager.saveMissionPresets(%file);
	}
	else {
		$PostFXPresetFormat = "Lab";
		foreach$(%field in $EPostFx_PostFxList SPC "PostFX"){
				eval("%value = $LabPostFx_Enabled_"@%field@";");
				$PostFxPreset_["Enabled",%field] = %value;
		}
		foreach$(%type in "DOF HDR LightRays SSAO"){
			foreach$(%field in $EPostFx_Fields_[%type]){
				eval("%value = $"@%type@"PostFx::"@%field@";");
				$PostFxPreset_[%type,%field] = %value;
			}
		}

		export("$PostFxPreset_*", %file);
	}*/

}
//------------------------------------------------------------------------------

//==============================================================================
//EPostFxManager.saveMissionPresets
function EPostFxManager::saveMissionPresets(%this) {
	%missionFile = $Client::MissionFile;
		%postFxFile = strreplace(%missionFile,".mis",".postfxpreset.cs");
		%this.savePresetsToFile(%postFxFile);
	
}
//------------------------------------------------------------------------------

//==============================================================================
// LOAD PostFx Presets
//==============================================================================
//==============================================================================
function EPostFxManager::selectPresetFileLoad(%this) {
	%default = $EPostFx_PresetFolder@"/default.pfx.cs";
	getSaveFilename($EPostFx_PresetFileFilter, "EPostFxManager.loadPresetHandler", %defaultFile);
}
//------------------------------------------------------------------------------
//==============================================================================
function EPostFxManager::loadPresetHandler(%this,%file) {
	PostFxManager.loadPresetHandler(%file);
	EPostFxManager.syncAll();
	
}
//------------------------------------------------------------------------------

//==============================================================================
function EPostFxManager::dumpPrefs(%this) {
	export("$LabPostFx*", "tlab/postFxPrefs.cs");
}
//------------------------------------------------------------------------------
