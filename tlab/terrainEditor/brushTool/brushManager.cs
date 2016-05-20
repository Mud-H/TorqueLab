//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TEP_BrushManager_SetHeight_Range_Min = 0;
$TEP_BrushManager_SetHeight_Range_Max = 200;
$TEP_BrushManager_BrushSize_Range_Min = 0;
$TEP_BrushManager_BrushSize_Range_Max = 200;
$TEP_BrushManager_Softness_Range_Min = 0;
$TEP_BrushManager_Softness_Range_Max = 200;
$TEP_BrushManager_Pressure_Range_Min = 0;
$TEP_BrushManager_Pressure_Range_Max = 200;



$TEP_BrushManager_SetHeight = 100;

$TEP_BrushManager_OptionFields = "heightRangeMin heightRangeMax";
$TEP_BrushManager_Types = "slopeMin slopeMax size pressure softness setheight";
function TEP_BrushManager::onWake( %this ) {
}

function TEP_BrushManager::init( %this ) {
	%toolbars = "EWTerrainEditToolbar TerrainPainterToolbar";

	foreach$(%type in $TEP_BrushManager_Types) {
		%this.brushTypeCtrls[%type] = "";
		%this.brushTypeSliders[%type] = "";
		%this.brushTypeCtrls[%type,"Editor"] = "";
		%this.brushTypeSliders[%type,"Editor"] = "";
		%this.brushTypeCtrls[%type,"Painter"] = "";
		%this.brushTypeSliders[%type,"Painter"] = "";
	}

	foreach$(%type in $TEP_BrushManager_Types) {
		%edit = TerrainPainterToolbar.findObjectByInternalName(%type,true);
		%slider = TerrainPainterToolbar.findObjectByInternalName(%type@"_slider",true);

		if (isObject(%edit))
			%this.brushTypeCtrls[%type,"Painter"] = strAddWord(%this.brushTypeSliders[%type,"Painter"],%edit.getId());

		if (isObject(%slider))
			%this.brushTypeSliders[%type,"Painter"] = strAddWord(%this.brushTypeSliders[%type,"Painter"],%slider.getId());

		%edit = EWTerrainEditToolbar.findObjectByInternalName(%type,true);
		%slider = EWTerrainEditToolbar.findObjectByInternalName(%type@"_slider",true);

		if (isObject(%edit))
			%this.brushTypeCtrls[%type,"Editor"] = strAddWord(%this.brushTypeSliders[%type,"Editor"],%edit.getId());

		if (isObject(%slider))
			%this.brushTypeSliders[%type,"Editor"] = strAddWord(%this.brushTypeSliders[%type,"Editor"],%slider.getId());
	}

	/*foreach$(%toolbar in %toolbars) {
		foreach$(%type in $TEP_BrushManager_Types){
			%edit = %toolbar.findObjectByInternalName(%type,true);
			%slider = %toolbar.findObjectByInternalName(%type@"_slider",true);
			if (isObject(%edit))
				%this.brushTypeCtrls[%type] = strAddWord(%this.brushTypeSliders[%type],%edit.getId());
			if (isObject(%slider))
				%this.brushTypeSliders[%type] = strAddWord(%this.brushTypeSliders[%type],%slider.getId());
		}
	}*/
	foreach$(%type in $TEP_BrushManager_Types) {
		%this.brushTypeCtrls[%type] = %this.brushTypeCtrls[%type,"Editor"] SPC %this.brushTypeCtrls[%type,"Painter"];
		%this.brushTypeSliders[%type] = %this.brushTypeSliders[%type,"Editor"] SPC %this.brushTypeSliders[%type,"Painter"];
	}

	%this.setDefaultBrush();
}

