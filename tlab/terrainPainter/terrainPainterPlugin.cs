//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function TerrainPainterPlugin::initParamsArray( %this,%cfgArray ) {
	%gid = 1;
	%cfgArray.group[%gid] = "Action values";
	//%cfgArray.setVal("maxBrushSize",       "40 40" TAB "maxBrushSize" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
	%cfgArray.setVal("DefaultBrushSize",       "2" TAB "brushSize" TAB "TextEdit" TAB "" TAB "TEP_BrushManager.validateBrushSize(*val*);" TAB %gid);
	%cfgArray.setVal("DefaultBrushType",       "box" TAB "brushType" TAB "TextEdit" TAB "" TAB "ETerrainEditor.setBrushType(*val*);" TAB %gid);
	%cfgArray.setVal("DefaultBrushPressure",       "50" TAB "brushPressure" TAB "TextEdit" TAB "" TAB "TEP_BrushManager.validateBrushPressure(*val*);" TAB %gid);
	%cfgArray.setVal("DefaultBrushSoftness",       "50" TAB "brushSoftness" TAB "TextEdit" TAB "" TAB "TEP_BrushManager.validateBrushSoftness(*val*);" TAB %gid);
	%cfgArray.setVal("DefaultBrushSlopeMin",       "0" TAB "BrushSlopeMin" TAB "TextEdit" TAB "" TAB "TEP_BrushManager.validateBrushSlopeMin(*val*);" TAB %gid);
	%cfgArray.setVal("DefaultBrushSlopeMax",       "90" TAB "BrushSlopeMax" TAB "TextEdit" TAB "" TAB "TEP_BrushManager.validateBrushSlopeMax(*val*);" TAB %gid);
	%gid = 2;
	%cfgArray.group[%gid] = "Action values";
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
}

//------------------------------------------------------------------------------
// TerrainPainterPlugin
//------------------------------------------------------------------------------

function TerrainPainterPlugin::onPluginLoaded( %this ) {	
   trace();
	%map = new ActionMap();
	%map.bindCmd( keyboard, "v", "EWTerrainPainterToolbarBrushType->ellipse.performClick();", "" );// Circle Brush
	%map.bindCmd( keyboard, "b", "EWTerrainPainterToolbarBrushType->box.performClick();", "" );// Box Brush
	%map.bindCmd( keyboard, "=", "TerrainPainterPlugin.keyboardModifyBrushSize(1);", "" );// +1 Brush Size
	%map.bindCmd( keyboard, "+", "TerrainPainterPlugin.keyboardModifyBrushSize(1);", "" );// +1 Brush Size
	%map.bindCmd( keyboard, "-", "TerrainPainterPlugin.keyboardModifyBrushSize(-1);", "" );// -1 Brush Size
	%map.bindCmd( keyboard, "[", "TerrainPainterPlugin.keyboardModifyBrushSize(-5);", "" );// -5 Brush Size
	%map.bindCmd( keyboard, "]", "TerrainPainterPlugin.keyboardModifyBrushSize(5);", "" );// +5 Brush Size
	/*%map.bindCmd( keyboard, "]", "PaintBrushSlopeControl->SlopeMinAngle.text += 5", "" );// +5 SlopeMinAngle
	%map.bindCmd( keyboard, "[", "PaintBrushSlopeControl->SlopeMinAngle.text -= 5", "" );// -5 SlopeMinAngle
	%map.bindCmd( keyboard, "'", "PaintBrushSlopeControl->SlopeMaxAngle.text += 5", "" );// +5 SlopeMaxAngle
	%map.bindCmd( keyboard, ";", "PaintBrushSlopeControl->SlopeMaxAngle.text -= 5", "" );// -5 Softness*/

	for(%i=1; %i<10; %i++) {
		%map.bindCmd( keyboard, %i, "TerrainPainterPlugin.keyboardSetMaterial(" @ (%i-1) @ ");", "" );
	}

	%map.bindCmd( keyboard, 0, "TerrainPainterPlugin.keyboardSetMaterial(10);", "" );
	TerrainPainterPlugin.map = %map;
	%map = new ActionMap();
	%map.bindCmd( keyboard, "lalt", "TPG.getCurrentHeight();", "" );// +1 Brush Size
	%map.bindCmd( keyboard, "ctrl 1", "TPG.getCurrentHeight(1);", "" );// +1 Brush Size
	%map.bindCmd( keyboard, "ctrl 2", "TPG.getCurrentHeight(2);", "" );// +1 Brush Size
	%map.bindCmd( keyboard, "ctrl 3", "TPG.getCurrentHeight(3);", "" );// +1 Brush Size
	%map.bindCmd( keyboard, "ctrl 4", "TPG.getCurrentHeight(4);", "" );// +1 Brush Size
	%map.bindCmd( keyboard, "lshift 1", "TPG.setCurrentHeight(1);", "" );// +1 Brush Size
	%map.bindCmd( keyboard, "lshift 2", "TPG.setCurrentHeight(2);", "" );// +1 Brush Size
	%map.bindCmd( keyboard, "lshift 3", "TPG.setCurrentHeight(3);", "" );// +1 Brush Size
	%map.bindCmd( keyboard, "lshift 4", "TPG.setCurrentHeight(4);", "" );// +1 Brush Size
	%map.bindCmd( keyboard, "-", "TPG.getCurrentSlope();", "" );// +1 Brush Size
	TPG.map = %map;
	//TPP_BrushManager.validateBrushSlopeMin(%this.defaultBrushSlopeMin);
	TPG_Window-->groupName.setText("Default");
	TPG_Window-->groupFolders.setText("");
	EPainterStack.clear();
}

