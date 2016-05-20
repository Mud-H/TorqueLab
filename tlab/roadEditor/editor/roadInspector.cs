//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$RoadInspectorFilterID = "0";
$RoadInspectorFilters["0"] = "-Object,-Mounting";
$RoadInspectorFilters["1"] = "+Object -Transform -Mounting";
$RoadInspectorFilters["2"] = "+Object +Transform -Mounting";
function RoadInspector::inspect( %this, %obj ) {
	%name = "";

	if ( isObject( %obj ) )
		%name = %obj.getName();
	else
		RoadFieldInfoControl.setText( "" );
	if ($RoadInspectorFilterID !$= "")
		%filter = $RoadInspectorFilters[$RoadInspectorFilterID];
	else
		%filter = "";
	%this.groupFilters = %filter;
	//RoadInspectorNameEdit.setValue( %name );
	Parent::inspect( %this, %obj );
}

function RoadInspector::onInspectorFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue ) {
	// Same work to do as for the regular WorldEditor Inspector.
	Inspector::onInspectorFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue );
}

function RoadInspector::onFieldSelected( %this, %fieldName, %fieldTypeStr, %fieldDoc ) {
	RoadFieldInfoControl.setText( "<font:ArialBold:14>" @ %fieldName @ "<font:ArialItalic:14> (" @ %fieldTypeStr @ ") " NL "<font:Arial:14>" @ %fieldDoc );
}

function RoadTreeView::onInspect(%this, %obj) {
	RoadInspector.inspect(%obj);
}