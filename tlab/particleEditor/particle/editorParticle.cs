//==============================================================================
// TorqueLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

$PE_PARTICLEEDITOR_DEFAULT_FILENAME = "art/gfx/particles/managedParticleData.cs";


//=============================================================================================
//    PE_ParticleEditor.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function PE_ParticleEditor::guiSync( %this,%listOnly ) {
	// Populate the selector with the particles assigned
	// to the current emitter.
	%containsCurrParticle = false;
	%popup = PEP_ParticleSelector;
	%popup.clear();

	if ($ParticleEditor_ShowActiveParticles || $ParticleEditor_ShowActiveParticles $= ""){
		foreach$( %particle in PE_EmitterEditor.currEmitter.particles ) {
		if( %particle.getId() == PE_ParticleEditor.currParticle )
			%containsCurrParticle = true;

		%popup.add( %particle, %particle.getId() );
		}
	}		
	else{
		foreach( %particle in PE_AllParticles ) {
			if( %particle.getId() == PE_ParticleEditor.currParticle )
				%containsCurrParticle = true;

			%popup.add( %particle.getName(), %particle.getId() );
		}
	}
	// Just in case the particle doesn't exist, fallback gracefully

	if (%listOnly)
		return;
	if( !%containsCurrParticle )
		PE_ParticleEditor.currParticle = getWord( PE_EmitterEditor.currEmitter.particles, 0 ).getId();

	
	%data = PE_ParticleEditor.currParticle;
	%popup.sort();
	%popup.setSelected( %data );
	%bitmap = searchForTexture( %data.getName(), %data.textureName );

	if( %bitmap !$= "" ) {
		PE_ParticleEditor-->PEP_previewImage.setBitmap( %bitmap );
		PE_ParticleEditor-->PEP_previewImageName.setText( %bitmap );
		PE_ParticleEditor-->PEP_previewImageName.tooltip = %bitmap;
	} else {
		PE_ParticleEditor-->PEP_previewImage.setBitmap( "" );
		PE_ParticleEditor-->PEP_previewImageName.setText( "None" );
		PE_ParticleEditor-->PEP_previewImageName.tooltip = "None";
	}

	PE_ParticleEditor-->PEP_inverseAlpha.setValue( %data.useInvAlpha );
	PE_ParticleEditor-->PEP_lifetimeMS_slider.setValue( %data.lifetimeMS );
	PE_ParticleEditor-->PEP_lifetimeMS_textEdit.setText( %data.lifetimeMS );
	PE_ParticleEditor-->PEP_lifetimeVarianceMS_slider.setValue( %data.lifetimeVarianceMS );
	PE_ParticleEditor-->PEP_lifetimeVarianceMS_textEdit.setText( %data.lifetimeVarianceMS );
	PE_ParticleEditor-->PEP_inheritedVelFactor_slider.setValue( %data.inheritedVelFactor );
	PE_ParticleEditor-->PEP_inheritedVelFactor_textEdit.setText( %data.inheritedVelFactor );
	PE_ParticleEditor-->PEP_constantAcceleration_slider.setValue( %data.constantAcceleration );
	PE_ParticleEditor-->PEP_constantAcceleration_textEdit.setText( %data.constantAcceleration );
	PE_ParticleEditor-->PEP_gravityCoefficient_slider.setValue( %data.gravityCoefficient );
	PE_ParticleEditor-->PEP_gravityCoefficient_textEdit.setText( %data.gravityCoefficient );
	PE_ParticleEditor-->PEP_dragCoefficient_slider.setValue( %data.dragCoefficient );
	PE_ParticleEditor-->PEP_dragCoefficient_textEdit.setText( %data.dragCoefficient );
	PE_ParticleEditor-->PEP_spinRandomMin_slider.setValue( %data.spinRandomMin );
	PE_ParticleEditor-->PEP_spinRandomMin_textEdit.setText( %data.spinRandomMin );
	PE_ParticleEditor-->PEP_spinRandomMax_slider.setValue( %data.spinRandomMax );
	PE_ParticleEditor-->PEP_spinRandomMax_textEdit.setText( %data.spinRandomMax  );
	PE_ParticleEditor-->PEP_spinRandomMax_slider.setValue( %data.spinRandomMax );
	PE_ParticleEditor-->PEP_spinRandomMax_textEdit.setText( %data.spinRandomMax  );
	PE_ParticleEditor-->PEP_spinSpeed_slider.setValue( %data.spinSpeed );
	PE_ParticleEditor-->PEP_spinSpeed_textEdit.setText( %data.spinSpeed );
	
	PE_ColorTintPickers.findObjectByInternalName("0").basecolor = %data.colors[ 0 ];
	PE_ColorTintPickers.findObjectByInternalName("1").basecolor = %data.colors[ 1 ];
	PE_ColorTintPickers.findObjectByInternalName("2").basecolor = %data.colors[ 2 ];
	PE_ColorTintPickers.findObjectByInternalName("3").basecolor = %data.colors[ 3 ];
	
