//==============================================================================
// TorqueLab GUI -> WidgetBuilder system
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Create quickly a set of GUI based on a template
//==============================================================================
//GuiEditorProfilesTree.init
//==============================================================================
function initializeGuiEditor() {	
   if ($TLab_GuiEditorLoaded)
		return;
   if ($Cfg_GuiEditor_GuiEditor_previewResolution $= "")
      exec("tlab/config.cfg.cs");
		
	info( "TorqueLab","->","Initializing Gui Editor" );	
	if (!isObject(GuiLab))
		$GuiLab = new scriptObject("GuiLab");
   if (!isObject(GuiEd))
	   $GuiEd = new scriptObject("GuiEd");
	delObj(GuiEdMap);
	new ActionMap(GuiEdMap);
	// GUIs.
	execGuiEdit(true);
	$TLab_GuiEditorLoaded = true;
}
//------------------------------------------------------------------------------
function execGuiLab() {
	execPattern( "tlab/guiEditor/lab/*.cs","templateManager" );
	execPattern( "tlab/guiEditor/system/*.cs" );
}
//==============================================================================
function execGuiTemplateManager(%execGui,%execMainGui) {
	return;
   if (!isObject(GuiEdTemplateManager)){
   exec( "tlab/guiEditor/gui/GuiEdTemplateManager.gui" );
		exec( "tlab/guiEditor/gui/GuiEdTemplateEditor.gui" );
		exec( "tlab/guiEditor/gui/GuiEdTemplateGroup.gui" );
		%init = true;
   }
   execPattern( "tlab/guiEditor/lab/templateManager/*.cs" );
   if (%init)
      Lab.initTemplateManager();
}
//==============================================================================
function execGuiEdit(%execGui,%execMainGui) {
	if (%execGui) {
		%execMainGui = true;
		exec( "./gui/guiEditorNewGuiDialog.ed.gui" );
		exec( "./gui/guiEditorPrefsDlg.ed.gui" );
		//exec( "./gui/guiEditorSelectDlg.ed.gui" );
		exec( "./gui/EditorChooseGUI.ed.gui" );

		exec( "tlab/guiEditor/gui/GuiEditFieldDuplicator.gui" );
		
	}

	if (%execMainGui) {
		exec( "tlab/guiEditor/gui/guiEditor.ed.gui" );
		//exec( "tlab/guiEditor/gui/CloneEditorGui.gui" );
	}

	if (!isObject(GuiEditor)) {
		addGuiEditorCtrl();
	}

	// Scripts.
	exec( "tlab/guiEditor/GuiEditorCallbacks.cs" );
	exec( "tlab/guiEditor/scripts/GuiEditorGui.cs" );
	exec( "tlab/guiEditor/scripts/guiEditor.ed.cs" );
	exec( "tlab/guiEditor/scripts/guiEditorTreeView.ed.cs" );
	exec( "./scripts/guiEditorInspector.ed.cs" );
	exec( "tlab/guiEditor/scripts/guiEditorProfiles.ed.cs" );
	exec( "./scripts/guiEditorGroup.ed.cs" );
	exec( "tlab/guiEditor/scripts/guiEditorUndo.cs" );
	exec( "tlab/guiEditor/scripts/guiEditorCanvas.ed.cs" );
	exec( "./scripts/guiEditorContentList.ed.cs" );
	exec( "./scripts/guiEditorStatusBar.ed.cs" );
	exec( "./scripts/guiEditorToolbox.ed.cs" );
	exec( "./scripts/guiEditorSelectDlg.ed.cs" );
	exec( "./scripts/guiEditorNewGuiDialog.ed.cs" );
	exec( "./scripts/fileDialogs.ed.cs" );
	exec( "./scripts/guiEditorPrefsDlg.ed.cs" );
	exec( "./scripts/EditorChooseGUI.ed.cs" );
	
	exec( "tlab/guiEditor/scripts/functionControls.cs" );
	execPattern( "tlab/guiEditor/lab/*.cs","templateManager" );
	execPattern( "tlab/guiEditor/system/*.cs" );
	GuiEd.InitGuiEditor();
}
//------------------------------------------------------------------------------
//==============================================================================
function destroyGuiEditor() {
}
//------------------------------------------------------------------------------
function pushEdDlg(%dlg,%layer,%center)
{
	//if ($InGuiEditor)
		//return;
	Canvas.pushDialog(%dlg,%layer,%center);
}	

//==============================================================================
// Toggle a Dialog GUI
function toggleEdDlg(%dlg)
{
	if (%dlg.isAwake())
		popDlg(%dlg);
	else
		pushEdDlg(%dlg);
}
//------------------------------------------------------------------------------