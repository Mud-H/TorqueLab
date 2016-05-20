//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
/*
$DbEd_EditorTreeTabPage = "0";
$DbEd_SelectionTabPage = "0";
//==============================================================================
function DbEd::initGui(%this) {
	SceneDatablockTreeTabBook.selectPage($DbEd_EditorTreeTabPage);
	DbEd_SelectionTabBook.selectPage($DbEd_SelectionTabPage);
}
//------------------------------------------------------------------------------
//==============================================================================
// DatablockEditor Main Tab Book
//==============================================================================

//==============================================================================
function SceneDatablockTreeTabBook::onTabSelected(%this, %text, %id) {
	$DbEd_EditorTreeTabPage = %id;

	switch(%id) {
	case 0:
		SceneDatablockTreeWindow-->DeleteSelection.visible = true;
		SceneDatablockTreeWindow-->CreateSelection.visible = false;
		SceneDatablockTreeWindow-->SaveSelection.visible = false;

	case 1:
		SceneDatablockTreeWindow-->DeleteSelection.visible = false;
		SceneDatablockTreeWindow-->CreateSelection.visible = true;
		SceneDatablockTreeWindow-->SaveSelection.visible = false;

	case 2:
		SceneDatablockTreeWindow-->DeleteSelection.visible = false;
		SceneDatablockTreeWindow-->CreateSelection.visible = false;
		SceneDatablockTreeWindow-->SaveSelection.visible = true;
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function DbEd_SelectionTabBook::onTabSelected(%this, %text, %id) {
	$DbEd_SelectionTabPage = %id;
}
//------------------------------------------------------------------------------

*/
