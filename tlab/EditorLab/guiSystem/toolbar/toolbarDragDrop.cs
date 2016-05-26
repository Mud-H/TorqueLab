//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Toolbar drag controls callbacks
//==============================================================================

//==============================================================================
// Start dragging a toolbar icon
function ToolbarIcon::onMouseDragged( %this,%a1,%a2,%a3 ) {
	Lab.startToolbarDrag(%this,%a1,%a2,%a3);
}
//------------------------------------------------------------------------------


//==============================================================================
// Start dragging a toolbar icons group
function ToolbarBoxChild::onMouseDragged( %this,%a1,%a2,%a3 ) {
	%src = %this.parentGroup;
	Lab.startToolbarDrag(%src,%a1,%a2,%a3);
}
//------------------------------------------------------------------------------

//==============================================================================
// Start dragging a toolbar icons group
function MouseStart::onMouseDragged( %this,%a1,%a2,%a3 ) {
	%group = %this.parentGroup.parentGroup;

	if (%group.getClassName() !$= "GuiStackControl")
		return;

	Lab.startToolbarDrag(%group,%a1,%a2,%a3);
}
//------------------------------------------------------------------------------
//==============================================================================
// Start dragging a toolbar icons group
function MouseBox::onMouseDragged( %this,%a1,%a2,%a3 ) {
	%group = %this.iconStack;
	Lab.startToolbarDrag(%group,%a1,%a2,%a3);
}
//------------------------------------------------------------------------------
//==============================================================================
// OLD: Replaced with MouseStart
function ToolbarGroup::onMouseDragged( %this,%a1,%a2,%a3 ) {
	%group = %this.parentGroup.parentGroup;

	if (%group.getClassName() !$= "GuiStackControl")
		return;

	Lab.startToolbarDrag(%group,%a1,%a2,%a3);
}
//------------------------------------------------------------------------------
//==============================================================================
// Start dragging a toolbar icons group
function Lab::startToolbarDrag( %this,%ctrl,%a1,%a2,%a3 ) {
	if (!isObject(%this)) {
		devLog("ToolbarBoxChild class is corrupted! Fix it!");
		return;
	}

	if (%ctrl.parentGroup.superClass !$= "EToolbarTrashClass") {
		%ctrl.originalParent = %ctrl.parentGroup;
		%ctrl.originalIndex = %ctrl.parentGroup.getObjectIndex(%ctrl);
	}
   
	if (%ctrl.getClassName() $= "GuiStackControl") {
		%callback = "Lab.onToolbarGroupDroppedDefault";
	} else {
		%callback = "Lab.onToolbarIconDroppedDefault";
	}
  
	startDragAndDropCtrl(%ctrl,"Toolbar",%callback);
	hide(%ctrl);
	Lab.showDisabledToolbarDropArea(	%ctrl);
}
//------------------------------------------------------------------------------


//==============================================================================

//==============================================================================
//==============================================================================
// Dragged control dropped over undefined control (gonna check if it's droppable)
function Lab::findObjectToolbarStack(%this,%checkObj,%defaultPluginStack) {
	%checkId = 0;

	while(%checkId < 5) {
		%result = %this.checkObjForToolbarStack(%checkObj,%defaultPluginStack);

		if (%result && %defaultPluginStack)
			return %checkObj SPC %previousObj;
		else if (%result)
			return %checkObj;

		%previousObj = %checkObj;
		%checkObj = %checkObj.parentGroup;
		%checkId++;
	}

	return false;
}
//------------------------------------------------------------------------------

