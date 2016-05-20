//==============================================================================
// Lab GuiManager -> Profile File Analyser
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

$ProfileFieldSet["colorFontIds"] = "fontColors[0] fontColors[1] fontColors[2] fontColors[3] fontColors[4] fontColors[5] fontColors[6] fontColors[7] fontColors[8] fontColors[9]";
$ProfileFieldSet["colorFont"] = "fontColor fontColorHL fontColorNA fontColorSEL fontColorLink fontColorLinkHL";
$ProfileFieldSet["All"] = $ProfileFieldSet["colorFontIds"] SPC $ProfileFieldSet["colorFont"];
$ProfileFieldsStore = "fontSize";
//==============================================================================
// Clear the Analyser saved globals
function clearProfilesGlobals() {
	foreach$(%cType in $ProfileUpdateColorList)
		$ProfileListColor[%cType] = "";

	GLab.resetFontProfilesList();
	$ProfileList["fontSource"] = "";

	foreach$(%prof in $ProfWithChilds)
		$ProfChilds[%prof] = "";

	$ProfWithChilds = "";

	foreach$(%prof in $ProfWithParent)
		$ProfParent[%prof] = "";

	$ProfWithParent = "";
}
//------------------------------------------------------------------------------
//==============================================================================
// Create a analyse console report of the scan result
function postProfileScanReport() {
	info("Profiles with childs:",$ProfWithChilds);

	foreach$(%prof in $ProfWithChilds) {
		info(%prof,"Childs:",$ProfChilds[%prof]);
	}

	info("Profiles with Parent:",$ProfWithParent);

	foreach$(%prof in $ProfWithParent) {
		info(%prof,"Parent:",$ProfParent[%prof]);
	}

	info("====================================","--","==================================");
	info("Store fields info");

	foreach$(%field in $ProfileFieldsStore) {
		foreach$(%profileName in $ProfStoreFieldProfiles[%field] ) {
			%value = $ProfStoreFieldDefault[%profileName,%field];
			info("Field:",%field,"in Profile:",%profileName,"Store as:",%value);
		}
	}

	info("====================================","--","==================================");
	info("Profile Color Set Info");

	foreach$(%cType in $ProfileUpdateColorList)
		info(%cType,"Profiles:",$ProfileListColor[%cType]);
}
//------------------------------------------------------------------------------

