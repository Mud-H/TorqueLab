//==============================================================================
// TorqueLab -> River Editor Node Manager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


function RiverInspector::inspect( %this, %obj ) {
	%name = "";

	if ( isObject( %obj ) )
		%name = %obj.getName();
	else
		RiverFieldInfoControl.setText( "" );

	//RiverInspectorNameEdit.setValue( %name );
	Parent::inspect( %this, %obj );
}

function RiverInspector::onInspectorFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue ) {
	// Same work to do as for the regular WorldEditor Inspector.
	Inspector::onInspectorFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue );
}

function RiverInspector::onFieldSelected( %this, %fieldName, %fieldTypeStr, %fieldDoc ) {
	RiverFieldInfoControl.setText( "<font:ArialBold:14>" @ %fieldName @ "<font:ArialItalic:14> (" @ %fieldTypeStr @ ") " NL "<font:Arial:14>" @ %fieldDoc );
}

