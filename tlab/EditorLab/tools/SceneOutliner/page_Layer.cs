//==============================================================================
// TorqueLab -> Editor Gui General Scripts
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
//SceneOutliner.initPageLayer
function SceneOutliner::initPageLayer(%this) {
   if (!isObject(SOutLayerArray))
	   new ArrayObject(SOutLayerArray);
	   
	 if (!isObject(SOutLayerSet))
	   new SimSet(SOutLayerSet); 
	   SOut_LayerListDefault.clear();
   %this.setDefaultLayer();
   SOut_LayerListTest.clear();
   SceneOutliner.setTestLayer();
}
//------------------------------------------------------------------------------

//==============================================================================
function SceneOutliner::setDefaultLayer(%this) {
	if (!isObject(SOutLayerSetDefault))
	   new SimSet(SOutLayerSetDefault);  
   SOutLayerSet.add(SOutLayerSetDefault);
   
	%list = checkMissionSimGroupForClass(MissionGroup);
	foreach$(%id in %list)
   {
      SOutLayerSetDefault.add(%id);
   }
   SOut_LayerListDefault.mirrorSet = SOutLayerSetDefault;
   SOut_LayerListDefault.doMirror();
}
//------------------------------------------------------------------------------
//==============================================================================
//SceneOutliner.setTestLayer();
function SceneOutliner::setTestLayer(%this) {
	if (!isObject(SOutLayerSetTest))
	   new SimSet(SOutLayerSetTest);  
   SOutLayerSet.add(SOutLayerSetTest);
   
	%list = checkMissionSimGroupForClass(MissionGroup,"TSStatic");
	foreach$(%id in %list)
   {
      SOutLayerSetTest.add(%id);
   }
   SOut_LayerListTest.mirrorSet = SOutLayerSetTest;
   SOut_LayerListTest.doMirror();
}
//------------------------------------------------------------------------------

//==============================================================================
//SceneOutliner.setTestLayer();
function SOut_LayerList::onMouseUp(%this,%itemHit, %mouseClickCount) {
 	%item = %this.getItemObject(%index);
 	devLog("onMouseUp = ",%itemHit,"Class",%mouseClickCount);

}
//------------------------------------------------------------------------------
//==============================================================================
//SceneOutliner.setTestLayer();
function SOut_LayerList::onSelect(%this,%index, %itemText) {
 	%item = %this.getItemObject(%index);
 	devLog("Object selected = ",%item,"Class",%item.getClassName());
 	%this.clearSelection();
 	Scene.selectObject(%item);
}
//------------------------------------------------------------------------------
//==============================================================================
//SceneOutliner.setTestLayer();
function SOut_LayerList::onUnselect(%this,%index, %itemText) {
%item = %this.getItemObject(%index);
 	devLog("Object onUnselect = ",%item,"Class",%item.getClassName());
 	Scene.unselectObject(%item);
}
//------------------------------------------------------------------------------
