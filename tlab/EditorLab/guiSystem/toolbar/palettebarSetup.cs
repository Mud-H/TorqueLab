//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
//==============================================================================
function Lab::updatePaletteBar(%this) {

	foreach(%paletteObj in LabPaletteGuiSet) {
		Lab.loadPalette( %paletteObj );
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::loadPalette(%this,%paletteObj) {
	if (!isObject(%paletteObj))
		return;
	%paletteGroup = 0;
	%i = %paletteObj.getCount();

		for( ; %i != 0; %i--) {
			%paletteObj.getObject(0).defaultParent = %paletteObj;
			%paletteObj.getObject(0).visible = 0;
			%paletteObj.getObject(0).groupNum = %paletteGroup;
			%paletteObj.getObject(0).paletteName = %paletteObj.getName();
			LabPaletteItemSet.add(%paletteObj.getObject(0));
			LabPaletteArray.addGuiControl(%paletteObj.getObject(0));
		}
	LabPaletteArray.paletteObjLoaded[%paletteObj.getName()] = true;
}
//==============================================================================
function Lab::hidePluginPalettes(%this) {
	hide(LabPaletteBar);
}
//==============================================================================
// Toggle the palette bar tools to activate those used by plugin
function Lab::togglePluginPalette(%this, %paletteName) {
	// since the palette window ctrl auto adjusts to child ctrls being visible,
	// loop through the array and pick out the children that belong to a certain tool
	// and label them visible or not visible
	show(LabPaletteBar);

	for( %i = 0; %i < LabPaletteArray.getCount(); %i++ )
		LabPaletteArray.getObject(%i).visible = 0;

	%windowMultiplier = 0;
	%paletteNameWordCount = getWordCount( %paletteName );

	for(%pallateNum = 0; %pallateNum < %paletteNameWordCount; %pallateNum++) {
		%currentPalette = getWord(%paletteName, %pallateNum);

		for( %i = 0; %i < LabPaletteArray.getCount(); %i++ ) {
			
			if( LabPaletteArray.getObject(%i).paletteName $= %paletteName)
				%found = true;
			if( LabPaletteArray.getObject(%i).paletteName $= %currentPalette) {
			   if (LabPaletteArray.getObject(%i).disabled)
			   {
			      LabPaletteArray.getObject(%i).visible = 0;
			      continue;  
			   }
			   
				LabPaletteArray.getObject(%i).visible = 1;
				%windowMultiplier++;
			}
		}
	}

	%buttonSizeFull = LabPaletteArray.rowSize+LabPaletteArray.rowSpacing;

	if (!%found){
		devLog("NoPaletter found that fit:",%paletteName);
		hide(LabPaletteBar);
		return;
	}
	// auto adjust the palette window extent according to how many
	// children controls we found; if none found, the palette window becomes invisible
	if( %windowMultiplier == 0 || %paletteName $= "")
		LabPaletteBar.visible = 0;
	else {
		LabPaletteBar.visible = 1;
		LabPaletteBar.extent = getWord(LabPaletteBar.extent, 0) SPC (4 + 26 * %windowMultiplier);
		LabPaletteBar.extent = LabPaletteBar.extent.x SPC LabPaletteArray.extent.y + 10;
	}
}
//------------------------------------------------------------------------------

