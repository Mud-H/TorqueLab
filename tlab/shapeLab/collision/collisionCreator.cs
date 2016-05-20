//==============================================================================
// TorqueLab -> ShapeLab -> Shape Selection
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// ShapeLab -> Collision editing
//==============================================================================

//==============================================================================
function ShapeLabColGenMenu::onSelect( %this,%id,%text ) {
	info(%this.internalName, "ShapeLabColGenMenu menu select:",%text,"ID:",%id);
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabColCreate_TypeMenu::onSelect( %this,%id,%text ) {
	info("Create collision type menu select:",%text,"ID:",%id);
	%hullVisible = (%text $= "Convex Hulls") ? true : false;
	ShapeLabColCreate_Hull.visible = %hullVisible;
	ShapeLabColCreate_NoHull.visible = !%hullVisible;
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabColCreate_TargetMenu::onSelect( %this,%id,%text ) {
	info("Create collision target menu select:",%text,"ID:",%id);
}
//------------------------------------------------------------------------------

//==============================================================================
function ShapeLabCollisions::generateMesh( %this ) {
	%colType = 	ShapeLabColCreate_TypeMenu.getText();
	%colTarget = 	ShapeLabColCreate_TargetMenu.getText();
	%this.editCollision();
}
//------------------------------------------------------------------------------
