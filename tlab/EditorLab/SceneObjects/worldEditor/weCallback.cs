//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function WorldEditor::onDragCopy( %this, %obj ) {
	logd("onDragCopy",%this.getSelectionSize());
	/*if (!isObject(%obj))
		return;
	if (%obj.isMemberOfClass("SimSet"))
		%obj = %obj.getObject(0);

	%obj.startDrag = %obj.getPosition();*/
	//Store the selection centroid for possible cloning
	%this.lastCentroidPos = %this.getSelectionCentroid();
	$DragCopyStarted = true;
}
//------------------------------------------------------------------------------


//==============================================================================
function WorldEditor::onEndDrag( %this, %obj ) {
	logd("WorldEditor::onEndDrag( %this, %obj )",$Button0Pressed);
	scene.doInspect(%obj);
	scene.doApplyInspect();

	

	if ($Cfg_Common_Grid_forceToGrid)
		%this.forceToGrid(%obj);

	if (!$DragCopyStarted)
		return;

	if (%this.lastMoveOffset !$="" && Lab.CloneDragEnabled && GlobalGizmoProfile.mode $= "move") {
		ECloneDrag.copyOffset = %this.lastMoveOffset;
		ETools.showTool(CloneDrag);
	}

	$DragCopyStarted = false;
}
//------------------------------------------------------------------------------


