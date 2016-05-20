//==============================================================================
// TorqueLab -> ShapeLab -> Main Scripts
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// @todo:
//
// - split node transform editboxes into X Y Z and rot X Y Z with spin controls
//   to allow easier manual editing
// - add groundspeed editing ( use same format as node transform editing )
//
// Known bugs/limitations:
//
// - resizing the GuiTextListCtrl should resize the columns as well
// - modifying the from/in/out properties of a sequence will change the sequence
//   order in the shape ( since it results in remove/add sequence commands )
// - deleting a node should not delete its children as well?
//
//==============================================================================

//==============================================================================
// ShapeLab -> First Initialization
//==============================================================================


//==============================================================================
// Dirty state and saving
//==============================================================================
function ShapeLab::isDirty( %this ) {
	return ( isObject( %this.shape ) && ShapeLabPropWindow-->saveBtn.isActive() );
}

function ShapeLab::setDirty( %this, %dirty ) {
	if ( %dirty )
		ShapeLabPropWindow.text = "ShapeLab utilities *";
	else
		ShapeLabPropWindow.text = "ShapeLab utilities";

	ShapeLabPropWindow-->saveBtn.setActive( %dirty );
	ShapeLab_MainButtonStack-->saveBtn.setActive( %dirty );
	ShapeLabSequences-->saveBtn.setActive( %dirty );
}

function ShapeLab::saveChanges( %this ) {
	if ( isObject( ShapeLab.shape ) ) {
		ShapeLab.saveConstructor( ShapeLab.shape );
		ShapeLab.shape.writeChangeSet();
		ShapeLab.shape.notifyShapeChanged();      // Force game objects to reload shape
		ShapeLab.setDirty( false );
	}
}




//------------------------------------------------------------------------------

function SLE_MainOptionsBook::onTabSelected( %this, %name, %index ) {
	ShapeLab.currentMainOptionsPage = %index;
	%this.activePage = %name;

	switch$ ( %name ) {
	case "Seq":
		ShapeLabPropWindow-->newBtn.ToolTip = "Add new sequence";
		ShapeLabPropWindow-->newBtn.Command = "ShapeLab.onAddSequence();";
		ShapeLabPropWindow-->newBtn.setActive( true );
		ShapeLabPropWindow-->deleteBtn.ToolTip = "Delete selected sequence (cannot be undone)";
		ShapeLabPropWindow-->deleteBtn.Command = "ShapeLabSequences.onDeleteSequence();";
		ShapeLabPropWindow-->deleteBtn.setActive( true );

	case "Node":
		ShapeLabPropWindow-->newBtn.ToolTip = "Add new node";
		ShapeLabPropWindow-->newBtn.Command = "ShapeLabNodes.addNode(ShapeLab_AddNodeName.getText());";
		ShapeLabPropWindow-->newBtn.setActive( true );
		ShapeLabPropWindow-->deleteBtn.ToolTip = "Delete selected node (cannot be undone)";
		ShapeLabPropWindow-->deleteBtn.Command = "ShapeLabNodes.deleteNode();";
		ShapeLabPropWindow-->deleteBtn.setActive( true );

	case "Detail":
		ShapeLabPropWindow-->newBtn.ToolTip = "";
		ShapeLabPropWindow-->newBtn.Command = "";
		ShapeLabPropWindow-->newBtn.setActive( false );
		ShapeLabPropWindow-->deleteBtn.ToolTip = "Delete the selected mesh or detail level (cannot be undone)";
		ShapeLabPropWindow-->deleteBtn.Command = "ShapeLab.deleteMesh();";
		ShapeLabPropWindow-->deleteBtn.setActive( true );

	case "Mat":
		ShapeLabPropWindow-->newBtn.ToolTip = "";
		ShapeLabPropWindow-->newBtn.Command = "";
		ShapeLabPropWindow-->newBtn.setActive( false );
		ShapeLabPropWindow-->deleteBtn.ToolTip = "";
		ShapeLabPropWindow-->deleteBtn.Command = "";
		ShapeLabPropWindow-->deleteBtn.setActive( false );
		// For some reason, the header is not resized correctly until the Materials tab has been
		// displayed at least once, so resize it here too
		ShapeLabMaterials-->materialListHeader.setExtent( getWord( ShapeLabMaterialList.extent, 0 ) SPC "19" );
	}
}









//------------------------------------------------------------------------------
// Shape Preview
//------------------------------------------------------------------------------

function ShapeLabPreviewGui::updatePreviewBackground( %color ) {
	ShapeLabPreviewGui-->previewBackground.color = %color;
	ShapeLabToolbar-->previewBackgroundPicker.color = %color;
}

function showShapeLabPreview() {
	%visible = ShapeLabToolbar-->showPreview.getValue();
	ShapeLabPreviewGui.setVisible( %visible );
}
