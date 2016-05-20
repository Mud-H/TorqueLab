//==============================================================================
// TorqueLab -> ShapeLab -> Sequence Editing
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// ShapeLab -> Sequence Editing
//==============================================================================


function ShapeLabPropWindow::onWake( %this ) {
	ShapeLab_TriggerList.triggerId = 1;
	ShapeLab_TriggerList.clear();
	ShapeLab_TriggerList.addRow( -1, "-1" TAB "Frame" TAB "Trigger" TAB "State" );
	ShapeLab_TriggerList.setRowActive( -1, false );
}

function ShapeLabPropWindow::update_onSeqSelectionChanged( %this ) {
	devLog("ShapeLabPropWindow::update_onSeqSelectionChanged REPLACED WITH SetActiveSequence");
	return;
	/*// Sync the Thread window sequence selection
	%row = ShapeLabSequenceList.getSelectedRow();

	if ( ShapeLab_ThreadSeqList.getSelectedRow() != ( %row-1 ) ) {
		ShapeLab_ThreadSeqList.setSelectedRow( %row-1 );
		return;  // selecting a sequence in the Thread window will re-call this function
	}

	//ShapeLabSeqFromMenu.clear();
	ShapeLabSequences-->blendSeq.clear();
	// Clear the trigger list
	ShapeLab_TriggerList.removeAll();
	// Update the active sequence data
	%seqName = ShapeLabSequenceList.getSelectedName();

	if ( %seqName !$= "" ) {
		// Enable delete button and edit boxes
		if ( ShapeLab.currentMainOptionsPage $= "1" )
			ShapeLabPropWindow-->deleteBtn.setActive( true );

		ShapeLabSequences-->seqName.setActive( true );
		ShapeLabSequences-->blendFlag.setActive( true );
		ShapeLabSequences-->cyclicFlag.setActive( true );
		ShapeLabSequences-->priority.setActive( true );
		ShapeLabSequences-->addTriggerBtn.setActive( true );
		ShapeLabSequences-->deleteTriggerBtn.setActive( true );
		// Initialise the sequence properties
		%blendData = ShapeLab.shape.getSequenceBlend( %seqName );
		ShapeLabSequences-->seqName.setText( %seqName );
		ShapeLabSequences-->cyclicFlag.setValue( ShapeLab.shape.getSequenceCyclic( %seqName ) );
		ShapeLabSequences-->blendFlag.setValue( getField( %blendData, 0 ) );
		ShapeLabSequences-->priority.setText( ShapeLab.shape.getSequencePriority( %seqName ) );
		// 'From' and 'Blend' sequence menus
		//ShapeLabSeqFromMenu.add( "Browse..." );
		%count = ShapeLabSequenceList.rowCount();

		for ( %i = 2; %i < %count; %i++ ) { // skip header row and <rootpose>
			%name = ShapeLabSequenceList.getItemName( %i );

			if ( %name !$= %seqName ) {
				//ShapeLabSeqFromMenu.add( %name );
				ShapeLabSequences-->blendSeq.add( %name );
			}
		}

		ShapeLabSequences-->blendSeq.setText( getField( %blendData, 1 ) );
		ShapeLabSequences-->blendFrame.setText( getField( %blendData, 2 ) );
		%this.syncPlaybackDetails();
		// Triggers (must occur after syncPlaybackDetails is called so the slider range is correct)
		%count = ShapeLab.shape.getTriggerCount( %seqName );

		for ( %i = 0; %i < %count; %i++ ) {
			%trigger = ShapeLab.shape.getTrigger( %seqName, %i );
			ShapeLab_TriggerList.addItem( getWord( %trigger, 0 ), getWord( %trigger, 1 ) );
		}
	} else {
		// Disable delete button and edit boxes
		if ( ShapeLab.currentMainOptionsPage $= "1" )
			ShapeLabPropWindow-->deleteBtn.setActive( false );

		ShapeLabSequences-->seqName.setActive( false );
		ShapeLabSequences-->blendFlag.setActive( false );
		ShapeLabSequences-->cyclicFlag.setActive( false );
		ShapeLabSequences-->priority.setActive( false );
		ShapeLabSequences-->addTriggerBtn.setActive( false );
		ShapeLabSequences-->deleteTriggerBtn.setActive( false );
		// Clear sequence properties
		ShapeLabSequences-->seqName.setText( "" );
		ShapeLabSequences-->cyclicFlag.setValue( 0 );
		ShapeLabSequences-->blendSeq.setText( "" );
		ShapeLabSequences-->blendFlag.setValue( 0 );
		ShapeLabSequences-->priority.setText( 0 );
		%this.syncPlaybackDetails();
	}

	%this.onTriggerSelectionChanged();
	

	// ShapeLabSequences-->sequenceListHeader.setExtent( getWord( ShapeLabSequenceList.extent, 0 ) SPC "19" );
	// Reset current frame
	//ShapeLabPreview.setKeyframe( ShapeLabPreview-->seqIn.getText() );
	*/
}

