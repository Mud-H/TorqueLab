//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Initialize default plugin settings
function MeshRoadInspector::inspect( %this, %obj ) {
	%name = "";

	if ( isObject( %obj ) )
		%name = %obj.getName();
	else
		MeshFieldInfoControl.setText( "" );

	//RiverInspectorNameEdit.setValue( %name );
	Parent::inspect( %this, %obj );
}

function MeshRoadInspector::onInspectorFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue ) {
	// Same work to do as for the regular WorldEditor Inspector.
	Inspector::onInspectorFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue );
}

function MeshRoadInspector::onFieldSelected( %this, %fieldName, %fieldTypeStr, %fieldDoc ) {
	MeshFieldInfoControl.setText( "<font:ArialBold:14>" @ %fieldName @ "<font:ArialItalic:14> (" @ %fieldTypeStr @ ") " NL "<font:Arial:14>" @ %fieldDoc );
}

function MeshRoadTreeView::onInspect(%this, %obj) {
	MeshRoadInspector.inspect(%obj);
}

