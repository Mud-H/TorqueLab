//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function ShapeLabShapeView::onShapeSelectionChanged( %this ) {
	ShapeLabShapeView.refreshShape();
	ShapeLabShapeView.currentDL = 0;
	
}
//==============================================================================
// GuiShapeEdPreview GuiControl Callbacks
//==============================================================================

//==============================================================================
function ShapeLabShapeView::onDetailChanged( %this ) {
	ShapeLabDetails.updateChangedDetail();
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabShapeView::onEditNodeTransform( %this, %node, %txfm, %gizmoID ) {
	ShapeLab.doEditNodeTransform( %node, %txfm, 1, %gizmoID );
}
//------------------------------------------------------------------------------

//==============================================================================
function ShapeLabShapeView::onNodeSelected( %this, %index ) {
	ShapeLab.selectTreeNode(%index);
}
//------------------------------------------------------------------------------