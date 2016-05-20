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
//==============================================================================
function SceneEditorTree::onAddSelection(%this, %obj, %isLastSelection) {
	Parent::onAddSelection(%this, %obj, %isLastSelection);
	//Scene.onAddSelection(%obj, %isLastSelection);
	//EWorldEditor.selectObject( %obj );
	%selSize = EWorldEditor.getSelectionSize();
	%lockCount = EWorldEditor.getSelectionLockCount();

	if( %lockCount < %selSize ) {
		SceneTreeWindow-->LockSelection.setStateOn(0);
		SceneTreeWindow-->LockSelection.command = "EWorldEditor.lockSelection(true); SceneEditorTree.toggleLock();";
	} else if ( %lockCount > 0 ) {
		SceneTreeWindow-->LockSelection.setStateOn(1);
		SceneTreeWindow-->LockSelection.command = "EWorldEditor.lockSelection(false); SceneEditorTree.toggleLock();";
	}

	if( %selSize > 0 && %lockCount == 0 )
		SceneTreeWindow-->DeleteSelection.command = "EditorMenuEditDelete();";
	else
		SceneTreeWindow-->DeleteSelection.command = "";

	//if( %isLastSelection )
	//SceneInspector.addInspect( %obj );
	//else
	//SceneInspector.addInspect( %obj, false );
}
//------------------------------------------------------------------------------
