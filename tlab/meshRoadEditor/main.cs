//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TLab_PluginName_["MeshRoadEditor"] = "Mesh Road Editor";
singleton GuiControlProfile( MeshRoadEditorProfile ) {
	canKeyFocus = true;
	opaque = true;
	fillColor = "192 192 192 192";
	category = "Editor";
};


//------------------------------------------------------------------------------
function initMeshRoadEditor() {
	devLog( "TorqueLab","->","Execing Mesh Road Editor");
	$MRoadManager = newScriptObject("MRoadManager");
	execMREP(true);
	
	Lab.addPluginEditor("MeshRoadEditor",MeshRoadEditorGui);
	Lab.addPluginGui("MeshRoadEditor",   MeshRoadEditorTools);
	Lab.addPluginToolbar("MeshRoadEditor",MeshRoadEditorToolbar);
	Lab.addPluginPalette("MeshRoadEditor",   MeshRoadEditorPalette);
	MeshRoadEditorPlugin.editorGui = MeshRoadEditorGui;
	%map = new ActionMap();
	%map.bindCmd( keyboard, "backspace", "MeshRoadEditorGui.deleteNode();", "" );
	%map.bindCmd( keyboard, "1", "MeshRoadEditorGui.prepSelectionMode();", "" );
	%map.bindCmd( keyboard, "2", "LabPaletteArray->MeshRoadEditorMoveMode.performClick();", "" );
	%map.bindCmd( keyboard, "3", "LabPaletteArray->MeshRoadEditorRotateMode.performClick();", "" );
	%map.bindCmd( keyboard, "4", "LabPaletteArray->MeshRoadEditorScaleMode.performClick();", "" );
	%map.bindCmd( keyboard, "5", "LabPaletteArray->MeshRoadEditorAddRoadMode.performClick();", "" );
	%map.bindCmd( keyboard, "=", "LabPaletteArray->MeshRoadEditorInsertPointMode.performClick();", "" );
	%map.bindCmd( keyboard, "numpadadd", "LabPaletteArray->MeshRoadEditorInsertPointMode.performClick();", "" );
	%map.bindCmd( keyboard, "-", "LabPaletteArray->MeshRoadEditorRemovePointMode.performClick();", "" );
	%map.bindCmd( keyboard, "numpadminus", "LabPaletteArray->MeshRoadEditorRemovePointMode.performClick();", "" );
	%map.bindCmd( keyboard, "z", "MeshRoadEditorShowSplineBtn.performClick();", "" );
	%map.bindCmd( keyboard, "x", "MeshRoadEditorWireframeBtn.performClick();", "" );
	%map.bindCmd( keyboard, "v", "MeshRoadEditorShowRoadBtn.performClick();", "" );
	MeshRoadEditorPlugin.map = %map;
	
	
}
//------------------------------------------------------------------------------
//==============================================================================
function execMREP(%loadGui) {
	//----------------------------------------------
	// Terrain Editor GUIs
	if (%loadGui) {
		exec( "tlab/meshRoadEditor/meshRoadEditor.cs" );
		exec( "tlab/meshRoadEditor/gui/meshRoadEditorGui.gui" );
		exec( "tlab/meshRoadEditor/gui/MeshRoadEditorTools.gui" );
		exec( "tlab/meshRoadEditor/gui/meshRoadEditorToolbar.gui");
		exec( "tlab/meshRoadEditor/gui/meshRoadEditorPaletteGui.gui");
	}

	exec( "tlab/meshRoadEditor/meshRoadEditorGui.cs" );
	exec( "tlab/meshRoadEditor/MeshRoadEditorPlugin.cs" );
	exec( "tlab/meshRoadEditor/MeshRoadEditorParams.cs" );
	execPattern( "tlab/meshRoadEditor/RoadManager/*.cs" );
	execPattern( "tlab/meshRoadEditor/Editor/*.cs" );
}
//------------------------------------------------------------------------------
//==============================================================================
function destroyMeshRoadEditor() {
}
