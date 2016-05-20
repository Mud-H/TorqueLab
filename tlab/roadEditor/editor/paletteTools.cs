//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function RoadEditorGui::paletteSync( %this, %mode ) {
	%evalShortcut = "LabPaletteArray-->" @ %mode @ ".setStateOn(1);";
	eval(%evalShortcut);
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::prepSelectionMode( %this ) {
	%mode = %this.getMode();

	if ( %mode $= "RoadEditorAddNodeMode"  ) {
		if ( isObject( %this.getSelectedRoad() ) )
			%this.deleteNode();
	}

	%this.setMode( "RoadEditorSelectMode" );
	LabPaletteArray-->RoadEditorSelectMode.setStateOn(1);
}
//------------------------------------------------------------------------------

//==============================================================================
// Road Palette Tools Buttons Clicks
//==============================================================================
function ERoadEditorSelectModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
}
//------------------------------------------------------------------------------
function ERoadEditorAddModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
}
//------------------------------------------------------------------------------
function ERoadEditorMoveModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
}
//------------------------------------------------------------------------------
function ERoadEditorScaleModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
}
//------------------------------------------------------------------------------
function ERoadEditorInsertModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
}
//------------------------------------------------------------------------------
function ERoadEditorRemoveModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
}
//------------------------------------------------------------------------------