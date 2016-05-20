//==============================================================================
// HelpersLab -> ForestData Brush and Items Generator
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$HLab_ForestDataGen_Folder = "art/models/boostEnv/";

$HLab_ForestDataGen_Default["scaleMin"] = 1;
$HLab_ForestDataGen_Default["scaleMax"] = 1;
$HLab_ForestDataGen_Default["scaleExponent"] = 1;
$HLab_ForestDataGen_Default["scaleVar"] = 0;
$HLab_ForestDataGen_Default["sinkMin"] = 0;
$HLab_ForestDataGen_Default["sinkMax"] = 0;
$HLab_ForestDataGen_Default["sinkRadius"] = 1;
$HLab_ForestDataGen_Default["sinkVar"] = 0;
$HLab_ForestDataGen_Default["slopeMin"] = 0;
$HLab_ForestDataGen_Default["slopeMax"] = 90;
$HLab_ForestDataGen_Default["slopeVar"] = 0;
$HLab_ForestDataGen_Default["elevationMin"] = -1000;
$HLab_ForestDataGen_Default["elevationMax"] = 1000;
$HLab_ForestDataGen_Default["elevationVar"] = 0;
$HLab_ForestDataGen_Fields = "scaleMin scaleMax scaleExponent sinkMin sinkMax sinkRadius slopeMin slopeMax elevationMin elevationMax";
$HLab_ForestDataGen_VarFields = "scaleVar sinkVar slopeVar elevationVar";
//==============================================================================
// Start forest data generation functions
//==============================================================================


