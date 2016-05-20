//==============================================================================
// GameLab -> Interface Development Gui
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function GLab::updateFontTypeList( %this ) {
	$GLab_FullFontList = "";
	doGuiGroupAction("ggFontTypesGLab","clear()");
	%fontCount = getFieldCount($GLab::FontList);

	for(%i=0; %i<%fontCount; %i++) {
		%font = getField($GLab::FontList,%i);
		%profileList =  $ProfileFontList[%font];

		if ($GLab_ShowOnlyFontInUse && %profileList $= "")
			continue;

		%addFull = %font;

		if ($GLab_FullFontList !$= "")
			%addFull = "\t"@%font;

		$GLab_FullFontList = $GLab_FullFontList@%addFull;
		doGuiGroupAction("ggFontTypesGLab","add(\""@%font@"\","@%id++@")");

		foreach$(%variation in $GLab::FontVariations[%font]) {
			doGuiGroupAction("ggFontTypesGLab","add(\""@%font SPC %variation@"\","@%id++@")");
			$GLab_FullFontList = $GLab_FullFontList@"\t"@%font SPC %variation;
			$GLab_FontVariations[%font SPC %variation] = %font;
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function GLab_FontListMenu::onSelectFont( %this ) {
	%fontFull = %this.getText();
	%fontSource = $GLab_FontVariations[%this.getText()];

	if (%fontSource $= "")
		%fontSource = %this.getText();

	$GLab_SelectedFont = %fontFull;
	GLab_SelectedFontVariations.setText($GLab::FontVariations[%fontSource]);
	%profileList =  $ProfileFontList[%fontFull];
	devLog("Profiles using this font:",%profileList);
	GLab_FontProfilesList.clearItems();

	foreach$(%profile in %profileList) {
		GLab_FontProfilesList.insertItem(%profile.getName(),%profile.getId());
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function GLab::addFontToList( %this ) {
	%name = $GLab_AddFontName;

	if (%name $= "" || %name $= "[type font name]")
		return;

	$GLab_AddFontName = "[type font name]";
	$GLab::FontList = $GLab::FontList	@ "\t"@%name;
	%this.updateFontTypeList();
}
//------------------------------------------------------------------------------
//==============================================================================
function GLab::removeFontFromList( %this ) {
	%name = $GLab_SelectedFont;

	if (%name $= "")
		return;

	devLog("Pre list = ",$GLab::FontList);
	%fontCount = getFieldCount($GLab::FontList);

	for(%i=0; %i<%fontCount; %i++) {
		%font = getField($GLab::FontList,%i);

		if (%font $= %name)
			continue;

		%addText = %font;

		if (%started)
			%addText = "\t"@%font;

		%fontList = %fontList@%addText;
		%started = true;
	}

	$GLab::FontList = %fontList;
	devLog("New list = ",%fontList);
	%this.updateFontTypeList();
}
//------------------------------------------------------------------------------

//==============================================================================
function GLab::resetFontProfilesList( %this ) {
	$FontTypesList = "";
	%fontCount = getFieldCount($GLab::FontList);

	for(%i=0; %i<%fontCount; %i++) {
		%font = getField($GLab::FontList,%i);
		$ProfileFontList[%font] = "";
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function GLab::replaceSelectedFonts( %this ) {
	%selectedIds = GLab_FontProfilesList.getSelectedItems();
	%replaceFont = GLab_FontReplaceMenu.getText();

	foreach$(%id in %selectedIds) {
		%profile = GLab_FontProfilesList.getItemText(%id);
		devLog("Profile:",%profile,"Font to replace:",%profile.fontType,"Replacement=",%replaceFont);
		%this.updateProfileField(%profile,"fontType",%replaceFont);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function GLab::updateFontVariations( %this ) {
	%font = $GLab_SelectedFont;
	%variations = GLab_SelectedFontVariations.getText();
	$GLab::FontVariations[%font] = %variations;
	%this.updateFontTypeList();
}
//------------------------------------------------------------------------------
//==============================================================================
function GLab::toggleFontBaseOnly( %this,%checkbox ) {
	if (!isObject(%checkbox))
		return;

	$GLab::FontBaseOnly[$GLab_SelectedFont] = %checkbox.isStateOn();
}
//------------------------------------------------------------------------------
