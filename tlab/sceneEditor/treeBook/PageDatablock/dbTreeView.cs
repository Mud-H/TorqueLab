//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function SceneDatablockTree::onAddSelection( %this, %index,%a ) {
   log("onAddSelection",%this,%index,%a);
   if ($SceneEd_DatablockTreeSetMode)
      %obj = %index;
   else
      %obj = %this.getItemValue(%index);   
   
   SceneEd.selectDatablock( %obj, true, true );
}
	

//------------------------------------------------------------------------------

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
//Check for some datablock exclusions
function markTree( %obj,%marked ) {
	   %item = SceneDatablockTree.findItemByObjectId(%obj.getId());
	   	SceneDatablockTree.markItem(%item,%marked);
}
//------------------------------------------------------------------------------
//==============================================================================
//Check for some datablock exclusions
function itemTree( %obj,%expandId,%collapseId ) {
	   %item = SceneDatablockTree.findItemByObjectId(%obj.getId());
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
