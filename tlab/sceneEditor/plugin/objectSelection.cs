//==============================================================================
// TorqueLab -> SceneEditor Inspector script
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================

//==============================================================================
// Prefabs
//==============================================================================
function SceneEditorPlugin::onSelectObject( %this,%obj ) {
	logd("SceneEditorPlugin::onSelectObject:",%obj);
	SceneEd.setActiveObject(%obj);

	if (%obj.getClassname() $= "Prefab")
		SceneEd.onPrefabSelected(%obj);
	//else
		//SceneEd.noPrefabSelected();

	%name = fileBase(%prefab.filename);
	%path = filePath(%prefab.filename);
	ObjectPrefabSelectedName.setText(%name);
	ObjectPrefabSelectedFolder.setText(%path);
	SEP_PrefabUnpackIcon.visible = 1;
}
function SceneEditorPlugin::onUnselect( %this,%obj ) {
	logd("SceneEditorPlugin::onUnselect:",%obj);
	%name = fileBase(%prefab.filename);
	%path = filePath(%prefab.filename);
	ObjectPrefabSelectedName.setText(%name);
	ObjectPrefabSelectedFolder.setText(%path);
	SEP_PrefabUnpackIcon.visible = 0;
}
