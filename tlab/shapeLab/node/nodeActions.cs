//==============================================================================
// TorqueLab -> ShapeLab -> Node Editing
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// This file implements all of the actions that can be applied by the GUI. Each
// action has 3 methods:
//
//    doit: called the first time the action is performed
//    undo: called to undo the action
//    redo: called to redo the action (usually the same as doit)
//
// In each case, the appropriate change is made to the shape, and the GUI updated.
//
// TSShapeConstructor keeps track of all the changes made and provides a simple
// way to save the modifications back out to a script file.
//==============================================================================
//------------------------------------------------------------------------------
// Add node
function ShapeLab::doAddNode( %this, %nodeName, %parentName, %transform ) {
	%action = %this.createAction( ActionAddNode, "Add node" );
	%action.nodeName = %nodeName;
	%action.parentName = %parentName;
	%action.transform = %transform;
	%this.doAction( %action );
}

function ActionAddNode::doit( %this ) {
	if ( ShapeLab.shape.addNode( %this.nodeName, %this.parentName, %this.transform ) ) {
		ShapeLab.onNodeAdded( %this.nodeName, -1 );
		return true;
	}

	return false;
}

function ActionAddNode::undo( %this ) {
	Parent::undo( %this );

	if ( ShapeLab.shape.removeNode( %this.nodeName ) )
		ShapeLab.onNodeRemoved( %this.nodeName, 1 );
}

//------------------------------------------------------------------------------
// Remove node
function ShapeLab::doRemoveNode( %this, %nodeName ) {
	%action = %this.createAction( ActionRemoveNode, "Remove node" );
	%action.nodeName =%nodeName;
	%action.nodeChildIndex = ShapeLab_NodeTree.getChildIndexByName( %nodeName );
	// Need to delete all child nodes of this node as well, so recursively collect
	// all of the names.
	%action.nameList = %this.getNodeNames( %nodeName, "" );
	%action.nameCount = getFieldCount( %action.nameList );

	for ( %i = 0; %i < %action.nameCount; %i++ )
		%action.names[%i] = getField( %action.nameList, %i );

	%this.doAction( %action );
}

function ActionRemoveNode::doit( %this ) {
	for ( %i = 0; %i < %this.nameCount; %i++ )
		ShapeLab.shape.removeNode( %this.names[%i] );

	// Update GUI
	ShapeLab.onNodeRemoved( %this.nameList, %this.nameCount );
	return true;
}

function ActionRemoveNode::undo( %this ) {
	Parent::undo( %this );
}

//------------------------------------------------------------------------------
// Rename node
function ShapeLab::doRenameNode( %this, %oldName, %newName ) {
	%action = %this.createAction( ActionRenameNode, "Rename node" );
	%action.oldName = %oldName;
	%action.newName = %newName;
	%this.doAction( %action );
}

function ActionRenameNode::doit( %this ) {
	if ( ShapeLab.shape.renameNode( %this.oldName, %this.newName ) ) {
		ShapeLab.onNodeRenamed( %this.oldName, %this.newName );
		return true;
	}

	return false;
}

function ActionRenameNode::undo( %this ) {
	Parent::undo( %this );

	if ( ShapeLab.shape.renameNode( %this.newName, %this.oldName ) )
		ShapeLab.onNodeRenamed( %this.newName, %this.oldName );
}

//------------------------------------------------------------------------------
// Set node parent
function ShapeLab::doSetNodeParent( %this, %name, %parent ) {
	if ( %parent $= "<root>" )
		%parent = "";

	%action = %this.createAction( ActionSetNodeParent, "Set parent node" );
	%action.nodeName = %name;
	%action.parentName = %parent;
	%action.oldParentName = ShapeLab.shape.getNodeParentName( %name );
	%this.doAction( %action );
}

function ActionSetNodeParent::doit( %this ) {
	if ( ShapeLab.shape.setNodeParent( %this.nodeName, %this.parentName ) ) {
		ShapeLab.onNodeParentChanged( %this.nodeName );
		return true;
	}

	return false;
}

function ActionSetNodeParent::undo( %this ) {
	Parent::undo( %this );

	if ( ShapeLab.shape.setNodeParent( %this.nodeName, %this.oldParentName ) )
		ShapeLab.onNodeParentChanged( %this.nodeName );
}

//------------------------------------------------------------------------------
// Edit node transform
function ShapeLab::doEditNodeTransform( %this, %nodeName, %newTransform, %isWorld, %gizmoID ) {
	// If dragging the 3D gizmo, combine all movement into a single action. Undoing
	// that action will return the node to where it was when the gizmo drag started.
	%last = ShapeLabUndoManager.getUndoAction( ShapeLabUndoManager.getUndoCount() - 1 );

	if ( ( %last != -1 ) && ( %last.class $= ActionEditNodeTransform ) &&
			( %last.nodeName $= %nodeName ) && ( %last.gizmoID != -1 ) && ( %last.gizmoID == %gizmoID ) ) {
		// Use the last action to do the edit, and modify it so it only applies
		// the latest transform
		%last.newTransform = %newTransform;
		%last.isWorld = %isWorld;
		%last.doit();
		ShapeLab.setDirty( true );
	} else {
		%action = %this.createAction( ActionEditNodeTransform, "Edit node transform" );
		%action.nodeName = %nodeName;
		%action.newTransform = %newTransform;
		%action.isWorld = %isWorld;
		%action.gizmoID = %gizmoID;
		%action.oldTransform = %this.shape.getNodeTransform( %nodeName, %isWorld );
		%this.doAction( %action );
	}
}

function ActionEditNodeTransform::doit( %this ) {
	ShapeLab.shape.setNodeTransform( %this.nodeName, %this.newTransform, %this.isWorld );
	ShapeLab.onNodeTransformChanged();
	return true;
}

function ActionEditNodeTransform::undo( %this ) {
	Parent::undo( %this );
	ShapeLab.shape.setNodeTransform( %this.nodeName, %this.oldTransform, %this.isWorld );
	ShapeLab.onNodeTransformChanged();
}

