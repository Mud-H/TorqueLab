//==============================================================================
// TorqueLab -> Specific GuiProfiles for RoadEditor
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
singleton GuiControlProfile( RoadEditorProfile ) {
	canKeyFocus = true;
	opaque = true;
	fillColor = "192 192 192 192";
	category = "Editor";
	fontColors[4] = "Fuchsia";
	fontColorLink = "Fuchsia";
};
//------------------------------------------------------------------------------

//==============================================================================
singleton GuiCursor(RoadEditorMoveCursor) {
	hotSpot = "4 4";
	renderOffset = "0 0";
	bitmapName = "art/images/cursors/macCursor";
	category = "Editor";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiCursor( RoadEditorMoveNodeCursor ) {
	hotSpot = "1 1";
	renderOffset = "0 0";
	bitmapName = "./Cursors/outline/drag_node_outline";
	category = "Editor";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiCursor( RoadEditorAddNodeCursor ) {
	hotSpot = "1 1";
	renderOffset = "0 0";
	bitmapName = "./Cursors/outline/add_to_end_outline";
	category = "Editor";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiCursor( RoadEditorInsertNodeCursor ) {
	hotSpot = "1 1";
	renderOffset = "0 0";
	bitmapName = "./Cursors/outline/insert_in_middle_outline";
	category = "Editor";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiCursor( RoadEditorResizeNodeCursor ) {
	hotSpot = "1 1";
	renderOffset = "0 0";
	bitmapName = "./Cursors/outline/widen_path_outline";
	category = "Editor";
};
//------------------------------------------------------------------------------
