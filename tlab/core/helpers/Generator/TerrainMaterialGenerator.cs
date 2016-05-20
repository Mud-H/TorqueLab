//==============================================================================
// HelpersLab -> ForestData Brush and Items Generator
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Create missing Material for TerrainMaterials
//==============================================================================

//==============================================================================
/// Generate forest brush elements and items from models found in specified folder
/// %baseFolder : Folder containing the models used for forest items
/// %name : Name of the root brush group containing generated brushes
/// %prefix : Prefix added to generated brush name (usefull to use brush filter)
/// %settingContainer : GuiControl containing children with setting as internalName
function fixTerrainMaterials(%loadNew,%reset)
{
	%searchFolder = "art/terrains/*materials.cs";

	//Now go through each files again to add a brush with latest items
	for(%matFile = findFirstFile(%searchFolder); %matFile !$= ""; %matFile = findNextFile(%searchFolder))
	{
		%fileObj = getFileReadObj(%matFile);
		%folder = filePath(%matFile);
		%fileBase = fileBase(%matFile);
		%newMatFile = %folder@"/pre_"@%fileBase@".cs";
		%fileWrite = "";

		while( !%fileObj.isEOF() )
		{
			%line = %fileObj.readLine();

			if (strFind(%line,"TerrainMaterial"))
			{
				%inTerrainMaterial = true;
			}
			else if(%inTerrainMaterial && strFind(%line,"diffuseMap"))
			{
				%lineFields = strreplace(%line,"\"","\t");
				%difMap = getField(%lineFields,getFieldCount(%lineFields)-2);
				%difMapName = fileBase(%difMap);
				%matObj = "Mat_"@%difMapName;
				%matObj = strReplaceList(%matObj,"-" TAB "_" NL " " TAB "_");

				if (isObject(%matObj) && %reset)
					delObj(%matObj);

				if (!isObject(%matObj))
				{
					if (!isObject(%fileWrite))
					{
						%fileWrite = getFileWriteObj(%newMatFile);
						%fileWrite.writeLine("//==============================================================================");
						%fileWrite.writeLine("//Auto generated material for TerrainMaterial in:"@%fileBase@"");
						%fileWrite.writeLine("//------------------------------------------------------------------------------");
						%fileWrite.writeLine("//==============================================================================");
					}

					%fileWrite.writeLine("//==============================================================================");
					%fileWrite.writeLine("singleton Material("@%matObj@")");
					%fileWrite.writeLine("{");
					%fileWrite.writeLine("	mapTo = \""@%difMapName@"\";");
					%fileWrite.writeLine("	footstepSoundId = 0;");
					%fileWrite.writeLine("	terrainMaterials = \"1\";");
					%fileWrite.writeLine("	ShowDust = \"1\";");
					%fileWrite.writeLine("	showFootprints = \"1\";");
					%fileWrite.writeLine("	materialTag0 = \"Terrain\";");
					%fileWrite.writeLine("	impactSoundId = 0;");
					%fileWrite.writeLine("};");
					%fileWrite.writeLine("//------------------------------------------------------------------------------");
				}
			}
			else if (strFind(%line,"};"))
			{
				%inTerrainMaterial = false;
			}
		}

		if (isObject(%fileWrite))
		{
			closeFileObj(%fileWrite);

			if (%loadNew)
				exec(%newMatFile);
		}

		closeFileObj(%fileObj);
	}

	fileBase("art/test/none.txt");
}
//------------------------------------------------------------------------------

function fixTerrainMaterialsStock(%loadNew,%reset)
{
	%searchFolder = "art/terrains/*materials.cs";

	//Now go through each files again to add a brush with latest items
	for(%matFile = findFirstFile(%searchFolder); %matFile !$= ""; %matFile = findNextFile(%searchFolder))
	{
		%fileObj = new FileObject();
		// Open a text file, if it exists
		%fileObj.OpenForRead(%matFile);
		%folder = filePath(%matFile);
		%fileBase = fileBase(%matFile);
		%newMatFile = %folder@"/pre_"@%fileBase@".cs";
		%fileWrite = "";

		while( !%fileObj.isEOF() )
		{
			%line = %fileObj.readLine();

			if (strFind(%line,"TerrainMaterial"))
			{
				%inTerrainMaterial = true;
			}
			else if(%inTerrainMaterial && strFind(%line,"diffuseMap"))
			{
				%lineFields = strreplace(%line,"\"","\t");
				%difMap = getField(%lineFields,getFieldCount(%lineFields)-2);
				%difMapName = fileBase(%difMap);
				%matObj = "Mat_"@%difMapName;
				%matObj = strReplaceList(%matObj,"-" TAB "_" NL " " TAB "_");

				if (isObject(%matObj) && %reset)
					delObj(%matObj);

				if (!isObject(%matObj))
				{
					if (!isObject(%fileWrite))
					{
						%fileWrite = new FileObject();
						%fileWrite.OpenForWrite(%newMatFile);
						%fileWrite.writeLine("//==============================================================================");
						%fileWrite.writeLine("//Auto generated material for TerrainMaterial in:"@%fileBase@"");
						%fileWrite.writeLine("//------------------------------------------------------------------------------");
						%fileWrite.writeLine("//==============================================================================");
					}

					%fileWrite.writeLine("//==============================================================================");
					%fileWrite.writeLine("singleton Material("@%matObj@")");
					%fileWrite.writeLine("{");
					%fileWrite.writeLine("	mapTo = \""@%difMapName@"\";");
					%fileWrite.writeLine("	footstepSoundId = 0;");
					%fileWrite.writeLine("	terrainMaterials = \"1\";");
					%fileWrite.writeLine("	ShowDust = \"1\";");
					%fileWrite.writeLine("	showFootprints = \"1\";");
					%fileWrite.writeLine("	materialTag0 = \"Terrain\";");
					%fileWrite.writeLine("	impactSoundId = 0;");
					%fileWrite.writeLine("}");
					%fileWrite.writeLine("//------------------------------------------------------------------------------");

					if (%loadNew)
					{
						%newMat = singleton Material(%matObj)
						{
							mapTo = %difMapName;
							footstepSoundId = 0;
							terrainMaterials = "1";
							ShowDust = "1";
							showFootprints = "1";
							materialTag0 = "Terrain";
							effectColor[0] = "0.42 0.42 0 1";
							effectColor[1] = "0.42 0.42 0 1";
							impactSoundId = "0";
						};
						%newMat.setFilename(%newMatFile);
						devLog("NewMat created:",%newMat,"File",%newMat.getFileName());
					}
				}
			}
			else if (strFind(%line,"};"))
			{
				%inTerrainMaterial = false;
			}
		}

		if (isObject(%fileWrite))
		{
			%fileWrite.close();
			%fileWrite.delete();
		}

		%fileObj.close();
		%fileObj.delete();
	}
}
//------------------------------------------------------------------------------