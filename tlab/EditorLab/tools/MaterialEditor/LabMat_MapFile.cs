//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$MaterialEditor_AutoUpdateMap = true;
//==============================================================================
//Select a texture for a material map type
function LabMat::CheckAutoUpdateMap( %this,%type, %texture,%layer) {
	if (!$MaterialEditor_AutoUpdateMap)
		return;

	devLog("CheckAutoUpdateMap Type",%type,"Text",%texture);
	%fileName = fileBase(%texture);
	eval("%removeSuffix = SceneEditorCfg.DiffuseSuffix;");
	%fileName = strReplace(%fileName,%removeSuffix,"");
	%ext = fileExt(%texture);
	%folder = filePath(%texture)@"/";

	if($Cfg_MaterialEditor_AutoAddNormal && %type !$="Normal") {
		%suffix = $Cfg_MaterialEditor_NormalSuffix;
		%testFile = %folder @%fileName@%suffix@%ext ;

		if (isImageFile(%testFile)) {
			devLog("Normal found:",%testFile);
			%this.updateTextureMapImage("Normal",%testFile,%layer,true);
		} else {
			devLog("Normal NOT found:",%testFile);
		}
	}

	if(SceneEditorCfg.AutoAddDiffuse && %type !$="Diffuse") {
		%suffix = $Cfg_MaterialEditor_DiffuseSuffix;
		%testFile = %folder @%fileName@%suffix@%ext;

		if (isImageFile(%testFile))
			%this.updateTextureMapImage("Diffuse",%testFile,%layer,true);
	}

	if($Cfg_MaterialEditor_AutoAddSpecular && %type !$="Specular") {
		%suffix = $Cfg_MaterialEditor_SpecularSuffix;
		%testFile = %folder @%fileName@%suffix@%ext;
devLog("Specular Check:",%testFile);
		if (isImageFile(%testFile))
			%this.updateSpecMap(true,%testFile,true);
	}

	if($Cfg_MaterialEditor_AutoAddAO && %type !$="AO") {
		%suffix = $Cfg_MaterialEditor_AOSuffix;
		%testFile = %folder @%fileName@%suffix@%ext;
		devLog("AO Check:",%testFile);

		if (isImageFile(%testFile))
			%this.updateAoMap(true,%testFile,true);
	}

	if($Cfg_MaterialEditor_AutoAddSmoothness && %type !$="Smoothness") {
		%suffix = $Cfg_MaterialEditor_SmoothnessSuffix;
		%testFile = %folder @%fileName@%suffix@%ext;
		devLog("Smoothness Check:",%testFile);

		if (isImageFile(%testFile))
			%this.updateRoughMap(true,%testFile,true);
	}

	if($Cfg_MaterialEditor_AutoAddMetalness && %type !$="Metalness") {
		%suffix = $Cfg_MaterialEditor_MetalnessSuffix;
		%testFile = %folder @%fileName@%suffix@%ext;
		devLog("Metalness Check:",%testFile);

		if (isImageFile(%testFile))
			%this.updateMetalMap(true,%testFile,true);
	}

	if($Cfg_MaterialEditor_AutoAddComposite && %type !$="Composite") {
		%suffix = $Cfg_MaterialEditor_CompositeSuffix;
		%testFile = %folder @%fileName@%suffix@%ext;
		devLog("Composite Check:",%testFile);

		if (isImageFile(%testFile))
			%this.updateCompMap(true,%testFile,true);
	}
}

//------------------------------------------------------------------------------
$MEP_NoTextureImage = "tlab/materialEditor/assets/unavailable";
//==============================================================================
//Select a texture for a material map type
function LabMat::openFile( %this ) {
   %this.openMapFile();
}
function LabMat::openMapFile( %this, %defaultFileName ) {
	logc("LabMat::openMapFile", %defaultFileName );
	%filters = LabMat.textureFormats;

	if (%defaultFileName $= "" || !isFile(%defaultFileName)) {
		if(LabMat.lastTextureFile !$= "")
			%defaultFileName = LabMat.lastTextureFile;
		else if (isFile($MEP_BaseObjectPath)) {
			%defaultFileName = $MEP_BaseObjectPath;
		} else
			%defaultFileName = "art/*.*";
	}

	%defaultPath = LabMat.lastTexturePath;
	%dlg = new OpenFileDialog() {
		Filters        = %filters;
		DefaultPath    = %defaultPath;
		DefaultFile    = %defaultFileName;
		ChangePath     = false;
		MustExist      = true;
	};
	%ret = %dlg.Execute();

	if(%ret) {
		LabMat.lastTexturePath = filePath( %dlg.FileName );
		LabMat.lastTextureFile = %filename = %dlg.FileName;
		%dlg.delete();
		return makeRelativePath( %filename, getMainDotCsDir() );
	}

	%dlg.delete();
	return;
}
//------------------------------------------------------------------------------

