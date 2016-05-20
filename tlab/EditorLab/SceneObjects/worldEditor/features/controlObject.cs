//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================


//==============================================================================
function toggleEditorControlObject(%val) {
   if (!%val)
      return;
   devLog("toggleEditorControlObject");
   Lab.toggleControlObject();
	
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::toggleControlObject(%this) {
	if (!isObject(%this.gameControlObject)) {
		warnLog("There's no Game control object stored:",%this.gameControlObject);
		return;
	}

	//If Client is controlling game object, set control camera, else do contrary...
	if (%this.gameControlObject == LocalClientConnection.getControlObject())
		Lab.setControlCameraMode(LocalClientConnection);
	else if (%this.gameControlClass $= "Player")
	   Lab.setControlPlayerMode(LocalClientConnection);
   else
      warnLog("There's no control object to toggle to");
	   
		
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::storeControlObjectState(%this,%client) {
   if (!isObject( %client.getControlObject())){
      warnLog("Couldn't find a control object for the client");
      %this.gameControlObject  = "";
      %this.gameControlClass = "";
      return;
   }  
   
	%this.gameControlObject = %client.getControlObject();
	%this.gameControlClass = %this.gameControlObject.getClassName();
	info("Initial control object stored:",%this.gameControlObject.getName(),"Of CLass",%this.gameControlClass);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setControlPlayerMode(%this,%client) { 
   if (!%this.gameControlObject.isMemberOfClass("Player"))
      return false;
      if (isObject( %client.camera)){
          %client.camera.scopeToClient(%client);
          %client.camera.client = %client;   
      }
   %client.setCOntrolObject(%this.gameControlObject);
      
    Lab.currentControlClass = "Player";
   if (!isObject(CtrlEdMap))
   {
      new ActionMap(CtrlEdMap);
      CtrlEdMap.bind(keyboard, "shift-ctrl c", toggleEditorControlObject);
      CtrlEdMap.bind(keyboard, "shift-ctrl v", toggleFirstPerson);
      CtrlEdMap.bind(keyboard, "lshift", sprint);
   }   
   CtrlEdMap.push(); 	
	Lab.controlMode = "Player";
	return true;
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setCameraPlayerMode(%this) {
	//devLog("Lab::setCameraPlayerMode");

	if (!isObject( LocalClientConnection.player)) {
		warnLog("You don't have a player assigned, set spawnPlayer true to spawn one automatically");
		return;
	}

	%player = LocalClientConnection.player;
	%player.setVelocity("0 0 0");

	if (LocalClientConnection.getControlObject() != LocalClientConnection.player)
		LocalClientConnection.setControlObject(%player);
   Lab.controlMode = "Player";
	%this.syncCameraGui();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setControlCameraMode(%this,%client) {
	
	if (!isObject(%client))
		%client = LocalClientConnection;
		
   %curClass = LocalClientConnection.getControlObject().getClassName();   
   
   %client.setCOntrolObject(%client.camera);  
   Lab.currentControlClass = "Camera";
   Lab.controlMode = "Camera";
   if (%curClass $= "Player"){
      %this.DropCameraAtPlayer(%client);
      return;
   }
   Lab.setCameraViewMode("Standard Camera");
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::DropPlayerAtCamera(%this,%client)
{
	if (!isObject(%client))
		%client = LocalClientConnection;
		
   // If the player is mounted to something (like a vehicle) drop that at the
   // camera instead. The player will remain mounted.
   %obj = %client.player.getObjectMount();
   if (!isObject(%obj))
      %obj = %client.player;

   %obj.setTransform(%client.camera.getTransform());
   %obj.setVelocity("0 0 0");

   %client.setControlObject(%client.player);
   clientCmdSyncEditorGui();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::DropCameraAtPlayer(%this,%client)
{
	if (!isObject(%client))
		%client = LocalClientConnection;
		
   %client.camera.setTransform(%client.player.getEyeTransform());
   %client.camera.setVelocity("0 0 0");
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}
//------------------------------------------------------------------------------
