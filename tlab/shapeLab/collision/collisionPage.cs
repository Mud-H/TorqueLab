//==============================================================================
// TorqueLab -> ShapeLab -> Shape Selection
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// ShapeLab -> Collision editing
//==============================================================================
//==============================================================================
function ShapeLab::initCollisionPage( %this ) {
	ShapeLabColCreate_TypeMenu.clear();
	ShapeLabColCreate_TypeMenu.add( "Box" );
	ShapeLabColCreate_TypeMenu.add( "Sphere" );
	ShapeLabColCreate_TypeMenu.add( "Capsule" );
	ShapeLabColCreate_TypeMenu.add( "10-DOP X" );
	ShapeLabColCreate_TypeMenu.add( "10-DOP Y" );
	ShapeLabColCreate_TypeMenu.add( "10-DOP Z" );
	ShapeLabColCreate_TypeMenu.add( "18-DOP" );
	ShapeLabColCreate_TypeMenu.add( "26-DOP" );
	ShapeLabColCreate_TypeMenu.add( "Convex Hulls" );
	ShapeLabColRollout-->colType.clear();
	ShapeLabColRollout-->colType.add( "Box" );
	ShapeLabColRollout-->colType.add( "Sphere" );
	ShapeLabColRollout-->colType.add( "Capsule" );
	ShapeLabColRollout-->colType.add( "10-DOP X" );
	ShapeLabColRollout-->colType.add( "10-DOP Y" );
	ShapeLabColRollout-->colType.add( "10-DOP Z" );
	ShapeLabColRollout-->colType.add( "18-DOP" );
	ShapeLabColRollout-->colType.add( "26-DOP" );
	ShapeLabColRollout-->colType.add( "Convex Hulls" );
}
//------------------------------------------------------------------------------
