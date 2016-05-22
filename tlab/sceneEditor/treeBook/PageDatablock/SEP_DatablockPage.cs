//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function SEP_DatablockPage::onPageSelected(%this,%book,%text,%id) {
	//devLog(" SEP_DatablockPage::onPageSelected(%this,%book,%text,%id) ",%this,%book,%text,%id);
	//%this.prepareData();
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_DatablockPage::prepareData( %this ) {
   if (SceneDatablockTree.getItemCount() <= 0)
	   SceneDatablockTree.rebuild();
}
