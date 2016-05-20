//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================
//exec("scripts/gui/profileStyleSetup.cs");


newSimSet("LabStyleGroup");
//==============================================================================
// Load the Specific UI Style settings
function Lab::loadGuiProfileStyle(%this,%style) {
	if (%style !$= "")
		$pref::Editor::GuiStyle = %style;

	foreach(%obj in LabStyleGroup) {
		%obj.delete();
	}

	//Initialize the selected style
	%fileInit = "tlab/gui/styles/"@$Pref::Editor::GuiStyle@"/initStyle.cs";

	if (isFile(%fileInit)) {
		exec(%fileInit);
		info($Pref::Editor::GuiStyle," Style initialized");
		Lab.initProfileStyle();
	}

	%filePathScript = "tlab/gui/styles/"@$Pref::Editor::GuiStyle@"/*.pstyle.cs";

	for(%file = findFirstFile(%filePathScript); %file !$= ""; %file = findNextFile(%filePathScript)) {
		exec( %file );

		foreach(%obj in $tmpGroup) {
			LabStyleGroup.add(%obj);
			$tmpGroup.myFile = %file;
		}
	}

	Lab.updateProfileStyle();
}

//------------------------------------------------------------------------------
//==============================================================================
// Load the Specific UI Style settings
function Lab::updateProfileStyle(%this) {
// LabStyleGroup = LabStyleGroup_Window.deepClone();
	foreach(%profileStyle in LabStyleGroup) {
		%profile =  %profileStyle.internalName;

		if (!isObject(%profile)) {
			%profile = getWord(strreplace(%profileStyle.getName(),"_"," "),0);
		}

		if (!isObject(%profile)) {
			warnLog("A profile style have a invalid target profile. We were looking for:",%profile);
			continue;
		}

		%profileStyle.internalName = %profile.getName();
		%count = %profileStyle.getDynamicFieldCount();

		for(%i=0; %i<%count; %i++) {
			%fieldFull = %profileStyle.getDynamicField(%i);
			%field = getField(%fieldFull,0);
			%value = getField(%fieldFull,1);

			if ($LabProfileField[%field] $= "") {
				warnLog("Field skip because not in list:",%field);
				continue;
			}

			if (%value $= "") {
				warnLog("The value for this field is empty:",%field);
				continue;
			}

			$LabProfileStyleDefault[%profile.getName(),%field] = %value;
			%profile.setFieldValue(%field,%value);
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the Specific UI Style settings
function Lab::getProfileStyleChanges(%this) {
	foreach(%profileStyle in LabStyleGroup) {
		%profile =  %profileStyle.internalName;

		if (!isObject(%profile)) {
			%profile = getWord(strreplace(%profileStyle.getName(),"_"," "),0);
		}

		if (!isObject(%profile)) {
			warnLog("A profile style have a invalid target profile. We were looking for:",%profile);
			continue;
		}

		%count = %profileStyle.getDynamicFieldCount();

		for(%i=0; %i<%count; %i++) {
			%fieldFull = %profileStyle.getDynamicField(%i);
			%field = getField(%fieldFull,0);
			%value = getField(%fieldFull,1);
			%newValue =  %profile.getFieldValue(%field);

			if(%value !$= %newValue) {
				%profileStyle.setFieldValue(%field,%newValue);
			}
		}
	}
}
//------------------------------------------------------------------------------
