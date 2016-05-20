//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Add sequence
//==============================================================================
function ShapeLab::doAddSequence( %this, %seqName, %from, %start, %end ) {
	%action = %this.createAction( ActionAddSequence, "Add sequence" );
	%action.seqName = %seqName;
	%action.origFrom = %from;
	%action.from = %from;
	%action.start = %start;
	%action.end = %end;
	%this.doAction( %action );
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionAddSequence::doit( %this ) {
	// If adding this sequence from an existing sequence, make a backup copy of
	// the existing sequence first, so we can edit the start/end frames later
	// without having to worry if the original source sequence has changed.
	if ( ShapeLab.shape.getSequenceIndex( %this.from ) >= 0 ) {
		%this.from = ShapeLab.getUniqueName( "sequence", "__backup__" @ %this.origFrom @ "_" );
		ShapeLab.shape.addSequence( %this.origFrom, %this.from );
	}

	// Add the sequence
	$collada::forceLoadDAE = Lab.forceLoadDAE;
	%success = ShapeLab.shape.addSequence( %this.from, %this.seqName, %this.start, %this.end );
	$collada::forceLoadDAE = false;

	if ( %success ) {
		ShapeLabPropWindow.update_onSequenceAdded( %this.seqName, -1 );
		return true;
	}

	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionAddSequence::undo( %this ) {
	Parent::undo( %this );

	// Remove the backup sequence if one was created
	if ( %this.origFrom !$= %this.from ) {
		ShapeLab.shape.removeSequence( %this.from );
		%this.from = %this.origFrom;
	}

	// Remove the actual sequence
	if ( ShapeLab.shape.removeSequence( %this.seqName ) )
		ShapeLab.update_onSequenceRemoved( %this.seqName );
}
//------------------------------------------------------------------------------

//==============================================================================
// Remove sequence
//==============================================================================
function ShapeLab::doRemoveSequence( %this, %seqName ) {
	%action = %this.createAction( ActionRemoveSequence, "Remove sequence" );
	%action.seqName = %seqName;
	%this.doAction( %action );
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionRemoveSequence::doit( %this ) {
	if ( ShapeLab.shape.removeSequence( %this.seqName ) ) {
		ShapeLab.update_onSequenceRemoved( %this.seqName );
		return true;
	}

	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionRemoveSequence::undo( %this ) {
	Parent::undo( %this );
}
//------------------------------------------------------------------------------
//==============================================================================
// Rename sequence
//==============================================================================
//==============================================================================
function ShapeLab::doRenameSequence( %this, %oldName, %newName ) {
	%action = %this.createAction( ActionRenameSequence, "Rename sequence" );
	devLog("doRenameSequence OLD",%oldName,"NEW",%newName);
	%action.oldName = %oldName;
	%action.newName = %newName;
	%this.doAction( %action );
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionRenameSequence::doit( %this ) {
	if ( ShapeLab.shape.renameSequence( %this.oldName, %this.newName ) ) {
		ShapeLab.update_onSequenceRenamed( %this.oldName, %this.newName );
		return true;
	}

	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionRenameSequence::undo( %this ) {
	Parent::undo( %this );

	if ( ShapeLab.shape.renameSequence( %this.newName, %this.oldName ) )
		ShapeLab.update_onSequenceRenamed( %this.newName, %this.oldName );
}
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
//Edit sequence source data
//==============================================================================

//==============================================================================
// Edit sequence source data ( parent, start or end )
function ShapeLab::doEditSeqSource( %this, %seqName, %from, %start, %end ) {
	%action = %this.createAction( ActionEditSeqSource, "Edit sequence source data" );
	%action.seqName = %seqName;
	%action.origFrom = %from;
	%action.from = %from;
	%action.start = %start;
	%action.end = %end;
	// To support undo, the sequence will be renamed instead of removed (undo just
	// removes the added sequence and renames the original back). Generate a unique
	// name for the backed up sequence
	%action.seqBackup = ShapeLab.getUniqueName( "sequence", "__backup__" @ %action.seqName @  "_" );

	// If editing an internal sequence, the source is the renamed backup
	if ( %action.from $= %action.seqName )
		%action.from = %action.seqBackup;

	%this.doAction( %action );
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionEditSeqSource::doit( %this ) {
	// If changing the source to an existing sequence, make a backup copy of
	// the existing sequence first, so we can edit the start/end frames later
	// without having to worry if the original source sequence has changed.
	if ( !startswith( %this.from, "__backup__" ) &&
			ShapeLab.shape.getSequenceIndex( %this.from ) >= 0 ) {
		%this.from = ShapeLab.getUniqueName( "sequence", "__backup__" @ %this.origFrom @ "_" );
		ShapeLab.shape.addSequence( %this.origFrom, %this.from );
	}

	// Get settings we want to retain
	%priority = ShapeLab.shape.getSequencePriority( %this.seqName );
	%cyclic = ShapeLab.shape.getSequenceCyclic( %this.seqName );
	%blend = ShapeLab.shape.getSequenceBlend( %this.seqName );
	// Rename this sequence (instead of removing it) so we can undo this action
	ShapeLab.shape.renameSequence( %this.seqName, %this.seqBackup );

	// Add the new sequence
	if ( ShapeLab.shape.addSequence( %this.from, %this.seqName, %this.start, %this.end ) ) {
		// Restore original settings
		if ( ShapeLab.shape.getSequencePriority ( %this.seqName ) != %priority )
			ShapeLab.shape.setSequencePriority( %this.seqName, %priority );

		if ( ShapeLab.shape.getSequenceCyclic( %this.seqName ) != %cyclic )
			ShapeLab.shape.setSequenceCyclic( %this.seqName, %cyclic );

		%newBlend = ShapeLab.shape.getSequenceBlend( %this.seqName );

		if ( %newBlend !$= %blend ) {
			// Undo current blend, then apply new one
			ShapeLab.shape.setSequenceBlend( %this.seqName, 0, getField( %newBlend, 1 ), getField( %newBlend, 2 ) );

			if ( getField( %blend, 0 ) == 1 )
				ShapeLab.shape.setSequenceBlend( %this.seqName, getField( %blend, 0 ), getField( %blend, 1 ), getField( %blend, 2 ) );
		}

		if ( ShapeLab.selectedSequence $= %this.seqName ) {
			ShapeLab.update_onSequenceSourceChanged( %this.seqName, %this.start, %this.end  );
			//ShapeLabSequenceList.editColumn( %this.seqName, 3, %this.end - %this.start + 1 );
			ShapeLabThreadViewer.syncPlaybackDetails();
		}

		return true;
	}

	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionEditSeqSource::undo( %this ) {
	Parent::undo( %this );

	// Remove the source sequence backup if one was created
	if ( ( %this.from !$= %this.origFrom ) && ( %this.from !$= %this.seqBackup ) ) {
		ShapeLab.shape.removeSequence( %this.from );
		%this.from = %this.origFrom;
	}

	// Remove the added sequence, and rename the backup back
	if ( ShapeLab.shape.removeSequence( %this.seqName ) &&
			ShapeLab.shape.renameSequence( %this.seqBackup, %this.seqName ) ) {
		if ( ShapeLabSequenceList.getSelectedName() $= %this.seqName ) {
			ShapeLab.update_onSequenceSourceChanged( %this.seqName, %this.start, %this.end  );
			//ShapeLabSequenceList.editColumn( %this.seqName, 3, %this.end - %this.start + 1 );
			ShapeLabThreadViewer.syncPlaybackDetails();
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Edit cyclic flag
//==============================================================================

//==============================================================================
function ShapeLab::doEditCyclic( %this, %seqName, %cyclic ) {
	if (%seqName $= "")
		return;
	%action = %this.createAction( ActionEditCyclic, "Toggle cyclic flag" );
	%action.seqName = %seqName;
	%action.cyclic = %cyclic;
	%this.doAction( %action );
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionEditCyclic::doit( %this ) {
	if ( ShapeLab.shape.setSequenceCyclic( %this.seqName, %this.cyclic ) ) {
		ShapeLab.update_onSequenceCyclicChanged( %this.seqName, %this.cyclic );
		return true;
	}

	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionEditCyclic::undo( %this ) {
	Parent::undo( %this );

	if ( ShapeLab.shape.setSequenceCyclic( %this.seqName, !%this.cyclic ) )
		ShapeLab.update_onSequenceCyclicChanged( %this.seqName, !%this.cyclic );
}
//------------------------------------------------------------------------------
//==============================================================================
// Edit blend properties
//==============================================================================

//==============================================================================
function ShapeLab::doEditBlend( %this, %seqName, %blend, %blendSeq, %blendFrame ) {
	%action = %this.createAction( ActionEditBlend, "Edit blend properties" );
	%action.seqName = %seqName;
	%action.blend = %blend;
	%action.blendSeq = %blendSeq;
	%action.blendFrame = %blendFrame;
	// Store the current blend settings
	%oldBlend = ShapeLab.shape.getSequenceBlend( %seqName );
	%action.oldBlend = getField( %oldBlend, 0 );
	%action.oldBlendSeq = getField( %oldBlend, 1 );
	%action.oldBlendFrame = getField( %oldBlend, 2 );

	// Use new values if the old ones do not exist ( for blend sequences embedded
	// in the DTS/DSQ file )
	if ( %action.oldBlendSeq $= "" )
		%action.oldBlendSeq = %action.blendSeq;

	if ( %action.oldBlendFrame $= "" )
		%action.oldBlendFrame = %action.blendFrame;

	%this.doAction( %action );
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionEditBlend::doit( %this ) {
	// If we are changing the blend reference ( rather than just toggling the flag )
	// we need to undo the current blend first.
	if ( %this.blend && %this.oldBlend ) {
		if ( !ShapeLab.shape.setSequenceBlend( %this.seqName, false, %this.oldBlendSeq, %this.oldBlendFrame ) )
			return false;
	}

	if ( ShapeLab.shape.setSequenceBlend( %this.seqName, %this.blend, %this.blendSeq, %this.blendFrame ) ) {
		ShapeLab.update_onSequenceBlendChanged( %this.seqName, %this.blend,
				%this.oldBlendSeq, %this.oldBlendFrame, %this.blendSeq, %this.blendFrame );
		return true;
	}

	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionEditBlend::undo( %this ) {
	Parent::undo( %this );

	// If we are changing the blend reference ( rather than just toggling the flag )
	// we need to undo the current blend first.
	if ( %this.blend && %this.oldBlend ) {
		if ( !ShapeLab.shape.setSequenceBlend( %this.seqName, false, %this.blendSeq, %this.blendFrame ) )
			return;
	}

	if ( ShapeLab.shape.setSequenceBlend( %this.seqName, %this.oldBlend, %this.oldBlendSeq, %this.oldBlendFrame ) ) {
		ShapeLab.update_onSequenceBlendChanged( %this.seqName, !%this.blend,
				%this.blendSeq, %this.blendFrame, %this.oldBlendSeq, %this.oldBlendFrame );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Edit sequence priority
//==============================================================================
//==============================================================================
function ShapeLab::doEditSequencePriority( %this, %seqName, %newPriority ) {
	%action = %this.createAction( ActionEditSequencePriority, "Edit sequence priority" );
	%action.seqName = %seqName;
	%action.newPriority = %newPriority;
	%action.oldPriority = %this.shape.getSequencePriority( %seqName );
	%this.doAction( %action );
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionEditSequencePriority::doit( %this ) {
	if ( ShapeLab.shape.setSequencePriority( %this.seqName, %this.newPriority ) ) {
		ShapeLab.update_onSequencePriorityChanged( %this.seqName );
		return true;
	}

	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionEditSequencePriority::undo( %this ) {
	Parent::undo( %this );

	if ( ShapeLab.shape.setSequencePriority( %this.seqName, %this.oldPriority ) )
		ShapeLab.update_onSequencePriorityChanged( %this.seqName );
}
//------------------------------------------------------------------------------
//==============================================================================
// Edit sequence ground speed
//==============================================================================

//==============================================================================
function ShapeLab::doEditSequenceGroundSpeed( %this, %seqName, %newSpeed ) {
	%action = %this.createAction( ActionEditSequenceGroundSpeed, "Edit sequence ground speed" );
	%action.seqName = %seqName;
	%action.newSpeed = %newSpeed;
	%action.oldSpeed = %this.shape.getSequenceGroundSpeed( %seqName );
	%this.doAction( %action );
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionEditSequenceGroundSpeed::doit( %this ) {
	if ( ShapeLab.shape.setSequenceGroundSpeed( %this.seqName, %this.newSpeed ) ) {
		ShapeLab.update_onSequenceGroundSpeedChanged( %this.seqName );
		return true;
	}

	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionEditSequenceGroundSpeed::undo( %this ) {
	Parent::undo( %this );

	if ( ShapeLab.shape.setSequenceGroundSpeed( %this.seqName, %this.oldSpeed ) )
		ShapeLab.update_onSequenceGroundSpeedChanged( %this.seqName );
}

//==============================================================================
// Add Trigger
//==============================================================================
//==============================================================================
// Add trigger
function ShapeLab::doAddTrigger( %this, %seqName, %frame, %state ) {
	%action = %this.createAction( ActionAddTrigger, "Add trigger" );
	%action.seqName = %seqName;
	%action.frame = %frame;
	%action.state = %state;
	devLog("AddTrigger",%seqName,%frame, %state);
	%this.doAction( %action );
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionAddTrigger::doit( %this ) {
	if ( ShapeLab.shape.addTrigger( %this.seqName, %this.frame, %this.state ) ) {
		ShapeLabPropWindow.update_onTriggerAdded( %this.seqName, %this.frame, %this.state );
		return true;
	}

	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionAddTrigger::undo( %this ) {
	Parent::undo( %this );

	if ( ShapeLab.shape.removeTrigger( %this.seqName, %this.frame, %this.state ) )
		ShapeLabPropWindow.update_onTriggerRemoved( %this.seqName, %this.frame, %this.state );
}
//------------------------------------------------------------------------------

//==============================================================================
// Remove Trigger
//==============================================================================

//==============================================================================
// Remove trigger
function ShapeLab::doRemoveTrigger( %this, %seqName, %frame, %state ) {
	%action = %this.createAction( ActionRemoveTrigger, "Remove trigger" );
	%action.seqName = %seqName;
	%action.frame = %frame;
	%action.state = %state;
	%this.doAction( %action );
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionRemoveTrigger::doit( %this ) {
	if ( ShapeLab.shape.removeTrigger( %this.seqName, %this.frame, %this.state ) ) {
		ShapeLabPropWindow.update_onTriggerRemoved( %this.seqName, %this.frame, %this.state );
		return true;
	}

	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionRemoveTrigger::undo( %this ) {
	Parent::undo( %this );

	if ( ShapeLab.shape.addTrigger( %this.seqName, %this.frame, %this.state ) )
		ShapeLabPropWindow.update_onTriggerAdded( %this.seqName, %this.frame, %this.state );
}
//------------------------------------------------------------------------------

//==============================================================================
// Edit Trigger
//==============================================================================

//==============================================================================
// Edit trigger
function ShapeLab::doEditTrigger( %this, %seqName, %oldFrame, %oldState, %frame, %state ) {
	%action = %this.createAction( ActionEditTrigger, "Edit trigger" );
	%action.seqName = %seqName;
	%action.oldFrame = %oldFrame;
	%action.oldState = %oldState;
	%action.frame = %frame;
	%action.state = %state;
	%this.doAction( %action );
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionEditTrigger::doit( %this ) {
	if ( ShapeLab.shape.addTrigger( %this.seqName, %this.frame, %this.state ) &&	ShapeLab.shape.removeTrigger( %this.seqName, %this.oldFrame, %this.oldState ) ) 
	{
		ShapeLab_TriggerList.updateItem( %this.oldFrame, %this.oldState, %this.frame, %this.state );
		return true;
	}

	return false;
}
//------------------------------------------------------------------------------
//==============================================================================
function ActionEditTrigger::undo( %this ) {
	Parent::undo( %this );

	if ( ShapeLab.shape.addTrigger( %this.seqName, %this.oldFrame, %this.oldState ) && ShapeLab.shape.removeTrigger( %this.seqName, %this.frame, %this.state ) )
		ShapeLab_TriggerList.updateItem( %this.frame, %this.state, %this.oldFrame, %this.oldState );
}
//------------------------------------------------------------------------------
