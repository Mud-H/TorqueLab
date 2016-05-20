//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Slope Angle Minimum/Maximum settings
//==============================================================================

//==============================================================================
// Set Slope Angle Min. - Brush have no effect on terrain with lower angle
function ETerrainEditor::setSlopeMin( %this,%ctrl ) {
	//Force the value into the TerrainEditor code and it will be returned validated
	%val = ETerrainEditor.setSlopeLimitMinAngle(%ctrl.getValue());
	//Set precision to 1 for gui display
	%formatVal = mFloatLength(%val,1);
	%ctrl.setValue(%formatVal);
	%ctrl.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
// Set Slope Angle Min. - Brush have no effect on terrain with higher angle
function ETerrainEditor::setSlopeMax( %this,%ctrl ) {
	//Force the value into the TerrainEditor code and it will be returned validated
	%val = ETerrainEditor.setSlopeLimitMaxAngle(%ctrl.getValue());
	//Set precision to 1 for gui display
	%formatVal = mFloatLength(%val,1);
	%ctrl.setValue(%formatVal);
	%ctrl.updateFriends();
}
//------------------------------------------------------------------------------

//==============================================================================
// Brush Tool Settings
//==============================================================================
//==============================================================================
// A brush settings have been changed
function ETerrainEditor::onBrushChanged( %this ) {
	Lab.currentEditor.syncBrushInfo();
}
//------------------------------------------------------------------------------

//==============================================================================
// Set the size of the brush (in game unit)
function ETerrainEditor::updateBrushSize( %this,%ctrl ) {
	%minBrushSize = 1;
	%maxBrushSize = getWord(ETerrainEditor.maxBrushSize, 0);
	//Convert float to closest integer
	%brushSize = mCeil(%ctrl.getValue());
	%brushSize = mClamp(%brushSize,%minBrushSize,%maxBrushSize);
	ETerrainEditor.setBrushSize(%brushSize);

	//Check the slider range and fix in case settings have changed
	if(%ctrl.isMemberOfClass("GuiSliderCtrl")) {
		%latestRange = %minBrushSize SPC %maxBrushSize;

		if (%ctrl.range !$= %latestRange)
			%ctrl.range = %latestRange;
	}

	//Set the validated value to control and update friends if there's any
	%ctrl.setValue(%brushSize);
	%ctrl.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the pressure of the brush
function ETerrainEditor::updateBrushPressure( %this,%ctrl ) {
	//Convert float to closest integer
	%brushPressure = %ctrl.getValue();
	%convPressure = %brushPressure/100;
	%clampPressure = mClamp(%convPressure,"0.0","1.0");
	ETerrainEditor.setBrushPressure(%clampPressure);
	%editorPressure = ETerrainEditor.getBrushPressure();
	%newPressure = %editorPressure * 100;
	%formatPressure = mFloatLength(%newPressure,1);
	//Set the validated value to control and update friends if there's any
	%ctrl.setValue(%formatPressure);
	%ctrl.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the softness of the brush - (Lower = Less effects)
function ETerrainEditor::updateBrushSoftness( %this,%ctrl ) {
	//Convert float to closest integer
	%brushSoftness = %ctrl.getValue();
	%convSoftness = %brushSoftness/100;
	%clampSoftness = mClamp(%convSoftness,"0","1");
	ETerrainEditor.setBrushSoftness(%clampSoftness);
	%editorSoftness = ETerrainEditor.getBrushSoftness();
	%newSoftness = %editorSoftness * 100;
	%formatSoftness = mFloatLength(%newSoftness,1);
	//Set the validated value to control and update friends if there's any
	%ctrl.setValue(%formatSoftness);
	%ctrl.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
// Toggle between brush shapes (Circle or Square)
function ETerrainEditor::toggleBrushType( %this, %brush ) {
	%this.setBrushType( %brush.internalName );
}
//------------------------------------------------------------------------------

//==============================================================================
// Terrain Editor Set Height Value
//==============================================================================
//==============================================================================
// Set the softness of the brush - (Lower = Less effects)
function ETerrainEditor::updateSetHeightValue( %this,%ctrl ) {
	//Convert float to closest integer
	%value = %ctrl.getValue();

	if (!strIsNumeric(%value)) {
		warnLog("Invalid non-numeric value specified:",%value);
		return;
	}

	ETerrainEditor.setHeightVal = %value;
	%ctrl.updateFriends();
}
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// TerrainPainterPlugin
//------------------------------------------------------------------------------

function TestMouse::onRightMouseDown(%this, %a,%b,%c) {
	devLog("TestMouse::onRightMouseDown(%this, %a,%b,%c)",%this, %a,%b,%c);
}
function TerrainEditor::offsetBrush(%this, %x, %y) {
	%curPos = %this.getBrushPos();
	%this.setBrushPos(getWord(%curPos, 0) + %x, getWord(%curPos, 1) + %y);
}

function TerrainPainterPlugin::validateBrushSize( %this ) {
	%minBrushSize = 1;
	%maxBrushSize = getWord(ETerrainEditor.maxBrushSize, 0);
	%val = $ThisControl.getText();

	if(%val < %minBrushSize)
		$ThisControl.setValue(%minBrushSize);
	else if(%val > %maxBrushSize)
		$ThisControl.setValue(%maxBrushSize);
}

function TerrainPainterPlugin::validateSlopeMaxAngle( %this ) {
	devLog("Called??");
	%maxval = ETerrainEditor.getSlopeLimitMaxAngle();
	PaintBrushSlopeControl-->SlopeMaxAngle.setText(%maxval);
}

function TerrainPainterPlugin::validateSlopeMinAngle( %this ) {
	%minval = ETerrainEditor.getSlopeLimitMinAngle();
	PaintBrushSlopeControl-->SlopeMinAngle.setText(%minval);
}