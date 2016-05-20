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
function tlabExecCore( %loadGui ) {
	if(%loadGui) {
		execPattern("tlab/core/*.gui");
	}

	execPattern("tlab/core/helpers/*.cs");
	execPattern("tlab/core/scripts/*.cs");
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
function tlabExecEditor(%loadGui ) {
	execPattern("tlab/EditorLab/SceneObjects/*.cs");
	execPattern("tlab/EditorLab/guiSystem/*.cs");
	exec("tlab/EditorLab/gui/EditorGui.cs");
	if (%loadGui) {
		//exec("tlab/EditorLab/gui/EditorMainDefault.gui");
		exec("tlab/EditorLab/gui/EditorGui.gui");
		//exec("tlab/EditorLab/gui/EditorGuiExtras.gui");
		exec("tlab/EditorLab/gui/cursors.cs");
		
		execPattern("tlab/EditorLab/guiSystem/*.gui","FrameWork");
		execPattern("tlab/EditorLab/SceneObjects/*.gui");
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
	
}
//tlabExecEditor(!$LabGuiExeced);
%execInit = strAddWord(%execInit,"tlabExecEditor");
//------------------------------------------------------------------------------
//Load the LabGui (Cleaned EditorGui files)
function tlabExecEditorLast(%loadGui ) {
	//execPattern("tlab/EditorLab/SceneObjects/*.cs");

	if (%loadGui) {
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
function tlabExecGui(%loadGui ) {
	if (%loadGui) {
		//exec("tlab/EditorLab/gui/CtrlCameraSpeedDropdown.gui");
		//exec("tlab/EditorLab/gui/CtrlSnapSizeSlider.gui");
		//exec("tlab/EditorLab/gui/messageBoxes/LabMsgBoxesGui.gui");
		//exec("tlab/EditorLab/gui/DlgManageSFXParameters.gui" );
		//exec("tlab/EditorLab/gui/LabWidgetsGui.gui");
		//exec("tlab/EditorLab/gui/DlgAddFMODProject.gui");
		//exec("tlab/EditorLab/gui/DlgEditorChooseLevel.gui");
		//exec("tlab/EditorLab/gui/DlgGenericPrompt.gui");
		//exec("tlab/EditorLab/gui/DlgObjectBuilder.gui");
		//exec("tlab/EditorLab/gui/DlgTimeAdjust.gui");
		//exec("tlab/EditorLab/gui/oldmatSelector/oldmatSelectorDlg.gui");
		//exec("tlab/EditorLab/gui/core/EditorLoadingGui.gui"); //Loaded at start
		//exec("tlab/EditorLab/gui/core/simViewDlg.ed.gui");
		//exec("tlab/EditorLab/gui/core/colorPicker.gui");
		//exec("tlab/EditorLab/gui/core/scriptEditorDlg.ed.gui");
		//exec("tlab/EditorLab/gui/core/GuiEaseEditDlg.ed.gui");
		//exec("tlab/EditorLab/gui/core/uvEditor.ed.gui");
		//exec("tlab/EditorLab/gui/TLabGameGui.gui");
		execPattern("tlab/EditorLab/gui/toolbars/*.gui");

	}

	//exec("tlab/EditorLab/gui/messageBoxes/LabMsgBoxesGui.cs");
	//exec("tlab/EditorLab/gui/DlgManageSFXParameters.cs" );
	//exec("tlab/EditorLab/gui/DlgAddFMODProject.cs");
	//exec("tlab/EditorLab/gui/DlgEditorChooseLevel.cs");
	//execPattern("tlab/EditorLab/gui/oldmatSelector/*.cs");
	execPattern("tlab/EditorLab/gui/toolbars/*.cs");

	//Don't do a execPattern on the gui/core, some are load individually
	//exec("tlab/EditorLab/gui/core/fileDialogBase.ed.cs");
	//exec("tlab/EditorLab/gui/core/GuiEaseEditDlg.ed.cs");
	//exec("tlab/EditorLab/gui/TLabGameGui.cs");
	//exec("tlab/EditorLab/gui/core/colorPicker.cs");
}
//tlabExecGui(!$LabGuiExeced);
%execMain = strAddWord(%execMain,"tlabExecGui");
//------------------------------------------------------------------------------
//Load the LabGui Ctrl (Cleaned EditorGui files)
function tlabExecGuiLast(%loadGui ) {
	if (%loadGui) {
		exec("tlab/EditorLab/gui/CtrlCameraSpeedDropdown.gui");
		exec("tlab/EditorLab/gui/CtrlSnapSizeSlider.gui");
		exec("tlab/EditorLab/gui/messageBoxes/LabMsgBoxesGui.gui");
		exec("tlab/EditorLab/gui/DlgManageSFXParameters.gui" );
		//exec("tlab/EditorLab/gui/LabWidgetsGui.gui");
		exec("tlab/EditorLab/gui/DlgAddFMODProject.gui");
		exec("tlab/EditorLab/gui/DlgEditorChooseLevel.gui");
		exec("tlab/EditorLab/gui/DlgGenericPrompt.gui");
		//exec("tlab/EditorLab/gui/DlgObjectBuilder.gui");
		exec("tlab/EditorLab/gui/DlgTimeAdjust.gui");
		exec("tlab/EditorLab/gui/oldmatSelector/oldmatSelectorDlg.gui");
		//exec("tlab/EditorLab/gui/core/EditorLoadingGui.gui"); //Loaded at start
		//exec("tlab/EditorLab/gui/core/simViewDlg.ed.gui");
		exec("tlab/EditorLab/gui/core/colorPicker.gui");
		exec("tlab/EditorLab/gui/core/scriptEditorDlg.ed.gui");
		exec("tlab/EditorLab/gui/core/GuiEaseEditDlg.ed.gui");
		exec("tlab/EditorLab/gui/core/uvEditor.ed.gui");
		exec("tlab/EditorLab/gui/TLabGameGui.gui");
		//execPattern("tlab/EditorLab/gui/toolbars/*.gui");
		execPattern("tlab/EditorLab/tools/*.gui");

	}

	execPattern("tlab/EditorLab/tools/*.cs");	
	exec("tlab/EditorLab/gui/messageBoxes/LabMsgBoxesGui.cs");
	exec("tlab/EditorLab/gui/DlgManageSFXParameters.cs" );
	exec("tlab/EditorLab/gui/DlgAddFMODProject.cs");
	exec("tlab/EditorLab/gui/DlgEditorChooseLevel.cs");
	execPattern("tlab/EditorLab/gui/oldmatSelector/*.cs");
	//execPattern("tlab/EditorLab/gui/toolbars/*.cs");

	//Don't do a execPattern on the gui/core, some are load individually
	exec("tlab/EditorLab/gui/core/fileDialogBase.ed.cs");
	exec("tlab/EditorLab/gui/core/GuiEaseEditDlg.ed.cs");
	exec("tlab/EditorLab/gui/TLabGameGui.cs");
	exec("tlab/EditorLab/gui/core/colorPicker.cs");
}
%execLast = strAddWord(%execLast,"tlabExecGuiLast");


//------------------------------------------------------------------------------
//Old Settings Dialog for temporary references
function tlabExecDialogsLast(%loadGui ) {
	if (%loadGui) {
		execPattern("tlab/EditorLab/gui/dialogs/*.gui");
		//execPattern("tlab/EditorLab/gui/editorDialogs/*.gui");
		
	}

	//exec("tlab/EditorLab/gui/commonDialogs.cs");
	execPattern("tlab/EditorLab/gui/dialogs/*.cs");
	//execPattern("tlab/EditorLab/gui/editorDialogs/*.cs");
	
}
%execLast = strAddWord(%execLast,"tlabExecDialogsLast");
//------------------------------------------------------------------------------
//Old Settings Dialog for temporary references
function tlabExecTools(%loadGui ) {
	if (%loadGui) {
		exec("tlab/EditorLab/gui/editorTools/ETools.gui");		
		execPattern("tlab/EditorLab/gui/editorTools/selfloadGuis/*.gui");		
	}
	execPattern("tlab/EditorLab/gui/editorTools/*.cs");
}
//tlabExecTools(!$LabGuiExeced);
//%execMain = strAddWord(%execMain,"tlabExecTools");
%execLast = strAddWord(%execLast,"tlabExecTools");
function execTools(%execGui ) {
	tlabExecTools(%execGui);
}



//------------------------------------------------------------------------------
$TLabExecInitList = %execInit;
$TLabExecMainList = %execMain;
$TLabExecLastList = %execLast;
function tlabExec( %loadGui) {
	tlabExecList("InitList",%loadGui);
	tlabExecList("MainList",%loadGui);
	tlabExecList("LastList",%loadGui);
}
function tlabExecList(%list,%loadGui ) {
	if (%loadGui $= "")
		%loadGui = !$LabGuiExeced;

	%execlist = $TLabExec[%list];

	foreach$(%func in %execlist) {
		timerStart(%list@"_"@%func);
		eval(%func@"(%loadGui);");
		timerStop();
	}
}
