//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TLab::DefaultPlugins = "SceneEditor";

$LabCfg_Layout_LeftMinWidth = "220";
$LabCfg_Layout_RightMinWidth = "280";


 

$Cfg_TLab_LeftFrameMin = "123";
$Cfg_TLab_RightFrameMin = "123";


//==============================================================================
// Make sure all GUIs are fine once the editor is launched
function FW::onResized(%this) {
   
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



//==============================================================================
// Initialize the Editor Frames
//==============================================================================


//==============================================================================
// EditorFrame - Left-Right Column
//==============================================================================


//==============================================================================
// FROM MAIN OLD FRAME
//==============================================================================