//==============================================================================
// Dragged control dropped over undefined control (gonna check if it's droppable)
function Lab::checkObjForToolbarStack(%this,%object,%defaultStack) {
	if (!isObject(%object))
		return false;

	if (%object.getClassname() $= "GuiStackControl") {
		if (%defaultStack && %object.superClass $= "ToolbarPluginStack")
			return true;
		else if (!%defaultStack && %object.superClass $= "ToolbarContainer")
			return true;
		else if (!%defaultStack && %object.superClass $= "PluginToolbarEnd")
			return true;
	}

	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
// Dragged control dropped over undefined control (gonna check if it's droppable)
function Lab::addToolbarItemToGroup(%this,%item,%group,%addBefore) {
	%divider = %group-->GroupDivider;

	if (isObject(%divider)) {
		%divider.extent.x = "6";

		foreach(%obj in %divider) {
			if (%obj.getClassName() $= "GuiMouseEventCtrl")
				%obj.extent.x = "6";
		}
	}
  
   if (isObject(%item.srcIcon)){
      %original = %item.srcIcon;
      delObj(%item);
   }
   else 
   {
     %original = %item;  
   }
      
	//%original = %this.getTrashedOriginalIcon(%item);

	
   %original.srcIcon = "";

	%group.add(%original);
	%group.updateStack();
	%group.reorderChild(%original,%addBefore);
	%item.originalParent = %group;
	%item.originalIndex = %group.getObjectIndex(%original);

	Lab.setToolbarDirty(%group);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::showDisabledToolbarDropArea(%this,%dragControl) {
	%isIcon = false;
	//Show the Trash Drop Zone
	show(EToolbarTrashDropZone);

	if (%dragControl.isMemberOfClass("GuiIconButtonCtrl"))
		%isIcon = true;

	EToolbarDisabledDrop.visible = 1;
	%startStack = LabToolbarStack-->FirstToolbarGroup;
	LabToolbarStack.bringToFront(%startStack);
	%lastStack = LabToolbarStack-->LastToolbarGroup;
	LabToolbarStack.pushToBack(%lastStack);

	//Only show StackEnd for groups
	if(%isIcon) {
		foreach(%gui in LabToolbarStack) {
			foreach(%stack in %gui) {
				%stackEnd = %stack-->SubStackEnd;

				if (!isObject(%stackEnd))
					continue;
            if (%stackEnd.toolbarName $= %dragControl.toolbarName)
                show(%stackEnd);
             else
               hide(%stackEnd);
            
				
				%stack.pushToBack(%stackEnd);
				%stackEnd.profile = "ToolsBoxDarkC";
				%stackEnd.setExtent(18,%gui.parentGroup.extent.y);
			}
		}
	} else {
		foreach(%gui in LabToolbarStack) {
			%stackEnd = %gui-->StackEnd;

			if (!isObject(%stackEnd))
				continue;

         if (%stackEnd.toolbarName $= %dragControl.toolbarName)
            show(%stackEnd);
         else
            hide(%stackEnd);
			%gui.pushToBack(%stackEnd);
			
			%stackEnd.profile = "ToolsBoxDarkC";
			%stackEnd.setExtent(27,%gui.parentGroup.extent.y);
		}
	}

	foreach(%gui in EToolbarDisabledDrop) {
		if (%gui.toolbarName !$= %dragControl.toolbarName) {
			%src = %gui.internalName;

			if (%src.visible $= "0" || !%src.visible)
				continue;

			%gui.visible = 1;
			%gui.setExtent(%src.extent.x,EditorGuiToolbar.extent.y);
			%gui.setPosition(%src.position.x,0);
		}
		else{
		   %gui.visible = 0;
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::hideDisabledToolbarDropArea(%this) {
	//Hide the Trash Drop Zone
	hide(EToolbarTrashDropZone);
	hide(LabToolbarStack-->ToolbarStart);
	hide(LabToolbarStack-->ToolbarEnd);

	foreach(%gui in LabToolbarStack) {
		%stackEnd = %gui-->StackEnd;

		if (isObject(%stackEnd))
			hide(%stackEnd);

		//foreach$(%stack in %gui.stackLists) {
		foreach(%subCtrl in %gui) {
			%subStackEnd = %subCtrl-->SubStackEnd;

			if (isObject(%subStackEnd))
				hide(%subStackEnd);
		}
	}

	foreach(%gui in EToolbarDisabledDrop)
		%gui.visible = 0;

	EToolbarDisabledDrop.visible = 0;
}
//------------------------------------------------------------------------------
