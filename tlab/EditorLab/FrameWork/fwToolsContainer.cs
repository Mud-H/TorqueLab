//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// TorqueLab Plugin Tools Container (Side settings area)
//==============================================================================

//==============================================================================
//Called from Toolbar and TerrainManager
function FW::setToolsWidth(%this,%width) {
   if (%width $= "")
   {
      if ($FW_ToolsWidth $= "")
         %width = $Cfg_UI_Editor_ToolFrameWidth;
      else
         %width = $FW_ToolsWidth;
   }
      
	%sideCtrl = EditorGui-->ToolsContainer;

  %sideCtrl.setExtent(%width,%sideCtrl.extent.y);
  $FW_ToolsWidth = %width;
}
//------------------------------------------------------------------------------
//==============================================================================
// Plugin GuiFrameSetCtrl Functions
//==============================================================================
//==============================================================================

//==============================================================================
//Called from Toolbar and TerrainManager
function Lab::togglePluginTools(%this) {
   %window = EditorGui-->ToolsContainer;
	if (%window.visible) {
		%this.hidePluginTools();
		return false;
//	} else if (!Lab.currentEditor.useTools) {
	//	return false;
	} else {
		%this.showPluginTools();
		return true;
	}
}
//------------------------------------------------------------------------------

//==============================================================================
//Called from EditorPlugin::activateGui
function Lab::checkPluginTools(%this) {  
   //First check if the plugins support the tools container
	%currentPlugin = Lab.currentEditor;
	if (!%currentPlugin.useTools) {
		%this.hidePluginTools();
		return;
	}
 %window = EditorGui-->ToolsContainer;
	if (!%window.visible) {
		%this.showPluginTools();
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::hidePluginTools(%this) {  
	//EditorFrameContent.lastToolsCol = getWord(EditorFrameContent.columns,2);
	 %window = EditorGui-->ToolsContainer;
	 hide(%window);
	 
	 FW.setToolsExpandButton(%window);

}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::showPluginTools(%this) {   
	%window = EditorGui-->ToolsContainer;
	 show(%window);
    FW.setToolsExpandButton(%window);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::isShownPluginTools(%this) { 

   return EditorGui-->ToolsContainer.visible;
}
//------------------------------------------------------------------------------


//==============================================================================
// EditorFrameMain  - Tool Frame (Right Column)
//==============================================================================

//==============================================================================
//Called from Toolbar and TerrainManager
function FW::postToolsContainerAdd(%this) {
	%toolCtrl = EditorGui-->ToolsContainer;
  
   %this.setToolsToggleButton(%toolCtrl);
}
//------------------------------------------------------------------------------
//==============================================================================
//Called from Toolbar and TerrainManager
function FW::setToolsToggleButton(%this,%toolCtrl) {
   
	%button = %toolCtrl->ToolsToggle;
	if (!isObject(%button))
	{
	   %button = %this.getToolsToggleButton();
	   %toolCtrl.add(%button);
	}
	%toolCtrl.pushToBack(%button);
	//%button.AlignCtrlToParent("right","4");
	//%button.AlignCtrlToParent("top","4");
	
	%button.visible = !%toolCtrl.noToggleButton;
 
}
//------------------------------------------------------------------------------
//==============================================================================
//Called from Toolbar and TerrainManager
function TEToolsCollapseBar::onClick(%this) {
	Lab.togglePluginTools();
	
}
//------------------------------------------------------------------------------
//==============================================================================
//Called from Toolbar and TerrainManager
function TEToolsCollapseButton::onClick(%this) {
	Lab.togglePluginTools();
	
}
//------------------------------------------------------------------------------
//==============================================================================
//Called from Toolbar and TerrainManager
function FW::getToolsToggleButton(%this) {
   
	%button =  new GuiContainer() {
      docking = "Left";
      margin = "0 0 0 0";
      padding = "0 0 0 0";
      anchorTop = "1";
      anchorBottom = "0";
      anchorLeft = "1";
      anchorRight = "0";
      position = "3 23";
      extent = "10 792";
      minExtent = "8 2";
      horizSizing = "right";
      vertSizing = "bottom";
      profile = "GuiDefaultProfile";
      visible = "1";
      active = "1";
      tooltipProfile = "GuiToolTipProfile";
      hovertime = "1000";
      isContainer = "1";
      internalName = "ToolsToggle";
      canSave = "1";
      canSaveDynamicFields = "0";

      new GuiButtonCtrl() {
         groupNum = "-1";
         buttonType = "PushButton";
         useMouseEvents = "0";
         position = "0 0";
         extent = "10 820";
         minExtent = "8 2";
         horizSizing = "right";
         vertSizing = "bottom";
         profile = "ToolsButtonProfile";
         visible = "1";
         active = "1";
         tooltipProfile = "GuiToolTipProfile";
         hovertime = "1000";
         isContainer = "1";
         superClass = "TEToolsCollapseBar";
         canSave = "1";
         canSaveDynamicFields = "0";

         new GuiIconButtonCtrl() {
            buttonMargin = "1 1";
            iconBitmap = "tlab/art/icons/24-assets/arrow_triangle_right.png";
            iconLocation = "Center";
            sizeIconToButton = "1";
            makeIconSquare = "0";
            textLocation = "Center";
            textMargin = "4";
            autoSize = "0";
            groupNum = "-1";
            buttonType = "PushButton";
            useMouseEvents = "0";
            position = "0 385";
            extent = "10 49";
            minExtent = "8 2";
            horizSizing = "right";
            vertSizing = "center";
            profile = "ToolsButtonArray";
            visible = "1";
            active = "1";
            tooltipProfile = "GuiToolTipProfile";
            hovertime = "1000";
            isContainer = "0";
            superClass = "TEToolsCollapseButton";
            canSave = "1";
            canSaveDynamicFields = "0";
         };
      };
   };
   return %button;
}
//------------------------------------------------------------------------------

//==============================================================================
//Called from Toolbar and TerrainManager
function FW::setToolsExpandButton(%this,%toolCtrl) {
	%container = %toolCtrl.getParent();
	%button = %container->ToolsExpander;
	if (!isObject(%button))
	{
	   %button = %this.getToolsExpandButton();
	   %container.add(%button);
	}
	%container.pushToBack(%button);
	//%button.AlignCtrlToParent("right","4");
	//%button.AlignCtrlToParent("top","4");
	%button.visible = !%toolCtrl.visible;
}
//------------------------------------------------------------------------------
//==============================================================================
//Called from Toolbar and TerrainManager
function TEToolsCollapseBar::onClick(%this) {
	Lab.togglePluginTools();
	
}
//------------------------------------------------------------------------------
//==============================================================================
//Called from Toolbar and TerrainManager
function TEToolsCollapseButton::onClick(%this) {
	Lab.togglePluginTools();
	
}
//------------------------------------------------------------------------------
//==============================================================================
//Called from Toolbar and TerrainManager
function TEToolsExpandBar::onClick(%this) {
	Lab.togglePluginTools();
	
}
//------------------------------------------------------------------------------
//==============================================================================
//Called from Toolbar and TerrainManager
function TEToolsExpandIcon::onClick(%this) {
	Lab.togglePluginTools();
	
}
//------------------------------------------------------------------------------
//==============================================================================
//Called from Toolbar and TerrainManager
function FW::getToolsExpandButton(%this) {
   
	%button =  new GuiContainer() {
      docking = "Right";
      margin = "0 0 0 0";
      padding = "0 0 0 0";
      anchorTop = "1";
      anchorBottom = "0";
      anchorLeft = "1";
      anchorRight = "0";
      position = "1071 0";
      extent = "10 820";
      minExtent = "8 2";
      horizSizing = "right";
      vertSizing = "bottom";
      profile = "GuiDefaultProfile";
      visible = "1";
      active = "1";
      tooltipProfile = "GuiToolTipProfile";
      hovertime = "1000";
      isContainer = "1";
      internalName = "ToolsExpander";
      canSave = "1";
      canSaveDynamicFields = "0";

      new GuiButtonCtrl() {
         groupNum = "-1";
         buttonType = "PushButton";
         useMouseEvents = "0";
         position = "0 0";
         extent = "10 820";
         minExtent = "8 2";
         horizSizing = "right";
         vertSizing = "bottom";
         profile = "ToolsButtonProfile";
         visible = "1";
         active = "1";
         tooltipProfile = "GuiToolTipProfile";
         hovertime = "1000";
         isContainer = "1";
         superClass = "TEToolsExpandBar";
         canSave = "1";
         canSaveDynamicFields = "0";

         new GuiIconButtonCtrl(f1) {
            buttonMargin = "1 1";
            iconBitmap = "tlab/art/icons/24-assets/arrow_triangle_left.png";
            iconLocation = "Center";
            sizeIconToButton = "1";
            makeIconSquare = "0";
            textLocation = "Center";
            textMargin = "4";
            autoSize = "0";
            groupNum = "-1";
            buttonType = "PushButton";
            useMouseEvents = "0";
            position = "0 385";
            extent = "10 49";
            minExtent = "8 2";
            horizSizing = "right";
            vertSizing = "center";
            profile = "ToolsButtonArray";
            visible = "1";
            active = "1";
            tooltipProfile = "GuiToolTipProfile";
            hovertime = "1000";
            isContainer = "0";
            superClass = "TEToolsExpandIcon";
            canSave = "1";
            canSaveDynamicFields = "0";
         };
      };
   };
   return %button;
}
//------------------------------------------------------------------------------

