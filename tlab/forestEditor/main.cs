//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TLab_PluginName_["ForestEditor"] = "Forest Editor";
singleton GuiControlProfile (ForestEditorProfile) {
	canKeyFocus = true;
	category = "Editor";
	fontColors[4] = "Magenta";
	fontColors[9] = "255 0 255 255";
	fontColorLink = "Magenta";
};
function initForestEditor() {
	info( "TorqueLab","->","Initializing Forest Editor");
	//$FEP_BrushSet = newSimSet("FEP_BrushSet");
	execFEP(true);
	if (!isObject(ForestEd))
	   new scriptObject(ForestEd);
	//Lab.createPlugin("ForestEditor");
	//Add the different editor GUIs to the LabEditor
	Lab.addPluginEditor("ForestEditor",ForestEditorGui);
	Lab.addPluginGui("ForestEditor",   ForestEditorTools);
	Lab.addPluginToolbar("ForestEditor",ForestEditorToolbar);
	Lab.addPluginPalette("ForestEditor",   ForestEditorPalette);
	Lab.addPluginDlg("ForestEditor",   ForestEditorDialogs);
	ForestEditorPlugin.editorGui = ForestEditorGui;
	ForestEditorPalleteWindow.position = getWord($pref::Video::mode, 0) - 209  SPC getWord(EditorGuiToolbar.extent, 1)-1;
	new SimSet(ForestTools) {
		new ForestBrushTool(ForestToolBrush) {
			internalName = "BrushTool";
			toolTip = "Paint Tool";
			buttonImage = "tlab/forest/images/brushTool";
		};
		new ForestSelectionTool(ForestToolSelection) {
			internalName = "SelectionTool";
			toolTip = "Selection Tool";
			buttonImage = "tlab/forest/images/selectionTool";
		};
	};
	%map = new ActionMap();
	%map.bindCmd( keyboard, "1", "ForestEditorSelectModeBtn.performClick();", "" ); // Select
	%map.bindCmd( keyboard, "2", "ForestEditorMoveModeBtn.performClick();", "" );   // Move
	%map.bindCmd( keyboard, "3", "ForestEditorRotateModeBtn.performClick();", "" ); // Rotate
	%map.bindCmd( keyboard, "4", "ForestEditorScaleModeBtn.performClick();", "" );  // Scale
	%map.bindCmd( keyboard, "5", "ForestEditorPaintModeBtn.performClick();", "" );  // Paint
	%map.bindCmd( keyboard, "6", "ForestEditorEraseModeBtn.performClick();", "" );  // Erase
	%map.bindCmd( keyboard, "7", "ForestEditorEraseSelectedModeBtn.performClick();", "" );  // EraseSelected
	//%map.bindCmd( keyboard, "backspace", "ForestEditorGui.onDeleteKey();", "" );
	//%map.bindCmd( keyboard, "delete", "ForestEditorGui.onDeleteKey();", "" );
	ForestEditorPlugin.map = %map;
}
function execFEP(%loadGui) {
	if (%loadGui) {
		exec( "./gui/forestEditorGui.gui" );
		exec( "./gui/ForestEditorTools.gui" );
		exec( "./gui/ForestEditorToolbar.gui" );
		exec( "./gui/forestEditorPalette.gui" );
		exec( "tlab/ForestEditor/gui/forestEditorDialogs.gui" );
	}

	// Load Client Scripts.
	exec( "tlab/ForestEditor/forestEditorGui.cs" );
	exec( "./tools.cs" );
	exec( "tlab/ForestEditor/ForestEditorPlugin.cs" );
	exec( "tlab/ForestEditor/ForestEditorSave.cs" );
	exec( "tlab/ForestEditor/ForestEditorScript.cs" );
	execPattern("tlab/ForestEditor/scripts/*.cs");
	execPattern("tlab/ForestEditor/dialogs/*.cs");
	execPattern("tlab/ForestEditor/toolsBook/*.cs");
}
function destroyForestEditor() {
}

// NOTE: debugging helper.
function reinitForest() {
	exec( "./main.cs" );
	exec( "./forestEditorGui.cs" );
	exec( "./tools.cs" );
}


