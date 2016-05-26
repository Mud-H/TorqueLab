//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function LabMat::openFile( %this, %fileType,%defaultFileName ) {


	switch$(%fileType) {
	case "Texture":
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

	case "Model":
		%filters = LabMat.modelFormats;

		if (%defaultFileName $= "")
			%defaultFileName = "*.dts";

		%defaultPath = LabMat.lastModelPath;

	case "File":
		%filters = "TorqueScript Files (*.cs)|*.cs";

		if (%defaultFileName $= "")
			%defaultFileName = "*.dts";

		%defaultPath = LabMat.lastModelPath;
	}

	%dlg = new OpenFileDialog() {
		Filters        = %filters;
		DefaultPath    = %defaultPath;
		DefaultFile    = %defaultFileName;
		ChangePath     = false;
		MustExist      = true;
	};
	%ret = %dlg.Execute();

	if(%ret) {
		switch$(%fileType) {
		case "Texture":
			LabMat.lastTexturePath = filePath( %dlg.FileName );
			LabMat.lastTextureFile = %filename = %dlg.FileName;

		case "Model":
			LabMat.lastModelPath = filePath( %dlg.FileName );
			LabMat.lastModelFile = %filename = %dlg.FileName;

		case "File":
			LabMat.lastScriptPath = filePath( %dlg.FileName );
			LabMat.lastScriptFile = %filename = %dlg.FileName;
		}
	}

	%dlg.delete();

	if(!%ret)
		return;
	else
		return makeRelativePath( %filename, getMainDotCsDir() );
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::copyMaterials( %this, %copyFrom, %copyTo) {
	// Make sure we copy and restore the map to.
	if (!isObject(%copyFrom) || !isObject(%copyTo))
		return;

	%mapTo = %copyTo.mapTo;
	%copyTo.assignFieldsFrom( %copyFrom );
	%copyTo.mapTo = %mapTo;
}
//------------------------------------------------------------------------------
//==============================================================================
// still needs to be optimized further
function LabMat::searchForTexture(%this,%material, %texture) {
	if( %texture !$= "" ) {
		// set the find signal as false to start out with
		%isFile = false;
		// sete the formats we're going to be looping through if need be
		%formats = ".png .jpg .dds .bmp .gif .jng .tga";

		// if the texture contains the correct filepath and name right off the bat, lets use it
		if( isFile(%texture) )
			%isFile = true;
		else {
			for( %i = 0; %i < getWordCount(%formats); %i++) {
				%testFileName = %texture @ getWord( %formats, %i );

				if(isFile(%testFileName)) {
					%isFile = true;
					break;
				}
			}
		}

		// if we didn't grab a proper name, lets use a string logarithm
		if( !%isFile ) {
			%materialDiffuse = %texture;
			%materialDiffuse2 = %texture;
			%materialPath = %material.getFilename();

			if( strchr( %materialDiffuse, "/") $= "" ) {
				%k = 0;

				while( strpos( %materialPath, "/", %k ) != -1 ) {
					%count = strpos( %materialPath, "/", %k );
					%k = %count + 1;
				}

				%materialsCs = getSubStr( %materialPath , %k , 99 );
				%texture =  strreplace( %materialPath, %materialsCs, %texture );
			} else
				%texture =  strreplace( %materialPath, %materialPath, %texture );

			// lets test the pathing we came up with
			if( isFile(%texture) )
				%isFile = true;
			else {
				for( %i = 0; %i < getWordCount(%formats); %i++) {
					%testFileName = %texture @ getWord( %formats, %i );

					if(isFile(%testFileName)) {
						%isFile = true;
						break;
					}
				}
			}

			// as a last resort to find the proper name
			// we have to resolve using find first file functions very very slow
			if( !%isFile ) {
				%k = 0;

				while( strpos( %materialDiffuse2, "/", %k ) != -1 ) {
					%count = strpos( %materialDiffuse2, "/", %k );
					%k = %count + 1;
				}

				%texture =  getSubStr( %materialDiffuse2 , %k , 99 );

				for( %i = 0; %i < getWordCount(%formats); %i++) {
					%searchString = "*" @ %texture @ getWord( %formats, %i );
					%testFileName = findFirstFile( %searchString );

					if( isFile(%testFileName) ) {
						%texture = %testFileName;
						%isFile = true;
						break;
					}
				}
			}

			return %texture;
		} else
			return %texture; //Texture exists and can be found - just return the input argument.
	}

	return ""; //No texture associated with this property.
}
//------------------------------------------------------------------------------

