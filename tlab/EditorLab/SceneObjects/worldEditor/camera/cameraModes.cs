//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Define some global value
//==============================================================================
$LabDefaultCameraView = "Standard Camera";

$LabCameraDisplayName[0] = "Top View";
$LabCameraDisplayName[1] = "Bottom View";
$LabCameraDisplayName[2] = "Front View";
$LabCameraDisplayName[3] = "Back View";
$LabCameraDisplayName[4] = "Left View";
$LabCameraDisplayName[5] = "Right View";
$LabCameraDisplayName[6] = "Standard Camera";
$LabCameraDisplayName[7] = "Isometric View";
$LabCameraDisplayName[8] = "1st Person Camera";
$LabCameraDisplayName[9] = "3rd Person Camera";
$LabCameraDisplayName[10] = "Orbit Camera";
$LabCameraDisplayName[11] = "Smooth Camera";
$LabCameraDisplayName[12] = "Smooth Rot Camera";

$LabCameraDisplayType["Top View"] = $EditTsCtrl::DisplayTypeTop;
$LabCameraDisplayType["Bottom View"] = $EditTsCtrl::DisplayTypeBottom;
$LabCameraDisplayType["Left View"] = $EditTsCtrl::DisplayTypeLeft;
$LabCameraDisplayType["Right View"] = $EditTsCtrl::DisplayTypeRight;
$LabCameraDisplayType["Front View"] = $EditTsCtrl::DisplayTypeFront;
$LabCameraDisplayType["Back View"] = $EditTsCtrl::DisplayTypeBack;
$LabCameraDisplayType["Isometric View"] = $EditTsCtrl::DisplayTypeIsometric;
$LabCameraDisplayType["Standard Camera"] = $EditTsCtrl::DisplayTypePerspective;
$LabCameraDisplayType["1st Person Camera"] = $EditTsCtrl::DisplayTypePerspective;
$LabCameraDisplayType["3rd Person Camera"] = $EditTsCtrl::DisplayTypePerspective;
$LabCameraDisplayType["Orbit Camera"] = $EditTsCtrl::DisplayTypePerspective;
$LabCameraDisplayType["Smooth Camera"] = $EditTsCtrl::DisplayTypePerspective;
$LabCameraDisplayType["Smooth Rot Camera"] = $EditTsCtrl::DisplayTypePerspective;

$LabCameraDisplayMode["Top View"] = "Standard";
$LabCameraDisplayMode["Bottom View"] ="Standard";
$LabCameraDisplayMode["Left View"] = "Standard";
$LabCameraDisplayMode["Right View"] = "Standard";
$LabCameraDisplayMode["Front View"] ="Standard";
$LabCameraDisplayMode["Back View"] = "Standard";
$LabCameraDisplayMode["Isometric View"] = "Standard";
$LabCameraDisplayMode["Standard Camera"] = "Standard";
$LabCameraDisplayMode["1st Person Camera"] = "Player";
$LabCameraDisplayMode["3rd Person Camera"] = "PlayerThird";
$LabCameraDisplayMode["Orbit Camera"] = "Orbit";
$LabCameraDisplayMode["Smooth Camera"] = "Newton";
$LabCameraDisplayMode["Smooth Rot Camera"] = "NewtonDamped";

//==============================================================================
function Lab::setCameraViewMode( %this, %mode ) {
	if(%mode $= "Top View" || %mode $="") {
		%mode = "Standard Camera";
	}

	Lab.SetEditorCameraView($LabCameraDisplayMode[%mode]);
	Lab.setCameraViewType( $LabCameraDisplayType[%mode] );
	Lab.currentCameraMode = %mode;
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::SetEditorCameraView(%this,%type) {
	%client = LocalClientConnection;

	switch$(%type) {
	case "Standard":
		// Switch to Fly Mode
		%client.camera.setFlyMode();
		%client.camera.newtonMode = "0";
		%client.camera.newtonRotation = "0";
		%client.setControlObject(%client.camera);

	case "Newton":
		// Switch to Newton Fly Mode without rotation damping
		%client.camera.setFlyMode();
		%client.camera.newtonMode = "1";
		%client.camera.newtonRotation = "0";
		%client.camera.setVelocity("0 0 0");
		%client.setControlObject(%client.camera);

	case "NewtonDamped":
		// Switch to Newton Fly Mode with damped rotation
		%client.camera.setFlyMode();
		%client.camera.newtonMode = "1";
		%client.camera.newtonRotation = "1";
		%client.camera.setAngularVelocity("0 0 0");
		%client.setControlObject(%client.camera);

	case "Orbit":
		LocalClientConnection.camera.setEditOrbitMode();
		%client.setControlObject(%client.camera);
		devLog("Orbit mode activated");

	case "FlyCamera":
		%client.camera.setFlyMode();
		%client.setControlObject(%client.camera);

	case "Player":
		%client.player.setVelocity("0 0 0");
		LocalClientConnection.setControlObject(LocalClientConnection.player);
		ServerConnection.setFirstPerson(1);
		$isFirstPersonVar = 1;

	case "PlayerThird":
		%client.player.setVelocity("0 0 0");
		%client.setControlObject(%client.player);
		ServerConnection.setFirstPerson(0);
		$isFirstPersonVar = 0;
	}

	if( %type != $EditTSCtrl::DisplayTypePerspective ) {
		Lab.setFlyCameraData();
	}

	//Lab.syncCameraGui();
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the current camera type info in different editor areas
function Lab::setCameraViewType( %this, %type ) {
	%gui = %this.currentEditor.editorGui;

	if( !isObject( %gui ) )
		return;

	if ($LabCameraDisplayType[%type] !$="")
		%type = $LabCameraDisplayType[%type];
	
	// Store the current camera rotation so we can restore it correctly when
	// switching back to perspective view
	if ( %gui.getDisplayType() == $EditTSCtrl::DisplayTypePerspective )
		%this.lastPerspectiveCamRotation = LocalClientConnection.camera.getRotation();

	%gui.setDisplayType( %type );

	if ( %gui.getDisplayType() == $EditTSCtrl::DisplayTypePerspective )
		LocalClientConnection.camera.setRotation( %this.lastPerspectiveCamRotation );

	%this.cameraDisplayType = %type;
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the camera information on the editor guis
function Lab::setFlyCameraData( %this,%forced ) {
	logb("Lab::setFlyCameraData( %this,%forced )", %this,%forced );

	switch( %displayType ) {
	case $EditTSCtrl::DisplayTypeTop:
		%name = "Top View";
		%camRot = "0 0 0";

	case $EditTSCtrl::DisplayTypeBottom:
		%name = "Bottom View";
		%camRot = "3.14159 0 0";

	case $EditTSCtrl::DisplayTypeLeft:
		%name = "Left View";
		%camRot = "-1.571 0 1.571";

	case $EditTSCtrl::DisplayTypeRight:
		%name = "Right View";
		%camRot = "-1.571 0 -1.571";

	case $EditTSCtrl::DisplayTypeFront:
		%name = "Front View";
		%camRot = "-1.571 0 3.14159";

	case $EditTSCtrl::DisplayTypeBack:
		%name = "Back View";
		%camRot = "-1.571 0 0";

	case $EditTSCtrl::DisplayTypeIsometric:
		%name = "Isometric View";
		%camRot = "0 0 0";
	}

	LocalClientConnection.camera.controlMode = "Fly";
	LocalClientConnection.camera.setRotation( %camRot );
	//EWorldEditorStatusBarCamera.setText(%name);
	//EditorGuiStatusBar.setCamera( %name,false );
	return;
}
//------------------------------------------------------------------------------

