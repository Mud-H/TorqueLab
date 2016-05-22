//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


$FWPackage = FWPackage_FWD;
package FWPackage_FWD {	
	//==============================================================================
   function Lab::openSidebar(%this,%check) {
      devLog("FWC open isdebar");
      //Set Window default extents
      %window = EditorGui-->SideBarContainer;
      if (%window.openExtent !$= "")
         %window.setExtent(%window.openExtent);      
     
      show(LabSideBar);
       FW.setSideBarToggleButton(%window);
    
   }
   //------------------------------------------------------------------------------
   //==============================================================================
   function Lab::closeSidebar(%this) {
       devLog("FWC closeSidebar isdebar");
      %window = EditorGui-->SideBarContainer;
      %window.openExtent = %window.extent;
      
       %window.setExtent("18",%window.extent.y);
        hide(LabSideBar);      
      
      FW.setSideBarToggleButton(%window);
   }
   //------------------------------------------------------------------------------

};

activatePackage( FWPackage_FWD );