function TerrainPainterPlugin::onActivated( %this ) {   
     warnLog("Skipping activation");
  // return;
	Parent::onActivated( %this );

 
	if( !isObject( ETerrainMaterialPersistMan ) )
		new PersistenceManager( ETerrainMaterialPersistMan );

	TerrainPainterToolbar-->ellipse.performClick();// Circle Brush
	EditorGui.bringToFront( ETerrainEditor );
	ETerrainEditor.setVisible( true );
	ETerrainEditor.attachTerrain();
	ETerrainEditor.makeFirstResponder( true );
	EditorGui-->TerrainPainter.setVisible(true);
	EditorGui-->TerrainPainterPreview.setVisible(true);
	TerrainPainterToolbar.setVisible(true);
	ETerrainEditor.onBrushChanged();
	EPainter.onActivated();
	TerrainPainterPlugin.syncBrushInfo();
	EditorGuiStatusBar.setSelection("");
	
	//GuiWindowCtrl::attach(EPainter,EPainterTools);

	
}

function TerrainPainterPlugin::onDeactivated( %this ) {
	Parent::onDeactivated( %this );
	//EditorGui-->TerrainPainter.setVisible(false);
	//EditorGui-->TerrainPainterPreview.setVisible(false);
	//EWTerrainPainterToolbar.setVisible(false);
	//ETerrainEditor.setVisible( false );
}
//TerrainPainterPlugin.syncBrushInfo
function TerrainPainterPlugin::syncBrushInfo( %this ) {
	// Update gui brush info
	PaintBrushSizeTextEditContainer-->textEdit.text = getWord(ETerrainEditor.getBrushSize(), 0);
	PaintBrushSlopeControl-->SlopeMinAngle.text = ETerrainEditor.getSlopeLimitMinAngle();
	PaintBrushSlopeControl-->SlopeMaxAngle.text = ETerrainEditor.getSlopeLimitMaxAngle();
	PaintBrushPressureTextEditContainer-->textEdit.text = ETerrainEditor.getBrushPressure()*100;
	%brushType = ETerrainEditor.getBrushType();
	eval( "TerrainPainterToolbar-->" @ %brushType @ ".setStateOn(1);" );
}


function TerrainPainterPlugin::keyboardModifyBrushSize( %this, %amt) {
	%val = PaintBrushSizeTextEditContainer-->textEdit.getText();
	%val += %amt;
	PaintBrushSizeTextEditContainer-->textEdit.setValue(%val);
	PaintBrushSizeTextEditContainer-->textEdit.forceValidateText();
	ETerrainEditor.setBrushSize( PaintBrushSizeTextEditContainer-->textEdit.getText() );
}

function TerrainPainterPlugin::keyboardSetMaterial( %this, %mat) {
	%name = "EPainterMaterialButton" @ %mat;
	%ctrl = EPainter.findObjectByInternalName(%name, true);

	if(%ctrl) {
		%ctrl.performClick();
	}
}

function TerrainPainterPlugin::onToolsResized( %this ) {
	devLog("Win");
	%fullWidth = EPainter_4MatPreview.extent.x;
	%cellWidth = mFloor(%fullWidth/4);
	EPainter_4MatPreview.columns = "0" SPC %cellWidth SPC %cellWidth*2 SPC %cellWidth*3;
	EPainter_4MatPreview.updateSizes();
}

//==============================================================================
function TerrainPainterTools::onPreEditorSave( %this ) {
	EPainterStack.clear();
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPainterTools::onPostEditorSave( %this ) {
	//EPostFxManager.moveToGui(SEP_PostFXManager_Clone);
}
//------------------------------------------------------------------------------