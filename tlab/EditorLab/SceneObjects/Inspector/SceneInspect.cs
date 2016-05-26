//==============================================================================
// TorqueLab -> Editor Gui General Scripts
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$InspectorMultiManual = false;
$InspectorManual = false;
$InspectorApplyManual = true;
$Cfg_Common_Objects_autoInspectApply = false;

//==============================================================================
// New Script System with better naming
//==============================================================================
//==============================================================================
function Scene::doInspect(%this, %obj,%forced) {
	//SceneInspector.inspect(%obj);
	//return;
	if (!$Cfg_Common_Objects_autoInspect && !%forced )
		return;
	if (!$SceneInspectorActive)
		return;
	if (SceneInspector.getNumInspectObjects() == 0) {
		%doInspect = true;
	} else if (SceneInspector.getNumInspectObjects() > 0) {
		if (SceneInspector.isVisible() && %obj !=  SceneInspector.getInspectObject(0))
			%doInspect = true;
	}
   
	if (!%doInspect)
		return;

	SceneInspector.inspect(%obj);
}
//------------------------------------------------------------------------------
//==============================================================================
function Scene::doApplyInspect(%this) {	
	if (!$Cfg_Common_Objects_autoInspectApply)
		return;
		
	if (!$SceneInspectorActive)
		return;
	SceneInspector.apply();
}
//------------------------------------------------------------------------------

//==============================================================================
function Scene::doRefreshInspect(%this) {
	if (!$Cfg_Common_Objects_autoInspectApply)
		return;
		
	if (!$SceneInspectorActive)
		return;
		
	SceneInspector.refresh();
}
//------------------------------------------------------------------------------

//==============================================================================
function Scene::doRemoveInspect(%this, %obj) {
	//SceneInspector.removeInspect(%obj);
	//return;
	if (!$Cfg_Common_Objects_autoInspect || !$SceneInspectorActive)
		return;

	if (!$SceneInspectorActive)
		return;

	SceneInspector.removeInspect(%obj);	
}
//------------------------------------------------------------------------------
//==============================================================================
function Scene::doAddInspect(%this, %obj,%isLast) {
//	SceneInspector.addInspect(%obj,%isLast);
	//return;
	if (!$Cfg_Common_Objects_allowMultiInspect || !$Cfg_Common_Objects_autoInspect)
		return;

	if (!$SceneInspectorActive)
		return;
	SceneInspector.addInspect(%obj,%isLast);
	
}
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
