//==============================================================================
// TorqueLab -> TerrainMaterialManager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================

/// The texture filename filter used with OpenFileDialog.
$TerrainEditor::TextureFileSpec = "Image Files (*.png, *.jpg, *.dds)|*.png;*.jpg;*.dds|All Files (*.*)|*.*|";
//==============================================================================
function TerrainEditor::init( %this ) {
	%this.attachTerrain();
	%this.setBrushSize( 9, 9 );
	
}
//------------------------------------------------------------------------------
//==============================================================================
function ETerrainEditor::saveTerrainToFile( %this,%obj,%file ) {
	if (!isObject(%obj))
		return;

	if (%file $= "")
		%file = addFilenameToPath(filePath(MissionGroup.getFileName()),%obj.getName(),"ter");

	%obj.setFieldValue("terrainFile",%file);
	%obj.save(%file);
	devLog("Terrain:",%obj.getName(),"Saved to",%file);
}
//------------------------------------------------------------------------------
//==============================================================================
//ETerrainEditor.setSelectType();
function ETerrainEditor::setSelectType( %this ) {
	%this.setBrushType("Selection");
}
//------------------------------------------------------------------------------
//==============================================================================
function EPainter_TerrainMaterialUpdateCallback( %mat, %matIndex ) {
	// Skip over a bad selection.
	if ( %matIndex == -1 || !isObject( %mat ) )
		return;

	// Update the material and the UI.
	ETerrainEditor.updateMaterial( %matIndex, %mat.getInternalName() );
	//EPainter.setup( %matIndex );
	EPainter.updateLayers();
}
//------------------------------------------------------------------------------
//==============================================================================
function EPainter_TerrainMaterialAddCallback( %mat, %matIndex ) {
	// Ignore bad materials.
	if ( !isObject( %mat ) )
		return;

	// Add it and update the UI.
	ETerrainEditor.addMaterial( %mat.getInternalName() );
	//EPainter.setup( %matIndex );
	EPainter.updateLayers();
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainEditor::setPaintMaterial( %this, %matIndex, %terrainMat ) {
	assert( isObject( %terrainMat ), "TerrainEditor::setPaintMaterial - Got bad material!" );
	ETerrainEditor.paintIndex = %matIndex;
	ETerrainMaterialSelected.selectedMatIndex = %matIndex;
	ETerrainMaterialSelected.selectedMat = %terrainMat;
	ETerrainMaterialSelected.bitmap = %terrainMat.diffuseMap;
	ETerrainMaterialSelected_N.bitmap = %terrainMat.normalMap;
	ETerrainMaterialSelected_M.bitmap = %terrainMat.macroMap;
	ETerrainMaterialSelectedEdit.Visible = isObject(%terrainMat);
	TerrainTextureText.text = %terrainMat.getInternalName();
	ProceduralTerrainPainterDescription.text = "Generate "@ %terrainMat.getInternalName() @" layer";
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainEditor::setup( %this ) {
	%action = %this.savedAction;
	%desc = %this.savedActionDesc;

	if ( %this.savedAction $= "" ) {
		%action = brushAdjustHeight;
	}

	%this.switchAction( %action );
}
//------------------------------------------------------------------------------
//==============================================================================
function onNeedRelight() {
	if( RelightMessage.visible == false )
		RelightMessage.visible = true;
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainEditor::onGuiUpdate(%this, %text) {
	%minHeight = getWord(%text, 1);
	%avgHeight = getWord(%text, 2);
	%maxHeight = getWord(%text, 3);
	%this.lastAverageHeight = %avgHeight;
	%mouseBrushInfo = " (Mouse) #: " @ getWord(%text, 0) @ "  avg: " @ %avgHeight @ " " @ ETerrainEditor.currentAction;
	%selectionInfo = "     (Selected) #: " @ getWord(%text, 4) @ "  avg: " @ getWord(%text, 5);
	TEMouseBrushInfo.setValue(%mouseBrushInfo);
	TESelectionInfo.setValue(%selectionInfo);
	EditorGuiStatusBar.setSelection("min: " @ %minHeight @ "  avg: " @ %avgHeight @ "  max: " @ %maxHeight);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainEditor::onActiveTerrainChange(%this, %newTerrain) {
	// Need to refresh the terrain painter.
	if ( Lab.currentEditor.getId() == TerrainPainterPlugin.getId() )
		EPainter.setup(ETerrainEditor.paintIndex);
}
//------------------------------------------------------------------------------

//==============================================================================
// Functions
//==============================================================================

//==============================================================================
function TerrainEditorSettingsGui::onWake(%this) {
	TESoftSelectFilter.setValue(ETerrainEditor.softSelectFilter);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainEditorSettingsGui::onSleep(%this) {
	ETerrainEditor.softSelectFilter = TESoftSelectFilter.getValue();
}
//------------------------------------------------------------------------------
//==============================================================================

function getPrefSetting(%pref, %default) {
	//
	if(%pref $= "")
		return(%default);
	else
		return(%pref);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainEditorPlugin::setEditorFunction(%this) {
	%terrainExists = parseMissionGroup( "TerrainBlock" );

	if( %terrainExists == false )
		LabMsgYesNoCancel("No Terrain","Would you like to create a New Terrain?", "Canvas.pushDialog(CreateNewTerrainGui);");

	return %terrainExists;
}
//------------------------------------------------------------------------------

