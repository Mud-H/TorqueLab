//==============================================================================
// TorqueLab -> TerrainMaterialManager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// TerrainObject Functions
//==============================================================================

//==============================================================================
// Manage the terrain name
//==============================================================================
//==============================================================================
// Sync the current profile values into the params objects
function TMG::applyTerrainName( %this ) {
	%newName = TMG_ActiveTerrainNameEdit.getText();

	if (%newName $= TMG.activeTerrain.getName())
		return;

	if (isObject(%newName)) {
		warnLog("There's already an object using that name:",%newName);
		return;
	}

	TMG.activeTerrain.setName(%newName);
	TMG.setActiveTerrain(TMG.activeTerrain);
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the current profile values into the params objects
function TMG_ActiveTerrainNameEdit::onValidate( %this ) {
	%newName = TMG_ActiveTerrainNameEdit.getText();

	if (%newName $= TMG.activeTerrain.getName()) {
		TMG_ActiveTerrainNameApply.active = 0;
		return;
	}

	TMG_ActiveTerrainNameApply.active = 1;
}
//------------------------------------------------------------------------------
//==============================================================================
// Manage the terrain file
//==============================================================================

//==============================================================================
// Sync the current profile values into the params objects
function TMG::relocateTerrainFile( %this ) {
	%newFile = TMG_ActiveTerrainFileEdit.getText();

	if (%newFile $= TMG.activeTerrain.terrainFile) {
		TMG_ActiveTerrainFileApply.active = 0;
		return;
	}

	%fileBase = fileBase(%newFile);
	%filePath = filePath(%newFile);
	%file = %filePath@"/"@%fileBase@".ter";
	TMG.saveTerrain(TMG.activeTerrain,%file);
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::saveTerrain(%this,%obj,%file) {
	if (%obj $= "")
		%obj = TMG.activeTerrain;

	if (!isObject(%obj))
		return;

	if (%file $= "")
		%file = addFilenameToPath(filePath(MissionGroup.getFileName()),%obj.getName(),"ter");

	%obj.save(%file);
	TMG.schedule(500,"updateTerrainFile",%obj,%file);
	devLog("Terrain:",%obj.getName(),"Saved to",%file);
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::updateTerrainFile(%this,%obj,%file) {
	if (!isObject(%obj))
		return;

	%obj.setFieldValue("terrainFile",%file);
	devLog("Terrain:",%obj.getName(),"terrainFileSet to",%file);
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the current profile values into the params objects
function TMG::getTerrainFile( %this ) {
	%currentFile = TMG.activeTerrain.terrainFile;
	//Canvas.cursorOff();
	getLoadFilename("*.*|*.*", "TMG.setTerrainFile", %currentFile);
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the current profile values into the params objects
function TMG::setTerrainFile( %this,%file ) {
	%fileBase = fileBase(%file);
	%filePath = filePath(%file);
	%newFile = %filePath@"/"@%fileBase@".ter";
	%filename = makeRelativePath( %newFile, getMainDotCsDir() );
	TMG.saveTerrain(TMG.activeTerrain, %filename);
	//%this.updateTerrainField("terrainFile",%filename);
}
//------------------------------------------------------------------------------

//==============================================================================
function TMG::updateTerrainField(%this,%field,%value) {
	%terrain = TMG.activeTerrain;
	%terrain.setFieldValue(%field,%value);
}
//------------------------------------------------------------------------------


//==============================================================================
// Terrain Layers Functions
//==============================================================================
//==============================================================================
function TMG::updateTerrainLayers(%this,%clearOnly) {
	TMG_TerrainLayerStack.clear();
	hide(TMG_TerrainLayerPill);
	show(TMG_TerrainLayerStack);

	if (%clearOnly)
		return;

	%mats = ETerrainEditor.getMaterials();

	for( %i = 0; %i < getRecordCount( %mats ); %i++ ) {
		%matInternalName = getRecord( %mats, %i );
		%mat = TerrainMaterialSet.findObjectByInternalName( %matInternalName );
		%pill = cloneObject(TMG_TerrainLayerPill);
		%pill.matObj = %mat;
		%pill.layerId = %i;
		%pill.internalName = "Layer_"@%i;
		%pill-->materialName.text = "Material:\c1" SPC %mat.internalName;
		%pill-->materialMouse.pill = %pill;
		%pill-->materialMouse.superClass = "TMG_GeneralMaterialMouse";
		%pill-->materialMouse.callback = "TMG_GeneralMaterialChangeCallback";
		TMG_TerrainLayerStack.add(%pill);
	}
}
//------------------------------------------------------------------------------
function TMG_GeneralMaterialMouse::onMouseDown(%this,%modifier,%mousePoint,%mouseClickCount) {
	if (%mouseClickCount > 1) {
		TMG.showGeneralMaterialDlg(%this.pill);
	}
}
//==============================================================================
function TMG::showGeneralMaterialDlg( %this,%pill ) {
	if (!isObject(%pill)) {
		warnLog("Invalid layer to change material");
		return;
	}

	if (%callback $= "")
		%callback = "TMG_LayerMaterialChangeCallback";

	%mat = %pill.matObj;
	TMG.changeMaterialPill = %pill;
	TMG.changeMaterialLive = %directUpdate;
	TerrainMaterialDlg.show( %pill.layerId, %mat,TMG_GeneralMaterialChangeCallback );
}
//------------------------------------------------------------------------------
//==============================================================================
// Callback from TerrainMaterialDlg returning selected material info
function TMG_GeneralMaterialChangeCallback( %mat, %matIndex, %activeIdx ) {
	TMG_LayerMaterialChangeCallback(%mat, %matIndex, %activeIdx );
	EPainter_TerrainMaterialUpdateCallback(%mat, %matIndex);
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::importLayerTextureMap(%this,%matInternalName) {
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::exportLayerTextureMap(%this,%matInternalName) {
}
//------------------------------------------------------------------------------

//==============================================================================
// TMG.getTerrainHeightRange
function TMG::getTerrainHeightRange( %this ) {
	%heightRange = ETerrainEditor.getHeightRange();
	return %heightRange;
}
//------------------------------------------------------------------------------
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
