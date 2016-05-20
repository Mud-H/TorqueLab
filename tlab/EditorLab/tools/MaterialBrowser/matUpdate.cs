//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function MatBrowser::changeMaterialName(%this,%name) {
	%mat = MatBrowser.selectedMaterial;

	if (!isObject(%mat))
		return;

	if (isObject(%name)) {
		labMsgOk("Object with same name exist","There's already an object using name:\c1" SPC	%name SPC "\c0. Please make sure to choose a unique name");
		return;
	}

	%mat.setName(%name);
	matEd_PersistMan.setDirty(%name);
	matEd_PersistMan.saveDirtyObject(%name);
}
//------------------------------------------------------------------------------

//==============================================================================
function MatBrowser::setAsActive(%this,%name) {
	%mat = MatBrowser.selectedMaterial;

	if (!isObject(%mat))
		return;

	MaterialEditorTools.setActiveMaterial(%mat);
}
//------------------------------------------------------------------------------
//==============================================================================
function MatBrowser::setListFilterText(%this,%text) {
	%filterText = %text;

	if (%text $= "" || strFind(%text,"Filter...")) {
		%text = "\c1Filter...";
		MatSel_ListFilterText.setText(%text);
		%filterText = "";
	}

	MatBrowser.filterText = %filterText;
	MatBrowser.loadFilter( MatBrowser.currentFilter, MatBrowser.currentStaticFilter );
}
//------------------------------------------------------------------------------
//==============================================================================
function MatBrowser::updateListFilteredText(%this,%filterText) {
}
//------------------------------------------------------------------------------
