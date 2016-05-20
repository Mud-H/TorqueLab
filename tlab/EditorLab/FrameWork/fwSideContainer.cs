//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Plugin GuiFrameSetCtrl Functions
//==============================================================================
//==============================================================================
//==============================================================================
//Called from Toolbar and TerrainManager
function FW::setSideBarWidth(%this,%width) { 
   if (%width $= "")
   {
      if ($FW_SideBarWidth $= "")
         %width = $Cfg_UI_Editor_SideFrameWidth;
      else
         %width = $FW_SideBarWidth;
   }      
	%sideCtrl = EditorGui-->SideBarContainer;
  %sideCtrl.setExtent(%width,%sideCtrl.extent.y);
  $FW_SideBarWidth = %width;
}
//------------------------------------------------------------------------------

//==============================================================================
//Called from Toolbar and TerrainManager
function FW::initSideContainer(%this) {
	
}
//------------------------------------------------------------------------------

//==============================================================================
//Called from Toolbar and TerrainManager
function FW::postSideContainerAdd(%this) {
	%sideCtrl = EditorGui-->SideBarContainer;

   %this.setSideBarToggleButton(%sideCtrl);
}
//------------------------------------------------------------------------------
//==============================================================================
//Called from Toolbar and TerrainManager
function FW::setSideBarToggleButton(%this,%sideCtrl) {
	
	%button = %sideCtrl->SideBarToggle;
	if (!isObject(%button))
	{
	   %button = %this.getSideToggleButton();
	   %sideCtrl.add(%button);
	}
	%sideCtrl.pushToBack(%button);
	%button.AlignCtrlToParent("right","4");
	%button.AlignCtrlToParent("top","4");
	%button.visible = 1;
}
//------------------------------------------------------------------------------
//==============================================================================
//Called from Toolbar and TerrainManager
function FW::getSideToggleButton(%this) {
   
	%button =  new GuiBitmapButtonCtrl() {
      bitmap = "tlab/art/icons/24-assets/arrow_triangle_right.png";
      bitmapMode = "Stretched";
      autoFitExtents = "0";
      useModifiers = "0";
      useStates = "1";
      groupNum = "-1";
      buttonType = "PushButton";
      useMouseEvents = "0";
      position = "188 0";
      extent = "11 15";
      minExtent = "8 2";
      horizSizing = "left";
      vertSizing = "bottom";
      profile = "GuiDefaultProfile";
      command = "Lab.toggleSidebar();";
      tooltipProfile = "GuiToolTipProfile";
      internalName = "SideBarToggle";
     
   };
   return %button;
}
//------------------------------------------------------------------------------
  