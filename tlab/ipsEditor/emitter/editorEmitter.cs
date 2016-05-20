//==============================================================================
// TorqueLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


$IPSE_Editor_DEFAULT_FILENAME = "art/gfx/particles/managedParticleEmitterData.cs";
$IPSE_ClassFields["SphereEmitterData"] = "ejectionOffsetVariance thetaMin thetaMax phiReferenceVel phiVariance";

//=============================================================================================
//    IPSE_Editor.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function IPSE_Editor::guiSync( %this ) {
	%data = IPSE_Editor.currEmitter;
   IPSE_Selector_Control-->TextEdit.setText(%data.getName());
	// Sync up sliders and number boxes.

	if( IPSE_Editor-->infiniteLoop.isStateOn() ) {		
		IPSE_Editor-->lifetimeMS_slider.setActive( false );
		IPSE_Editor-->lifetimeMS_edit.setActive( false );
		IPSE_Editor-->lifetimeVarianceMS_slider.setActive( false );
		IPSE_Editor-->lifetimeVarianceMS_edit.setActive( false );
	} else {
	
		IPSE_Editor-->lifetimeMS_slider.setActive( true );
		IPSE_Editor-->lifetimeMS_edit.setActive( true );
		IPSE_Editor-->lifetimeVarianceMS_slider.setActive( true );
		IPSE_Editor-->lifetimeVarianceMS_edit.setActive( true );
		IPSE_Editor-->lifetimeMS_slider.setValue( %data.lifetimeMS );
		IPSE_Editor-->lifetimeMS_edit.setText( %data.lifetimeMS );
		IPSE_Editor-->lifetimeVarianceMS_slider.setValue( %data.lifetimeVarianceMS );
		IPSE_Editor-->lifetimeVarianceMS_edit.setText( %data.lifetimeVarianceMS );
	}

	IPSE_Editor-->ejectionPeriodMS_slider.setValue( %data.ejectionPeriodMS );
	IPSE_Editor-->ejectionPeriodMS_edit.setText( %data.ejectionPeriodMS );
	IPSE_Editor-->periodVarianceMS_slider.setValue( %data.periodVarianceMS );
	IPSE_Editor-->periodVarianceMS_edit.setText( %data.periodVarianceMS );
	IPSE_Editor-->ejectionVelocity_slider.setValue( %data.ejectionVelocity );
	IPSE_Editor-->ejectionVelocity_edit.setText( %data.ejectionVelocity );
	IPSE_Editor-->velocityVariance_slider.setValue( %data.velocityVariance );
	IPSE_Editor-->velocityVariance_edit.setText( %data.velocityVariance );
	IPSE_Editor-->orientParticles.setValue( %data.orientParticles );
	IPSE_Editor-->alignParticles.setValue( %data.alignParticles );
	IPSE_Editor-->alignDirection.setText( %data.alignDirection );
	IPSE_Editor-->thetaMin_slider.setValue( %data.thetaMin );
	IPSE_Editor-->thetaMin_edit.setText( %data.thetaMin );
	IPSE_Editor-->thetaMax_slider.setValue( %data.thetaMax );
	IPSE_Editor-->thetaMax_edit.setText( %data.thetaMax );
	IPSE_Editor-->phiVariance_slider.setValue( %data.phiVariance );
	IPSE_Editor-->phiVariance_edit.setText( %data.phiVariance );
	IPSE_Editor-->ejectionOffset_slider.setValue( %data.ejectionOffset );
	IPSE_Editor-->ejectionOffset_edit.setText( %data.ejectionOffset );
	%blendTypeId = IPSE_Editor-->blendStyle.findText( %data.blendStyle );
	IPSE_Editor-->blendStyle.setSelected( %blendTypeId, false );
	IPSE_Editor-->softnessDistance_slider.setValue( %data.softnessDistance );
	IPSE_Editor-->softnessDistance_edit.setText( %data.softnessDistance );
	IPSE_Editor-->ambientFactor_slider.setValue( %data.ambientFactor );
	IPSE_Editor-->ambientFactor_edit.setText( %data.ambientFactor );
	IPSE_Editor-->softParticles.setValue( %data.softParticles );
	IPSE_Editor-->reverseOrder.setValue( %data.reverseOrder );
	IPSE_Editor-->useEmitterSizes.setValue( %data.useEmitterSizes );
	IPSE_Editor-->useEmitterColors.setValue( %data.useEmitterColors );

	// Sync up particle selectors.

	for( %index = 0; %index < 4; %index ++ ) {
		%ctrl = "IPSE_EmitterParticle" @ ( %index + 1 );
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
//    IPSE_Selector_Control.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function IPSE_Selector_Control::onRenameItem( %this ) {
	Parent::onRenameItem( %this );
	//FIXME: need to check for validity of name and name clashes
	IPSE_Editor.setEmitterDirty();
	// Resort menu.
	%this-->PopupMenu.sort();
}
