//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
// Load the Scene Editor Plugin scripts, load Guis if %loadgui = true
function ShapeLab::addSequencePill(%this,%seqName) {
	%cyclic = ShapeLab.shape.getSequenceCyclic( %seqName );
	%blend = getField( ShapeLab.shape.getSequenceBlend( %seqName ), 0 );
	%frameCount = ShapeLab.shape.getSequenceFrameCount( %seqName );
	%priority = ShapeLab.shape.getSequencePriority( %seqName );
	%sourceData = ShapeLab.getSequenceSource( %seqName );
	%seqFrom = rtrim( getFields( %sourceData, 0, 1 ) );
	%seqStart = getField( %sourceData, 2 );
	%seqEnd = getField( %sourceData, 3 );
	%seqFromTotal = getField( %sourceData, 4 );
	hide(ShapeLab_SeqPillSrc);
	show(ShapeLab_SeqPillStack);
	%pill = cloneObject(ShapeLab_SeqPillSrc,"",%seqName,ShapeLab_SeqPillStack);
	
	%pill-->seqName.setText(%seqName);
	%pill-->frameOut.setText(%seqEnd);
	%pill-->frameIn.setText(%seqStart);
	%pill-->frameOut.pill = %pill;
	%pill-->frameIn.pill = %pill;
	%pill-->seqName.pill = %pill;		
	%pill-->seqName.active = 1;
	%pill-->frameOut.active = 1;
	%pill-->frameIn.active = 1;
	%pill-->deleteBtn.seqName = %seqName;
	
	
	%pill.seqName = %seqName;
	%pill.expanded = false;
	//%pill.superClass = "ShapeLab_SeqPillRollout";
	%pill.internalName = %seqName;
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the Scene Editor Plugin scripts, load Guis if %loadgui = true
function ShapeLab_SeqListEdit::onAction(%this) {
	devLog("ShapeLab_SeqListEdit::onAction(%this)");
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the Scene Editor Plugin scripts, load Guis if %loadgui = true
function ShapeLab_SeqListEdit::onActive(%this) {
	devLog("ShapeLab_SeqListEdit::onActive(%this)");
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the Scene Editor Plugin scripts, load Guis if %loadgui = true
function ShapeLab_SeqListEdit::onGainFirstResponder(%this) {
	devLog("ShapeLab_SeqListEdit::onGainFirstResponder(%this)");
	%seqName = %this.pill.seqName;
	ShapeLab.setActiveSequence(%seqName);
	
}
//------------------------------------------------------------------------------

//==============================================================================
// Load the Scene Editor Plugin scripts, load Guis if %loadgui = true
function ShapeLab_SeqListEdit::onValidate(%this) {
	%type = %this.internalName;
	%pill = %this.pill;
	%seqName = %pill.seqName;
	%seqIntName = %pill.internalName;
	
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
	

	case "seqName":
		ShapeLab.updateSequenceName(%seqName,%this.getText());
		/*
		%newName = %this.getText();

		if (%newName !$= %seqName) {
			ShapeLab.onEditSequenceName(%seqName,%newName);
			%pill.seqName = %newName;
		}*/
	}

}
//------------------------------------------------------------------------------
