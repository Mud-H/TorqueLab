//==============================================================================
// Lab GuiManager -> Init Lab GUI Manager System
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function GLab_ProfilesTree::updateFilters( %this,%filter ) {
}
//------------------------------------------------------------------------------
function GLab_ProfilesTree::init( %this ) {
	%this.clear();
	$GLab_TreeToolsProfileList = "";
	$GLab_TreeGameProfileList = "";

	foreach( %obj in GameProfileGroup ) {
		// Create a visible name.
		%name = %obj.getName();

		if( %name $= "" )
			continue;

		//if ($GLab_IsToolProfile[%obj.getName()] && !$GLab::ShowEditorsProfile)
		// continue;
		if (strFindWords(%obj.category,"Tools Editor") && !$GLab::ShowEditorsProfile)
			continue;

		// Find which group to put the control in.
		if( %obj.category $= "" ) {
			warnLog("Couln't find a category for this profile:",%obj.getName(),"! Category set to GameCore");

			if ($GLab_IsToolProfile[%obj.getName()])
				%obj.category = "Tools";
			else
				%obj.category = "GameCore";
		}

		%group = %this.findChildItemByName( 0, %obj.category );

		if( !%group )
			%group = %this.insertItem( 0, %obj.category );

		// Insert the item.
		%id = %this.insertItem( %group, %name, %obj.getId(), "" );
	}

	%this.sort( 0, true, true, false );
	%this.schedule(50,"buildVisibleTree");
}
//==============================================================================
//==============================================================================
function GLab_ProfilesTree::checkVisible( %this,%ctrl ) {
	devLog("GLab_ProfilesTree::checkVisible",$GLab::ShowEditorsProfile);
	// $GLab::ShowEditorsProfile = !$GLab::ShowEditorsProfile;
	%ctrl.setActive(false);
	GLab_ProfilesTree.init();
	%this.buildVisibleTree();
	%ctrl.schedule(20,"setActive",true);
	// %ctrl.setSTateOn($GLab::ShowEditorsProfile);
	devLog("GLab_ProfilesTree::end",$GLab::ShowEditorsProfile);
}
//------------------------------------------------------------------------------
function GLab_ProfilesTree::onSelect( %this,%itemId ) {
	logd("GLab_ProfilesTree::onSelect",%itemId);
	%profile = GLab_ProfilesTree.getItemValue( %itemId );

	if (!isObject(%profile))
		warnlog("Invalid profile selected from the tree:",%profile);

	GLab.setSelectedProfile(%profile);
}
//------------------------------------------------------------------------------
//==============================================================================
function GLab_ProfilesTree::onMouseUp( %this,%hitItemId,%mouseClickCount ) {
	logd("GLab_ProfilesTree::onMouseUp",%hitItemId,%mouseClickCount);
	// GLab.setSelectedProfile();
}
//==============================================================================
function GLab_ProfilesTreeRollout::onExpanded( %this ) {
	GLab_ProfilesTree.buildVisibleTree();
}
