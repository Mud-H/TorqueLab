//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


$FWPackage = FWPackage_Stock;
package FWPackage_Stock {	
   //Called from Toolbar and TerrainManager
function FW::postToolsContainerAdd(%this) {  
	%toolCtrl = EditorGui-->ToolsContainer;
  %toolCtrl.AlignCtrlToParent("right","0");
	%toolCtrl.AlignCtrlToParent("top","0");
   %this.setToolsToggleButton(%toolCtrl);
}
function FW::postSideBarContainerAdd(%this) {
   %sideCtrl = EditorGui-->SideBarContainer;
   %sideCtrl.extent.y = %sideCtrl.getParent().extent.y - 100;
    %sideCtrl.AlignCtrlToParent("left","0");
	%sideCtrl.AlignCtrlToParent("bottom","0");
	
}
	//==============================================================================
   function Lab::openSidebar(%this,%check) {
      devLog("FWC open isdebar");
      //Set Window default extents
      %window = EditorGui-->SideBarContainer;
      if (%window.openExtent !$= "")
         %window.setExtent(%window.openExtent);      
     
      show(LabSideBar);
      // FW.setSideBarToggleButton(%window);
    
   }
   //------------------------------------------------------------------------------
   //==============================================================================
   function Lab::closeSidebar(%this) {
       devLog("FWC closeSidebar isdebar");
      %window = EditorGui-->SideBarContainer;
      %window.openExtent = %window.extent;
      
       %window.setExtent("18",%window.extent.y);
        hide(LabSideBar);      
      
     // FW.setSideBarToggleButton(%window);
   }
   //------------------------------------------------------------------------------

//==============================================================================
// Plugin GuiFrameSetCtrl Functions
//==============================================================================
//==============================================================================



};

activatePackage( FWPackage_Stock );
