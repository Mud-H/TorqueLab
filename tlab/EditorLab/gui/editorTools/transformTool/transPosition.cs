//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Position And Translation Functions
//==============================================================================
//==============================================================================

//==============================================================================
// Position  Transform Functions
//==============================================================================

//==============================================================================
function ETransformTool::getPosition( %this,%axis ) {
	%pos = EWorldEditor.getSelectionCentroid();

	if (%axis $= "x" || %axis $= "")
		%this.setFieldValueContainers("PosX",getWord(%pos, 0));

	if (%axis $= "y" || %axis $= "")
		%this.setFieldValueContainers("PosY",getWord(%pos, 1));

	if (%axis $= "z" || %axis $= "")
		%this.setFieldValueContainers("PosZ",getWord(%pos, 2));

	// Turn off relative as we're populating absolute values
	%this-->PosRelative.setValue(0);
	// Finally, set the Position check box as active.  The user
	// likely wants this if they're getting the position.
	//%this-->DoPosition.setValue(1);
}
//------------------------------------------------------------------------------


//==============================================================================
function ETransformTool::setSelPosition( %this,%axis ) {
	%posX = %this-->PosX.getText();
	%posY = %this-->PosY.getText();
	%posZ = %this-->PosZ.getText();
	%position = %posX SPC %posY SPC %posZ;
	//EWorldEditor.transformSelection(doPosition, point, doRelativePos, doRotate,rotation, doRelativeRot, doRotLocal, scaleType, scale, isRelative,  isLocal );
	EWorldEditor.transformSelection(true,%position,false,false,"",false,false,   "","",false,false);
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformTool::clearPosition( %this,%axis ) {
	if (%axis $= "x" || %axis $= "")
		%this.setFieldValueContainers("PosX","0");

	if (%axis $= "y" || %axis $= "")
		%this.setFieldValueContainers("PosY","0");

	if (%axis $= "z" || %axis $= "")
		%this.setFieldValueContainers("PosZ","0");
}
//------------------------------------------------------------------------------
