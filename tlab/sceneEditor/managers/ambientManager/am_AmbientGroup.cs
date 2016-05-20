//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================



//------------------------------------------------------------------------------
//SEP_AmbientManager.updateAmbientGroups();
function SEP_AmbientManager::updateAmbientGroups( %this ) {	
	AM_AmbientGroupMenu.clear();
	AM_AmbientGroupDelMenu.clear();
	%id = -1;
	AM_AmbientGroupMenu.add("Default","-1");
	foreach$(%ambGroup in $MissionAmbientGroups){
		AM_AmbientGroupMenu.add(%ambGroup.internalName,%id++);
		AM_AmbientGroupDelMenu.add(%ambGroup.internalName,%id);
	}
	AM_AmbientGroupMenu.setText("Default");
	AM_AmbientGroupNewName.setText("");
	AM_AmbientGroupDelMenu.setText("Select Ambient");
}

function AM_AmbientGroupMenu::onSelect( %this,%id,%intName ) {
	devLog("SetAmbient ID:",%id,"Name:",%intName);
	setAmbientId(%id);
	
	%group = MissionAmbientGroup.findObjectByInternalName(%intName);
	foreach(%obj in %group){
		AM_GroupInspectorSrc.addInspect(%obj,1);
	}
}
function SEP_AmbientManager::addNewAmbient( %this,%name ) {

	%name = AM_AmbientGroupNewName.getText();
	if (%name $= "")
		return;
	SceneEd.createAmbientGroup(%name);
}