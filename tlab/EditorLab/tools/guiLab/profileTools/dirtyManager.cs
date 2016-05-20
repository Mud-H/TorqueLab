//==============================================================================
// GuiLab -> Profiles Dirty management
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Profile saving functions
//==============================================================================
//==============================================================================
//==============================================================================
// Set profile as dirty or remove from dirty list
function GLab::setProfileDirty( %this,%profile,%dirty ) {
	%name = %profile.getName();

	if (%dirty) {
		$DirtyList = strAddWord($DirtyList,%name,true);
		%name = %name SPC"*";
	} else {
		$DirtyList = strRemoveWord($DirtyList,%name);
	}

	if (%profile.getName() !$= $GLab_SelectedObject)
		%isNotSelected = true;

	if (!%isNotSelected) {
		GLab_SaveSelectProfileButton.setActive(%dirty);
		%id = GLab_ProfilesTree.getSelectedItem();
	} else {
		%id =  GLab_ProfilesTree.findItemByValue (%profile.getId());
	}

	if (%id $= "0") {
		devLog("Profile not in tree, can't change dirty name");
		return;
	}

	GLab_ProfilesTree.editItem( %id, %name, %profile.getId() );
}
//------------------------------------------------------------------------------
//==============================================================================
// Set profile as dirty or remove from dirty list
function GLab::setProfileNotDirty( %this,%profile ) {
	%name = %profile.getName();
	$DirtyList = strRemoveWord($DirtyList,%name);

	if (%profile.getName() $= $GLab_SelectedObject) {
		GLab_SaveSelectProfileButton.setActive(false);
		%id = GLab_ProfilesTree.getSelectedItem();
	} else {
		%id =  GLab_ProfilesTree.findItemByValue (%profile.getId());
	}

	if (%id $= "0") {
		devLog("Profile not in tree, can't change dirty name");
		return;
	}

	GLab_ProfilesTree.editItem( %id, %name, %profile.getId() );
}
//------------------------------------------------------------------------------

//==============================================================================
function GLab::saveSelectedProfile( %this ) {
	%id = GLab_ProfilesTree.getSelectedItem();
	%profile = GLab_ProfilesTree.getItemValue( %id ).getName();
	%this.saveProfile(%profile,true);
	GLab.setProfileDirty(%profile,  false );
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the Specific UI Style settings
function GLab::saveAllDirtyProfiles( %this) {
	//Store the dirty profiles list so it keep complete for loops
	%dirtyProfiles = $DirtyList;

	//To avoid issue, profile aren't set to dirty before saving is confirmed
	foreach$(%profile in %dirtyProfiles)
		GameProfilesPM.setDirty( %profile );

	//Save all dirty profiles using Persistence Manager
	GameProfilesPM.saveDirty();

	//Set all profiles to not dirty (remove the * after name)
	foreach$(%profile in %dirtyProfiles)
		%this.setProfileNotDirty( %profile );

	//Make sure the dirtylist is empty
	$DirtyList = "";

	//If set, rescan all profiles to update special information globals
	if ($GLab_RescanProfilesOnSave)
		scanAllProfileFile();
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the Specific UI Style settings
function GLab::saveProfile(%this,%profile,%forced,%noRescan ) {
	if (%forced && !GameProfilesPM.isDirty(%profile))
		GameProfilesPM.setDirty( %profile );

	if (strstr($ProfStoreFieldProfiles["fontSize"],%profile.getName()) !$= "-1") {
		%defaultFontSize = $ProfStoreFieldDefault[%profile.getName(),"fontSize"];
		%currentFontSize = %profile.fontSize;

		if ( %profile.fontSize !$= %defaultFontSize) {
			%profile.fontSize =  %defaultFontSize;
			%restoreFontSize = true;
		}
	}

	GameProfilesPM.saveDirtyObject( %profile );
	GLab.setProfileDirty(%profile,false);

	if (%restoreFontSize) {
		%profile.fontSize = %currentFontSize;
		%profile.dump();
	}

	if ($GLab_RescanProfilesOnSave && !%noRescan)
		scanAllProfileFile();
}
//------------------------------------------------------------------------------
