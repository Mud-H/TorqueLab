//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$VisibilityOptionsLoaded = false;
$VisibilityClassLoaded = false;
$SideBarVIS_Initialized = false;
//==============================================================================
function SideBarVIS::toggleVisibility(%this) {	
	ETools.toggleTool("VisibilityLayers");
	visibilityToggleBtn.setStateOn(%this.visible);

	if ( %this.visible  ) {
		%this.onShow();
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function SideBarVIS::onShow( %this ) {
	%this.init();
	
}
//==============================================================================
function SideBarVIS::selectSelectable( %this,%opt ) {
	%selArray = %this-->theClassSelArray;
	for ( %i = 0; %i < %this.classArray.count(); %i++ )
	{
	   %class = %this.classArray.getKey( %i );
	   %selVar = "$" @ %class @ "::isSelectable";
	   %isOn = (%opt $= "All") ? 1 : 0;
	   eval(%selVar@ " = "@%isOn@";");
	}
	
}
function SideBarVIS::selectRenderable( %this,%opt ) {
	%selArray = %this-->theClassSelArray;
	for ( %i = 0; %i < %this.classArray.count(); %i++ )
	{
	   %class = %this.classArray.getKey( %i );
	   %selVar = "$" @ %class @ "::isRenderable";
	   %isOn = (%opt $= "All") ? 1 : 0;
	   eval(%selVar@ " = "@%isOn@";");
	}
	
}
//==============================================================================
function SideBarVIS::onResized( %this ) {	
	%classArray = %this-->theClassVisArray;
	%arrayExtentX = SideBarVIS.extent.x;
	%colWidth = (%arrayExtentX-28)/2;
	
	%classArray.colSize = %colWidth;
	%classArray.refresh();
	
	SideBar_VisibleList.colSize = %colWidth;
	SideBar_VisibleList.refresh();
	
}
function SideBarVIS::rebuildUI( %this ) {	
	%this.updateOptions();
		%this.updateClass();
	
}

//==============================================================================
function SideBarVIS::init( %this ) {
	// Create the array if it doesn't already exist.
	if ( !isObject( ar_SideBarVIS ) )
		%array = newArrayObject("ar_SideBarVIS");

	// Create the array if it doesn't already exist.
	if ( !isObject( ar_SideBarVISClass ) )
		%classArray = newArrayObject("ar_SideBarVISClass");

	%this.array = ar_SideBarVIS;
	%this.classArray = ar_SideBarVISClass;
	%this.updatePresetMenu();

	if(!$SideBarVIS_Initialized) {
		//SideBarVIS.position = visibilityToggleBtn.position;
		%this.initOptionsArray();
		%this.initClassArray();
		
		$SideBarVIS_Initialized = true;
	}
	SideBarVIS_CheckPill.visible = 0;
}
//------------------------------------------------------------------------------




//==============================================================================
function Lab::setVisibleDistanceScale( %this,%value ) {
	$Cfg_Common_Objects_visibleDistanceScale = %value;
	$pref::WorldEditor::visibleDistanceScale = %value;	
}