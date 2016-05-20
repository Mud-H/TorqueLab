//==============================================================================
// TorqueLab -> TerrainMaterialManager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function TMG::setActiveTerrain(%this,%terrainId) {
	/*	if (isObject(TMG.activeTerrain) && isObject(TMG.activeTerrain)){
			if (TMG.activeTerrain.getId() $= %terrainId.getId()){
				warnlog(TMG.activeTerrain.getName(),"Terrain is already active");
				return;
			}
		}
	*/
	TMG.activeDataUpdated = false;

	if (!isObject(%terrainId)) {
		TMG.activeTerrain = "";
		TMG.activeHeightInfo = "";
	}

	TMG.activeTerrain = %terrainId;
	TMG.activeTerrainId = %terrainId.getId();
	TMG_ActiveTerrainMenu.setText(TMG.activeTerrain.getName());
	%this.getActiveFolders();
	//===========================================================================
	// GENERAL TAB INFORMATIONS
	TMG_ActiveTerrainNameEdit.setText(TMG.activeTerrain.getName());
	TMG_ActiveTerrainNameApply.active = 0;
	TMG_ActiveTerrainFileEdit.setText(TMG.activeTerrain.terrainFile);
	TMG_ActiveTerrainFileApply.active = 0;
	//---------------------------------------------------------------------------
	TMG_PageMaterialLayers-->heightmapModeStack-->Current.visible = !%terrainId.isNew;

	//TMG_MaterialLayersNewTerrain.visible = %terrainId.isNew;
	if (%terrainId.isNew) {
		if (TMG.heightmapMode $= "Current")
			TMG.changeHeightmapMode("","Source");

		%terrainName = getUniqueName("theTerrain");
	} else {
		TMG.changeHeightmapMode("","Current");
		%terrainName = %terrainId.getName();
		ETerrainEditor.attachTerrain(%terrainId);
		TMG.activeHeightInfo = ETerrainEditor.getHeightRange();
		TMG.activeHeightRangeInfo = TMG.activeHeightInfo.x SPC "\c1(\c2"@TMG.activeHeightInfo.y@"\c1/\c2"@TMG.activeHeightInfo.z@"\c1)";
		TMG.activeHeightRange = "\c2"@TMG.activeHeightInfo.x;
		TMG.activeHeightMin = "\c3"@TMG.activeHeightInfo.y;
		TMG.activeHeightMax = "\c4"@TMG.activeHeightInfo.z;
		TMG_HeightmapOptions-->heightScale.setText(TMG.activeHeightInfo.x);
		TMG_HeightmapOptions-->squareSize.setText(%terrainId.squareSize);
		TMG.activeTexturesCount = ETerrainEditor.getNumTextures();
		TMG.infoTexturesCount = "All terrains textures used:\c2" SPC TMG.activeTexturesCount;
		TMG.infoTexturesActive = "Active terrain textures used:\c2" SPC getRecordCount(ETerrainEditor.getMaterials());
		TMG.infoBlockCount = "Total terrain blocks:\c2" SPC ETerrainEditor.getTerrainBlockCount();
		TMG.infoBlockMatCount = "Total block materials:\c2" SPC getRecordCount(ETerrainEditor.getTerrainBlocksMaterialList());
		TMG.infoBlockList = "Terrain blocks list:\c2" SPC ETerrainEditor.getTerrainBlocksMaterialList();
		TMG_PageGeneral-->storeFolders.setStateOn(%terrainId.storeFolders);
		TMG.infoActions1 = "Totals actions:\c2" SPC ETerrainEditor.getActionName(1);
		syncParamArray(TMG.terrainArray);
		TMG_PageMaterialLayers-->terrainX.setText(%terrainId.position.x);
		TMG_PageMaterialLayers-->terrainY.setText(%terrainId.position.y);
		TMG_PageMaterialLayers-->terrainZ.setText(%terrainId.position.z);
	}

	%this.validateImportTerrainName(%terrainName);
	%this.getTerrainHeightmapName();
	%this.updateTerrainLayers();
	devLog("updateMaterialLayers from setActiveTerrain");

	if (ETerrainEditor.getMaterials() $= "")
		TMG.schedule(1000,"updateMaterialLayers");
	else
		%this.updateMaterialLayers();

	if (TMG.AutoGenerateHeightmap)
		%this.prepareHeightmap();

	TMG.activeDataUpdated = true;
}
//------------------------------------------------------------------------------

//==============================================================================
function TMG::updateTerrainList(%this,%selectCurrent) {
	TMG_ActiveTerrainMenu.clear();
	%list = getMissionObjectClassList("TerrainBlock");

	if (getWordCount(%list) < 1) {
		TMG_ActiveTerrainMenu.add("No terrain found",0);
		TMG_ActiveTerrainMenu.setText("No terrain found");
		return;
	}

	foreach$(%terrain in %list) {
		TMG_ActiveTerrainMenu.add(%terrain.getName(),%terrain.getId());
	}

	//TMG_ActiveTerrainMenu.add("New terrain",0);

	if (!%selectCurrent)
		return;

	if (!isObject(TMG.activeTerrain))
		TMG.setActiveTerrain(getWord(%list,0));
	else if (%selectCurrent)
		TMG.setActiveTerrain(TMG.activeTerrain);

	//TMG_ActiveTerrainMenu.setSelected(TMG.activeTerrain.getId());
}
//------------------------------------------------------------------------------


//==============================================================================
function TMG_ActiveTerrainMenu::onSelect(%this,%id,%text) {
	if (%id $= "0") {
		TMG.activeTerrain = newScriptObject("TMG_NewTerrain");
		TMG_NewTerrain.isNew = true;
		TMG_NewTerrain.dataFolder =
			%id = TMG.activeTerrain.getId();
	}

	if (!isObject(%id))
		return;

	TMG.setActiveTerrain(%id);
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::storeFolderToTerrain(%this,%storeToTerrain) {
	if (!isObject(TMG.ActiveTerrain))
		return;

	if (!%storeToTerrain && TMG.ActiveTerrain.getFieldValue("storeFolders") $= "")
		return;

	TMG.ActiveTerrain.setFieldValue("storeFolders",%storeToTerrain);
}
//------------------------------------------------------------------------------
//==============================================================================
// Export Single Layer Map
//==============================================================================

//==============================================================================
function TMG::getTerrainHeightmapName(%this) {
	TMG.activeHeightmapName = "";

	if (!isObject(TMG.ActiveTerrain))
		return;

	%heightMapName = TMG.ActiveTerrain.getName()@"_hm_"@TMG.ActiveTerrain.squareSize;

	if (TMG.activeHeightRange !$="")
		%heightMapName = %heightMapName @"_"@mCeil(TMG.activeHeightInfo.x);

	TMG.activeHeightmapName = %heightMapName;
	return %heightMapName;
}
//------------------------------------------------------------------------------
