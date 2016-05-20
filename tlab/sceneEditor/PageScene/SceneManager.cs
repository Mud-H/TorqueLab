//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================

//==============================================================================
function SEP_ScenePage::init( %this ) {
	SEP_SceneTreeRenameMenu.clear();
	SEP_SceneTreeRenameMenu.add("Object name",0);
	SEP_SceneTreeRenameMenu.add("Internal name",1);
	%selected = SceneEditorTree.renameInternal;

	if (%selected $= "")
		%selected = "0";

	SEP_SceneTreeRenameMenu.setSelected(%selected);
}
//------------------------------------------------------------------------------

//==============================================================================
function SEP_SceneTreeRenameMenu::onSelect( %this,%id,%text ) {
	SEP_ScenePage.setRenameMode(%id);
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_ScenePage::setRenameMode( %this,%modeId ) {
	if (%modeId $= "0")
		SceneEditorTree.renameInternal = false;
	else
		SceneEditorTree.renameInternal = true;
}
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//==============================================================================
function SEP_ScenePage::updateContent( %this ) {
}
//------------------------------------------------------------------------------
