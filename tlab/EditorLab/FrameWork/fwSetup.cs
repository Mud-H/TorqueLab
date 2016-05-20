//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$AutoFrameWork = 0;
$NoDefaultFrameWork = 1;
$DefaultFrameWork = "Default";
$FrameWork_MainChildrens = "FullEditorContainer EditorContainer ExtraContainer DialogContainer ToolsContainer SideBarContainer";
$FW_MainCoreGuis = "EWorldEditor ETerrainEditor LabPaletteBar LabPluginBar";
	function GuiControl::onResized( %this ) {
	    warnLog("Onresized! ");
	}
//==============================================================================
// TorqueLab Plugin Tools Container (Side settings area)
//==============================================================================
// SHould simply have to sa default framework
function Lab::setDefaultFrameWork( %this ) {
   if ($NoDefaultFrameWork)
   {
      warnLog("SetDefault FrameWork skipped! ");
      return;  
   }
   if ($AutoFrameWork)
   {
      devLog("AutoFramework called");
     Lab.changeMainFrame( $DefaultFrameWork);
     return;
   }
  // foreach$(%gui in $FW_MainCoreGuis)
  // {
   //   delObj(  %gui);
  // }
   
   delObj(EditorGuiDefault);
   %file ="tlab/EditorLab/FrameWork/Default/EditorMainDefault.gui";
   exec(%file);
   %fileCS ="tlab/EditorLab/FrameWork/Default/EditorMainDefault.cs";
   exec(%fileCS);
   		

   foreach(%ctrl in EditorGuiMainDefault)
		{
			%ctrl.defaultParent = EditorGuiMainDefault;
			%ctrls = strAddWord(%ctrls,%ctrl.getId());
		}
	foreach$(%ctrl in %ctrls)
		EditorGuiMain.add(%ctrl);	
		
 FW.setSideBarWidth();
 FW.setToolsWidth();  
   //Lab.addFrameWorkExtras();
  // Lab.setDefaultLayoutSize();
 

}

function Lab::setDefaultLayoutSize( %this, %onlyIfSmaller ) {
	EditorGui-->SideBarContainer.minExtent = $LabCfg_Layout_LeftMinWidth SPC "100";
	EditorGui-->ToolsContainer.minExtent = $LabCfg_Layout_RightMinWidth SPC "100";
	
   if (!%onlyIfSmaller || $LabCfg_Layout_RightMinWidth >  EditorGui-->ToolsContainer.extent.x)
    EditorGui-->ToolsContainer.setExtent($LabCfg_Layout_RightMinWidth,EditorGui-->ToolsContainer.extent.y);
   if (!%onlyIfSmaller || $LabCfg_Layout_LeftMinWidth >  EditorGui-->SideBarContainer.extent.x)
      EditorGui-->SideBarContainer.setExtent($LabCfg_Layout_LeftMinWidth,EditorGui-->SideBarContainer.extent.y);   
   
}
function FW::checkCoreGuis( %this ) {
	 EditorGui-->FullEditorContainer.add(ETerrainEditor);
  EditorGui-->FullEditorContainer.add(EWorldEditor);
  EditorGui-->ExtraContainer.add(LabPluginBar);
  EditorGui-->ExtraContainer.add(LabPaletteBar);
  EditorGui-->ExtraContainer.add(LabPluginThrash);
   EditorGui-->SideBarContainer.add(EditorSideBarCtrl);
  
}


//==============================================================================
// Unsused - Focus on making work by using a EditorGui Main full extra GUI
function Lab::addFrameWorkExtras( %this ) {
   delObj(EditorGuiExtras);
    foreach$(%gui in $FW_MainCoreGuis)
   {
      delObj(  %gui);
   }
   exec("tlab/EditorLab/gui/EditorGuiExtras.gui");
   if (!isObject(FWExtraSet))
      new SimSet(FWExtraSet);
   FWExtraSet.deleteAllObjects();
  foreach(%gui in EditorGuiExtras)
  {    
     %addList = "";
      %cont = EditorGui.findObjectByInternalName(%gui.internalName @ "Container",true);
      foreach(%child in %gui)
      {
         %child.extraType = %gui.internalName;
         FWExtraSet.add(%child);
         %child.defaultGui = %gui;
         %addList = strAddWord(%addList,%child.getId(),1);        
      }
       foreach$(%obj in %addList)
            %cont.add(%obj);
  }  
}

