//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


$FWPackage = FWPackage_FWC;
package FWPackage_FWC {	
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

//==============================================================================
// Plugin GuiFrameSetCtrl Functions
//==============================================================================
//==============================================================================

//==============================================================================
//Called from Toolbar and TerrainManager
function Lab::togglePluginTools(%this) {
	if (getWordCount(EditorFrameContent.columns) > 2) {
		%this.hidePluginTools();
		return false;
	} else if (!Lab.currentEditor.useTools) {
		return false;
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
	 hide(window);
	 
	 FW.setToolsExpandButton(%window);

}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::showPluginTools(%this) {   
	%window = EditorGui-->ToolsContainer;
	 show(window);
    FW.setToolsExpandButton(%window);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::isShownPluginTools(%this) { 

   return EditorGui-->ToolsContainer.visible;
}
//------------------------------------------------------------------------------

};

activatePackage( FWPackage_FWC );
