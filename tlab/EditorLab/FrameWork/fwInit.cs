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
$LabCfg_Layout_LeftMinWidth = "220";
$LabCfg_Layout_RightMinWidth = "280";
//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function Lab::initFrameWorkSystem( %this )
{
    exec("tlab/EditorLab/FrameWork/fwSetup.cs");
    exec("tlab/EditorLab/FrameWork/fwInit.cs");
    exec("tlab/EditorLab/FrameWork/fwMainArea.cs");
    exec("tlab/EditorLab/FrameWork/fwToolsContainer.cs");
    exec("tlab/EditorLab/FrameWork/fwSideContainer.cs");
    
    
}
//------------------------------------------------------------------------------

//==============================================================================
// Clear the editors menu (for dev purpose only as now)
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
