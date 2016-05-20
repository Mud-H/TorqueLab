//==============================================================================
// TorqueLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

$IPSP_Editor_DEFAULT_FILENAME = "art/gfx/particles/managedParticleData.cs";


//=============================================================================================
//    IPSP_Editor.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function IPSP_Editor::guiSync( %this,%listOnly ) {
	// Populate the selector with the particles assigned
	// to the current emitter.
	%containsCurrParticle = false;
	%popup = IPSP_Selector;
	%popup.clear();

   
  
	if ($IpsEditor_ShowActiveParticles || $IpsEditor_ShowActiveParticles $= ""){
		foreach$( %particle in IPSE_Editor.currEmitter.particles ) {
		if( %particle.getId() == IPSP_Editor.currParticle )
			%containsCurrParticle = true;

		%popup.add( %particle, %particle.getId() );
		}
	}		
	else{
		foreach( %particle in IPS_AllParticles ) {
			if( %particle.getId() == IPSP_Editor.currParticle )
				%containsCurrParticle = true;

			%popup.add( %particle.getName(), %particle.getId() );
		}
	}
	// Just in case the particle doesn't exist, fallback gracefully

	if (%listOnly)
		return;
	if( !%containsCurrParticle )
		IPSP_Editor.currParticle = getWord( IPSE_Editor.currEmitter.particles, 0 ).getId();

	
	%data = IPSP_Editor.currParticle;
	%popup.sort();
	%popup.setSelected( %data );
	%bitmap = searchForTexture( %data.getName(), %data.textureName );

    IPSP_Selector_Control-->TextEdit.setText(%data.getName());
	if( %bitmap !$= "" ) {
		IPSP_Editor-->textureName_preview.setBitmap( %bitmap );
		IPSP_Editor-->textureName.setText( %bitmap );
		IPSP_Editor-->textureName.tooltip = %bitmap;
	} else {
		IPSP_Editor-->textureName_preview.setBitmap( "" );
		IPSP_Editor-->textureName.setText( "None" );
		IPSP_Editor-->textureName.tooltip = "None";
	}

	IPSP_Editor-->useInvAlpha.setValue( %data.useInvAlpha );
	IPSP_Editor-->lifetimeMS_slider.setValue( %data.lifetimeMS );
	IPSP_Editor-->lifetimeMS_edit.setText( %data.lifetimeMS );
	IPSP_Editor-->lifetimeVarianceMS_slider.setValue( %data.lifetimeVarianceMS );
	IPSP_Editor-->lifetimeVarianceMS_edit.setText( %data.lifetimeVarianceMS );
	IPSP_Editor-->inheritedVelFactor_slider.setValue( %data.inheritedVelFactor );
	IPSP_Editor-->inheritedVelFactor_edit.setText( %data.inheritedVelFactor );
	IPSP_Editor-->constantAcceleration_slider.setValue( %data.constantAcceleration );
	IPSP_Editor-->constantAcceleration_edit.setText( %data.constantAcceleration );
	IPSP_Editor-->gravityCoefficient_slider.setValue( %data.gravityCoefficient );
	IPSP_Editor-->gravityCoefficient_edit.setText( %data.gravityCoefficient );
	IPSP_Editor-->dragCoefficient_slider.setValue( %data.dragCoefficient );
	IPSP_Editor-->dragCoefficient_edit.setText( %data.dragCoefficient );
	IPSP_Editor-->spinRandomMin_slider.setValue( %data.spinRandomMin );
	IPSP_Editor-->spinRandomMin_edit.setText( %data.spinRandomMin );
	IPSP_Editor-->spinRandomMax_slider.setValue( %data.spinRandomMax );
	IPSP_Editor-->spinRandomMax_edit.setText( %data.spinRandomMax  );
	IPSP_Editor-->spinRandomMax_slider.setValue( %data.spinRandomMax );
	IPSP_Editor-->spinRandomMax_edit.setText( %data.spinRandomMax  );
	IPSP_Editor-->spinSpeed_slider.setValue( %data.spinSpeed );
	IPSP_Editor-->spinSpeed_edit.setText( %data.spinSpeed );
	
	IPS_ColorTintPickers.findObjectByInternalName("0").basecolor = %data.colors[ 0 ];
	IPS_ColorTintPickers.findObjectByInternalName("1").basecolor = %data.colors[ 1 ];
	IPS_ColorTintPickers.findObjectByInternalName("2").basecolor = %data.colors[ 2 ];
	IPS_ColorTintPickers.findObjectByInternalName("3").basecolor = %data.colors[ 3 ];
	
