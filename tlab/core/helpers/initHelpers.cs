//==============================================================================
// GameLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

$HLab_ScreenshotsFolder = "core/dump/screenshots/";
//-----------------------------------------------------------------------------
// Load up our main GUI which lets us see the game.

//============================================================================
//Taking screenshot
function loadToolsHelpers()
{
	%pattern = "./*.cs";

	for( %file = findFirstFile( %pattern ); %file !$= ""; %file = findNextFile( %pattern))
	{
		if (fileBase(%file) $= "initHelpers") continue;

		exec(%file);
	}

	%pattern = "./*.cs.dso";

	for( %file = findFirstFile( %pattern ); %file !$= ""; %file = findNextFile( %pattern))
	{
		if (fileBase(%file) $= "initHelpers") continue;

		exec(%file);
	}

	$HelperLabLoaded = true;
}
//----------------------------------------------------------------------------
//Load the helpers on execution
loadToolsHelpers();