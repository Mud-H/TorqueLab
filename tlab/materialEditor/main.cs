//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Based on MaterialEditor by Dave Calabrese and Travis Vroman of Gaslight Studios
//==============================================================================
$TLab_PluginName_MaterialEditor = "Material Editor";
//==============================================================================
function initMaterialEditor() {
	info( "TorqueLab","->","Initializing Material Editor");
	execMEP(true);
	//exec("./gui/profiles.ed.cs");
	$MatEd = newScriptObject("MatEd");
	//Lab.createPlugin("MaterialEditor","Material Editor");
	MaterialEditorPlugin.superClass = "WEditorPlugin";
	Lab.addPluginGui("MaterialEditor",MaterialEditorTools); //Tools renamed to Gui to store stuff
	Lab.addPluginDlg("MaterialEditor",matEd_cubemapEditor);
	Lab.addPluginDlg("MaterialEditor",matEd_addCubemapWindow);
	Lab.addPluginDlg("MaterialEditor",MaterialEditorDialogs);
	Lab.addPluginToolbar("MaterialEditor",MaterialEditorToolbar);
	new PersistenceManager(matEd_PersistMan);
	

}
//------------------------------------------------------------------------------
//==============================================================================
function execMEP(%loadGui) {
	if (%loadGui) {
		// Load MaterialEditor Guis
		exec("tlab/materialEditor/gui/matEd_cubemapEditor.gui");
		exec("tlab/materialEditor/gui/matEd_addCubemapWindow.gui");
		exec("tlab/materialEditor/gui/matEdNonModalGroup.gui");
		exec("tlab/materialEditor/gui/MaterialEditorToolbar.gui");
		exec("tlab/materialEditor/gui/MaterialEditorTools.gui");
		exec("tlab/materialEditor/gui/MaterialEditorDialogs.gui");

	}

	// Load Client Scripts.
	exec("./MaterialEditorPlugin.cs");
	execPattern("tlab/materialEditor/scripts/*.cs");
	execPattern("tlab/materialEditor/scene/*.cs");
	execPattern("tlab/materialEditor/material/*.cs");
	execPattern("tlab/materialEditor/guiScripts/*.cs");
}
//==============================================================================
function destroyMaterialEditor() {
}
//------------------------------------------------------------------------------