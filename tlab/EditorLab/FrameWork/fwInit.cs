//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// TorqueLab have a flexible UI FrameWork system that allow to switch layout
// of the WorkSpace. For that, the EditorGui setup is special and must be editted
// carefully. A guide will be provided in future to assist UI modifications.
//------------------------------------------------------------------------------
// The EditorGui.gui only contain the root guiControls. (MenuBar, Toolbar, Status)
// The Main editor GUIS are stored in the EditorMainDefault.gui which is similar
// as FrameWork GUIs but contain the default GuiControls as childs
//==============================================================================

if (!isObject(FW))
   new ScriptObject(FW);
$LabCfg_Layout_LeftMinWidth = "220";
$LabCfg_Layout_RightMinWidth = "280";
$TLab::DefaultPlugins = "SceneEditor";

$LabCfg_Layout_LeftMinWidth = "220";
$LabCfg_Layout_RightMinWidth = "280"; 

$Cfg_TLab_LeftFrameMin = "123";
$Cfg_TLab_RightFrameMin = "123";
$FWCoreGuiList = "EWorldEditor ETerrainEditor LabPluginBar LabPaletteBar LabPluginThrash LabSideBar StackStartToolbar StackEndToolbar";
$FWCoreGuiParent["EWorldEditor"] = "FullEditor";
$FWCoreGuiParent["ETerrainEditor"] = "FullEditor";
$FWCoreGuiParent["LabPluginBar"] = "Extra";
$FWCoreGuiParent["LabPaletteBar"] = "Extra";
$FWCoreGuiParent["LabPluginThrash"] = "Extra";
$FWCoreGuiParent["LabSideBar"] = "SideBar";
$FWCoreGuiParent["StackStartToolbar"] = "Toolbar";
$FWCoreGuiParent["StackEndToolbar"] = "Toolbar";

$AutoFrameWork = 0;
$NoDefaultFrameWork = 1;
$DefaultFrameWork = "Default";
$FrameWork_MainChildrens = "FullEditorContainer EditorContainer ExtraContainer DialogContainer ToolsContainer SideBarContainer";
$FW_MainCoreGuis = "EWorldEditor ETerrainEditor LabPaletteBar LabPluginBar";
//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function Lab::initFrameWorkSystem( %this )
{
    exec("tlab/EditorLab/FrameWork/fwSetup.cs");
    exec("tlab/EditorLab/FrameWork/fwInit.cs");
    exec("tlab/EditorLab/FrameWork/fwChangeSet.cs");
    exec("tlab/EditorLab/FrameWork/fwToolsContainer.cs");
    exec("tlab/EditorLab/FrameWork/fwSideContainer.cs");   
    
   FW.checkEditorCore();
    exec("tlab/EditorLab/FrameWork/Default/package.cs");   
   EToolOverlayGui.callOnChildren("visible",0);
}
//------------------------------------------------------------------------------
//==============================================================================
// EditorFrame - Left-Right Column
//==============================================================================

//==============================================================================
// Make sure all GUIs are fine once the editor is launched
function FW::checkEditorCore(%this,%resetData)
{
   %missing = "None";
    foreach$(%coreGui in $FWCoreGuiList)
    {
       
        if (!isObject(%coreGui))
        {
            %coreGui = %this.resetEditorCoreGui(%coreGui);
            %missing = strAddWord(%missing,%coreGui);
        }
        if (!isObject(%coreGui))
        {
           warnLog("A core UI features check have failed. The missing item is:",%coreGui);
           continue;
        }
        
     
        %parentInt = $FWCoreGuiParent[%coreGui.getName()];
        %parent = EditorGui.findObjectByInternalName(%parentInt@"Container",true);
        
        if (!isObject(%parent))
            continue;
         %parent.add(%coreGui);
        // %parent.superClass = "EditorGuiContainers";
    }
   
    //devLog("EditorCore Checked! Updated Guis:",%missing);
    if (%resetData)
      FW.resetData();
}
//------------------------------------------------------------------------------
//==============================================================================
// Emergency function to reset the Gui data after failure (Ex: Add plugins icon)
function FW::resetData(%this)
{
  Lab.updatePluginsBar();
  Lab.initAllToolbarGroups();
  Lab.updatePluginsTools();
  Lab.updatePaletteBar();
}
//------------------------------------------------------------------------------
//==============================================================================
// Called 500 ms after EditorGui onWake (Often overwrite by FrameWork package
function FW::preEditorWake(%this) {
   fw.checkEditorCore();
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
//==============================================================================
// Activate FrameWork base - Called from EditorLaunchGuiSetup
function Lab::activateFrameWork( %this )
{
    if (!isObject(FW))
        new ScriptObject(FW);
    foreach$(%child in $FrameWork_MainChildrens)
    {
        %ctrl = EditorGuiMain.findObjectByInternalName(%child,true);
        if (!isobject(%ctrl))
            continue;
        %ctrl.fitIntoParents();
    }
    EditorGui-->WorldContainer.pushToBack(EditorDialogs);
    if (isObject(Lab.frameWorkGui))
    {
        if(Lab.frameWorkGui.isMethod("activateFrameWork"))
            Lab.frameWorkGui.activateFrameWork();
    }
    
    
}
//------------------------------------------------------------------------------

//==============================================================================
// Reminder of usefull function call each time a container have been resized
function EditorGuiContainers::onResized( %this, %data )
{
  //  echo( "EditorGuiContainers onResized" SPC %this SPC %this.getName() );
}
