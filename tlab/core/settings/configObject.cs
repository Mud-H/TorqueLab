//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function tlabSave(  ) {
	LabCfg.writeBaseConfig();
}
//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function LabCfg::writeBaseConfig( %this ) {
	%cfg = "tlab/config.cfg.cs";
	//Lab.writeCurrentParamsConfig();
	LabCfg.file = %cfg;
	export("$Cfg_*", %cfg, false);
	info("TorqueLab latest configs saved successfully.");
}
//------------------------------------------------------------------------------

//==============================================================================
// Config System With Globals
//==============================================================================

//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function EditorPlugin::getCfg( %this,%field ) {
	%value = $Cfg_[%this.plugin,%field];
	return %value;
}
//------------------------------------------------------------------------------
//==============================================================================
// Check config and if no value, add the one sent
function EditorPlugin::checkCfg( %this,%field, %defaultValue ) {
	%value = $Cfg_[%this.plugin,%field];

	if (%value $= "") {
		return;
		$Cfg_[%this.plugin,%field] = %defaultValue;
		%value = %defaultValue;
	}

	if (%defaultValue !$= "") {
		$CfgDefault_[%this.plugin,%field] = %defaultValue;
	}

	return %value;
}
//------------------------------------------------------------------------------
//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function EditorPlugin::setCfg( %this,%field,%value,%isDefault ) {
	%fullField = %this.plugin @"_"@%field;
	LabCfg.setCfg(%fullField,%value,%isDefault);
}
//------------------------------------------------------------------------------
//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function ParamArray::getCfg( %this,%field ) {
	%value = $Cfg_[%this.cfgData,%field];
	return %value;
}
//------------------------------------------------------------------------------
//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function ParamArray::setCfg( %this,%field,%value,%isDefault ) {
	LabCfg.setCfg(%this.cfgData @"_"@%field,%value,%isDefault);
}
//------------------------------------------------------------------------------

//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function LabCfg::setCfg( %this,%field,%value,%isDefault ) {
	logd("LabCfg::setCfg  %field:",%field,"%value",%value,"%isDefault",%isDefault);	

	if ( %isDefault )
		$CfgDefault_[%field] = %value;
	else
		$Cfg_[%field] = %value;
}
//------------------------------------------------------------------------------

//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function Lab::addDefaultSetting( %this,%field, %default ) {
	%current = LabCfg.value(%field);

	if (%current $= "")
		LabCfg.setValue(%field,%default);
	else
		LabCfg.setValue(%field,%current);

	LabCfg.setDefaultValue(%field,%default);
}
//------------------------------------------------------------------------------

//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function LabCfg::getAllConfigs( %this ) {
	LPD_ConfigNameMenu.clear();
	//Start with main config
	LPD_ConfigNameMenu.add("config",%cid++);

	for(%file = findFirstFile("tlab/core/settings/cfgs/*.cfg.cs"); %file !$= ""; %file = findNextFile("tlab/core/settings/cfgs/*.cfg.cs")) {
		%fileBase = fileBase(fileBase(%file));
		//if (%fileBase $= "default")
		//	continue;
		%fileName = %fileBase;
		LPD_ConfigNameMenu.add(%fileName,%cid++);

		if (LabCfg.currentCfgFile $= %fileName)
			%selected = %cid;
	}

	if (LabCfg.currentCfgFile $= "")
		LabCfg.currentCfgFile = "config";

	LPD_ConfigNameEdit.setText(LabCfg.currentCfgFile);
}
//------------------------------------------------------------------------------
//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function LabCfg::loadSelectedCfg( %this ) {
	%fileText = LPD_ConfigNameEdit.getText();
	%filename = fileBase(%fileText)@".cfg.cs";
	%cfgFile = "tlab/core/settings/cfgs/"@%filename;

	if (fileBase(%fileText) $= "config")
		%cfgFile = "tlab/config.cfg.cs";

	if (!isFile(%cfgFile))
		return;

	LabCfg.file = %cfgFile;
	Lab.initConfigSystem(%cfgFile);
}
//------------------------------------------------------------------------------
//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function LabCfg::loadBaseCfg( %this ) {
	%cfg = "tlab/config.cfg.cs";
	%fileName = "config.cfg.cs";

	if (!isFile(%cfg)) {
		%fileName = "default.cfg.cs";
		%cfg = "tlab/core/settings/defaults.cfg.cs";
	}

	if (!isFile(%cfg)) {
		LPD_ConfigNameEdit.setText("defaults.cfg.cs");
		devLog("No default config found, creating new one with current settings");
		return;
	}

	LPD_ConfigNameEdit.setText(%fileName);
	LabCfg.file = %cfg;
	Lab.initConfigSystem();
}
//------------------------------------------------------------------------------
//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function LabCfg::saveSelectedCfg( %this,%forced ) {
	%fileText = LPD_ConfigNameEdit.getText();

	if (%fileText $= "default" && !%forced) {
		LabMsgYesNo("Warning! Overwritting default config","You are about to overwrite the default config file which can cause problem if some settings are corrupted. We recommend to not" SPC
						"overwrite the file and use a custom file name instead. Do you want to proceed with overwritting default config?","LabCfg.saveSelectedCfg(true);","");
		return;
	}

	%filename = fileBase(%fileText)@".cfg.cs";

	//If this is the main config, store it in root
	if (fileBase(%fileText) $= "config")
		%cfgFile = "tlab/"@%filename;
	else
		%cfgFile = "tlab/core/settings/cfgs/"@%filename;

	LabCfg.file = %cfgFile;
	LabCfg.currentCfgFile = fileBase(%fileText);
	export("$Cfg_*", %cfgFile, false);
	//export("$CfgDefault_*", "tlab/core/settings/defaults.cfg.cs", false);
	LabCfg.getAllConfigs();
}
//------------------------------------------------------------------------------


//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function LabCfg::saveDefaults( %this ) {
	export("$CfgDefault_*", "tlab/core/settings/defaults.cfg.cs", false);
}
//------------------------------------------------------------------------------
//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function LabCfg::resetDefaults( %this ) {
	exec("tlab/core/settings/defaults.cfg.cs");
}
//------------------------------------------------------------------------------
