//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// ACTION CALLBACKS
//==============================================================================
//==============================================================================
function ShapeLab::update_onSequenceChanged( %this, %seqName ) {
	devLog("ShapeLab::update_onSequenceChanged %seqName", %seqName );	
	
	
		// Clear the trigger list
		ShapeLabPreview_TriggerCont.clear();
	ShapeLab_TriggerList.removeAll();
	%count = ShapeLab.shape.getTriggerCount( %seqName );
	for ( %i = 0; %i < %count; %i++ ) {
		%trigger = ShapeLab.shape.getTrigger( %seqName, %i );
		ShapeLab_TriggerList.addItem( getWord( %trigger, 0 ), getWord( %trigger, 1 ) );
	}
ShapeLabThreadViewer.syncPlaybackDetails();
}
//==============================================================================
function ShapeLab::update_onSequenceRemoved( %this, %seqName ) {
	devLog("ShapeLab::update_onSequenceRemoved %seqName", %seqName );
	// --- MISC ---
	ShapeLabHints.updateHints();
	%seqPill = ShapeLab_SeqPillStack.findObjectByInternalName(%seqName);
	delObj(%seqPill);
	
	// --- SEQUENCES TAB ---
	//%isSelected = ( ShapeLabSequenceList.getSelectedName() $= %seqName );
	//ShapeLabSequenceList.removeItem( %seqName );

	//if ( %isSelected )
		//ShapeLabPropWindow.update_onSeqSelectionChanged();

	// --- THREADS WINDOW ---
	ShapeLabShapeView.refreshThreadSequences();
}
//==============================================================================
//------------------------------------------------------------------------------
function ShapeLab::update_onSequenceRenamed( %this, %oldName, %newName ) {
	devLog("ShapeLab::update_onSequenceRenamed",%oldName, %newName );
	// --- MISC ---
	ShapeLabHints.updateHints();
	// Rename the proxy sequence as well
	%oldProxy = ShapeLab.getProxyName( %oldName );
	%newProxy = ShapeLab.getProxyName( %newName );

	if ( ShapeLab.shape.getSequenceIndex( %oldProxy ) != -1 )
		ShapeLab.shape.renameSequence( %oldProxy, %newProxy );

	SL_ActiveSequence-->activeSeqName.setText(%newName);
	ShapeLab.selectedSequence = %newName;
	SL_ActiveSequence.seqName = %newName;
	// --- SEQUENCES TAB ---
	%seqPill = ShapeLab_SeqPillStack.findObjectByInternalName(%oldName);
	%seqPill-->seqName.setText(%newName);
	%seqPill.seqName = %newName; 
	%seqPill.internalName = %newName; 

	
	
//	ShapeLabSequenceList.editColumn( %oldName, 0, %newName );

	//if ( ShapeLabSequenceList.getSelectedName() $= %newName )
		//ShapeLabSequences-->seqName.setText( %newName );

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
//==============================================================================
//------------------------------------------------------------------------------
function ShapeLab::update_onSequenceCyclicChanged( %this, %seqName, %cyclic ) {
	devLog("ShapeLab::update_onSequenceCyclicChanged %seqName %cyclic",%seqName, %cyclic );
	// --- MISC ---
	// Apply the same transformation to the proxy animation if necessary
	%proxyName = ShapeLab.getProxyName( %seqName );

	if ( ShapeLab.shape.getSequenceIndex( %proxyName ) != -1 )
		ShapeLab.shape.setSequenceCyclic( %proxyName, %cyclic );

	// --- SEQUENCES TAB ---
	if (%seqName $= ShapeLab.selectedSequence)
		SL_ActiveSequence-->Cyclic.setStateOn(%cyclic);
	
	//ShapeLabSequenceList.editColumn( %seqName, 1, %cyclic ? "yes" : "no" );

	//if ( ShapeLabSequenceList.getSelectedName() $= %seqName )
		//ShapeLabSequences-->cyclicFlag.setStateOn( %cyclic );
}
//==============================================================================
//------------------------------------------------------------------------------
function ShapeLab::update_onSequenceBlendChanged( %this, %seqName, %blend,
		%oldBlendSeq, %oldBlendFrame, %blendSeq, %blendFrame ) {
			devLog("ShapeLab::update_onSequenceBlendChanged %seqName, %blend,	%oldBlendSeq, %oldBlendFrame, %blendSeq, %blendFrame",%seqName, %blend,	%oldBlendSeq, %oldBlendFrame, %blendSeq, %blendFrame );
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
	if (%seqName $= ShapeLab.selectedSequence)
	{
		SL_ActiveSequence-->Blend.setStateOn(%blend);
		SL_ActiveSequence-->blendFlag.setStateOn( %blend );
		SL_ActiveSequence-->blendSeq.setText( %blendSeq );
		SL_ActiveSequence-->blendFrame.setText( %blendFrame );
	}
}
//==============================================================================
//------------------------------------------------------------------------------
function ShapeLab::update_onSequencePriorityChanged( %this, %seqName ) {
	devLog("ShapeLab::update_onSequencePriorityChanged %seqName",%seqName );
	// --- SEQUENCES TAB ---
	%priority = ShapeLab.shape.getSequencePriority( %seqName );
	// --- SEQUENCES TAB ---
	if (%seqName $= ShapeLab.selectedSequence)
		SL_ActiveSequence-->priority.setText(%seqName);
	
	//ShapeLabSequenceList.editColumn( %seqName, 4, %priority );

	//if ( ShapeLabSequenceList.getSelectedName() $= %seqName )
		//ShapeLabSequences-->priority.setText( %priority );
}
//==============================================================================
//------------------------------------------------------------------------------
function ShapeLab::update_onSequenceGroundSpeedChanged( %this, %seqName ) {
	devLog("ShapeLab::update_onSequenceGroundSpeedChanged %seqName",%seqName );
	// nothing to do yet
}
function ShapeLab::update_onSequenceSourceChanged( %this, %seqName,%startFrame,%endFrame ) {
	devLog("ShapeLab::update_onSequenceSourceChanged %seqName",%seqName );
	// --- SEQUENCES TAB ---
	%priority = ShapeLab.shape.getSequencePriority( %seqName );
	// --- SEQUENCES TAB ---
	if (%seqName $= ShapeLab.selectedSequence){	
		SL_ActiveSequence-->frameOut.setText(%endFrame);
		SL_ActiveSequence-->frameIn.setText(%startFrame);
		ShapeLabPreview-->seqOut.setText( %endFrame );
		ShapeLabPreview-->seqIn.setText( %startFrame );
	}
	
	//ShapeLabSequenceList.editColumn( %seqName, 4, %priority );

	//if ( ShapeLabSequenceList.getSelectedName() $= %seqName )
		//ShapeLabSequences-->priority.setText( %priority );
}
//==============================================================================