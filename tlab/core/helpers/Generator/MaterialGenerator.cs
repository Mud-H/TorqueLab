//==============================================================================
// HelpersLab -> ForestData Brush and Items Generator
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

$HLab_MatGen_Suffix["diffuseMap"] = "_d";
$HLab_MatGen_Suffix["normalMap"] = "_n";
$HLab_MatGen_Suffix["roughMap"] = "_smo";
$HLab_MatGen_Suffix["aoMap"] = "_ao";
$HLab_MatGen_Suffix["metalMap"] = "_met";
/*
singleton Material(ChatExt_Front_baseDtGround_PavedStoneTilesA)
{
   mapTo = "baseDtGround_PavedStoneTilesA";
   diffuseMap[0] = "art/models/DarkTerritory/Textures/Ground/Pavement/PavedStoneTilesA_d.png";
   smoothness[0] = "1";
   translucentBlendOp = "None";
   normalMap[0] = "art/models/DarkTerritory/Textures/Ground/Pavement/PavedStoneTilesA_n.png";
   roughMap[0] = "art/models/DarkTerritory/Textures/Ground/Pavement/PavedStoneTilesA_smo.png";
   aoMap[0] = "art/models/DarkTerritory/Textures/Ground/Pavement/PavedStoneTilesA_ao.png";
   metalMap[0] = "art/models/DarkTerritory/Textures/Ground/Pavement/PavedStoneTilesA_met.png";
};
*/
//==============================================================================
// Create missing Material for TerrainMaterials
//==============================================================================

