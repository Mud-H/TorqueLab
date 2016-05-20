//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//------------------------------------------------------------------------------
// Called from onInitialEditorLaunch to set the initial sidebar states
function Lab::initSideBar(%this) {	
	
   $SideBarVIS_Initialized = 0;
   timerStepStart("InitSideBar","SideBar");
   timerStep("registerObjects","SideBar");   
   Scene.registerObjects();
   
 
	timerStep("SideBarVIS","SideBar");   
	SideBarVIS.init();
	SideBarVIS.onResized();
	 
 
	 timerStep("initFileBrowser","SideBar");  
	Lab.initFileBrowser();

	 timerStep("initObjectCreator","SideBar");  
	Lab.initObjectCreator();
	
	
	FileBrowser.onResized();
	 
	SideBarMainBook.selectPage($SideBarMainBook_CurrentPage);
	 timerStep("bringToFront","SideBar");  
	EditorGui-->SideBarContainer.bringToFront(EditorSideBarCtrl);
	
	 timerStep("Done","SideBar");  
	 timerStepDump("SideBar");  
	
}
//==============================================================================
function Lab::setSidebarWidth(%this,%width) {
   FW.setSideBarWidth(%width);
   
	

}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::toggleSidebar(%this) {
	if (EditorSideBarCtrl.visible)
		Lab.closeSidebar();
	else
		Lab.openSidebar();
}
	//==============================================================================
   function Lab::openSidebar(%this,%check) {
      
      //Set Window default extents
      %window = EditorGui-->SideBarContainer;
      if (%window.openExtent !$= "")
         %window.setExtent(%window.openExtent);      
     
      show(EditorSideBarCtrl);
       FW.setSideBarToggleButton(%window);
    
   }
   //------------------------------------------------------------------------------
   //==============================================================================
   function Lab::closeSidebar(%this) {
       
      %window = EditorGui-->SideBarContainer;
      %window.openExtent = %window.extent;
      
       %window.setExtent("18",%window.extent.y);
        hide(EditorSideBarCtrl);      
      
      FW.setSideBarToggleButton(%window);
   }
   //------------------------------------------------------------------------------