/*	IPS_ColorTintPickers->0.color = %data.colors[ 0 ];
	IPS_ColorTintPickers->1.color = %data.colors[ 1 ];
	IPS_ColorTintPickers->2.color = %data.colors[ 2 ];
	IPS_ColorTintPickers->3.color = %data.colors[ 3 ];*/
	
	IPSP_Editor-->sizes0_slider.setValue( %data.sizes[ 0 ] );
	IPSP_Editor-->sizes0_edit.setText( %data.sizes[ 0 ] );
	IPSP_Editor-->sizes1_slider.setValue( %data.sizes[ 1 ] );
	IPSP_Editor-->sizes1_edit.setText( %data.sizes[ 1 ] );
	IPSP_Editor-->sizes2_slider.setValue( %data.sizes[ 2 ] );
	IPSP_Editor-->sizes2_edit.setText( %data.sizes[ 2 ] );
	IPSP_Editor-->sizes3_slider.setValue( %data.sizes[ 3 ] );
	IPSP_Editor-->sizes3_edit.setText( %data.sizes[ 3 ] );
	IPSP_Editor-->times0_slider.setValue( %data.times[ 0 ] );
	IPSP_Editor-->times0_edit.setText( %data.times[ 0 ] );
	IPSP_Editor-->times1_slider.setValue( %data.times[ 1 ] );
	IPSP_Editor-->times1_edit.setText( %data.times[ 1 ] );
	IPSP_Editor-->times2_slider.setValue( %data.times[ 2 ] );
	IPSP_Editor-->times2_edit.setText( %data.times[ 2 ] );
	IPSP_Editor-->times3_slider.setValue( %data.times[ 3 ] );
	IPSP_Editor-->times3_edit.setText( %data.times[ 3 ] );
}


//=============================================================================================
//    IPS_ColorTintSwatch.
//=============================================================================================
function IPS_ParticleColorPicker::ColorPicked( %this,%color ) {
	devLog("IPS_ParticleColorPicker::ColorPicked",%color);
	%this.baseColor = %color;
	%id = %this.internalName;
	IPSP_Editor.updateParticle( "colors[" @ %id @ "]", %color );	
	
}
function IPS_ParticleColorPicker::ColorUpdated( %this,%color ) {
	devLog("IPS_ParticleColorPicker::ColorUpdated",%color);
}


/*
function IPS_ColorTintSwatch::updateParticleColor( %this, %color ) {
	devLog("IPS_ColorTintSwatch::updateParticleColor( %this, %color )",%color);
	%arrayNum = %this.arrayNum;
	%r = getWord( %color, 0 );
	%g = getWord( %color, 1 );
	%b = getWord( %color, 2 );
	%a = getWord( %color, 3 );
	%color = %r SPC %g SPC %b SPC %a;
	%this.color = %color;
	IPSP_Editor.updateParticle( "colors[" @ %arrayNum @ "]", %color );
}
*/
//=============================================================================================
//    IPSP_Selector_Control.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function IPSP_Selector_Control::onRenameItem( %this ) {
	Parent::onRenameItem( %this );
	//FIXME: need to check for validity of name and name clashes
	IPSP_Editor.setParticleDirty();
	// Resort menu.
	%this-->PopupMenu.sort();
}

//=============================================================================================
//    IPSP_NewParticleButton.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function IPSP_NewParticleButton::onDefaultClick( %this ) {
	IPSP_Editor.showNewDialog();
}

//---------------------------------------------------------------------------------------------

function IPSP_NewParticleButton::onCtrlClick( %this ) {
	for( %i = 1; %i < 5; %i ++ ) {
		%popup = "IPSE_EmitterParticleSelector" @ %i;

		if( %popup.getSelected() == IPSP_Selector.getSelected() ) {
			%replaceSlot = %i;
			break;
		}
	}

	IPSP_Editor.showNewDialog( %replaceSlot );
}
