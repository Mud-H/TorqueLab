//-----------------------------------------------------------------------------
// Copyright (c) 2014 Daniel Buckmaster
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

$Nav::EditorOpen = false;
function NavEd_ActionBook::onTabSelected(%this,%text,%index) {
	switch$(%text) {
	case "Edit":
		NavEditorGui.prepSelectionMode();

	case "Link":
		NavEditorGui.setMode("LinkMode");

	case "Cover":
		NavEditorGui.setMode("CoverMode");

	case "Tile":
		NavEditorGui.setMode("TileMode");

	case "Test":
		NavEditorGui.setMode("TestMode");
	}
}


function NavEditorGui::onEditorActivated(%this) {
	if(%this.selectedObject)
		%this.selectObject(%this.selectedObject);

	%this.prepSelectionMode();
}

function NavEditorGui::onEditorDeactivated(%this) {
	if(%this.getMesh())
		%this.deselect();
}

function NavEditorGui::onModeSet(%this, %mode) {
	// Callback when the nav editor changes mode. Set the appropriate dynamic
	// GUI contents in the properties/actions boxes.
	NavInspector.setVisible(false);
	%actions = NavEditorOptionsWindow-->ActionsBox;
	%actions->SelectActions.setVisible(false);
	%actions->LinkActions.setVisible(false);
	%actions->CoverActions.setVisible(false);
	%actions->TileActions.setVisible(false);
	%actions->TestActions.setVisible(false);
	%properties = NavEd_PropertiesBox;
	NavEd_PropertiesBox->LinkProperties.setVisible(false);
	NavEd_PropertiesBox-->TileProperties.setVisible(false);
	NavEd_PropertiesBox-->TestProperties.setVisible(false);

	switch$(%mode) {
	case "SelectMode":
		NavEd_ActionBook.selectPage(0);
		NavInspector.setVisible(true);
		%actions->SelectActions.setVisible(true);

	case "LinkMode":
		NavEd_ActionBook.selectPage(1);
		%actions->LinkActions.setVisible(true);
		NavEd_PropertiesBox->LinkProperties.setVisible(true);

	case "CoverMode":
		//
		NavEd_ActionBook.selectPage(2);
		%actions->CoverActions.setVisible(true);

	case "TileMode":
		NavEd_ActionBook.selectPage(3);
		%actions->TileActions.setVisible(true);
		NavEd_PropertiesBox->TileProperties.setVisible(true);

	case "TestMode":
		NavEd_ActionBook.selectPage(4);
		%actions->TestActions.setVisible(true);
		NavEd_PropertiesBox->TestProperties.setVisible(true);
	}
}

function NavEditorGui::paletteSync(%this, %mode) {
	// Synchronise the palette (small buttons on the left) with the actual mode
	// the nav editor is in.
	%evalShortcut = "ToolsPaletteArray-->" @ %mode @ ".setStateOn(1);";
	eval(%evalShortcut);
}

function NavEditorGui::onEscapePressed(%this) {
	return false;
}

function NavEditorGui::selectObject(%this, %obj) {
	NavTreeView.clearSelection();

	if(isObject(%obj))
		NavTreeView.selectItem(%obj);

	%this.onObjectSelected(%obj);
}

function NavEditorGui::onObjectSelected(%this, %obj) {
	if(isObject(%this.selectedObject))
		%this.deselect();

	%this.selectedObject = %obj;

	if(isObject(%obj)) {
		%this.selectMesh(%obj);
		NavInspector.inspect(%obj);
	}
}

function NavEditorGui::deleteMesh(%this) {
	if(isObject(%this.selectedObject)) {
		%this.selectedObject.delete();
		%this.selectObject(-1);
	}
}

function NavEditorGui::deleteSelected(%this) {
	switch$(%this.getMode()) {
	case "SelectMode":

		// Try to delete the selected NavMesh.
		if(isObject(NavEditorGui.selectedObject))
			MessageBoxYesNo("Warning",
								 "Are you sure you want to delete" SPC NavEditorGui.selectedObject.getName(),
								 "NavEditorGui.deleteMesh();");

	case "TestMode":
		%this.getPlayer().delete();
		%this.onPlayerDeselected();

	case "LinkMode":
		%this.deleteLink();
		%this.isDirty = true;
	}
}

function NavEditorGui::buildSelectedMeshes(%this) {
	if(isObject(%this.getMesh())) {
		%this.getMesh().build(NavEditorGui.backgroundBuild, NavEditorGui.saveIntermediates);
		%this.isDirty = true;
	}
}


function NavEditorGui::prepSelectionMode(%this) {
	%this.setMode("SelectMode");
	LabPaletteArray-->NavEditorSelectMode.setStateOn(1);
}

//-----------------------------------------------------------------------------

function ENavEditorPaletteButton::onClick(%this) {
	// When clicking on a pelette button, add its description to the bottom of
	// the editor window.
	EditorGuiStatusBar.setInfo(%this.DetailedDesc);
}

//-----------------------------------------------------------------------------
