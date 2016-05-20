//==============================================================================
// TorqueLab -> Core Menubar handlers
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function EditorOpenDeclarationInTorsion( %object ) {
	%fileName = %object.getFileName();

	if( %fileName $= "" )
		return;

	EditorOpenFileInTorsion( makeFullPath( %fileName ), %object.getDeclarationLine() );
}


function EditorOpenFileInTorsion( %file, %line ) {
	// Make sure we have a valid path to the Torsion installation.
	%torsionPath = $Cfg_TorsionPath;

	if( !isFile( %torsionPath ) ) {
		LabMsgOK(
			"Torsion Not Found",
			"Torsion not found at '" @ %torsionPath @ "'.  Please set the correct path in the preferences."
		);
		return;
	}

	// If no file was specified, take the current mission file.

	if( %file $= "" )
		%file = makeFullPath( $Server::MissionFile );

	// Open the file in Torsion.
	%args = "\"" @ %file;

	if( %line !$= "" )
		%args = %args @ ":" @ %line;

	%args = %args @ "\"";
	shellExecute( %torsionPath, %args );
}


function EditorOpenTorsionProject( %projectFile ) {
	// Make sure we have a valid path to the Torsion installation.
	%torsionPath = $Cfg_TorsionPath;

	if( !isFile( %torsionPath ) ) {
		LabMsgOK(
			"Torsion Not Found",
			"Torsion not found at '" @ %torsionPath @ "'.  Please set the correct path in the preferences."
		);
		return;
	}

	// Determine the path to the .torsion file.

	if( %projectFile $= "" ) {
		%projectName = fileBase( getExecutableName() );
		%projectFile = makeFullPath( %projectName @ ".torsion" );

		if( !isFile( %projectFile ) ) {
			%projectFile = findFirstFile( "*.torsion", false );

			if( !isFile( %projectFile ) ) {
				LabMsgOK(
					"Project File Not Found",
					"Cannot find .torsion project file in '" @ getMainDotCsDir() @ "'."
				);
				return;
			}
		}
	}

	// Open the project in Torsion.
	shellExecute( %torsionPath, "\"" @ %projectFile @ "\"" );
}