//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
postEvent("SceneChanged","There's no DATA");
joinEvent("SceneChanged",SceneEditorTree);
//==============================================================================
function SceneEditorTree::onSceneChanged( %this,%data ) {
   devLog("SceneEditorTree::onSceneChanged DATA:",%data);
	%this.rebuild();
}
//------------------------------------------------------------------------------

//==============================================================================
function SceneEditorTree::rebuild( %this ) {
	%this.clear();
	%this.open(MissionGroup);
	%this.buildVisibleTree();
}
//------------------------------------------------------------------------------

//==============================================================================
function SceneEditorTree::toggleLock( %this ) {
	if(  SceneTreeWindow-->LockSelection.command $= "EWorldEditor.lockSelection(true); SceneEditorTree.toggleLock();" ) {
		SceneTreeWindow-->LockSelection.command = "EWorldEditor.lockSelection(false); SceneEditorTree.toggleLock();";
		SceneTreeWindow-->DeleteSelection.command = "";
	} else {
		SceneTreeWindow-->LockSelection.command = "EWorldEditor.lockSelection(true); SceneEditorTree.toggleLock();";
		SceneTreeWindow-->DeleteSelection.command = "EditorMenuEditDelete();";
	}
}
//------------------------------------------------------------------------------
//==============================================================================
/*
function SceneEditorTree::onMouseUp( %this,%hitItemId, %mouseClickCount ) {
	devLog("SceneEditorTree::onMouseUp( %this,%hitItemId, %mouseClickCount )",%hitItemId, %mouseClickCount);
	%obj = %this.getItemValue(%hitItemId);

	if (!isObject(%obj))
		return;

	switch$(%obj.getClassName()) {
	case "SimGroup":
		if(%mouseClickCount > 1) {
			%obj.treeExpanded = !%obj.treeExpanded;
			%this.expandItem(%hitItemId,%obj.treeExpanded);
		}
	}
}
*/
//------------------------------------------------------------------------------
//==============================================================================
function SceneEditorTree::onMouseUp( %this,%hitItemId, %mouseClickCount ) {
	Parent::onMouseUp(%this, %hitItemId, %mouseClickCount);
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEditorTree::onRightMouseUp( %this,%hitItemId, %mouseClickCount ) {
	Parent::onRightMouseUp(%this, %hitItemId, %mouseClickCount);
	devLog("SceneEditorTree::onRightMouseUp( %this,%hitItemId, %mouseClickCount )",%this,%hitItemId, %mouseClickCount );
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEditorTree::onRightMouseDown( %this,%hitItemId, %mouseClickCount ) {
	Parent::onRightMouseDown(%this, %hitItemId, %mouseClickCount);
	devLog("SceneEditorTree::onRightMouseDown( %this,%hitItemId, %mouseClickCount )",%this,%hitItemId, %mouseClickCount );
}
//------------------------------------------------------------------------------