//==============================================================================

//------------------------------------------------------------------------------
//==============================================================================
// Per-Layer Material Options

// For update maps
// %action : 1 = change map
// %action : 0 = remove map

function LabMat::updateTextureMap( %this, %type, %action ) {
	%layer = LabMat.currentLayer;
	%bitmapCtrl = LabMat_MapsStack.findObjectByInternalName( %type @ "MapDisplayBitmap", true );
	%textCtrl = LabMat_MapsStack.findObjectByInternalName( %type @ "MapNameText", true );

	if( %action ) {
		%texture = LabMat.openMapFile(LabMat.currentMaterial.diffuseMap[0]);

		if( %texture !$= "" ) {
			%this.updateTextureMapImage(%type,%texture,%layer);
		}
	} else {
		%textCtrl.setText("None");
		%bitmapCtrl.setBitmap($MEP_NoTextureImage);
		LabMat.updateActiveMaterial(%type @ "Map[" @ %layer @ "]","");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateTextureMapImage( %this, %type, %texture,%layer,%skipAutoCheck ) {
	%bitmapCtrl = LabMat_MapsStack.findObjectByInternalName( %type @ "MapDisplayBitmap", true );
	%textCtrl = LabMat_MapsStack.findObjectByInternalName( %type @ "MapNameText", true );

	%texture = strreplace(%texture,"//","/");
	if (!%skipAutoCheck)
		%this.CheckAutoUpdateMap(%type, %texture,%layer);

	if (isImageFile(%texture))
		%bitmapCtrl.setBitmap(%texture);

	%bitmap = %bitmapCtrl.bitmap;
	%bitmap = strreplace(%bitmap,"tlab/materialEditor/scripts/","");
	%bitmapCtrl.setBitmap(%bitmap);
	%textCtrl.setText(%bitmap);
	LabMat.updateActiveMaterial(%type @ "Map[" @ %layer @ "]","\"" @ %bitmap @ "\"");
}
//------------------------------------------------------------------------------
//==============================================================================

function LabMat::updateDetailScale(%this,%newScale) {
	%layer = LabMat.currentLayer;
	%detailScale = "\"" @ %newScale SPC %newScale @ "\"";
	LabMat.updateActiveMaterial("detailScale[" @ %layer @ "]", %detailScale);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateDetailNormalStrength(%this,%newStrength) {
	%layer = LabMat.currentLayer;
	%detailStrength = "\"" @ %newStrength @ "\"";
	LabMat.updateActiveMaterial("detailNormalMapStrength[" @ %layer @ "]", %detailStrength);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateDiffuseMap(%this,%action,%texture,%skipAutoCheck) {
	%layer = LabMat.currentLayer;
	%this.updateTextureMapImage("Diffuse",%texture,%layer,%skipAutoCheck);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateNormalMap(%this,%action,%texture,%skipAutoCheck) {
	%layer = LabMat.currentLayer;
	%this.updateTextureMapImage("Normal",%texture,%layer,%skipAutoCheck);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateSpecMap(%this,%action,%texture,%skipAutoCheck) {
		%layer = LabMat.currentLayer;
//	%this.updateTextureMapImage("specularMap",%texture,%layer,%skipAutoCheck);
	//return;
//}
	%layer = LabMat.currentLayer;
	devlog("Specmap",%action,%texture,%skipAutoCheck);
	if( %action ) {
		if (%texture $= "")
			%texture = LabMat.openMapFile();

		if( %texture !$= "" ) {
			if (!%skipAutoCheck)
				%this.CheckAutoUpdateMap("Specular", %texture,%layer);

			LabMat.updateActiveMaterial("pixelSpecular[" @ LabMat.currentLayer @ "]", 0);
			LabMat_MapsStack-->specularMapDisplayBitmap.setBitmap(%texture);
			%bitmap = LabMat_MapsStack-->specularMapDisplayBitmap.bitmap;
			%bitmap = strreplace(%bitmap,"tlab/materialEditor/scripts/","");
			LabMat_MapsStack-->specularMapDisplayBitmap.setBitmap(%bitmap);
			LabMat_MapsStack-->specularMapNameText.setText(%bitmap);
			LabMat.updateActiveMaterial("specularMap[" @ %layer @ "]","\"" @ %bitmap @ "\"");
		}
	} else {
		LabMat_MapsStack-->specularMapNameText.setText("None");
		LabMat_MapsStack-->specularMapDisplayBitmap.setBitmap($MEP_NoTextureImage);
		LabMat.updateActiveMaterial("specularMap[" @ %layer @ "]","");
	}

	LabMat.guiSync( LabMat_previewMaterial );
}
//------------------------------------------------------------------------------
//PBR Script
//==============================================================================
function LabMat::updateCompMap(%this,%action,%texture,%skipAutoCheck) {
	%layer = LabMat.currentLayer;

	if( %action ) {
		if (%texture $= "")
			%texture = LabMat.openMapFile();

		if( %texture !$= "" ) {
			if (!%skipAutoCheck)
				%this.CheckAutoUpdateMap("Composite", %texture,%layer);

			LabMat.updateActiveMaterial("pixelSpecular[" @ LabMat.currentLayer @ "]", 0);
			LabMat_MapsStack-->compMapDisplayBitmap.setBitmap(%texture);
			%bitmap = LabMat_MapsStack-->compMapDisplayBitmap.bitmap;
			%bitmap = strreplace(%bitmap,"tlab/materialEditor/scripts/","");
			LabMat_MapsStack-->compMapDisplayBitmap.setBitmap(%bitmap);
			LabMat_MapsStack-->compMapNameText.setText(%bitmap);
			LabMat_MapsStack-->specularMapDisplayBitmap.setBitmap(%bitmap);
			LabMat_MapsStack-->specularMapNameText.setText(%bitmap);
			LabMat.updateActiveMaterial("specularMap[" @ %layer @ "]","\"" @ %bitmap @ "\"");
		}
	} else {
		LabMat_MapsStack-->compMapNameText.setText("None");
		LabMat_MapsStack-->compMapDisplayBitmap.setBitmap($MEP_NoTextureImage);
		LabMat_MapsStack-->specularMapNameText.setText("None");
		LabMat_MapsStack-->specularMapDisplayBitmap.setBitmap($MEP_NoTextureImage);
		LabMat.updateActiveMaterial("specularMap[" @ %layer @ "]","");
	}

	LabMat.guiSync( LabMat_previewMaterial );
}
//------------------------------------------------------------------------------
function LabMat::updateRoughMap(%this,%action,%texture,%skipAutoCheck) {
	%layer = LabMat.currentLayer;

	if( %action ) {
		if (%texture $= "")
			%texture = LabMat.openFile("texture");

		if( %texture !$= "" ) {
			if (!%skipAutoCheck)
				%this.CheckAutoUpdateMap("Smoothness", %texture,%layer);

			LabMat_MapsStack-->roughMapDisplayBitmap.setBitmap(%texture);
			%bitmap = LabMat_MapsStack-->roughMapDisplayBitmap.bitmap;
			%bitmap = strreplace(%bitmap,"tools/materialEditor/scripts/","");
			LabMat_MapsStack-->roughMapDisplayBitmap.setBitmap(%bitmap);
			LabMat_MapsStack-->roughMapNameText.setText(%bitmap);
			LabMat.updateActiveMaterial("roughMap[" @ %layer @ "]","\"" @ %bitmap @ "\"");
		}
	} else {
		LabMat_MapsStack-->roughMapNameText.setText("None");
		LabMat_MapsStack-->roughMapDisplayBitmap.setBitmap($MEP_NoTextureImage);
		LabMat.updateActiveMaterial("roughMap[" @ %layer @ "]","");
	}

	LabMat.guiSync( LabMat_previewMaterial );
}

function LabMat::updateaoMap(%this,%action,%texture,%skipAutoCheck) {
	%layer = LabMat.currentLayer;

	if( %action ) {
		if (%texture $= "")
			%texture = LabMat.openFile("texture");

		if( %texture !$= "" ) {
			if (!%skipAutoCheck)
				%this.CheckAutoUpdateMap("AO", %texture,%layer);

			LabMat_MapsStack-->aoMapDisplayBitmap.setBitmap(%texture);
			%bitmap = LabMat_MapsStack-->aoMapDisplayBitmap.bitmap;
			%bitmap = strreplace(%bitmap,"tools/materialEditor/scripts/","");
			LabMat_MapsStack-->aoMapDisplayBitmap.setBitmap(%bitmap);
			LabMat_MapsStack-->aoMapNameText.setText(%bitmap);
			LabMat.updateActiveMaterial("aoMap[" @ %layer @ "]","\"" @ %bitmap @ "\"");
		}
	} else {
		LabMat_MapsStack-->aoMapNameText.setText("None");
		LabMat_MapsStack-->aoMapDisplayBitmap.setBitmap($MEP_NoTextureImage);
		LabMat.updateActiveMaterial("aoMap[" @ %layer @ "]","");
	}

	LabMat.guiSync( LabMat_previewMaterial );
}

function LabMat::updatemetalMap(%this,%action,%texture,%skipAutoCheck) {
	devLog("updatemetalMap:",%action,%texture,%skipAutoCheck);
	%layer = LabMat.currentLayer;

	if( %action ) {
		if (%texture $= "")
			%texture = LabMat.openFile("texture");

		if( %texture !$= "" ) {
			if (!%skipAutoCheck)
				%this.CheckAutoUpdateMap("Metalness", %texture,%layer);

			LabMat_MapsStack-->metalMapDisplayBitmap.setBitmap(%texture);
			%bitmap = LabMat_MapsStack-->metalMapDisplayBitmap.bitmap;
			%bitmap = strreplace(%bitmap,"tools/materialEditor/scripts/","");
			LabMat_MapsStack-->metalMapDisplayBitmap.setBitmap(%bitmap);
			LabMat_MapsStack-->metalMapNameText.setText(%bitmap);
			LabMat.updateActiveMaterial("metalMap[" @ %layer @ "]","\"" @ %bitmap @ "\"");
		}
	} else {
		LabMat_MapsStack-->metalMapNameText.setText("None");
		LabMat_MapsStack-->metalMapDisplayBitmap.setBitmap($MEP_NoTextureImage);
		LabMat.updateActiveMaterial("metalMap[" @ %layer @ "]","");
	}

	LabMat.guiSync( LabMat_previewMaterial );
}

//PBR Script End
//==============================================================================
function LabMat::updateRotationOffset(%this, %isSlider, %onMouseUp) {
	%layer = LabMat.currentLayer;
	%X = LabMat_MapsStack-->RotationTextEditU.getText();
	%Y = LabMat_MapsStack-->RotationTextEditV.getText();
	LabMat_MapsStack-->RotationCrosshair.setPosition(45*mAbs(%X)-2, 45*mAbs(%Y)-2);
	LabMat.updateActiveMaterial("rotPivotOffset[" @ %layer @ "]","\"" @ %X SPC %Y @ "\"",%isSlider,%onMouseUp);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateRotationSpeed(%this, %isSlider, %onMouseUp) {
	%layer = LabMat.currentLayer;
	%speed = LabMat_MapsStack-->RotationSpeedTextEdit.getText();
	LabMat.updateActiveMaterial("rotSpeed[" @ %layer @ "]",%speed,%isSlider,%onMouseUp);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateScrollOffset(%this, %isSlider, %onMouseUp) {
	%layer = LabMat.currentLayer;
	%X = LabMat_MapsStack-->ScrollTextEditU.getText();
	%Y = LabMat_MapsStack-->ScrollTextEditV.getText();
	LabMat_MapsStack-->ScrollCrosshair.setPosition( -(23 * %X)+20, -(23 * %Y)+20);
	LabMat.updateActiveMaterial("scrollDir[" @ %layer @ "]","\"" @ %X SPC %Y @ "\"",%isSlider,%onMouseUp);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateScrollSpeed(%this, %isSlider, %onMouseUp) {
	%layer = LabMat.currentLayer;
	%speed = LabMat_MapsStack-->ScrollSpeedTextEdit.getText();
	LabMat.updateActiveMaterial("scrollSpeed[" @ %layer @ "]",%speed,%isSlider,%onMouseUp);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateWaveType(%this) {
	for( %radioButton = 0; %radioButton < LabMat_MapsStack-->WaveButtonContainer.getCount(); %radioButton++ ) {
		if( LabMat_MapsStack-->WaveButtonContainer.getObject(%radioButton).getValue() == 1 )
			%type = LabMat_MapsStack-->WaveButtonContainer.getObject(%radioButton).waveType;
	}

	%layer = LabMat.currentLayer;
	LabMat.updateActiveMaterial("waveType[" @ %layer @ "]", %type);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateWaveAmp(%this, %isSlider, %onMouseUp) {
	%layer = LabMat.currentLayer;
	%amp = LabMat_MapsStack-->WaveTextEditAmp.getText();
	LabMat.updateActiveMaterial("waveAmp[" @ %layer @ "]", %amp, %isSlider, %onMouseUp);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateWaveFreq(%this, %isSlider, %onMouseUp) {
	%layer = LabMat.currentLayer;
	%freq = LabMat_MapsStack-->WaveTextEditFreq.getText();
	LabMat.updateActiveMaterial("waveFreq[" @ %layer @ "]", %freq, %isSlider, %onMouseUp);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateSequenceFPS(%this, %isSlider, %onMouseUp) {
	%layer = LabMat.currentLayer;
	%fps = LabMat_MapsStack-->SequenceTextEditFPS.getText();
	LabMat.updateActiveMaterial("sequenceFramePerSec[" @ %layer @ "]", %fps, %isSlider, %onMouseUp);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateSequenceSSS(%this, %isSlider, %onMouseUp) {
	%layer = LabMat.currentLayer;
	%sss = 1 / LabMat_MapsStack-->SequenceTextEditSSS.getText();
	LabMat.updateActiveMaterial("sequenceSegmentSize[" @ %layer @ "]", %sss, %isSlider, %onMouseUp);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateAnimationFlags(%this) {
	LabMat.setMaterialDirty();
	%single = true;

	if(LabMat_MapsStack-->RotationAnimation.getValue() == true) {
		if(%single == true)
			%flags = %flags @ "$Rotate";
		else
			%flags = %flags @ " | $Rotate";

		%single = false;
	}

	if(LabMat_MapsStack-->ScrollAnimation.getValue() == true) {
		if(%single == true)
			%flags = %flags @ "$Scroll";
		else
			%flags = %flags @ " | $Scroll";

		%single = false;
	}

	if(LabMat_MapsStack-->WaveAnimation.getValue() == true) {
		if(%single == true)
			%flags = %flags @ "$Wave";
		else
			%flags = %flags @ " | $Wave";

		%single = false;
	}

	if(LabMat_MapsStack-->ScaleAnimation.getValue() == true) {
		if(%single == true)
			%flags = %flags @ "$Scale";
		else
			%flags = %flags @ " | $Scale";

		%single = false;
	}

	if(LabMat_MapsStack-->SequenceAnimation.getValue() == true) {
		if(%single == true)
			%flags = %flags @ "$Sequence";
		else
			%flags = %flags @ " | $Sequence";

		%single = false;
	}

	if(%flags $= "")
		%flags = "\"\"";

	%action = %this.createUndo(ActionUpdateActiveMaterialAnimationFlags, "Update Active Material");
	%action.material = LabMat.currentMaterial;
	%action.object = LabMat.currentObject;
	%action.layer = LabMat.currentLayer;
	%action.newValue = %flags;
	%oldFlags = LabMat.currentMaterial.getAnimFlags(LabMat.currentLayer);

	if(%oldFlags $= "")
		%oldFlags = "\"\"";

	%action.oldValue = %oldFlags;
	LabMat.submitUndo( %action );
	eval("LabMat_previewMaterial.animFlags[" @ LabMat.currentLayer @ "] = " @ %flags @ ";");
	LabMat_previewMaterial.flush();
	LabMat_previewMaterial.reload();

	if (LabMat.livePreview == true) {
		eval("LabMat.currentMaterial.animFlags[" @ LabMat.currentLayer @ "] = " @ %flags @ ";");
		LabMat.currentMaterial.flush();
		LabMat.currentMaterial.reload();
	}
}
//------------------------------------------------------------------------------
//==============================================================================

//These two functions are focused on object/layer specific functionality
function LabMat::updateColorMultiply(%this,%color) {
	%propName = "diffuseColor[" @ LabMat.currentLayer @ "]";
	%this.syncGuiColor(LabMat_MapsStack-->colorTintSwatch, %propName, %color);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateSpecularCheckbox(%this,%value) {
	LabMat.updateActiveMaterial("pixelSpecular[" @ LabMat.currentLayer @ "]", %value);
	LabMat.guiSync( LabMat_previewMaterial );
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateSpecular(%this, %color) {
	%propName = "specular[" @ LabMat.currentLayer @ "]";
	%this.syncGuiColor(LabMat_MapsStack-->specularColorSwatch, %propName, %color);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateSubSurfaceColor(%this, %color) {
	%propName = "subSurfaceColor[" @ LabMat.currentLayer @ "]";
	%this.syncGuiColor(LabMat_MapsStack-->subSurfaceColorSwatch, %propName, %color);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateEffectColor0(%this, %color) {
	%this.syncGuiColor(LabMat_MapsStack-->effectColor0Swatch, "effectColor[0]", %color);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateEffectColor1(%this, %color) {
	%this.syncGuiColor(LabMat_MapsStack-->effectColor1Swatch, "effectColor[1]", %color);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateBehaviorSound(%this, %type, %sound) {
	%defaultId = -1;
	%customName = "";

	switch$ (%sound) {
	case "<Soft>":
		%defaultId = 0;

	case "<Hard>":
		%defaultId = 1;

	case "<Metal>":
		%defaultId = 2;

	case "<Snow>":
		%defaultId = 3;

	default:
		%customName = %sound;
	}

	%this.updateActiveMaterial(%type @ "SoundId", %defaultId);
	%this.updateActiveMaterial("custom" @ %type @ "Sound", %customName);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateSoundPopup(%this, %type, %defaultId, %customName) {
	%ctrl = LabMat_MapsStack.findObjectByInternalName( %type @ "SoundPopup", true );

	switch (%defaultId) {
	case 0:
		%name = "<Soft>";

	case 1:
		%name = "<Hard>";

	case 2:
		%name = "<Metal>";

	case 3:
		%name = "<Snow>";

	default:
		if (%customName $= "")
			%name = "<None>";
		else
			%name = %customName;
	}

	%r = %ctrl.findText(%name);

	if (%r != -1)
		%ctrl.setSelected(%r, false);
	else
		%ctrl.setText(%name);
}
//------------------------------------------------------------------------------
//==============================================================================
//These two functions are focused on environment specific functionality
function LabMat::updateLightColor(%this, %color) {
	matEd_previewObjectView.setLightColor(%color);
	matEd_lightColorPicker.color = %color;
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updatePreviewBackground(%this,%color) {
	matEd_previewBackground.color = %color;
	MaterialPreviewBackgroundPicker.color = %color;
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::updateAmbientColor(%this,%color) {
	matEd_previewObjectView.setAmbientLightColor(%color);
	matEd_ambientLightColorPicker.color = %color;
}
//------------------------------------------------------------------------------
//==============================================================================
// PBR Script - Set/Get PBR CHannels
//==============================================================================
//==============================================================================
// Set PBR Channel in selectors
function LabMat::setRoughChan(%this, %value) {
	LabMat.updateActiveMaterial("SmoothnessChan[" @ LabMat.currentLayer @ "]", %value);
	LabMat.guiSync( LabMat_previewMaterial );
}
//------------------------------------------------------------------------------
function LabMat::setAOChan(%this, %value) {
	LabMat.updateActiveMaterial("aoChan[" @ LabMat.currentLayer @ "]", %value);
	LabMat.guiSync( LabMat_previewMaterial );
}
//------------------------------------------------------------------------------
function LabMat::setMetalChan(%this, %value) {
	LabMat.updateActiveMaterial("metalChan[" @ LabMat.currentLayer @ "]", %value);
	LabMat.guiSync( LabMat_previewMaterial );
}
//------------------------------------------------------------------------------
//==============================================================================
// Get PBR Channels
function LabMat::getRoughChan(%this, %channel) {
	%guiElement = "roughChanBtn" @ %channel;
	%guiElement.setStateOn(true);
}
//------------------------------------------------------------------------------
function LabMat::getAOChan(%this, %channel) {
	%guiElement = "AOChanBtn" @ %channel;
	%guiElement.setStateOn(true);
}
//------------------------------------------------------------------------------
function LabMat::getMetalChan(%this, %channel) {
	%guiElement = "metalChanBtn" @ %channel;
	%guiElement.setStateOn(true);
}
//------------------------------------------------------------------------------
