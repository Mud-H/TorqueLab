//==============================================================================
// GameLab -> Interface Development Gui
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function GLab::openProfileCreator( %this ) {
	loga("GLab::openProfileCreator( %this )",%this);
	show(GLab_NewProfileDlg);
	GLab_CloneProfileMenu.setText($GLab_SelectedObject.getName());
}
//------------------------------------------------------------------------------

//==============================================================================
// Load the Specific UI Style settings
function GLab::createProfileClone(%this ) {
	%name = $GLab_NewProfileName;
	%cloneSrc = GLab_CloneProfileMenu.getText();

	if( %name $= "" ) {
		LabMsgOk("Invalid profile name","The name of the new profile is invalid. Please make sure to enter a valid name. Name was:" SPC %name);
		return;
	}

	if( !isObject(%cloneSrc)) {
		LabMsgOk("Invalid profile source","The specified profile source:" SPC %cloneSrc SPC "is not a valid profile, please choose a valid one");
		return;
	}

	if( isObject( %name ) ) {
		LabMsgOk("Name already use","The name you have choose for your profile is already used by another game object. Please choose another.");
		return;
	}

	eval( "new GuiControlProfile( " @ %name @ " : " @ %cloneSrc.getName() @ " );" );
	%name.setFilename(%cloneSrc.getFilename());
	%category = %cloneSrc.category;
	%group = GLab_ProfilesTree.findChildItemByName( 0, %category );
	%id = GLab_ProfilesTree.insertItem( %group, %name, %name.getId(), "" );
	GLab_ProfilesTree.selectItem( %id );
	// Mark it as needing to be saved.
	GameProfileGroup.add(%name);
	%this.setProfileDirty( %name, true );
	hide(GLab_NewProfileDlg);
}
//------------------------------------------------------------------------------

//==============================================================================
// Load the Specific UI Style settings
function GLab::deleteSelectedProfile(%this ) {
	%profile = $GLab_SelectedObject;
	msgBoxYesNo("Delete profile:" SPC %profile.getName(),"You are about to delete the profile named:" SPC %profile.getName() @ ". Are you sure you want to delete it?","GLab.deleteProfile("@%profile.getName()@");");
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the Specific UI Style settings
function GLab::deleteProfile(%this,%profile ) {
	if( isObject( "GameProfilesPM" ) )
		new PersistenceManager( GameProfilesPM );

	// Clear dirty state.
	%this.setProfileDirty( %profile, false );
	// Remove from tree.
	%id = GLab_ProfilesTree.findItemByValue( %profile.getId() );
	GLab_ProfilesTree.removeItem( %id );
	// Remove from file.
	GameProfilesPM.removeObjectFromFile( %profile );
	// Delete profile object.
	%profile.delete();
}
//------------------------------------------------------------------------------
