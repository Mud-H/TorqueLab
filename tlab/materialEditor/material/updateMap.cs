//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$MaterialEditor_AutoUpdateMap = true;
//==============================================================================
//Select a texture for a material map type
function MaterialEditorTools::CheckAutoUpdateMap( %this,%type, %texture,%layer) {
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
function MaterialEditorTools::openMapFile( %this, %defaultFileName ) {
	logc("MaterialEditorTools::openMapFile", %defaultFileName );
	%filters = MaterialEditorTools.textureFormats;

	if (%defaultFileName $= "" || !isFile(%defaultFileName)) {
		if(MaterialEditorTools.lastTextureFile !$= "")
			%defaultFileName = MaterialEditorTools.lastTextureFile;
		else if (isFile($MEP_BaseObjectPath)) {
			%defaultFileName = $MEP_BaseObjectPath;
		} else
			%defaultFileName = "art/*.*";
	}

	%defaultPath = MaterialEditorTools.lastTexturePath;
	%dlg = new OpenFileDialog() {
		Filters        = %filters;
		DefaultPath    = %defaultPath;
		DefaultFile    = %defaultFileName;
		ChangePath     = false;
		MustExist      = true;
	};
	%ret = %dlg.Execute();

	if(%ret) {
		MaterialEditorTools.lastTexturePath = filePath( %dlg.FileName );
		MaterialEditorTools.lastTextureFile = %filename = %dlg.FileName;
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

function MaterialEditorTools::updateTextureMap( %this, %type, %action ) {
	%layer = MaterialEditorTools.currentLayer;
	%bitmapCtrl = MaterialEditorPropertiesWindow.findObjectByInternalName( %type @ "MapDisplayBitmap", true );
	%textCtrl = MaterialEditorPropertiesWindow.findObjectByInternalName( %type @ "MapNameText", true );

	if( %action ) {
		%texture = MaterialEditorTools.openMapFile(MaterialEditorTools.currentMaterial.diffuseMap[0]);

		if( %texture !$= "" ) {
			%this.updateTextureMapImage(%type,%texture,%layer);
		}
	} else {
		%textCtrl.setText("None");
		%bitmapCtrl.setBitmap($MEP_NoTextureImage);
		MaterialEditorTools.updateActiveMaterial(%type @ "Map[" @ %layer @ "]","");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateTextureMapImage( %this, %type, %texture,%layer,%skipAutoCheck ) {
	%bitmapCtrl = MaterialEditorPropertiesWindow.findObjectByInternalName( %type @ "MapDisplayBitmap", true );
	%textCtrl = MaterialEditorPropertiesWindow.findObjectByInternalName( %type @ "MapNameText", true );

	%texture = strreplace(%texture,"//","/");
	if (!%skipAutoCheck)
		%this.CheckAutoUpdateMap(%type, %texture,%layer);

	if (isImageFile(%texture))
		%bitmapCtrl.setBitmap(%texture);

	%bitmap = %bitmapCtrl.bitmap;
	%bitmap = strreplace(%bitmap,"tlab/materialEditor/scripts/","");
	%bitmapCtrl.setBitmap(%bitmap);
	%textCtrl.setText(%bitmap);
	MaterialEditorTools.updateActiveMaterial(%type @ "Map[" @ %layer @ "]","\"" @ %bitmap @ "\"");
}
//------------------------------------------------------------------------------
//==============================================================================

function MaterialEditorTools::updateDetailScale(%this,%newScale) {
	%layer = MaterialEditorTools.currentLayer;
	%detailScale = "\"" @ %newScale SPC %newScale @ "\"";
	MaterialEditorTools.updateActiveMaterial("detailScale[" @ %layer @ "]", %detailScale);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateDetailNormalStrength(%this,%newStrength) {
	%layer = MaterialEditorTools.currentLayer;
	%detailStrength = "\"" @ %newStrength @ "\"";
	MaterialEditorTools.updateActiveMaterial("detailNormalMapStrength[" @ %layer @ "]", %detailStrength);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateDiffuseMap(%this,%action,%texture,%skipAutoCheck) {
	%layer = MaterialEditorTools.currentLayer;
	%this.updateTextureMapImage("Diffuse",%texture,%layer,%skipAutoCheck);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateNormalMap(%this,%action,%texture,%skipAutoCheck) {
	%layer = MaterialEditorTools.currentLayer;
	%this.updateTextureMapImage("Normal",%texture,%layer,%skipAutoCheck);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateSpecMap(%this,%action,%texture,%skipAutoCheck) {
		%layer = MaterialEditorTools.currentLayer;
//	%this.updateTextureMapImage("specularMap",%texture,%layer,%skipAutoCheck);
	//return;
//}
	%layer = MaterialEditorTools.currentLayer;
	devlog("Specmap",%action,%texture,%skipAutoCheck);
	if( %action ) {
		if (%texture $= "")
			%texture = MaterialEditorTools.openMapFile();

		if( %texture !$= "" ) {
			if (!%skipAutoCheck)
				%this.CheckAutoUpdateMap("Specular", %texture,%layer);

			MaterialEditorTools.updateActiveMaterial("pixelSpecular[" @ MaterialEditorTools.currentLayer @ "]", 0);
			MaterialEditorPropertiesWindow-->specularMapDisplayBitmap.setBitmap(%texture);
			%bitmap = MaterialEditorPropertiesWindow-->specularMapDisplayBitmap.bitmap;
			%bitmap = strreplace(%bitmap,"tlab/materialEditor/scripts/","");
			MaterialEditorPropertiesWindow-->specularMapDisplayBitmap.setBitmap(%bitmap);
			MaterialEditorPropertiesWindow-->specularMapNameText.setText(%bitmap);
			MaterialEditorTools.updateActiveMaterial("specularMap[" @ %layer @ "]","\"" @ %bitmap @ "\"");
		}
	} else {
		MaterialEditorPropertiesWindow-->specularMapNameText.setText("None");
		MaterialEditorPropertiesWindow-->specularMapDisplayBitmap.setBitmap($MEP_NoTextureImage);
		MaterialEditorTools.updateActiveMaterial("specularMap[" @ %layer @ "]","");
	}

	MaterialEditorTools.guiSync( materialEd_previewMaterial );
}
//------------------------------------------------------------------------------
//PBR Script
//==============================================================================
function MaterialEditorTools::updateCompMap(%this,%action,%texture,%skipAutoCheck) {
	%layer = MaterialEditorTools.currentLayer;

	if( %action ) {
		if (%texture $= "")
			%texture = MaterialEditorTools.openMapFile();

		if( %texture !$= "" ) {
			if (!%skipAutoCheck)
				%this.CheckAutoUpdateMap("Composite", %texture,%layer);

			MaterialEditorTools.updateActiveMaterial("pixelSpecular[" @ MaterialEditorTools.currentLayer @ "]", 0);
			MaterialEditorPropertiesWindow-->compMapDisplayBitmap.setBitmap(%texture);
			%bitmap = MaterialEditorPropertiesWindow-->compMapDisplayBitmap.bitmap;
			%bitmap = strreplace(%bitmap,"tlab/materialEditor/scripts/","");
			MaterialEditorPropertiesWindow-->compMapDisplayBitmap.setBitmap(%bitmap);
			MaterialEditorPropertiesWindow-->compMapNameText.setText(%bitmap);
			MaterialEditorPropertiesWindow-->specularMapDisplayBitmap.setBitmap(%bitmap);
			MaterialEditorPropertiesWindow-->specularMapNameText.setText(%bitmap);
			MaterialEditorTools.updateActiveMaterial("specularMap[" @ %layer @ "]","\"" @ %bitmap @ "\"");
		}
	} else {
		MaterialEditorPropertiesWindow-->compMapNameText.setText("None");
		MaterialEditorPropertiesWindow-->compMapDisplayBitmap.setBitmap($MEP_NoTextureImage);
		MaterialEditorPropertiesWindow-->specularMapNameText.setText("None");
		MaterialEditorPropertiesWindow-->specularMapDisplayBitmap.setBitmap($MEP_NoTextureImage);
		MaterialEditorTools.updateActiveMaterial("specularMap[" @ %layer @ "]","");
	}

	MaterialEditorTools.guiSync( materialEd_previewMaterial );
}
//------------------------------------------------------------------------------
function MaterialEditorTools::updateRoughMap(%this,%action,%texture,%skipAutoCheck) {
	%layer = MaterialEditorTools.currentLayer;

	if( %action ) {
		if (%texture $= "")
			%texture = MaterialEditorTools.openFile("texture");

		if( %texture !$= "" ) {
			if (!%skipAutoCheck)
				%this.CheckAutoUpdateMap("Smoothness", %texture,%layer);

			MaterialEditorPropertiesWindow-->roughMapDisplayBitmap.setBitmap(%texture);
			%bitmap = MaterialEditorPropertiesWindow-->roughMapDisplayBitmap.bitmap;
			%bitmap = strreplace(%bitmap,"tools/materialEditor/scripts/","");
			MaterialEditorPropertiesWindow-->roughMapDisplayBitmap.setBitmap(%bitmap);
			MaterialEditorPropertiesWindow-->roughMapNameText.setText(%bitmap);
			MaterialEditorTools.updateActiveMaterial("roughMap[" @ %layer @ "]","\"" @ %bitmap @ "\"");
		}
	} else {
		MaterialEditorPropertiesWindow-->roughMapNameText.setText("None");
		MaterialEditorPropertiesWindow-->roughMapDisplayBitmap.setBitmap($MEP_NoTextureImage);
		MaterialEditorTools.updateActiveMaterial("roughMap[" @ %layer @ "]","");
	}

	MaterialEditorTools.guiSync( materialEd_previewMaterial );
}

function MaterialEditorTools::updateaoMap(%this,%action,%texture,%skipAutoCheck) {
	%layer = MaterialEditorTools.currentLayer;

	if( %action ) {
		if (%texture $= "")
			%texture = MaterialEditorTools.openFile("texture");

		if( %texture !$= "" ) {
			if (!%skipAutoCheck)
				%this.CheckAutoUpdateMap("AO", %texture,%layer);

			MaterialEditorPropertiesWindow-->aoMapDisplayBitmap.setBitmap(%texture);
			%bitmap = MaterialEditorPropertiesWindow-->aoMapDisplayBitmap.bitmap;
			%bitmap = strreplace(%bitmap,"tools/materialEditor/scripts/","");
			MaterialEditorPropertiesWindow-->aoMapDisplayBitmap.setBitmap(%bitmap);
			MaterialEditorPropertiesWindow-->aoMapNameText.setText(%bitmap);
			MaterialEditorTools.updateActiveMaterial("aoMap[" @ %layer @ "]","\"" @ %bitmap @ "\"");
		}
	} else {
		MaterialEditorPropertiesWindow-->aoMapNameText.setText("None");
		MaterialEditorPropertiesWindow-->aoMapDisplayBitmap.setBitmap($MEP_NoTextureImage);
		MaterialEditorTools.updateActiveMaterial("aoMap[" @ %layer @ "]","");
	}

	MaterialEditorTools.guiSync( materialEd_previewMaterial );
}

function MaterialEditorTools::updatemetalMap(%this,%action,%texture,%skipAutoCheck) {
	devLog("updatemetalMap:",%action,%texture,%skipAutoCheck);
	%layer = MaterialEditorTools.currentLayer;

	if( %action ) {
		if (%texture $= "")
			%texture = MaterialEditorTools.openFile("texture");

		if( %texture !$= "" ) {
			if (!%skipAutoCheck)
				%this.CheckAutoUpdateMap("Metalness", %texture,%layer);

			MaterialEditorPropertiesWindow-->metalMapDisplayBitmap.setBitmap(%texture);
			%bitmap = MaterialEditorPropertiesWindow-->metalMapDisplayBitmap.bitmap;
			%bitmap = strreplace(%bitmap,"tools/materialEditor/scripts/","");
			MaterialEditorPropertiesWindow-->metalMapDisplayBitmap.setBitmap(%bitmap);
			MaterialEditorPropertiesWindow-->metalMapNameText.setText(%bitmap);
			MaterialEditorTools.updateActiveMaterial("metalMap[" @ %layer @ "]","\"" @ %bitmap @ "\"");
		}
	} else {
		MaterialEditorPropertiesWindow-->metalMapNameText.setText("None");
		MaterialEditorPropertiesWindow-->metalMapDisplayBitmap.setBitmap($MEP_NoTextureImage);
		MaterialEditorTools.updateActiveMaterial("metalMap[" @ %layer @ "]","");
	}

	MaterialEditorTools.guiSync( materialEd_previewMaterial );
}

//PBR Script End
//==============================================================================
function MaterialEditorTools::updateRotationOffset(%this, %isSlider, %onMouseUp) {
	%layer = MaterialEditorTools.currentLayer;
	%X = MaterialEditorPropertiesWindow-->RotationTextEditU.getText();
	%Y = MaterialEditorPropertiesWindow-->RotationTextEditV.getText();
	MaterialEditorPropertiesWindow-->RotationCrosshair.setPosition(45*mAbs(%X)-2, 45*mAbs(%Y)-2);
	MaterialEditorTools.updateActiveMaterial("rotPivotOffset[" @ %layer @ "]","\"" @ %X SPC %Y @ "\"",%isSlider,%onMouseUp);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateRotationSpeed(%this, %isSlider, %onMouseUp) {
	%layer = MaterialEditorTools.currentLayer;
	%speed = MaterialEditorPropertiesWindow-->RotationSpeedTextEdit.getText();
	MaterialEditorTools.updateActiveMaterial("rotSpeed[" @ %layer @ "]",%speed,%isSlider,%onMouseUp);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateScrollOffset(%this, %isSlider, %onMouseUp) {
	%layer = MaterialEditorTools.currentLayer;
	%X = MaterialEditorPropertiesWindow-->ScrollTextEditU.getText();
	%Y = MaterialEditorPropertiesWindow-->ScrollTextEditV.getText();
	MaterialEditorPropertiesWindow-->ScrollCrosshair.setPosition( -(23 * %X)+20, -(23 * %Y)+20);
	MaterialEditorTools.updateActiveMaterial("scrollDir[" @ %layer @ "]","\"" @ %X SPC %Y @ "\"",%isSlider,%onMouseUp);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateScrollSpeed(%this, %isSlider, %onMouseUp) {
	%layer = MaterialEditorTools.currentLayer;
	%speed = MaterialEditorPropertiesWindow-->ScrollSpeedTextEdit.getText();
	MaterialEditorTools.updateActiveMaterial("scrollSpeed[" @ %layer @ "]",%speed,%isSlider,%onMouseUp);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateWaveType(%this) {
	for( %radioButton = 0; %radioButton < MaterialEditorPropertiesWindow-->WaveButtonContainer.getCount(); %radioButton++ ) {
		if( MaterialEditorPropertiesWindow-->WaveButtonContainer.getObject(%radioButton).getValue() == 1 )
			%type = MaterialEditorPropertiesWindow-->WaveButtonContainer.getObject(%radioButton).waveType;
	}

	%layer = MaterialEditorTools.currentLayer;
	MaterialEditorTools.updateActiveMaterial("waveType[" @ %layer @ "]", %type);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateWaveAmp(%this, %isSlider, %onMouseUp) {
	%layer = MaterialEditorTools.currentLayer;
	%amp = MaterialEditorPropertiesWindow-->WaveTextEditAmp.getText();
	MaterialEditorTools.updateActiveMaterial("waveAmp[" @ %layer @ "]", %amp, %isSlider, %onMouseUp);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateWaveFreq(%this, %isSlider, %onMouseUp) {
	%layer = MaterialEditorTools.currentLayer;
	%freq = MaterialEditorPropertiesWindow-->WaveTextEditFreq.getText();
	MaterialEditorTools.updateActiveMaterial("waveFreq[" @ %layer @ "]", %freq, %isSlider, %onMouseUp);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateSequenceFPS(%this, %isSlider, %onMouseUp) {
	%layer = MaterialEditorTools.currentLayer;
	%fps = MaterialEditorPropertiesWindow-->SequenceTextEditFPS.getText();
	MaterialEditorTools.updateActiveMaterial("sequenceFramePerSec[" @ %layer @ "]", %fps, %isSlider, %onMouseUp);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateSequenceSSS(%this, %isSlider, %onMouseUp) {
	%layer = MaterialEditorTools.currentLayer;
	%sss = 1 / MaterialEditorPropertiesWindow-->SequenceTextEditSSS.getText();
	MaterialEditorTools.updateActiveMaterial("sequenceSegmentSize[" @ %layer @ "]", %sss, %isSlider, %onMouseUp);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateAnimationFlags(%this) {
	MaterialEditorTools.setMaterialDirty();
	%single = true;

	if(MaterialEditorPropertiesWindow-->RotationAnimation.getValue() == true) {
		if(%single == true)
			%flags = %flags @ "$Rotate";
		else
			%flags = %flags @ " | $Rotate";

		%single = false;
	}

	if(MaterialEditorPropertiesWindow-->ScrollAnimation.getValue() == true) {
		if(%single == true)
			%flags = %flags @ "$Scroll";
		else
			%flags = %flags @ " | $Scroll";

		%single = false;
	}

	if(MaterialEditorPropertiesWindow-->WaveAnimation.getValue() == true) {
		if(%single == true)
			%flags = %flags @ "$Wave";
		else
			%flags = %flags @ " | $Wave";

		%single = false;
	}

	if(MaterialEditorPropertiesWindow-->ScaleAnimation.getValue() == true) {
		if(%single == true)
			%flags = %flags @ "$Scale";
		else
			%flags = %flags @ " | $Scale";

		%single = false;
	}

	if(MaterialEditorPropertiesWindow-->SequenceAnimation.getValue() == true) {
		if(%single == true)
			%flags = %flags @ "$Sequence";
		else
			%flags = %flags @ " | $Sequence";

		%single = false;
	}

	if(%flags $= "")
		%flags = "\"\"";

	%action = %this.createUndo(ActionUpdateActiveMaterialAnimationFlags, "Update Active Material");
	%action.material = MaterialEditorTools.currentMaterial;
	%action.object = MaterialEditorTools.currentObject;
	%action.layer = MaterialEditorTools.currentLayer;
	%action.newValue = %flags;
	%oldFlags = MaterialEditorTools.currentMaterial.getAnimFlags(MaterialEditorTools.currentLayer);

	if(%oldFlags $= "")
		%oldFlags = "\"\"";

	%action.oldValue = %oldFlags;
	MaterialEditorTools.submitUndo( %action );
	eval("materialEd_previewMaterial.animFlags[" @ MaterialEditorTools.currentLayer @ "] = " @ %flags @ ";");
	materialEd_previewMaterial.flush();
	materialEd_previewMaterial.reload();

	if (MaterialEditorTools.livePreview == true) {
		eval("MaterialEditorTools.currentMaterial.animFlags[" @ MaterialEditorTools.currentLayer @ "] = " @ %flags @ ";");
		MaterialEditorTools.currentMaterial.flush();
		MaterialEditorTools.currentMaterial.reload();
	}
}
//------------------------------------------------------------------------------
//==============================================================================

//These two functions are focused on object/layer specific functionality
function MaterialEditorTools::updateColorMultiply(%this,%color) {
	%propName = "diffuseColor[" @ MaterialEditorTools.currentLayer @ "]";
	%this.syncGuiColor(MaterialEditorPropertiesWindow-->colorTintSwatch, %propName, %color);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateSpecularCheckbox(%this,%value) {
	MaterialEditorTools.updateActiveMaterial("pixelSpecular[" @ MaterialEditorTools.currentLayer @ "]", %value);
	MaterialEditorTools.guiSync( materialEd_previewMaterial );
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateSpecular(%this, %color) {
	%propName = "specular[" @ MaterialEditorTools.currentLayer @ "]";
	%this.syncGuiColor(MaterialEditorPropertiesWindow-->specularColorSwatch, %propName, %color);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateSubSurfaceColor(%this, %color) {
	%propName = "subSurfaceColor[" @ MaterialEditorTools.currentLayer @ "]";
	%this.syncGuiColor(MaterialEditorPropertiesWindow-->subSurfaceColorSwatch, %propName, %color);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateEffectColor0(%this, %color) {
	%this.syncGuiColor(MaterialEditorPropertiesWindow-->effectColor0Swatch, "effectColor[0]", %color);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateEffectColor1(%this, %color) {
	%this.syncGuiColor(MaterialEditorPropertiesWindow-->effectColor1Swatch, "effectColor[1]", %color);
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateBehaviorSound(%this, %type, %sound) {
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
function MaterialEditorTools::updateSoundPopup(%this, %type, %defaultId, %customName) {
	%ctrl = MaterialEditorPropertiesWindow.findObjectByInternalName( %type @ "SoundPopup", true );

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
function MaterialEditorTools::updateLightColor(%this, %color) {
	matEd_previewObjectView.setLightColor(%color);
	matEd_lightColorPicker.color = %color;
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updatePreviewBackground(%this,%color) {
	matEd_previewBackground.color = %color;
	MaterialPreviewBackgroundPicker.color = %color;
}
//------------------------------------------------------------------------------
//==============================================================================
function MaterialEditorTools::updateAmbientColor(%this,%color) {
	matEd_previewObjectView.setAmbientLightColor(%color);
	matEd_ambientLightColorPicker.color = %color;
}
//------------------------------------------------------------------------------
//==============================================================================
// PBR Script - Set/Get PBR CHannels
//==============================================================================
//==============================================================================
// Set PBR Channel in selectors
function MaterialEditorTools::setRoughChan(%this, %value) {
	MaterialEditorTools.updateActiveMaterial("SmoothnessChan[" @ MaterialEditorTools.currentLayer @ "]", %value);
	MaterialEditorTools.guiSync( materialEd_previewMaterial );
}
//------------------------------------------------------------------------------
function MaterialEditorTools::setAOChan(%this, %value) {
	MaterialEditorTools.updateActiveMaterial("aoChan[" @ MaterialEditorTools.currentLayer @ "]", %value);
	MaterialEditorTools.guiSync( materialEd_previewMaterial );
}
//------------------------------------------------------------------------------
function MaterialEditorTools::setMetalChan(%this, %value) {
	MaterialEditorTools.updateActiveMaterial("metalChan[" @ MaterialEditorTools.currentLayer @ "]", %value);
	MaterialEditorTools.guiSync( materialEd_previewMaterial );
}
//------------------------------------------------------------------------------
//==============================================================================
// Get PBR Channels
function MaterialEditorTools::getRoughChan(%this, %channel) {
	%guiElement = "roughChanBtn" @ %channel;
	%guiElement.setStateOn(true);
}
//------------------------------------------------------------------------------
function MaterialEditorTools::getAOChan(%this, %channel) {
	%guiElement = "AOChanBtn" @ %channel;
	%guiElement.setStateOn(true);
}
//------------------------------------------------------------------------------
function MaterialEditorTools::getMetalChan(%this, %channel) {
	%guiElement = "metalChanBtn" @ %channel;
	%guiElement.setStateOn(true);
}
//------------------------------------------------------------------------------
