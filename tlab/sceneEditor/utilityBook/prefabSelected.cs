//==============================================================================
// TorqueLab -> SceneEditor Inspector script
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================

//==============================================================================
// Hack to force LevelInfo update after Cubemap change...
//==============================================================================


function SceneEd::onPrefabSelected( %this,%prefab ) {
	warnLog("SceneEditorPlugin::onPrefabSelected:",%prefab);
	%name = fileBase(%prefab.filename);
	%path = filePath(%prefab.filename);
	ObjectPrefabSelectedName.setText(%name);
	ObjectPrefabSelectedFolder.setText(%path);
	SceneEd.selectedPrefab = %prefab;
	SEP_PrefabUnpackIcon.visible = 1;
	SceneEd_PrefabInfoCtrl.visible = 1;
}
function SceneEd::noPrefabSelected( %this ) {
	warnLog("SceneEditorPlugin::noPrefabSelected:");
	return;
	SceneEd.selectedPrefab = "";
	SceneEd_PrefabInfoCtrl.visible = 0;
	SEP_PrefabUnpackIcon.schedule(200,"setVisible",false);
}
function ObjectPrefabSelectedName::onValidate( %this ) {
	warnLog("Current prefab name changed to:",%this.getText());
}

function SceneEd::unpackSelectedPrefab( %this ) {
	warnLog("Current prefab name changed to:",%this.getText());

	if (!isObject(SceneEd.selectedPrefab)) {
		SceneEd.noPrefabSelected();
		return;
	}

	%success = Lab.ExpandPrefab(SceneEd.selectedPrefab);

	if (%success)
		SceneEd.noPrefabSelected();
}