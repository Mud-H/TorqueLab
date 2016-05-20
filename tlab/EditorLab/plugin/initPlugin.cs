//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TLab::DefaultPlugins = "SceneEditor";

//==============================================================================
// Create the Plugin object with initial data
function Lab::createPlugin(%this,%pluginName,%displayName,%parentPlugin,%alwaysEnable,%isModule) {
	//Set plugin object name and verify if already existing
	%plugName = %pluginName@"Plugin";

	if (isObject( %plugName)) {
		warnLog("Plugin already created:",%pluginName);
		return %plugName;
	}

	if (%displayName $= "")
		%displayName = %pluginName;

	%pluginOrder = $Cfg_[%pluginName,"pluginOrder"];
	%enabled = $Cfg_[%pluginName,"isEnabled"];
	//Create the ScriptObject for the Plugin
	%pluginObj = new ScriptObject( %plugName ) {
		superClass = "EditorPlugin"; //Default to EditorPlugin class
		editorGui = EWorldEditor; //Default to EWorldEditor
		editorMode = "World";
		displayName = %displayName;
		toolTip = %displayName;
		alwaysOn = %alwaysEnable;
		isEnabled = %enabled;
		useTools = false;
	};

	if (%parentPlugin !$= "")
		%pluginObj.parentPlugin = %parentPlugin;

	if (%isModule) {
		%pluginObj.module = %pluginName;
		return %pluginObj;
	}

	%pluginObj.plugin = %pluginName;
	%pluginObj.pluginOrder = %pluginOrder;
	%pluginObj.shortPlugin = %shortObjName;
	LabPluginGroup.add(%pluginObj);

	if (strFind($TLab::DefaultPlugins,%pluginName))
		%pluginObj.isDefaultPlugin = true;

	if (%alwaysEnable)
		$PluginAlwaysOn[%pluginName] = true;
		
	if (%pluginObj.isMethod("onPluginCreated"))
		%pluginObj.onPluginCreated();
		
	if (isObject(%pluginObj.parentPlugin)) {		
		
		if (%pluginObj.parentPlugin.isMethod("onPluginCreated"))
		%pluginObj.parentPlugin.onPluginCreated();
	}
		
	//Moved in execPlugin since for some plugin the scripts are not loaded yet
	//if (%pluginObj.isMethod("onPluginCreated"))
	//%pluginObj.onPluginCreated();
	//Lab.initPluginData(%pluginObj);
	return %pluginObj;
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the Plugin scripts - if no execPLUGINNAME, the scripts were loaded using old system
function Lab::execPlugin(%this,%pluginObj) {
	if (%pluginObj.parentPlugin !$= "") {
		%sidePluginObj = %pluginObj;
		%parentPluginObj = %pluginObj.parentPlugin@"Plugin";

		if (!isObject(%pluginObj)) {
			warnLog("Something went wrong while trying to exec plugin with parent! PluginObj:",%pluginObj,"Parent",	%pluginObj.parentPlugin);
			return;
		}

		%pluginObj = %parentPluginObj;
	}

	if (%pluginObj.initialized) {
	   if (%pluginObj.plugin !$= "TerrainEditor")
	   {
		   warnLog("execPlugin called on already initialized plugin:",%pluginObj.plugin,"It shouldn't have happen and the execPlugin is cancelled");
		   return;
	   }
	}

	%pluginName = %pluginObj.plugin;
	%execFunc = "init" @ %pluginName;
	%accel = Lab.addToEditorsMenu( %pluginObj.getName() );
	
	
	if (isFunction(%execFunc) && !%pluginObj.initialized)
		call(%execFunc);

	if (!isObject(%pluginObj.paramArray))
			Lab.initPluginConfig(%pluginObj);
			
	if (!LabPaletteArray.paletteObjLoaded[%pluginName@"Palette"])
		%this.loadPalette(%pluginName@"Palette");

	if (!%pluginObj.paramArray.firstReadDone)
		%this.readConfigArray(%pluginObj.paramArray);
		
	if (%pluginObj.isMethod("onPluginLoaded"))
		%pluginObj.onPluginLoaded();

	%pluginObj.initialized = true;

	if (isObject(%sidePluginObj)) {		
		
		if (%sidePluginObj.isMethod("onPluginLoaded"))
		%sidePluginObj.onPluginLoaded();
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Module are mini plugin which don't need tools or icons
function Lab::createModule(%this,%pluginName,%displayName,%alwaysEnable) {
	%moduleObj = %this.createPlugin(%pluginName,%displayName,%alwaysEnable,true);
	%moduleObj.isModule = true;
	LabPluginGroup.remove(%moduleObj);
	LabPluginModGroup.add(%moduleObj);
}
//------------------------------------------------------------------------------
//==============================================================================
// Module are mini plugin which don't need tools or icons
function Lab::initAllModule(%this) {
	foreach(%module  in LabPluginModGroup)
		%this.initModule(%module);
}
//------------------------------------------------------------------------------
//==============================================================================
// Module are mini plugin which don't need tools or icons
function Lab::initModule(%this,%module) {
	%initFunc = "init"@%module.plugin;

	if (isFunction(%initFunc))
		call(%initFunc);
}
//------------------------------------------------------------------------------
//==============================================================================
// Plugins Initialization Scripts
//==============================================================================



//==============================================================================
// Initialize the PluginObj configs
function Lab::initPluginConfig(%this,%pluginObj) {
	%pluginName = %pluginObj.plugin;

	if (isFile("tlab/"@%pluginName@"/defaults.cs"))
		exec("tlab/"@%pluginName@"/defaults.cs");

	//Moving toward new params array system
	%newArray = Lab.createParamsArray("Plugins",%pluginName,%pluginObj);
	%newArray.displayName = %pluginObj.displayName;
	%pluginObj.paramArray = %newArray;
	%newArray.pluginObj = %pluginObj;
	%newArray.paramCallback = "Lab.onParamPluginBuild";
	//Default plugin settings
	%newArray.setVal("pluginOrder",      "99" TAB "pluginOrder" TAB "" TAB "" TAB %pluginObj.getName());
	%newArray.setVal("isEnabled",      "1" TAB "isEnabled" TAB "" TAB "" TAB %pluginObj.getName());
	%newArray.cfgData = %pluginName;
	
	%extraParamsGui = %pluginObj.plugin@"Params";
	if (isObject(%extraParamsGui))
	{
	   %stack = %extraParamsGui-->ParamsStack;
	   if (isObject(%stack))
	      %newArray.extraStack = %stack;  
	}

	if (%pluginObj.isMethod("initParamsArray"))
		%pluginObj.initParamsArray(%newArray);
}
//------------------------------------------------------------------------------


//==============================================================================
// Make the Active Plugins Enabled - Called from Editor::open
//==============================================================================
//==============================================================================
//Call when Editor is open, check that all plugin are enabled correctly
function Lab::updateActivePlugins(%this) {
	foreach(%pluginObj in LabPluginGroup) {
		%enabled = %pluginObj.getCfg("isEnabled");
		%pluginObj.setPluginEnable(%enabled,true);
	}

	Lab.updatePluginsMenu();
}
//------------------------------------------------------------------------------
function Lab::enablePlugin(%this,%pluginObj,%enabled) {
	if (!isObject(%pluginObj)) {
		warnLog("Trying to enable invalid plugin:",%pluginObj);
		return;
	}

	if (!%pluginObj.initialized) {
		%this.execPlugin(%pluginObj);
		info("Plugin was not execed:",%pluginObj.plugin,"Check",%pluginObj.initialized);
	}

	if (%pluginObj.isEnabled)
		warnLog(%pluginObj.plugin,"The plugin is already enabled...");

	%pluginObj.setPluginEnable(true);
}
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
function Lab::disablePlugin(%this,%pluginObj) {
	if (!isObject(%pluginObj)) {
		warnLog("Trying to enable invalid plugin:",%pluginObj);
		return;
	}

	if (!%pluginObj.isEnabled)
		warnLog(%pluginObj.plugin,"The plugin is already disabled...");

	%pluginObj.setPluginEnable(false);
}
//------------------------------------------------------------------------------
//==============================================================================
function EditorPlugin::setPluginEnable(%this,%enabled,%noMenuUpdate) {
	if (%this.alwaysOn)
		%enabled = "1";
	
	$Cfg_[%this.plugin,"isEnabled"] = %enabled;
	%name = %this.plugin;
	%this.isEnabled = %enabled;
	Lab.checkPluginIcon(%this);

	//Simply rebuild the editorMenus since the GuiMenuBar is primitive...
	if (!%noMenuUpdate)
		Lab.updatePluginsMenu();
}
//------------------------------------------------------------------------------