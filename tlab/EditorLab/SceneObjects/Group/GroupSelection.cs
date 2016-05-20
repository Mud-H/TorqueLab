//==============================================================================
// TorqueLab -> Editor Gui General Scripts
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Set object selected in scene (Trees and WorldEditor)
//==============================================================================

function Scene::selectObjectGroup(%this, %simGroup) {
	%isLast = false;

	foreach(%child in %simGroup) {
		if (%simGroup.getObjectIndex(%child) >= %simGroup.getCount()-1)
			%isLast = true;

		Scene.onAddSelection(%child,%isLast,SceneGroup);
	}
}



function Scene::addSelectionToActiveGroup(%this) {	
	%count = EWorldEditor.getSelectionSize();
	if (%count < 1) {
		warnLog("There's no selected objects to copy!");
		return;
	}
	%group = %this.getActiveSimGroup();	
	for( %j=0; %j<%count; %j++) {
		%obj = EWorldEditor.getSelectedObject( %j );
		%group.add(%obj);
	}	
}
