//==============================================================================
// TorqueLab -> ETransformTool - Translation functions
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Get selection Dimension
//==============================================================================

//==============================================================================
function ETransformTool::getDimension( %this,%axis ) {
	%obj = EWorldEditor.getSelectedObject(0);

	if( !isObject(%obj) ) {
		// No SceneObjects selected
		return;
	}

	%size = %obj.getObjectBox();
	%scale = %obj.getScale();
	%sizex = (getWord(%size, 3) - getWord(%size, 0)) * getWord(%scale, 0);
	%sizey = (getWord(%size, 4) - getWord(%size, 1)) * getWord(%scale, 1);
	%sizez = (getWord(%size, 5) - getWord(%size, 2)) * getWord(%scale, 2);
	%scale = %obj.scale;

	if (%axis $= "x" || %axis $= "")
		%this.setFieldValueContainers("TransX",%sizex);

	if (%axis $= "y" || %axis $= "")
		%this.setFieldValueContainers("TransY",%sizey);

	if (%axis $= "z" || %axis $= "")
		%this.setFieldValueContainers("TransZ",%sizez);
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformTool::clearTrans( %this,%axis ) {
	if (%axis $= "x" || %axis $= "")
		%this.setFieldValueContainers("TransX","0");

	if (%axis $= "y" || %axis $= "")
		%this.setFieldValueContainers("TransY","0");

	if (%axis $= "z" || %axis $= "all")
		%this.setFieldValueContainers("TransZ","0");
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformTool::setSelTrans( %this,%axis ) {
	%transX = %this-->TransX.getText();
	%transY = %this-->TransY.getText();
	%transZ = %this-->TransZ.getText();
	%pos = EWorldEditor.getSelectionCentroid();
	%pos = "0 0 0";

	if (%axis $= "x" || %axis $= "")
		%pos.x = %transX;

	if (%axis $= "y" || %axis $= "")
		%pos.y = %transY;

	if (%axis $= "z" || %axis $= "")
		%pos.z = %transZ;

	// Turn off relative as we're populating absolute values
	%relative = %this-->PosRelative.getValue();
	//EWorldEditor.transformSelection(doPosition, point, doRelativePos, doRotate,rotation, doRelativeRot, doRotLocal, scaleType, scale, isRelative,  isLocal );
	EWorldEditor.transformSelection(true,%pos,true,false,"",false,false,   "","",false,false);
}
//------------------------------------------------------------------------------