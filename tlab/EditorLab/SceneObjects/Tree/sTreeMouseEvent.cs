//==============================================================================
// TorqueLab -> Scene Tree Mouse Events Callbacks
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Tree Items Drag and Dropping
//==============================================================================
//==============================================================================
// Called when an item as been dropped
function SceneObjectsTree::onDragDropped(%this) {
	Scene.setDirty();
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneObjectsTree::isValidDragTarget( %this, %id, %obj ) {
	if( %obj.isMemberOfClass( "Path" ) )
		return EWorldEditor.areAllSelectedObjectsOfType( "Marker" );

	if( %obj.name $= "CameraBookmarks" )
		return EWorldEditor.areAllSelectedObjectsOfType( "CameraBookmark" );
	else
		return ( %obj.getClassName() $= "SimGroup" );
}
//------------------------------------------------------------------------------

//==============================================================================
// Set object selected in scene (Trees and WorldEditor)
//==============================================================================
//==============================================================================
function SceneObjectsTree::onMouseUp( %this,%hitItemId, %mouseClickCount ) {
	devLog("SceneObjectsTree::onMouseUp( %this,%hitItemId, %mouseClickCount )",%hitItemId, %mouseClickCount);
	%obj = %this.getItemValue(%hitItemId);
	

	//if (!isObject(%obj))
		return;
%previous = %this.lastHitItemId;
%this.lastHitItemId = %hitItemId;

	switch$(%obj.getClassName()) {
	case "SimGroup":
		if(%mouseClickCount > 1 && %previous $= %hitItemId) {
			%obj.treeExpanded = !%obj.treeExpanded;
			%this.expandItem(%hitItemId,%obj.treeExpanded);
		}
	}
}
function SceneObjectsTree::onRightMouseUp( %this, %itemId, %mouse, %obj ) {
	devLog("SceneObjectsTree::onRightMouseUp",%itemId, %mouse, %obj);
	%this.showContextMenu(%itemId, %mouse, %obj );
}

//==============================================================================
// Set object selected in scene (Trees and WorldEditor)
//==============================================================================
function SceneObjectsTree::showContextMenu( %this, %itemId, %mouse, %obj ) {
	logd("SceneObjectsTree::showContextMenu( %this, %itemId, %mouse, %obj )",%this, %itemId, %mouse, %obj);
	%haveObjectEntries = false;
	%haveLockAndHideEntries = true;
	delObj(SceneTreePopup);
	%popup = new PopupMenu( SceneTreePopup ) {
		superClass = "MenuBuilder";
		isPopup = "1";
	};
	%popup.object = %obj;
	%id = -1;

	// Open context menu if this is a CameraBookmark
	if( %obj.isMemberOfClass( "CameraBookmark" ) ) {
		%popup.addItem(%id++,"Go To Bookmark" TAB "" TAB "EManageBookmarks.jumpToBookmark( %this.bookmark.getInternalName() );");
		SceneTreePopup.bookmark = %obj;
	}
	// Open context menu if this is set CameraBookmarks group.
	else if( %obj.name $= "CameraBookmarks" ) {
		%popup.addItem(%id++,"Add Camera Bookmark" TAB "" TAB "EManageBookmarks.addCameraBookmarkByGui();");
	}
	// Open context menu if this is a SimGroup
	else if( %obj.isMemberOfClass( "SimGroup" ) ) {	
		%popup.addItem(%id++,"Rename" TAB "" TAB "SceneEditorTree.showItemRenameCtrl( SceneEditorTree.findItemByObjectId( %this.object ) );");
		%popup.addItem(%id++,"Delete" TAB "" TAB "EWorldEditor.deleteMissionObject( %this.object );");
		%popup.addItem(%id++,"Inspect" TAB "" TAB "inspectObject( %this.object );");
		%popup.addItem(%id++,"-");
		//%popup.addItem(%id++,"Toggle Lock Children" TAB "" TAB "EWorldEditor.toggleLockChildren( %this.object );");
		//%popup.addItem(%id++,"Toggle Hide Children" TAB "" TAB "EWorldEditor.toggleHideChildren( %this.object );");
		//%popup.addItem(%id++,"-");
		%popup.addItem(%id++,"Show Childrens" TAB "" TAB "Scene.showGroupChilds( "@%obj@" );");
		%popup.addItem(%id++,"Hide Childrens" TAB "" TAB "Scene.hideGroupChilds( "@%obj@" );");
		%popup.addItem(%id++,"Toggle Children Visibility" TAB "" TAB "Scene.toggleHideGroupChilds( "@%obj@" );");
		%popup.addItem(%id++,"-");
		%popup.addItem(%id++,"Group" TAB "" TAB "Scene.addSimGroup( true );");
		%popup.addItem(%id++,"-");
		%popup.addItem(%id++,"Add New Objects Here" TAB "" TAB "Scene.setNewObjectGroup( %this.object );");
		%popup.addItem(%id++,"Add Children to Selection" TAB "" TAB "EWorldEditor.selectAllObjectsInSet( "@%obj@" , false );");
		%popup.addItem(%id++,"Remove Children from Selection" TAB "" TAB "EWorldEditor.selectAllObjectsInSet( "@%obj@" , true );");
		%popup.addItem(%id++,"-");
		%popup.addItem(%id++,"Lock auto-arrange" TAB "" TAB "SceneEd.toggleAutoArrangeGroupLock("@%obj@");");
		%popup.autoLockId = %id;

		%popup.addItem(%id++,"Check for Light Shapes" TAB "" TAB "checkLightShapes("@%obj@");");
		
		if (%obj.prefabFile !$= "") {
			%popup.addItem(%id++,"Collapse Prefab group" TAB "" TAB "Lab.CollapsePrefab( "@%obj.getId()@");");
		}

		if (%obj.autoArrangeLocked)
			%popup.setItem(%popup.autoLockId,"Unlock auto-arrange","","SceneEd.toggleAutoArrangeGroupLock( %this.object);");
		else
			%popup.setItem(%popup.autoLockId,"Lock auto-arrange","","SceneEd.toggleAutoArrangeGroupLock( %this.object);");

		%hasChildren = %obj.getCount() > 0;
		%popup.enableItem( 10, %hasChildren );
		%popup.enableItem( 11, %hasChildren );
		%haveObjectEntries = true;
		%haveLockAndHideEntries = false;
	}
	// Open generic context menu.
	else {
		%popup.addItem(%id++,"Rename" TAB "" TAB "SceneEditorTree.showItemRenameCtrl( SceneEditorTree.findItemByObjectId( %this.object ) );");
		%popup.addItem(%id++,"Delete" TAB "" TAB "EWorldEditor.deleteMissionObject( %this.object );");
		%popup.addItem(%id++,"Inspect" TAB "" TAB "inspectObject( %this.object );");
		%popup.addItem(%id++,"-");
		%popup.addItem(%id++,"Locked" TAB "" TAB "%this.object.setLocked( !%this.object.locked ); EWorldEditor.syncGui();");
		%popup.addItem(%id++,"Hidden" TAB "" TAB "EWorldEditor.hideObject( %this.object, !%this.object.hidden ); EWorldEditor.syncGui();");
		%popup.addItem(%id++,"-");
		%popup.addItem(%id++,"Group" TAB "" TAB "Scene.addSimGroup( true );");

		if( %obj.isMemberOfClass( "Prefab" ) ) {
			%popup.addItem(%id++, "Expand Prefab to group" TAB "" TAB "devlog(\"Expanding\");Lab.ExpandPrefab( "@%obj.getId()@");");
		}

		%haveObjectEntries = true;
	}
	
	  // Specialized version for ConvexShapes. 
      if( %obj.isMemberOfClass( "ConvexShape" ) )
      {
      	%popup.addItem(%id++,"-");
			%popup.addItem(%id++,"Convert to Zone" TAB "" TAB "EWorldEditor.convertSelectionToPolyhedralObjects( \"Zone\" );");
		%popup.addItem(%id++,"Convert to Portal" TAB "" TAB "EWorldEditor.convertSelectionToPolyhedralObjects( \"Portal\" );");
		%popup.addItem(%id++,"Convert to Occluder" TAB "" TAB "EWorldEditor.convertSelectionToPolyhedralObjects( \"OcclusionVolume\" );");
		%popup.addItem(%id++,"Convert to Sound Space" TAB "" TAB "EWorldEditor.convertSelectionToPolyhedralObjects( \"SFXSpace\" );");
	
      }
      
      // Specialized version for polyhedral objects.
      else if( %obj.isMemberOfClass( "Zone" ) || %obj.isMemberOfClass( "Portal" ) || %obj.isMemberOfClass( "OcclusionVolume" ) || %obj.isMemberOfClass( "SFXSpace" ) ){
      	%popup.addItem(%id++,"-");
			%popup.addItem(%id++,"Convert to ConvexShape" TAB "" TAB "EWorldEditor.convertSelectionToConvexShape();");        
      }


// Handle multi-selection.
	if( %this.getSelectedItemsCount() > 1 ) {
		%popup.addItem(%id++,"Delete Selection" TAB "" TAB "EditorMenuEditDelete();");
		%popup.addItem(%id++,"Group Selection" TAB "" TAB "Scene.addSimGroup( true );");
	}

	if( %haveObjectEntries ) {
		%popup.enableItem( 0, %obj.isNameChangeAllowed() && %obj.getName() !$= "MissionGroup" );
		%popup.enableItem( 1, %obj.getName() !$= "MissionGroup" );

		if( %haveLockAndHideEntries ) {
			%popup.checkItem( 4, %obj.locked );
			%popup.checkItem( 5, %obj.hidden );
		}

		%popup.enableItem( 7, %this.isItemSelected( %itemId ) );
	}

	%popup.showPopup( Canvas );
}
