//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// ShapeLab -> Trigger Editing
//==============================================================================


function ShapeLabPropWindow::onTriggerSelectionChanged( %this ) {
	%row = ShapeLab_TriggerList.getSelectedRow();

	if ( %row > 0 ) { // skip header row
		%text = ShapeLab_TriggerList.getRowText( %row );
		ShapeLabSequences-->triggerFrame.setActive( true );
		ShapeLabSequences-->triggerNum.setActive( true );
		ShapeLabSequences-->triggerOnOff.setActive( true );
		ShapeLabSequences-->triggerFrame.setText( getField( %text, 1 ) );
		ShapeLabSequences-->triggerNum.setText( getField( %text, 2 ) );
		ShapeLabSequences-->triggerOnOff.setValue( getField( %text, 3 ) $= "on" );
	} else {
		// No trigger selected
		ShapeLabSequences-->triggerFrame.setActive( false );
		ShapeLabSequences-->triggerNum.setActive( false );
		ShapeLabSequences-->triggerOnOff.setActive( false );
		ShapeLabSequences-->triggerFrame.setText( "" );
		ShapeLabSequences-->triggerNum.setText( "" );
		ShapeLabSequences-->triggerOnOff.setValue( 0 );
	}
}



function ShapeLabPropWindow::update_onTriggerAdded( %this, %seqName, %frame, %state ) {
	// --- SEQUENCES TAB ---
	// Add trigger to list if this sequence is selected
	if ( ShapeLab.selectedSequence $= %seqName )
		ShapeLab_TriggerList.addItem( %frame, %state );
}

function ShapeLabPropWindow::update_onTriggerRemoved( %this, %seqName, %frame, %state ) {
	// --- SEQUENCES TAB ---
	// Remove trigger from list if this sequence is selected
	if ( ShapeLab.selectedSequence $= %seqName )
		ShapeLab_TriggerList.removeItem( %frame, %state );
}


function ShapeLab::onAddTrigger( %this ) {
	// Can only add triggers if a sequence is selected
	%seqName = ShapeLab.selectedSequence;

	if ( %seqName !$= "" ) {
		%sourceData = ShapeLab.getSequenceSource( %seqName );
	%seqFrom = rtrim( getFields( %sourceData, 0, 1 ) );
	%seqStart = getField( %sourceData, 2 );
	%seqEnd = getField( %sourceData, 3 );
	%seqFromTotal = getField( %sourceData, 4 );
	
		// Add a new trigger at the current frame
		%frame = mRound( ShapeLabSeqSlider.getValue() );
		if (%frame > %seqEnd)
			%frame = %seqEnd;
		if (%frame < %seqStart)
			%frame = %seqStart;
		%state = ShapeLab_TriggerList.rowCount() % 30;
		ShapeLab.doAddTrigger( %seqName, %frame, %state );
	}
}

function ShapeLab::onDeleteTrigger( %this ) {
	// Can only delete a trigger if a sequence and trigger are selected
	%seqName = ShapeLab.selectedSequence;

	if ( %seqName !$= "" ) {
		%row = ShapeLab_TriggerList.getSelectedRow();

		if ( %row > 0 ) {
			%text = ShapeLab_TriggerList.getRowText( %row );
			%frame = getWord( %text, 1 );
			%state = getWord( %text, 2 );
			%state *= ( getWord( %text, 3 ) $= "on" ) ? 1 : -1;
			ShapeLab.doRemoveTrigger( %seqName, %frame, %state );
		}
	}
}

function ShapeLab_TriggerList::onEditSelection( %this ) {
	// Can only edit triggers if a sequence and trigger are selected
	%seqName =  ShapeLab.selectedSequence;

	if ( %seqName !$= "" ) {
		%row = ShapeLab_TriggerList.getSelectedRow();

		if ( %row > 0 ) {
			%text = %this.getRowText( %row );
			%oldFrame = getWord( %text, 1 );
			%oldState = getWord( %text, 2 );
			%oldState *= ( getWord( %text, 3 ) $= "on" ) ? 1 : -1;
			%frame = mRound( ShapeLab_TriggerData-->triggerFrame.getText() );
			%state = mRound( mAbs( ShapeLab_TriggerData-->triggerNum.getText() ) );
			%state *= ShapeLab_TriggerData-->triggerOnOff.getValue() ? 1 : -1;

			if ( ( %frame >= 0 ) && ( %state != 0 ) )
				ShapeLab.doEditTrigger( %seqName, %oldFrame, %oldState, %frame, %state );
		}
	}
}
