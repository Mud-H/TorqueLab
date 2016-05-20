//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================




//------------------------------------------------------------------------------
//SEP_AmbientManager.updateAmbientGroups();
function SceneEd::updateAmbientGroups( %this ) {
	SceneEd.ambientGroups = $MissionAmbientGroups;
	SESetup_AmbientsMenu.clear();
	SESetup_DeleteAmbientMenu.clear();
	%id = -1;
	SESetup_AmbientsMenu.add("Default","-1");
	foreach$(%ambGroup in $MissionAmbientGroups){
		SESetup_AmbientsMenu.add(%ambGroup.internalName,%id++);
		SESetup_DeleteAmbientMenu.add(%ambGroup.internalName,%id);
	}
	SESetup_AmbientsMenu.setText("Default");
	SESetup_NewAmbientName.setText("");
	SESetup_DeleteAmbientMenu.setText("Select Ambient");
	
	SEP_AmbientManager.updateAmbientGroups();
}

function SESetup_AmbientsMenu::onSelect( %this,%id,%intName ) {
	devLog("SetAmbient ID:",%id,"Name:",%intName);
	setAmbientId(%id);
}
function SceneEd::addNewAmbient( %this,%name ) {

	%name = SESetup_NewAmbientName.getText();
	if (%name $= "")
		return;
	%this.createAmbientGroup(%name);
}
function SceneEd::createAmbientGroup( %this,%name ) {

	%name = SESetup_NewAmbientName.getText();
	if (%name $= "")
		return;
	
	if (!isObject(MissionAmbientGroup))
		%missionAmbGroup = newSimGroup("MissionAmbientGroup");
	
	%ambGroup = new SimGroup();
	MissionAmbientGroup.add(%ambGroup);
	%ambGroup.internalName = %name;
	%items = SESetup_NewAmbientSrcs.getSelectedItems();
	foreach$(%item in %items){
		%obj = SESetup_NewAmbientSrcs.getItemObject(%item);
		%ambObj[%item] = new ScriptObject();
		%ambObj[%item].internalName = %obj.getClassName();
		for(%i=0;%i<%obj.getFieldCount();%i++){
			%field = %obj.getField(%i);
			devLog("Field = ",%field);
			if (%field $= "internalName" || %field $= "name" || %field $= "parentGroup" )
				continue;
			%value = %obj.getFieldValue(%field);
			eval("%ambObj[%item]."@%field@" = %value;");
			devLog(%ambObj[%item].internalName,"Field",%field,"Set to:",%value);
			//%ambObj.setFieldValue(%field,%value);
		}
		%ambGroup.add(%ambObj[%item]);
	}
	%file = MissionGroup.getFileName();
	%ambFile = strReplace(%file,".mis",".amb.cs");
	MissionAmbientGroup.save(%ambFile);
	
	$MissionAmbientGroups = strAddWord($MissionAmbientGroups,%ambGroup.getId(),true);
	%this.updateAmbientGroups();
		
}
function SceneEd::deleteAmbient( %this,%srcMenu ) {
	if (isObject(%srcMenu))
		%intName = %srcMenu.getText();
	else
		%intName = SESetup_DeleteAmbientMenu.getText();
	if (!isObject(MissionAmbientGroup))
		return;
	%obj = MissionAmbientGroup.findObjectByInternalName(%intName);
	if (!isObject(%obj))
		return;
	
	delObj(%obj);
	%file = MissionGroup.getFileName();
	%ambFile = strReplace(%file,".mis",".amb.cs");
	MissionAmbientGroup.save(%ambFile);
	%this.updateAmbientGroups();
}	
function SceneEd::saveCurrentAmbient( %this ) {
	%currentAmbGroup = getWord($MissionAmbientGroups,$MissionAmbientGroupId);
	if (!isObject(%currentAmbGroup))
		return;
	
	foreach(%ambObj in %currentAmbGroup){
		%type = %ambObj.internalName;
		%updated = false;
			foreach(%envObj in mgEnvironment){
				devLog("ClassName = ",%envObj.getClassName());
				if (%envObj.getClassName() $= %type && !%updated ){
					for(%i=0;%i<%envObj.getFieldCount();%i++){
						%field = %envObj.getField(%i);
						devLog("Field = ",%field);
						if (%field $= "internalName" || %field $= "name" || %field $= "parentGroup" )
							continue;
						%value = %envObj.getFieldValue(%field);
						eval("%ambObj."@%field@" = %value;");
						devLog(%ambObj.internalName,"Field",%field,"Set to:",%value);
						//%ambObj.setFieldValue(%field,%value);
					}			
					%updated = true;
				}
			}
	}
	%file = MissionGroup.getFileName();
	%ambFile = strReplace(%file,".mis",".amb.cs");
	MissionAmbientGroup.save(%ambFile);
	
	
}	
