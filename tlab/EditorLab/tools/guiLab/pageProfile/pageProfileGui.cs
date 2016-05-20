//==============================================================================
// GameLab -> Interface Development Gui
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$GLab_ProfileBook_ActivePageId = 0;

//==============================================================================
function GLab::initProfilePage( %this ) {
	GLab.initProfileParams();
	GLab_ProfileBook.selectPage($GLab_ProfileBook_ActivePageId);
	GLab_ProfilesTree.init();
	GLab_ActiveFieldRollout.expanded = false;
	GLab_ActiveFieldRollout.caption = $GLab_Caption_NoFieldSelected;
	GLab.setSelectedProfile($GLab_SelectedObject);
	//GLab_ProfilesTree.open( GameProfileGroup );
	//GLab_ProfilesTree.buildVisibleTree(true);
}
//------------------------------------------------------------------------------

//==============================================================================
function GLab_ProfileBook::onTabSelected( %this,%text,%id ) {
	$GLab_ProfileBook_ActivePageId = %id;
	$GLab_ProfileBook_ActivePageText = %text;
}
//------------------------------------------------------------------------------