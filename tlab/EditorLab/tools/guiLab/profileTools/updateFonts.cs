//==============================================================================
// Lab GuiManager -> Init Lab GUI Manager System
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Profile Fonts Update from font source
//==============================================================================

//==============================================================================
// Update all the Color Sets assigned profiles
function GLab::updateProfilesFonts(%this ) {
	foreach$(%profile in $ProfileList["fontSource"]) {
		%fontSrc = %profile.getFieldValue("fontSource");
		%fontType = $GuiFont[%fontSrc];
		GLab.updateProfileField(%profile,"fontType",%fontType);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Update a single profile field and set profile dirty if changed
function GLab::updateFontSize(%this,%ratio,%all ) {
	if (!$GLab_FontResizeEnabled) {
		//warnLog("Font Resizing mode is disabled, exiting updateFontSize function.");
		return;
	}

	if (%ratio $="") {
		%ratio = %this.getGuiRatio();
	}

	%list = $ProfStoreFieldProfiles["fontSize"];

	if (!%all)
		%list = $ResizableProfiles;

	foreach$(%profileName in %list ) {
		devLog("RESIZING PROFILE:",%profileName);
		%value = $ProfStoreFieldDefault[%profileName,"fontSize"];
		%newValue = %value * %ratio;
		%newValue = mCeil(%newValue);
		%this.updateProfileField(%profileName,"fontSize",%newValue,true);

		foreach$(%child in $ProfChilds[%profileName]) {
			%childValue = $ProfStoreFieldDefault[%profileName,"fontSize"];

			if (%childValue $= "")
				continue;

			%childValue = $ProfStoreFieldDefault[%child,"fontSize"];
			%newChildValue = %childValue * %ratio;
			%newChildValue = mCeil(%newChildValue);
			devLog(%child,"Updating a child with own value",%childValue,"Changed to:",%newChildValue);
			%this.updateProfileField(%child,"fontSize",%newChildValue,true);
		}
	}
}

//==============================================================================
// Update a single profile field and set profile dirty if changed
function GLab::restoreFontSize(%this,%ratio,%all ) {
	%list = $ProfStoreFieldProfiles["fontSize"];

	if (!%all)
		%list = $ResizableProfiles;

	foreach$(%profileName in %list ) {
		devLog("RESIZING PROFILE:",%profileName);
		%value = $ProfStoreFieldDefault[%profileName,"fontSize"];
		//%newValue = %value * %ratio;
		//%newValue = mCeil(%newValue);
		%this.updateProfileField(%profileName,"fontSize",%value,true);
	}
}