//==============================================================================
function LabMat::convertTextureFields(%this) {
	// Find the absolute paths for the texture filenames so that
	// we can properly wire up the preview materials and controls.
	for(%diffuseI = 0; %diffuseI < 4; %diffuseI++) {
		%diffuseMap = LabMat.currentMaterial.diffuseMap[%diffuseI];
		%diffuseMap = searchForTexture(LabMat.currentMaterial, %diffuseMap);
		LabMat.currentMaterial.diffuseMap[%diffuseI] = %diffuseMap;
	}

	for(%normalI = 0; %normalI < 4; %normalI++) {
		%normalMap = LabMat.currentMaterial.normalMap[%normalI];
		%normalMap = searchForTexture(LabMat.currentMaterial, %normalMap);
		LabMat.currentMaterial.normalMap[%normalI] = %normalMap;
	}

	for(%overlayI = 0; %overlayI < 4; %overlayI++) {
		%overlayMap = LabMat.currentMaterial.overlayMap[%overlayI];
		%overlayMap = searchForTexture(LabMat.currentMaterial, %overlayMap);
		LabMat.currentMaterial.overlayMap[%overlayI] = %overlayMap;
	}

	for(%detailI = 0; %detailI < 4; %detailI++) {
		%detailMap = LabMat.currentMaterial.detailMap[%detailI];
		%detailMap = searchForTexture(LabMat.currentMaterial, %detailMap);
		LabMat.currentMaterial.detailMap[%detailI] = %detailMap;
	}

	for(%detailNormalI = 0; %detailNormalI < 4; %detailNormalI++) {
		%detailNormalMap = LabMat.currentMaterial.detailNormalMap[%detailNormalI];
		%detailNormalMap = searchForTexture(LabMat.currentMaterial, %detailNormalMap);
		LabMat.currentMaterial.detailNormalMap[%detailNormalI] = %detailNormalMap;
	}

	for(%lightI = 0; %lightI < 4; %lightI++) {
		%lightMap = LabMat.currentMaterial.lightMap[%lightI];
		%lightMap = searchForTexture(LabMat.currentMaterial, %lightMap);
		LabMat.currentMaterial.lightMap[%lightI] = %lightMap;
	}

	for(%toneI = 0; %toneI < 4; %toneI++) {
		%toneMap = LabMat.currentMaterial.toneMap[%toneI];
		%toneMap = searchForTexture(LabMat.currentMaterial, %toneMap);
		LabMat.currentMaterial.toneMap[%toneI] = %toneMap;
	}

	for(%specI = 0; %specI < 4; %specI++) {
		%specMap = LabMat.currentMaterial.specularMap[%specI];
		%specMap = searchForTexture(LabMat.currentMaterial, %specMap);
		LabMat.currentMaterial.specularMap[%specI] = %specMap;
	}

	//PBR Script
	for(%roughI = 0; %roughI < 4; %roughI++) {
		%roughMap = LabMat.currentMaterial.roughMap[%roughI];
		%roughMap = searchForTexture(LabMat.currentMaterial, %roughMap);
		LabMat.currentMaterial.roughMap[%specI] = %roughMap;
	}

	for(%aoI = 0; %aoI < 4; %aoI++) {
		%aoMap = LabMat.currentMaterial.aoMap[%aoI];
		%aoMap = searchForTexture(LabMat.currentMaterial, %aoMap);
		LabMat.currentMaterial.aoMap[%specI] = %aoMap;
	}

	for(%metalI = 0; %metalI < 4; %metalI++) {
		%metalMap = LabMat.currentMaterial.metalMap[%metalI];
		%metalMap = searchForTexture(LabMat.currentMaterial, %metalMap);
		LabMat.currentMaterial.metalMap[%metalI] = %metalMap;
	}

	//PBR ScriptEnd
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::isLabMatitorMaterial(%this, %material) {
	return ( %material.getFilename() $= "" ||
													%material.getFilename() $= "tlab/gui/oldmatSelector.ed.gui" ||
															%material.getFilename() $= "tlab/materialEditor/scripts/materialEditor.ed.cs" );
}
//------------------------------------------------------------------------------
