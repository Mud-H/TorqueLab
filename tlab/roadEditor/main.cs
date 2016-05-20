//==============================================================================
// TorqueLab -> RoadEditorPlugin Initialization
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TLab_PluginName_["RoadEditor"] = "Road Editor";
//==============================================================================
function initRoadEditor() {
	info( "TorqueLab","->","Initializing Road and Path Editor" );
	execREP(true);
	//Lab.createPlugin("RoadEditor","Road Editor");
	Lab.addPluginEditor("RoadEditor",RoadEditorGui);
	Lab.addPluginGui("RoadEditor",RoadEditorTools);
	//Lab.addPluginGui("RoadEditor",RoadEditorOptionsWindow);
	//Lab.addPluginGui("RoadEditor",RoadEditorTreeWindow);
	Lab.addPluginToolbar("RoadEditor",RoadEditorToolbar);
	Lab.addPluginPalette("RoadEditor",   RoadEditorPalette);
	RoadEditorPlugin.editorGui = RoadEditorGui;
	$REP = newScriptObject("REP");
	%map = new ActionMap();
	%map.bindCmd( keyboard, "backspace", "RoadEditorGui.onDeleteKey();", "" );
	%map.bindCmd( keyboard, "1", "RoadEditorGui.prepSelectionMode();", "" );
	%map.bindCmd( keyboard, "2", "LabPaletteArray->RoadEditorMoveMode.performClick();", "" );
	%map.bindCmd( keyboard, "4", "LabPaletteArray->RoadEditorScaleMode.performClick();", "" );
	%map.bindCmd( keyboard, "5", "LabPaletteArray->RoadEditorAddRoadMode.performClick();", "" );
	%map.bindCmd( keyboard, "=", "LabPaletteArray->RoadEditorInsertPointMode.performClick();", "" );
	%map.bindCmd( keyboard, "numpadadd", "LabPaletteArray->RoadEditorInsertPointMode.performClick();", "" );
	%map.bindCmd( keyboard, "-", "LabPaletteArray->RoadEditorRemovePointMode.performClick();", "" );
	%map.bindCmd( keyboard, "numpadminus", "LabPaletteArray->RoadEditorRemovePointMode.performClick();", "" );
	%map.bindCmd( keyboard, "z", "RoadEditorShowSplineBtn.performClick();", "" );
	%map.bindCmd( keyboard, "x", "RoadEditorWireframeBtn.performClick();", "" );
	%map.bindCmd( keyboard, "v", "RoadEditorShowRoadBtn.performClick();", "" );
	RoadEditorPlugin.map = %map;
	$RoadManager = newScriptObject("RoadManager");
	//RoadEditorPlugin.initSettings();
}
//------------------------------------------------------------------------------
//==============================================================================
// Load all the Scripts and GUIs (if specified)
function execREP(%loadGui) {
	if (%loadGui) {
		exec( "tlab/roadEditor/gui/guiProfiles.cs" );
		exec( "tlab/roadEditor/gui/roadEditorGui.gui" );
		exec( "tlab/roadEditor/gui/RoadEditorTools.gui" );
		exec( "tlab/roadEditor/gui/roadEditorToolbar.gui");
		exec( "tlab/roadEditor/gui/RoadEditorPaletteGui.gui");
	}

	exec( "tlab/roadEditor/roadEditorGui.cs" );
	exec( "tlab/roadEditor/RoadEditorPlugin.cs" );
	execPattern("tlab/roadEditor/editor/*.cs");
	execPattern("tlab/roadEditor/nodeManager/*.cs");
}
//------------------------------------------------------------------------------
//==============================================================================
function destroyRoadEditor() {
}
//------------------------------------------------------------------------------