//==============================================================================
// Called 500 ms after EditorGui onWake (Often overwrite by FrameWork package
function FW::preEditorWake(%this) {	

}
//------------------------------------------------------------------------------
//==============================================================================
// Called 500 ms after EditorGui onWake (Often overwrite by FrameWork package
function FW::postEditorWake(%this) {	
   FW.setSideBarWidth();
 FW.setToolsWidth();  
	Lab.setDefaultLayoutSize(true);	
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
//Lab.changeMainFrame("FWC");
//==============================================================================
function Lab::changeMainFrame( %this,%set ) {
	if(%set $= "")
		%set = "default";	
		
	%newFrameBase = "EditorMain"@%set;
	delObj(%newFrameBase);
	%file ="tlab/EditorLab/FrameWork/"@%set@"/EditorMain"@%set@".gui";
	if (!isFile(%file))
		return;
	%csfile = strreplace(%file,".gui",".cs");
	if (isFile(%csfile))
		exec(%csfile);
	Lab.frameWorkSet = %set;
	Lab.frameWorkFile = %file;
	Lab.frameWorkGui = %newFrameBase;
	Lab.detachMainGuis();	
	//First delete the current EditorMainFrame
	foreach(%gui in EditorGuiMain)
		%defList = strAddWord(%defList,%gui.getId());		
	
   foreach$(%gui in %defList)
      %gui.defaultParent.add(%gui);
	
	
	foreach$(%child in $FrameWork_MainChildrens)
	{
		%container= EditorGuiMain.findObjectByInternalName(%child,true);
		if (!isObject(	%container))
			continue;
		warnLog("A default MainFrame child is still present:",%child,"Gui",%container,"It will be delete now!");
		delObj(%container);		
	}
	EditorGuiMain.deleteAllObjects();
	//EditorGuiMain.deleteAllObjects();	
	exec(%file);
	foreach(%ctrl in %newFrameBase)
	{
		%ctrl.defaultParent = %newFrameBase;
		%ctrls = strAddWord(%ctrls,%ctrl.getId());
	}
	foreach$(%ctrl in %ctrls)
		EditorGuiMain.add(%ctrl);	
		
	Lab.attachMainGuis();
	
	if (%newFrameBase.isMethod("initFrameWork"))
		%newFrameBase.initFrameWork();
		
   //Look for package but first deactivate previous if exist
   if (isPackage($FWPackage))
      deactivatePackage( $FWPackage );
   $FWPackage = "";
   %file ="tlab/EditorLab/FrameWork/"@%set@"/package.cs";
   if (isFile(%file))
   {
      exec(%file);
      
   }
   devLog("Framework changed to:",%set,"Package activated?",$FWPackage);
   FW.postSideContainerAdd();
   FW.postToolsContainerAdd();
   
   postEvent("FrameWorkChanged");
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::detachMainGuis( %this ) {
	newSimGroup("EditorMainDetachedGroup");
	if (!isObject(EditorMainDetachedSet))
		newSimSet("EditorMainDetachedSet");
	%childList = "FullEditorContainer EditorContainer ExtraContainer DialogContainer ToolsContainer SideBarContainer";
	foreach$(%child in %childList){
		%container= EditorGuiMain.findObjectByInternalName(%child,true);
		foreach(%gui in %container)
		{			
			%gui.childrenOf = %child;
			%detachList = strAddWord(%detachList,%gui.getId());
		}
	}
	foreach$(%gui in %detachList)
	{
		EditorMainDetachedGroup.add(%gui);
		EditorMainDetachedSet.add(%gui);
	}
		
	devLog("MainGui detached count:",EditorMainDetachedGroup.getCount());
	//EditorMainDetachedGroup.dumpData();
	//Lab.attachMainGuis(EditorMainDetachedSet);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::attachMainGuis( %this,%source ) {	
   if (%source $= "")
      %source = EditorMainDetachedGroup;
	foreach(%gui in %source)
	{
		%container = EditorGuiMain.findObjectByInternalName(%gui.childrenOf,true);
		if (!isObject(%container))
		{
			warnLog(%gui,"have a missing childrenOf setting:",%gui);
			continue;
		}
		%attachList = strAddField(%attachList,%gui.getId() SPC %container);
			
	}
	for(%i= 0;%i < getFieldCOunt(%attachList);%i++)
	{
	   %data = getField(%attachList,%i);
	   
		%data.y.add(%data.x);
		
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::saveFrameWork( %this ) {	
	Lab.detachMainGuis();
	
	foreach(%gui in EditorGuiMain)
		%defList = strAddWord(%defList,%gui.getId());		
	
	foreach$(%gui in %defList)
		%gui.defaultParent.add(%gui);
	
	GuiEd.saveGuiToFile(Lab.frameWorkGui,Lab.frameWorkFile);
	
	foreach(%ctrl in Lab.frameWorkGui)
	{
		%ctrl.defaultParent = Lab.frameWorkGui;
		%ctrls = strAddWord(%ctrls,%ctrl.getId());
	}
	foreach$(%ctrl in %ctrls)
		EditorGuiMain.add(%ctrl);	
		
	Lab.schedule(200,"attachMainGuis");
}
//------------------------------------------------------------------------------
//==============================================================================

//------------------------------------------------------------------------------




