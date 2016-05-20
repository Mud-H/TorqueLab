//==============================================================================
// TorqueLab -> TerrainMaterialManager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TMG_TerrainTextureFormats = "JPG DDS PNG";
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_ScatterSkyManager.buildParams();
function TMG::buildTerrainInfoParams( %this ) {
	%arCfg = Lab.createBaseParamsArray("TMG_Terrain",TMG_TerrainArrayStack);
	%arCfg.updateFunc = "TMG.updateTerrainParam";
	%arCfg.style = "StyleA";
	%arCfg.useNewSystem = true;
	%arCfg.group[%gid++] = "TerrainBlock Parameters";
	%arCfg.setVal("terrainFile",       "" TAB "terrainFile" TAB "FileSelect" TAB "callback>>TMG.getTerrainFile();" TAB "TMG.activeTerrain" TAB %gid);
	%arCfg.setVal("castShadows",        "" TAB "castShadows" TAB "Checkbox" TAB "" TAB "TMG.activeTerrain" TAB %gid);
	%arCfg.setVal("squareSize",   "" TAB "squareSize" TAB "SliderEdit" TAB "range>>0 10;;tickAt>>0.1" TAB "TMG.activeTerrain" TAB %gid);
	%arCfg.setVal("baseTexSize",        "" TAB "baseTexSize" TAB "TextEdit" TAB "" TAB "TMG.activeTerrain" TAB %gid);
	%arCfg.setVal("baseTexFormat",        "" TAB "baseTexFormat" TAB "DropDown" TAB "itemList>>$TMG_TerrainTextureFormats" TAB "TMG.activeTerrain" TAB %gid);
	%arCfg.setVal("lightMapSize",        "" TAB "lightMapSize" TAB "TextEdit" TAB "" TAB "TMG.activeTerrain" TAB %gid);
	%arCfg.setVal("screenError",        "" TAB "screenError" TAB "TextEdit" TAB "" TAB "TMG.activeTerrain" TAB %gid);
	buildParamsArray(%arCfg,isObject(TMG.activeTerrain));
	%this.terrainArray = %arCfg;
}
//------------------------------------------------------------------------------
//syncParamArray(TMG.terrainArray);
//==============================================================================
function TMG::updateTerrainParam(%this,%field,%value,%ctrl,%array) {
}
//------------------------------------------------------------------------------
