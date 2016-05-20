//==============================================================================
// TorqueLab -> TerrainMaterialManager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TMG_ExportLayerFormat = "png";
//==============================================================================
function TMG::initExporter( %this) {
}
//------------------------------------------------------------------------------

//==============================================================================
function TMG::exportEverything( %this,%target,%noOverwrite) {
	%terObj = %this.activeTerrain;

	if (!isObject(%terObj)) {
		warnlog("Trying to export layers and heightmap for invalid terrain:",%terObj);
		return;
	}

	if (%terObj.isNew) {
		warnLog("You must import an heightmap on a new terrain before exporting.");
		return;
	}

	if (%target $= "") {
		getFolderName("","TMG.exportEverything",TMG.dataFolder,"Select destination folder");
		return;
	}

	%this.exportHeightMap(%target,%noOverwrite);
	%this.exportTerrainLayersToPath(%target,"png");
}
//------------------------------------------------------------------------------

//==============================================================================
function TMG::exportTerrainData( %this,%target) {
	if (%target $= "") {
		getFolderName("","TMG.exportTerrainData","Select folder to export terrain data");
		return;
	}

	if (%target $= "Source")
		%folder = TMG.sourceFolder;
	else if (%target $= "Target")
		%folder = TMG.targetFolder;
	else if (%target $= "Terrain")
		%folder = TMG.terrainFolder;
	else
		%folder = %target;

	if (!isDirectory(%folder)) {
		warnLog("Invalid folder specified for export:",%folder);
		return;
	}

	%exportHeightmap = TMG_ExportTypeRadios-->exportHeightmap.isStateOn();
	%exportTexturemap = TMG_ExportTypeRadios-->exportTexturemap.isStateOn();
	%exportAll = TMG_ExportTypeRadios-->exportAll.isStateOn();

	if (%exportHeightmap || %exportAll)
		%this.exportHeightMap(%folder,%noOverwrite);

	if (%exportTexturemap || %exportAll)
		%this.exportTerrainLayersToPath(%folder,"png");
}
//------------------------------------------------------------------------------

//==============================================================================
// Export Functions
//==============================================================================
function TMG::exportHeightMapToFile( %this) {
	getSaveFilename("Png Files|*.png","TMG.exportHeightMapFile",TMG.dataFolder,true);
}
//==============================================================================
function TMG::exportHeightMap( %this,%folder,%noOverwrite) {
	devLog("exportHeightMap folder",%folder);
	%terObj = %this.activeTerrain;

	if (!isObject(%terObj)) {
		warnlog("Trying to export layers for invalid terrain:",%terObj);
		return;
	}

	if ($TMG_ExportLayerFormat $= "")
		$TMG_ExportLayerFormat = "png";

	if (%folder $= "")
		%folder = %terObj.dataFolder;

	if (%folder $= "") {
		%baseFile = MissionGroup.getFilename();
		%folder = filePath( MissionGroup.getFilename());
	}

	if ($TMG_ExportFolderName $= "")
		$TMG_ExportFolderName = "Default";

	%exportPath = %folder@"/"@$TMG_ExportFolderName;
	%hmName = TMG.getTerrainHeightmapName();
	%filePrefix = %hmName@".png";
	%exportFile = %folder @ "/" @ %filePrefix;
	%result = %this.exportHeightMapFile(%exportFile,"png",%noOverwrite);
	//%ret = %terObj.exportHeightMap( %folder @ "/" @ %filePrefix,"png" );

	//devLog("Terrain HeightMap exported to:",%folder @ "/" @ %filePrefix,"Result=",%ret);
	if (%result)
		return %folder @ "/" @ %filePrefix;

	return "";
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::exportHeightMapFile( %this,%file,%format,%noOverwrite) {
	%terObj = %this.activeTerrain;

	if (!isObject(%terObj)) {
		warnlog("Trying to export layers for invalid terrain:",%terObj);
		return;
	}

	if (%format $= "")
		%format = "png";

	if (%noOverwrite)
		devLog("No overwrite");

	if (isFile(%file))
		devLog("File exist");

	if (isFile(%file) && %noOverwrite) {
		%newFile = filePath(%file)@"/"@fileBase(%file)@"_"@%inc++@fileExt(%file);

		while (isFile(%newFile)) {
			%newFile = filePath(%file)@"/"@fileBase(%file)@"_"@%inc++@fileExt(%file);

			if (%inc > 20) {
				warnLog("Couldn't find a unique file name after 20 attempts. Export aborted!");
				return false;
			}
		}

		%file = %newFile;
	}

	%ret = %terObj.exportHeightMap( %file,%format );

	if (%ret)
		info("Heightmap exported to:",%file);
	else
		info("Heightmap failed to export to:",%file,"See console for report");

	return %ret;
}
//------------------------------------------------------------------------------

//==============================================================================
function TMG::exportTerrainLayersToPath( %this,%exportPath,%format,%layerId) {
	devLog("exportTerrainLayersToPath %exportPath,%format,%layerId",%exportPath,%format,%layerId);
	%terObj = %this.activeTerrain;

	if (!isObject(%terObj)) {
		warnlog("Trying to export layers for invalid terrain:",%terObj);
		return;
	}

	if (%terObj.isNew) {
		warnLog("You must import an heightmap on a new terrain before exporting.");
		return;
	}

	if (%format $= "")
		%format = "png";

	if (%exportPath $= "") {
		getFolderName("","TMG.exportTerrainLayersToPath",TMG.dataFolder,"Select destination folder",%format, %layerId);
		return;
	}

	if (%exportPath $= "Default") {
		%exportPath = TMG.terrainFolder;
	}

	%filePrefix = %terObj.getName() @ "_layerMap";

	if (%layerId !$= "") {
		%ret = %terObj.exportSingleLayerMap( %layerId, %exportPath @ "/" @ %filePrefix, %format );
		devLog(%layerId," layer exported to:",%exportPath @ "/" @ %filePrefix,"Result=",%ret);
	} else {
		%ret = %terObj.exportLayerMaps( %exportPath @ "/" @ %filePrefix, "png" );
		devLog("Terrain exported to:",%exportPath @ "/" @ %filePrefix,"Result=",%ret);
	}
}
//------------------------------------------------------------------------------