//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function initTerrainEditor() {
   if ($TerrainEditorInitDone)
   {
      warnLog("TerrainEditor is already initted", "It happen since both are include in one folder");
      return;
   }
   
   info( "TorqueLab","->","Initializing Terrain Editor" );
	   
	execTerrainEd(true);
	//Add the plugin GUI elements
	//----------------------------------------------
	// Terrain Editor Plugin
	//Lab.createPlugin("TerrainEditor","Terrain Editor");
	Lab.addPluginToolbar("TerrainEditor",EWTerrainEditToolbar);
	Lab.addPluginGui("TerrainEditor",TerrainEditorTools);
	Lab.addPluginPalette("TerrainEditor",TerrainEditorPalette);
	Lab.addPluginDlg("TerrainEditor",TerrainEditorDialogs);
	Lab.addPluginDlg("TerrainEditor",TerrainMaterialDlg);
	TerrainEditorPlugin.PM = new PersistenceManager();
	TerrainEditorPlugin.setEditorMode("Terrain");
	//----------------------------------------------
	// Terrain Painter Plugin
	//Lab.createPlugin("TerrainPainter","Terrain Painter");
	//Lab.addPluginGui("TerrainPainter",TerrainPainterTools);
	//Lab.addPluginToolbar("TerrainPainter",TerrainPainterToolbar);
	//Lab.addPluginPalette("TerrainPainter",TerrainPainterPalette);
	//Lab.addPluginDlg("TerrainPainter",TerrainPainterDialogs);
	//TerrainPainterPlugin.PM = new PersistenceManager();
	//TerrainPainterPlugin.setEditorMode("Terrain");
	%map = new ActionMap();
	newSimSet("FilteredTerrainMaterialsSet");
	TerrainMaterialDlg-->materialFilter.setText("");
	//Create scriptobject for paint generator
	$TPG = newScriptObject("TPG");
	$TMG = newScriptObject("TMG");
	$TerrainEditorInitDone = true;
	$TEPainter = newScriptObject("TEPainter");
	$TESculpt = newScriptObject("TESculpt");
}

function execTerrainEd(%loadGui) {
	//----------------------------------------------
	// Terrain Editor GUIs
	if (%loadGui ) {
		exec("tlab/terrainEditor/gui/TerrainCreatorGui.gui" );
		exec("tlab/terrainEditor/gui/TerrainImportGui.gui" );
		exec("tlab/terrainEditor/gui/TerrainExportGui.gui" );
		exec("tlab/terrainEditor/gui/TerrainEditorVSettingsGui.gui");
		exec("tlab/terrainEditor/gui/TerrainEditorPalette.gui");
		exec("tlab/terrainEditor/gui/EWTerrainEditToolbar.gui");
		exec("tlab/terrainEditor/gui/TerrainEditorDialogs.gui");
		exec("tlab/terrainEditor/gui/TerrainEditorTools.gui");
		exec("tlab/terrainEditor/gui/TerrainMaterialDlg.gui");
		exec("tlab/terrainEditor/gui/TerrainPaintGeneratorGui.gui");
		//exec("tlab/terrainEditor/gui/TerrainManagerGui.gui" );
	}

	exec("tlab/terrainEditor/gui/TerrainImportGui.cs");
	exec("tlab/terrainEditor/gui/TerrainExportGui.cs");
	exec("tlab/terrainEditor/gui/TerrainCreatorGui.cs" );

	

	exec("tlab/terrainEditor/terrainEditorPlugin.cs");
	execPattern("tlab/terrainEditor/scripts/*.cs");
   execPattern("tlab/terrainEditor/brushTool/*.cs");
	execPattern("tlab/terrainEditor/painter/*.cs");
	execPattern("tlab/terrainEditor/terrainMaterials/*.cs");
	execPattern("tlab/terrainEditor/autoPainter/*.cs");   

}


