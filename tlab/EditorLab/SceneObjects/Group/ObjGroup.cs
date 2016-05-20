//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================
$SEP_AutoGroupActiveSystem = true;
//==============================================================================
function Scene::setActiveSimGroup( %this, %group ) {
	logd("Scene::setActiveSimGroup",%group);

	if (!isObject(%group))// || !isObject(Scene.activeSimGroup))
		return;

	if (!%group.isMemberOfClass("SimGroup"))
		return;

	if (isObject(Scene.activeSimGroup))
		if (Scene.activeSimGroup.getId() $= %group.getId())
			return;

	%currentGroup = Scene.activeSimGroup;	
	Scene.activeSimGroup = %group;	
	Scene.ActiveGroup = %group;	
	
	//Update the active group mark on each trees
	foreach$(%tree in Scene.sceneTrees) {
		if (isObject(%currentGroup)){
			%oldItem = %tree.findItemByObjectId(%currentGroup.getId());
			if (%oldItem !$= "-1")
				%tree.markItem(%oldItem,false);
		}
			
		%newItem = %tree.findItemByObjectId(%group.getId());
		if (%newItem !$= "-1")
			%tree.markItem(%newItem,true);
	}
	
	return Scene.activeSimGroup;
}
//------------------------------------------------------------------------------
//==============================================================================
function Scene::getActiveSimGroup( %this) {
	if (!isObject(%this.activeSimGroup))
		%this.activeSimGroup = %this.setActiveSimGroup(MissionGroup);
	else if (!%this.activeSimGroup.isMemberOfClass("SimGroup"))
		%this.activeSimGroup = %this.setActiveSimGroup(MissionGroup);

	return %this.activeSimGroup;
}
//------------------------------------------------------------------------------
//==============================================================================
function Scene::getNewObjectGroup( %this ) {
	return %this.objectGroup;
}
//------------------------------------------------------------------------------
//==============================================================================
function Scene::setNewObjectGroup( %this, %group ) {
	logd("Scene::setNewObjectGroup",%group);

	if( %this.objectGroup ) {
		%oldItemId = SceneEditorTree.findItemByObjectId( %this.objectGroup );

		if( %oldItemId > 0 )
			SceneEditorTree.markItem( %oldItemId, false );
	}

	if (!isObject(%group))return;

	%group = %group.getID();
	%this.objectGroup = %group;
	if (!isObject(SceneEditorTree))
	   return;
	%itemId = SceneEditorTree.findItemByObjectId( %group );
	SceneEditorTree.markItem( %itemId );
}
//------------------------------------------------------------------------------


//==============================================================================
function Scene::setGroupActive( %this, %group,%active ) {
	logd("Scene::setNewObjectGroup",%group);

	if( %this.objectGroup ) {
		%oldItemId = SceneEditorTree.findItemByObjectId( %this.objectGroup );

		if( %oldItemId > 0 )
			SceneEditorTree.markItem( %oldItemId, false );
	}

	if (!isObject(%group))return;

	%group = %group.getID();
	%this.objectGroup = %group;
	%itemId = SceneEditorTree.findItemByObjectId( %group );
	SceneEditorTree.markItem( %itemId );
}
//------------------------------------------------------------------------------