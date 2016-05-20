//==============================================================================
// TorqueLab -> Manage the Editor Camera
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
/* Con::setIntVariable( "$EditTsCtrl::DisplayTypeTop", DisplayTypeTop);                = 0
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeBottom", DisplayTypeBottom);          = 1
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeFront", DisplayTypeFront);            = 2
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeBack", DisplayTypeBack);              = 3
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeLeft", DisplayTypeLeft);              = 4
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeRight", DisplayTypeRight);            = 5
   Con::setIntVariable( "$EditTsCtrl::DisplayTypePerspective", DisplayTypePerspective);= 6
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeIsometric", DisplayTypeIsometric);    = 7*/
//==============================================================================



$LabCameraTypesIcon = "tlab/art/icons/toolbar/ToggleCamera";

//------------------------------------------------------------------------------
//==============================================================================
// Set the initial editor camera and store the game camera settings
function Lab::setInitialCamera(%this) {
	logb("Lab::setInitialCamera( %this )");
	%client = LocalClientConnection;
	Lab.storeControlObjectState(%client);
	//Store the current client camera state so we can restore it when exiting
	Lab.storeClientCameraState(%client);

	//Create a specific Camera Object for editor so we are not changing client camera
	if (!isObject(%this.editCamera)) {
		%this.editCamera = spawnObject("Camera", "Observer","LabEditCam");
		%this.editCamera.scopeToClient(%client);
		//LocalClientConnection.camera.scopeToClient(LocalClientConnection);
	}

	if ($Cfg_Common_Camera_LaunchDefaultAlways || Lab.controlMode $= "")
		Lab.controlMode = $Cfg_Common_Camera_DefaultControlMode;

	if (isObject(ECamViewGui))
		ECamViewGui.visible = $Cfg_Common_Camera_CamViewEnabled;

	MissionCleanup.add(%this.editCamera);
	//%this.editCamera.scopeToClient(%client);
	%client.camera = %this.editCamera;
	%client.setCameraObject(%client.camera);
	Lab.clientWasControlling = %client.getControlObject();
	//Check if we use FreeView mode or Player Controlled camera

	if ( !LocalClientConnection.getControlObject().isMemberOfClass("Player"))
		Lab.controlMode = "Camera";

	if (Lab.controlMode $= "Player") {
		%result = %this.setControlPlayerMode(%client);

		if (%result)
			return;
	}

	Lab.controlMode = "Camera";
	//if (Lab.launchInFreeview)
	Lab.currentCameraMode = "Standard Camera";

	if (isObject(%client.player)) {
		Lab.initialPlayerSuperClass = %client.player.superClass;
		%client.player.superClass = "EditorPlayerData";
		commandToServer('dropCameraAtPlayer');
	}

	//Set back the current camera or set default
	Lab.setControlCameraMode();
}
//==============================================================================
function Lab::storeClientCameraState(%this,%client) {
	Lab.initialControlObject = %client.getControlObject();
	Lab.initialCameraObject = %client.getCameraObject();

	if (isObject(%client.camera)) {
		Lab.initialCamera = %client.camera;
		Lab.initialCameraControlMode = %client.camera.controlMode;
		Lab.initialCameraDatablock = %client.camera.dataBlock;
		Lab.initialCameraPosition = %client.camera.position;
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::restoreClientCameraState(%this) {
	//Needed since it's called when quitting and CLient obj is deleted
	if (!isObject(LocalClientConnection))
		return;
	%client = LocalClientConnection;
	%client.setControlObject(Lab.initialControlObject);
	%client.setCameraObject(Lab.initialCameraObject);

	if (isObject(%client.camera)) {
		%client.camera = Lab.initialCamera;
		%client.camera.controlMode = Lab.initialCameraControlMode;
		%client.camera.dataBlock = Lab.initialCameraDatablock;
		%client.camera.position = Lab.initialCameraPosition;
	}
}
//------------------------------------------------------------------------------

