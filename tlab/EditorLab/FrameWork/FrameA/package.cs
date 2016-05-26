//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


$FWPackage = FWPackage_FrameSetA;
package FWPackage_FrameSetA {	
   
   
    function FW::setSideBarWidth(%this,%width) {
	 EditorFrameContent.setLeftCol(%width);
}
function FW::postEditorWake(%this) {	
	
	if ($LabCfg_Layout_CurrentRightWidth $= "")
	  EditorFrameContent.setRightCol();
	  
	Lab.checkPluginTools();
	
	%this.lockEditorFrameContent(false);
}
//==============================================================================
function Lab::openSidebar(%this,%check) {
	if (LabSideBar.visible && %check){		
		return;
	}
	EditorGui-->SideBarContainer.isOpen = true;
	LabSideBar.visible = 1;

	if (EditorFrameContent.lastColumns $= "")
		EditorFrameContent.lastColumns = "0 220";
	
	EditorFrameContent.setLeftCol(EditorFrameContent.lastLeftColWidth);
	//%this.setSidebarWidth(getWord(EditorFrameContent.lastColumns,1));


	SideBarMainBook.selectPage($SideBarMainBook_CurrentPage);
	 FW.setSideBarToggleButton(%window);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::closeSidebar(%this) {
	if (EditorGui-->SideBarContainer.isOpen){
		EditorFrameContent.lastColumns = EditorFrameContent.columns;
		EditorFrameContent.lastLeftColWidth = getWord(EditorFrameContent.columns,1);
	}

	EditorGui-->SideBarContainer.isOpen = false;
	LabSideBar.visible = 0;
		EditorFrameContent.setLeftCol("18",true);
		 FW.setSideBarToggleButton(%window);
	//EditorFrameContent.columns = "0 18";
	//EditorFrameContent.updateSizes();
}
//------------------------------------------------------------------------------


   //==============================================================================
// Make sure all GUIs are fine once the editor is launched

function EditorFrameContent::onWake(%this) {
	EditorFrameContent.frameMinExtent(0,$LabCfg_Layout_LeftMinWidth,100);
	EditorFrameContent.frameMinExtent(2,$LabCfg_Layout_RightMinWidth,100);	
	if (!$EditorFrameContentInit)
	   %this.schedule(50,"init");
}

//------------------------------------------------------------------------------
function EditorFrameContent::onSleep(%this) {
	if (EditorGui-->SideBarContainer.isOpen) {
		$SideBar_CurentColumns = EditorFrameContent.columns;
		EditorFrameContent.lastColumns = EditorFrameContent.columns;
	}
}
//==============================================================================

//==============================================================================
// Make sure all GUIs are fine once the editor is launched
function EditorFrameContent::onResized(%this) {
	//EditorFrameMain.minExtent = %this.getExtent().x - 220 SPC "12";
	//%this.checkCol();
	if(isObject(FileBrowser))
		FileBrowser.onResized();
	
	if(isObject(ObjectCreator))
		ObjectCreator.onResized();
		
	if(isObject(SideBarVIS))
		SideBarVIS.onResized();
	
	if (isObject(ECamViewGui))
		ECamViewGui.checkArea();
	if (Lab.currentEditor.isMethod("onLayoutResized"))
	   Lab.currentEditor.onLayoutResized();
   
   
   %colPosZ = %this.columns.z;
   if (%colPosZ !$= ""){      
      %colWidthZ = 	%this.extent.x - %colPosZ;	
       %this.rightColumnSize = %colWidthZ;     
   }
}
};

activatePackage( FWPackage_FrameSetA );