//==============================================================================
/// Generate forest brush elements and items from models found in specified folder
/// %baseFolder : Folder containing the models used for forest items
/// %name : Name of the root brush group containing generated brushes
/// %prefix : Prefix added to generated brush name (usefull to use brush filter)
/// %settingContainer : GuiControl containing children with setting as internalName
function buildForestDataFromFolder(%baseFolder,%name,%prefix,%settingContainer,%deleteExisting,%isMissionItems)
{
	%fullFolder = %baseFolder@"/";
	%createBrushGroup = true;

	foreach$(%field in $HLab_ForestDataGen_Fields SPC $HLab_ForestDataGen_VarFields)
	{
		%elementVal[%field] = $HLab_ForestDataGen_Default[%field];
		logd("Element value set to DEFAULT:",%elementVal[%field]);
	}

	if (isObject(%settingContainer))
	{
		foreach$(%field in $HLab_ForestDataGen_Fields SPC $HLab_ForestDataGen_VarFields)
		{
			%ctrl = %settingContainer.findObjectByInternalName(%field,true);

			if (%ctrl.getText() !$="")
				%elementVal[%field] = %ctrl.getText();

			logd("Element value set to:",%elementVal[%field]);
		}
	}

	if (!isDirectory(%fullFolder ))
	{
		warnLog("Invalid folder to serach for forest models:",%fullFolder);
		return;
	}

	if (%name $= "")
		%name = getParentFolder(%baseFolder);

	%prefixItem = "FI_";

	if (%isMissionItems)
	{
		%prefixItem = "MFI_";
	}

	%itemFile = generateForestItemsFromFolder(%baseFolder,%prefixItem,%deleteExisting,%isMissionItems);
	%rootGroup = new SimGroup()
	{
		internalName = %name;
	};
	%searchFolder = %fullFolder@"*.dae";

	//Now go through each files again to add a brush with latest items
	for(%daeFile = findFirstFile(%searchFolder); %daeFile !$= ""; %daeFile = findNextFile(%searchFolder))
	{
		%intName = %prefix@"_"@fileBase(%daeFile);
		%dataName = %prefixItem@fileBase(%daeFile);
		%filePath = filePath(%daeFile);
		%subFolders = strreplace(%filePath,%baseFolder,"");
		%subFolders = trim(strreplace(%subFolders,"/"," "));

		if (%subFolders $= "")
			%subFolders = getParentFolder(%filePath);

		%level = 1;
		%parentGroup = %rootGroup;
		%folderCount = getWordCount(%subFolders);
		%folderId = 0;

		foreach$(%subFolder in %subFolders)
		{
			%folderId++;

			if (%folderId >= %folderCount && !isObject(%subGroup[%subFolder]))
			{
				%subGroup[%subFolder] = new ForestBrush()
				{
					internalName = %subFolder;
					parentGroup = %parentGroup;
				};
				%parentGroup.add(%subGroup[%subFolder]);
			}
			else if (!isObject(%subGroup[%subFolder]))
			{
				%subGroup[%subFolder] = new SimGroup()
				{
					internalName = %subFolder;
				};
				%parentGroup.add(%subGroup[%subFolder]);
			}

			%parentGroup = %subGroup[%subFolder];
		}

		if (%deleteExisting)
		{
			%existBrush = ForestBrushGroup.findObjectByInternalName(%intName,true);
			delObj(%existBrush);
		}

		%newBrush = new ForestBrushElement()
		{
			internalName = %intName;
			canSave = "1";
			canSaveDynamicFields = "1";
			ForestItemData = %dataName;
			probability = "1";
			rotationRange = "360";
			scaleMin = "1";
			scaleMax = "1";
			scaleExponent = "1";
			sinkMin = "0";
			sinkMax = "0";
			sinkRadius = "1";
			slopeMin = "0";
			slopeMax = "90";
			elevationMin = "-10000";
			elevationMax = "10000";
		};

		foreach$(%field in $HLab_ForestDataGen_Fields)
			%newBrush.setFieldValue(%field,%elementVal[%field]);

		//Check for random variations fields
		foreach$(%field in $HLab_ForestDataGen_VarFields)
		{
			%fieldBase = strreplace(%field,"Var","");
			%value = %elementVal[%field];
			logd("Variation=",%value,"%fieldBase",%fieldBase,"%field",%field);

			if (%value $= "" || %value == 0)
				continue;

			%fieldMin = %fieldBase @ "Min";
			%currenMin = %newBrush.getFieldValue(%fieldMin);
			%varRangeMin = %currenMin * %value / 100;
			%varMin = getRandom(-%varRangeMin*100,%varRangeMin*100);
			%newMin = %currenMin + (%varMin / 100);
			%fieldMax = %fieldBase @ "Max";
			%currenMax = %newBrush.getFieldValue(%fieldMax);
			%varRangeMax = %currenMax * %value / 100;
			%varMax = getRandom(-%varRangeMax*100,%varRangeMax*100);
			%newMax = %currenMax + (%varMax / 100);
			logd("Variation=",%value);
			logd("MIN %varRangeMin = ",	%varRangeMin,"%currenMin=",%currenMin,"%newMin=",%newMin);
			logd("MAX %varRangeMax = ",	%varRangeMax,"%currenMax=",%currenMax,"%newMax=",%newMax);
			%newBrush.setFieldValue(%fieldMin,%newMin);
			%newBrush.setFieldValue(%fieldMax,%newMax);
		}

		%parentGroup.add(%newBrush);
	}

	if (%backup)
	{
		%brushFile = "art/forest/tmp_"@%name@"Brushes.txt";
		%brushWrite = new FileObject();
		%brushWrite.OpenForWrite(%brushFile);
		%brushWrite.writeObject(%rootGroup);
		%brushWrite.close();
		%brushWrite.delete();
		ForestBrushGroup.save("art/forest/brushes.backup");
	}

	if (!isObject(ForestBrushGroup))
		exec("art/forest/brushes.cs");

	if (%isMissionItems && isFile(%itemFile))
	{
		%missionBrushGroup = "MissionForestBrushGroup";

		if (!isObject(%missionBrushGroup))
		{
			%missionBrushGroup = newSimGroup("MissionForestBrushGroup","","MissionForestBrushes");
			//	%missionBrushGroup.add(%rootGroup);
			//	%missionBrushGroup.setFileName(%itemFile);
			//%itemWrite = getFileWriteObj(%itemFile,true);
			//%itemWrite.writeObject(%missionBrushGroup);
			//closeFileObj(%itemWrite);
			//return;
		}

		%missionBrushGroup.setFileName(%itemFile);
		%missionBrushGroup.add(%rootGroup);
		%missionBrushGroup.save(%itemFile);
		return;
	}

	ForestBrushGroup.add(%rootGroup);
	ForestBrushGroup.save( "art/forest/brushes.cs" );
}
//------------------------------------------------------------------------------
//==============================================================================
function generateForestItemsFromFolder(%baseFolder,%prefix,%deleteExisting,%isMissionItems)
{
	%fullFolder = %baseFolder@"/";
	%folderWords = trim(strreplace(%fullFolder,"/"," "));
	%folderName = getLastWord(%folderWords);

	if (%isMissionItems)
	{
		/*%missionItemGroup = MissionGroup-->ForestItems;
		if (!isObject(%missionItemGroup)){
			%missionItemGroup = new SimGroup();
			%missionItemGroup.internalName = "ForestItems";
			%missionItemGroup.parentGroup = "MissionGroup";
		}
		%missionItemGroup.visible = 0;*/
		%missionFile = MissionGroup.getFilename();
		%missionPath = filePath(%missionFile);
		%missionFileName = fileBase(%missionFile);
		%itemFile = %missionPath@"/"@%missionFileName@".forestitems.cs";
	}
	else
	{
		%itemFile = "art/forest/auto/fh_"@%folderName@"Items.cs";
	}

	%itemWrite = getFileWriteObj(%itemFile,isFile(%itemFile));
	%searchFolder = %fullFolder@"*.dae";
	logd("%baseFolder",%baseFolder,"%searchFolder",%searchFolder);

	for(%daeFile = findFirstFile(%searchFolder); %daeFile !$= ""; %daeFile = findNextFile(%searchFolder))
	{
		logd("File",fileBase(%daeFile));
		%intName = fileBase(%daeFile);
		//%intName = %prefix@"_"@fileBase(%daeFile);
		%dataName = %prefix@%intName;

		if (isObject(%dataName))
			continue;

		%itemWrite.writeLine("datablock TSForestItemData("@%dataName@")");
		%itemWrite.writeLine("{");
		%itemWrite.writeLine("internalName = \"item_"@%intName@"\";");
		%itemWrite.writeLine("shapeFile = \""@%daeFile@"\";");
		%itemWrite.writeLine("};");
	}

	closeFileObj(%itemWrite);
	exec(%itemFile);
	return %itemFile;
}
//------------------------------------------------------------------------------
