//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function SEP_DatablockPage::onPageSelected(%this,%book,%text,%id) {
	//devLog(" SEP_DatablockPage::onPageSelected(%this,%book,%text,%id) ",%this,%book,%text,%id);
	//%this.prepareData();
	//Check if selected object have a datablock
	%this.checkObjectDatablock();
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_DatablockPage::prepareData( %this ) {
   if (SceneDatablockTree.getItemCount() <= 0)
	   
   SceneEd.populateTrees();
   SceneDatablockTree.buildVisibleTree();
}


//==============================================================================
function SEP_DatablockPage::checkObjectDatablock( %this, %object ) {
   //If no object supplied, try with WorldEditor selected object 0
  
   if (%object $= "")
   {
      if (EWorldEditor.getSelectionSize() <= 0)
         return;
      %object = EWorldEditor.getSelectedObject(0);
   }
   
   
	// Select datablock of object if this is a GameBase object.
	
	
	if( %object.isMethod( "getDatablock" ) )
	{
	   %datablock = %object.getDatablock();
	   devLog("GetDatablock Method data:",%datablock);
		SceneEd.selectDatablock( %object.getDatablock() );
	}
	else if( %object.isMemberOfClass( "GameBase" ) )
	{
	   %datablock = %object.getDatablock();
	   devLog("GameBase data:",%datablock);
		SceneEd.selectDatablock( %object.getDatablock() );
	}
	else if( %object.isMemberOfClass( "SFXEmitter" ) && isObject( %object.track ) )
	{
		SceneEd.selectDatablock( %object.track );
	}
	else if( %object.isMemberOfClass( "LightBase" ) && isObject( %object.animationType ) )
	{
		SceneEd.selectDatablock( %object.animationType );
	}
   
}
//------------------------------------------------------------------------------