//==============================================================================
// Navigate through Creator Book Data
//generateMaterialFromFolder("art/models/DarkTerritory/Textures/");
function generateMaterialFromFolder(%folder,%overwrite,%autoLoad )
{
	%fileWrite = getFileWriteObj(%folder@"materials.txt");
	%searchExts = "*.png" TAB "*.dds" TAB "*.tga" ;
	%nextPath = findFirstFileMultiExpr(%searchExts );

	while ( %nextPath !$= "" )
	{
		%fullPath = %nextPath;
		%nextPath = findNextFileMultiExpr(%searchExts );

		// Is this file in the current folder?
		if ( !strFind(%fullPath,%folder))
		{
			continue;
		}

		devLog("Analysing textures file:",%fullPath);
		//We are looking for the diffuse image which end with predefine suffix (_d)
		%diffuseSuffix = $HLab_MatGen_Suffix["diffuseMap"];
		%texName = fileBase(%fullPath);
		%nameLen = strlen(%texName);
		%suffixLen = strlen(%diffuseSuffix);
		%startAt = %nameLen - %suffixLen ;
		%compareSuffix = getSubStr(%texName,%startAt);
		devLog("Comparing suffix for file:",%texName,"DifSuf=",%diffuseSuffix,"NameLen=",%nameLen,"Got string:",%compareSuffix);

		if (%compareSuffix !$= %diffuseSuffix)
		{
			continue;
		}

		%baseTexName = getSubStr(%texName,0,%startAt);
		%path = filePath(%fullPath);
		%subPath = strReplace(%fullPath,%folder,"");
		%pathFields = strReplace(%subPath,"/","\t");
		%firstFolder = getField(%pathFields,0);
		devLog("SubPah = ",%subPath,"FirstFolder=",%firstFolder);
		%fieldsCount = getFieldCount(%pathFields);
		%ext = fileExt(%fullPath);
		%diffuseMap = strreplace(%fullPath,%ext,"");
		%normalSuffix = $HLab_MatGen_Suffix["normalMap"];
		%normalMap = %path @"/"@%baseTexName@%normalSuffix;
		%normalFile = %normalMap@%ext;
		%smoothSuffix = $HLab_MatGen_Suffix["roughMap"];
		%roughMap = %path @"/"@%baseTexName@%smoothSuffix;
		%roughFile = %roughMap@%ext;
		%aoSuffix = $HLab_MatGen_Suffix["aoMap"];
		%aoMap = %path @"/"@%baseTexName@%aoSuffix;
		%aoFile = %aoMap@%ext;
		%metSuffix = $HLab_MatGen_Suffix["metalMap"];
		%metalMap = %path @"/"@%baseTexName@%metSuffix;
		%metalFile = %metalMap@%ext;
		devLog("Naked tex name is:",%baseTexName,"Path:",%path,"NormalCheck = ",%normalFile);

		if (isObject("Mat_baseDt"@%firstFolder@"_"@%baseTexName))
		{
			if (%overwrite)
			{
				devLog("Matfound and deleted!");
				delObj("Mat_baseDt"@%firstFolder@"_"@%baseTexName);
			}
			else
			{
				devLog("Material already exist");
				continue;
			}
		}

		%fileWrite.writeLine("//==============================================================================");
		%fileWrite.writeLine("singleton Material(Mat_baseDt"@%firstFolder@"_"@%baseTexName@")");
		%fileWrite.writeLine("{");
		%fileWrite.writeLine("" TAB "mapTo = \"baseDt"@%firstFolder@"_"@%baseTexName@"\";");
		%fileWrite.writeLine("" TAB "diffuseMap[0] = \""@%diffuseMap@"\";");

		if (isFile(%normalFile))
			%fileWrite.writeLine("" TAB "normalMap[0] = \""@%normalMap@"\";");

		if (isFile(%roughFile))
			%fileWrite.writeLine("" TAB "roughMap[0] = \""@%roughMap@"\";");

		if (isFile(%aoFile))
			%fileWrite.writeLine("" TAB "aoMap[0] = \""@%aoMap@"\";");

		if (isFile(%metalFile))
			%fileWrite.writeLine("" TAB "metalMap[0] = \""@%metalMap@"\";");

		%fileWrite.writeLine("" TAB "smoothness[0] = \"1\";");
		%fileWrite.writeLine("" TAB "metalness[0] = \"0\";");
		%fileWrite.writeLine("" TAB "translucentBlendOp = \"None\";");
		%fileWrite.writeLine("};");
		//	%fileWrite.writeLine("");
		//	%fileWrite.writeLine("");
		//We have a valid diffuse texture, check if we have a material already
	}

	closeFileObj(%fileWrite);

	if (%autoLoad)
		exec(%folder@"materials.txt");
}
/*
		if (SceneEditorTools.meshRootFolder !$= "") {
			%found = strFind(%fullPath,SceneEditorTools.meshRootFolder);
			devLog("Check path member of root:",%fullPath,"Found=",%found);

			if (!%found) {
				%fullPath = findNextFileMultiExpr( %searchExts );
				continue;
			}
		}

		if (strstr(%fullPath, "cached.dts") != -1) {
			%fullPath = findNextFileMultiExpr( %searchExts );
			continue;
		}

		%fullPath = makeRelativePath( %fullPath, getMainDotCSDir() );
		%splitPath = strreplace( %fullPath, "/", " " );

		if( getWord(%splitPath, 0) $= "tools" ) {
			%fullPath = findNextFileMultiExpr( %searchExts );
			continue;
		}

		%ext = fileExt(%fullPath);
		%dirCount = getWordCount( %splitPath ) - 1;
		%pathFolders = getWords( %splitPath, 0, %dirCount - 1 );
		// Add this file's path (parent folders) to the
		// popup menu if it isn't there yet.
		%temp = strreplace( %pathFolders, " ", "/" );
		%r = FileBrowserMenu.findText( %temp );

		if ( %r == -1 ) {
			FileBrowserMenu.add( %temp );
		}

		// Is this file in the current folder?
		if ( stricmp( %pathFolders, %address ) == 0 ) {
			%this.addFileIcon( %fullPath );
		}
		// Then is this file in a subfolder we need to add
		// a folder icon for?
		else {
			%wordIdx = 0;
			%add = false;

			if ( %address $= "" ) {
				%add = true;
				%wordIdx = 0;
			} else {
				for ( ; %wordIdx < %dirCount; %wordIdx++ ) {
					%temp = getWords( %splitPath, 0, %wordIdx );

					if ( stricmp( %temp, %address ) == 0 ) {
						%add = true;
						%wordIdx++;
						break;
					}
				}
			}

			if ( %add == true ) {
				%folder = getWord( %splitPath, %wordIdx );
				%ctrl = %this.findIconCtrl( %folder );

				if ( %ctrl == -1 )
					%this.addFolderIcon( %folder );
			}
		}

		%fullPath = findNextFileMultiExpr(%searchExts );
	}

	FileBrowserArray.sort( "alphaIconCompare" );

	for ( %i = 0; %i < FileBrowserArray.getCount(); %i++ ) {
		FileBrowserArray.getObject(%i).autoSize = false;
	}

	FileBrowser.addFolderUpIcon();
	%this.setViewId($FileBrowser_ViewId);
	FileBrowserArray.refresh();

	//FileBrowserArray.frozen = false;
	//FileBrowserArray.refresh();
	// Recalculate the array for the parent guiScrollCtrl
	FileBrowserArray.getParent().computeSizes();
	%this.address = %address;
	FileBrowserMenu.sort();
	%str = strreplace( %address, " ", "/" );
	%r = FileBrowserMenu.findText( %str );

	if ( %r != -1 )
		FileBrowserMenu.setSelected( %r, false );
	else
		FileBrowserMenu.setText( %str );

	FileBrowserMenu.tooltip = %str;
}
*/
//------------------------------------------------------------------------------