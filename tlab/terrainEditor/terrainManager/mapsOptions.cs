//==============================================================================
// TorqueLab -> Procedural Terrain Painter GUI script
// Copyright NordikLab Studio, 2014
//==============================================================================
//==============================================================================
// Heightmap for re-importing
//==============================================================================
//==============================================================================
function TMG_AutoExportLayerRadio::onClick( %this) {
	%mode = %this.internalName;
	TMG.setAutoExportMode(%mode);
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::setAutoExportMode( %this,%mode) {
	%radio = TMG_AutoExportLayerRadios.findObjectByInternalName(%mode);
	%radio.setStateOn(true);
	TMG.autoExportLayerMode = %mode;
}
//------------------------------------------------------------------------------
//==============================================================================
function TMG::getLayersMapFolder( %this) {
	if (TMG.autoExportLayerMode $= "Target")
		return TMG.targetFolder;
	else if (TMG.autoExportLayerMode $= "Terrain") {
		return TMG.terrainFolder;
	}

	return TMG.sourceFolder;
}
//------------------------------------------------------------------------------

//==============================================================================
function TMG_ShowMapPreviewCheck::onClick( %this) {
	TMG.ShowMapPreview = %this.isStateOn();

	foreach(%pill in TMG_MaterialLayersStack) {
		%previewContainer = %pill-->imageButton.parentGroup.parentGroup;
		%previewContainer.visible = TMG.ShowMapPreview;
	}
}
//------------------------------------------------------------------------------