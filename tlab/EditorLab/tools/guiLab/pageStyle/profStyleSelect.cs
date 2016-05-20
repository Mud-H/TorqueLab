//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$LabProfileStyleActive = "";





//==============================================================================
function GLab::updateProfileStylesList( %this,%profile ) {
	GLab_ProfileStylesMenu.clear();
	Lab.scanProfileStyles();
	GLab_ProfileStylesMenu.add("Default",0);
	%selected = "0";

	foreach$(%style in $LabProfileStyles) {
		GLab_ProfileStylesMenu.add(%style,%sId++);

		if ($LabProfileStyleActive $= %style)
			%selected = %sId;
	}

	GLab_ProfileStylesMenu.setSelected(%selected,false);
}
//------------------------------------------------------------------------------

//==============================================================================
function GLab_ProfileStylesMenu::onSelect( %this,%id,%text ) {
	GLab.selectProfileStyle(%text);
}
//------------------------------------------------------------------------------

//==============================================================================
function GLab::selectProfileStyle( %this,%style,%apply ) {
	if (%style !$= $LabProfileStyleActive)
		GLab_StyleUpdatedInfo.visible = 1;

	$LabProfileStyleActive = %style;
	GLab_ProfileStylesNameEdit.setText(%style);
	GLab_ProfileStyleDataTree.init();
	//if (%apply)
	Lab.importProfilesStyle(%style);
}
//------------------------------------------------------------------------------

//==============================================================================
function GLab::importProfileStyle( %this,%style ) {
	if (%style $= "")
		%style = $LabProfileStyleActive;

	if (%style $= "")
		return;

	Lab.importProfilesStyle(%style);
}
//------------------------------------------------------------------------------
//==============================================================================
function GLab::saveProfileStyle( %this,%style ) {
	if (%style $= "")
		%style = GLab_ProfileStylesNameEdit.getText();

	if (%style $= "")
		return;

	Lab.exportCurrentProfilesStyle(%style);
	$LabProfileStyleActive = %style;
	%this.updateProfileStylesList();
}
//------------------------------------------------------------------------------

//==============================================================================
function GLab::saveOriginalProfileFromStyle( %this ) {
	Lab.saveAllDirtyProfilesStyle();
}
//------------------------------------------------------------------------------

//==============================================================================
// Profiles Style Tree Functions and Callbacks
//==============================================================================

//==============================================================================
function GLab_ProfileStyleDataTree::init( %this ) {
	if (!isObject($LabStyleCurrentGroup))
		return;

	%this.clear();

	foreach( %obj in $LabStyleCurrentGroup ) {
		// Create a visible name.
		%profile = %obj.internalName;

		if( %profile $= "" )
			continue;

		%group = %this.findChildItemByName( 0, %profile.category );

		if( !%group )
			%group = %this.insertItem( 0, %profile.category );

		// Insert the item.
		%id = %this.insertItem( %group, %profile, %obj.getId(), "" );
	}

	%this.sort( 0, true, true, false );
	%this.schedule(50,"buildVisibleTree");
}
//==============================================================================
function GLab_ProfileStyleDataTree::onSelect( %this,%itemId ) {
	%profile = GLab_ProfileStyleDataTree.getItemText( %itemId );
	devLog("GLab_ProfileStyleDataTree onSelect Item:",%itemId,"Profile:",%profile);
	%style = $LabProfileStyleActive;
	%styleObj = %profile.getName()@"_"@%style@"Style";
	devLog("StyleObj=",%styleObj);

	//Check if we click a profile and not a group
	if (!isObject(%styleObj)) {
		return;
	}

	GLab.editProfileStyle(%styleObj);
}
//------------------------------------------------------------------------------