// Update the GUI in response to a sequence being added
function ShapeLabPropWindow::update_onSequenceAdded( %this, %seqName, %oldIndex ) {
	devLog("ShapeLabPropWindow::update_onSequenceAdded REPLACE ME PLEASE");
	// --- MISC ---
	ShapeLabHints.updateHints();
	ShapeLab.addSequencePill(%seqName);
/*
	// --- SEQUENCES TAB ---
	if ( %oldIndex == -1 ) {
		// This is a brand new sequence => add it to the list and make it the
		// current selection
		%row = ShapeLabSequenceList.insertItem( %seqName, ShapeLabSequenceList.rowCount() );
		ShapeLabSequenceList.scrollVisible( %row );
		ShapeLabSequenceList.setSelectedRow( %row );
	} else {
		// This sequence has been un-deleted => add it back to the list at the
		// original position
		ShapeLabSequenceList.insertItem( %seqName, %oldIndex );
	}
	*/
}
/*
function ShapeLabPropWindow::update_onSequenceRemoved( %this, %seqName ) {
	devLog("ShapeLabPropWindow::update_onSequenceRemoved REPLACE ME PLEASE");
	// --- MISC ---
	ShapeLabHints.updateHints();
	// --- SEQUENCES TAB ---
	%isSelected = ( ShapeLabSequenceList.getSelectedName() $= %seqName );
	ShapeLabSequenceList.removeItem( %seqName );

	if ( %isSelected )
		ShapeLabPropWindow.update_onSeqSelectionChanged();

	// --- THREADS WINDOW ---
	ShapeLabShapeView.refreshThreadSequences();
}

function ShapeLabPropWindow::update_onSequenceRenamed( %this, %oldName, %newName ) {
	devLog("ShapeLabPropWindow::update_onSequenceRenamed REPLACE ME PLEASE",%oldName, %newName );
	// --- MISC ---
	ShapeLabHints.updateHints();
	// Rename the proxy sequence as well
	%oldProxy = ShapeLab.getProxyName( %oldName );
	%newProxy = ShapeLab.getProxyName( %newName );

	if ( ShapeLab.shape.getSequenceIndex( %oldProxy ) != -1 )
		ShapeLab.shape.renameSequence( %oldProxy, %newProxy );

	// --- SEQUENCES TAB ---
	ShapeLabSequenceList.editColumn( %oldName, 0, %newName );

	if ( ShapeLabSequenceList.getSelectedName() $= %newName )
		ShapeLabSequences-->seqName.setText( %newName );

	// --- THREADS WINDOW ---
	// Update any threads that use this sequence
	%active = ShapeLabShapeView.activeThread;

	for ( %i = 0; %i < ShapeLabShapeView.getThreadCount(); %i++ ) {
		ShapeLabShapeView.activeThread = %i;

		if ( ShapeLabShapeView.getThreadSequence() $= %oldName )
			ShapeLabShapeView.setThreadSequence( %newName, 0, ShapeLabShapeView.threadPos, 0 );
		else if ( ShapeLabShapeView.getThreadSequence() $= %oldProxy )
			ShapeLabShapeView.setThreadSequence( %newProxy, 0, ShapeLabShapeView.threadPos, 0 );
	}

	ShapeLabShapeView.activeThread = %active;
}

function ShapeLabPropWindow::update_onSequenceCyclicChanged( %this, %seqName, %cyclic ) {
	devLog("ShapeLabPropWindow::update_onSequenceCyclicChanged REPLACE ME PLEASE",%oldName, %newName );
	// --- MISC ---
	// Apply the same transformation to the proxy animation if necessary
	%proxyName = ShapeLab.getProxyName( %seqName );

	if ( ShapeLab.shape.getSequenceIndex( %proxyName ) != -1 )
		ShapeLab.shape.setSequenceCyclic( %proxyName, %cyclic );

	// --- SEQUENCES TAB ---
	ShapeLabSequenceList.editColumn( %seqName, 1, %cyclic ? "yes" : "no" );

	if ( ShapeLabSequenceList.getSelectedName() $= %seqName )
		ShapeLabSequences-->cyclicFlag.setStateOn( %cyclic );
}

function ShapeLabPropWindow::update_onSequenceBlendChanged( %this, %seqName, %blend,
		%oldBlendSeq, %oldBlendFrame, %blendSeq, %blendFrame ) {
			devLog("ShapeLabPropWindow::update_onSequenceBlendChanged REPLACE ME PLEASE",%oldName, %newName );
	// --- MISC ---
	// Apply the same transformation to the proxy animation if necessary
	%proxyName = ShapeLab.getProxyName( %seqName );

	if ( ShapeLab.shape.getSequenceIndex( %proxyName ) != -1 ) {
		if ( %blend && %oldBlend )
			ShapeLab.shape.setSequenceBlend( %proxyName, false, %oldBlendSeq, %oldBlendFrame );

		ShapeLab.shape.setSequenceBlend( %proxyName, %blend, %blendSeq, %blendFrame );
	}

	ShapeLabShapeView.updateNodeTransforms();
	// --- SEQUENCES TAB ---
	ShapeLabSequenceList.editColumn( %seqName, 2, %blend ? "yes" : "no" );

	if ( ShapeLabSequenceList.getSelectedName() $= %seqName ) {
		ShapeLabSequences-->blendFlag.setStateOn( %blend );
		ShapeLabSequences-->blendSeq.setText( %blendSeq );
		ShapeLabSequences-->blendFrame.setText( %blendFrame );
	}
}

function ShapeLabPropWindow::update_onSequencePriorityChanged( %this, %seqName ) {
	devLog("ShapeLabPropWindow::update_onSequencePriorityChanged REPLACE ME PLEASE",%oldName, %newName );
	// --- SEQUENCES TAB ---
	%priority = ShapeLab.shape.getSequencePriority( %seqName );
	ShapeLabSequenceList.editColumn( %seqName, 4, %priority );

	if ( ShapeLabSequenceList.getSelectedName() $= %seqName )
		ShapeLabSequences-->priority.setText( %priority );
}

function ShapeLabPropWindow::update_onSequenceGroundSpeedChanged( %this, %seqName ) {
	devLog("ShapeLabPropWindow::update_onSequenceGroundSpeedChanged REPLACE ME PLEASE",%oldName, %newName );
	// nothing to do yet
}
*/
function ShapeLabPropWindow::syncPlaybackDetails( %this ) {
	devLog("ShapeLabPropWindow::syncPlaybackDetails REPLACE ME PLEASE",%oldName, %newName );
	%seqName = ShapeLabSequenceList.getSelectedName();

	if ( %seqName !$= "" ) {
		// Show sequence in/out bars
		ShapeLabPreview-->seqInBar.setVisible( true );
		ShapeLabPreview-->seqOutBar.setVisible( true );
		// Sync playback controls
		%sourceData = ShapeLab.getSequenceSource( %seqName );
		%seqFrom = rtrim( getFields( %sourceData, 0, 1 ) );
		%seqStart = getField( %sourceData, 2 );
		%seqEnd = getField( %sourceData, 3 );
		%seqFromTotal = getField( %sourceData, 4 );

		// Display the original source for edited sequences
		if ( startswith( %seqFrom, "__backup__" ) ) {
			%backupData = ShapeLab.getSequenceSource( getField( %seqFrom, 0 ) );
			%seqFrom = rtrim( getFields( %backupData, 0, 1 ) );
		}

		//ShapeLabSeqFromMenu.setText( %seqFrom );
		//ShapeLabSeqFromMenu.tooltip = ShapeLabSeqFromMenu.getText();   // use tooltip to show long names
		ShapeLabSequences-->startFrame.setText( %seqStart );
		ShapeLabSequences-->endFrame.setText( %seqEnd );
		%val = ShapeLabPreview_FrameSlider.getValue() / getWord( ShapeLabPreview_FrameSlider.range, 1 );
		ShapeLabPreview_FrameSlider.range = "0" SPC ( %seqFromTotal-1 );
		ShapeLabPreview_FrameSlider.setValue( %val * getWord( ShapeLabPreview_FrameSlider.range, 1 ) );
		//ShapeLabThreadSlider.range = ShapeLabPreview_FrameSlider.range;
		//ShapeLabThreadSlider.setValue( ShapeLabPreview_FrameSlider.value );
		ShapeLabPreview.setSequence( %seqName );
		ShapeLabPreview.setPlaybackLimit( "in", %seqStart );
		ShapeLabPreview.setPlaybackLimit( "out", %seqEnd );
	} else {
		// Hide sequence in/out bars
		ShapeLabPreview-->seqInBar.setVisible( false );
		ShapeLabPreview-->seqOutBar.setVisible( false );
		//ShapeLabSeqFromMenu.setText( "" );
		//ShapeLabSeqFromMenu.tooltip = "";
		ShapeLabSequences-->startFrame.setText( 0 );
		ShapeLabSequences-->endFrame.setText( 0 );
		ShapeLabPreview_FrameSlider.range = "0 1";
		ShapeLabPreview_FrameSlider.setValue( 0 );
		//ShapeLabThreadSlider.range = ShapeLabPreview_FrameSlider.range;
		//ShapeLabThreadSlider.setValue( ShapeLabPreview_FrameSlider.value );
		ShapeLabPreview.setPlaybackLimit( "in", 0 );
		ShapeLabPreview.setPlaybackLimit( "out", 1 );
		ShapeLabPreview.setSequence( "" );
	}
}


