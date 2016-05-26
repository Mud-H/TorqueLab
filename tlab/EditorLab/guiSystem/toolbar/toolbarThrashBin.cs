//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Toolbar Dropping a dragged item callbacks
//==============================================================================

//==============================================================================
// Dragged control dropped in Toolbar Trash Bin
function EToolbarTrashClass::onControlDropped(%this, %control, %dropPoint) {
	Lab.checkTrashDroppedCtrl(%control);
}
//------------------------------------------------------------------------------


//==============================================================================
// Dragged control dropped in Toolbar Trash Bin
function Lab::checkTrashDroppedCtrl(%this, %control) {
	devLog("Lab::checkTrashDroppedCtrl(%this, %control)",%this, %control);
	%droppedCtrl = %control.dragSourceControl;
	Lab.hideDisabledToolbarDropArea(	);
	show(%droppedCtrl);

	if (%control.dropType !$= "Toolbar") {
		warnLog("Toolbar thrash dropped invalid droptype ctrl:",%control);
		return;
	}

	if (%droppedCtrl.isMemberOfClass("GuiStackControl"))
		%this.addStackToGuiTrash(%droppedCtrl);
	else
		%this.addCtrlToGuiTrash(%droppedCtrl);
}
//------------------------------------------------------------------------------
//==============================================================================
// Dragged control dropped in Toolbar Trash Bin
function Lab::addStackToGuiTrash(%this,%ctrl) {
	for(%i = %ctrl.getCount()-1; %i >= 0; %i--) {
		%obj = %ctrl.getObject(%i);

		if (strFindWords(%obj.internalName,"DisabledIcons StackEnd GroupDivider"))
			continue;

		%this.addCtrlToGuiTrash(%obj);
	}

	%stack = %ctrl.toolbar;
	delObj(%ctrl);

	if (isObject(%stack))
		Lab.updateToolbarStack(%stack);
	else
		Lab.updateAllToolbarStacks();
}
//------------------------------------------------------------------------------
//==============================================================================
// Dragged control dropped in Toolbar Trash Bin
function Lab::addCtrlToGuiTrash(%this, %ctrl,%alreadyCloned) {   
  
   if (!%alreadyCloned)
	   Lab.setToolbarDirty(%ctrl.toolbarName );

	if (%ctrl.isMemberOfClass("GuiIconButtonCtrl"))
		%container = EToolbarIconTrash;
	else
		%container = EToolbarBoxTrash;

	if (%alreadyCloned) {
		%container.add(%ctrl);
		return;
	}

	%result = Lab.setToolbarCtrlDisabled(%ctrl,"1");

	if (%result) {
		%trashCtrl = %ctrl.deepClone();
		%trashCtrl.srcIcon = %ctrl;
	} else
		%trashCtrl = %ctrl;

	%container.add(%trashCtrl);
}
//------------------------------------------------------------------------------
//==============================================================================
// Dragged control dropped in Toolbar Trash Bin
function Lab::getTrashedOriginalIcon(%this,  %ctrl) {
	//%disabledGroup = %ctrl.toolbar-->DisabledIcons;
	if (!isObject(%ctrl.srcIcon))
		return %ctrl;

	%originalIcon = %ctrl.srcIcon;
	delObj(%ctrl);
	return %originalIcon;
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setToolbarCtrlDisabled(%this,%ctrl) {
	%icon.canSaveDynamicFields = true;

	if (!isObject( %ctrl.toolbar)) {
		%toolbar = %this.getIconRootStack(%ctrl);

		if (!isObject(%toolbar))
			return false;

		%ctrl.toolbar = %toolbar;
	}

	%disabledGroup = %ctrl.toolbar-->DisabledIcons;

	if (!isObject(%disabledGroup))
		return false;

	//if (%ctrl.internalName $="")
	//%ctrl.internalName = getUniqueInternalName(%ctrl.toolbar.getName()@"_Icon",%disabledGroup,true);
//	%clone = %ctrl.deepClone();
//	%clone.srcIcon = %ctrl;
	%disabledGroup.add(%ctrl);
	return true;
	/*
	return;



	if (%icon.locked $= %disabled)
		return;

	EGuiCustomizer-->saveToolbar.active = true;
	%icon.locked = %disabled;

	if (!%disabled)
		return;

	%disabledGroup = %icon.toolbar-->DisabledIcons;

	if (%disabled && isObject(%disabledGroup))
		%disabledGroup.add(%icon);*/
}
//------------------------------------------------------------------------------
//==============================================================================
// Dragged control dropped in Toolbar Trash Bin
function Lab::initToolbarTrash(%this,%showAll) {
	EToolbarIconTrash.clear();
	EToolbarBoxTrash.clear();

	foreach(%gui in LabToolbarStack) {
		%disabledGroup = %gui-->DisabledIcons;

		if (!isObject(%disabledGroup))
			continue;

		for(%i=%disabledGroup.getCount()-1; %i >=0; %i--) {
			%ctrl = %disabledGroup.getObject(%i);
			%ctrl.toolbarName = %gui.toolbarName;
			%clone = %ctrl.deepClone();
			%clone.srcCtrl = %icon;
         %clone.srcIcon = %ctrl;
        
			if (%showAll)
				show(%clone);
			else
				hide(%clone);

			%this.addCtrlToGuiTrash(%clone,true);
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Dragged control dropped in Toolbar Trash Bin
function Lab::initToolbarPluginTrash(%this) {
	foreach(%plugin in LabPluginGuiSet)
		Lab.setToolbarPluginTrash(%plugin); 
}

//==============================================================================
// Dragged control dropped in Toolbar Trash Bin
function Lab::setToolbarPluginTrash(%this, %pluginObj) {
	for(%i=EToolbarIconTrash.getCount()-1; %i >=0; %i--) {
		%ctrl = EToolbarIconTrash.getObject(%i);

		if (%ctrl.pluginName !$= %pluginObj.plugin && %ctrl.pluginName !$= "")
			hide(%ctrl);
		else
			show(%ctrl);
	}

	for(%i=EToolbarBoxTrash.getCount()-1; %i >=0; %i--) {
		%ctrl = EToolbarBoxTrash.getObject(%i);

		if (%ctrl.pluginName !$= %pluginObj.plugin && %ctrl.pluginName !$= "")
			hide(%ctrl);
		else
			show(%ctrl);
	}
}
//------------------------------------------------------------------------------
