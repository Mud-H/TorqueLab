//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================

$LabStyleColorTypes = "Color1 Color2 Color3 Background1 Background2 Background3 DarkColor1 DarkColor2 DarkColor3 LightColor1 LightColor2 LightColor3";
$ProfileFields["general"] = "tab canKeyFocus mouseOverSelected modal opaque bitmap hasBitmapArray category";
$ProfileFields["fill"] = "fillColor fillColorHL fillColorNA fillColorSEL";
$ProfileFields["border"] = "border borderThickness borderColor borderColorHL borderColorNA bevelColorHL bevelColorLL";
$ProfileFields["misc"] = "autoSizeWidth autoSizeHeight returnTab numbersOnly cursorColor  soundButtonDown soundButtonOver profileForChildren ";
$ProfileFields["font"] = "fontUse fontType fontSize fontCharset fontColors fontColor fontColorHL fontColorNA fontColorSEL fontColorLink fontColorLinkHL justify textOffset";
$ProfileFields["border"] = "border borderThickness borderColor borderColorHL borderColorNA";

$ProfileFieldList = $ProfileFields["general"] SPC $ProfileFields["fill"] SPC $ProfileFields["border"] SPC $ProfileFields["misc"] SPC $ProfileFields["font"] SPC $ProfileFields["border"];

if( !isObject( "ProfileStyles_PM" ) )
	new PersistenceManager( ProfileStyles_PM );

//==============================================================================
// Load the Specific UI Style settings
function Lab::initProfileStyleData(%this,%style) {
	$LabStyleGroup = newSimSet("LabStyleGroup");
	$LabProfileGroup = newSimSet("LabProfileGroup");
	$LabProfileList = "";

	//Init the updatable fields globals (Quick check only for now)
	foreach$(%field in $ProfileFieldList)
		$LabProfileField[%field] = "True";

	foreach( %obj in GuiDataGroup ) {
		if( !%obj.isMemberOfClass( "GuiControlProfile" ) )
			continue;

		%catCheck = getSubStr(%obj.category,0,4);

		if(%catCheck !$= "Edit" && %catCheck !$= "Tool"  ) continue;

		$LabProfileList = trim($LabProfileList SPC %obj.getName());
		$LabProfileObject[%obj.getName()] = %obj;
		LabProfileGroup.add( %obj);
	}

	//Now load the Current style
	// Lab.loadGuiProfileStyle();
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the Specific UI Style settings
function Lab::scanProfileStyles(%this) {
	$LabProfileStyles = "";
	%searchPat = "tlab/gui/styles/*.styles.cs";

	for(%file = findFirstFile(%searchPat); %file !$= ""; %file = findNextFile(%searchPat)) {
		%style = fileBase(fileBase(%file));
		$LabProfileStyles = strAddWord($LabProfileStyles,%style);
	}

	devLog("List of styles found:",$LabProfileStyles);
}
//------------------------------------------------------------------------------

//==============================================================================
// Load the Specific UI Style settings
function Lab::saveAllDirtyProfilesStyle(%this) {
	foreach$(%profile in $LabProfileStylesDirtyList)
		ProfileStyles_PM.setDirty( %profile );

	ProfileStyles_PM.saveDirty();
}
//------------------------------------------------------------------------------