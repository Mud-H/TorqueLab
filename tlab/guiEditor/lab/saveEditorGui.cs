//==============================================================================
// TorqueLab -> EditorGui Save function (Can't save with all added controls)
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
//Close editor call
function Lab::saveEditorGui(%this,%filename) {
	%currentObject = EditorGui;

	if (!Lab.editorGuisDetached) {
		warnLog("Undetached Editor Saving disabled");
		return;
		Lab.detachAllEditorGuis();
		%reattach = true;
	}

	if( isWriteableFileName( %filename ) ) {
		//
		// Extract any existent TorqueScript before writing out to disk
		//
		%fileObject = new FileObject();
		%fileObject.openForRead( %filename );
		%skipLines = true;
		%beforeObject = true;
		// %var++ does not post-increment %var, in torquescript, it pre-increments it,
		// because ++%var is illegal.
		%lines = -1;
		%beforeLines = -1;
		%skipLines = false;

		while( !%fileObject.isEOF() ) {
			%line = %fileObject.readLine();

			if( %line $= "//--- OBJECT WRITE BEGIN ---" )
				%skipLines = true;
			else if( %line $= "//--- OBJECT WRITE END ---" ) {
				%skipLines = false;
				%beforeObject = false;
			} else if( %skipLines == false ) {
				if(%beforeObject)
					%beforeNewFileLines[ %beforeLines++ ] = %line;
				else
					%newFileLines[ %lines++ ] = %line;
			}
		}

		%fileObject.close();
		%fileObject.delete();
		%fo = new FileObject();
		%fo.openForWrite(%filename);

		// Write out the captured TorqueScript that was before the object before the object
		for( %i = 0; %i <= %beforeLines; %i++)
			%fo.writeLine( %beforeNewFileLines[ %i ] );

		%fo.writeLine("//--- OBJECT WRITE BEGIN ---");
		%fo.writeObject(%currentObject, "%guiContent = ");
		%fo.writeLine("//--- OBJECT WRITE END ---");

		// Write out captured TorqueScript below Gui object
		for( %i = 0; %i <= %lines; %i++ )
			%fo.writeLine( %newFileLines[ %i ] );

		%fo.close();
		%fo.delete();
		EditorGui.setFileName( makeRelativePath( %filename, getMainDotCsDir() ) );
		GuiEditorStatusBar.print( "Saved file '" @ EditorGui.getFileName() @ "'" );
	} else
		LabMsgOkCancel( "Error writing to file", "There was an error writing to file '" @ %filename @ "'. The file may be read-only.", "Ok", "Error" );

	if (%reattach) {
		Lab.attachAllEditorGuis();
	}
}
