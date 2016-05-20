//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Copy, Clone and Duplicated Objects Functions
//==============================================================================

//==============================================================================
// Make a copie of all selected objects
function WorldEditor::copySelection( %this,%offset, %copies ) {
	%count = EWorldEditor.getSelectionSize();

	if (%count < 1) {
		warnLog("There's no selected objects to copy!");
		return;
	}

	%addToGroup = Scene.getActiveSimGroup();

	for (%i=1; %i<=%copies; %i++) {
		for( %j=0; %j<%count; %j++) {
			%obj = EWorldEditor.getSelectedObject( %j );

			if( !%obj.isMemberOfClass("SceneObject") ) continue;

			%obj.startDrag = "";
			%clone = %obj.clone();
			%clone.position.x += %offset.x * %i;
			%clone.position.y += %offset.y * %i;
			%clone.position.z += %offset.z * %i;
			%addToGroup.add(%clone);
		}
	}
Scene.doRefreshInspect();

}
//------------------------------------------------------------------------------
//==============================================================================
// Remove selected objects from their group (delete group if empty)
function cloneSelect(%this) {
	if (!isObject("WorldEditorQuickGroup"))
		$QuickGroup = newSimSet("WEditorQuickGroup");

	WEditorQuickGroup.clear();
	%count = EWorldEditor.getSelectionSize();

	for( %i=0; %i<%count; %i++) {
		%obj = EWorldEditor.getSelectedObject( %i );
		WEditorQuickGroup.add(%obj.deepClone());
	}

	EWorldEditor.clearSelection();

	foreach(%obj in WEditorQuickGroup)
		EWorldEditor.selectObject(%obj);

	WEditorQuickGroup.clear();
}
//------------------------------------------------------------------------------