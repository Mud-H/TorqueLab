//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================
//==============================================================================
// Load the Specific UI Style settings
function Lab::saveSingleProfileStyleChanges(%this,%profile) {
	%profileStyle = %profile.getName()@"_Style";

	if (isObject(%profileStyle)) {
		//There's a style profile object for this profile, save changes to this
		foreach$(%field in $ProfileFieldList) {
			%mainDefault = $LabProfileStyleDefault[%profile.getName(),%field] = %value;

			if (%mainDefault $= "")
				%mainDefault = $LabProfileDefault[%profile.getName(),%field];

			%mainValue =  %profile.getFieldValue(%field);

			if(%mainValue !$= %mainDefault) {
				%profileStyle.setFieldValue(%field,%mainValue);
			}
		}

		//Now save it to file
		%group = %profileStyle.getGroup();
		%file = %group.myFile;

		if (isFile(%file)) {
			%result = %group.save(%file,false,"$tmpGroup = ");
		}

		if (%result)
			info("Changes have been saved to file:",%file);
	} else {
		info("Profile changes saved to original file");
		//Standard profile file so save it as usual (from GuiEditor)
		GuiEditorProfilesPM.saveDirtyObject( %profile );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the Specific UI Style settings
function Lab::saveProfileStyleChanges(%this) {
	//First compare the values of all profiles
	foreach(%profileStyle in LabStyleGroup) {
		%profile =  %profileStyle.internalName;

		if (!isObject(%profile)) {
			%profile = getWord(strreplace(%profileStyle.getName(),"_"," "),0);
		}

		if (!isObject(%profile)) {
			warnLog("A profile style have a invalid target profile. We were looking for:",%profile);
			continue;
		}

		foreach$(%field in $ProfileFieldList) {
			%mainDefault = $LabProfileStyleDefault[%profile.getName(),%field] = %value;

			if (%mainDefault $= "")
				%mainDefault = $LabProfileDefault[%profile.getName(),%field];

			%mainValue =  %profile.getFieldValue(%field);

			if(%mainValue !$= %mainDefault) {
				%profileStyle.setFieldValue(%field,%default);
			}
		}
	}

	//Now save all grouped style profiles to their file
	foreach$(%group in $LabStyleGroupList) {
		%file = %group.myFile;

		if (isFile(%file)) {
			%result = %group.save(%file,false,"$tmpGroup = ");
		}

		if (%result)
			info("Changes have been saved to file:",%file);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Load the Specific UI Style settings
function Lab::exportAllUnmapProfileStyle(%this,%fileName) {
	//First compare the values of all profiles
	%newGroup = new SimGroup();

	foreach(%profile in LabProfileGroup) {
		//Ignore profile with a style clone already
		%copy = LabStyleGroup.findObjectByInternalName(%profile.getName());

		if (isObject(%copy)) {
			continue;
		}

		%newStyle = newScriptObject(%profile.getName()@"_Style");

		//%newStyle.name = %fileName@"_Style";
		foreach$(%field in $ProfileFieldList) {
			if (%field $= "name") continue;

			%value = %profile.getFieldValue(%field);

			if (%value $= "") continue;

			eval("%newStyle."@%field@" = %value;");
			//%newStyle.setFieldValue(%field, %value);
		}

		%newStyle.internalName = %profile.getName();
		%newGroup.add(%newStyle);
	}

	if (%fileName $= "") %fileName = "unmapped";

	%file = "art/gui/"@$Pref::Editor::GuiStyle@"/"@%fileName@".pstyle.cs";
	%result = %newGroup.save(%file,false,"$tmpGroup = ");

	if (%result) info("Save to file success:",%file);
	else info("Save to file failed:",%file);
}
//------------------------------------------------------------------------------