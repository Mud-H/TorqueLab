//==============================================================================
// Lab GuiManager -> Init Lab GUI Manager System
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Update all the Color Sets assigned profiles
//GLab.updateProfilesSetType("DefaultFontA","colorFont");
function GLab::updateProfilesSetType(%this,%set,%type ) {
	foreach$(%profile in $GLab::ColorSetProfiles_[%type,%set]) {
		%this.setProfileColorFromSet(%set,%type);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Profile Fonts Update from font source
//==============================================================================

//==============================================================================
// Update all the Color Sets assigned profiles
function GLab::setProfileColorFromSet(%this,%set,%type ) {
	%profile = $GLab_SelectedObject;
	$GLab::ColorSetProfiles_[%type,%set] = strAddWord($GLab::ColorSetProfiles_[%type,%set],%profile.getName(), true);
	GLab.updateProfileColorsSet(%profile,%set,%type);

	switch$(%type) {
	case "colorFont":
		GLab.updateSingleProfileColors(%profile,%set,%type);

	case "colorFill":
		GLab.updateSingleProfileColors(%profile,%set);
	}
}
//------------------------------------------------------------------------------




//==============================================================================
// Update all the Color Sets assigned profiles
function GLab::updateProfileColorsSet(%this,%profile,%set,%type ) {
	if (!isObject(GuiColor_Group))
		return;

	switch$(%type) {
	case "colorFont":
		info("Updating profile font colors",%profile.getName(),"Set",%set);
		%srcGroup = GuiColor_Group.findObjectByInternalName(%type,true);
		%srcColor = %srcGroup.findObjectByInternalName(%set,true);
		%count = %srcColor.getDynamicFieldCount();

		for(%i = 0; %i < %count; %i++) {
			%id = ""; //Reset the ID so it's blank for fieal with no id
			%fieldFull = %srcColor.getDynamicField(%i);
			%field = getField(%fieldFull,0);
			%value = getField(%fieldFull,1);
			//If last char is numeric, we have to set it as field <=> id
			%strlen = strLen(%field);
			%lastChar = getSubStr(%field,%strlen-1);

			if (strIsNumeric(%lastChar)) {
				%field = getSubStr(%field,0,%strlen-1);
				%id = %lastChar;
				devLog("FontColors ID converted to field:",%field,"ID:",%id);
			}

			%current = %profile.getFieldValue(%field,%id);
			%isDirty = false;

			if(%current !$= %value) {
				%isDirty = true;
				warnLog("-> Should be set to dirty",%field,%value,%current);
			}

			devLog(%profile.getName(),"Updating fontColor field:",%field," from:",%current,"To:",%value);
			GLab.updateProfileField(%profile,%field,%value,!%isDirty);
		}

	case "colorFill":
		%srcColor = GuiColor_Group.findObjectByInternalName(%type,true);
		%value = %srcColor.getFieldValue(%type);
		devLog(%profile.getName(),"Updating fillColor from:",%profile.fillColor,"To:",%value);
		//GLab.updateProfileField(%profile,"fillColor",%value);
	}
}
//------------------------------------------------------------------------------