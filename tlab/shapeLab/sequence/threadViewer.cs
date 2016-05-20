//==============================================================================
// TorqueLab -> ShapeLab -> Threads and Animation
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// ShapeLab -> Threads and Animation
//==============================================================================

//==============================================================================
function ShapeLabThreadViewer::onWake( %this ) {
	SLE_ThreadTransLastsCheck.setValue( 1 );
	%this-->transitionTime.setText( "0.5" );
	%this-->transitionTo.clear();
	%this-->transitionTo.add( "synched position", 0 );
	%this-->transitionTo.add( "slider position", 1 );
	%this-->transitionTo.setSelected( 0 );
	%this-->transitionTarget.clear();
	%this-->transitionTarget.add( "plays during transition", 0 );
	%this-->transitionTarget.add( "pauses during transition", 1 );
	%this-->transitionTarget.setSelected( 0 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabThreadViewer::syncPlaybackDetails( %this ) {
	%seqName = ShapeLab.selectedSequence;
	
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
		ShapeLabPreview-->seqIn.setText( %seqStart );
		ShapeLabPreview-->seqOut.setText( %seqEnd );
		%val = ShapeLabSeqSlider.getValue() / getWord( ShapeLabSeqSlider.range, 1 );
		ShapeLabSeqSlider.range = "0" SPC ( %seqFromTotal-1 );
		ShapeLabSeqSlider.setValue( %val * getWord( ShapeLabSeqSlider.range, 1 ) );
		SLE_ThreadSlider.range = ShapeLabSeqSlider.range;
		SLE_ThreadSlider.setValue( ShapeLabSeqSlider.value );
		ShapeLabPreview.setSequence( %seqName,%sourceData );
		ShapeLabPreview.setPlaybackLimit( "in", %seqStart );
		ShapeLabPreview.setPlaybackLimit( "out", %seqEnd );
	} else {
		// Hide sequence in/out bars
		ShapeLabPreview-->seqInBar.setVisible( false );
		ShapeLabPreview-->seqOutBar.setVisible( false );
		ShapeLabSeqFromMenu.setText( "" );
		ShapeLabSeqFromMenu.tooltip = "";
		ShapeLabPreview-->seqIn.setText( 0 );
		ShapeLabPreview-->seqOut.setText( 0 );
		ShapeLabSeqSlider.range = "0 1";
		ShapeLabSeqSlider.setValue( 0 );
		SLE_ThreadSlider.range = ShapeLabSeqSlider.range;
		SLE_ThreadSlider.setValue( ShapeLabSeqSlider.value );
		ShapeLabPreview.setPlaybackLimit( "in", 0 );
		ShapeLabPreview.setPlaybackLimit( "out", 1 );
		ShapeLabPreview.setSequence( "" );
	}
	
}
//------------------------------------------------------------------------------

//==============================================================================
function SLE_ThreadSlider::onMouseDragged( %this ) {
	if ( ShapeLabThreadViewer-->transitionTo.getText() $= "synched position" ) {
		// Pause the active thread when the slider is dragged
		if ( ShapeLabPreview-->pauseBtn.getValue() == 0 )
			ShapeLabPreview-->pauseBtn.performClick();

		ShapeLabPreview.setKeyframe( %this.getValue() );
	}
	SLE_ThreadFrameEdit.setText( %this.getValue() );
}
//------------------------------------------------------------------------------
//==============================================================================
function SLE_ThreadFrameEdit::onValidate( %this ) {
	%value = mRound(%this.getText());
	SLE_ThreadSlider.setValue(%value);
}
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// ShapeLab -> Button commands
//==============================================================================
function ShapeLabThreadViewer::goToStart( %this ) {
	ShapeLabPreview.setKeyframe( ShapeLabPreview-->seqOut.getText() );
}

function ShapeLabThreadViewer::stepBkwd( %this ) {
	ShapeLabPreview.setKeyframe( mCeil(ShapeLabSeqSlider.getValue() - 1) );
}
function ShapeLabThreadViewer::playBkwd( %this ) {
	ShapeLabPreview.setNoProxySequence();
	ShapeLabPreview.setThreadDirection( -1 );
}
function ShapeLabThreadViewer::playFwd( %this ) {
	ShapeLabPreview.setNoProxySequence();
	ShapeLabPreview.setThreadDirection( 1 );
}

function ShapeLabThreadViewer::stepFwd( %this ) {
	ShapeLabPreview.setKeyframe( mFloor(ShapeLabSeqSlider.getValue() + 1) );
}

function ShapeLabThreadViewer::pause( %this ) {
	ShapeLabPreview.setThreadDirection( 0 );
}

function ShapeLabThreadViewer::goToEnd( %this ) {
	ShapeLabPreview.setKeyframe( ShapeLabPreview-->seqOut.getText() );
}




