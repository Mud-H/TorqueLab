//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$LabPlugin::ToolbarPos = "306 0";

//==============================================================================
// Activate the interface for a plugin
//==============================================================================


//==============================================================================
//Reinitialize all plugin data
function Lab::togglePluginParamUI(%this) {
	if (LabParamsDlg.isAwake()) {
		popDlg(LabParamsDlg);
		return;
	}

	pushDlg(LabParamsDlg);
	%currentPlugin = Lab.currentEditor.plugin;
	%id = LabParamsTree.findItemByValue("Plugins_"@%currentPlugin);

	if (%id <= 0)
		return;

	LabParamsTree.clearSelection();
	LabParamsTree.selectItem(%id);
}
//------------------------------------------------------------------------------


//==============================================================================
// Manage the GUI for the plugins
//==============================================================================
function Lab::addGuiToPluginSet(%this,%plugin,%gui) {
	%pluginSimSet = %plugin@"_GuiSet";

	if (!isObject(%pluginSimSet)) {
		%pluginSimSet = newSimSet(%pluginSimSet);
	}

	%gui.plugin = %plugin;
	%pluginSimSet.add(%gui);
	LabPluginGuiSet.add(%gui);
}
function Lab::addPluginEditor(%this,%plugin,%gui,%notFullscreen) {
	%this.addGuiToPluginSet(%plugin,%gui);

	if(%notFullscreen) {
		%this.addGui(%gui,"ExtraGui");
	} else {
		%this.addGui(%gui,"EditorGui");
	}

	// Simset Holding Editor Guis for the plugin
}
//------------------------------------------------------------------------------
//==============================================================================
// Plugin Tools (Right Column UI)
function Lab::addPluginGui(%this,%plugin,%gui) {
	%this.addGuiToPluginSet(%plugin,%gui);
	%pluginObj = %plugin@"Plugin";
	%pluginObj.useTools = true;
	%pluginObj.toolsGui = %gui;
	%gui.superClass = "FrameSetPluginTools";
	/*%pluginFrameSet = %plugin@"_FrameSet";

	if (!isObject(%pluginFrameSet)) {
		newSimSet(%pluginFrameSet);
	}

	%pluginFrameSet.add(%gui);
	*/
	// Simset Holding Editor Guis for the plugin
	%this.addGui(%gui,"Tool");
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::addPluginToolbar(%this,%plugin,%gui) {
	%gui.plugin = %plugin;
	%this.addGuiToPluginSet(%plugin,%gui);
	%this.addGui(%gui,"Toolbar");
}
function Lab::addPluginDlg(%this,%plugin,%gui) {
	%pluginObj = %plugin@"Plugin";
	%pluginObj.dialogs = %gui;
	%this.addGuiToPluginSet(%plugin,%gui);
	%gui.pluginObj = %pluginObj;
	%gui.superClass = "PluginDlg";
	%this.addGui(%gui,"Dialog");
	%gui.isDlg = true;
}

function Lab::addPluginPalette(%this,%plugin,%gui) {
	%gui.internalName = %plugin;
	%this.addGui(%gui,"Palette");
	Lab.pluginHavePalette[%plugin] = true;
}

function Lab::addPluginSideBar(%this,%plugin,%gui) {
	%gui.internalName = %plugin;
	%this.addGui(%gui,"SideBar");
	Lab.pluginSideBar[%plugin] = true;
}


//==============================================================================
// Editor Main menu functions
//==============================================================================
//==============================================================================
function Lab::updatePluginsMenu(%this) {
	if (!$Cfg_UI_Menu_UseNativeMenu)
		Lab.clearSubmenuItems($LabMenuEditorSubMenu.x,$LabMenuEditorSubMenu.y);

	foreach(%pluginObj in LabPluginGroup)
		%this.addToEditorsMenu(%pluginObj);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::addToEditorsMenu( %this, %pluginObj ) {
	%displayName = %pluginObj.displayName;
	%accel = "";

	if (!%pluginObj.isEnabled)
		return;

	if ($Cfg_UI_Menu_UseNativeMenu) {
		%windowMenu = Lab.findMenu( "Editors" );
		%count = %windowMenu.getItemCount();
		%alreadyExists = false;

		for ( %i = 0; %i < %count; %i++ ) {
			%thisName = getField(%windowMenu.Item[%i], 2);

			if(%plugin.getName() $= %thisName)
				%alreadyExists = true;
		}

		if( %accel $= "" && %count < 9 )
			%accel = "F" @ %count + 1;
		else
			%accel = "";

		if(!%alreadyExists)
			%windowMenu.addItem( %count, %displayName TAB %accel TAB %pluginObj.getName() );
	} else if ($LabMenuEditor[%plugin.plugin] $= "") {
		%menuId = getWord($LabMenuEditorSubMenu,0);
		%itemId = getWord($LabMenuEditorSubMenu,1);
		%subId = $LabMenuEditorNextId++;
		$WorldEdMenuPlugin[%pluginObj.getName()] = %menuId SPC %itemId SPC %subId;
		$LabMenuSubMenuItem[%menuId,%itemId,%subId] = %displayName TAB "" TAB "Lab.setEditor(\""@%pluginObj.getName()@"\");" TAB ""@%pluginObj.getName()@".isActivated;" TAB "4";
		Lab.addSubMenuItemData(WorldEdMenu,%menuId,%itemId,%subId,$LabMenuSubMenuItem[%menuId,%itemId,%subId]);
		//Lab.addSubmenuItem(%menuId,%itemId,%displayName,%subId,"",-1);
	}

	return %accel;
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::removeFromEditorsMenu( %this,  %pluginObj ) {
	if (!isObject(%windowMenu))
		return;

	%windowMenu = Lab.findMenu( "Editors" );
	%pluginName = %pluginObj.getName();
	%count = %windowMenu.getItemCount();
	%removeId = -1;

	for ( %i = 0; %i < %count; %i++ ) {
		%thisName = getField(%windowMenu.Item[%i],2);

		if(%pluginName $= %thisName) {
			%windowMenu.removeItem(%i);
			break;
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function Lab::resetEditorsMenu( %this ) {
	%windowMenu = Lab.findMenu( "Editors" );
	%count = %windowMenu.getItemCount();

	for ( %i = %count; %i > 0; %i--)
		%windowMenu.removeItem(%i-1);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::updatePluginsTools(%this) {	

	foreach(%tools in LabToolGuiSet) {
		EditorGui-->ToolsContainer.add(%tools);
	}
}
//------------------------------------------------------------------------------
