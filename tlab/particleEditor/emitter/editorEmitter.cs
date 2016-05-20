//==============================================================================
// TorqueLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


$PE_EMITTEREDITOR_DEFAULT_FILENAME = "art/gfx/particles/managedParticleEmitterData.cs";


//=============================================================================================
//    PE_EmitterEditor.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function PE_EmitterEditor::guiSync( %this ) {
	%data = PE_EmitterEditor.currEmitter;

	// Sync up sliders and number boxes.

	if( PE_EmitterEditor-->PEE_infiniteLoop.isStateOn() ) {
		devLog("PEE_infiniteLoop is ON");
		PE_EmitterEditor-->PEE_lifetimeMS_slider.setActive( false );
		PE_EmitterEditor-->PEE_lifetimeMS_textEdit.setActive( false );
		PE_EmitterEditor-->PEE_lifetimeVarianceMS_slider.setActive( false );
		PE_EmitterEditor-->PEE_lifetimeVarianceMS_textEdit.setActive( false );
	} else {
		devLog("PEE_infiniteLoop is OFF");
		PE_EmitterEditor-->PEE_lifetimeMS_slider.setActive( true );
		PE_EmitterEditor-->PEE_lifetimeMS_textEdit.setActive( true );
		PE_EmitterEditor-->PEE_lifetimeVarianceMS_slider.setActive( true );
		PE_EmitterEditor-->PEE_lifetimeVarianceMS_textEdit.setActive( true );
		PE_EmitterEditor-->PEE_lifetimeMS_slider.setValue( %data.lifetimeMS );
		PE_EmitterEditor-->PEE_lifetimeMS_textEdit.setText( %data.lifetimeMS );
		PE_EmitterEditor-->PEE_lifetimeVarianceMS_slider.setValue( %data.lifetimeVarianceMS );
		PE_EmitterEditor-->PEE_lifetimeVarianceMS_textEdit.setText( %data.lifetimeVarianceMS );
	}

	PE_EmitterEditor-->PEE_ejectionPeriodMS_slider.setValue( %data.ejectionPeriodMS );
	PE_EmitterEditor-->PEE_ejectionPeriodMS_textEdit.setText( %data.ejectionPeriodMS );
	PE_EmitterEditor-->PEE_periodVarianceMS_slider.setValue( %data.periodVarianceMS );
	PE_EmitterEditor-->PEE_periodVarianceMS_textEdit.setText( %data.periodVarianceMS );
	PE_EmitterEditor-->PEE_ejectionVelocity_slider.setValue( %data.ejectionVelocity );
	PE_EmitterEditor-->PEE_ejectionVelocity_textEdit.setText( %data.ejectionVelocity );
	PE_EmitterEditor-->PEE_velocityVariance_slider.setValue( %data.velocityVariance );
	PE_EmitterEditor-->PEE_velocityVariance_textEdit.setText( %data.velocityVariance );
	PE_EmitterEditor-->PEE_orientParticles.setValue( %data.orientParticles );
	PE_EmitterEditor-->PEE_alignParticles.setValue( %data.alignParticles );
	PE_EmitterEditor-->PEE_alignDirection.setText( %data.alignDirection );
	PE_EmitterEditor-->PEE_thetaMin_slider.setValue( %data.thetaMin );
	PE_EmitterEditor-->PEE_thetaMin_textEdit.setText( %data.thetaMin );
	PE_EmitterEditor-->PEE_thetaMax_slider.setValue( %data.thetaMax );
	PE_EmitterEditor-->PEE_thetaMax_textEdit.setText( %data.thetaMax );
	PE_EmitterEditor-->PEE_phiVariance_slider.setValue( %data.phiVariance );
	PE_EmitterEditor-->PEE_phiVariance_textEdit.setText( %data.phiVariance );
	PE_EmitterEditor-->PEE_ejectionOffset_slider.setValue( %data.ejectionOffset );
	PE_EmitterEditor-->PEE_ejectionOffset_textEdit.setText( %data.ejectionOffset );
	%blendTypeId = PE_EmitterEditor-->PEE_blendType.findText( %data.blendStyle );
	PE_EmitterEditor-->PEE_blendType.setSelected( %blendTypeId, false );
	PE_EmitterEditor-->PEE_softnessDistance_slider.setValue( %data.softnessDistance );
	PE_EmitterEditor-->PEE_softnessDistance_textEdit.setText( %data.softnessDistance );
	PE_EmitterEditor-->PEE_ambientFactor_slider.setValue( %data.ambientFactor );
	PE_EmitterEditor-->PEE_ambientFactor_textEdit.setText( %data.ambientFactor );
	PE_EmitterEditor-->PEE_softParticles.setValue( %data.softParticles );
	PE_EmitterEditor-->PEE_reverseOrder.setValue( %data.reverseOrder );
	PE_EmitterEditor-->PEE_useEmitterSizes.setValue( %data.useEmitterSizes );
	PE_EmitterEditor-->PEE_useEmitterColors.setValue( %data.useEmitterColors );

	// Sync up particle selectors.

	for( %index = 0; %index < 4; %index ++ ) {
		%ctrl = "PEE_EmitterParticle" @ ( %index + 1 );
		%popup = %ctrl-->PopUpMenu;
		%particle = getWord( %data.particles, %index );

		if( isObject( %particle ) )
			%popup.setSelected( %particle.getId(), false );
		else
			%popup.setSelected( 0, false ); // Select "None".
	}
}

//---------------------------------------------------------------------------------------------


//=============================================================================================
//    PEE_EmitterSelector_Control.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function PEE_EmitterSelector_Control::onRenameItem( %this ) {
	Parent::onRenameItem( %this );
	//FIXME: need to check for validity of name and name clashes
	PE_EmitterEditor.setEmitterDirty();
	// Resort menu.
	%this-->PopupMenu.sort();
}
