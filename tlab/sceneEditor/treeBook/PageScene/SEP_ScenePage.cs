//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function SEP_ScenePage::onPageSelected(%this,%book,%text,%id) {
	//devLog(" SEP_DatablockPage::onPageSelected(%this,%book,%text,%id) ",%this,%book,%text,%id);
	SceneTreeWindow-->DeleteSelection.visible = true;
		SceneTreeWindow-->LockSelection.visible = true;
		SceneTreeWindow-->AddSimGroup.visible = true;
		
		
   %this.checkObject();
}
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function SEP_ScenePage::checkObject( %this,%object ) {
   if (%object $= "")
   {
      if (EWorldEditor.getSelectionSize() <= 0)
         return;
      %object = EWorldEditor.getSelectedObject(0);
   }
   if (isObject(%object))
      Scene.doInspect(%object);
}
