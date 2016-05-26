//==============================================================================
// GameLab -> Interface Development Gui
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Update a single profile field and set profile dirty if changed
function GLab::profileIsFieldOwner(%this,%profileName,%field ) {
	%field = strreplace(%field,"[","");
	%field = strreplace(%field,"]","");
	%ownList = $ProfOwnedFields[%profileName];
	%ownField = strFind(%ownList,%field);
	return %ownField;
}
//------------------------------------------------------------------------------

//==============================================================================
// Update a single profile field and set profile dirty if changed
function GLab::findParentFieldSource(%this,%profileName,%field ) {
	%parentName = $ProfParent[%profileName];
	%field = strreplace(%field,"[","");
	%field = strreplace(%field,"]","");

	while(%parentName !$= "" ) {
		%profileName = %parentName;

		if (%profileName $= "") {
			return false;
		}

		%ownList = $ProfOwnedFields[%profileName];
		%ownField = strFind(%ownList,%field);

		if (%ownField)
			return %profileName;

		%parentName = $ProfParent[%profileName];

		if (%parentName $= %profileName) {
			return false;
		}
	}

	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
// Profile fields update functions
//==============================================================================

//==============================================================================
// Update a single profile field and set profile dirty if changed
function GLab::updateProfileField(%this,%profile,%fieldData,%value,%updateChilds ) {
	%fieldWords = strreplace(%fieldData,"_"," ");
	%field = getWord(%fieldWords,0);
	%fieldId = getWord(%fieldWords,1);
	%realField = %field;

	if (%fieldId !$= "") {
		%realField = %field@"["@%fieldId@"]";
		devLog("Real field set with index:",%realField);
	}

	if (%field $= "colorFont" ||%field $= "colorFont") {
		warnLog("We are not saving color sets in profile anymore:",%field);
		return;
	}

	if (%field $= "fontSize") {
		if (strstr($ProfStoreFieldProfiles["fontSize"],%profile.getName()) !$= "-1") {
			$ProfStoreFieldDefault[%profile.getName(),"fontSize"] = %value;
		}
	}

	%profileName = %profile.getName();

	if ($GLab::SaveParentProfileField) {
		%ownList = $ProfOwnedFields[%profile.getName()];
		%ownField = strFind(%ownList,%realField);

		if (!%ownField) {
			//This is a parent field
			%parent = %this.findParentFieldSource(%profileName,%field);

			if (isObject(%parent)) {
				%profile = %parent;
			} else {
				warnLog("Couln't find a valid parent to save field to for profile:",%profileName);
				return;
			}
		}
	}

	%this.setProfileFieldValue(%profile,%fieldWords,%value,!%updateChilds);

	if ($GLab_UpdateColorsOnSetChanged && strFind($ProfileUpdateColorList,%field)) {
		devLog("UpdateColors on set changed:",%field,"Value",%value);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Update a single profile field and set profile dirty if changed
function GLab::setProfileFieldValue(%this,%profile,%fieldData,%value,%skipChildren ) {
	%current =  %profile.getFieldValue(%field);

	if (%current $= %value)
		return;

	%field = getWord(%fieldData,0);
	%fieldId = getWord(%fieldData,1);
	devLog("setProfileFieldValue Field",%field,"Field ID",%fieldId);
	%profile.setFieldValue(%field,%value,%fieldId);
	GLab.setProfileDirty( %profile, true );

	if(!%skipChildren)
		GLab.updateProfileChildsField( %profile,%fieldData);
}
//------------------------------------------------------------------------------

//==============================================================================
//GLab.updateProfileChildsField("ToolsButtonProfile","fontcolors 1");
function GLab::updateProfileChildsField(%this,%profile,%fieldData,%subLevel ) {
	if ($ProfChilds[%profile.getName()] $= "") {
		devLog("NoChilds for profile:",%profile.getName());
		return;
	}

	if (%field $= "colorFont" ||%field $= "colorFont") {
		warnLog("Trying to store invalid field to childrens:",%field);
		return;
	}

	%field = getWord(%fieldData,0);
	%fieldId = getWord(%fieldData,1);
	devLog(%profile.getName(),"Cild Field",%field,"Field ID",%fieldId,"Value",%value);
	%value = %profile.getFieldValue(%field,%fieldId);

	if (%field $= "Name") {
		warnLog("Trying to update child name which is really bad...Aborted! Profile",%profile.getName(),"Name",%value);
		return;
	}

	foreach$(%child in $ProfChilds[%profile.getName()]) {
		devLog("Child is ",%child,"Wonded Fields:",$ProfOwnedFields[%child]);
		%ownField = strstr($ProfOwnedFields[%child],%field);

		if (%ownField !$= "-1" ) {
			continue;
		}

		if (!isObject(%child)) {
			warnLog("SKIPPING Trying to update an invalid child:",%child);
			continue;
		}

		devLog(%child,"Child field:",%field,"Set to:",%value);
		//%this.setProfileFieldValue(%child,%fieldData,%value,false);
		%child.setFieldValue(%field,%value,%fieldId);
		GLab.updateProfileChildsField( %child,%fieldData,%subLevel++);
	}
}
//==============================================================================
function GLab::eraseSelectedField( %this ) {
	%profileName = $GLab_SelectedObject.getName();
	%text = "You are about to delete \c2"@ $GLab_SelectedField @"\c0 reference from profile:\c2"@ %profileName @"\c0.";
	%text = %text @ "This will remove the field from profile file and the profile will use the parent value.\n Proceed to field removal?";
	msgBoxYesNo("Erase field from profile",%text,"GLab.eraseProfileField();","");
//  removeProfileField(%profileName,$GLab_SelectedField);
	// %this.eraseProfileField( %profileName, $GLab_SelectedField);
}
function GLab::eraseProfileField( %this,%profileName,%field ) {
	%profileName = $GLab_SelectedObject.getName();
	//  removeProfileField(%profileName,$GLab_SelectedField);
	GameProfilesPM.setDirty(%profileName);
	GameProfilesPM.removeField(%profileName,$GLab_SelectedField);
	GameProfilesPM.saveDirtyObject(%profileName);
	//Now we should set the parent value right now
	%parent = GLab.findParentFieldSource(%profileName,%field);
	%parentValue = %parent.getFieldValue(%field);

	if (%parentValue $= "")
		return;

	GLab.updateProfileField(%profileName,%field,%parentValue,true);
	devLog("GLab::eraseProfileField skipped");
   return;
	%this.syncProfileField(%field);
}
// removeProfileFieldSet(ToolsTextAlt_C,"colorFont");
//------------------------------------------------------------------------------
