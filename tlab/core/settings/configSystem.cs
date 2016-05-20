//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function Lab::initConfigSystem( %this,%cfgFile ) {
	//Start by building all the ConfigArray Params
	$LabConfigArrayGroup = newSimGroup("LabConfigArrayGroup");
	exec("tlab/core/commonSettings.cs");
	
	LabParamsGroup.clear();
	%this.initCommonParams();
 
   exec("tlab/core/settings/defaults.cfg.cs");
   
   //Overwrite the GuiEditor Globals
    exec("guiEditor/system/guiEd_Default.cfg.cs");
   if (isFile(%cfgFile))
       exec(%cfgFile);
   else
      exec("tlab/config.cfg.cs");
  
	//Only read default Editor Params (No Plugin, they will be read when activated)
	Lab.readAllConfigArray(true,true);	
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::exportCfgPrefs(%this) {
	export("$Cfg_*", "tlab/configPrefs.cs", false);
}
//------------------------------------------------------------------------------


//==============================================================================
// Set params settings group to their default value
//==============================================================================

//------------------------------------------------------------------------------
//==============================================================================
// Read all field for each ParamArray and set the current value in options
//==============================================================================
function Lab::readAllConfigArray(%this,%skipPluginArray,%setEmptyToDefault) {
	foreach(%array in LabParamsGroup)
		%this.readConfigArray(%array,%skipPluginArray,%setEmptyToDefault);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::readConfigArray(%this,%array,%skipPluginArray,%setEmptyToDefault) {	
	if (!isObject(%array))
	{
		warnLog("Trying to read a config array for an invalid ArrayObject",	%array);
		return;
	}
	if (isObject(%array.pluginObj))
	{
		if (!%array.pluginObj.initialized || %skipPluginArray)
			return;
	}
	
	%i = 0;
	
	for( ; %i < %array.count() ; %i++) {
		%field = %array.getKey(%i);
		
      %value = $Cfg_[%array.cfgData,%field];
      
		if (%setEmptyToDefault && %value $= "") {		  
		   %data = %array.getValue(%i);
		   %value = getField(%data,0);
		   devLog("readConfigArray About to set EmptyToDefault Field:",  %field,"Default", 	%value,"Data",%data);
		   continue;
         $Cfg_[%array.cfgData,%field] = %value;      
		}
		if (%value !$= "")
		   setParamFieldValue(%array,%field,%value);		
	}
	%array.firstReadDone = true;
}
//------------------------------------------------------------------------------
