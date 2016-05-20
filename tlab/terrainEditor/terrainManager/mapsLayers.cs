//==============================================================================
// TorqueLab -> TerrainMaterialManager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function TMG::refreshMaterialLayersPage(%this) {
	%info = "The material layers page allow to simply reimport your terrain with current or new texture map." SPC
			  "Those maps can be produced by external tools like L3TD or WorldMachine. Simply specify the folder containing" SPC
			  "the sources texture map and select the map you want to use for each terrain layers. Then you select the color channel" SPC
			  "you want to use. You can also assign a new heightmap which would be use to recreate the terrain (Similar to standard import" SPC
			  "but you keep your current material layers information).";
	TMG_PageMaterialLayers-->materialLayersInfo.setText(%info);
	//Change heightmap mode to current mode, else it will use the default current mode
	TMG.changeMapFolderMode(TMG_PageMaterialLayers-->mapModeStack-->relative,TMG.mapFolderMode);
	//TMG.updateMaterialLayers();
}
//------------------------------------------------------------------------------

//==============================================================================
function TMG::scanTextureMapFolder(%this) {
	TMG.textureMapList = "";
	TMG.customMapList = "";
	%folder = TMG.sourceFolder;

	if (!isDirectory(%folder)) {
		warnLog("Invalid folder specified:",%folder," Make sure the folder exist and contain the images sources");
		TMG.textureMapList = "Nothing found in scanned folder." TAB "-" TAB "-";
		return;
	}

	%files = getMultiExtensionFileList(%folder,"png bmp tga jpg");

	for(%i = 0; %i < getRecordCount(%files); %i++) {
		%file = %folder@getRecord(%files,%i);
		%file = validatePath(%file);
		%this.addTextureMap(%file);
	}

	return;

	if (!isDirectory(TMG.terrainFolder))
		return;

	%files = getMultiExtensionFileList(TMG.terrainFolder,"png bmp tga jpg");

	for(%i = 0; %i < getRecordCount(%files); %i++) {
		%file = %folder@getRecord(%files,%i);
		%file = validatePath(%file);
		%this.addTextureMap(%file,true);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::addTextureMap(%this,%file,%customList) {
	%bmpInfo = getBitmapinfo(%file );
	%fields = %file TAB getWord( %bmpInfo, 2 ) TAB getWord( %bmpInfo, 0 );
	//TMG.textureMapList = strAddRecord(TMG.textureMapList,%fields);

	if (!%customList) {
		TMG.textureMapList = strAddRecord(TMG.textureMapList,%fields);
	} else {
		TMG.customMapList = strAddRecord(TMG.customMapList,%fields);
		devLog("Add custom map:",%file,"Info",%bmpInfo);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Add New Terrain Layers
//==============================================================================
//==============================================================================
function TMG::addMaterialLayer(%this,%matInternalName,%layerId,%mapFile,%channel) {
	if (%layerId $= "") {
		%layerId =TMG_MaterialLayersStack.getCount();
		%isNewMat = true;
	}

	if(%matInternalName $= "") {
		%mats = ETerrainEditor.getMaterials();
		%matInternalName = getRecord(%mats,0);
		%isNewMat = true;
		%isNewMat = true;
	}

	%terObj = %this.activeTerrain;
	%mat = TerrainMaterialSet.findObjectByInternalName( %matInternalName );

	if (!isObject(%mat))
		%matInternalName = "**" @ %matInternalName @ "**";

	%pill = cloneObject(TMG_MaterialLayersPill);
	%pill.matObj = %mat;
	%pill.layerId = %layerId;
	%pill.internalName = "Layer_"@%layerId;
	%pill.matInternalName = %matInternalName;
	%pill-->materialName.text = "Material:\c1" SPC %matInternalName;

	if (%isNewMat) {
		%texture = "Not selected";
		%file = "\"\"";
	} else {
		%texture = "Default Alpha Map -\c2"@%layerId@"_"@%matInternalName;
		%fileName = %terObj.getName()@"_layerMap_"@%layerId@"_"@%matInternalName;
		%layersMapFolder = TMG.getLayersMapFolder();
		%file = addFilenameToPath(%layersMapFolder,%fileName@".png");

		if (!isFile(%file))
			%file = "\"\"";

		//%texture = %terObj.getName()@"_layerMap_"@%layerId@"_"@%matInternalName;
		devLog("Layer default map file=",%file,isFile(%file));
	}

	%file = validatePath(%file);
	%pill.file = %file;
	%pill.texture = %texture;
	%pill.fileName = %fileName;
	%pill.useDefaultLayer = true;
	%pill.defaultLayerFile = %terObj.getName()@"_layerMap_"@%layerId@"_"@%matInternalName;
	%pill-->exportMapBtn.command = "TMG.selectSingleTextureMapFolder("@%layerId@");";
	%pill-->removeMapBtn.command = "TMG.removeLayerMap("@%pill@");";
	%pill-->materialMouse.pill = %pill;
	%pill-->materialMouse.superClass = "TMG_LayerMaterialMouse";
	%previewContainer = %pill-->imageButton.parentGroup.parentGroup;
	%previewContainer.visible = TMG.ShowMapPreview;
	%bmpFile = %file;

	if (!isFile(%bmpFile))
		%bmpFile = "tlab/terrainEditor/gui/images/textureMapNotGenerated.png";

	%pill-->imageButton.setBitmap(%bmpFile);

	foreach(%radio in %pill-->channelStack) {
		%radio.superClass = "TMG_MapChannelRadio";
		%radio.setStateOn(false);
	}

	TMG_MaterialLayersStack.add(%pill);
	%this.updateMaterialLayerMenu(%pill);
	%this.setLayerMapFile(%layerId,%file,"1","r",true);
	return %layerId;
}
//------------------------------------------------------------------------------

function TMG::setMaterialLayerChannel(%this,%layerId,%channel) {
	eval("%pill = TMG_MaterialLayersStack-->Layer_"@%layerId@";");

	if (!isObject(%pill)) {
		warnLog("setLayerMapFile called for invalidPill");
		return;
	}

	switch$(%channel) {
	case "Red":
		%channel = "r";

	case "Green":
		%channel = "g";

	case "Blue":
		%channel = "b";

	case "Alpha":
		%channel = "a";
	}

	eval("%radioCtrl = %pill-->"@%channel@";");

	if (!isObject(%radioCtrl)) {
		warnLog("Can't find channel radio for:",%channel);
		return;
	}

	%radioCtrl.setStateOn(true);
	%pill.activeChannels = %channel;
	devLog("Layer:",%layerId,"Channel set to:",%channel);
}
//==============================================================================
// Terrain Layers Functions
//==============================================================================
//==============================================================================
function TMG::updateMaterialLayers(%this) {
	%terObj = %this.activeTerrain;

	if (TMG.activeDataUpdated) {
		warnLog("Trying to update layers while data is already updated");
		return;
	}

	if (TMG.autoExportLayerMode !$= "Never" ) {
		eval("%exportPath = TMG."@TMG.autoExportLayerMode@"Folder;");
		devLog("Exporting all layers to path:",%exportPath);
		%this.exportTerrainLayersToPath(%exportPath);
	}

	%this.scanTextureMapFolder();
	TMG_PageMaterialLayers-->heightmapCurrentText.text = TMG.activeHeightmapName;
	//Update the default HeightMap Map Menu
	TMG.updateMaterialLayerMenu();
	hide(TMG_MaterialLayersPill);
	show(TMG_MaterialLayersStack);
	TMG_MaterialLayersStack.clear();
	%mats = ETerrainEditor.getMaterials();

	for( %i = 0; %i < getRecordCount( %mats ); %i++ ) {
		%matInternalName = getRecord( %mats, %i );
		%this.addMaterialLayer(%matInternalName,%i);
	}

	TMG_PageMaterialLayers-->reimportButton.active = 0;
}
//------------------------------------------------------------------------------

//==============================================================================
// Material Layer Available Map Menu
//==============================================================================
//==============================================================================
function TMG::updateAllMaterialLayersMenu(%this) {
	foreach(%pill in TMG_MaterialLayersStack)
		%this.updateMaterialLayerMenu(%pill);
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::updateMaterialLayerMenu(%this,%pill) {
	%terObj = %this.activeTerrain;
	%this.scanTextureMapFolder();
	%fullList = TMG.textureMapList;

	//Heightmap Map Menu Update
	if (%pill $= "") {
		%mapMenu = TMG_PageMaterialLayers-->heightMapMenu;
		%mapMenu.clear();
		%mid = -1;
		//%mapMenu.add(%terObj.getName()@"_heightmap.png",0);
		//%mapMenu.file[0] = "default";
	}
	//Pill Map Menu Update
	else {
		if (!isObject(%pill))
			return;

		%mapMenu = %pill-->mapMenu;
		%mapMenu.clear();
		%mapMenu.pill = %pill;
		%mapMenu.layerId = %pill.layerId;
		%mapMenu.add(%pill.texture,0);
		%mapMenu.file[0] = %pill.fileName;
		%mapMenu.channels[0] = "1";
		%mapMenu.command = "TMG.selectLayerMapMenu("@%mapMenu@","@%pill.layerId@");";

		if ( getRecordCount(TMG.customMapList) > 0)
			%fullList = %fullList NL TMG.customMapList;
	}

	//Common Map Menu Update

	for(%j=0; %j<getRecordCount(%fullList); %j++) {
		%record = getRecord(%fullList,%j);
		%file = getField(%record,0);
		%onlyFile = fileBase(%file)@fileExt(%file);
		%mapMenu.add(%onlyFile,%mid++);
		%mapMenu.file[%mid] = %file;
		%mapMenu.channels[%mid] = getField(%record,1);
	}

	%mapMenu.setSelected(0,false);
}
//------------------------------------------------------------------------------

//==============================================================================
function TMG::selectLayerMapMenu(%this,%menu,%layerId,%channels) {
	%pill = %menu.pill;

	if (!isObject(%pill))
		return;

	%pill.useDefaultLayer = false;

	if (%menu.getSelected() $= "0")
		%pill.useDefaultLayer = true;

	%terObj = %this.activeTerrain;
	%layerId = %menu.layerId;
	%file = %menu.file[%menu.getSelected()];
	%channels = %menu.channels[%menu.getSelected()];
	%this.setLayerMapFile(%layerId,%file,%channels);
}
//------------------------------------------------------------------------------

//==============================================================================
function TMG::setLayerMapFile(%this,%layerId,%file,%channels,%activeChannel,%dontAdd) {
	eval("%pill = TMG_MaterialLayersStack-->Layer_"@%layerId@";");

	if (!isObject(%pill)) {
		warnLog("setLayerMapFile called for invalidPill");
		return;
	}

	%file = validatePath(%file);

	if (!isFile(%file)) {
		devLog("InvalidFile:",%file);
		%file = "Invalid file";
	}

	%pill.file = %file;
	%stack = %pill-->channelStack;

	foreach(%radio in %stack) {
		%radio.visible = 0;
		%radio.setStateOn(false);
	}

	if (%channels $= "3") {
		%pill.channelRadios = "R G B";
	} else if (%channels $= "1") {
		%pill.channelRadios = "R";
	} else if (%channels $= "4") {
		%pill.channelRadios = "R G B A";
	}

	%set = false;

	foreach$(%chan in %pill.channelRadios) {
		%radio = %stack.findObjectByInternalName(%chan);
		%radio.visible = 1;

		if (!%set)
			%radio.setStateOn(true);

		%set = true;
	}

	if (%file $= "default" || %dontAdd) {
		return;
	}

	if (isFile(%pill.file))
		%pill-->imageButton.setBitmap(%pill.file);

	%menu = %pill-->mapMenu;
	%nextMenuId = %menu.size();
	%menu.add(%onlyFile,%nextMenuId);
	%menu.file[%nextMenuId] = %file;
	%menu.setText(fileBase(%file));

	if (%activeChannel !$= "")
		%this.setMaterialLayerChannel(%layerId,%activeChannel);
}
//------------------------------------------------------------------------------
//==============================================================================
// Layer Materials Update
//==============================================================================
//==============================================================================
function TMG_LayerMaterialMouse::onMouseDown(%this,%modifier,%mousePoint,%mouseClickCount) {
	if (%mouseClickCount > 1) {
		TMG.showLayerMaterialDlg(%this.pill,%this.callback);
	}
}
//==============================================================================
function TMG::showLayerMaterialDlg( %this,%pill,%callback ) {
	if (!isObject(%pill)) {
		warnLog("Invalid layer to change material");
		return;
	}

	if (%callback $= "")
		%callback = "TMG_LayerMaterialChangeCallback";

	%mat = %pill.matObj;
	TMG.changeMaterialPill = %pill;
	TMG.changeMaterialLive = %directUpdate;
	TerrainMaterialDlg.showByObjectId( %mat, %callback );
}
//------------------------------------------------------------------------------
//==============================================================================
// Callback from TerrainMaterialDlg returning selected material info
function TMG_LayerMaterialChangeCallback( %mat, %matIndex, %activeIdx ) {
	%pill = TMG.changeMaterialPill;

	if (!isObject(%pill)) {
		warnLog("Change material failed because no active layer found");
		return;
	}

	%pill.matObj = %mat;
	%pill.matInternalName = %mat.internalName;
	%layerId = %pill.layerId;

	foreach$(%layersStack in "TMG_MaterialLayersStack TMG_TerrainLayerStack") {
		%layersPill = %layersStack.findObjectByInternalName("Layer_"@%layerId,true);
		%layersPill-->materialName.text = "Material:\c1" SPC %mat.internalName;
	}
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
//==============================================================================
// Terrain Data Folder (used as base for exporting)
//==============================================================================

//==============================================================================
function TMG::removeLayerMap( %this,%pill) {
	delObj(%pill);
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::removeAllLayerMaps( %this) {
	TMG_MaterialLayersStack.deleteAllObjects();
}
//------------------------------------------------------------------------------

//==============================================================================
// Terrain Data Folder (used as base for exporting)
//==============================================================================





//==============================================================================
/*
new TerrainBlock(TerrainTile_x0y0) {
         terrainFile = "art/Levels/Demo/MiniTerrain/MiniTerrainDemo.ter";
         castShadows = "1";
         squareSize = "2";
         baseTexSize = "256";
         baseTexFormat = "JPG";
         lightMapSize = "256";
         screenError = "16";
         position = "-256 -256 0";
         rotation = "1 0 0 0";
         canSave = "1";
         canSaveDynamicFields = "1";
            scale = "1 1 1";
            tile = "0";
      };
*/
