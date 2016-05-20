//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//ShapeLab.setActiveSequence()
//==============================================================================
// Load the Scene Editor Plugin scripts, load Guis if %loadgui = true
function ShapeLab::setActiveSequence(%this,%seqName) {
	devLog("ShapeLab::setActiveSequence(%seqName)",%seqName);
	
	
	%cyclic = ShapeLab.shape.getSequenceCyclic( %seqName );
	%blend = getField( ShapeLab.shape.getSequenceBlend( %seqName ), 0 );
	%frameCount = ShapeLab.shape.getSequenceFrameCount( %seqName );
	%priority = ShapeLab.shape.getSequencePriority( %seqName );
	%sourceData = ShapeLab.getSequenceSource( %seqName );
	%seqFrom = rtrim( getFields( %sourceData, 0, 1 ) );
	%seqStart = getField( %sourceData, 2 );
	%seqEnd = getField( %sourceData, 3 );
	%seqFromTotal = getField( %sourceData, 4 );
	
	%cont = SL_ActiveSequence;
	%cont.seqName = %seqName;	
	%cont-->activeSeqName.setText(%seqName);
	%cont-->Cyclic.setStateOn(%cyclic);
	%cont-->Blend.setStateOn(%blend);
	%cont-->frameCount.setText(%frameCount);
	%cont-->priority.setText(%priority);
	//%cont-->seqName.setText(%seqName);
	%cont-->frameOut.setText(%seqEnd);
	%cont-->frameIn.setText(%seqStart);
	%cont-->sourceSeq.setText("Source");
	%cont-->blendSeq.setText("No blend");
	%cont-->Cyclic.pill = %cont;
	%cont-->Blend.pill = %cont;
	%cont-->frameCount.pill = %cont;
	%cont-->priority.pill = %cont;
	%cont-->seqName.pill = %cont;
	%cont-->sourceSeq.pill = %cont;
	%cont-->blendSeq.pill = %cont;
	%cont-->frameOut.pill = %cont;
	%cont-->frameIn.pill = %cont;
	%cont-->frameCount.active = 1;
	%cont-->priority.active = 1;
	%cont-->seqName.active = 1;
	%cont-->frameOut.active = 1;
	%cont-->frameIn.active = 1;
	%cont-->Blend.active = 1;
	%cont-->Cyclic.active = 1;
	if (%seqName $= "")
	{
		%cont-->activeSeqName.setText("No sequence selected");
		return;
	}
	if (%seqName $= ShapeLab.selectedSequence)
	{
		devLog("setActiveSequence The sequence is already active");
		//return;
	}
	//ShapeLabPreview-->endFrame.setText( %seqEnd );
	//ShapeLabPreview-->startFrame.setText( %seqStart );
	ShapeLab.selectedSequence = %seqName;
	//SL_ActiveSequence-->Cyclic.setStateOn(%cyclic);
	//ShapeLabPreview.setSequence(%seqName);
	ShapeLab.update_onSequenceChanged(%seqName);
	
	
}

//------------------------------------------------------------------------------

//==============================================================================

function ShapeLab_ActiveSeqEdit::onValidate(%this) {
	%pill = %this.pill;
	%seqName = %pill.seqName;
	ShapeLab.updateSequenceName(%seqName,%this.getText());
}
//==============================================================================
// Load the Scene Editor Plugin scripts, load Guis if %loadgui = true
function ShapeLab_SeqEdit::onValidate(%this) {
	%type = %this.internalName;
	%pill = %this.pill;
	%seqName = %pill.seqName;
	%seqName = %pill.internalName;
	switch$(%type) {
		case "frameIn":
			%frameCount = getWord( ShapeLabSeqSlider.range, 1 );
			// Force value to a frame index within the slider range
			%val = mRound( %this.getText() );

			if ( %val < 0 ) %val = 0;

			if ( %val > %frameCount ) %val = %frameCount;

			if ( %val >= %pill-->frameOut.getText() )
				%val = %pill-->frameOut.getText() - 1;

			%this.setText( %val );
			SL_ActiveSequence-->apply.active = true;
			ShapeLab.onEditSequenceSource("",%pill);

		case "frameOut":
			%frameCount = getWord( ShapeLabSeqSlider.range, 1 );
			// Force value to a frame index within the slider range
			%val = mRound( %this.getText() );

			if ( %val < 0 ) %val = 0;

			if ( %val > %frameCount ) %val = %frameCount;

			if ( %val <= %pill-->frameIn.getText() )
				%val = %pill-->frameIn.getText() + 1;

			%this.setText( %val );
			ShapeLab.onEditSequenceSource("",%pill);
			//SL_ActiveSequence-->apply.active = true;

		case "priority":
				ShapeLab.onEditPriority(  );
			
			

		case "seqName":
			ShapeLab.updateSequenceName(%seqName,%this.getText());
			/*%newName = %this.getText();

			if (%newName !$= %seqName) {
				ShapeLab.onEditSequenceName(%seqName,%newName);
				%pill.seqName = %newName;
			}
			*/
		}
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the Scene Editor Plugin scripts, load Guis if %loadgui = true
function ShapeLab_SeqCheck::onClick(%this) {
	%type = %this.internalName;	
	%seqName = SL_ActiveSequence.seqName;

	switch$(%type) {
	case "Cyclic":
		devLog("Set cyclic to",%this.isStateOn(),"For",%seqName);
		ShapeLab.updateSequenceCyclic( %seqName, %this.isStateOn() );

	case "Blend":
		ShapeLab.updateSequenceBlend( );
		//ShapeLab.onEditBlend(%seqName,%pill );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the Scene Editor Plugin scripts, load Guis if %loadgui = true
function ShapeLab_SeqMenu::onSelect(%this,%id,%text) {
	%type = %this.internalName;
	%pill = %this.pill;
	%seqName = %pill.seqName;

	switch$(%type) {
	case "sourceSeq":
		if ( %text $= "Browse..." ) {
			%seqFrom = rtrim( getFields( ShapeLab.getSequenceSource( %seqName ), 0, 1 ) );
			%this.setText( %seqFrom );
			// Allow the user to browse for an external source of animation data
			%startAt = ShapeLab.shape.baseShape; //Start at current loaded shape path
		   if (isFile(ShapeLab.currentSeqPath))
		    %startAt = ShapeLab.currentSeqPath;
		   
			getLoadFilename( "Anim Files|*.dae;*.dsq|COLLADA Files|*.dae|DSQ Files|*.dsq|Google Earth Files|*.kmz", %this @ ".onBrowseSelect",%startAt );
		} else {
			ShapeLab.updateSequenceSource( %text,%pill );
		}

	case "blendSeq":
		ShapeLab.updateSequenceBlend( );
	}
}
//------------------------------------------------------------------------------
function ShapeLab_SeqMenu::onBrowseSelect( %this, %path ) {
	%path = makeRelativePath( %path, getMainDotCSDir() );
	%this.lastPath = %path;
	%this.setText( %path );
	ShapeLab.updateSequenceSource( %path,%this.pill );
}
/*
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
*/

