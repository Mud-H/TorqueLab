//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
return;

function initTerrainPainter() {
   if ($TerrainPainterInitDone)
   {
      warnLog("TerrainEditor is already initted", "It happen since both are include in one folder");
      return;
   }
   
   info( "TorqueLab","->","Initializing Terrain Painter" );
	   
	execTerrainPainter(true);
	//Add the plugin GUI elements

	//----------------------------------------------
	// Terrain Painter Plugin
	Lab.createPlugin("TerrainPainter","Terrain Painter");
	Lab.addPluginGui("TerrainPainter",TerrainPainterTools);
	Lab.addPluginToolbar("TerrainPainter",TerrainPainterToolbar);
	Lab.addPluginPalette("TerrainPainter",TerrainPainterPalette);
	Lab.addPluginDlg("TerrainPainter",TerrainPainterDialogs);
	TerrainPainterPlugin.PM = new PersistenceManager();
	TerrainPainterPlugin.setEditorMode("Terrain");
	%map = new ActionMap();
	newSimSet("FilteredTerrainMaterialsSet");
	TerrainMaterialDlg-->materialFilter.setText("");
	//Create scriptobject for paint generator
	$TPG = newScriptObject("TPG");
	$TerrainPainterInitDone = true;
}

function execTerrainPainter(%loadGui) {
	
	//----------------------------------------------
	// Terrain Painter GUIs
	if (%loadGui) {
		exec("tlab/terrainPainter/gui/ProceduralTerrainPainterGui.gui" );
		exec("tlab/terrainPainter/gui/TerrainPaintGeneratorGui.gui");
		exec("tlab/terrainPainter/gui/TerrainPainterTools.gui");
		exec("tlab/terrainPainter/gui/TerrainMaterialDlg.gui");
		exec("tlab/terrainPainter/gui/TerrainBrushSoftnessCurveDlg.gui");
		exec("tlab/terrainPainter/gui/TerrainPainterToolbar.gui");
		exec("tlab/terrainPainter/gui/TerrainPainterPalette.gui");
		exec("tlab/terrainPainter/gui/TerrainPainterDialogs.gui");
	}

	exec("tlab/terrainPainter/terrainPainterPlugin.cs");
	execPattern("tlab/terrainPainter/painter/*.cs");
	execPattern("tlab/terrainPainter/terrainMaterials/*.cs");
	execPattern("tlab/terrainPainter/autoPainter/*.cs");
}


function destroyTerrainPainter() {
   devLog("Test");
   execPattern("tlab/terrainPainter/painter/*.cs");
   devLog("Test");
}
