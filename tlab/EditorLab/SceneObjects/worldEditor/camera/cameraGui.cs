//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function Lab::initEditorCamera() {
	
	EWorldEditorStatusBarCamera.clear();
	%i=0;

	while($LabCameraDisplayName[%i] !$= "") {
		EWorldEditorStatusBarCamera.add($LabCameraDisplayName[%i],%i);
		%i++;
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//Commonly use from game scripts so added to call new sync function
function EditorGui::syncCameraGui( %this ) {
   Lab.syncCameraGui();
}
//==============================================================================
// Sync the camera information on the editor guis
function Lab::syncCameraGui( %this,%forced ) {
	if( !EditorIsActive() || !isObject(Lab.currentEditor.editorGui))
		return;

	// Sync projection type
	%displayType = Lab.currentEditor.editorGui.getDisplayType();

	if (!%displayType)
		%displayType = 6;

	Lab.checkMenuItem("viewTypeMenu",0,7,%displayType);
	//Non-Perspective Cameras: (Top, Bottom, Left, Right, Front, Back,Isometric)
	//Perspective Cameras: Standard Camera - 1st Person Camera -3rd Person Camera - Orbit Camera -Smooth Camera - Smooth Rot Camera
	EWorldEditorStatusBarCamera.setText($LabCameraDisplayName[%displayType]);
	//Camera Speed
	EditorGuiToolbar-->CameraSpeedEdit.setText($Camera::movementSpeed);
	return;

	if( %displayType != $EditTSCtrl::DisplayTypePerspective ) {
		%this.syncNonPerspectiveCameraGui(%forced);
	} else {
		%this.syncPerspectiveCameraGui(%forced);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the camera information on the editor guis
function Lab::syncNonPerspectiveCameraGui( %this,%forced ) {
	logb("Lab::syncNonPerspectiveCameraGui( %this,%forced )", %this,%forced );

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
	EWorldEditorStatusBarCamera.setText(%name);
	//EditorGuiStatusBar.setCamera( %name,false );
	return;
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the camera information on the editor guis
function Lab::syncPerspectiveCameraGui( %this,%forced ) {
	logb("Lab::syncPerspectiveCameraGui( %this,%forced )", %this,%forced );
	%cameraTypesButton = EditorGuiToolbar-->cameraTypes;

	if (isObject(%cameraTypesButton))
		%cameraTypesButton.setBitmap($LabCameraTypesIcon); //Default Toggle Camera Icon

	// Sync camera settings.
	%flyModeRadioItem = -1;

	if(LocalClientConnection.getControlObject() != LocalClientConnection.player) {
		%mode = LocalClientConnection.camera.getMode();

		if(%mode $= "Fly" && LocalClientConnection.camera.newtonMode) {
			if(LocalClientConnection.camera.newtonRotation == true) {
				EditorGui-->NewtonianRotationCamera.setStateOn(true);
				//%cameraTypesButton.setBitmap("tlab/art/buttons/menubar/smooth-cam-rot");
				%flyModeRadioItem = 4;
				%camModeName = "Smooth Rot Camera";
			} else {
				EditorGui-->NewtonianCamera.setStateOn(true);
				//%cameraTypesButton.setBitmap("tlab/art/buttons/menubar/smooth-cam");
				%flyModeRadioItem = 3;
				%camModeName = "Smooth Camera";
			}
		} else if(%mode $= "EditOrbit") {
			EditorGui-->OrbitCamera.setStateOn(true);
			//%cameraTypesButton.setBitmap("tlab/art/buttons/menubar/orbit-cam");
			%flyModeRadioItem = 1;
			%camModeName = "Orbit Camera";
		} else { // default camera mode
			//EditorGui-->StandardCamera.setStateOn(true);
			//%cameraTypesButton.setBitmap("tlab/art/buttons/toolbar/camera");
			%flyModeRadioItem = 0;
			%camModeName = "Standard Camera";
		}

		//quick way select menu bar options
		Lab.checkFindItem("Camera",0,1,0);
		Lab.checkMenuItem("EditorFreeCameraTypeOptions",0, 4, %flyModeRadioItem);
		Lab.checkMenuItem("EditorPlayerCameraTypeOptions",0, 4, -1);
	} else if (!$isFirstPersonVar) { // if 3rd person
		//EditorGui-->trdPersonCamera.setStateOn(true);
		//%cameraTypesButton.setBitmap("tlab/art/buttons/toolbar/3rd-person-camera");
		%flyModeRadioItem = 1;
		//quick way select menu bar options
		Lab.checkFindItem("Camera",0,1,1);
		Lab.checkMenuItem("EditorPlayerCameraTypeOptions",0, 2, %flyModeRadioItem);
		%camModeName = "3rd Person Camera";
	} else if ($isFirstPersonVar) { // if 1st Person
		EditorGui-->PlayerCamera.setStateOn(true);
		//%cameraTypesButton.setBitmap("tlab/art/buttons/toolbar/player");
		%flyModeRadioItem = 0;
		//quick way select menu bar options
		Lab.checkFindItem("Camera",0,1,1);
		Lab.checkMenuItem("EditorPlayerCameraTypeOptions",0, 2, %flyModeRadioItem);
		Lab.checkMenuItem("EditorFreeCameraTypeOptions",0, 4, -1);
		%camModeName = "1st Person Camera";
	}

	EWorldEditorStatusBarCamera.setText(%camModeName);
	//EditorGuiStatusBar.setCamera(%camModeName,false);
}
//------------------------------------------------------------------------------
