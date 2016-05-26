//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================



//==============================================================================
// Change Active FrameWork
//==============================================================================


//Lab.changeMainFrame("FWC");
//==============================================================================
// Called a new FrameWork to be used
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
// Save the frameWork setup
//==============================================================================

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



