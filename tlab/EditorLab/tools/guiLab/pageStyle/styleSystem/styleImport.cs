//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================
//==============================================================================
// Load the Specific UI Style settings
function Lab::importProfilesStyle(%this,%style) {
	$LabProfileStylesDirtyList = "";
	ProfileStyles_PM.clearAll();

	if (%style $= "")
		%style = "Default";

	%styleGroup = %style@"_ProfStyleGroup";
	%file = "tlab/gui/styles/"@%style@"/"@%style@".styles.cs";
	delObj(%styleGroup);
	exec(%file);

	foreach(%profStyle in %styleGroup) {
		%profile = %profStyle.internalName;

		if (!isObject(%profile))
			continue;

		foreach$(%field in $ProfileFieldList) {
			%value = %profStyle.getFieldValue(%field);
			%current = %profile.getFieldValue(%field);

			if (%value $= "")
				continue;

			if (%value !$= %current) {
				$LabProfileStylesDirtyList = strAddWord($LabProfileStylesDirtyList,%profile.getName(),true);
				devLog(%profile.getName(),"field:",%field,"Changed from:",%current,"To:",%value);
				//%profile.setFieldValue(%field,%value);
				GLab.updateProfileField(%profile,%field,%value);
			}
		}
	}

	info("New style applied to TorqueLab interface:",%style);
}
//------------------------------------------------------------------------------