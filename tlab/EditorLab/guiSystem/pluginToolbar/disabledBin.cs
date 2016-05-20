//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function Lab::openDisabledPluginsBin( %this,%removeOnly ) {
	%bin = EditorGui-->pluginBarTrash;

	if (%bin.isVisible())
		return;

	%bin.position.y = LabPluginBar.extent.y + 4;

	if (%removeOnly) {
		%bin.extent.y = %bin.lowBox;
	} else {
		%bin.extent.y = %bin.hiBox;
	}

	show(%bin);
}
//------------------------------------------------------------------------------
//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function Lab::closeDisabledPluginsBin( %this,%autoClose ) {
	%bin = EditorGui-->pluginBarTrash;

	if (%autoClose && %bin.extent.y $= %bin.hiBox) {
		warnLog("The Disabled plugin box is in full mode and must be close with button");
		return;
	}

	hide(%bin);
}
//------------------------------------------------------------------------------
//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function Lab::clearDisabledPluginsBin( %this ) {
	%bin = EditorGui-->DisabledPluginsBox;
	%bin.deleteAllObjects();
}
//------------------------------------------------------------------------------
//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function Lab::updatePluginIconContainer( %this,%type ) {
	if (%type $= "")
		%doBoth = true;

	if (%type $= "Enabled" || %doBoth) {
		LabPluginArray.refresh();
		Lab.refreshPluginToolbar();
	}

	if (%type $= "Disabled" || %doBoth) {
		EditorGui-->DisabledPluginsBox.refresh();
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Add the various TorqueLab GUIs to the container and set they belong
function Lab::addPluginIconToDisabledBin(%this,%originalIcon,%trashedIcon) {
	hide(%originalIcon);
	delObj(%trashedIcon);
	Lab_DisabledPluginsBox.add(%originalIcon);
	%originalIcon.class =  "PluginIconDisabled";
	info(%originalIcon.internalName," dropped in DisabledPluginsBox and should be set disabled",%originalIcon.pluginObj);
	Lab.disablePlugin(%originalIcon.pluginObj);
	Lab.refreshPluginToolbar();
}
//------------------------------------------------------------------------------


//==============================================================================
// Add the various TorqueLab GUIs to the container and set they belong
function Lab::onToolbarPluginIconDropped(%this,%originalIcon) {
	Lab.enablePlugin(%originalIcon.pluginObj);
	Lab.refreshPluginToolbar();
}
//------------------------------------------------------------------------------

