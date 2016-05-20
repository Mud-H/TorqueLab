//==============================================================================
// TorqueLab -> ShapeLab -> Sequence Editing
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab::validateSequenceName( %this,%seqName ) {
	for(%i=0;%i<ShapeLab.shape.getSequenceCount();%i++)
	{
		%name = ShapeLab.shape.getSequenceName(%i);
		devLog("Sequence Index",%i,"Name",%name,"Check",%seqName);
		if (%name $= %seqName)
		{
			devLog("Sequence is valid",%seqName);
			return %name;
		}
	}
	devLog("Sequence is invalid",%seqName);
	return "-1";
}
//==============================================================================
function ShapeLab::updateShapeSequenceData( %this,%seqName,%newName ) {
	ShapeLab_SeqPillStack.clear(); //Clear the new sequence lisitng stack
	//ShapeLabSequenceList.clear();
	//ShapeLabSequenceList.addRow( -1, "Name" TAB "Cyclic" TAB "Blend" TAB "Frames" TAB "Priority" );
	//ShapeLabSequenceList.setRowActive( -1, false );
	//ShapeLabSequenceList.addRow( 0, "<rootpose>" TAB "" TAB "" TAB "" TAB "" );
	%count = ShapeLab.shape.getSequenceCount();

	%sourceMenu = SL_ActiveSequence-->sourceSeq;
	%blendMenu = SL_ActiveSequence-->blendSeq;
	%sourceMenu.clear();
	%blendMenu.clear();
	ShapeLab_ThreadIdList.clear();
	ShapeLab_ThreadSeqList.clear();
	ShapeLab_ThreadSeqList.addRow( 0, "<rootpose>" TAB "" TAB "" TAB "" TAB "" );
	for ( %i = 0; %i < %count; %i++ ) {
		%name = ShapeLab.shape.getSequenceName( %i );		
		// Ignore __backup__ sequences (only used by editor)
		if ( !startswith( %name, "__backup__" ) ) {			
			ShapeLab.addSequencePill(%name);	
			ShapeLab_ThreadSeqList.addRow( %i, %name, %i ); 
			//if ( %name !$= %seqName ) {		
			%sourceMenu.add( %name );
			%blendMenu.add( %name );
			//}
		}
	}
	ShapeLabThreadViewer.onAddThread();        // add thread 0
}
//------------------------------------------------------------------------------
//==============================================================================
// SEQUENCE CREATE/ADD/DELETE FUNCTIONS
//==============================================================================