/*	PE_ColorTintPickers->0.color = %data.colors[ 0 ];
	PE_ColorTintPickers->1.color = %data.colors[ 1 ];
	PE_ColorTintPickers->2.color = %data.colors[ 2 ];
	PE_ColorTintPickers->3.color = %data.colors[ 3 ];*/
	
	PE_ParticleEditor-->PEP_pointSize_slider0.setValue( %data.sizes[ 0 ] );
	PE_ParticleEditor-->PEP_pointSize_textEdit0.setText( %data.sizes[ 0 ] );
	PE_ParticleEditor-->PEP_pointSize_slider1.setValue( %data.sizes[ 1 ] );
	PE_ParticleEditor-->PEP_pointSize_textEdit1.setText( %data.sizes[ 1 ] );
	PE_ParticleEditor-->PEP_pointSize_slider2.setValue( %data.sizes[ 2 ] );
	PE_ParticleEditor-->PEP_pointSize_textEdit2.setText( %data.sizes[ 2 ] );
	PE_ParticleEditor-->PEP_pointSize_slider3.setValue( %data.sizes[ 3 ] );
	PE_ParticleEditor-->PEP_pointSize_textEdit3.setText( %data.sizes[ 3 ] );
	PE_ParticleEditor-->PEP_pointTime_slider0.setValue( %data.times[ 0 ] );
	PE_ParticleEditor-->PEP_pointTime_textEdit0.setText( %data.times[ 0 ] );
	PE_ParticleEditor-->PEP_pointTime_slider1.setValue( %data.times[ 1 ] );
	PE_ParticleEditor-->PEP_pointTime_textEdit1.setText( %data.times[ 1 ] );
	PE_ParticleEditor-->PEP_pointTime_slider2.setValue( %data.times[ 2 ] );
	PE_ParticleEditor-->PEP_pointTime_textEdit2.setText( %data.times[ 2 ] );
	PE_ParticleEditor-->PEP_pointTime_slider3.setValue( %data.times[ 3 ] );
	PE_ParticleEditor-->PEP_pointTime_textEdit3.setText( %data.times[ 3 ] );
}


//=============================================================================================
//    PE_ColorTintSwatch.
//=============================================================================================
function PE_ParticleColorPicker::ColorPicked( %this,%color ) {
	devLog("PE_ParticleColorPicker::ColorPicked",%color);
	%this.baseColor = %color;
	%id = %this.internalName;
	PE_ParticleEditor.updateParticle( "colors[" @ %id @ "]", %color );	
	
}
function PE_ParticleColorPicker::ColorUpdated( %this,%color ) {
	devLog("PE_ParticleColorPicker::ColorUpdated",%color);
}


/*
function PE_ColorTintSwatch::updateParticleColor( %this, %color ) {
	devLog("PE_ColorTintSwatch::updateParticleColor( %this, %color )",%color);
	%arrayNum = %this.arrayNum;
	%r = getWord( %color, 0 );
	%g = getWord( %color, 1 );
	%b = getWord( %color, 2 );
	%a = getWord( %color, 3 );
	%color = %r SPC %g SPC %b SPC %a;
	%this.color = %color;
	PE_ParticleEditor.updateParticle( "colors[" @ %arrayNum @ "]", %color );
}
*/
//=============================================================================================
//    PEP_ParticleSelector_Control.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function PEP_ParticleSelector_Control::onRenameItem( %this ) {
	Parent::onRenameItem( %this );
	//FIXME: need to check for validity of name and name clashes
	PE_ParticleEditor.setParticleDirty();
	// Resort menu.
	%this-->PopupMenu.sort();
}

//=============================================================================================
//    PEP_NewParticleButton.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function PEP_NewParticleButton::onDefaultClick( %this ) {
	PE_ParticleEditor.showNewDialog();
}

//---------------------------------------------------------------------------------------------

function PEP_NewParticleButton::onCtrlClick( %this ) {
	for( %i = 1; %i < 5; %i ++ ) {
		%popup = "PEE_EmitterParticleSelector" @ %i;

		if( %popup.getSelected() == PEP_ParticleSelector.getSelected() ) {
			%replaceSlot = %i;
			break;
		}
	}

	PE_ParticleEditor.showNewDialog( %replaceSlot );
}
