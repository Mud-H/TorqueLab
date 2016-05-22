//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

/*
function SEP_GroupPage::onVisible( %this,%isVisible ) {
	if (!%isVisible)
		return;

	%this.updateContent();
}


function SEP_GroupPage::updateContent( %this ) {
	SceneObjectGroupSet.clear();
	%list = Lab.getMissionObjectClassList("Prefab");

	if (%list !$= "") {
		%prefabGroup = newSimSet("",SceneObjectGroupSet,"Prefabs");
	}

	foreach$(%obj in %list) {
		%obj.internalName = fileBase(%obj.filename);
		%prefabGroup.add(%obj);
	}

	if (LabSceneObjectGroups.getCount() > 0)
		%groupGroup = newSimSet("",SceneObjectGroupSet,"Group");

	foreach(%obj in LabSceneObjectGroups)
		%groupGroup.add(%obj);

	SEP_GroupTree.open(SceneObjectGroupSet);
}
*/