//==============================================================================
// Scan all profile files found in profile folder
function scanAllToolProfileFile(%postReport) {
	clearProfilesGlobals();
	scanProfileFile("tlab/gui/profiles/baseProfiles.cs");
	%filePathScript = "tlab/gui/profiles/*.prof.cs";

	for(%file = findFirstFile(%filePathScript); %file !$= ""; %file = findNextFile(%filePathScript)) {
		scanProfileFile(%file);
	}

	$ProfileScanDone = true;

	if (%postReport)
		postProfileScanReport();
}
//------------------------------------------------------------------------------
//==============================================================================
// Scan all profile files found in profile folder
function scanAllProfileFile(%postReport) {
	clearProfilesGlobals();
	scanProfileFile("art/gui/baseProfiles.cs");
	%filePathScript = "art/gui/*.prof.cs";

	for(%file = findFirstFile(%filePathScript); %file !$= ""; %file = findNextFile(%filePathScript)) {
		scanProfileFile(%file);
	}

	$ProfileScanDone = true;

	if (%postReport)
		postProfileScanReport();

	if ( IsDirectory( "tlab/" ) )
		scanAllToolProfileFile(%postReport);
}
//------------------------------------------------------------------------------
//==============================================================================
// Scan a profile line by line and get needed data
function scanProfileFile( %file ) {
	%fileObj = getFileReadObj(%file);

	if (!isObject(%fileObj)) return;

	while( !%fileObj.isEOF() ) {
		%line = %fileObj.readline();

		//------------------------------------------------------------------------
		//Check for GuiControlProfile definition START
		if (strstr(%line,"GuiControlProfile") !$= "-1") {
			%lineFix =  strchr( %line , "(" );
			%lineFix = strReplace(%lineFix,")"," ");
			%lineFix = strReplace(%lineFix,"{","");
			%lineFix = trim(strReplace(%lineFix,"(",""));
			%lineFix = strReplace(%lineFix,":","\t");
			%profileName = trim(getField(%lineFix,0));
			%profileLink = trim(getField(%lineFix,1));
			//Set an empty global to store the fields that this profile own
			$ProfOwnedFields[%profileName] = "";

			//Check for reference to other profile
			if (isObject(%profileLink)) {
				$ProfChilds[%profileLink] = strAddWord($ProfChilds[%profileLink],%profileName);
				$ProfWithChilds = strAddWord($ProfWithChilds,%profileLink,true);
				$ProfParent[%profileName] = %profileLink;
				$ProfWithParent = strAddWord($ProfWithParent,%profileName,true);
			}
		}
		//------------------------------------------------------------------------
		//Check for GuiControlProfile definition END
		else if (strstr(%line,"};") !$= "-1") {
			%profileName = "";
			%fontType = "";
		}
		//------------------------------------------------------------------------
		//Check for GuiControlProfile field definition
		else if (strstr(%line,"=") !$= "-1") {
			%trimLine = trim(%line);
			%trimLine = strreplace(%trimLine,"="," ");
			%trimLine = strreplace(%trimLine,"\"","");
			%trimLine = strreplace(%trimLine,";","");
			%field = trim(getWord(%trimLine,0));
			%value = trim(removeWord(%trimLine,0));
			$ProfOwnedFields[%profileName] = strAddWord($ProfOwnedFields[%profileName],%field,true);

			if (strstr($ProfileFieldsStore,%field) !$= "-1" ) {
				$ProfStoreFieldProfiles[%field] =  strAddWord($ProfStoreFieldProfiles[%field],%profileName);
				$ProfStoreFieldDefault[%profileName,%field] = %value;
			}

			if (%field $= "fontSource" ) {
				$ProfileList["fontSource"] = strAddWord($ProfileList["fontSource"],%profileName);
			}

			if (%field $= "fontType") {
				$ProfileFontList[%value] = strAddWord($ProfileFontList[%value],%profileName);
				$FontTypesList =  strAddWord($FontTypesList,%value);
			}

			if (strstr($ProfileUpdateColorList,%field) !$= "-1" ) {
				$ProfileListColor[%field]  = strAddWord($ProfileListColor[%field],%profileName);
			}
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
//cleanProfileFile("art/gui/TextLab.prof.cs");
function removeProfileFieldSet( %profile,%set ) {
	%targetFields = $ProfileFieldSet[%set];

	if (%targetFields !$= "")
		removeProfileField(%profile,%targetFields);
}

function removeProfileField( %profile,%targetFields ) {
	devLog("removeProfileField",%profile.getName(),%targetFields);
	%file = %profile.getFilename();

	if (!isFile(%file))
		return;

	%fileObj = getFileReadObj(%file);

	if (!isObject(%fileObj)) return;

	while( !%fileObj.isEOF() ) {
		%line = %fileObj.readline();
		%skipLine = false;

		if (strstr(%line,"//") !$= "-1") {
			%skipLine = false;
		} else if (strstr(%line,"GuiControlProfile") !$= "-1") {
			// Prints 3456789
			%lineFix =  strchr( %line , "(" );
			%lineFix = strReplace(%lineFix,":"," ");
			%lineFix = strReplace(%lineFix,")"," ");
			%lineFix = trim(strReplace(%lineFix,"(",""));
			%profileName = getWord(%lineFix,0);
			%targetProfile = false;

			if (%profileName $= %profile.getName()) {
				%targetProfile = true;
			}
		} else if (strstr(%line,"};") !$= "-1") {
			//Check if default field is there
			if (%fontType !$= "") {
				%line[%i++] = "fontType = \"" @%fontType@"\";";
			}

			%currentProfile = "";
			%fontType = "";
		} else if (!%targetProfile) {
			%targetProfile = %targetProfile;
		} else if (strstr(%line,"=") !$= "-1") {
			%trimLine = trim(%line);
			%trimLine = strreplace(%trimLine,"="," ");
			%trimLine = strreplace(%trimLine,"\"","");
			%trimLine = strreplace(%trimLine,";","");
			%field = trim(getWord(%trimLine,0));
			%value = trim(removeWord(%trimLine,0));

			foreach$(%target in %targetFields) {
				if (%field $= %target) {
					%skipLine = true;
					//Now we should set the parent value right now
					%parent = GLab.findParentFieldSource(%profileName,%field);
					%parentValue = %parent.getFieldValue(%field);

					if (%parentValue !$= "")
						GLab.updateProfileField(%profileName,%field,%parentValue,true);

					break;
				}
			}
		}

		if (%skipLine) {
			continue;
		}

		%line[%i++] = %line;
	}

	closeFileObj(%fileObj);
	%fileObj = getFileWriteObj(%file);
	info("-----------------SavingFile",%file);

	//%j = 1;
	for(%j=1; %j <= %i; %j++) {
		%fileObj.writeLine(%line[%j]);
	}

	closeFileObj(%fileObj);

	if (%profile.getId() $= $GLab_SelectedObject.getId())
		GLab.syncProfileParamArray();
}
//------------------------------------------------------------------------------