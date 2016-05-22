//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
 //------------------------------------------------------------------------------
   // Create the EventManager.
   if (!isObject($EManager))
   {
      $EManager = new EventManager()
       {
         queue = "EManager";
         };
   }
//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function Lab::activateEventManager( %this )
{
   setEvent("SceneChanged");
  
}

function setEvent( %event )
{
   // Trigger the event.
    $EManager.registerEvent( %event);
}
function postEvent( %event, %data )
{
   if (	!$EManager.isRegisteredEvent(%event))
      $EManager.registerEvent( %event);
   // Trigger the event.
    $EManager.postEvent( %event, %data);
}
function joinEvent( %event, %listener )
{
   if (!isObject(%listener))
   {
     %class = "LabListener";
      if (%listener !$= "")
         %class = %listener;
         
       %listener = new ScriptMsgListener() {
         // class = "LabMsgListener";
         superClass = %class;
      };
   }
 
  
   // Trigger the event.
    $EManager.subscribe( %listener,  %event);
    return %listener;
}

