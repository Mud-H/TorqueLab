//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function Lab::updatePluginsBar(%this,%reset) {
	if (%reset)
		%this.resetPluginsBar();

	foreach(%pluginObj in LabPluginGroup) {
		Lab.addPluginToBar( %pluginObj );
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::resetPluginsBar(%this) {
	LabPluginArray.deleteAllObjects();
	Lab.clearDisabledPluginsBin();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::updatePluginsMenu(%this,%reset) {
	foreach(%pluginObj in LabPluginGroup)
		%this.addToEditorsMenu(%pluginObj);
}
//------------------------------------------------------------------------------
function Lab::checkAllPluginIcon(%this) {
	foreach(%pluginObj in LabPluginGroup)
		%this.checkPluginIcon(%pluginObj);
}
//==============================================================================
// Add a plugin icon to the plugin bar
//==============================================================================
//==============================================================================
function Lab::checkPluginIcon( %this, %pluginObj ) {
	%enabled =  %pluginObj.isEnabled;
	%disabledBox = EditorGui-->DisabledPluginsBox;
	if (%enabled)
	{
		%goodIcon = LabPluginArray.findObjectByInternalName(%pluginObj.plugin);
		%badIcon = %disabledBox.findObjectByInternalName(%pluginObj.plugin);
	}
	else
	{
		%badIcon = LabPluginArray.findObjectByInternalName(%pluginObj.plugin);
		%goodIcon = %disabledBox.findObjectByInternalName(%pluginObj.plugin);
	}
	delObj(%badIcon);	
	
	devLog(%pluginObj.plugin,"Enable",%enabledIcon);
	devLog(%pluginObj.plugin,"Disable",%disabledIcon);
	if (!isObject(%goodIcon)) {
		%icon = %this.createPluginIcon(%pluginObj);
		%icon.visible = true;
		
		if (%enabled) 
			LabPluginArray.add(%icon);
		 else 
			%disabledBox.add(%icon);
	
	}

	
}
//==============================================================================
function Lab::addPluginToBar( %this, %pluginObj ) {
	if (%pluginObj.isHidden)
		return;

	%this.checkPluginIcon(%pluginObj);
	return;
	//First check if the Icon object is in on control
	%enabled =  %pluginObj.isEnabled;
	%containerEnabled = LabPluginArray;
	%containerDisabled = EditorGui-->DisabledPluginsBox;
	%toolArrayEnabled = %containerEnabled.findObjectByInternalName(%pluginObj.plugin);
	%toolArrayDisabled = %containerDisabled.findObjectByInternalName(%pluginObj.plugin);

	if (isObject(%toolArrayEnabled)) {
		if (%enabled) {
			%alreadyExists = true;
			%toolArrayEnabled.visible = true;
		} else
			delObj(%toolArrayEnabled);
	}

	if (isObject(%toolArrayDisabled)) {
		if (!%enabled) {
			%alreadyExists = true;
			%toolArrayDisabled.visible = true;
		} else
			delObj(%toolArrayDisabled);
	}

	//If the Plugin Icon already exist, exit now
	if(%alreadyExists)
		return;

	%icon = %this.createPluginIcon(%pluginObj);
	%icon.visible = true;

	if (%enabled) {
		LabPluginArray.add(%icon);
	} else {
		EditorGui-->DisabledPluginsBox.add(%icon);
	}

	return;
	%icon.visible = %enabled;

	if (%icon.invalid)
		%icon.visible = false;

	if (!%enabled && !isObject(%toolArrayDisabled) && !%icon.invalid) {
		//%disabledIcon = %icon.deepClone();
		%icon.visible = true;
	}

	//Lab.sortPluginsBar();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::createPluginIcon( %this, %pluginObj ) {
	if (!IsDirectory($LabPlugin_ButtonsFolder))
		$LabPlugin_ButtonsFolder = "tlab/themes/common/buttons/plugin/";

	%icon = $LabPlugin_ButtonsFolder@%pluginObj.plugin@"Icon";

	if (!isFile(%icon@"_n.png")) {
		%invalidImg = true;
		%icon = $LabPlugin_ButtonsFolder@"TerrainEditorIcon";
	}

	%button = cloneObject(EditorGui-->PluginIconSrc);
	%button.internalName = %pluginObj.plugin;
	%button.superClass = "";
	%button.command = "Lab.setEditor(" @ %pluginObj.getName()@ ");";
	%button.bitmap = %icon;
	%button.tooltip = %pluginObj.tooltip;
	%button.visible = true;
	%button.useMouseEvents = true;
	%button.pluginObj = %pluginObj;
	%button.class = "PluginIcon";

	if (%invalidImg)
		%button.invalid = true;

	return %button;
}
//------------------------------------------------------------------------------



//==============================================================================
function Lab::togglePluginBarSize( %this ) {
	%collapsed = LabPluginArray.isCollapsed;

	if (%collapsed)
		Lab.expandPluginBar();
	else
		Lab.collapsePluginBar();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::expandPluginBar( %this ) {
	LabPluginArray.isCollapsed = false;

	foreach(%icon in LabPluginArray) {
		%enabled = %icon.pluginObj.isEnabled;
		%icon.visible = %enabled;
	}

	%this.refreshPluginToolbar();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::collapsePluginBar( %this ) {
	LabPluginArray.isCollapsed = true;

	foreach(%icon in LabPluginArray) {
		if (%icon.pluginObj.getId() $= Lab.currentEditor.getId())
			%icon.visible = true;
		else
			%icon.visible = false;
	}

	Lab.refreshPluginToolbar();
}
//------------------------------------------------------------------------------
$PluginToolbarBitmap[0] = "tlab/art/buttons/default/collapse-toolbar";
$PluginToolbarBitmap[1] = "tlab/art/buttons/default/expand-toolbar";
//==============================================================================
function Lab::refreshPluginToolbar( %this ) {
	%collapsed = LabPluginArray.isCollapsed;
	LabPluginArray.refresh();
	LabPluginBar.position = "0 -2";
	LabPluginBar.extent.y = "42";
	LabPluginArray.position.y = 2;
	LabPluginBar.extent.x = LabPluginArray.extent.x + 10;
	LabPluginBar-->resizeArrow.AlignCtrlToParent("right");
	LabPluginBar-->resizeArrow.setBitmap($PluginToolbarBitmap[%collapsed]);
	hide(LabPluginBarDecoy);
}
//------------------------------------------------------------------------------
//==============================================================================
// Plugin Toolbar GuiControl Functions
//==============================================================================

//==============================================================================
function LabPluginBar::reset( %this ) {
	devLog("LabPluginBar::reset is calling expandPluginBar instead");
	Lab.expandPluginBar();
	return;
	%count = LabPluginArray.getCount();

	for( %i = 0 ; %i < %count; %i++ )
		LabPluginArray.getObject(%i).setVisible(true);

	%this.setExtent((29 + 4) * %count + 12, 33);
	%this.isClosed = 0;
	LabPluginBar.isDynamic = 0;
	LabPluginBarDecoy.setVisible(false);
	LabPluginBarDecoy.setExtent((29 + 4) * %count + 4, 31);
	%this-->resizeArrow.setBitmap( "tlab/art/buttons/default/collapse-toolbar" );
}
//------------------------------------------------------------------------------
//==============================================================================
function LabPluginBar::expand( %this, %close ) {
	devLog("LabPluginBar::expand is calling expandPluginBar instead");
	Lab.expandPluginBar();
	return;
	%this.isClosed = !%close;
	%this.toggleSize();
}
//------------------------------------------------------------------------------
//==============================================================================
function LabPluginBar::resize( %this ) {
	devLog("LabPluginBar::resize is calling refreshPluginToolbar instead");
	Lab.refreshPluginToolbar();
	return;
	%this.isClosed = ! %this.isClosed ;
	%this.toggleSize();
}
//------------------------------------------------------------------------------
//==============================================================================
function LabPluginBar::toggleSize( %this, %useDynamics ) {
	devLog("LabPluginBar::toggleSize is calling togglePluginBarSize instead");
	Lab.togglePluginBarSize();
}
//------------------------------------------------------------------------------
//==============================================================================
function LabPluginBarDecoy::onMouseEnter( %this ) {
	Lab.togglePluginBarSize(true);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabPluginBarDecoy::onMouseLeave( %this ) {
	Lab.togglePluginBarSize(true);
}
//------------------------------------------------------------------------------
