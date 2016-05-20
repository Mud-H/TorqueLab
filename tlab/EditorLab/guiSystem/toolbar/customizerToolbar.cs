//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Toolbar Dropping a dragged item callbacks
//==============================================================================

//==============================================================================
// Create a new Icon Group to be dragged in Toolbar
//==============================================================================
//==============================================================================
// Start dragging a new icons group to be added to toolbar
function NewToolbarGroup::onMouseDragged( %this,%a1,%a2,%a3 ) {
	if (!isObject(%this)) {
		devLog("ToolbarGroup class is corrupted! Fix it!");
		return;
	}

	%group = %this.parentGroup;
	%group.isNewGroup = true;
	startDragAndDropCtrl(%group,"Toolbar","Lab.onNewToolbarGroupDropped");
	//Lab.showDisabledToolbarDropArea(	%group);
}
//------------------------------------------------------------------------------

//==============================================================================
// Dragged control dropped over undefined control (gonna check if it's droppable)
function Lab::createNewToolbarIconGroup(%this) {
	%newGroup = new GuiStackControl() {
		stackingType = "Horizontal";
		horizStacking = "Left to Right";
		vertStacking = "Top to Bottom";
		padding = "2";
		dynamicSize = "1";
		dynamicNonStackExtent = "0";
		dynamicPos = "0";
		changeChildSizeToFit = "1";
		changeChildPosition = "1";
		position = "472 0";
		extent = "68 30";
		minExtent = "4 16";
		profile = "ToolsFillDarkB";
		visible = "1";
		active = "1";
		internalName = "NewToolbarGroup";
		superClass = "ToolbarContainer";
		new GuiContainer() {
			position = "0 0";
			extent = "16 30";
			minExtent = "4 2";
			profile = "ToolsFillDarkB";
			visible = "1";
			active = "1";
			tooltipProfile = "ToolsToolTipProfile";
			internalName = "GroupDivider";
			new GuiBitmapCtrl() {
				bitmap = "tlab/art/icons/toolbar/GroupDivider.png";
				wrap = "0";
				position = "1 6";
				extent = "4 18";
				minExtent = "4 2";
				profile = "ToolsDefaultProfile";
				visible = "1";
				active = "1";
			};
			new GuiMouseEventCtrl() {
				extent = "16 30";
				minExtent = "8 2";
				visible = "1";
				active = "1";
				superClass = "MouseStart";
				internalName = "MouseStart";
			};
		};
		new GuiContainer() {
			extent = "18 32";
			profile = "ToolsBoxDarkC";
			isContainer = "1";
			internalName = "SubStackEnd";
			superClass = "SubStackEnd";
			visible = "0";
			new GuiBitmapCtrl() {
				bitmap = "tlab/art/icons/toolbar/DropLeft.png";
				wrap = "0";
				position = "4 7";
				extent = "12 16";
				internalName = "SubStackEndImg";
				superClass = "SubStackEndImg";
			};
		};
	};
	%name = EGC_ToolbarPage-->newGroupName.getText();

	if (%name !$= "")
		%newGroup.internalName = %name;

	return %newGroup;
}
