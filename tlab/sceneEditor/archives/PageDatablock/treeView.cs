//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function SceneDatablockTree::rebuild( %this ) {
	%this.clear();
	SceneEd.populateTrees();
	//%this.open(SceneDatablockSet);
	if (SceneDatablockTree.getItemCount() > 0)
	   %this.buildVisibleTree();
}
//------------------------------------------------------------------------------
//==============================================================================
// Add the datablocks to the treeView 
function SceneEd::populateTrees(%this,%classList) {
	if (DbEd.populatingTree)
		return;
		
   if (!isObject(SceneDatablockSet))
      new SimSet(SceneDatablockSet);

	// Populate datablock tree.
	if( %this.excludeClientOnlyDatablocks )
		%set = DataBlockGroup;
	else
		%set = DataBlockSet;

	SceneDatablockTree.clear();
	DbEd.populatingTree = true;

	foreach( %datablock in %set ) {
		%unlistedFound = false;
		%id = %datablock.getId();

		foreach( %obj in UnlistedDatablocks )
			if( %obj.getId() == %id ) {
				%unlistedFound = true;
				break;
			}

		if( %unlistedFound )
			continue;

		if (DbEd.activeClasses !$= "" && getRecordCount(DbEd.activeClasses) < 2) {
			if (!strFind(DbEd.activeClasses,%datablock.getClassName()))
				continue;
		}

		%this.addExistingItem( %datablock, true );
	}

	SceneDatablockTree.sort( 0, true, false, false );
	// Populate datablock type tree.
	//%classList = DbEd.activeClasses;
	//if (%classList $= "")
	%classList = enumerateConsoleClasses( "SimDatablock" );
	SceneDatablockTree.clear();

	foreach$( %datablockClass in %classList ) {
		if(    !%this.isExcludedDatablockType( %datablockClass )
				 && SceneDatablockTree.findItemByName( %datablockClass ) == 0 )
			SceneDatablockTree.insertItem( 0, %datablockClass );
	}

	SceneDatablockTree.sort( 0, false, false, false );
	DbEd.populatingTree = false;
}
//------------------------------------------------------------------------------
//==============================================================================
//Add an existing datablock to the tree
function SceneEd::addExistingItem( %this, %datablock, %dontSort ) {
	%tree = SceneDatablockTree;
	// Look up class at root level.  Create if needed.
	%class = %datablock.getClassName();
	
	%classSet = SceneDatablockSet.findObjectByInternalName(%class);
	
	if (!isObject(%classSet))
     %classSet = newSimSet("",SceneDatablockSet,%class);
   
   %classSet.add(%datablock);
      
	%parentID = %tree.findItemByName( %class );
   devLog("ParentId",%parentID);
	//if( %parentID == 0 )
		//%parentID = %tree.insertItem( 0, %class );

	// If the datablock is already there, don't
	// do anything.
	%id = %tree.findItemByValue( %datablock.getId());

	if( %id)
		return;

	// It doesn't exist so add it.
	%name = %datablock.getName();

	if( %this.DBPM.isDirty( %datablock ) )
		%name = %name @ " *";

   devLog("Insert items in group",%datablock.getId(),%parentID);
	//%id = SceneDatablockTree.insertItem( %parentID, %name, %datablock.getId() );
	DbEd.dbClassList = strAddWord(DbEd.dbClassList,%class,true);
	DbEd.dbClassItemIds[%class] = strAddWord(DbEd.dbClassItemIds[%class],%id,true);
	DbEd.dbClassItemNames[%class] = strAddWord(DbEd.dbClassItems[%class],%name,true);

	//if( !%dontSort )
		//SceneDatablockTree.sort( %parentID, false, false, false );

	return %id;
}
//------------------------------------------------------------------------------
//==============================================================================
//Check for some datablock exclusions
function SceneEd::isExcludedDatablockType( %this, %className ) {
	switch$( %className ) {
	case "SimDatablock":
		return true;

	case "SFXTrack": // Abstract.
		return true;

	case "SFXFMODEvent": // Internally created.
		return true;

	case "SFXFMODEventGroup": // Internally created.
		return true;
	}

	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
//Check for some datablock exclusions
function markTree( %obj,%marked ) {
	   %item = SceneDatablockTree.findItemByObjectId(%obj);
	   	SceneDatablockTree.markItem(%item,%marked);
}
//------------------------------------------------------------------------------
//==============================================================================
//Check for some datablock exclusions
function itemTree( %obj,%expandId,%collapseId ) {
	   %item = SceneDatablockTree.findItemByObjectId(%obj);
	   SceneDatablockTree.setItemImages(%item,%expandId,%collapseId);
	    
}
//------------------------------------------------------------------------------


//==============================================================================
// SceneEd.buildClassList
function SceneEd::buildClassList(%this) {
	%classList = enumerateConsoleClasses( "SimDatablock" );

	foreach$( %datablockClass in %classList ) {
		DbEd_ActiveClassList.insertItem(%datablockClass,DbEd_ActiveClassList.getCount());
		DbEd.allClasses = strAddWord(DbEd.allClasses,%datablockClass,true);
	}
}
//==============================================================================
//DbEd.selectAllClasses();
function DbEd::selectAllClasses( %this ) {
	DbEd_ActiveClassList.clearSelection();

	for(%i=0; %i<DbEd_ActiveClassList.getItemCount(); %i++) {
		DbEd_ActiveClassList.setSelected(%i,true);
		//DbEd_ActiveClassList.setSelected(3,true);
	}

	DbEd.allClassesSelected = true;
	DbEd.activeClasses = DbEd.allClasses;
}
//------------------------------------------------------------------------------
//==============================================================================
//DbEd.refreshTree();
function DbEd::refreshTree( %this ) {
	if (!DbEd.allClassesSelected) {
		DbEd.activeClasses = "";
		%selected = DbEd_ActiveClassList.getSelectedItems();

		foreach$( %id in %selected ) {
			%text = DbEd_ActiveClassList.getItemText(%id);
			DbEd.activeClasses = strAddWord(DbEd.activeClasses,%text,true);
		}
	}

	SceneEd.populateTrees(DbEd.activeClasses);
}
//------------------------------------------------------------------------------
//==============================================================================
//DbEd.refreshTree();
function DbEd_ActiveClassList::onSelect( %this,%index,%text ) {
	DbEd.allClassesSelected = false;
}
//------------------------------------------------------------------------------
//==============================================================================
//DbEd.refreshTree();
function DbEd_ActiveClassList::onUnselect( %this,%index,%text ) {
	DbEd.allClassesSelected = false;
}
//------------------------------------------------------------------------------
//==============================================================================
// TreeView Events
//==============================================================================

//==============================================================================
function SceneDatablockTree::onDeleteSelection( %this ) {
	%this.undoDeleteList = "";
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneDatablockTree::onDeleteObject( %this, %object ) {
	// Append it to our list.
	%this.undoDeleteList = %this.undoDeleteList TAB %object;
	// We're gonna delete this ourselves in the
	// completion callback.
	return true;
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneDatablockTree::onObjectDeleteCompleted( %this ) {
	//MEDeleteUndoAction::submit( %this.undoDeleteList );
	// Let the world editor know to
	// clear its selection.
	//EWorldEditor.clearSelection();
	//EWorldEditor.isDirty = true;
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneDatablockTree::onClearSelection(%this) {
	Scene.doInspect( 0 );
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneDatablockTree::onAddSelection( %this, %index,%a ) {
   devLog("onAddSelection",%this,%index,%a);
   %obj = %this.getSelectedObject(%index);
    devLog("OBJ",%obj);
   SceneEd.selectDatablock( %index, true, true );
	//Scene.doInspect(%datablock);
}
	

//------------------------------------------------------------------------------
//==============================================================================
function SceneDatablockTree::onRemoveSelection( %this, %id ) {
	%obj = %this.getItemValue( %id );

	if( isObject( %obj ) )
		SceneEd.unselectDatablock( %obj, true );
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneDatablockTree::onRightMouseUp( %this, %id, %mousePos ) {
	%datablock = %this.getItemValue( %id );

	if( !isObject( %datablock ) )
		return;

	if( !isObject( SceneDatablockTreePopup ) )
		new PopupMenu( SceneDatablockTreePopup ) {
		superClass = "MenuBuilder";
		isPopup = true;
		item[ 0 ] = "Delete" TAB "" TAB "SceneEd.selectDatablock( %this.datablockObject ); SceneEd.deleteDatablock( %this.datablockObject );";
		item[ 1 ] = "Jump to Definition in Torsion" TAB "" TAB "EditorOpenDeclarationInTorsion( %this.datablockObject );";
		datablockObject = "";
	};

	SceneDatablockTreePopup.datablockObject = %datablock;

	SceneDatablockTreePopup.showPopup( Canvas );
}
//------------------------------------------------------------------------------
