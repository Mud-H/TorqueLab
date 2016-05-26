//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
/*
%cfgArray.setVal("FIELD",       "DEFAULT" TAB "TEXT" TAB "TextEdit" TAB "" TAB "");
*/
//==============================================================================

function TerrainEditorPlugin::initParamsArray( %this,%cfgArray ) {
	%cfgArray.group[%gid++] = "Action values";
	%cfgArray.setVal("maxBrushSize",       "40 40" TAB "maxBrushSize" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("BrushSize",       "2" TAB "brushSize" TAB "TextEdit" TAB "" TAB "TEP_BrushManager.validateBrushSize(*val*);" TAB %gid);
	%cfgArray.setVal("BrushType",       "box" TAB "brushType" TAB "TextEdit" TAB "" TAB "ETerrainEditor.setBrushType(*val*);" TAB %gid);
	%cfgArray.setVal("BrushPressure",       "1" TAB "brushPressure" TAB "TextEdit" TAB "" TAB "TEP_BrushManager.validateBrushPressure(*val*);" TAB %gid);
	%cfgArray.setVal("BrushSoftness",       "1" TAB "brushSoftness" TAB "TextEdit" TAB "" TAB "TEP_BrushManager.validateBrushSoftness(*val*);" TAB %gid);
	%cfgArray.setVal("BrushSetHeight",       "1" TAB "Brush set height" TAB "TextEdit" TAB "" TAB "TEP_BrushManager.validateBrushSetHeight(*val*);" TAB %gid);
	%cfgArray.setVal("BrushSetHeightRange",       "0 100" TAB "Brush set height range" TAB "TextEdit" TAB "" TAB "TEP_BrushManager.brushSetHeightRange(*val*);" TAB %gid);
	%cfgArray.group[%gid++] = "Action values";
	%cfgArray.setVal("adjustHeightVal",       "10" TAB "adjustHeightVal" TAB "TextEdit" TAB "" TAB "ETerrainEditor"  TAB %gid);
	%cfgArray.setVal("setHeightVal",       "100" TAB "setHeightVal" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("scaleVal",       "1" TAB "scaleVal" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("smoothFactor",       "0.1" TAB "smoothFactor" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("noiseFactor",       "1.0" TAB "noiseFactor" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("softSelectRadius",       "50" TAB "softSelectRadius" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("softSelectFilter",       "1.000000 0.833333 0.666667 0.500000 0.333333 0.166667 0.000000" TAB "softSelectFilter" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("softSelectDefaultFilter",       "1.000000 0.833333 0.666667 0.500000 0.333333 0.166667 0.000000" TAB "softSelectDefaultFilter" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("slopeMinAngle",       "0" TAB "slopeMinAngle" TAB "TextEdit" TAB "" TAB "ETerrainEditor.setSlopeLimitMinAngle(*val*);" TAB %gid);
	%cfgArray.setVal("slopeMaxAngle",       "90" TAB "slopeMaxAngle" TAB "TextEdit" TAB "" TAB "ETerrainEditor.setSlopeLimitMaxAngle(*val*);" TAB %gid);
	%cfgArray.group[%gid++] = "Default values";
	%cfgArray.setVal("DefaultBrushSize",       "8" TAB "Default brush size" TAB "TextEdit" TAB "" TAB "TerrainEditorPlugin" TAB %gid);
	%cfgArray.setVal("DefaultBrushType",       "box" TAB "Default brush type" TAB "TextEdit" TAB "" TAB "TerrainEditorPlugin" TAB %gid);
	%cfgArray.setVal("DefaultBrushPressure",       "43" TAB "Default brush pressure" TAB "TextEdit" TAB "" TAB "TerrainEditorPlugin" TAB %gid);
	%cfgArray.setVal("DefaultBrushSoftness",       "43" TAB "Default brush softness" TAB "TextEdit" TAB "" TAB "TerrainEditorPlugin" TAB %gid);
	%cfgArray.setVal("DefaultBrushSetHeight",       "100" TAB "Default brush set height" TAB "TextEdit" TAB "" TAB "TerrainEditorPlugin" TAB %gid);
}

function TerrainEditorPlugin::initBinds( %this ) {
	%map = new ActionMap();
		%map.bindCmd( keyboard, "lalt", "TMG.getCurrentHeight();", "" );// +1 Brush Size	
	%map.bindCmd( keyboard, "1", "LabPaletteArray->brushAdjustHeight.performClick();", "" );    //Grab Terrain
	%map.bindCmd( keyboard, "2", "LabPaletteArray->raiseHeight.performClick();", "" );     // Raise Height
	%map.bindCmd( keyboard, "3", "LabPaletteArray->lowerHeight.performClick();", "" );     // Lower Height
	%map.bindCmd( keyboard, "4", "LabPaletteArray->smoothHeight.performClick();", "" );    // Average Height
	%map.bindCmd( keyboard, "5", "LabPaletteArray->smoothSlope.performClick();", "" );    // Smooth Slope
	%map.bindCmd( keyboard, "6", "LabPaletteArray->paintNoise.performClick();", "" );      // Noise
	%map.bindCmd( keyboard, "7", "LabPaletteArray->flattenHeight.performClick();", "" );   // Flatten
	%map.bindCmd( keyboard, "8", "LabPaletteArray->setHeight.performClick();", "" );       // Set Height
	%map.bindCmd( keyboard, "9", "LabPaletteArray->setEmpty.performClick();", "" );    // Clear Terrain
	%map.bindCmd( keyboard, "0", "LabPaletteArray->clearEmpty.performClick();", "" );  // Restore Terrain
	%map.bindCmd( keyboard, "v", "EWTerrainEditToolbarBrushType->ellipse.performClick();", "" );// Circle Brush
	%map.bindCmd( keyboard, "b", "EWTerrainEditToolbarBrushType->box.performClick();", "" );// Box Brush
	%map.bindCmd( keyboard, "=", "TerrainEditorPlugin.keyboardModifyBrushSize(1);", "" );// +1 Brush Size
	%map.bindCmd( keyboard, "+", "TerrainEditorPlugin.keyboardModifyBrushSize(1);", "" );// +1 Brush Size
	%map.bindCmd( keyboard, "-", "TerrainEditorPlugin.keyboardModifyBrushSize(-1);", "" );// -1 Brush Size
	%map.bindCmd( keyboard, "[", "TerrainEditorPlugin.keyboardModifyBrushSize(-5);", "" );// -5 Brush Size
	%map.bindCmd( keyboard, "]", "TerrainEditorPlugin.keyboardModifyBrushSize(5);", "" );// +5 Brush Size
	/*%map.bindCmd( keyboard, "]", "TerrainBrushPressureTextEditContainer->textEdit.text += 5", "" );// +5 Pressure
	%map.bindCmd( keyboard, "[", "TerrainBrushPressureTextEditContainer->textEdit.text -= 5", "" );// -5 Pressure
	%map.bindCmd( keyboard, "'", "TerrainBrushSoftnessTextEditContainer->textEdit.text += 5", "" );// +5 Softness
	%map.bindCmd( keyboard, ";", "TerrainBrushSoftnessTextEditContainer->textEdit.text -= 5", "" );// -5 Softness*/
	TerrainEditorPlugin.map = %map;
}

//------------------------------------------------------------------------------
// TerrainEditorPlugin
//------------------------------------------------------------------------------

function TerrainEditorPlugin::onPluginLoaded( %this ) {	
	ETerrainEditor.isDirty = false;
	ETerrainEditor.isMissionDirty = false;
	ETerrainEditor.init();
	%this.initBinds();
	Lab.terrainMenu = new PopupMenu() {
		superClass = "MenuBuilder";
		barTitle = "Terrain";
		item[0] = "Smooth Heightmap" TAB "" TAB "ETerrainEditor.onSmoothHeightmap();";
	};
	TerrainEditorPlugin.setParam("BrushSize",TerrainEditorPlugin.defaultBrushSize);
	TerrainEditorPlugin.setParam("BrushPressure",TerrainEditorPlugin.defaultBrushPressure);
	TerrainEditorPlugin.setParam("BrushSoftness",TerrainEditorPlugin.defaultBrushSoftness);
	TerrainEditorPlugin.setParam("BrushSetHeight",TerrainEditorPlugin.defaultBrushSetHeight);
	TEP_BrushManager.init();
}

function TerrainEditorPlugin::onActivated( %this ) {
	Parent::onActivated( %this );
	ETerrainEditor.switchAction( %action );
	%palette = LabPaletteArray.findObjectByInternalName( %action, true );
	if (isObject(%palette))
		%palette.setStateOn( true );
	EWTerrainEditToolbar-->ellipse.performClick(); // Circle Brush
	//Lab.menuBar.insert( , Lab.menuBar.dynamicItemInsertPos );
// Add our menu.
EPainter.setDisplayModes();
	Lab.insertDynamicMenu(Lab.terrainMenu);
	EditorGui.bringToFront( ETerrainEditor );
	ETerrainEditor.setVisible( true );
	ETerrainEditor.attachTerrain();
	ETerrainEditor.makeFirstResponder( true );
	EWTerrainEditToolbar.setVisible( true );
	ETerrainEditor.onBrushChanged();
	//devLog("TerrainEditorPlugin::onActivated","Exit premature, try rest manually");
	//return;
	ETerrainEditor.setup();
	TerrainEditorPlugin.syncBrushInfo();
	EditorGuiStatusBar.setSelection("");
}

function TerrainEditorPlugin::onDeactivated( %this ) {
	Parent::onDeactivated( %this );

	EWTerrainEditToolbar.setVisible( false );
	ETerrainEditor.setVisible( false );
	Lab.removeDynamicMenu(Lab.terrainMenu);
}

function TerrainEditorPlugin::syncBrushInfo( %this ) {
   return;
	// Update gui brush info
	TerrainBrushSizeTextEditContainer-->textEdit.text = getWord(ETerrainEditor.getBrushSize(), 0);
	TerrainBrushPressureTextEditContainer-->textEdit.text = ETerrainEditor.getBrushPressure()*100;
	TerrainBrushSoftnessTextEditContainer-->textEdit.text = ETerrainEditor.getBrushSoftness()*100;
	TerrainSetHeightTextEditContainer-->textEdit.text = ETerrainEditor.setHeightVal;
	%brushType = ETerrainEditor.getBrushType();
	eval( "EWTerrainEditToolbar-->" @ %brushType @ ".setStateOn(1);" );
}

function TerrainEditorPlugin::validateBrushSize( %this ) {
	%minBrushSize = 1;
	%maxBrushSize = getWord(ETerrainEditor.maxBrushSize, 0);
	%val = $ThisControl.getText();

	if(%val < %minBrushSize)
		$ThisControl.setValue(%minBrushSize);
	else if(%val > %maxBrushSize)
		$ThisControl.setValue(%maxBrushSize);
}

function TerrainEditorPlugin::keyboardModifyBrushSize( %this, %amt) {
	%val = TerrainBrushSizeTextEditContainer-->textEdit.getText();
	%val += %amt;
	TerrainBrushSizeTextEditContainer-->textEdit.setValue(%val);
	TerrainBrushSizeTextEditContainer-->textEdit.forceValidateText();
	ETerrainEditor.setBrushSize( TerrainBrushSizeTextEditContainer-->textEdit.getText() );
}

function TerrainEditorPlugin::onPluginCreated( %this) {
	
}

//------------------------------------------------------------------------------
// TerrainTextureEditorTool
//------------------------------------------------------------------------------
//REMOVEME Seem to be unused
/*
function TerrainTextureEditorTool::onActivated( %this ) {
	EditorGui.bringToFront( ETerrainEditor );
	ETerrainEditor.setVisible( true );
	ETerrainEditor.attachTerrain();
	ETerrainEditor.makeFirstResponder( true );
	EditorGui-->TextureEditor.setVisible(true);
	EditorGuiStatusBar.setSelection("");
}

function TerrainTextureEditorTool::onDeactivated( %this ) {
	EditorGui-->TextureEditor.setVisible(false);
	ETerrainEditor.setVisible( false );
}
*/
