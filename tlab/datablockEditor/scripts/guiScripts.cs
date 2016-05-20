//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$DbEd_EditorTreeTabPage = "0";
$DbEd_SelectionTabPage = "0";
//==============================================================================
function DbEd::initGui(%this) {
	DatablockEditorTreeTabBook.selectPage($DbEd_EditorTreeTabPage);
	DbEd_SelectionTabBook.selectPage($DbEd_SelectionTabPage);
}
//------------------------------------------------------------------------------
//==============================================================================
// DatablockEditor Main Tab Book
//==============================================================================

//==============================================================================
function DatablockEditorTreeTabBook::onTabSelected(%this, %text, %id) {
	$DbEd_EditorTreeTabPage = %id;

	switch(%id) {
	case 0:
		DatablockEditorTreeWindow-->DeleteSelection.visible = true;
		DatablockEditorTreeWindow-->CreateSelection.visible = false;
		DatablockEditorTreeWindow-->SaveSelection.visible = false;

	case 1:
		DatablockEditorTreeWindow-->DeleteSelection.visible = false;
		DatablockEditorTreeWindow-->CreateSelection.visible = true;
		DatablockEditorTreeWindow-->SaveSelection.visible = false;

	case 2:
		DatablockEditorTreeWindow-->DeleteSelection.visible = false;
		DatablockEditorTreeWindow-->CreateSelection.visible = false;
		DatablockEditorTreeWindow-->SaveSelection.visible = true;
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function DbEd_SelectionTabBook::onTabSelected(%this, %text, %id) {
	$DbEd_SelectionTabPage = %id;
}
//------------------------------------------------------------------------------


