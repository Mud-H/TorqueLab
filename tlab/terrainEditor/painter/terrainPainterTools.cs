//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//TerrainPainterTools.browseDefaultTexturesFolder();
$TPT_SubMaterialSuffix = "_1";
//==============================================================================
function TerrainPainterTools::browseDefaultTexturesFolder( %this) {
	devLog("Set default terrain textures folder to:",%folder);
	%startFrom = %this.defaultTexturesFolder;

	if (%startFrom $= "")
		%startFrom = "art/";

	getFolderName("","TerrainPainterTools.onSelectDefaultTexturesFolder",%startFrom);
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainPainterTools::onSelectDefaultTexturesFolder( %this, %folder) {
	devLog("Set default terrain textures folder to:",%folder);
	%this.defaultTexturesFolder = %folder;
	%this-->defaultTexturesFolder.setText(%folder);
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainPainterTools::setDefaultTexturesFolder( %this, %folder) {
	devLog("Set default terrain textures folder to:",%folder);
	%this.defaultTexturesFolder = %folder;
	%this-->defaultTexturesFolder.setText(%folder);
}
//------------------------------------------------------------------------------

//==============================================================================
// Create GroundCover Terrain Material CLone
//==============================================================================
