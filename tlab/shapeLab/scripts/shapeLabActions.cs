//==============================================================================
// TorqueLab -> TSShapeConstructor Actions Managers
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// The TSShapeConstructor object allows you to apply a set of transformations
// to a space shape after it is loaded by Torque, but _before_ the shape is used
// by any other object (eg. Player, StaticShape etc). The sort of transformations
// available include adding, renaming and removing nodes and sequences. This GUI
// is a visual wrapper around TSShapeConstructor which allows you to build up the
// transformation set without having to get your hands dirty with TorqueScript.
//
// Removing a node, sequence, mesh or detail poses a problem. These operations
// permanently delete a potentially large amount of data scattered throughout
// the shape, and there is no easy way to restore it if the user 'undoes' the
// delete. Although it is possible to store the deleted data somewhere and restore
// it on undo, it is not easy to get right, and ugly as hell to implement. For
// example, removing a node would require storing the node name, the
// translation/rotation/scale matters bit for each sequence, all node transform
// keyframes, the IDs of any objects that were attached to the node, skin weights
// etc, then restoring all that data into the original place on undo. Frankly,
// TSShape was never designed to be modified dynamically like that.
//
// So......currently we wimp out completely and just don't support undo for those
// remove operations. Lame, I know, but the best I can do for now.
//
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

//==============================================================================
// The ShapeLab uses its own UndoManager


function ShapeLabUndoManager::onUndo( %this ) {
}

function ShapeLabUndoManager::onRedo( %this ) {
}

function ShapeLabUndoManager::onAddUndo( %this ) {
	devLog("ShapeLabUndoManager::onAddUndo",%this,"NextUndo",%this.getNextUndoName(),"Count",%this.getUndoCount());
	
}

function ShapeLabUndoManager::onRemoveUndo( %this ) {
	devLog("ShapeLabUndoManager::onRemoveUndo",%this,"NextUndo",%this.getNextUndoName(),"Count",%this.getUndoCount());
}

function ShapeLabUndoManager::onClear( %this ) {
}

function ShapeLabUndoManager::updateUndoMenu( %this, %editMenu ) {
	Lab.updateUndoMenu();
}

//------------------------------------------------------------------------------
// Helper functions for creating and applying GUI operations

function ShapeLab::createAction( %this, %class, %desc ) {
	pushInstantGroup();
	%action = new UndoScriptAction() {
		class = %class;
		superClass = BaseShapeLabAction;
		actionName = %desc;
		done = 0;
	};
	popInstantGroup();
	return %action;
}

function ShapeLab::doAction( %this, %action ) {
	if ( %action.doit() ) {
		ShapeLab.setDirty( true );
		%action.addToManager( ShapeLabUndoManager );
	} else {
		LabMsgOK( "Error", %action.actionName SPC "failed. Check the console for error messages.", "" );
	}
}

function BaseShapeLabAction::redo( %this ) {
	// Default redo action is the same as the doit action
	if ( %this.doit() ) {
		ShapeLab.setDirty( true );
	} else {
		LabMsgOK( "Error", "Redo" SPC %this.actionName SPC "failed. Check the console for error messages.", "" );
	}
}

function BaseShapeLabAction::undo( %this ) {
	ShapeLab.setDirty( true );
}

//------------------------------------------------------------------------------

function ShapeLab::doRemoveShapeData( %this, %type, %name ) {
	// Removing data from the shape cannot be undone => so warn the user first
	LabMsgYesNo( "Warning", "Deleting a " @ %type @ " cannot be undone. Do " @
					 "you want to continue?", "ShapeLab.doRemove" @ %type @ "( \"" @ %name @ "\" );", "" );
}



//------------------------------------------------------------------------------
// Update bounds
function ShapeLab::doSetBounds( %this ) {
	%action = %this.createAction( ActionSetBounds, "Set bounds" );
	%action.oldBounds = ShapeLab.shape.getBounds();
	%action.newBounds = ShapeLabShapeView.computeShapeBounds();
	%this.doAction( %action );
}

function ActionSetBounds::doit( %this ) {
	return ShapeLab.shape.setBounds( %this.newBounds );
}

function ActionSetBounds::undo( %this ) {
	Parent::undo( %this );
	ShapeLab.shape.setBounds( %this.oldBounds );
}
