//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
//
//==============================================================================
//------------------------------------------------------------------------------
//Create Lab Editor Core Objects
exec("tlab/core/initLabEditor.cs");
exec("tlab/core/commonSettings.cs");

//------------------------------------------------------------------------------
//Load GameLab system (In-Game Editor)
function tlabExecCore( %loadGui )
{
    if(%loadGui)
    {
        execPattern("tlab/core/*.gui");
    }
    execPattern("tlab/core/helpers/*.cs");
    execPattern("tlab/core/scripts/*.cs","ed.cs");
    execPattern("tlab/core/settings/*.cs","cfg.cs");
    execPattern("tlab/core/menubar/*.cs");
    execPattern("tlab/core/eventManager/*.cs");
    exec("tlab/core/classBase/popupMenu.cs");
    exec("tlab/core/classBase/guiInspector.cs");
    exec("tlab/core/classBase/Inspector.cs");
    exec("tlab/core/classBase/guiTreeViewCtrl.cs");
    exec("tlab/core/classBase/guiSwatchButtonCtrl.cs");
    exec("tlab/core/classBase/guiFrameSetCtrl.cs");
}
//tlabExecCore(!$LabGuiExeced);
%execInit = strAddWord(%execInit,"tlabExecCore");


//------------------------------------------------------------------------------
//Load the Editor Menubar Scripts
/*
function tlabExecMenubar( %loadGui ) {
	exec("tlab/EditorLab/menubar/manageMenu.cs");
	exec("tlab/EditorLab/menubar/defineMenus.cs");
	exec("tlab/EditorLab/menubar/menuHandlers.cs");
	exec("tlab/EditorLab/menubar/labstyle/menubarScript.cs");
	exec("tlab/EditorLab/menubar/labstyle/buildWorldMenu.cs");
	exec("tlab/EditorLab/menubar/native/buildNativeMenu.cs");
	exec("tlab/EditorLab/menubar/native/lightingMenu.cs");
}
//tlabExecMenubar(!$LabGuiExeced);
%execMain = strAddWord(%execMain,"tlabExecMenubar");*/
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//Load the LabGui (Cleaned EditorGui files)
function tlabExecEditor(%loadGui )
{
    execPattern("tlab/EditorLab/SceneObjects/*.cs");
    execPattern("tlab/EditorLab/guiSystem/*.cs");
    exec("tlab/EditorLab/gui/EditorGui.cs");
    if (%loadGui)
    {
        //exec("tlab/EditorLab/gui/EditorMainDefault.gui");
        exec("tlab/EditorLab/gui/EditorGui.gui");
        //exec("tlab/EditorLab/gui/EditorGuiExtras.gui");
       
        //exec("tlab/EditorLab/gui/editorCore/StackStartToolbar.gui");
        //exec("tlab/EditorLab/gui/editorCore/StackEndToolbar.gui");
        execPattern("tlab/EditorLab/SceneObjects/*.gui");
        execPattern("tlab/EditorLab/gui/GameLab/*.gui");
         execPattern("tlab/EditorLab/tools/*.gui");
    }
    exec("tlab/EditorLab/FrameWork/fwInit.cs");
    Lab.initFrameWorkSystem();
    exec("tlab/EditorLab/EditorOpen.cs");
    exec("tlab/EditorLab/EditorClose.cs");
    exec("tlab/EditorLab/EditorScript.cs");
    exec("tlab/EditorLab/EditorActivate.cs");
    exec("tlab/EditorLab/EditorCallbacks.cs");
    exec("tlab/EditorLab/TorqueLabPackage.cs");
    execPattern("tlab/EditorLab/guiSystem/*.cs");
    //execPattern("tlab/EditorLab/worldEditor/*.cs");
    execPattern("tlab/EditorLab/plugin/*.cs");
    execPattern("tlab/EditorLab/guiSystem/*.cs");
    //execPattern("tlab/EditorLab/Layout/*.cs");
    execPattern("tlab/EditorLab/gui/GameLab/*.cs");
      execPattern("tlab/EditorLab/tools/*.cs");
}
//tlabExecEditor(!$LabGuiExeced);
%execInit = strAddWord(%execInit,"tlabExecEditor");
//------------------------------------------------------------------------------
//Load the LabGui (Cleaned EditorGui files)
function tlabExecEditorLast(%loadGui )
{
    //execPattern("tlab/EditorLab/SceneObjects/*.cs");
    if (%loadGui)
    {
        //execPattern("tlab/EditorLab/SceneObjects/*.gui");
    }
    exec("tlab/EditorLab/EditorClose.cs");
}
//tlabExecEditorLast(!$LabGuiExeced);
%execLast = strAddWord(%execLast,"tlabExecEditorLast");
//------------------------------------------------------------------------------
//Load the Tools scripts (Toolbar and special functions)
/*function tlabExecToolbar(%loadGui ) {
	execPattern("tlab/EditorLab/toolbar/*.cs");
}
tlabExecToolbar(!$LabGuiExeced);
%execMain = strAddWord(%execMain,"tlabExecToolbar");*/

//------------------------------------------------------------------------------
//Load the LabGui Ctrl (Cleaned EditorGui files)
function tlabExecGuiLast(%loadGui ,%skip)
{
    if (%loadGui)
    {
        exec("tlab/EditorLab/gui/messageBoxes/LabMsgBoxesGui.gui");
        //exec("tlab/EditorLab/gui/LabWidgetsGui.gui");

        //execPattern("tlab/EditorLab/gui/toolbars/*.gui");
       if (!%skip)
        execPattern("tlab/EditorLab/gui/dlgs/*.gui");
    }
  
    exec("tlab/EditorLab/gui/messageBoxes/LabMsgBoxesGui.cs");
    
     if (!%skip)
    execPattern("tlab/EditorLab/gui/dlgs/*.cs");
    //Don't do a execPattern on the gui/core, some are load individually
EditorMap.bindCmd( keyboard, "ctrl g", "toggleDlg(LabTestGui);","" );
}
%execLast = strAddWord(%execLast,"tlabExecGuiLast");



//------------------------------------------------------------------------------
//Old Settings Dialog for temporary references
function tlabExecTools(%loadGui )
{
    if (%loadGui)
    {       
        exec("tlab/EditorLab/gui/editorTools/ETools.gui");
         //ETools independant guis are loaded from initTools
    }
    execPattern("tlab/EditorLab/gui/editorTools/*.cs");
}
//tlabExecTools(!$LabGuiExeced);
//%execMain = strAddWord(%execMain,"tlabExecTools");
%execLast = strAddWord(%execLast,"tlabExecTools");
function execTools(%execGui )
{
    tlabExecTools(%execGui);
}



//------------------------------------------------------------------------------
$TLabExecInitList = %execInit;
$TLabExecMainList = %execMain;
$TLabExecLastList = %execLast;
function tlabExec( %loadGui)
{
    tlabExecList("InitList",%loadGui);
    tlabExecList("MainList",%loadGui);
    tlabExecList("LastList",%loadGui);
}
function tlabExecList(%list,%loadGui )
{
    if (%loadGui $= "")
        %loadGui = !$LabGuiExeced;
    %execlist = $TLabExec[%list];
    foreach$(%func in %execlist)
    {
        timerStart(%list@"_"@%func);
        eval(%func@"(%loadGui);");
        timerStop();
    }
}
