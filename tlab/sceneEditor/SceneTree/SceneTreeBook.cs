//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function SceneEditorTreeTabBook::onTabSelected( %this,%text, %index ) {
	if( %index == 0) {
		SceneTreeWindow-->DeleteSelection.visible = true;
		SceneTreeWindow-->LockSelection.visible = true;
		SceneTreeWindow-->AddSimGroup.visible = true;
	}else if( %index == 1) { //Datablock Page
		SceneEd.showDatablockPage();
		devLog("Showm");
	} 
	else {
	   
		SceneTreeWindow-->DeleteSelection.visible = false;
		SceneTreeWindow-->LockSelection.visible = false;
		SceneTreeWindow-->AddSimGroup.visible = false;
	}
}
//------------------------------------------------------------------------------

/// @name EditorPlugin Methods
/// @{
/*
function SEP_GroupTree::updateContent( %this ) {
	SceneObjectGroupSet.clear();
	%list = Lab.getMissionObjectClassList("Prefab");

	foreach$(%obj in %list) {
		%obj.internalName = fileBase(%obj.filename);
		SceneObjectGroupSet.add(%obj);
	}

	foreach(%obj in LabSceneObjectGroups)
		SceneObjectGroupSet.add(%obj);

	SEP_GroupTree.open(SceneObjectGroupSet);
}




function SEP_GroupTree::handleRenameObject( %this, %name, %obj ) {
	// %obj.setName(%name);
	%obj.internalName = %name;
}

function SEP_GroupTree::onMouseUp( %this, %object,%clicks ) {
	if (%clicks < 2)
		return;

	%object = %this.selectedId;
	%className = %object.getClassName();

	if (%className $= "SimSet" ) {
		Lab.selectObjectGroup(%object);
		return;
	}

	$LabSingleSelection = true;
	EWorldEditor.clearSelection();
	EWorldEditor.selectObject( %object );
}
function SEP_GroupTree::onSelect( %this, %object ) {
	%this.selectedId = %object;
}
//-----------------------------------------------------------------------------
*/