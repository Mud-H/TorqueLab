//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function Lab::OrbitCameraChange(%this, %size, %center,%client) {
	if (%client $= "")
		%client = LocalClientConnection;
	%camera = %client.camera;

	if (!isObject(%camera))
	{
		warnLog("No Camera found for OrbitCameraChange. Function aborted!");
		return;	
	}
	if(%size > 0) {
		%camera.setValidEditOrbitPoint(true);
		%camera.setEditOrbitPoint(%center);
	} else {
		%camera.setValidEditOrbitPoint(false);
	}
}
//------------------------------------------------------------------------------



//==============================================================================
function Lab::fitCameraToSelection( %this,%orbit ) {
	if (%orbit) {
		Lab.setCameraViewMode("Orbit Camera",false);
	}

	//GuiShapeLabPreview have it's own function
	if (isObject(Lab.fitCameraGui)) {
		Lab.fitCameraGui.fitToShape();
		return;
	}

	%radius = EWorldEditor.getSelectionRadius()+1;
	LocalClientConnection.camera.autoFitRadius(%radius);
	LocalClientConnection.setControlObject(LocalClientConnection.camera);
	%this.syncCameraGui();
}
//------------------------------------------------------------------------------
//==============================================================================

function Lab::EditorOrbitCameraChange(%this,%client, %size, %center) {
	Lab.OrbitCameraChange(%size, %center,%client);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::EditorCameraFitRadius(%this,%client, %radius) {
	%client.camera.autoFitRadius(%radius);
	%client.setControlObject(%client.camera);
	Lab.syncCameraGui();
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::DropPlayerAtCamera(%this,%client) {
	// If the player is mounted to something (like a vehicle) drop that at the
	// camera instead. The player will remain mounted.
	%obj = %client.player.getObjectMount();

	if (!isObject(%obj))
		%obj = %client.player;

	%obj.setTransform(%client.camera.getTransform());
	%obj.setVelocity("0 0 0");
	%client.setControlObject(%client.player);
	Lab.syncCameraGui();
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::DropCameraAtPlayer(%this,%client) {
	%client.camera.setTransform(%client.player.getEyeTransform());
	%client.camera.setVelocity("0 0 0");
	%client.setControlObject(%client.camera);
	Lab.syncCameraGui();
}
//------------------------------------------------------------------------------


function Lab::CycleCameraFlyType(%this,%client) {
	if (%client $= "")
		%client = LocalClientConnection;

	if(%client.camera.getMode() $= "Fly") {
		if(%client.camera.newtonMode == false) { // Fly Camera
			// Switch to Newton Fly Mode without rotation damping
			%client.camera.newtonMode = "1";
			%client.camera.newtonRotation = "0";
			%client.camera.setVelocity("0 0 0");
		} else if(%client.camera.newtonRotation == false) { // Newton Camera without rotation damping
			// Switch to Newton Fly Mode with damped rotation
			%client.camera.newtonMode = "1";
			%client.camera.newtonRotation = "1";
			%client.camera.setAngularVelocity("0 0 0");
		} else { // Newton Camera with rotation damping
			// Switch to Fly Mode
			%client.camera.newtonMode = "0";
			%client.camera.newtonRotation = "0";
		}

		%client.setControlObject(%client.camera);
		Lab.syncCameraGui();
	}
}
function Lab::CameraAutoFit(%this,%client,%radius) {
	 %client.camera.autoFitRadius(%radius);
   %client.setControlObject(%client.camera);
  clientCmdSyncEditorGui();
}


//==============================================================================
// Default T3D Game Editor Camera Server Commands
//==============================================================================


function Lab::TogglePathCamera(%this,%client, %val)
	{
		if(%val)
		{
			%control = %client.PathCamera;
		}
		else
		{
			%control = %client.camera;
		}
		%client.setControlObject(%control);
		clientCmdSyncEditorGui();
	}

	function Lab::ToggleCamera(%this,%client)
	{
		if (%client.getControlObject() == %client.player)
		{
			%client.camera.setVelocity("0 0 0");
			%control = %client.camera;
		}
		else
		{
			%client.player.setVelocity("0 0 0");
			%control = %client.player;
		}
		%client.setControlObject(%control);
		clientCmdSyncEditorGui();	
	}

	function Lab::SetEditorCameraPlayer(%this,%client)
	{
		// Switch to Player Mode
		%client.player.setVelocity("0 0 0");
		%client.setControlObject(%client.player);
		ServerConnection.setFirstPerson(1);
		$isFirstPersonVar = 1;

		clientCmdSyncEditorGui();
	}

	function Lab::SetEditorCameraPlayerThird(%this,%client)
	{
		// Swith to Player Mode
		%client.player.setVelocity("0 0 0");
		%client.setControlObject(%client.player);
		ServerConnection.setFirstPerson(0);
		$isFirstPersonVar = 0;

		clientCmdSyncEditorGui();
	}

	

	function Lab::SetEditorCameraStandard(%this,%client)
	{
		// Switch to Fly Mode
		%client.camera.setFlyMode();
		%client.camera.newtonMode = "0";
		%client.camera.newtonRotation = "0";
		%client.setControlObject(%client.camera);
		clientCmdSyncEditorGui();
	}

	function Lab::SetEditorCameraNewton(%this,%client)
	{
		// Switch to Newton Fly Mode without rotation damping
		%client.camera.setFlyMode();
		%client.camera.newtonMode = "1";
		%client.camera.newtonRotation = "0";
		%client.camera.setVelocity("0 0 0");
		%client.setControlObject(%client.camera);
		clientCmdSyncEditorGui();
	}

	function Lab::SetEditorCameraNewtonDamped(%this,%client)
	{
		// Switch to Newton Fly Mode with damped rotation
		%client.camera.setFlyMode();
		%client.camera.newtonMode = "1";
		%client.camera.newtonRotation = "1";
		%client.camera.setAngularVelocity("0 0 0");
		%client.setControlObject(%client.camera);
		clientCmdSyncEditorGui();
	}

	function Lab::SetEditorOrbitCamera(%this,%client)
	{
		%client.camera.setEditOrbitMode();
		%client.setControlObject(%client.camera);
		clientCmdSyncEditorGui();
	}

	function Lab::SetEditorFlyCamera(%this,%client)
	{
		%client.camera.setFlyMode();
		%client.setControlObject(%client.camera);
		clientCmdSyncEditorGui();
	}

	