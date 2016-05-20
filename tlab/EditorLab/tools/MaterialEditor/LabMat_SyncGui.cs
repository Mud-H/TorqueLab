//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================

function LabMat::guiSync( %this, %material ) {
	%this.preventUndo = true;

	//Setup our headers
	if( LabMat.currentMode $= "material" ) {
		MatEdMaterialMode-->selMaterialName.setText(LabMat.currentMaterial.name);
		MatEdMaterialMode-->selMaterialMapTo.setText(LabMat.currentMaterial.mapTo);
	} else {
		if( LabMat.currentObject.isMethod("getModelFile") ) {
			%sourcePath = LabMat.currentObject.getModelFile();

			if( %sourcePath !$= "" ) {
				MatEdTargetMode-->selMaterialMapTo.ToolTip = %sourcePath;
				%sourceName = fileName(%sourcePath);
				MatEdTargetMode-->selMaterialMapTo.setText(%sourceName);
				MatEdTargetMode-->selMaterialName.setText(LabMat.currentMaterial.name);
			}
		} else {
			%info = LabMat.currentObject.getClassName();
			MatEdTargetMode-->selMaterialMapTo.ToolTip = %info;
			MatEdTargetMode-->selMaterialMapTo.setText(%info);
			MatEdTargetMode-->selMaterialName.setText(LabMat.currentMaterial.name);
		}
	}

   LabMat_ActiveCtrl-->activeMatName.text = LabMat.currentMaterial.name;
	LabMat_PropertiesStack-->alphaRefTextEdit.setText((%material).alphaRef);
	LabMat_PropertiesStack-->alphaRefSlider.setValue((%material).alphaRef);
	LabMat_PropertiesStack-->doubleSidedCheckBox.setValue((%material).doubleSided);
	LabMat_PropertiesStack-->transZWriteCheckBox.setValue((%material).translucentZWrite);
	LabMat_PropertiesStack-->alphaTestCheckBox.setValue((%material).alphaTest);
	LabMat_PropertiesStack-->castShadows.setValue((%material).castShadows);
	LabMat_PropertiesStack-->castDynamicShadows.setValue((%material).castDynamicShadows);//PBR Scripts
	LabMat_PropertiesStack-->translucentCheckbox.setValue((%material).translucent);

	switch$((%material).translucentBlendOp) {
	case "None":
		%selectedNum = 0;

	case "Mul":
		%selectedNum = 1;

	case "Add":
		%selectedNum = 2;

	case "AddAlpha":
		%selectedNum = 3;

	case "Sub":
		%selectedNum = 4;

	case "LerpAlpha":
		%selectedNum = 5;
	}

	LabMat_PropertiesStack-->blendingTypePopUp.setSelected(%selectedNum);

	if((%material).cubemap !$= "") {
		LabMat_PropertiesStack-->matEd_cubemapEditBtn.setVisible(1);
		LabMat_PropertiesStack-->reflectionTypePopUp.setSelected(1);
	} else if((%material).dynamiccubemap) {
		LabMat_PropertiesStack-->matEd_cubemapEditBtn.setVisible(0);
		LabMat_PropertiesStack-->reflectionTypePopUp.setSelected(2);
	} else if((%material).planarReflection) {
		LabMat_PropertiesStack-->matEd_cubemapEditBtn.setVisible(0);
		LabMat_PropertiesStack-->reflectionTypePopUp.setSelected(3);
	} else {
		LabMat_PropertiesStack-->matEd_cubemapEditBtn.setVisible(0);
		LabMat_PropertiesStack-->reflectionTypePopUp.setSelected(0);
	}

	LabMat_PropertiesStack-->effectColor0Swatch.color = (%material).effectColor[0];
	LabMat_PropertiesStack-->effectColor1Swatch.color = (%material).effectColor[1];
	LabMat_PropertiesStack-->showFootprintsCheckbox.setValue((%material).showFootprints);
	LabMat_PropertiesStack-->showDustCheckbox.setValue((%material).showDust);
	LabMat.updateSoundPopup("Footstep", (%material).footstepSoundId, (%material).customFootstepSound);
	LabMat.updateSoundPopup("Impact", (%material).impactSoundId, (%material).customImpactSound);
	//layer specific controls are located here
	%layer = LabMat.currentLayer;

	if((%material).diffuseMap[%layer] $= "") {
		LabMat_MapsStack-->diffuseMapNameText.setText( "None" );
		LabMat_MapsStack-->diffuseMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
		LabMat_MapsStack-->diffuseFileNameText.setText( "" );
	} else {
		LabMat_MapsStack-->diffuseMapNameText.setText( (%material).diffuseMap[%layer] );
		LabMat_MapsStack-->diffuseFileNameText.setText( fileBase((%material).diffuseMap[%layer]) );
		LabMat_MapsStack-->diffuseMapDisplayBitmap.setBitmap( (%material).diffuseMap[%layer] );
	}

	if((%material).normalMap[%layer] $= "") {
		LabMat_MapsStack-->normalMapNameText.setText( "None" );
		LabMat_MapsStack-->normalMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
		LabMat_MapsStack-->normalFileNameText.setText( "" );
	} else {
		LabMat_MapsStack-->normalMapNameText.setText( (%material).normalMap[%layer] );
		LabMat_MapsStack-->normalMapDisplayBitmap.setBitmap( (%material).normalMap[%layer] );
		LabMat_MapsStack-->normalFileNameText.setText( fileBase((%material).normalMap[%layer]) );
	}

	if((%material).overlayMap[%layer] $= "") {
		LabMat_MapsStack-->overlayMapNameText.setText( "None" );
		LabMat_MapsStack-->overlayMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		LabMat_MapsStack-->overlayMapNameText.setText( (%material).overlayMap[%layer] );
		LabMat_MapsStack-->overlayMapDisplayBitmap.setBitmap( (%material).overlayMap[%layer] );
	}

	if((%material).detailMap[%layer] $= "") {
		LabMat_MapsStack-->detailMapNameText.setText( "None" );
		LabMat_MapsStack-->detailMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		LabMat_MapsStack-->detailMapNameText.setText( (%material).detailMap[%layer] );
		LabMat_MapsStack-->detailMapDisplayBitmap.setBitmap( (%material).detailMap[%layer] );
	}

	if((%material).detailNormalMap[%layer] $= "") {
		LabMat_MapsStack-->detailNormalMapNameText.setText( "None" );
		LabMat_MapsStack-->detailNormalMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		LabMat_MapsStack-->detailNormalMapNameText.setText( (%material).detailNormalMap[%layer] );
		LabMat_MapsStack-->detailNormalMapDisplayBitmap.setBitmap( (%material).detailNormalMap[%layer] );
	}

	if((%material).lightMap[%layer] $= "") {
		LabMat_MapsStack-->lightMapNameText.setText( "None" );
		LabMat_MapsStack-->lightMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		LabMat_MapsStack-->lightMapNameText.setText( (%material).lightMap[%layer] );
		LabMat_MapsStack-->lightMapDisplayBitmap.setBitmap( (%material).lightMap[%layer] );
	}

	if((%material).toneMap[%layer] $= "") {
		LabMat_MapsStack-->toneMapNameText.setText( "None" );
		LabMat_MapsStack-->toneMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		LabMat_MapsStack-->toneMapNameText.setText( (%material).toneMap[%layer] );
		LabMat_MapsStack-->toneMapDisplayBitmap.setBitmap( (%material).toneMap[%layer] );
	}

	//PBR Scripts
	LabMat_MapsStack-->FlipRBCheckbox.setValue((%material).FlipRB[%layer]);
	LabMat_MapsStack-->invertSmoothnessCheckbox.setValue((%material).invertSmoothness[%layer]);

	if((%material).specularMap[%layer] $= "") {
		LabMat_MapsStack-->specularMapNameText.setText( "None" );
		LabMat_MapsStack-->specularMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
		LabMat_MapsStack-->specularFileNameText.setText( "" );
		LabMat_MapsStack-->compMapNameText.setText( "None" );
		LabMat_MapsStack-->compMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		LabMat_MapsStack-->specularMapNameText.setText( (%material).specularMap[%layer] );
		LabMat_MapsStack-->specularMapDisplayBitmap.setBitmap( (%material).specularMap[%layer] );
		LabMat_MapsStack-->specularFileNameText.setText( fileBase((%material).specularMap[%layer]) );
		LabMat_MapsStack-->compMapNameText.setText( (%material).specularMap[%layer] );
		LabMat_MapsStack-->compMapDisplayBitmap.setBitmap( (%material).specularMap[%layer] );
	}

	//PBR Script
	if((%material).roughMap[%layer] $= "") {
		LabMat_MapsStack-->roughMapNameText.setText( "None" );
		LabMat_MapsStack-->roughMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		LabMat_MapsStack-->roughMapNameText.setText( (%material).roughMap[%layer] );
		LabMat_MapsStack-->roughMapDisplayBitmap.setBitmap( (%material).roughMap[%layer] );
	}

	if((%material).aoMap[%layer] $= "") {
		LabMat_MapsStack-->aoMapNameText.setText( "None" );
		LabMat_MapsStack-->aoMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		LabMat_MapsStack-->aoMapNameText.setText( (%material).aoMap[%layer] );
		LabMat_MapsStack-->aoMapDisplayBitmap.setBitmap( (%material).aoMap[%layer] );
	}

	if((%material).metalMap[%layer] $= "") {
		LabMat_MapsStack-->metalMapNameText.setText( "None" );
		LabMat_MapsStack-->metalMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		LabMat_MapsStack-->metalMapNameText.setText( (%material).metalMap[%layer] );
		LabMat_MapsStack-->metalMapDisplayBitmap.setBitmap( (%material).metalMap[%layer] );
	}

	// material damage

	if((%material).albedoDamageMap[%layer] $= "") {
		LabMat_MapsStack-->albedoDamageMapNameText.setText( "None" );
		LabMat_MapsStack-->albedoDamageMapDisplayBitmap.setBitmap( $MEP_NoTextureImage);
	} else {
		LabMat_MapsStack-->albedoDamageMapNameText.setText( (%material).albedoDamageMap[%layer] );
		LabMat_MapsStack-->albedoDamageMapDisplayBitmap.setBitmap( (%material).albedoDamageMap[%layer] );
	}

	if((%material).normalDamageMap[%layer] $= "") {
		LabMat_MapsStack-->normalDamageMapNameText.setText( "None" );
		LabMat_MapsStack-->normalDamageMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		LabMat_MapsStack-->normalDamageMapNameText.setText( (%material).normalDamageMap[%layer] );
		LabMat_MapsStack-->normalDamageMapDisplayBitmap.setBitmap( (%material).normalDamageMap[%layer] );
	}

	if((%material).compositeDamageMap[%layer] $= "") {
		LabMat_MapsStack-->compositeDamageMapNameText.setText( "None" );
		LabMat_MapsStack-->compositeDamageMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		LabMat_MapsStack-->compositeDamageMapNameText.setText( (%material).normalDamageMap[%layer] );
		LabMat_MapsStack-->compositeDamageMapDisplayBitmap.setBitmap( (%material).compositeDamageMap[%layer] );
	}

	LabMat_MapsStack-->minDamageTextEdit.setText((%material).minDamage[%layer]);
	LabMat_MapsStack-->minDamageSlider.setValue((%material).minDamage[%layer]);
	//PBR Script End
	//LabMat_PropertiesStack-->detailScaleTextEdit.setText( getWord((%material).detailScale[%layer], 0) );
	//LabMat_PropertiesStack-->detailNormalStrengthTextEdit.setText( getWord((%material).detailNormalMapStrength[%layer], 0) );
	LabMat_MapsStack-->colorTintSwatch.color = (%material).diffuseColor[%layer];
	LabMat_MapsStack-->specularColorSwatch.color = (%material).specular[%layer];

	if (!MatEd.PBRenabled) {
		LabMat_MapsStack-->specularPowerTextEdit.setText((%material).specularPower[%layer]);
		LabMat_MapsStack-->specularPowerSlider.setValue((%material).specularPower[%layer]);
		LabMat_MapsStack-->specularStrengthTextEdit.setText((%material).specularStrength[%layer]);
		LabMat_MapsStack-->specularStrengthSlider.setValue((%material).specularStrength[%layer]);
		LabMat_MapsStack-->pixelSpecularCheckbox.setValue((%material).pixelSpecular[%layer]);
	} else {
		//PBR Script
		LabMat_MapsStack-->SmoothnessTextEdit.setText((%material).Smoothness[%layer]);
		LabMat_MapsStack-->SmoothnessSlider.setValue((%material).Smoothness[%layer]);
		LabMat_MapsStack-->MetalnessTextEdit.setText((%material).Metalness[%layer]);
		LabMat_MapsStack-->MetalnessSlider.setValue((%material).Metalness[%layer]);
		//PBR Script End
	}

	LabMat_PropertiesStack-->glowCheckbox.setValue((%material).glow[%layer]);
	LabMat_PropertiesStack-->emissiveCheckbox.setValue((%material).emissive[%layer]);
	LabMat_PropertiesStack-->parallaxTextEdit.setText((%material).parallaxScale[%layer]);
	LabMat_PropertiesStack-->parallaxTextEdit.setText((%material).parallaxScale[%layer]);
	LabMat_PropertiesStack-->parallaxSlider.setValue((%material).parallaxScale[%layer]);
	LabMat_PropertiesStack-->useAnisoCheckbox.setValue((%material).useAnisotropic[%layer]);
	LabMat_PropertiesStack-->vertLitCheckbox.setValue((%material).vertLit[%layer]);
	LabMat_PropertiesStack-->vertColorSwatch.color = (%material).vertColor[%layer];
	LabMat_PropertiesStack-->subSurfaceCheckbox.setValue((%material).subSurface[%layer]);
	LabMat_PropertiesStack-->subSurfaceColorSwatch.color = (%material).subSurfaceColor[%layer];
	LabMat_PropertiesStack-->subSurfaceRolloffTextEdit.setText((%material).subSurfaceRolloff[%layer]);
	LabMat_PropertiesStack-->minnaertTextEdit.setText((%material).minnaertConstant[%layer]);
	// Animation properties
	LabMat_PropertiesStack-->RotationAnimation.setValue(0);
	LabMat_PropertiesStack-->ScrollAnimation.setValue(0);
	LabMat_PropertiesStack-->WaveAnimation.setValue(0);
	LabMat_PropertiesStack-->ScaleAnimation.setValue(0);
	LabMat_PropertiesStack-->SequenceAnimation.setValue(0);
	%flags = (%material).getAnimFlags(%layer);
	%wordCount = getWordCount( %flags );

	for(%i = 0; %i != %wordCount; %i++) {
		switch$(getWord( %flags, %i)) {
		case "$rotate":
			LabMat_PropertiesStack-->RotationAnimation.setValue(1);

		case "$scroll":
			LabMat_PropertiesStack-->ScrollAnimation.setValue(1);

		case "$wave":
			LabMat_PropertiesStack-->WaveAnimation.setValue(1);

		case "$scale":
			LabMat_PropertiesStack-->ScaleAnimation.setValue(1);

		case "$sequence":
			LabMat_PropertiesStack-->SequenceAnimation.setValue(1);
		}
	}

	LabMat_PropertiesStack-->RotationTextEditU.setText( getWord((%material).rotPivotOffset[%layer], 0) );
	LabMat_PropertiesStack-->RotationTextEditV.setText( getWord((%material).rotPivotOffset[%layer], 1) );
	LabMat_PropertiesStack-->RotationSpeedTextEdit.setText( (%material).rotSpeed[%layer] );
	LabMat_PropertiesStack-->RotationSliderU.setValue( getWord((%material).rotPivotOffset[%layer], 0) );
	LabMat_PropertiesStack-->RotationSliderV.setValue( getWord((%material).rotPivotOffset[%layer], 1) );
	LabMat_PropertiesStack-->RotationSpeedSlider.setValue( (%material).rotSpeed[%layer] );
	LabMat_PropertiesStack-->RotationCrosshair.setPosition( 45*mAbs(getWord((%material).rotPivotOffset[%layer], 0))-2, 45*mAbs(getWord((%material).rotPivotOffset[%layer], 1))-2 );
	LabMat_PropertiesStack-->ScrollTextEditU.setText( getWord((%material).scrollDir[%layer], 0) );
	LabMat_PropertiesStack-->ScrollTextEditV.setText( getWord((%material).scrollDir[%layer], 1) );
	LabMat_PropertiesStack-->ScrollSpeedTextEdit.setText( (%material).scrollSpeed[%layer] );
	LabMat_PropertiesStack-->ScrollSliderU.setValue( getWord((%material).scrollDir[%layer], 0) );
	LabMat_PropertiesStack-->ScrollSliderV.setValue( getWord((%material).scrollDir[%layer], 1) );
	LabMat_PropertiesStack-->ScrollSpeedSlider.setValue( (%material).scrollSpeed[%layer] );
	LabMat_PropertiesStack-->ScrollCrosshair.setPosition( -(23 * getWord((%material).scrollDir[%layer], 0))+20, -(23 * getWord((%material).scrollDir[%layer], 1))+20);
	%waveType = (%material).waveType[%layer];

	for( %radioButton = 0; %radioButton < LabMat_PropertiesStack-->WaveButtonContainer.getCount(); %radioButton++ ) {
		if( %waveType $= LabMat_PropertiesStack-->WaveButtonContainer.getObject(%radioButton).waveType )
			LabMat_PropertiesStack-->WaveButtonContainer.getObject(%radioButton).setStateOn(1);
	}

	LabMat_PropertiesStack-->WaveTextEditAmp.setText( (%material).waveAmp[%layer] );
	LabMat_PropertiesStack-->WaveTextEditFreq.setText( (%material).waveFreq[%layer] );
	LabMat_PropertiesStack-->WaveSliderAmp.setValue( (%material).waveAmp[%layer] );
	LabMat_PropertiesStack-->WaveSliderFreq.setValue( (%material).waveFreq[%layer] );
	%numFrames = mRound( 1 / (%material).sequenceSegmentSize[%layer] );
	LabMat_PropertiesStack-->SequenceTextEditFPS.setText( (%material).sequenceFramePerSec[%layer] );
	LabMat_PropertiesStack-->SequenceTextEditSSS.setText( %numFrames );
	LabMat_PropertiesStack-->SequenceSliderFPS.setValue( (%material).sequenceFramePerSec[%layer] );
	LabMat_PropertiesStack-->SequenceSliderSSS.setValue( %numFrames );
	// Accumulation PBR Script
	LabMat_PropertiesStack-->accuCheckbox.setValue((%material).accuEnabled[%layer]);
	LabMat_PropertiesStack-->accuCheckbox.setValue((%material).accuEnabled[%layer]);
	//TODO: This is wrong, need to check back with PBR branch editor to figure why
	//%this.getRoughChan((%material).SmoothnessChan[%layer]);
	//%this.getAOChan((%material).AOChan[%layer]);
	//%this.getMetalChan((%material).metalChan[%layer]);
	//ENDTODO
	//PBR Script End
	%this.preventUndo = false;
}

//------------------------------------------------------------------------------