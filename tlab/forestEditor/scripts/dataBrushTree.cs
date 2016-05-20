//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function ForestEditBrushTree::initTree( %this ) {
	ForestEditBrushTree.open( ForestBrushGroup );
	ForestManagerBrushTree.open( ForestBrushGroup );
}

function ForestEditorGui::setBrushFilter( %this,%text ) {
	%filtertext = strreplace(%text,"Filter...","");
	ForestEditBrushTree.setFilterText(%filtertext);
	FEP_BrushFilter.setText(%text);
}

$FEP_GlobalScale = 1.5;
function ForestEditorGui::newGroup( %this ) {
	%internalName = getUniqueInternalName( "Group", ForestBrushGroup, true );
	%selected = ForestEditBrushTree.getSelectedObject();

	if (%selected.getClassName() $= "SimGroup")
		%addTo = %selected;
	else if (%selected.parentGroup.getClassName() $= "SimGroup")
		%addTo = %selected.parentGroup;

	%brush = new SimGroup() {
		internalName = %internalName;
		parentGroup = ForestBrushGroup;
	};

	if (isObject(%addTo))
		%addTo.add(%brush);

	MECreateUndoAction::submit( %brush );
	ForestEditBrushTree.open( ForestBrushGroup );
	ForestEditBrushTree.buildVisibleTree(true);
	%item = ForestEditBrushTree.findItemByObjectId( %brush );
	ForestEditBrushTree.clearSelection();
	ForestEditBrushTree.addSelection( %item );
	ForestEditBrushTree.scrollVisible( %item );
	ForestEditorPlugin.dirty = true;
	FEP_Manager.updateBrushData();
}

function ForestEditorGui::newBrush( %this ) {
	%internalName = getUniqueInternalName( "Brush", ForestBrushGroup, true );
	%selected = ForestEditBrushTree.getSelectedObject();

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
	ForestEditBrushTree.open( ForestBrushGroup );
	ForestEditBrushTree.buildVisibleTree(true);
	%item = ForestEditBrushTree.findItemByObjectId( %brush );
	ForestEditBrushTree.clearSelection();
	ForestEditBrushTree.addSelection( %item );
	ForestEditBrushTree.scrollVisible( %item );
	ForestEditorPlugin.dirty = true;
	FEP_Manager.updateBrushData();
}

function ForestEditorGui::newElement( %this ) {
	%sel = ForestEditBrushTree.getSelectedObject();

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
	ForestEditBrushTree.clearSelection();
	ForestEditBrushTree.buildVisibleTree( true );
	%item = ForestEditBrushTree.findItemByObjectId( %element.getId() );
	ForestEditBrushTree.scrollVisible( %item );
	ForestEditBrushTree.addSelection( %item );
	ForestEditorPlugin.dirty = true;
	FEP_Manager.updateBrushData();
}
function ForestEditBrushTree::onRemoveSelection( %this, %obj ) {
	%this.buildVisibleTree( true );
	ForestTools->BrushTool.collectElements();

	if ( %this.getSelectedItemsCount() == 1 )
		ForestEditorInspector.inspect( %obj );
	else
		ForestEditorInspector.inspect( "" );
}

function ForestEditBrushTree::onAddSelection( %this, %obj ) {
	%this.buildVisibleTree( true );
	ForestTools->BrushTool.collectElements();

	if ( %this.getSelectedItemsCount() == 1 )
		ForestEditorInspector.inspect( %obj );
	else
		ForestEditorInspector.inspect( "" );
}
function ForestEditBrushTree::onSelect( %this,%item ) {
	if (%item.getClassName() $= "SimSet") {
		foreach(%obj in %item) {
			%itemId = %this.findItemByObjectId(%obj);
			%this.addSelection(%itemId);
		}
	}
}


function ForestEditBrushTree::onDeleteSelection( %this ) {
	%list = ForestEditBrushTree.getSelectedObjectList();

	foreach(%obj in %list)
		if (%obj.isCoreSet)
			%removeList = strAddWord(%removeList,%obj);

	foreach$(%obj in %removeList)
		%list = strRemoveWord(%obj);

	MEDeleteUndoAction::submit( %list, true );
	ForestEditorPlugin.dirty = true;
}

function ForestEditBrushTree::onDragDropped( %this ) {
	ForestEditorPlugin.dirty = true;
}


function ForestEditBrushTree::handleRenameObject( %this, %name, %obj ) {
	if ( %name !$= "" ) {
		%found = ForestBrushGroup.findObjectByInternalName( %name );

		if ( isObject( %found ) && %found.getId() != %obj.getId() ) {
			LabMsgOK( "Error", "Brush or Element with that name already exists.", "" );
			// true as in, we handled it, don't rename the object.
			return true;
		}
	}

	// Since we aren't showing any groups whens inspecting a ForestBrushGroup
	// we can't push this event off to the inspector to handle.
	//return GuiTreeViewCtrl::handleRenameObject( %this, %name, %obj );
	// The instant group will try to add our
	// UndoAction if we don't disable it.
	pushInstantGroup();
	%nameOrClass = %obj.getName();

	if ( %nameOrClass $= "" )
		%nameOrClass = %obj.getClassname();

	%action = new InspectorFieldUndoAction() {
		actionName = %nameOrClass @ "." @ "internalName" @ " Change";
		objectId = %obj.getId();
		fieldName = "internalName";
		fieldValue = %obj.internalName;
		arrayIndex = 0;
		inspectorGui = "";
	};
	// Restore the instant group.
	popInstantGroup();
	%action.addToManager( Editor.getUndoManager() );
	EWorldEditor.isDirty = true;
	return false;
}