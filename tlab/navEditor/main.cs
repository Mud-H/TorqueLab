//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Based on MaterialLab by Dave Calabrese and Travis Vroman of Gaslight Studios
//==============================================================================
$TLab_PluginName_["NavEditor"] = "Nav Editor";
//==============================================================================
function initNavEditor() {
	info( "TorqueLab","->","Initializing Nav Editor");
	execNavEd(true);
	$NavEd = newScriptObject("NavEd");
	//exec("./gui/profiles.ed.cs");
	//Lab.createPlugin("NavEditor","Nav Editor");
	NavEditorPlugin.superClass = "WEditorPlugin";
	Lab.addPluginEditor("NavEditor",NavEditorGui); //Tools renamed to Gui to store stuff
	Lab.addPluginGui("NavEditor",NavEditorTools); //Tools renamed to Gui to store stuff
	//Lab.addPluginDlg("NavEditor",CreateNewNavMeshDlg);
	Lab.addPluginDlg("NavEditor",NavEditorConsoleDlg);
	Lab.addPluginToolbar("NavEditor",NavEditorToolbar);
	Lab.addPluginPalette("NavEditor",NavEditorPalette);
}
//------------------------------------------------------------------------------
//==============================================================================
function execNavEd(%loadGui) {
	if (%loadGui) {
		// Load MaterialLab Guis
		exec("tlab/navEditor/gui/NavEditorConsoleDlg.gui");
		exec("tlab/navEditor/gui/CreateNewNavMeshDlg.gui");
		exec("tlab/navEditor/gui/NavEditorToolbar.gui");
		exec("tlab/navEditor/gui/NavEditorTools.gui");
		exec("tlab/navEditor/gui/NavEditorGui.gui");
		exec("tlab/navEditor/gui/NavEditorPalette.gui");
	}

	// Load Client Scripts.
	exec("./NavEditorPlugin.cs");
	execPattern("tlab/navEditor/scripts/*.cs");
	execPattern("tlab/navEditor/editor/*.cs");
}
//==============================================================================
function destroyMaterialLab() {
}
//------------------------------------------------------------------------------

$Nav::WalkFlag = 1 << 0;
$Nav::SwimFlag = 1 << 1;
$Nav::JumpFlag = 1 << 2;
$Nav::LedgeFlag = 1 << 3;
$Nav::DropFlag = 1 << 4;
$Nav::ClimbFlag = 1 << 5;
$Nav::TeleportFlag = 1 << 6;