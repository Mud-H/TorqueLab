//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$FEP_ElementProperties = "scaleMin scaleMax scaleExponent elevationMin elevationMax sinkMin sinkMax sinkRadius slopeMin slopeMax rotationRange probability ForestItemData";


//==============================================================================
function FEP_Manager::initBrushData( %this,%reset ) {
	if (isObject(ForestBrushGroup)) {
		if (ForestBrushGroup.isMember(FEP_LevelBrushSet))
			ForestBrushGroup.remove(FEP_LevelBrushSet);

		if (ForestBrushGroup.getCount() <= 0)
			%reset = true;
	}

	if (%reset)
		delObj(ForestBrushGroup);

	if (!isObject(ForestBrushGroup)) {
	    FEP_Manager.noBrushFile = false;
		%brushFile = "art/forest/brushes.cs";
   
		if ( isFile( %brushFile ) )
		   exec( "art/forest/brushes.cs" );
      else
         FEP_Manager.noBrushFile = true;
		   		   
			//createPath( %brushPath );

		// This creates the ForestBrushGroup, all brushes, and elements.
		//exec( "art/forest/brushes.cs" );
	}

	if ( !isObject( ForestBrushGroup ) ) {
		new SimGroup( ForestBrushGroup );
		ForestBrushGroup.internalName = "ForestBrush";
		%this.showError = true;
	}

	MissionCleanup.add(ForestBrushGroup);
	%this.updateBrushData();
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::updateBrushData( %this ) {
	if (isObject(MissionForestBrushGroup)) {
		MissionCleanup.add(MissionForestBrushGroup);

		if (!isObject(FEP_LevelBrushSet))
			$FEP_LevelBrushSet = newSimSet("FEP_LevelBrushSet","","Level Brushes");
		else
			FEP_LevelBrushSet.clear();

		foreach(%obj in MissionForestBrushGroup)
			FEP_LevelBrushSet.add(%obj);

		ForestBrushGroup.add(FEP_LevelBrushSet);
	}

	ForestEditBrushTree.open( ForestBrushGroup );
	ForestEditBrushTree.buildVisibleTree( true );
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::saveBrushData( %this,%forced ) {
	if (!FEP_Manager.dirty && !%forced)
		return;

	if (isObject(MissionForestBrushGroup)) {
		if (ForestBrushGroup.isMember(FEP_LevelBrushSet))
			ForestBrushGroup.remove(FEP_LevelBrushSet);

		MissionForestBrushGroup.save( MissionForestBrushGroup.getFileName() );
	}

	ForestBrushGroup.save( "art/forest/brushes.cs" );

	if (isObject(MissionForestBrushGroup))
		ForestBrushGroup.add( FEP_LevelBrushSet );
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::detachMissionBrushData( %this ) {
	if (isObject(MissionForestBrushGroup))
		MissionCleanup.add(MissionForestBrushGroup);
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::attachMissionBrushData( %this ) {
	if (isObject(MissionForestBrushGroup))
		ForestBrushGroup.add(MissionForestBrushGroup);
}
//------------------------------------------------------------------------------
//==============================================================================
// Brush Tree Functions
//==============================================================================

//==============================================================================
function FEP_Manager::newBrushGroup( %this ) {
	%internalName = getUniqueInternalName( "Group", ForestBrushGroup, true );
	//Add new group to the closest SimGroup if none selected, add to root
	%selected = ForestManagerBrushTree.getSelectedObject();

	if (isObject(%selected)) {
		if (%selected.getClassName() $= "SimGroup")
			%addTo = %selected;
		else if (%selected.parentGroup.getClassName() $= "SimGroup")
			%addTo = %selected.parentGroup;
		//Extra check in case an element inside a brush is selected
		else if (%selected.parentGroup.parentGroup.getClassName() $= "SimGroup")
			%addTo = %selected.parentGroup.parentGroup;
	}

	if (!isObject(%addTo))
		%addTo = ForestBrushGroup;

	%internalName = getUniqueInternalName( %addTo.internalName@"_", ForestBrushGroup, true );
	%brush = new SimGroup() {
		internalName = %internalName;
		parentGroup = %addTo;
	};
	MECreateUndoAction::submit( %brush );
	ForestManagerBrushTree.open( ForestBrushGroup );
	ForestManagerBrushTree.buildVisibleTree(true);
	%item = ForestManagerBrushTree.findItemByObjectId( %brush );
	ForestManagerBrushTree.clearSelection();
	ForestManagerBrushTree.addSelection( %item );
	ForestManagerBrushTree.scrollVisible( %item );
	ForestEditorPlugin.dirty = true;
	FEP_Manager.updateBrushData();
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::newBrush( %this ) {
	%internalName = getUniqueInternalName( "Brush", ForestBrushGroup, true );
	%selected = ForestManagerBrushTree.getSelectedObject();

	if (%selected.getClassName() $= "SimGroup")
		%addTo = %selected;
	else if (%selected.parentGroup.getClassName() $= "SimGroup")
		%addTo = %selected.parentGroup;

	%brush = new ForestBrush() {
		internalName = %internalName;
		parentGroup = %parent;
	};

	if (isObject(%addTo))
		%addTo.add(%brush);

	devLog("Brush:",%brush,"Parent",%brush.parentGroup);
	MECreateUndoAction::submit( %brush );
	ForestManagerBrushTree.open( ForestBrushGroup );
	ForestManagerBrushTree.buildVisibleTree(true);
	%item = ForestManagerBrushTree.findItemByObjectId( %brush );
	ForestManagerBrushTree.clearSelection();
	ForestManagerBrushTree.addSelection( %item );
	ForestManagerBrushTree.scrollVisible( %item );
	ForestEditorPlugin.dirty = true;
	FEP_Manager.initBrushData();
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::newBrushElement( %this ) {
	%sel = ForestManagerBrushTree.getSelectedObject();

	if ( !isObject( %sel ) )
		%parentGroup = ForestBrushGroup;
	else {
		if ( %sel.getClassName() $= "ForestBrushElement" )
			%parentGroup = %sel.parentGroup;
		else
			%parentGroup = %sel;
	}

	%internalName = getUniqueInternalName( "Element", ForestBrushGroup, true );
	%element = new ForestBrushElement() {
		internalName = %internalName;
		parentGroup =  %parentGroup;
	};
	MECreateUndoAction::submit( %element );
	ForestManagerBrushTree.clearSelection();
	ForestManagerBrushTree.buildVisibleTree( true );
	%item = ForestManagerBrushTree.findItemByObjectId( %element.getId() );
	ForestManagerBrushTree.scrollVisible( %item );
	ForestManagerBrushTree.addSelection( %item );
	ForestEditorPlugin.dirty = true;
	FEP_Manager.updateBrushData();
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::deleteSelectedBrush( %this ) {
	%selItemList = ForestManagerBrushTree.getSelectedItemsList();
	%selObjList = ForestManagerBrushTree.getSelectedObjectList();
	devLog("Selected ITEMS:",%selItemList);
	devLog("Selected OBJECTS:",%selObjList);
}
//------------------------------------------------------------------------------
//==============================================================================
// Brush Tree Callbacks
//==============================================================================
//==============================================================================
function ForestManagerBrushTree::onAddGroupSelected( %this,%simGroup ) {
	devLog("ForestManagerBrushTree::onAddGroupSelected( %this,%simGroup )",%this,%simGroup );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onAddMultipleSelectionBegin( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onAddMultipleSelectionBegin( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onAddMultipleSelectionEnd( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onAddMultipleSelectionEnd( %this,%arg1 )",%this,%arg1 );
	FEP_Manager.updateBrushProperties();
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onBeginReparenting( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onBeginReparenting( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onClearSelection( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onClearSelection( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onDeleteObject( %this,%obj ) {
	devLog("ForestManagerBrushTree::onDeleteObject( %this,%obj )",%this,%obj );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onDeleteSelection( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onDeleteSelection( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onDragDropped( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onDragDropped( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onEndReparenting( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onEndReparenting( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onInspect( %this,%item ) {
	devLog("ForestManagerBrushTree::onInspect( %this,%item )",%this,%item );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onKeyDown( %this,%modifier,%keyCode ) {
	devLog("ForestManagerBrushTree::onKeyDown( %this,%modifier,%keyCode )",%this,%modifier,%keyCode );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onMouseDragged( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onMouseDragged( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onObjectDeleteCompleted( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onObjectDeleteCompleted( %this,%arg1 )",%this,%arg1 );
	ForestDataManager.setDirty(ForestBrushGroup);
	ForestDataManager.dirty = true;
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onRemoveSelection( %this,%item ) {
	devLog("ForestManagerBrushTree::onRemoveSelection( %this,%item )",%this,%item );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onReparent( %this,%item,%oldParent,%newParent ) {
	devLog("ForestManagerBrushTree::onReparent( %this,%item,%oldParent,%newParent )",%this,%item,%oldParent,%newParent );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onRightMouseDown( %this,%itemId,%mousePos,%obj ) {
	devLog("ForestManagerBrushTree::onRightMouseDown( %this,%itemId,%mousePos,%obj )",%this,%itemId,%mousePos,%obj );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onRightMouseUp( %this,%itemId,%mousePos,%obj ) {
	devLog("ForestManagerBrushTree::onRightMouseUp( %this,%itemId,%mousePos,%obj )",%this,%itemId,%mousePos,%obj );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onSelect( %this,%item ) {
	devLog("ForestManagerBrushTree::onSelect( %this,%item )",%this,%item );
	FEP_Manager.updateBrushTreeSelection(%item);
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onUnselect( %this,%item ) {
	devLog("ForestManagerBrushTree::onUnselect( %this,%item )",%this,%item );
	FEP_Manager.updateBrushTreeSelection();
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::updateForestBrush( %this,%item ) {
}
//------------------------------------------------------------------------------

function FEP_Manager::updateBrushTreeSelection( %this,%item ) {
	%selectedItem = ForestManagerBrushTree.getSelectedItem();
	%selectedObject = ForestManagerBrushTree.getSelectedObject();
	%selCount = ForestManagerBrushTree.getSelectedItemsCount();
	devLog("Item=",%item,"SelectedItem = ",%selectedItem,"SelectedObj=",%selectedObject);
	FEP_Manager.selectedBrushName = %item.internalName;
	%this.selectedObject = %item;

	if (!isObject(%item)) {
		foreach(%cont in FEP_ManagerBrushProperties)
			%cont.visible = false;

		return;
	}

	%class = %item.getClassName();

	foreach(%cont in FEP_ManagerBrushProperties) {
		if (%cont.internalName $= %class)
			%cont.visible = true;
		else
			%cont.visible = false;
	}

	switch$(%class) {
	case "ForestBrushElement":
		FEP_Manager.updateBrushTreeElement(%item);

	case "ForestBrush":
		FEP_Manager.updateBrushTreeBrush(%item);

	case "SimGroup":
		FEP_Manager.updateBrushTreeGroup(%item);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::updateBrushTreeElement( %this,%item ) {
	foreach$(%field in $FEP_ElementProperties)
		eval("FEP_ManagerBrushProperties-->"@%field@".setText(%item."@%field@");");
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::updateBrushTreeBrush( %this,%item ) {
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::updateBrushTreeGroup( %this,%item ) {
}
//------------------------------------------------------------------------------
function FEP_ManagerElementEdit::onValidate( %this ) {
	%element = FEP_Manager.selectedObject;

	if (%element.getClassName() !$= "ForestBrushElement")
		return;

	%value = %this.getValue();
	eval("%element."@%this.internalName@" = %value;");
	FEP_Manager.setDirty(true);
}
/*

void 	onInspect (int itemOrObjectId)
void 	onKeyDown (int modifier, int keyCode)
void 	onMouseDragged ()
void 	onMouseUp (int hitItemId, int mouseClickCount)
void 	onObjectDeleteCompleted ()
void 	onRemoveSelection (int itemOrObjectId)
void 	onReparent (int itemOrObjectId, int oldParentItemOrObjectId, int newParentItemOrObjectId)
void 	onRightMouseDown (int itemId, Point2I mousePos, SimObject object)
void 	onRightMouseUp (int itemId, Point2I mousePos, SimObject object)
void 	onSelect (int itemOrObjectId)
void 	onUnselect (int itemOrObjectId)

	onAddGroupSelected (SimGroup group)
void 	onAddMultipleSelectionBegin ()
void 	onAddMultipleSelectionEnd ()
void 	onAddSelection (int itemOrObjectId, bool isLastSelection)
void 	onBeginReparenting ()
void 	onClearSelection ()
void 	onDefineIcons ()
bool 	onDeleteObject (SimObject object)
void 	onDeleteSelection ()
void 	onDragDropped ()
void 	onEndReparenting ()
*/