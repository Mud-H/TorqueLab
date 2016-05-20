//==============================================================================
// Lab GuiManager -> Profile File Analyser
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//$ProfileFieldSet["colorFontIds"] = "fontColors[0] fontColors[1] fontColors[2] fontColors[3] fontColors[4] fontColors[5] fontColors[6] fontColors[7] fontColors[8] fontColors[9]";
//$ProfileFieldSet["colorFont"] = "fontColor fontColorHL fontColorNA fontColorSEL fontColorLink fontColorLinkHL";

//==============================================================================
// Remove all reference for a color type (used to force the colorSet)
function GLab::ClearProfileColorType(%this,%type,%profile) {
	if (%profile $= "")
		%profile = $GLab_SelectedProfile;

	%fieldList =  $ProfileFieldSet[%type] ;

	if (%fieldList $= "" || !isObject(%profile))
		return false;

	info("Removing fields:",%fieldList,"From Profile:",%profile);
	removeProfileField(%profile,%fieldList);
}
//------------------------------------------------------------------------------
