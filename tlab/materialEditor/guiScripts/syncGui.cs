//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================

function MaterialEditorTools::guiSync( %this, %material ) {
	%this.preventUndo = true;

	//Setup our headers
	if( MaterialEditorTools.currentMode $= "material" ) {
		MatEdMaterialMode-->selMaterialName.setText(MaterialEditorTools.currentMaterial.name);
		MatEdMaterialMode-->selMaterialMapTo.setText(MaterialEditorTools.currentMaterial.mapTo);
	} else {
		if( MaterialEditorTools.currentObject.isMethod("getModelFile") ) {
			%sourcePath = MaterialEditorTools.currentObject.getModelFile();

			if( %sourcePath !$= "" ) {
				MatEdTargetMode-->selMaterialMapTo.ToolTip = %sourcePath;
				%sourceName = fileName(%sourcePath);
				MatEdTargetMode-->selMaterialMapTo.setText(%sourceName);
				MatEdTargetMode-->selMaterialName.setText(MaterialEditorTools.currentMaterial.name);
			}
		} else {
			%info = MaterialEditorTools.currentObject.getClassName();
			MatEdTargetMode-->selMaterialMapTo.ToolTip = %info;
			MatEdTargetMode-->selMaterialMapTo.setText(%info);
			MatEdTargetMode-->selMaterialName.setText(MaterialEditorTools.currentMaterial.name);
		}
	}

	MaterialEditorPropertiesWindow-->alphaRefTextEdit.setText((%material).alphaRef);
	MaterialEditorPropertiesWindow-->alphaRefSlider.setValue((%material).alphaRef);
	MaterialEditorPropertiesWindow-->doubleSidedCheckBox.setValue((%material).doubleSided);
	MaterialEditorPropertiesWindow-->transZWriteCheckBox.setValue((%material).translucentZWrite);
	MaterialEditorPropertiesWindow-->alphaTestCheckBox.setValue((%material).alphaTest);
	MaterialEditorPropertiesWindow-->castShadows.setValue((%material).castShadows);
	MaterialEditorPropertiesWindow-->castDynamicShadows.setValue((%material).castDynamicShadows);//PBR Scripts
	MaterialEditorPropertiesWindow-->translucentCheckbox.setValue((%material).translucent);

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

	MaterialEditorPropertiesWindow-->blendingTypePopUp.setSelected(%selectedNum);

	if((%material).cubemap !$= "") {
		MaterialEditorPropertiesWindow-->matEd_cubemapEditBtn.setVisible(1);
		MaterialEditorPropertiesWindow-->reflectionTypePopUp.setSelected(1);
	} else if((%material).dynamiccubemap) {
		MaterialEditorPropertiesWindow-->matEd_cubemapEditBtn.setVisible(0);
		MaterialEditorPropertiesWindow-->reflectionTypePopUp.setSelected(2);
	} else if((%material).planarReflection) {
		MaterialEditorPropertiesWindow-->matEd_cubemapEditBtn.setVisible(0);
		MaterialEditorPropertiesWindow-->reflectionTypePopUp.setSelected(3);
	} else {
		MaterialEditorPropertiesWindow-->matEd_cubemapEditBtn.setVisible(0);
		MaterialEditorPropertiesWindow-->reflectionTypePopUp.setSelected(0);
	}

	MaterialEditorPropertiesWindow-->effectColor0Swatch.color = (%material).effectColor[0];
	MaterialEditorPropertiesWindow-->effectColor1Swatch.color = (%material).effectColor[1];
	MaterialEditorPropertiesWindow-->showFootprintsCheckbox.setValue((%material).showFootprints);
	MaterialEditorPropertiesWindow-->showDustCheckbox.setValue((%material).showDust);
	MaterialEditorTools.updateSoundPopup("Footstep", (%material).footstepSoundId, (%material).customFootstepSound);
	MaterialEditorTools.updateSoundPopup("Impact", (%material).impactSoundId, (%material).customImpactSound);
	//layer specific controls are located here
	%layer = MaterialEditorTools.currentLayer;

	if((%material).diffuseMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->diffuseMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->diffuseMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
		MaterialEditorPropertiesWindow-->diffuseFileNameText.setText( "" );
	} else {
		MaterialEditorPropertiesWindow-->diffuseMapNameText.setText( (%material).diffuseMap[%layer] );
		MaterialEditorPropertiesWindow-->diffuseFileNameText.setText( fileBase((%material).diffuseMap[%layer]) );
		MaterialEditorPropertiesWindow-->diffuseMapDisplayBitmap.setBitmap( (%material).diffuseMap[%layer] );
	}

	if((%material).normalMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->normalMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->normalMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
		MaterialEditorPropertiesWindow-->normalFileNameText.setText( "" );
	} else {
		MaterialEditorPropertiesWindow-->normalMapNameText.setText( (%material).normalMap[%layer] );
		MaterialEditorPropertiesWindow-->normalMapDisplayBitmap.setBitmap( (%material).normalMap[%layer] );
		MaterialEditorPropertiesWindow-->normalFileNameText.setText( fileBase((%material).normalMap[%layer]) );
	}

	if((%material).overlayMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->overlayMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->overlayMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		MaterialEditorPropertiesWindow-->overlayMapNameText.setText( (%material).overlayMap[%layer] );
		MaterialEditorPropertiesWindow-->overlayMapDisplayBitmap.setBitmap( (%material).overlayMap[%layer] );
	}

	if((%material).detailMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->detailMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->detailMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		MaterialEditorPropertiesWindow-->detailMapNameText.setText( (%material).detailMap[%layer] );
		MaterialEditorPropertiesWindow-->detailMapDisplayBitmap.setBitmap( (%material).detailMap[%layer] );
	}

	if((%material).detailNormalMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->detailNormalMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->detailNormalMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		MaterialEditorPropertiesWindow-->detailNormalMapNameText.setText( (%material).detailNormalMap[%layer] );
		MaterialEditorPropertiesWindow-->detailNormalMapDisplayBitmap.setBitmap( (%material).detailNormalMap[%layer] );
	}

	if((%material).lightMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->lightMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->lightMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		MaterialEditorPropertiesWindow-->lightMapNameText.setText( (%material).lightMap[%layer] );
		MaterialEditorPropertiesWindow-->lightMapDisplayBitmap.setBitmap( (%material).lightMap[%layer] );
	}

	if((%material).toneMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->toneMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->toneMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		MaterialEditorPropertiesWindow-->toneMapNameText.setText( (%material).toneMap[%layer] );
		MaterialEditorPropertiesWindow-->toneMapDisplayBitmap.setBitmap( (%material).toneMap[%layer] );
	}

	//PBR Scripts
	MaterialEditorPropertiesWindow-->FlipRBCheckbox.setValue((%material).FlipRB[%layer]);
	MaterialEditorPropertiesWindow-->invertSmoothnessCheckbox.setValue((%material).invertSmoothness[%layer]);

	if((%material).specularMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->specularMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->specularMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
		MaterialEditorPropertiesWindow-->specularFileNameText.setText( "" );
		MaterialEditorPropertiesWindow-->compMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->compMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		MaterialEditorPropertiesWindow-->specularMapNameText.setText( (%material).specularMap[%layer] );
		MaterialEditorPropertiesWindow-->specularMapDisplayBitmap.setBitmap( (%material).specularMap[%layer] );
		MaterialEditorPropertiesWindow-->specularFileNameText.setText( fileBase((%material).specularMap[%layer]) );
		MaterialEditorPropertiesWindow-->compMapNameText.setText( (%material).specularMap[%layer] );
		MaterialEditorPropertiesWindow-->compMapDisplayBitmap.setBitmap( (%material).specularMap[%layer] );
	}

	//PBR Script
	if((%material).roughMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->roughMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->roughMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		MaterialEditorPropertiesWindow-->roughMapNameText.setText( (%material).roughMap[%layer] );
		MaterialEditorPropertiesWindow-->roughMapDisplayBitmap.setBitmap( (%material).roughMap[%layer] );
	}

	if((%material).aoMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->aoMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->aoMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		MaterialEditorPropertiesWindow-->aoMapNameText.setText( (%material).aoMap[%layer] );
		MaterialEditorPropertiesWindow-->aoMapDisplayBitmap.setBitmap( (%material).aoMap[%layer] );
	}

	if((%material).metalMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->metalMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->metalMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		MaterialEditorPropertiesWindow-->metalMapNameText.setText( (%material).metalMap[%layer] );
		MaterialEditorPropertiesWindow-->metalMapDisplayBitmap.setBitmap( (%material).metalMap[%layer] );
	}

	// material damage

	if((%material).albedoDamageMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->albedoDamageMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->albedoDamageMapDisplayBitmap.setBitmap( $MEP_NoTextureImage);
	} else {
		MaterialEditorPropertiesWindow-->albedoDamageMapNameText.setText( (%material).albedoDamageMap[%layer] );
		MaterialEditorPropertiesWindow-->albedoDamageMapDisplayBitmap.setBitmap( (%material).albedoDamageMap[%layer] );
	}

	if((%material).normalDamageMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->normalDamageMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->normalDamageMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		MaterialEditorPropertiesWindow-->normalDamageMapNameText.setText( (%material).normalDamageMap[%layer] );
		MaterialEditorPropertiesWindow-->normalDamageMapDisplayBitmap.setBitmap( (%material).normalDamageMap[%layer] );
	}

	if((%material).compositeDamageMap[%layer] $= "") {
		MaterialEditorPropertiesWindow-->compositeDamageMapNameText.setText( "None" );
		MaterialEditorPropertiesWindow-->compositeDamageMapDisplayBitmap.setBitmap( $MEP_NoTextureImage );
	} else {
		MaterialEditorPropertiesWindow-->compositeDamageMapNameText.setText( (%material).normalDamageMap[%layer] );
		MaterialEditorPropertiesWindow-->compositeDamageMapDisplayBitmap.setBitmap( (%material).compositeDamageMap[%layer] );
	}

	MaterialEditorPropertiesWindow-->minDamageTextEdit.setText((%material).minDamage[%layer]);
	MaterialEditorPropertiesWindow-->minDamageSlider.setValue((%material).minDamage[%layer]);
	//PBR Script End
	//MaterialEditorPropertiesWindow-->detailScaleTextEdit.setText( getWord((%material).detailScale[%layer], 0) );
	//MaterialEditorPropertiesWindow-->detailNormalStrengthTextEdit.setText( getWord((%material).detailNormalMapStrength[%layer], 0) );
	MaterialEditorPropertiesWindow-->colorTintSwatch.color = (%material).diffuseColor[%layer];
	MaterialEditorPropertiesWindow-->specularColorSwatch.color = (%material).specular[%layer];

	if (!MatEd.PBRenabled) {
		MaterialEditorPropertiesWindow-->specularPowerTextEdit.setText((%material).specularPower[%layer]);
		MaterialEditorPropertiesWindow-->specularPowerSlider.setValue((%material).specularPower[%layer]);
		MaterialEditorPropertiesWindow-->specularStrengthTextEdit.setText((%material).specularStrength[%layer]);
		MaterialEditorPropertiesWindow-->specularStrengthSlider.setValue((%material).specularStrength[%layer]);
		MaterialEditorPropertiesWindow-->pixelSpecularCheckbox.setValue((%material).pixelSpecular[%layer]);
	} else {
		//PBR Script
		MaterialEditorPropertiesWindow-->SmoothnessTextEdit.setText((%material).Smoothness[%layer]);
		MaterialEditorPropertiesWindow-->SmoothnessSlider.setValue((%material).Smoothness[%layer]);
		MaterialEditorPropertiesWindow-->MetalnessTextEdit.setText((%material).Metalness[%layer]);
		MaterialEditorPropertiesWindow-->MetalnessSlider.setValue((%material).Metalness[%layer]);
		//PBR Script End
	}

	MaterialEditorPropertiesWindow-->glowCheckbox.setValue((%material).glow[%layer]);
	MaterialEditorPropertiesWindow-->emissiveCheckbox.setValue((%material).emissive[%layer]);
	MaterialEditorPropertiesWindow-->parallaxTextEdit.setText((%material).parallaxScale[%layer]);
	MaterialEditorPropertiesWindow-->parallaxTextEdit.setText((%material).parallaxScale[%layer]);
	MaterialEditorPropertiesWindow-->parallaxSlider.setValue((%material).parallaxScale[%layer]);
	MaterialEditorPropertiesWindow-->useAnisoCheckbox.setValue((%material).useAnisotropic[%layer]);
	MaterialEditorPropertiesWindow-->vertLitCheckbox.setValue((%material).vertLit[%layer]);
	MaterialEditorPropertiesWindow-->vertColorSwatch.color = (%material).vertColor[%layer];
	MaterialEditorPropertiesWindow-->subSurfaceCheckbox.setValue((%material).subSurface[%layer]);
	MaterialEditorPropertiesWindow-->subSurfaceColorSwatch.color = (%material).subSurfaceColor[%layer];
	MaterialEditorPropertiesWindow-->subSurfaceRolloffTextEdit.setText((%material).subSurfaceRolloff[%layer]);
	MaterialEditorPropertiesWindow-->minnaertTextEdit.setText((%material).minnaertConstant[%layer]);
	// Animation properties
	MaterialEditorPropertiesWindow-->RotationAnimation.setValue(0);
	MaterialEditorPropertiesWindow-->ScrollAnimation.setValue(0);
	MaterialEditorPropertiesWindow-->WaveAnimation.setValue(0);
	MaterialEditorPropertiesWindow-->ScaleAnimation.setValue(0);
	MaterialEditorPropertiesWindow-->SequenceAnimation.setValue(0);
	%flags = (%material).getAnimFlags(%layer);
	%wordCount = getWordCount( %flags );

	for(%i = 0; %i != %wordCount; %i++) {
		switch$(getWord( %flags, %i)) {
		case "$rotate":
			MaterialEditorPropertiesWindow-->RotationAnimation.setValue(1);

		case "$scroll":
			MaterialEditorPropertiesWindow-->ScrollAnimation.setValue(1);

		case "$wave":
			MaterialEditorPropertiesWindow-->WaveAnimation.setValue(1);

		case "$scale":
			MaterialEditorPropertiesWindow-->ScaleAnimation.setValue(1);

		case "$sequence":
			MaterialEditorPropertiesWindow-->SequenceAnimation.setValue(1);
		}
	}

	MaterialEditorPropertiesWindow-->RotationTextEditU.setText( getWord((%material).rotPivotOffset[%layer], 0) );
	MaterialEditorPropertiesWindow-->RotationTextEditV.setText( getWord((%material).rotPivotOffset[%layer], 1) );
	MaterialEditorPropertiesWindow-->RotationSpeedTextEdit.setText( (%material).rotSpeed[%layer] );
	MaterialEditorPropertiesWindow-->RotationSliderU.setValue( getWord((%material).rotPivotOffset[%layer], 0) );
	MaterialEditorPropertiesWindow-->RotationSliderV.setValue( getWord((%material).rotPivotOffset[%layer], 1) );
	MaterialEditorPropertiesWindow-->RotationSpeedSlider.setValue( (%material).rotSpeed[%layer] );
	MaterialEditorPropertiesWindow-->RotationCrosshair.setPosition( 45*mAbs(getWord((%material).rotPivotOffset[%layer], 0))-2, 45*mAbs(getWord((%material).rotPivotOffset[%layer], 1))-2 );
	MaterialEditorPropertiesWindow-->ScrollTextEditU.setText( getWord((%material).scrollDir[%layer], 0) );
	MaterialEditorPropertiesWindow-->ScrollTextEditV.setText( getWord((%material).scrollDir[%layer], 1) );
	MaterialEditorPropertiesWindow-->ScrollSpeedTextEdit.setText( (%material).scrollSpeed[%layer] );
	MaterialEditorPropertiesWindow-->ScrollSliderU.setValue( getWord((%material).scrollDir[%layer], 0) );
	MaterialEditorPropertiesWindow-->ScrollSliderV.setValue( getWord((%material).scrollDir[%layer], 1) );
	MaterialEditorPropertiesWindow-->ScrollSpeedSlider.setValue( (%material).scrollSpeed[%layer] );
	MaterialEditorPropertiesWindow-->ScrollCrosshair.setPosition( -(23 * getWord((%material).scrollDir[%layer], 0))+20, -(23 * getWord((%material).scrollDir[%layer], 1))+20);
	%waveType = (%material).waveType[%layer];

	for( %radioButton = 0; %radioButton < MaterialEditorPropertiesWindow-->WaveButtonContainer.getCount(); %radioButton++ ) {
		if( %waveType $= MaterialEditorPropertiesWindow-->WaveButtonContainer.getObject(%radioButton).waveType )
			MaterialEditorPropertiesWindow-->WaveButtonContainer.getObject(%radioButton).setStateOn(1);
	}

	MaterialEditorPropertiesWindow-->WaveTextEditAmp.setText( (%material).waveAmp[%layer] );
	MaterialEditorPropertiesWindow-->WaveTextEditFreq.setText( (%material).waveFreq[%layer] );
	MaterialEditorPropertiesWindow-->WaveSliderAmp.setValue( (%material).waveAmp[%layer] );
	MaterialEditorPropertiesWindow-->WaveSliderFreq.setValue( (%material).waveFreq[%layer] );
	%numFrames = mRound( 1 / (%material).sequenceSegmentSize[%layer] );
	MaterialEditorPropertiesWindow-->SequenceTextEditFPS.setText( (%material).sequenceFramePerSec[%layer] );
	MaterialEditorPropertiesWindow-->SequenceTextEditSSS.setText( %numFrames );
	MaterialEditorPropertiesWindow-->SequenceSliderFPS.setValue( (%material).sequenceFramePerSec[%layer] );
	MaterialEditorPropertiesWindow-->SequenceSliderSSS.setValue( %numFrames );
	// Accumulation PBR Script
	MaterialEditorPropertiesWindow-->accuCheckbox.setValue((%material).accuEnabled[%layer]);
	MaterialEditorPropertiesWindow-->accuCheckbox.setValue((%material).accuEnabled[%layer]);
	//TODO: This is wrong, need to check back with PBR branch editor to figure why
	//%this.getRoughChan((%material).SmoothnessChan[%layer]);
	//%this.getAOChan((%material).AOChan[%layer]);
	//%this.getMetalChan((%material).metalChan[%layer]);
	//ENDTODO
	//PBR Script End
	%this.preventUndo = false;
}

//------------------------------------------------------------------------------

//==============================================================================
// Color Picker Helpers - They are all using colorPicker.ed.gui in order to function
// These functions are mainly passed callbacks from getColorI/getColorF callbacks

function MaterialEditorTools::syncGuiColor(%this, %guiCtrl, %propname, %color) {
	%layer = MaterialEditorTools.currentLayer;
	%r = getWord(%color,0);
	%g = getWord(%color,1);
	%b = getWord(%color,2);
	%a = getWord(%color,3);
	%colorSwatch = (%r SPC %g SPC %b SPC %a);
	%color = "\"" @ %r SPC %g SPC %b SPC %a @ "\"";
	%guiCtrl.color = %colorSwatch;
	
	MaterialEditorTools.updateActiveMaterial(%propName, %color);
}
//------------------------------------------------------------------------------
