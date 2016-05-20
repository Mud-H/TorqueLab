//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================
$SceneEditor_DropSinglePosition = "";
$SceneEditor_AddObjectLastZ = true;
$SceneEditor_AddObjectCurrentSel = true;
//==============================================================================
// CHeck for custom drop setting, if not the WorldEditor will use it dropType
// When $SceneEd_SkipEditorDrop is true, the WorldEditor dropSelection is skip
//		and position is set from script
function Scene::getCreateObjectPosition() {
	%focusPoint = LocalClientConnection.getControlObject().getLookAtPoint();
	$SceneEd_SkipEditorDrop = true;

	if ($SceneEditor_DropSinglePosition !$= "")
		%dropPos = $SceneEditor_DropSinglePosition;
	else if (Scene.dropMode $= "currentSel" && isObject(EWorldEditor.getSelectedObject(0)))
		%dropPos = EWorldEditor.getSelectedObject(0).position;
	else if (Scene.dropMode $= "currentSel" )
		warnLog("Can't drop object at current selection, because nothing is selected");

	//else if( %focusPoint $= "" && strFind(strlwr(EWorldEditor.droptype),"toterrain"))
	//	%dropPos = "0 0 0";
	$SceneEditor_DropSinglePosition = "";
	

	if (%dropPos !$= "")
		return %dropPos;

	%position =  getWord( %focusPoint, 1 ) SPC getWord( %focusPoint, 2 ) SPC getWord( %focusPoint, 3 );

	if (%position.z !$= "" && Scene.dropMode $= "currentSelZ") {
		%position =  getWord( %focusPoint, 1 ) SPC getWord( %focusPoint, 2 ) SPC getWord( %focusPoint, 3 );

		if ($WorldEditor_LastSelZ !$= "")
			%position.z = $WorldEditor_LastSelZ;
	
		return %position;
	}
	

	$SceneEd_SkipEditorDrop = false;
	return %position;
}
//------------------------------------------------------------------------------
//==============================================================================
// CHeck for custom drop setting, if not the WorldEditor will use it dropType
// When $SceneEd_SkipEditorDrop is true, the WorldEditor dropSelection is skip
//		and position is set from script
function Scene::getCreateObjectTransform() {
	%position = Scene.getCreateObjectPosition();
	
	%transform = %position SPC "1 0 0 0";
	
	if (!$Cfg_Scene_IgnoreDropSelRotation && (Scene.dropMode $= "currentSel" || Scene.dropMode $= "currentSelZ")){
		%selRot = EWorldEditor.getSelectedObject(0).rotation;
		%transform = %position SPC %selRot;
		logd("Dropping object at selected rotation:",%selRot);
	}
	return %transform;
}
//------------------------------------------------------------------------------
//==============================================================================
function Scene::onObjectCreated( %this, %objId,%noDrop ) {
	// Can we submit an undo action?
	if ( isObject( %objId ) )
		MECreateUndoAction::submit( %objId );

	SceneEditorTree.clearSelection();
	EWorldEditor.clearSelection();
	EWorldEditor.selectObject( %objId );
		
	if (!%noDrop)
		EWorldEditor.dropSelection( true );
}
//------------------------------------------------------------------------------
//==============================================================================
function Scene::onFinishCreateObject( %this, %objId ) {
	%activeGroup = Scene.getActiveSimGroup();
	%activeGroup.add( %objId );

	if( %objId.isMemberOfClass( "SceneObject" )||%objId.isMemberOfClass( "Trigger" ) ) {
		%objId.position = %this.getCreateObjectPosition();
		%transform = %this.getCreateObjectTransform();
	%position = getWords(%transform,0,2);
	%rotation = getWords(%transform,3,6);
%objId.position = %position;
	%objId.rotation = %rotation;
		//flush new position
		%objId.setTransform( %objId.getTransform() );
	}
	%this.onObjectCreated( %objId );
}
//------------------------------------------------------------------------------
//==============================================================================
//Scene.createSimGroup();
function Scene::createSimGroup( %this ) {
	if ( !$missionRunning )
		return;

	%addToGroup = Scene.getActiveSimGroup();
	%objId = new SimGroup() {
		internalName = getUniqueInternalName( %this.objectGroup.internalName, MissionGroup, true );
		position = %this.getCreateObjectPosition();
		parentGroup = %addToGroup;
	};
}
//------------------------------------------------------------------------------
//==============================================================================
function Scene::createStatic( %this, %file,%trans ) {
	if ( !$missionRunning )
		return;

	%addToGroup = Scene.getActiveSimGroup();
	
	if (%trans !$= ""){
		%transform = %trans;
		$SceneEd_SkipEditorDrop = true;
	}
	else 
		%transform = %this.getCreateObjectTransform();
		
	%position = getWords(%transform,0,2);
	%rotation = getWords(%transform,3,6);

	
	%objId = new TSStatic() {
		shapeName = %file;
		position = %position;
		rotation = %rotation;
		parentGroup = %addToGroup;
	};
	%objId.setTransform(%transform);
	%this.onObjectCreated( %objId,$SceneEd_SkipEditorDrop );
}
//------------------------------------------------------------------------------
//==============================================================================
function Scene::createPrefab( %this, %file ) {
	if ( !$missionRunning )
		return;

	if( !isObject(%this.objectGroup) )
		Scene.setNewObjectGroup( MissionGroup );
		
	%transform = %this.getCreateObjectTransform();
	%position = getWords(%transform,0,2);
	%rotation = getWords(%transform,3,6);
	%objId = new Prefab() {
		filename = %file;
		position = %position;
		rotation = %rotation;
		parentGroup = %this.objectGroup;
	};


	if( isObject( %objId ) )
		%this.onFinishCreateObject( %objId );

	//%this.onObjectCreated( %objId );
}
//------------------------------------------------------------------------------
//==============================================================================
function Scene::createObject( %this, %cmd ) {
	if ( !$missionRunning )
		return;

	if( !isObject(%this.objectGroup) )
		Scene.setNewObjectGroup( MissionGroup );

	devLog("SkipDrop",$SceneEd_SkipEditorDrop,"Pos",%objId.position);
	pushInstantGroup();
	%objId = eval(%cmd);
	popInstantGroup();

	if( isObject( %objId ) )
		%this.onFinishCreateObject( %objId );

	return %objId;
}
//------------------------------------------------------------------------------
//==============================================================================
function Scene::createMesh( %this, %file ) {
	if ( !$missionRunning )
		return;

	%addToGroup = Scene.getActiveSimGroup();
	%transform = %this.getCreateObjectTransform();
	%position = getWords(%transform,0,2);
	%rotation = getWords(%transform,3,6);
	
		
	%objId = new TSStatic() {
		shapeName = %file;
		position = %position;
		rotation = %rotation;
		parentGroup = %addToGroup;
		internalName = fileBase(%file);
		allowPlayerStep = $Cfg_TSStatic_Default_AllowPlayerStep;
	};
	
	 // Get a TSShapeConstructor for this object (use the ShapeLab
   // utility functions to create one if it does not already exist).
   %shapePath = getObjectShapeFile( %objId );
   %shape = findConstructor( %shapePath );
   if ( !isObject( %shape ) )
      %shape = createConstructor( %shapePath );
   if ( !isObject( %shape ) )
   {
      echo( "Failed to create TSShapeConstructor for " @ %objId.getId() );
    
   }else {
   	if ( %shape.getNodeIndex( "NoWalk" ) !$= "-1" ){
   		info("Node NoWalk found on object:",%objId,"allowPlayerStep set to false");
   		%objId.allowPlayerStep = false;
   	}
   }
   
	%this.onObjectCreated( %objId,$SceneEd_SkipEditorDrop );
}
//------------------------------------------------------------------------------
function genericCreateObject( %class ) {
	if ( !isClass( %class ) ) {
		warn( "createObject( " @ %class @ " ) - Was not a valid class." );
		return;
	}

	%cmd = "return new " @ %class @ "();";
	%obj = Scene.createObject( %cmd );
	// In case the caller wants it.
	return %obj;
}