//==============================================================================
// Set the size of the brush (in game unit)
function TEP_BrushManager::setDefaultBrush( %this ) {
	foreach$(%type in $TEP_BrushManager_Types) {
		%list = TEP_BrushManager.brushTypeCtrls[%type,"Editor"];

		foreach$(%ctrl in %list) {
			%cfg = "DefaultBrush"@%type;
			%default = TerrainEditorPlugin.getCfg(%cfg);
			%ctrl.setValue(%default);
			%ctrl.updateFriends();
		}
	}

	foreach$(%type in $TEP_BrushManager_Types) {
		%list = TEP_BrushManager.brushTypeCtrls[%type,"Painter"];

		foreach$(%ctrl in %list) {
			%cfg = "DefaultBrush"@%type;
			%default = TerrainEditorPlugin.getCfg(%cfg);
			%ctrl.setValue(%default);
			%ctrl.updateFriends();
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Brush Size update and validation
//==============================================================================
//==============================================================================
// Set the size of the brush (in game unit)
function TEP_BrushManager::updateBrushSize( %this,%ctrl ) {
	%validValue = %this.validateBrushSize(%ctrl.getValue());
	%maxBrushSize = getWord(ETerrainEditor.maxBrushSize, 0);

	//Check the slider range and fix in case settings have changed
	if(%ctrl.isMemberOfClass("GuiSliderCtrl")) {
		%latestRange = "1" SPC %maxBrushSize;

		if (%ctrl.range !$= %latestRange)
			%ctrl.range = %latestRange;
	}

	%ctrl.setValue(%validValue);
	%ctrl.updateFriends();
	Lab.currentEditor.setParam("BrushSize",%validValue);
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the size of the brush (in game unit)
function TEP_BrushManager::validateBrushSize( %this,%value ) {
	%minBrushSize = 1;
	%maxBrushSize = getWord(ETerrainEditor.maxBrushSize, 0);
	//Convert float to closest integer
	%brushSize = mCeil(%value);
	%brushSize = mClamp(%brushSize,%minBrushSize,%maxBrushSize);
	ETerrainEditor.setBrushSize(%brushSize);
	//Set the validated value to control and update friends if there's any
	//foreach(%slider in $GuiGroup_TEP_BrushSizeSlider) {
	//	%slider.setValue(%brushSize);
	//	%slider.updateFriends();
	//}
	return %brushSize;
}
//------------------------------------------------------------------------------
//==============================================================================
// Brush Pressure update and validation
//==============================================================================
//==============================================================================
// Set the pressure of the brush
function TEP_BrushManager::updateBrushPressure( %this,%ctrl ) {
	//Convert float to closest integer
	%brushPressure = %ctrl.getValue();
	%validValue = %this.validateBrushPressure(%brushPressure);
	Lab.currentEditor.setParam("BrushPressure",%validValue);
	%ctrl.setValue(%validValue);
	%ctrl.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the pressure of the brush
function TEP_BrushManager::validateBrushPressure( %this,%brushPressure ) {
	//Convert float to closest integer
	%convPressure = %brushPressure/100;
	%clampPressure = mClamp(%convPressure,"0.0","1.0");
	ETerrainEditor.setBrushPressure(%clampPressure);
	%editorPressure = ETerrainEditor.getBrushPressure();
	%newPressure = %editorPressure * 100;
	%formatPressure = mFloatLength(%newPressure,1);
	//Set the validated value to control and update friends if there's any
	//foreach(%slider in $GuiGroup_TEP_PressureSlider) {
	//	%slider.setValue(%formatPressure);
	//	%slider.updateFriends();
	//}
	return %formatPressure;
}
//------------------------------------------------------------------------------
//==============================================================================
// Brush Softness update and validation
//==============================================================================

//==============================================================================
// Set the softness of the brush - (Lower = Less effects)
function TEP_BrushManager::updateBrushSoftness( %this,%ctrl ) {
	//Convert float to closest integer
	%brushSoftness = %ctrl.getValue();
	%validValue = %this.validateBrushSoftness(%brushSoftness);
	Lab.currentEditor.setParam("BrushSoftness",%validValue);
	logd("BrushSoftness",%validValue);
	%ctrl.setValue(%validValue);
	%ctrl.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the softness of the brush - (Lower = Less effects)
function TEP_BrushManager::validateBrushSoftness( %this,%value ) {
	//Convert float to closest integer
	%brushSoftness = %value;
	%convSoftness = %brushSoftness/100;
	%clampSoftness = mClamp(%convSoftness,"0","1");
	ETerrainEditor.setBrushSoftness(%clampSoftness);
	%editorSoftness = ETerrainEditor.getBrushSoftness();
	%newSoftness = %editorSoftness * 100;
	%formatSoftness = mFloatLength(%newSoftness,1);
	return %formatSoftness;
}
//------------------------------------------------------------------------------

//==============================================================================
// Brush Softness update and validation
//==============================================================================
//==============================================================================
// Set the softness of the brush - (Lower = Less effects)
function TEP_BrushManager::updateSetHeightValue( %this,%ctrl ) {
	//Convert float to closest integer
	%validValue = %this.validateBrushSetHeight(%ctrl.getValue());

	if (%validValue $= "")
		return;

	%ctrl.setValue(%validValue);
	%ctrl.updateFriends();
	Lab.currentEditor.setParam("BrushSetHeight",%validValue);
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the softness of the brush - (Lower = Less effects)
function TEP_BrushManager::validateBrushSetHeight( %this,%value ) {
	//Convert float to closest integer
	if (!strIsNumeric(%value)) {
		warnLog("Invalid non-numeric value specified:",%value);
		return;
	}

	%value = mFloatLength(%value,2);
	ETerrainEditor.setHeightVal = %value;
	return %value;

	foreach(%slider in $GuiGroup_TEP_SetHeightSlider) {
		%slider.setValue(%value);
		%slider.updateFriends();
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Brush SlopeMin update and validation
//==============================================================================

//==============================================================================
// Set Slope Angle Min. - Brush have no effect on terrain with lower angle
function TEP_BrushManager::setSlopeMin( %this,%ctrl ) {
	%validValue = %this.validateBrushSlopeMin(%ctrl.getValue());
	%plugin = Lab.currentEditor;
	Lab.currentEditor.setParam("BrushSlopeMin",%validValue);
	logd("TEP_BrushManager::setSlopeMin",%validValue);
	%formatVal = mFloatLength(%validValue,1);
	%ctrl.setValue(%formatVal);
	%ctrl.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
// Set Slope Angle Min. - Brush have no effect on terrain with lower angle
function TEP_BrushManager::validateBrushSlopeMin( %this,%value ) {
	//Force the value into the TerrainEditor code and it will be returned validated
	%val = ETerrainEditor.setSlopeLimitMinAngle(%value);
	//Set precision to 1 for gui display
	return %val;	
}
//------------------------------------------------------------------------------
//==============================================================================
// Brush SlopeMax update and validation
//==============================================================================

//==============================================================================
// Set Slope Angle Min. - Brush have no effect on terrain with lower angle
function TEP_BrushManager::setSlopeMax( %this,%ctrl ) {
	%validValue = %this.validateBrushSlopeMax(%ctrl.getValue());
	Lab.currentEditor.setParam("BrushSlopeMax",%validValue);
	%formatVal = mFloatLength(%validValue,1);
	%ctrl.setValue(%formatVal);
	%ctrl.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
// Set Slope Angle Min. - Brush have no effect on terrain with lower angle
function TEP_BrushManager::validateBrushSlopeMax( %this,%value ) {
	//Force the value into the TerrainEditor code and it will be returned validated
	%val = ETerrainEditor.setSlopeLimitMaxAngle(%value);
	//Set precision to 1 for gui display
	%formatVal = mFloatLength(%val,1);
	return %val;

	//Set the validated value to control and update friends if there's any
	foreach(%slider in $GuiGroup_TPP_Slider_SlopeMax) {
		%slider.setValue(%formatVal);
		%slider.updateFriends();
	}

	return %val;
}
//------------------------------------------------------------------------------

function PaintBrushSizeSliderCtrlContainer::onWake(%this) {
	%this-->slider.range = "1" SPC getWord(ETerrainEditor.maxBrushSize, 0);
	%this-->slider.setValue(PaintBrushSizeTextEditContainer-->textEdit.getValue());
}

function PaintBrushPressureSliderCtrlContainer::onWake(%this) {
	%this-->slider.setValue(PaintBrushPressureTextEditContainer-->textEdit.getValue() / 100);
}

function PaintBrushSoftnessSliderCtrlContainer::onWake(%this) {
	%this-->slider.setValue(PaintBrushSoftnessTextEditContainer-->textEdit.getValue() / 100);
}

//------------------------------------------------------------------------------------

function TerrainBrushSizeSliderCtrlContainer::onWake(%this) {
	%this-->slider.range = "1" SPC getWord(ETerrainEditor.maxBrushSize, 0);
	%this-->slider.setValue(TerrainBrushSizeTextEditContainer-->textEdit.getValue());
}

function TerrainBrushPressureSliderCtrlContainer::onWake(%this) {
	%this-->slider.setValue(TerrainBrushPressureTextEditContainer-->textEdit.getValue() / 100.0);
}

function TerrainBrushSoftnessSliderCtrlContainer::onWake(%this) {
	%this-->slider.setValue(TerrainBrushSoftnessTextEditContainer-->textEdit.getValue() / 100.0);
}

function TerrainSetHeightSliderCtrlContainer::onWake(%this) {
	%this-->slider.setValue(TerrainSetHeightTextEditContainer-->textEdit.getValue());
}