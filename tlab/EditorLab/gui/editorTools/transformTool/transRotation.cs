//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
// Rotation Transform Functions
//==============================================================================
function ETransformTool::getAbsRotation( %this ,%axis) {
	%obj = EWorldEditor.getSelectedObject(0);

	if( !isObject(%obj) ) {
		// No SceneObjects selected
		return;
	}

	%rot = %obj.getEulerRotation();
	%rot2 = %obj.getRotation();
}
//==============================================================================
// XYZ ROTATION MODE FUNCTIONS
//==============================================================================
//==============================================================================
//Get Current Euler Rotation for type
function ETransformTool::getRotation( %this ) {
	%obj = %this.getSelectionObj();

	if (%obj $= "")
		return;

	%rot = %obj.getEulerRotation();

	if (%axis $= "x" || %axis $= "")
		%this.setFieldValueContainers("RotX",getWord(%rot, 0));

	if (%axis $= "y" || %axis $= "")
		%this.setFieldValueContainers("RotY",getWord(%rot, 1));

	if (%axis $= "z" || %axis $= "")
		%this.setFieldValueContainers("RotZ",getWord(%rot, 2));
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformTool::setSelRotation( %this,%axis ) {
	%rotX = %this-->RotX.getText();
	%rotY = %this-->RotY.getText();
	%rotZ = %this-->RotZ.getText();
	%rotation = "0 0 0";

	if (%axis $= "x" || %axis $= "")
		%rotation.x += %rotX;

	if (%axis $= "y" || %axis $= "")
		%rotation.y += %rotY;

	if (%axis $= "z" || %axis $= "")
		%rotation.z += %rotZ;

	%relative = %this-->RotRelative.isStateOn();
	%localCenter = %this-->RotLocalCenter.isStateOn();
	//EWorldEditor.transformSelection(doPosition, point, doRelativePos, doRotate,rotation, doRelativeRot, doRotLocal, scaleType, scale, isRelative,  isLocal );
	EWorldEditor.transformSelection(false,"",false,true,%rotation,%relative,%localCenter,   "","",false,false);
	//EWorldEditor.transformSelection(false,"",false,true,"0 0 "@mDegToRad(90),true,false,   "","",false,false);
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformTool::clearRotation( %this,%axis ) {
	if (%axis $= "x" || %axis $= "")
		%this.setFieldValueContainers("RotX","0");

	if (%axis $= "y" || %axis $= "")
		%this.setFieldValueContainers("RotY","0");

	if (%axis $= "z" || %axis $= "")
		%this.setFieldValueContainers("RotZ","0");
}
//------------------------------------------------------------------------------
//==============================================================================
// EULER ROTATION MODE FUNCTIONS
//==============================================================================

//==============================================================================
//Get Current Euler Rotation for type
function ETransformTool::getEulerRotation( %this ) {
	%obj = %this.getSelectionObj();

	if (%obj $= "")
		return;

	%rot = %obj.getEulerRotation();

	if (%axis $= "h" || %axis $= "")
		%this.setFieldValueContainers("RotH",getWord(%rot, 0));

	if (%axis $= "p" || %axis $= "")
		%this.setFieldValueContainers("RotP",getWord(%rot, 1));

	if (%axis $= "b" || %axis $= "")
		%this.setFieldValueContainers("RotB",getWord(%rot, 2));
}
//==============================================================================
function ETransformTool::setSelEulerRotation( %this,%axis ) {
	%rotH = %this-->RotH.getText();
	%rotP = %this-->RotP.getText();
	%rotB = %this-->RotB.getText();
	%rotation = "0 0 0";

	if (%axis $= "h" || %axis $= "")
		%rotation.x += %rotH;

	if (%axis $= "p" || %axis $= "")
		%rotation.y += %rotP;

	if (%axis $= "b" || %axis $= "")
		%rotation.z += %rotB;

	%relative = %this-->RotRelative.isStateOn();
	%localCenter = %this-->RotLocalCenter.isStateOn();
	//EWorldEditor.transformSelection(doPosition, point, doRelativePos, doRotate,rotation, doRelativeRot, doRotLocal, scaleType, scale, isRelative,  isLocal );
	EWorldEditor.transformSelection(false,"",false,true,%rotation,%relative,%localCenter,   "","",false,false);
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformTool::clearEulerRotation( %this,%axis ) {
	if (%axis $= "h" || %axis $= "")
		%this.setFieldValueContainers("RotH","0");

	if (%axis $= "p" || %axis $= "")
		%this.setFieldValueContainers("RotP","0");

	if (%axis $= "b" || %axis $= "")
		%this.setFieldValueContainers("RotB","0");
}
//------------------------------------------------------------------------------