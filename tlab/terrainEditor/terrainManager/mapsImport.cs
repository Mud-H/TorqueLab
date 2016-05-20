//==============================================================================
// TorqueLab -> TerrainManager - Import Heightmaps and Layers
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$pref::Directories::Terrain = "art/Levels/FarmDemo/";

$TMG_DefaultHeightmap_Invalid = "tlab/terrainEditor/gui/images/heightmapInvalid.png";
$TMG_DefaultHeightmap_NotGenerated = "tlab/terrainEditor/gui/images/heightmapN.png";
$TMG_DefaultHeightmap_Invalid = "tlab/terrainEditor/gui/images/heightmapInvalid.png";
//==============================================================================
// Prepare and import the terrain HeightMap (Manager Import Terrain Button)
//==============================================================================
//==============================================================================
function TMG::updateImportHeightmapBmp(%this,%doImport) {
	if (TMG.heightmapMode $= "Source") {
		%hmMenu = TMG_PageMaterialLayers-->heightMapMenu;
		%heightmapSrc = TMG_SourceHeightMapMenu.getText();
		%heightmapFile = TMG.SourceFolder@"/"@%heightmapSrc;

		if (!isFile(%heightmapFile))
			%bmpFile = "tlab/terrainEditor/gui/images/heightmapNotSelected.png";
		else
			%bmpFile = %heightmapFile;
	} else if (TMG.heightmapMode $= "Current") {
		%heightmapFile = TMG.terrainHeightMap;

		if (!isFile(%heightmapFile)) {
			TMG_GenerateHeightmapButton.visible = 1;
			%bmpFile = "tlab/terrainEditor/gui/images/heightmapNotGenerated.png";
		} else {
			TMG_GenerateHeightmapButton.visible = 0;
			%bmpFile = %heightmapFile;
		}
	} else if (TMG.heightmapMode $= "Browse") {
		%heightmapFile = TMG.browseHeightMap;

		if (!isFile(%heightmapFile))
			%bmpFile = "tlab/terrainEditor/gui/images/heightmapNotSelected.png";
		else
			%bmpFile = %heightmapFile;
	}

	//%heightmapFile = TMG.currentHeightMap;
	%heightmapFile = validatePath(%heightmapFile,true);

	if(!isFile(%heightmapFile)) {
		warnLog("Invalid file or terrainObj file:",	%heightmapFile);
		%heightmapFile = "";
		%bmpFile = "tlab/terrainEditor/gui/images/heightmapInvalid.png";
	}

	TMG_ImportHeightMapBmp.setBitmap(%bmpFile);
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::prepareAllLayers(%this,%doImport) {
	%folder = TMG.DataFolder@"/TerData/"@TMG.activeTerrain.getName();
	%this.prepareOpacityMaps(%folder);

	foreach(%pill in TMG_MaterialLayersStack) {
		devLog("Prepare layer file:",%pill.file);

		if (%pill.useDefaultLayer && !isFile(%pill.file)) {
			%pill.file = %folder@"/"@%pill.defaultLayerFile@".png";
		}

		devLog("DefaultLayerFile = ",%pill.defaultLayerFile);
		devLog("Pill File 0 = ",%pill-->mapMenu.file[0]);

		if (!isFile(%pill.file)) {
			%pill.file = %folder@"/"@%pill-->mapMenu.file[0]@".png";
			warnLog(%pill.layerId," Layer file created from menu text:",%pill.file);
		}

		devLog("Final Pill File= ",%pill.file );
		%pill.activeChannels = "";
		%stack = %pill-->channelStack;

		foreach$(%chan in %pill.channelRadios) {
			%radio = %stack.findObjectByInternalName(%chan);

			if (%radio.isStateOn())
				%pill.activeChannels = strAddWord(%pill.activeChannels,%chan);
		}
	}

	%this.prepareHeightmap(%folder);

	if (%doImport)
		%this.importTerrain();

	%this.updateImportHeightmapBmp();
}
//------------------------------------------------------------------------------

//==============================================================================
function TMG::prepareHeightmap(%this,%folder) {
	if (%folder $= "")
		%folder = TMG.DataFolder@"/TerData/"@TMG.activeTerrain.getName();

	TMG.terrainHeightMap = %this.exportHeightMap(%folder);
	%this.updateImportHeightmapBmp();
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::prepareOpacityMaps(%this,%folder) {
	if (%folder $= "")
		%folder = TMG.DataFolder@"/TerData/"@TMG.activeTerrain.getName();

	%this.exportTerrainLayersToPath(%folder,"png");
}
//------------------------------------------------------------------------------
//==============================================================================
// Import Terrain Heightmap + Layers
//==============================================================================
//==============================================================================
function TMG::importTerrain(%this) {
	%terObj = %this.activeTerrain;
	%folder = TMG_PageMaterialLayers-->textureSourceFolder.text;
	%hmMenu = TMG_PageMaterialLayers-->heightMapMenu;

	if (%hmMenu.getSelected() !$= "0") {
		%file = %folder @"/"@%hmMenu.getText();
		%file = strreplace(%file,"//","/");
		TMG.currentHeightMap = %file;
	}

	TMG.currentHeightMap = strreplace(TMG.currentHeightMap,"//","/");
	%heightmapFile = TMG.currentHeightMap;

	if (TMG.heightmapMode $= "Current") {
		%heightmapFile = TMG.terrainHeightMap;
		devLog("Reimporting current heightmap:",%heightmapFile);
	} else if (TMG.heightmapMode $= "Source") {
		%heightmapSrc = %hmMenu.getText();
		%heightmapFile = TMG.SourceFolder@"/"@%heightmapSrc;
		devLog("Reimporting with heightmap:",%heightmapSrc,"File:",%heightmapFile);
	}

	if(!isFile(%heightmapFile)) {
		warnLog("Invalid file or terrainObj terrain:",	%terObj);
		warnLog("Invalid file or terrainObj file:",	TMG.currentHeightMap);
		return;
	}

	%metersPerPixel = TMG_HeightmapOptions-->squareSize.getText();
	%heightScale = TMG_HeightmapOptions-->heightScale.getText();
	%flipYAxis = TMG_HeightmapOptions-->flipAxisCheck.isStateOn();

	foreach(%pill in TMG_MaterialLayersStack) {
		%fixFile = %pill.file;

		if (!isFile(%fixFile))
			%preImportFailed = true;

		%opacityNames = strAddRecord(%opacityNames,%fixFile TAB %pill.activeChannels);
		%materialNames = strAddRecord(%materialNames,%pill.matInternalName);
	}

	if (%preImportFailed) {
		devLog("Pre heightmap import failed! Importing heightmap with",getRecordCount(%opacityNames)," opacity maps and ",getRecordCount(%materialNames),"Materials.");
		devLog("Opacity list:",%opacityNames);
		devLog("Material list:",%materialNames);
		return;
	}

	%name = TMG_PageMaterialLayers-->importTerrainName.getText();
	%bmpInfo = getBitmapinfo(%heightmapFile );
	devLog("HeightMap info:",%bmpInfo);
	%size = getWord(%bmpInfo,0);
	%worldSize = (%size * %metersPerPixel);
	%position = TMG_PageMaterialLayers-->terrainX.getText() SPC TMG_PageMaterialLayers-->terrainY.getText() SPC TMG_PageMaterialLayers-->terrainZ.getText();
	devLog("Position PRE:",%position);

	if (TMG.centerImportTerrain) {
		%position.x =  %worldSize/-2;
		%position.y =  %worldSize/-2;
	}

	devLog("Import terrain:",%name,"SquareSize",%metersPerPixel,"HeightScale",%heightScale,"Flipped",%flipYAxis);
	//%saveToFile = filePath(MissionGroup.getFileName());
	//if (isObject(%name))
	//	%saveToFile = %name.getFileName();
	//%defaultTerrainDir = $pref::Directories::Terrain;
	//$pref::Directories::Terrain = %saveToFile;
	delObj(%name);
	%updated = nameToID( %name );
	%obj = TerrainBlock::import(  %name,
											%heightmapFile,
											%metersPerPixel,
											%heightScale,
											%opacityNames,
											%materialNames,
											%flipYAxis );
	%obj.terrainHeight = %heightScale;
	%obj.dataFolder = TMG.dataFolder;
	%obj.sourceFolder = TMG.sourceFolder;
	%obj.targetFolder = TMG.targetFolder;
	%obj.position = %position;
	//%obj.setFilename(%saveToFile);

	if ( isObject( %obj ) ) {
		if( %obj != %updated ) {
			// created a new TerrainBlock
			// Submit an undo action.
			MECreateUndoAction::submit(%obj);
		}

		%obj.schedule(500,"setPosition",%position);
		assert( isObject( EWorldEditor ),
				  "ObjectBuilderGui::processNewObject - EWorldEditor is missing!" );
		// Select it in the editor.
		EWorldEditor.clearSelection();
		EWorldEditor.selectObject(%obj);
		// When we drop the selection don't store undo
		// state for it... the creation deals with it.
		EWorldEditor.dropSelection( true );
		ETerrainEditor.isDirty = true;
		EPainter.updateLayers();
		TMG.activeTerrain = %obj;
		TMG.updateTerrainList(true);
		//TMG.saveTerrain();
		//ETerrainEditor.saveTerrainToFile(%obj);
		//TMG.setActiveTerrain(%obj);
	} else {
		warnLog("Something bad happen and heightmap import failed!");
		devLog("Importing heightmap with",getRecordCount(%opacityNames)," opacity maps and ",getRecordCount(%materialNames),"Materials.");
	}

	//$pref::Directories::Terrain = %defaultTerrainDir;
}


//------------------------------------------------------------------------------
//==============================================================================
// Export Single Layer Map
//==============================================================================

//==============================================================================
function TMG::selectSingleTextureMapFolder( %this, %layerId) {
	%folder = TerrainManagerGui-->dataFolder.getText();
	getFolderName(%filter,"TMG.setSingleTextureMapFolder",%folder,"Select Export Folder",%layerId);
}
//------------------------------------------------------------------------------
function TMG::setSingleTextureMapFolder( %this, %path,%layerId) {
	%this.exportTerrainLayersToPath(%path,"png",%layerId);
}
//------------------------------------------------------------------------------

//==============================================================================
function TMG_ImportTerrainNameEdit::onValidate(%this) {
	%name = %this.getText();
	TMG.validateImportTerrainName(%name);
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::validateImportTerrainName(%this,%name) {
	if (isObject(%name)) {
		%info = "\c2Terrain exist and will be overridden with heightmap";
	} else {
		%info = "\c1New terrain will be created from heightmap";
	}

	TMG_PageMaterialLayers-->importTerrainNameStatus.setText(%info);
	TMG_PageMaterialLayers-->importTerrainName.setText(%name);
}
//------------------------------------------------------------------------------
//==============================================================================
// Heightmap for re-importing
//==============================================================================
//==============================================================================
function TMG::changeHeightmapMode( %this, %ctrl, %mode) {
	TMG.heightmapMode = %mode;

	if (TMG.heightmapMode $= "")
		TMG.heightmapMode = %ctrl.internalName;

	TMG_HeightmapOptions-->flipAxisCheck.active = 1;
	TMG_PageMaterialLayers-->currentHeightmap.visible = 0;
	TMG_PageMaterialLayers-->browseHeightmap.visible = 0;
	TMG_PageMaterialLayers-->sourceHeightmap.visible = 0;
	eval("TMG_PageMaterialLayers-->"@TMG.heightmapMode@"Heightmap.visible = 1;");
	eval("TMG_PageMaterialLayers-->heightmapModeStack-->"@TMG.heightmapMode@".setStateOn(true);");

	if (%mode $= "Current") {
		TMG_HeightmapOptions-->flipAxisCheck.setStateOn(true);
		//TMG_HeightmapOptions-->flipAxisCheck.active = 1;
	}

	TMG.updateImportHeightmapBmp();
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG_SourceHeightMapMenu::onSelect( %this, %id, %text) {
	TMG.updateImportHeightmapBmp();
}
//------------------------------------------------------------------------------
function TMG::selectHeightMapImage( %this) {
	getLoadFilename("Png Files|*.png","TMG.setHeightMapImage",TMG.terrainHeightMap);
}
//==============================================================================
function TMG::setHeightMapImage( %this,%file) {
	%file = validatePath(%file,true);
	devLog("HeightMap selected:",%file);
	TMG.browseHeightMap = %file;
	TMG_PageMaterialLayers-->heightmapFile.setText(%file);
	TMG.updateImportHeightmapBmp();
}
function TMG_FlipAxisCheck::onClick( %this) {
	if (TMG.heightmapMode $= "Current" && !%this.isStateOn()) {
		LabMsgOkCancel("Terrain will be inverted","Torque3D export the terrain heightmap with flipped Y axis. If you choose to not flip the Y axis while importing current terrain heightmap" SPC
							"exported from Torque3D, your terrain will be inverted. If you also use exported layer maps, those won't fit with terrain shape. If you want to toggle Y Axis Flipping off, click OK.",
							"","TMG_HeightmapOptions-->flipAxisCheck.setStateOn(true);");
	}
}
function TMG_CenterTerrainCheckbox::onClick( %this) {
	TMG.setCenteredTerrain(%this.isStateOn());
}
function TMG::setCenteredTerrain( %this,%isCentered) {
	if(%isCentered) {
		TMG_ImportOptions-->TerrainX.active = 0;
		TMG_ImportOptions-->TerrainY.active = 0;
		return;
	}

	TMG_ImportOptions-->TerrainX.active = 1;
	TMG_ImportOptions-->TerrainY.active = 1;
}
