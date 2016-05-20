//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$FEP_BrushManager_Slope_Range_Min = 0;
$FEP_BrushManager_Slope_Range_Max = 90;
$FEP_BrushManager_BrushSize_Range_Min = 0;
$FEP_BrushManager_BrushSize_Range_Max = 200;
$FEP_BrushManager_Softness_Range_Min = 0;
$FEP_BrushManager_Softness_Range_Max = 200;
$FEP_BrushManager_Pressure_Range_Min = 0;
$FEP_BrushManager_Pressure_Range_Max = 200;




$FEP_BrushManager_OptionFields = "heightRangeMin heightRangeMax";
function FEP_BrushManager::onWake( %this ) {
}
//------------------------------------------------------------------------------

//==============================================================================
// Brush Updates Setting Functions
//==============================================================================

//==============================================================================
// Set the pressure of the brush
function FEP_BrushManager::setBrushPressure( %this,%ctrl ) {
	if (ForestEditorPlugin.brushPressure $= "")
		ForestEditorPlugin.brushPressure = ForestEditorPlugin.getCfg("defaultBrushPressure");

	if (isObject(%ctrl))
		%brushPressure = %ctrl.getValue();
	else
		%brushPressure = ForestEditorPlugin.brushPressure;

	//Convert float to closest integer
	%convPressure = %brushPressure/100;
	%clampPressure = mClamp(%convPressure,"0.0","1.0");
	ForestToolBrush.pressure = %clampPressure;
	%newPressure = %clampPressure * 100;
	%formatPressure = mFloatLength(%newPressure,1);
	%this.syncBrushCtrls("brushPressure",%formatPressure);
	ForestEditorPlugin.brushPressure = %formatPressure;
	ForestEditorPlugin.setParam("BrushPressure",%formatPressure);
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the pressure of the brush
function FEP_BrushManager::setBrushHardness( %this,%ctrl ) {
	if (ForestEditorPlugin.brushHardness $= "")
		ForestEditorPlugin.brushHardness = ForestEditorPlugin.getCfg("defaultBrushHardness");

	if (isObject(%ctrl))
		%brushHardness = %ctrl.getValue();
	else
		%brushHardness = ForestEditorPlugin.brushHardness;

	//Convert float to closest integer
	// %brushHardness = %ctrl.getValue();
	%convHardness = %brushHardness/100;
	%clampHardness = mClamp(%convHardness,"0.0","1.0");
	ForestToolBrush.Hardness = %clampHardness;
	%newHardness = %clampHardness * 100;
	%formatHardness = mFloatLength(%newHardness,1);
	%this.syncBrushCtrls("brushHardness",%formatHardness);
	ForestEditorPlugin.brushHardness = %formatHardness;
	ForestEditorPlugin.setParam("BrushHardness",%formatHardness);
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_BrushManager::setGlobalScale( %this,%ctrl ) {
	if (ForestEditorPlugin.globalScale $= "")
		ForestEditorPlugin.globalScale = ForestEditorPlugin.getCfg("defaultGlobalScale");

	if (isObject(%ctrl))
		%value = %ctrl.getValue();
	else
		%value = ForestEditorPlugin.globalScale;

	%minScaleSize = "0.1";
	%maxScaleSize = "10";
	%value = mClamp(%value,%minScaleSize,%maxScaleSize);
	%value = mFloatLength(%value,2);
	
	$Forest::GlobalScale = %value;
	%this.syncBrushCtrls("globalScale",%value);
	ForestEditorPlugin.globalScale = %value;
	ForestEditorPlugin.setParam("GlobalScale",%value);
	
	if (isObject(%ctrl))
		%ctrl.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_BrushManager::setBrushSize( %this,%ctrl ) {
	if (ForestEditorPlugin.brushSize $= "")
		ForestEditorPlugin.brushSize = ForestEditorPlugin.getCfg("defaultBrushSize");

	if (isObject(%ctrl))
		%value = %ctrl.getValue();
	else
		%value = ForestEditorPlugin.brushSize;

	%this.syncBrushCtrls("brushSize",%value);
	ForestToolBrush.size = %value;
	ForestEditorPlugin.brushSize = %value;
	ForestEditorPlugin.setParam("BrushSize",%value);
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_BrushManager::validateBrushSize( %this,%size ) {
	%minBrushSize = 1;
	%maxBrushSize = getWord(ETerrainEditor.maxBrushSize, 0);
	%val = mRound(%size);

	if(%val < %minBrushSize)
		%val = %minBrushSize;
	else if(%val > %maxBrushSize)
		%val = %maxBrushSize;

	return %val;
}
//------------------------------------------------------------------------------



//==============================================================================
// Brush Options Setting Functions
//==============================================================================
//==============================================================================
function TPP_BrushManagerSizeRangeEdit::onValidate( %this ) {
	%infoWords = strreplace(%this.internalName,"_"," ");
	%group = getWord(%infoWords,0);
	%type = getWord(%infoWords,1);
	%field = getWord(%infoWords,2);
	%value = %this.getText();
	%rangeFull = $FEP_BrushManager_[%group,"Range","Min"] SPC $FEP_BrushManager_[%group,"Range","Max"];
	%min = ETerrainEditor.getSlopeLimitMinAngle();
	/*
		foreach(%ctrl in $GuiGroup_FEP_Slider_SlopeMin) {

			%ctrl.range = %rangeFull;
			%ctrl.setValue(%min);
		}
	*/
	%max = ETerrainEditor.getSlopeLimitMaxAngle();
	/*
		foreach(%ctrl in $GuiGroup_FEP_Slider_SlopeMax) {

			%ctrl.range = %rangeFull;
			%ctrl.setValue(%max);
		}
		*/
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_BrushManagerOptionEdit::onValidate( %this ) {
	%infoWords = strreplace(%this.internalName,"_"," ");
	%group = getWord(%infoWords,0);
	%type = getWord(%infoWords,1);
	%field = getWord(%infoWords,2);
	%value = %this.getText();
	FEP_BrushManager.doBrushOption(%group,%type,%field,%value);
}
//------------------------------------------------------------------------------

//==============================================================================
function FEP_BrushManager::doBrushOption( %this,%group,%type,%field,%value ) {
	devlog("FEP_BrushManager::doBrushOption( %this,%group,%type,%field,%value )", %this,%group,%type,%field,%value );
	%fullField = %group@"_"@%type@"_"@%field;
	FEP_BrushManager.setFieldValue(%fullField,%value);
	$FEP_BrushManager_[%fullField] = %value;

	if (%group $= "BrushSize" && %type $= "Range" && %field $= "Max")
		ETerrainEditor.maxBrushSize = %value;

	switch$(%type) {
	case "range":
		%guiGroup = "FEP_"@%group@"Slider";
		%rangeFull = $FEP_BrushManager_[%group,"Range","Min"] SPC $FEP_BrushManager_[%group,"Range","Max"];

		foreach(%ctrl in $GuiGroup_FEP_Slider_[%group]) {
			devLog("HeightSlider:",%ctrl);
			%ctrl.range = %rangeFull;
			%ctrl.setValue(%value);
		}
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function FEP_BrushManager::syncBrushCtrls( %this,%data,%value ) {
	//PossibleData = globalScale BrushHardness brushPressure BrushSize
	foreach$(%ctrl in "FEP_BrushManager ForestEditorToolbar") {
		%dataCtrl = %ctrl.findObjectByInternalName(%data,true);

		if (!isObject(%dataCtrl))
			continue;

		%dataCtrl.setValue(%value);
		%dataCtrl.updateFriends();
	}
}
//------------------------------------------------------------------------------