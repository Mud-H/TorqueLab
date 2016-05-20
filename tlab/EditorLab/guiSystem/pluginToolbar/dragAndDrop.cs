//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function LabPluginBar::checkIconDrop( %this,%droppedOn,%icon,%position ) {
	%originalCtrl = %icon.dragSourceControl;
	if (%originalCtrl.class $= "PluginIconDisabled") {
		Lab.enablePlugin(	%originalCtrl.pluginObj);
	}

	if (%icon.parentGroup.dropType !$= "PluginIcon") {
		warnLog("Only plugins icons can be drop in the Plugin Bar");
		Parent::onControlDropped( %this,%icon,%position );
		return false;
	}
	return true;
}

//==============================================================================
// Plugin Bar Drag and Drop
//==============================================================================

//==============================================================================
function PluginIcon::onMouseDragged( %this,%a1,%a2,%a3 ) {
	if (!isObject(%this)) {
		devLog("PluginIcon class is corrupted! Fix it!");
		return;
	}

	startDragAndDropCtrl(%this,"PluginIcon");
	
	hide(%this);
	LabPluginArray.refresh();
	Lab.openDisabledPluginsBin(true);
	Lab.refreshPluginToolbar();
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginIcon::DragFailed( %this ) {
	Lab.closeDisabledPluginsBin(true);
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginIcon::DragSuccess( %this ) {
	Lab.updatePluginIconContainer();
	Lab.closeDisabledPluginsBin(true);
	%this.active = 1;
	if (!%this.pluginObj.isEnabled) {
		warnLog("Disabled plugin icon dropped, set it enabled:",%this.pluginObj.plugin);
		Lab.enablePlugin(%this.pluginObj);
	}

	Lab.refreshPluginToolbar();
}
//------------------------------------------------------------------------------

//==============================================================================
function PluginIconContainer::onControlDropped( %this,%ctrl,%position ) {
	%originalCtrl = %ctrl.dragSourceControl;
	if (%originalCtrl.class !$= "PluginIcon")
		return;		

	devLog(" PluginIconContainer::onControlDropped");

	//Simply remove it and add it so it go to end
	%this.remove(%originalCtrl);
	%this.add(%originalCtrl);
	show(%originalCtrl);
	delObj(%ctrl);
	
	%originalCtrl.DragSuccess();
	Lab.refreshPluginToolbar();
}
//------------------------------------------------------------------------------

//==============================================================================
function PluginIcon::onControlDropped( %this,%ctrl,%position ) {
	%originalCtrl = %ctrl.dragSourceControl;

	if (%originalCtrl.class !$= "PluginIcon")
		return;		

devLog(" PluginIcon::onControlDropped");
	//Let's add it just before this
	//%this.parentGroup.add(%ctrl);
	show(%originalCtrl);

	if (!LabPluginArray.isMember(%originalCtrl))
		LabPluginArray.add(%originalCtrl);

	LabPluginArray.reorderChild(%originalCtrl,%this);
	delObj(%ctrl);
	
	%originalCtrl.DragSuccess();
	Lab.refreshPluginToolbar();
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginBar::onControlDropped( %this,%ctrl,%position ) {
	%originalCtrl = %ctrl.dragSourceControl;
%isValid = LabPluginBar.checkIconDrop(%this,%ctrl,%position);
	if (!%isValid)
		return;

	devLog(" PluginBar::onControlDropped");
	//Let's add it just before this
	//%this.parentGroup.add(%ctrl);
	show(%originalCtrl);	
	
	if (!LabPluginArray.isMember(%originalCtrl))
		LabPluginArray.add(%originalCtrl);

	LabPluginArray.pushToBack(%originalCtrl);
	//LabPluginArray.reorderChild(%originalCtrl,%this);
	delObj(%ctrl);
	%originalCtrl.DragSuccess();
	Lab.refreshPluginToolbar();
}
//------------------------------------------------------------------------------


//=============================================================è=================
function DisabledPluginIcon::onMouseDragged( %this,%a1,%a2,%a3 ) {
	startDragAndDropCtrl(%this,"PluginIcon");
	hide(%this);
	Lab.openDisabledPluginsBin(true);
}
//------------------------------------------------------------------------------




//==============================================================================
// DisabledPlugin Bar Drag and Drop
//==============================================================================

//==============================================================================
function PluginIconDisabled::onMouseDragged( %this,%a1,%a2,%a3 ) {
	if (!isObject(%this)) {
		devLog("PluginIcon class is corrupted! Fix it!");
		return;
	}

	startDragAndDropCtrl(%this,"PluginIcon");
	hide(%this);
	Lab.openDisabledPluginsBin(true);
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginIconDisabled::DragFailed( %this ) {
	Lab.closeDisabledPluginsBin(true);
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginIconDisabled::DragSuccess( %this ) {
	Lab.updatePluginIconContainer();
	Lab.closeDisabledPluginsBin(true);
}
//------------------------------------------------------------------------------

//==============================================================================
function DisabledPluginsBox::onControlDropped( %this,%ctrl,%position ) {
	%originalIcon = %ctrl.dragSourceControl;

	if (%ctrl.parentGroup.dropType !$= "PluginIcon") {
		warnLog("Only plugins icons can be drop in the Plugin Bar");
		return;
	}
	%this.add(%originalIcon);
	show(%originalIcon);
	delObj(%ctrl);

	//%this.add(%originalIcon);
	info(%originalIcon.internalName," dropped in DisabledPluginsBox and should be set disabled",%originalIcon.pluginObj);
	Lab.disablePlugin(%originalIcon.pluginObj);
	
}
//------------------------------------------------------------------------------