/*
function ShapeLabSequences::onAddSequence( %this, %name ) {
	devLog("ShapeLabSequences::onAddSequence REPLACE ME PLEASE",%oldName, %newName );
	if ( %name $= "" )
		%name = ShapeLab.getUniqueName( "sequence", "mySequence" );

	// Use the currently selected sequence as the base
	%from = ShapeLabSequenceList.getSelectedName();
	%row = ShapeLabSequenceList.getSelectedRow();

	if ( ( %row < 2 ) && ( ShapeLabSequenceList.rowCount() > 2 ) )
		%row = 2;

	if ( %from $= "" ) {
		// No sequence selected => open dialog to browse for one
		%startAt = ShapeLab.shape.baseShape; //Start at current loaded shape path
		if (isFile(ShapeLab.currentSeqPath))
		  %startAt = ShapeLab.currentSeqPath;
		getLoadFilename( "Anim Files|*.dae;*.dsq|COLLADA Files|*.dae|DSQ Files|*.dsq|Google Earth Files|*.kmz", %this @ ".onAddSequenceFromBrowse", %startAt );	

		return;
	} else {
		// Add the new sequence
		%start = ShapeLabSequences-->startFrame.getText();
		%end = ShapeLabSequences-->endFrame.getText();
		ShapeLab.doAddSequence( %name, %from, %start, %end );
	}
}

function ShapeLabSequences::onAddSequenceFromBrowse( %this, %path ) {
	devLog("ShapeLabSequences::onAddSequenceFromBrowse REPLACE ME PLEASE",%oldName, %newName );
	// Add a new sequence from the browse path
	%path = makeRelativePath( %path, getMainDotCSDir() );
	ShapeLabFromMenu.lastPath = %path;
	%name = ShapeLab.getUniqueName( "sequence", "mySequence" );
	ShapeLab.doAddSequence( %name, %path, 0, -1 );
}

// Delete the selected sequence
function ShapeLabSequences::onDeleteSequence( %this ) {
	devLog("ShapeLabSequences::onDeleteSequence REPLACE ME PLEASE",%oldName, %newName );
	%row = ShapeLabSequenceList.getSelectedRow();

	if ( %row != -1 ) {
		%seqName = ShapeLabSequenceList.getItemName( %row );
		ShapeLab.doRemoveShapeData( "Sequence", %seqName );
	}
}

// Get the name of the currently selected sequence
function ShapeLabSequenceList::getSelectedName( %this ) {
	%row = %this.getSelectedRow();
	return ( %row > 1 ) ? %this.getItemName( %row ) : "";    // ignore header row
}

// Get the sequence name from the indexed row
function ShapeLabSequenceList::getItemName( %this, %row ) {
	return getField( %this.getRowText( %row ), 0 );
}

// Get the index in the list of the sequence with the given name
function ShapeLabSequenceList::getItemIndex( %this, %name ) {
	for ( %i = 1; %i < %this.rowCount(); %i++ ) { // ignore header row
		if ( %this.getItemName( %i ) $= %name )
			return %i;
	}

	return -1;
}

// Change one of the fields in the sequence list
function ShapeLabSequenceList::editColumn( %this, %name, %col, %text ) {
	%row = %this.getItemIndex( %name );
	%rowText = setField( %this.getRowText( %row ), %col, %text );
	// Update the Properties and Thread sequence lists
	%id = %this.getRowId( %row );

	if ( %col == 0 )
		ShapeLab_ThreadSeqList.setRowById( %id, %text );   // Sync name in Thread window

	%this.setRowById( %id, %rowText );
}

function ShapeLabSequenceList::addItem( %this, %name ) {
	return %this.insertItem( %name, %this.rowCount() );
}

function ShapeLabSequenceList::insertItem( %this, %name, %index ) {
	%cyclic = ShapeLab.shape.getSequenceCyclic( %name ) ? "yes" : "no";
	%blend = getField( ShapeLab.shape.getSequenceBlend( %name ), 0 ) ? "yes" : "no";
	%frameCount = ShapeLab.shape.getSequenceFrameCount( %name );
	%priority = ShapeLab.shape.getSequencePriority( %name );
	// Add the item to the Properties and Thread sequence lists
	%this.seqId++; // use this to keep the row IDs synchronised
	ShapeLab_ThreadSeqList.addRow( %this.seqId, %name, %index-1 );   // no header row
	return %this.addRow( %this.seqId, %name TAB %cyclic TAB %blend TAB %frameCount TAB %priority, %index );
}

function ShapeLabSequenceList::removeItem( %this, %name ) {
	%index = %this.getItemIndex( %name );

	if ( %index >= 0 ) {
		%this.removeRow( %index );
		ShapeLab_ThreadSeqList.removeRow( %index-1 );   // no header row
	}
}
*/
function ShapeLabAnim_SeqPillMenu::onSelect( %this, %id, %text ) {
	if ( %text $= "Browse..." ) {
		// Reset menu text
		%seqName = ShapeLabSequenceList.getSelectedName();
		%seqFrom = rtrim( getFields( ShapeLab.getSequenceSource( %seqName ), 0, 1 ) );
		%this.setText( %seqFrom );
		%startAt = ShapeLab.shape.baseShape; //Start at current loaded shape path
		if (isFile(ShapeLab.currentSeqPath))
		  %startAt = ShapeLab.currentSeqPath;
		getLoadFilename( "Anim Files|*.dae;*.dsq|COLLADA Files|*.dae|DSQ Files|*.dsq|Google Earth Files|*.kmz", %this @ ".onBrowseSelect", %startAt );		
	} else {
		ShapeLabSequences.onEditSequenceSource( %text );
	}
}

function ShapeLabAnim_SeqPillMenu::onBrowseSelect( %this, %path ) {
	%path = makeRelativePath( %path, getMainDotCSDir() );
	%this.lastPath = %path;
	%this.setText( %path );
	ShapeLabSequences.onEditSequenceSource( %path );
}