//==============================================================================
function ShapeLab::onAddSequence( %this, %name ) {
	if ( %name $= "" )
		%name = ShapeLab.getUniqueName( "sequence", "mySequence" );

	// Use the currently selected sequence as the base
	%curSeqName = %this.selectedSequence;

	if ( %curSeqName $= "" ) {
		// No sequence selected => open dialog to browse for one
		%startAt = ShapeLab.shape.baseShape; //Start at current loaded shape path
		if (isFile(ShapeLab.currentSeqPath))
		  %startAt = ShapeLab.currentSeqPath;
		getLoadFilename( "Anim Files|*.dae;*.dsq|COLLADA Files|*.dae|DSQ Files|*.dsq|Google Earth Files|*.kmz", %this @ ".onAddSequenceFromBrowse", %startAt );
		return;
	} else {
		%sourceData = ShapeLab.getSequenceSource( %curSeqName );
		%from = rtrim( getFields( %sourceData, 0, 1 ) );
		%start = getField( %sourceData, 2 );
		%end = getField( %sourceData, 3 );
		%frameCount = getField( %sourceData, 4 );
		// Add the new sequence
		ShapeLab.doAddSequence( %name, %curSeqName, %start, %end );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab::onAddSequenceFromBrowse( %this, %path ) {
	// Add a new sequence from the browse path
	%path = makeRelativePath( %path, getMainDotCSDir() );
	ShapeLabFromMenu.lastPath = %path;
	ShapeLab.currentSeqPath = %path;
	%name = ShapeLab.getUniqueName( "sequence", "mySequence" );
	ShapeLab.doAddSequence( %name, %path, 0, -1 );
	
}
//------------------------------------------------------------------------------

function ShapeLab::onDeleteSequence( %this,%button ) {
	devLog("ShapeLab::onDeleteSequence" );
	if (isObject(%button))
		%sequence = %button.seqName;
	else
		%sequence = ShapeLab.selectedSequence;
		
	if (%sequence $= "")
		return;
	ShapeLab.doRemoveShapeData( "Sequence", %sequence );
	
}
//==============================================================================
// SEQUENCE UPDATE FUNCTIONS
//==============================================================================

//==============================================================================
function ShapeLab::updateSequenceName( %this,%seqName,%newName ) {
	if (%seqName $= "")
		%seqName = ShapeLab.selectedSequence;
		
	if (%newName $= "")
		%newName = SL_ActiveSequence-->seqName.getText();
		
	if ( %seqName !$= "" && %newName !$= "" && %newName !$= %seqName)
		ShapeLab.doRenameSequence( %seqName, %newName );
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab::updateSequenceCyclic( %this,%seqName,%isCyclic ) {
	if ( %seqName !$= "" ) 		
		ShapeLab.doEditCyclic( %seqName, %isCyclic );
	
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab::updateSequencePriority( %this,%seqName ) {
	if (%seqName $= "")
		%seqName = ShapeLab.selectedSequence;
		
	if ( %seqName !$= "" ) {
		%newPriority = SL_ActiveSequence-->priority.getText();

		if ( %newPriority !$= "" )
			ShapeLab.doEditSequencePriority( %seqName, %newPriority );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLab::updateSequenceSource( %this, %path ,%pill ) {
	// ignore for shapes without sequences
	if (ShapeLab.shape.getSequenceCount() == 0)
		return;

	if (!isObject(%pill))
		%pill = SL_ActiveSequence;
	
	%start = %pill-->frameIn.getText();
	%end = %pill-->frameOut.getText();
	%seqName = %pill.seqName;
	if (%seqName $= "")
		return;

	if ( ( %start !$= "" ) && ( %end !$= "" ) ) {
		%oldSource = ShapeLab.getSequenceSource( %seqName );

		if ( %path $= "" )
			%path = rtrim( getFields( %oldSource, 0, 0 ) );

		if ( getFields( %oldSource, 0, 3 ) !$= ( %path TAB "" TAB %start TAB %end ) )
			ShapeLab.doEditSeqSource( %seqName, %path, %start, %end );
	}
}


//==============================================================================
function ShapeLab::updateSequenceBlend( %this,%seqName ) {
	if (%seqName $= "")
		%seqName = ShapeLab.selectedSequence;

	if ( %seqName !$= "" ) {
		// Get the blend flags (current and new)
		%oldBlendData = ShapeLab.shape.getSequenceSource( "Fake" );
		%oldBlend = getField( %oldBlendData, 0 );
		
		//MustBe Active Sequence
		if (ShapeLab.selectedSequence !$= %seqName)
		{
			devLog("Trying to update blend on unselected sequence:",%seqName,"Selected",ShapeLab.selectedSequence);
			return;
		}
		

		%blend = SL_ActiveSequence-->blend.isStateOn();
		if (%blend $= "")
			%blend = %oldBlend;

		// Ignore changes to the blend reference for non-blend sequences
		if ( !%oldBlend && !%isBlend )
			return;


		// OK - we're trying to change the blend properties of this sequence. The
		// new reference sequence and frame must be set.
		%blendSeq = SL_ActiveSequence-->blendSeq.getText();
		%blendFrame = SL_ActiveSequence-->blendFrame.getText();

		if ( ( %blendSeq $= "" ) || ( %blendFrame $= "" ) ) {
			LabMsgOK( "Blend reference not set", "The blend reference sequence and " @
						 "frame must be set before changing the blend flag or frame." );
			SL_ActiveSequence-->blend.setStateOn( %oldBlend );
			return;
		}

		// Get the current blend properties (use new values if not specified)
		%oldBlendSeq = getField( %oldBlendData, 1 );

		if ( %oldBlendSeq $= "" )
			%oldBlendSeq = %blendSeq;

		%oldBlendFrame = getField( %oldBlendData, 2 );

		if ( %oldBlendFrame $= "" )
			%oldBlendFrame = %blendFrame;

		// Check if there is anything to do
		if ( ( %oldBlend TAB %oldBlendSeq TAB %oldBlendFrame ) !$= ( %blend TAB %blendSeq TAB %blendFrame ) )
			ShapeLab.doEditBlend( %seqName, %blend, %blendSeq, %blendFrame );
	}
}
//------------------------------------------------------------------------------



//==============================================================================
// ShapeLab Sequence Update -> Name
//==============================================================================
//==============================================================================


//==============================================================================
// ShapeLab Sequence Update -> Cyclic
//==============================================================================


//==============================================================================
// ShapeLab Sequence Update -> Priority
//==============================================================================


//==============================================================================
// ShapeLab Sequence Update -> Belnding
//==============================================================================

//==============================================================================
// ShapeLab Sequence Update -> Belnding
//==============================================================================
//==============================================================================
function ShapeLab::onEditSequenceSource( %this, %from ,%pill ) {
	// ignore for shapes without sequences
	if (ShapeLab.shape.getSequenceCount() == 0)
		return;
	
	if (%pill $= "")
		%pill = SL_ActiveSequence;
		
	%start = %pill-->frameIn.getText();
	%end = %pill-->frameOut.getText();	
	%seqName = ShapeLab.validateSequenceName(%pill.seqName);
	if (%seqName $= "-1")
	{
		warnLog("Edit source data for invalid sequence:",%pill.seqName);
		return;
	}

	
	if ( ( %start !$= "" ) && ( %end !$= "" ) ) {
		%oldSource = ShapeLab.getSequenceSource( %seqName );

		if ( %from $= "" )
			%from = rtrim( getFields( %oldSource, 0, 0 ) );
devLog("onEditSequenceSource Start",%start,"End",%end,"Seq",%seqName,"From",%from);
		if ( getFields( %oldSource, 0, 3 ) !$= ( %from TAB "" TAB %start TAB %end ) )
			ShapeLab.doEditSeqSource( %seqName, %from, %start, %end );
	}
}
