

//==============================================================================
// Load the Specific UI Style settings
function Lab::initProfileStyle(%this,%style) {
	if (%style !$= "")
		$pref::Editor::LabStyle = %style;
}

//------------------------------------------------------------------------------
//==============================================================================
// Load the Specific UI Style settings
function Lab::setProfileStyleColors(%this) {
}

//------------------------------------------------------------------------------
//==============================================================================
// Load the Specific UI Style settings
function Lab::setDefaultProfileStyle(%this) {
	foreach$(%profile in $LabProfileList) {
		foreach$(%field in $ProfileFieldList) {
			%default = $LabProfileDefault[%profile.getName(),%field];
			%value =  %profile.getFieldValue(%field);

			if(%value !$= %default)
				%profile.setFieldValue(%field,%default);
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the Specific UI Style settings
function verifyProfiles() {
	foreach( %obj in GuiDataGroup ) {
		if( !%obj.isMemberOfClass( "GuiControlProfile" ) )
			continue;

		if (!isFile(%obj.bitmap))
			warnLog("Invalid bitmap found on profile:",%obj.getName(),"Image",%obj.bitmap);
	}
}
//------------------------------------------------------------------------------



//==============================================================================
// Load the Specific UI Style settings
function Lab::updateColorStyleProfilesList(%this,%colorType,%list,%overwrite) {
	%color = $LabStyle[%colorType];

	if (%color $= "") {
		warnLog("No color found of this type:",%colorType);
		return;
	}

	if (%overwrite)
		$LabProfiles[%colorType] = %list;
	else
		%list = trim($LabProfiles[%colorType] SPC %list);

	export("$LabProfiles*","tlab/gui/styles/default/styleProfileConfig.cs");
}
//------------------------------------------------------------------------------

function Lab::refreshColorStyle(%this,%field) {
	%fileInit = "tlab/gui/styles/"@$Pref::Editor::GuiStyle@"/initStyle.cs";

	if (isFile(%fileInit))
		exec(%fileInit);

	foreach$(%colorType in $LabStyleColorTypes) {
		Lab.updateProfilesColor(%colorType,"fillColor");
	}

	foreach$(%group in  $LabStyleGroups[$Pref::Editor::GuiStyle] ) {
		Lab.updateProfilesColorGroup(%group);
	}
}

//==============================================================================
// Load the Specific UI Style settings
//Lab.updateProfilesColor("Color1","fillColor");
function Lab::updateProfilesColor(%this,%colorType,%colorField,%list,%addDefault) {
	%color = $LabStyle[%colorType];

	if (%color $= "") {
		warnLog("No color found of this type:",%colorType);
		return;
	}

	if (%list $= "")
		%list = $LabProfilesStyle[%colorType];
	else if (%addDefault)
		%list = trim($LabProfilesStyle[%colorType] SPC %list);

	Lab.setProfilesListField(%list,%colorField,%color);
}
//==============================================================================
// Load the Specific UI Style settings
//Lab.updateProfilesColor("Color1","fillColor");
function Lab::updateProfilesColorGroup(%this,%group) {
	%groupSetting = $StyleColorGroup[%group];
	%groupProfiles = $StyleColorGroupProfiles[%group];

	foreach$(%profile in %groupProfiles) {
		%fieldCount = getFieldCount(%groupSetting);

		for(%i = 0; %i<%fieldCount; %i++) {
			%record = getField(%groupSetting,%i);
			%field = getWord(%record,0);
			%colorType = getWord(%record,1);
			%color = $LabStyle[%colorType];

			if (%field $= "" || %color $= "") continue;

			%profile.setFieldValue(%field,%color);
			%changed = true;
		}

		if (%changed)  GuiEditor.forceSaveProfile(%profile);

		%changed = false;
	}
}
//------------------------------------------------------------------------------
//Lab.setProfilesListField($profileFontList["Arial"],"fontType","Gotham Book");
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function Lab::setProfilesListField( %this,%list,%field,%value ) {
	foreach$(%profile in %list) {
		%profile.setFieldValue(%field,%value);
		GuiEditor.forceSaveProfile(%profile);
	}
}
//------------------------------------------------------------------------------