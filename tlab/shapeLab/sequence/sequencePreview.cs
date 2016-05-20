//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$ShapeLab_NoProxyPreview = false;
//==============================================================================
// GuiShapeLabPreview - Called when the position of the active thread has changed, such as during playback
function ShapeLabShapeView::onThreadPosChanged( %this, %pos, %inTransition ) {
	// Update sliders
	%frame = ShapeLabPreview.threadPosToKeyframe( %pos );

	if (isObject(ShapeLabSeqSlider))
		ShapeLabSeqSlider.setValue( %frame );

	if (isObject(ShapeLabPreview-->currentFrame))
		ShapeLabPreview-->currentFrame.setText( mRound(%frame) );

	if ( ShapeLabThreadViewer-->transitionTo.getText() $= "synched position" ) {
		SLE_ThreadSlider.setValue( %frame );

		// Highlight the slider during transitions
		if ( %inTransition )
			SLE_ThreadSlider.profile = ToolsSliderMarker;
		else
			SLE_ThreadSlider.profile = ToolsSliderProfile;
	}
}
//==============================================================================
// Set the sequence to play
function ShapeLabPreview::setSequence( %this, %seqName,%seqSource ) {
	%this.usingProxySeq = false;

	if ( SLE_ThreadTransLastsCheck.getValue() ) {
		%transTime = SLE_ThreadTransDuration.getText();

		if ( ShapeLabThreadViewer-->transitionTo.getText() $= "synched position" )
			%transPos = -1;
		else
			%transPos = %this.keyframeToThreadPos( SLE_ThreadSlider.getValue() );

		%transPlay = ( ShapeLabThreadViewer-->transitionTarget.getText() $= "plays during transition" );
	} else {
		%transTime = 0;
		%transPos = 0;
		%transPlay = 0;
	}

	// No transition when sequence is not changing
	if ( %seqName $= ShapeLabShapeView.getThreadSequence() )
		%transTime = 0;

	if ( %seqName !$= "" ) {
		// To be able to effectively scrub through the animation, we need to have all
		// frames available, even if it was added with only a subset. If that is the
		// case, then create a proxy sequence that has all the frames instead.
		if (%seqSource $= "")
			%seqSource = ShapeLab.getSequenceSource( %seqName );
		%from = rtrim( getFields( %seqSource, 0, 1 ) );
		%startFrame = getField( %seqSource, 2 );
		%endFrame = getField( %seqSource, 3 );
		%frameCount = getField( %seqSource, 4 );

		if ( ( %startFrame != 0 ) || ( %endFrame != ( %frameCount-1 ) ) && !$ShapeLab_NoProxyPreview ) {
			%proxyName = ShapeLab.getProxyName( %seqName );

			if ( ShapeLab.shape.getSequenceIndex( %proxyName ) != -1 ) {
				ShapeLab.shape.removeSequence( %proxyName );
				ShapeLabShapeView.refreshThreadSequences();
			}

			ShapeLab.shape.addSequence( %from, %proxyName );
			// Limit the transition position to the in/out range
			%transPos = mClamp( %transPos, 0, 1 );
		}

		ShapeLabShapeView.setThreadSequence( %seqName, %transTime, %transPos, %transPlay );
	}

	SL_ActiveThreadInfo-->activeSeqText.setText("Sequence:\c1 "@%seqName);
	//ShapeLab.setActiveSequence(%seqName);
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabPreview::getTimelineBitmapPos( %this, %val, %width ) {
	%frameCount = getWord( ShapeLabSeqSlider.range, 1 );
	%pos_x = getWord( ShapeLabSeqSlider.getPosition(), 0 );
	%len_x = getWord( ShapeLabSeqSlider.getExtent(), 0 ) - %width;
	return %pos_x + ( ( %len_x * %val / %frameCount ) );
}

// Set the in or out sequence limit
function ShapeLabPreview::setPlaybackLimit( %this, %limit, %val ) {
	// Determine where to place the in/out bar on the slider
	%thumbWidth = 8;    // width of the thumb bitmap
	%pos_x = %this.getTimelineBitmapPos( %val, %thumbWidth );

	if ( %limit $= "in" ) {
		%this.seqStartFrame = %val;
		%this-->seqIn.setText( %val );
		%this-->seqInBar.setPosition( %pos_x, 0 );
	} else {
		%this.seqEndFrame = %val;
		%this-->seqOut.setText( %val );
		%this-->seqOutBar.setPosition( %pos_x, 0 );
	}
}
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// ShapeLab -> Sequence Editing
//==============================================================================
//------------------------------------------------------------------------------

function ShapeLabSeqSlider::onMouseDragged( %this ) {
	// Pause the active thread when the slider is dragged
	if ( ShapeLabPreview-->pauseBtn.getValue() == 0 )
		ShapeLabPreview-->pauseBtn.performClick();

	ShapeLabPreview.setKeyframe( %this.getValue() );
}

//==============================================================================
function ShapeLabPreview::threadPosToKeyframe( %this, %pos ) {
	if ( %this.usingProxySeq ) {
		%start = getWord( ShapeLabSeqSlider.range, 0 );
		%end = getWord( ShapeLabSeqSlider.range, 1 );
	} else {
		%start = ShapeLabPreview.seqStartFrame;
		%end = ShapeLabPreview.seqEndFrame;
	}

	return %start + ( %end - %start ) * %pos;
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabPreview::keyframeToThreadPos( %this, %frame ) {
	if ( %this.usingProxySeq ) {
		%start = getWord( ShapeLabSeqSlider.range, 0 );
		%end = getWord( ShapeLabSeqSlider.range, 1 );
	} else {
		%start = ShapeLabPreview.seqStartFrame;
		%end = ShapeLabPreview.seqEndFrame;
	}

	return ( %frame - %start ) / ( %end - %start );
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabPreview::setKeyframe( %this, %frame ) {
	ShapeLabSeqSlider.setValue( %frame );
	ShapeLabPreview-->currentFrame.setText(  mRound(%frame) );

	if ( ShapeLabThreadViewer-->transitionTo.getText() $= "synched position" )
		SLE_ThreadSlider.setValue( %frame );

	// Update the position of the active thread => if outside the in/out range,
	// need to switch to the proxy sequence
	if ( !%this.usingProxySeq  && !$ShapeLab_NoProxyPreview) {
		if ( ( %frame < %this.seqStartFrame ) || ( %frame > %this.seqEndFrame) ) {
			%this.usingProxySeq = true;
			%proxyName = ShapeLab.getProxyName( ShapeLabShapeView.getThreadSequence() );
			ShapeLabShapeView.setThreadSequence( %proxyName, 0, 0, false );
		}
	}

	ShapeLabShapeView.threadPos = %this.keyframeToThreadPos( %frame );
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabPreview::setNoProxySequence( %this ) {
	// no need to use the proxy sequence during playback
	if ( %this.usingProxySeq ) {
		%this.usingProxySeq = false;
		%seqName = ShapeLab.getUnproxyName( ShapeLabShapeView.getThreadSequence() );
		ShapeLabShapeView.setThreadSequence( %seqName, 0, 0, false );
		ShapeLabShapeView.threadPos = %this.keyframeToThreadPos( ShapeLabSeqSlider.getValue() );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabPreview::togglePause( %this ) {
	if ( %this-->pauseBtn.getValue() == 0 ) {
		%this.lastDirBkwd = %this-->playBkwdBtn.getValue();
		%this-->pauseBtn.performClick();
	} else {
		%this.setNoProxySequence();

		if ( %this.lastDirBkwd )
			%this-->playBkwdBtn.performClick();
		else
			%this-->playFwdBtn.performClick();
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the direction of the current thread (-1: reverse, 0: paused, 1: forward)
function ShapeLabPreview::setThreadDirection( %this, %dir ) {
	// Update thread direction
	ShapeLabShapeView.threadDirection = %dir;

	// Sync the controls in the thread window
	switch ( %dir ) {
	case -1:
		ShapeLabThreadViewer-->playBkwdBtn.setStateOn( 1 );

	case 0:
		ShapeLabThreadViewer-->pauseBtn.setStateOn( 1 );

	case 1:
		ShapeLabThreadViewer-->playFwdBtn.setStateOn( 1 );
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabPreview::togglePingPong( %this ) {
	ShapeLabShapeView.threadPingPong = %this-->pingpong.getValue();

	if ( %this-->playFwdBtn.getValue() )
		%this-->playFwdBtn.performClick();
	else if ( %this-->playBkwdBtn.getValue() )
		%this-->playBkwdBtn.performClick();
}
//------------------------------------------------------------------------------
function ShapeLabPreview::onEditSeqInOut( %this, %type, %val ) {
	devLog("ShapeLabPreview::onEditSeqInOut  %type, %val", %type, %val );
	%frameCount = getWord( ShapeLabSeqSlider.range, 1 );
	// Force value to a frame index within the slider range
	%val = mRound( %val );

	if ( %val < 0 )
		%val = 0;

	if ( %val > %frameCount )
		%val = %frameCount;

	// Enforce 'in' value must be < 'out' value
	if ( %type $= "in" ) {
		if ( %val >= %this-->seqOut.getText() )
			%val = %this-->seqOut.getText() - 1;

		SL_ActiveSequence-->frameIn.setText( %val );
		%this-->seqIn.setText( %val );
	} else {
		if ( %val <= %this-->seqIn.getText() )
			%val = %this-->seqIn.getText() + 1;

		SL_ActiveSequence-->frameOut.setText( %val );
		%this-->seqOut.setText( %val );
	}

	ShapeLab.onEditSequenceSource( "",SL_ActiveSequence );
}
