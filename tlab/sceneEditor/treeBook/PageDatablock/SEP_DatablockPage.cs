//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function SEP_DatablockPage::onPageSelected(%this,%book,%text,%id) {
	//devLog(" SEP_DatablockPage::onPageSelected(%this,%book,%text,%id) ",%this,%book,%text,%id);
	
	SceneEditorUtilityBook-->InspectorIcons_Datablock.visible = true;
	SEP_DatablockPage.prepareData();
	//Check if selected object have a datablock
	%this.checkObjectDatablock();
	
	joinEvent("SceneSelectionChanged",SEP_DatablockPage);
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_DatablockPage::prepareData( %this,%reset ) {
   if (%reset)
     SceneDatablockTree.clear(); 
   else if ( SceneEd.dbTreePopulated)
      return;
   
   SceneEd.populateDBTree();
   if ($SceneEd_DatablockTreeSetMode)
      SceneDatablockTree.open(SceneDatablockSet);
   else
      SceneDatablockTree.buildVisibleTree();
   
   SceneEd.dbTreePopulated = true;
}

//==============================================================================
// SceneSelectionChanged Listener
function SEP_DatablockPage::onSceneSelectionChanged( %this,%data ) {
 
  if ($SceneEd_TreeMode $= "Scene")
   return;
    devLog("SEP_DatablockPage::onSceneSelectionChanged",%data);
    %this.checkObjectDatablock();
}



//==============================================================================
function SEP_DatablockPage::datablockFieldModified( %this, %object, %fieldName, %arrayIndex, %oldValue, %newValue ) {
	devLog("SEP_DatablockPage::datablockFieldModified( %this,  %object, %fieldName, %arrayIndex, %oldValue, %newValue )",%this, %object, %fieldName, %arrayIndex, %oldValue, %newValue );
	
	
	SceneEd.setDatablockDirty(%object,true);

   
}
//------------------------------------------------------------------------------

//==============================================================================
function SceneEd::canBeClientSideDatablock( %className ) {
	switch$( %className ) {
	case "SFXProfile" or
			"SFXPlayList" or
			"SFXAmbience" or
			"SFXEnvironment" or
			"SFXState" or
			"SFXDescription" or
			"SFXFMODProject":
		return true;

	default:
		return false;
	}
}
//------------------------------------------------------------------------------


