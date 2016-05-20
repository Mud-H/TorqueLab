//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Brush Options Setting Functions
//==============================================================================

//==============================================================================
function TEP_BrushManagerOptionEdit::onValidate( %this ) {
	%infoWords = strreplace(%this.internalName,"_"," ");
	%group = getWord(%infoWords,0);
	%type = getWord(%infoWords,1);
	%field = getWord(%infoWords,2);
	%value = %this.getText();
	TEP_BrushManager.doBrushOption(%group,%type,%field,%value);
}
//------------------------------------------------------------------------------

//==============================================================================
function TEP_BrushManager::doBrushOption( %this,%group,%type,%field,%value ) {
	%fullField = %group@"_"@%type@"_"@%field;
	TEP_BrushManager.setFieldValue(%fullField,%value);
	$TEP_BrushManager_[%fullField] = %value;

	if (%group $= "BrushSize" && %type $= "Range" && %field $= "Max")
		ETerrainEditor.maxBrushSize = %value;

	switch$(%type) {
	case "range":
		%rangeFull = $TEP_BrushManager_[%group,"Range","Min"] SPC $TEP_BrushManager_[%group,"Range","Max"];
		%toolbars = "EWTerrainEditToolbar TerrainPainterToolbar";

		foreach$(%toolbar in %toolbars) {
			%edit = %toolbar.findObjectByInternalName(%field,true);
			%slider = %toolbar.findObjectByInternalName(%field@"_slider",true);

			if (isObject(%slider)) {
				%slider.range = %rangeFull;
				%slider.setValue(%value);
			}
		}
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function TEP_BrushManager::brushSetHeightRange( %this,%range ) {
	$TEP_BrushManager_["SetHeight","Range","Min"] = %range.x;
	$TEP_BrushManager_["SetHeight","Range","Max"] = %range.y;
	%slider = EWTerrainEditToolbar.findObjectByInternalName("SetHeight_slider",true);

	if (!isObject(%slider))
		return;

	%slider.range = %range;
	%value = %slider.getValue();
	%slider.setValue(%value);
}
//------------------------------------------------------------------------------