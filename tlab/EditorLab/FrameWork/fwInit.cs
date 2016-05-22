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
// Called after initEditorGui
function Lab::setDefaultFrameWork( %this ) {
   
}
//------------------------------------------------------------------------------
//==============================================================================
function EditorGuiContainers::onResized( %this, %data )
{
    echo( "EditorGuiContainers onResized" SPC %this SPC %this.getName() );
}
