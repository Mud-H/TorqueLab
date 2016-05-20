//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function Lab::setGizmoFieldFromCtrl(%this,%ctrl) {
	%field = %ctrl.internalName;
	%value = GlobalGizmoProfile.getFieldValue(%field);
	%newvalue = %ctrl.getValue();
	devLog("NewValu =",%ctrl.getValue());
	GlobalGizmoProfile.setFieldValue(%field,%newvalue);
}
//------------------------------------------------------------------------------

//==============================================================================
// Handle the escape bind
/* addField( "alignment",           TYPEID< GizmoAlignment >(),   Offset(alignment, GizmoProfile ) );
   addField( "mode",                TYPEID< GizmoMode >(),   Offset(mode, GizmoProfile ) );

   addField( "snapToGrid",          TypeBool,   Offset(snapToGrid, GizmoProfile) );
   addField( "allowSnapRotations",  TypeBool,   Offset(allowSnapRotations, GizmoProfile) );
   addField( "rotationSnap",        TypeF32,    Offset(rotationSnap, GizmoProfile) );
   addField( "allowSnapScale",      TypeBool,   Offset(allowSnapScale, GizmoProfile) );
   addField( "scaleSnap",           TypeF32,    Offset(scaleSnap, GizmoProfile) );
   addField( "renderWhenUsed",      TypeBool,   Offset(renderWhenUsed, GizmoProfile) );
   addField( "renderInfoText",      TypeBool,   Offset(renderInfoText, GizmoProfile) );
   addField( "renderPlane",         TypeBool,   Offset(renderPlane, GizmoProfile) );
   addField( "renderPlaneHashes",   TypeBool,   Offset(renderPlaneHashes, GizmoProfile) );
   addField( "renderSolid",         TypeBool,   Offset(renderSolid, GizmoProfile) );
   addField( "renderMoveGrid",      TypeBool,   Offset( renderMoveGrid, GizmoProfile ) );
   addField( "gridColor",           TypeColorI, Offset(gridColor, GizmoProfile) );
   addField( "planeDim",            TypeF32,    Offset(planeDim, GizmoProfile) );
   addField( "gridSize",            TypePoint3F, Offset(gridSize, GizmoProfile) );
   addField( "screenLength",        TypeS32,    Offset(screenLen, GizmoProfile) );
   addField( "rotateScalar",        TypeF32,    Offset(rotateScalar, GizmoProfile) );
   addField( "scaleScalar",         TypeF32,    Offset(scaleScalar, GizmoProfile) );
   addField( "flags",               TypeS32,    Offset(flags, GizmoProfile) );

//Declared in gui/profiles/gizmo.prof.cs
singleton GizmoProfile( LabGizmoProfile ) {
    // This isnt a GuiControlProfile but fits in well here.
    // Don't really have to initialize this now because that will be done later
    // based on the saved editor prefs.
    screenLength = 100;
    category = "Editor";
    gridColor = "0 156 0 80";
};
//------------------------------------------------------------------------------
singleton GizmoProfile( GlobalGizmoProfile ) {
    // This isnt a GuiControlProfile but fits in well here.
    // Don't really have to initialize this now because that will be done later
    // based on the saved editor prefs.
    screenLength = 100;
    category = "Editor";
};
 */
//==============================================================================
function Lab::setGizmoMode( %this, %mode ) {
	GlobalGizmoProfile.mode = %mode;
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setGizmoAlignment( %this, %alignment ) {
	GlobalGizmoProfile.setFieldValue(alignment, %alignment);
	EWorldEditor.syncGui();
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::setGizmoGridColor( %this, %color ) {
	GlobalGizmoProfile.gridColor = %color;
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setGizmoScalar( %this, %mode, %scalar ) {
	GlobalGizmoProfile.setFieldValue(%mode@"Scalar",%scalar);
}
//------------------------------------------------------------------------------

//==============================================================================
function EditTSCtrl::updateGizmoMode( %this, %mode ) {
	// Called when the gizmo mode is changed from C++
	if ( %mode $= "None" )
		EditorGuiToolbar->NoneModeBtn.performClick();
	else if ( %mode $= "Move" )
		EditorGuiToolbar->MoveModeBtn.performClick();
	else if ( %mode $= "Rotate" )
		EditorGuiToolbar->RotateModeBtn.performClick();
	else if ( %mode $= "Scale" )
		EditorGuiToolbar->ScaleModeBtn.performClick();
}
//------------------------------------------------------------------------------

//==============================================================================

function EWorldEditorAlignPopup::onSelect(%this, %id, %text) {
	if ( GlobalGizmoProfile.mode $= "Scale" && %text $= "World" ) {
		EWorldEditorAlignPopup.setSelected(1);
		return;
	}

	GlobalGizmoProfile.alignment = %text;
}
//------------------------------------------------------------------------------
//==============================================================================

function EWorldEditorNoneModeBtn::onClick(%this) {
	GlobalGizmoProfile.mode = "None";
	EditorGuiStatusBar.setInfo("Selection arrow.");
}
//------------------------------------------------------------------------------
//==============================================================================
function EWorldEditorMoveModeBtn::onClick(%this) {
	GlobalGizmoProfile.mode = "Move";
	%cmdCtrl = "CTRL";

	if( $platform $= "macos" )
		%cmdCtrl = "CMD";

	EditorGuiStatusBar.setInfo( "Move selection.  SHIFT while dragging duplicates objects.  " @ %cmdCtrl @ " to toggle soft snap.  ALT to toggle grid snap." );
}
//------------------------------------------------------------------------------
//==============================================================================
function EWorldEditorRotateModeBtn::onClick(%this) {
	GlobalGizmoProfile.mode = "Rotate";
	EditorGuiStatusBar.setInfo("Rotate selection.");
}
//------------------------------------------------------------------------------
//==============================================================================
function EWorldEditorScaleModeBtn::onClick(%this) {
	GlobalGizmoProfile.mode = "Scale";
	EditorGuiStatusBar.setInfo("Scale selection.");
}
//------------------------------------------------------------------------------


//==============================================================================

function GizmoModeButton::onClick(%this) {
	GlobalGizmoProfile.mode = %this.mode;
	EditorGuiStatusBar.setInfo( %this.toolTip);
}
//------------------------------------------------------------------------------
