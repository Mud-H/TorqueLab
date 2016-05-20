//==============================================================================
// TorqueLab -> TerrainMaterialManager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TMG_Init = false;
$TMG_CurrentPage = 0;
$TMG_LayersImportExportPage = 0;
$TMG_LayerOptionsPage = 0;
//==============================================================================
//TerrainManagerGui onWake and onSleep callbacks
//==============================================================================
//==============================================================================
function TerrainManagerGui::onWake(%this) {
	TMG_LayerOptionsBook.selectPage($TMG_LayerOptionsPage);
	TMG_LayersImportExportBook.selectPage($TMG_LayersImportExportPage);
	TMG_MainBook.selectPage($TMG_CurrentPage);
	TMG.setCenteredTerrain(TMG.centerImportTerrain);

	if ($InGuiEditor)
		return;

	if (!TMG.initialized)
		TMG.init();

	TMG.refreshData();
	Lab.hidePluginTools();
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainManagerGui::onSleep(%this) {
	if (isObject(TMG_GroundCoverClone-->MainContainer))
		SEP_GroundCover.add(TMG_GroundCoverClone-->MainContainer);

	Lab.showPluginTools();
}
//------------------------------------------------------------------------------

//==============================================================================
// Called before GUI is saved in GuiEditor
function TerrainManagerGui::onPreEditorSave(%this) {
	if (isObject(TMG_GroundCoverClone-->MainContainer))
		SEP_GroundCover.add(TMG_GroundCoverClone-->MainContainer);
}
//------------------------------------------------------------------------------
//==============================================================================
// Called after GUI is saved in GuiEditor
function TerrainManagerGui::onPostEditorSave(%this) {
	devLog("TerrainManagerGui::onPostEditorSave",TMG_GroundCoverClone-->MainContainer);
}
//------------------------------------------------------------------------------

//==============================================================================
// Common TMG functions
//==============================================================================

//==============================================================================
function TMG::init(%this) {
	TMG.ShowMapPreview = true;
	TMG.setAutoExportMode("Never");
	TerrainManagerGui-->dataFolder.setText("");
	TMG.initialized = true;
}
//------------------------------------------------------------------------------

//==============================================================================
function TMG::refreshData(%this) {
	hide(SEP_GroundCover);
	TMG_GroundCoverClone.add(SEP_GroundCover-->MainContainer);
	TMG.updateTerrainList(true);
	%this.refreshMaterialLayersPage();
	%this.buildTerrainInfoParams();
}
//------------------------------------------------------------------------------




//==============================================================================
function TMG::toggleTools(%this,%button) {
	%visible = Lab.togglePluginTools();
	%button.setStateOn(%visible);
}
//------------------------------------------------------------------------------


//==============================================================================
function TMG_MainBook::onTabSelected(%this,%text,%id) {
	$TMG_CurrentPage = %id;
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG_LayersImportExportBook::onTabSelected(%this,%text,%id) {
	$TMG_LayersImportExportPage = %id;
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG_LayerOptionsBook::onTabSelected(%this,%text,%id) {
	$TMG_LayerOptionsPage = %id;
}
//------------------------------------------------------------------------------
