//==============================================================================
// Lab GuiManager -> Profile Updater functions
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$ResizableProfiles = "GuiButtonText";

//==============================================================================
// Profile Colors Update from selected Color Set
//==============================================================================

//==============================================================================
// Update all the Color Sets assigned profiles
function GLab::updateProfilesColors(%this,%refreshList ) {
	if ($ProfileUpdateColorList $="" || %refreshList)
		updateAllProfileFiles();

	foreach$(%cType in $ProfileUpdateColorList) {
		//------------------------------------------------------------------------
		//Check for Fill set which use single global object
		%isFill = strstr(%cType,"Fill");

		if (%isFill !$= "-1") {
			GLab.updateProfilesFillColor(%cType);
			continue;
		}

		foreach$(%profile in $ProfileListColor[%cType]) {
			%this.updateSingleProfileColors(%profile,%cType);
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Update all the Color Sets assigned profiles
function GLab::updateSingleProfileColors(%this,%profile,%cType ) {
	info("Updating single profile colors",%profile.getName(),"Type",%cType);

	if (!isObject(GuiColor_Group))
		return;

	%srcGroup = GuiColor_Group.findObjectByInternalName(%cType,true);
	%colorSet = %profile.getFieldValue(%cType);
	%srcColor = %srcGroup.findObjectByInternalName(%colorSet,true);
	%count = %srcColor.getDynamicFieldCount();

	for(%i = 0; %i < %count; %i++) {
		%fieldFull = %srcColor.getDynamicField(%i);
		%field = getField(%fieldFull,0);
		%value = getField(%fieldFull,1);
		%current = %profile.getFieldValue(%field);
		%isDirty = false;

		if(%current !$= %value) {
			%isDirty = true;
			warnLog("-> Should be set to dirty",%field,%value,%current);
		}

		%strlen = strLen(%field);
		%lastChar = getSubStr(%field,%strlen-1);

		if (strIsNumeric(%lastChar)) {
			%beforeLastChar = getSubStr(%field,0,%strlen-1);
			%field = %beforeLastChar@"["@%lastChar@"]";
		}

		GLab.updateProfileField(%profile,%field,%value,!%isDirty);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Update ColorFill color set (use an object to hold all values)
function GLab::updateProfilesFillColor(%this,%cType ) {
	if (!isObject(GuiColor_Group))
		return;

	%srcColor = GuiColor_Group.findObjectByInternalName(%cType,true);

	foreach$(%profile in $ProfileListColor[%cType]) {
		%colorSet = %profile.getFieldValue(%cType);
		%value = %srcColor.getFieldValue(%colorSet);
		GLab.updateProfileField(%profile,"fillColor",%value);
	}
}
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//devLog("Default:",$ProfStoreFieldDefault[GuiButtonText,"fontSize"],"MenuDefault:",$ProfStoreFieldDefault[GuiButtonText_Menu,"fontSize"]);