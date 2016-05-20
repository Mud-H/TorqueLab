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
   // Create an event.
   // $EManager.registerEvent( "EventManagerCreated" );
   // Create a listener and subscribe.
    //$EListener = new ScriptMsgListener() {
      ///  class = LabListener;
   // };
   // $EManager.subscribe( $EListener, "FrameWorkChanged" );
   // Trigger the event.
    //$EManager.postEvent( "FrameWorkChanged", "Data" );
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
 
    devLog("Event Joined",%listener);
   // Trigger the event.
    $EManager.subscribe( %listener,  %event);
    return %listener;
}

function LabListener::onTestThis( %this, %data )
{
    echo( "FrameWorkChanged onTestThis" );
}
function EditorGuiMain::onFrameWorkChanged( %this, %data )
{
    echo( "EditorGuiMain Triggered" );
}
function GuiCOntrol::onResized( %this, %data )
{
    echo( "GuiCOntrol onResized" );
}
function ResizeTestWin::onResized( %this, %data )
{
    echo( "ResizeTestWin onResized" );
}