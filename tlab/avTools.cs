//==============================================================================
// TorqueLab -> AlterVerse Tools adaptation script
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

$pref::Console::DevLogLevel = 1;
$pref::Mumble::useVoice = 0;
GlobalActionMap.unbind(keyboard,"F10");
GlobalActionMap.unbind(keyboard,"F11");
GlobalActionMap.bindCmd(keyboard,"F10","initTorqueLab(\"Gui\",true);","");
GlobalActionMap.bindCmd(keyboard,"F11","initTorqueLab(\"World\");","");
package LabTools
{
   function lateToolStart()
   {
      
   }
   function toggleToolNow(%type)
   {     
       globalActionMap.unbind( keyboard, "F10");
       if (%type $= "gui")
         initTorqueLab("Gui",true);
      else
          initTorqueLab("World");
   }
   function guiEditorToggle(%make)
   {    
   	if (!%make)
         return;  
       GlobalActionMap.unbind( keyboard, "F10");
       GlobalActionMap.bindCmd( keyboard, "f10", "toggleGuiEdit(true);","" );
       //GlobalActionMap.bindCmd(keyboard,"F10","initTorqueLab(\"Gui\",true);","");
       toggleGuiEdit(true);             
   }

   function worldEditorToggle(%make)
   {
      if (!%make)
         return;      
      GlobalActionMap.unbind( keyboard, "f11");
      GlobalActionMap.bind(keyboard, "f11", toggleEditor);
      toggleEditor(true);     
   }
  
   function GuiEd::launchEditor( %this,%loadLast ) {
        globalActionMap.unbind( mouse, button1);
        Parent::launchEditor( %this,%loadLast );
   }
   function GuiEd::closeEditor( %this ) {
        globalActionMap.bind( mouse, button1, toggleCursor );
        Parent::closeEditor( %this );
   }
   function EditorGui::onWake( %this ) {	  
	   globalActionMap.unbind( mouse, button1);
	   Parent::onWake( %this );
    }
     function EditorGui::onSleep( %this ) {	  
	   globalActionMap.bind( mouse, button1, toggleCursor );
	   Parent::onSleep( %this );
    }
   
};
activatePackage(LabTools);

