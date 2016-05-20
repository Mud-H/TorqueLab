//==============================================================================
// TorqueLab -> Scene Tree Reparenting Callbacks
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
//==============================================================================
// Detach the GUIs not saved with EditorGui (For safely save EditorGui)
//==============================================================================
//==============================================================================
//==============================================================================
// Delete all selected objects (Called by delete key)
function Scene::DeleteSelection(%this) {
	//Let's simply delete selected World Objects and the scenetrees should get updated
		Scene.doInspect("");
	//By deleting the object itself, the SceneTree remove it instantly.
	for(%i=EWorldEditor.getSelectionSize()-1;%i>=0;%i--)
	{
		delObj(EWorldEditor.getSelectedObject(i));
	}
	
}
//------------------------------------------------------------------------------
function Scene::onObjectDeleteCompleted(%this) {
	// Let the world editor know to
	// clear its selection.
	
	EWorldEditor.clearSelection();
	EWorldEditor.isDirty = true;
}


/*
$QuickDeleteMode = "2";
//==============================================================================
// Remove selected objects from their group (delete group if empty)
function Scene::DeleteSelection(%this) {
	if ($QuickDeleteMode $= "1") {
		%this.quickDeleteSelection();
		return;
	} else if($QuickDeleteMode $= "2") {
		Scene.doInspect("");
		SceneEditorTree.deleteSelection();

		return;
		%selSize = EWorldEditor.getSelectionSize();

		if( %selSize > 0 )
		EWorldEditor.clearSelection();
			SceneEditorTree.deleteSelection();

		return;
	}
	else if($QuickDeleteMode $= "3") {
		SceneEditorTree.deleteSelection();
		%selSize = EWorldEditor.getSelectionSize();
		if (%count <= 0)
		return;
		Scene.doInspect("");
		SceneEditorTree.deleteSelection();
	Scene.setDirty();
		if( %selSize > 0 )
			SceneEditorTree.deleteSelection();

		return;
	}

	%count = EWorldEditor.getSelectionSize();
	EWorldEditor.clearSelection();
	devLog("Selected objects:",%count);
	if (%count <= 0)
		return;
		
	devLog("Deleting objects:",%count);

	Scene.setDirty();

	
	//foreach$(%tree in Scene.sceneTrees)
		//%tree.deleteSelection();
}
//------------------------------------------------------------------------------
//==============================================================================
// Remove selected objects from their group (delete group if empty)
function Scene::quickDeleteSelection(%this) {
	if (!isObject("WorldEditorQuickGroup"))
		$QuickGroup = newSimSet("WEditorQuickGroup");

	WEditorQuickGroup.clear();
	%count = EWorldEditor.getSelectionSize();

	for( %i=0; %i<%count; %i++) {
		%obj = EWorldEditor.getSelectedObject( %i );
		WEditorQuickGroup.add(%obj);
	}
	EWorldEditor.clearSelection();
	WEditorQuickGroup.deleteAllObjects();
}
//------------------------------------------------------------------------------

*/
