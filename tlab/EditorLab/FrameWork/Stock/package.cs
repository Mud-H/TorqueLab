//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


$FWPackage = FWPackage_Stock;
package FWPackage_Stock {	
	//==============================================================================
   function Lab::openSidebar(%this,%check) {
      devLog("FWC open isdebar");
      //Set Window default extents
      %window = EditorGui-->SideBarContainer;
      if (%window.openExtent !$= "")
         %window.setExtent(%window.openExtent);      
     
      show(EditorSideBarCtrl);
      // FW.setSideBarToggleButton(%window);
    
   }
   //------------------------------------------------------------------------------
   //==============================================================================
   function Lab::closeSidebar(%this) {
       devLog("FWC closeSidebar isdebar");
      %window = EditorGui-->SideBarContainer;
      %window.openExtent = %window.extent;
      
       %window.setExtent("18",%window.extent.y);
        hide(EditorSideBarCtrl);      
      
     // FW.setSideBarToggleButton(%window);
   }
   //------------------------------------------------------------------------------

//==============================================================================
// Plugin GuiFrameSetCtrl Functions
//==============================================================================
//==============================================================================



};

activatePackage( FWPackage_Stock );
