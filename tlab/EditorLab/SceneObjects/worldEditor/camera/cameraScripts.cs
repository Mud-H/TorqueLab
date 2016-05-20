//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//------------------------------------------------------------------------------

function MenuCameraStatus::onWake( %this ) {
	%this.add( "Standard Camera" );
	%this.add( "1st Person Camera" );
	%this.add( "3rd Person Camera" );
	%this.add( "Orbit Camera" );
	%this.add( "Top View" );
	%this.add( "Bottom View" );
	%this.add( "Left View" );
	%this.add( "Right View" );
	%this.add( "Front View" );
	%this.add( "Back View" );
	%this.add( "Isometric View" );
	%this.add( "Smooth Camera" );
	%this.add( "Smooth Rot Camera" );
}
function MenuCameraStatus::onSelect( %this, %id, %text ) {
	Lab.setCameraViewMode(%text);
}



//==============================================================================
function Lab::CreateCameraViewContextMenu(%this) {
	if( !isObject( GLab.contextMenuField ) )
		Lab.contextMenuCamView = new PopupMenu() {
		superClass = "MenuBuilder";
		isPopup = true;
		item[ 0 ] = "Free view" TAB "" TAB "Lab.setCameraViewMode(\"Standard Camera\");";
		item[ 1 ] = "Orbit view" TAB "" TAB "Lab.setCameraViewMode(\"Orbit Camera\");";
		item[ 2 ] = "1st Person" TAB "" TAB "Lab.setCameraViewMode(\"1st Person Camera\");";
		item[ 3 ] = "3rd Person" TAB "" TAB "Lab.setCameraViewMode(\"3rd Person Camera\");";
		item[ 4 ] = "Smooth move" TAB "" TAB "Lab.setCameraViewMode(\"Smooth Camera\");";
		item[ 5 ] = "Smooth rot." TAB "" TAB "Lab.setCameraViewMode(\"Smooth Rot Camera\");";
		object = -1;
		profile = "ToolsDropdownProfile";
	};
}
function Lab::showCameraViewContextMenu( %this ) {
	loga("Lab::showCameraViewContextMenu( %this )",%this);

	if( !isObject( Lab.CreateCameraViewContextMenu ))
		Lab.CreateCameraViewContextMenu();

	//Lab.contextMenuField.setItem(0,"-->"@%this.linkedField@"<-- Field Options","");
	//Lab.contextMenuField.setItem(2,"-->"@$GLab_SelectedProfile@"<-- Profile Options","");
	//$GLab_ProfileFieldContext = %this.linkedField;
	Lab.contextMenuCamView.showPopup( Canvas );
}