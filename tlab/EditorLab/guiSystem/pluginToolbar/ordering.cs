//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Plugin Tool Bar Ordering Management (Sort and Store)
//==============================================================================

//==============================================================================
// Reorder the Plugins Bar from the store settings (If Default true, default will be used)
function Lab::sortPluginsBar(%this,%default) {
	if (%default)
		LabPluginArray.sort("sortPluginByDefaultOrder");
	else
		LabPluginArray.sort("sortPluginByOrder");

	LabPluginArray.refresh();
	//%this.updatePluginIconOrder(); //WIP Fast - Should only be updated when leaving
}
//------------------------------------------------------------------------------
//Compare plugin order A and B and return result
function sortPluginByOrder(%objA,%objB) {
	if ( %objA.pluginObj.pluginOrder > %objB.pluginObj.pluginOrder)
		return "1";

	if ( %objA.pluginObj.pluginOrder < %objB.pluginObj.pluginOrder)
		return "-1";

	return "0";
}
//------------------------------------------------------------------------------
//Compare plugin order A and B and return result
function sortPluginByDefaultOrder(%objA,%objB) {
	%orderA = $Cfg_[%objA.pluginObj.plugin,"pluginOrder"];
	%orderB = $Cfg_[%objB.pluginObj.plugin,"pluginOrder"];
	if ( %orderA > %orderB)
		return "1";

	if (%orderA < %orderB)
		return "-1";

	return "0";
}
//------------------------------------------------------------------------------

//==============================================================================
//Store the current plugins order to setting (isDefault true will store defaults)
function Lab::updatePluginIconOrder(%this,%isDefault) {
	%count = LabPluginArray.getCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%icon = LabPluginArray.getObject(%i);
		%icon.pluginObj.pluginOrder = %i+1;
		%curDefault = %icon.pluginObj.getCfg("pluginOrderDefault");
		
		if (%isDefault || %curDefault $= "")
			%icon.pluginObj.setCfg("pluginOrderDefault",%icon.pluginObj.pluginOrder);

		if (!%isDefault)
			%icon.pluginObj.setCfg("pluginOrder",%icon.pluginObj.pluginOrder);
	}
}
//------------------------------------------------------------------------------
