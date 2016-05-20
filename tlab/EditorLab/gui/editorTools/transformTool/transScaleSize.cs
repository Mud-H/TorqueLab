//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
// Scale/Size Functions
//==============================================================================
//==============================================================================
function ETransformTool::getScale( %this,%axis ) {
	%obj = %this.getSelectionObj();

	if (%obj $= "")
		return;

	%scale = %obj.scale;

	if (%axis $= "x" || %axis $= "")
		%this.setFieldValueContainers("ScaleX",getWord(%scale, 0));

	if (%axis $= "y" || %axis $= "")
		%this.setFieldValueContainers("ScaleY",getWord(%scale, 1));

	if (%axis $= "z" || %axis $= "")
		%this.setFieldValueContainers("ScaleZ",getWord(%scale, 2));
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformTool::setSelScale( %this,%axis ) {
	%scaleX = %this-->ScaleX.getText();
	%scaleY = %this-->ScaleY.getText();
	%scaleZ = %this-->ScaleZ.getText();
	%scale = "0 0 0";

	if (%axis $= "x" || %axis $= "")
		%scale.x += %scaleX;

	if (%axis $= "y" || %axis $= "")
		%scale.y += %scaleY;

	if (%axis $= "z" || %axis $= "")
		%scale.z += %scaleZ;

	//EWorldEditor.transformSelection(doPosition, point, doRelativePos, doRotate,rotation, doRelativeRot, doRotLocal, scaleType, scale, isRelative,  isLocal );
	EWorldEditor.transformSelection(true,"",false,false,"",false,false,"",%scale,false,false);
}
//------------------------------------------------------------------------------
//==============================================================================
function ETransformTool::clearScale( %this,%axis ) {
	if (%axis $= "x" || %axis $= "")
		%this.setFieldValueContainers("ScaleX","0");

	if (%axis $= "y" || %axis $= "")
		%this.setFieldValueContainers("ScaleY","0");

	if (%axis $= "z" || %axis $= "")
		%this.setFieldValueContainers("ScaleZ","0");
}
//------------------------------------------------------------------------------

//==============================================================================
// OLD FUNCTIONS FOR REFERENCE
//==============================================================================
function ETransformSelection::getAbsScaleOLD( %this ) {
	%count = EWorldEditor.getSelectionSize();
	// If we have more than one SceneObject selected,
	// we must exit.
	%obj = -1;

	for( %i=0; %i<%count; %i++) {
		%test = EWorldEditor.getSelectedObject( %i );

		if( %test.isMemberOfClass("SceneObject") ) {
			if( %obj != -1 )
				return;

			%obj = %test;
		}
	}

	if( %obj == -1 ) {
		// No SceneObjects selected
		return;
	}

	%scale = %obj.scale;
	%scalex = getWord(%scale, 0);
	%this-->ScaleX.setText(%scalex);

	if( ETransformSelectionScaleProportional.getValue() == false ) {
		%this-->ScaleY.setText(getWord(%scale, 1));
		%this-->ScaleZ.setText(getWord(%scale, 2));
	} else {
		%this-->ScaleY.setText(%scalex);
		%this-->ScaleZ.setText(%scalex);
	}

	// Turn off relative as we're populating absolute values
	%this-->ScaleRelative.setValue(0);
	// Finally, set the Scale check box as active.  The user
	// likely wants this if they're getting the position.
	%this-->DoScale.setValue(1);
}

function ETransformSelection::getAbsSizeOLD( %this ) {
	%count = EWorldEditor.getSelectionSize();
	// If we have more than one SceneObject selected,
	// we must exit.
	%obj = -1;

	for( %i=0; %i<%count; %i++) {
		%test = EWorldEditor.getSelectedObject( %i );

		if( %test.isMemberOfClass("SceneObject") ) {
			if( %obj != -1 )
				return;

			%obj = %test;
		}
	}

	if( %obj == -1 ) {
		// No SceneObjects selected
		return;
	}

	%size = %obj.getObjectBox();
	%scale = %obj.getScale();
	%sizex = (getWord(%size, 3) - getWord(%size, 0)) * getWord(%scale, 0);
	%this-->SizeX.setText( %sizex );

	if( ETransformSelectionSizeProportional.getValue() == false ) {
		%this-->SizeY.setText( (getWord(%size, 4) - getWord(%size, 1)) * getWord(%scale, 1) );
		%this-->SizeZ.setText( (getWord(%size, 5) - getWord(%size, 2)) * getWord(%scale, 2) );
	} else {
		%this-->SizeY.setText( %sizex );
		%this-->SizeZ.setText( %sizex );
	}

	// Turn off relative as we're populating absolute values
	%this-->SizeRelative.setValue(0);
	// Finally, set the Size check box as active.  The user
	// likely wants this if they're getting the position.
	%this-->DoSize.setValue(1);
}