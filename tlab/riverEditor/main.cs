//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TLab_PluginName_RiverEditor = "River Editor";
function initRiverEditor() {
	info( "TorqueLab","->","Initializing River Editor");
	execRiverEd(true);
	// Add ourselves to EditorGui, where all the other tools reside
	//Lab.createPlugin("RiverEditor","River Editor");
	Lab.addPluginEditor("RiverEditor",RiverEditorGui);
	Lab.addPluginGui("RiverEditor",RiverEditorTools);
	Lab.addPluginToolbar("RiverEditor",RiverEditorToolbar);
	Lab.addPluginPalette("RiverEditor",   RiverEditorPalette);
	Lab.addPluginDlg("RiverEditor",RiverEditorDialogs);
	RiverEditorPlugin.editorGui = RiverEditorGui;
	$RiverEd = newScriptObject("RiverEd");
	%map = new ActionMap();
	%map.bindCmd( keyboard, "backspace", "RiverEditorGui.deleteNode();", "" );
	%map.bindCmd( keyboard, "1", "RiverEditorGui.prepSelectionMode();", "" );
	%map.bindCmd( keyboard, "2", "LabPaletteArray->RiverEditorMoveMode.performClick();", "" );
	%map.bindCmd( keyboard, "3", "LabPaletteArray->RiverEditorRotateMode.performClick();", "" );
	%map.bindCmd( keyboard, "4", "LabPaletteArray->RiverEditorScaleMode.performClick();", "" );
	%map.bindCmd( keyboard, "5", "LabPaletteArray->RiverEditorAddRiverMode.performClick();", "" );
	%map.bindCmd( keyboard, "=", "LabPaletteArray->RiverEditorInsertPointMode.performClick();", "" );
	%map.bindCmd( keyboard, "numpadadd", "LabPaletteArray->RiverEditorInsertPointMode.performClick();", "" );
	%map.bindCmd( keyboard, "-", "LabPaletteArray->RiverEditorRemovePointMode.performClick();", "" );
	%map.bindCmd( keyboard, "numpadminus", "LabPaletteArray->RiverEditorRemovePointMode.performClick();", "" );
	%map.bindCmd( keyboard, "z", "RiverEditorShowSplineBtn.performClick();", "" );
	%map.bindCmd( keyboard, "x", "RiverEditorWireframeBtn.performClick();", "" );
	%map.bindCmd( keyboard, "v", "RiverEditorShowRoadBtn.performClick();", "" );
	RiverEditorPlugin.map = %map;
	$RiverManager = newScriptObject("RiverManager");
	// RiverEditorPlugin.initSettings();
}
function execRiverEd(%loadGui) {
	if (%loadGui) {
		exec( "tlab/riverEditor/gui/riverEditorGui.gui" );
		exec( "tlab/riverEditor/gui/RiverEditorTools.gui" );
		exec( "tlab/riverEditor/gui/riverEditorToolbar.gui" );
		exec( "tlab/riverEditor/gui/riverEditorPaletteGui.gui" );
		exec( "tlab/riverEditor/gui/riverEditorDialogs.gui" );
	}

	exec( "tlab/riverEditor/riverEditorGui.cs" );
	exec( "tlab/riverEditor/RiverEditorPlugin.cs" );
	execPattern("tlab/riverEditor/scripts/*.cs");
	execPattern("tlab/riverEditor/nodeManager/*.cs");
}

function destroyRiverEditor() {
}

