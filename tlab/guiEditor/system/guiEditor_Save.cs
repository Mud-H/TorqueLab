//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function GuiEditCanvas::save( %this, %selectedOnly, %noPrompt ) {
	// Get the control we should save.
	if( isObject(%selectedOnly) ) {
		%currentObject = %selectedOnly;
	} else if( %selectedOnly ) {
		%selected = GuiEditor.getSelection();

		//EditorGui prevention - Selection name must be same as file name
		if (GuiEditorContent.getObject( 0 ).getName() $= "EditorGui") {
			%fileNameMustMatch = %selected.getObject( 0 ).getName();
		}

		if( !%selected.getCount() )
			return;
		else if( %selected.getCount() > 1 ) {
			LabMsgOk( "Invalid selection", "Only a single control hierarchy can be saved to a file.  Make sure you have selected only one control in the tree view." );
			return;
		}

		%currentObject = %selected.getObject( 0 );
	} else if( GuiEditorContent.getCount() > 0 )
		%currentObject = GuiEditorContent.getObject( 0 );
	else
		return;

	// Store the current guide set on the control.
	if (%currentObject.preSaveFunction !$= "")
		eval(%currentObject.preSaveFunction);

	GuiEditor.writeGuides( %currentObject );
	%currentObject.canSaveDynamicFields = true; // Make sure the guides get saved out.

	// Construct a base filename.

	if( %currentObject.getName() !$= "" )
		%name =  %currentObject.getName() @ ".gui";
	else
		%name = "Untitled.gui";

	// Construct a path.

	if( %selectedOnly
			&& %currentObject != GuiEditorContent.getObject( 0 )
			&& %currentObject.getFileName() $= GuiEditorContent.getObject( 0 ).getFileName() ) {
		// Selected child control that hasn't been yet saved to its own file.
		%currentFile = GuiEditor.LastPath @ "/" @ %name;
		%currentFile = makeRelativePath( %currentFile, getMainDotCsDir() );
	} else {
		%currentFile = %currentObject.getFileName();

		if( %currentFile $= "") {
			// No file name set on control.  Force a prompt.
			%noPrompt = false;

			if( GuiEditor.LastPath !$= "" ) {
				%currentFile = GuiEditor.LastPath @ "/" @ %name;
				%currentFile = makeRelativePath( %currentFile, getMainDotCsDir() );
			} else
				%currentFile = expandFileName( %name );
		} else
			%currentFile = expandFileName( %currentFile );
	}

	// Get the filename.

	if( !%noPrompt ) {
		%filename = GuiBuilder::getSaveName( %currentFile );

		if( %filename $= "" )
			return;

		if (%fileNameMustMatch !$= "" && %fileNameMustMatch !$= fileBase(%filename)) {
			devLog("You are trying to save EditorGui selection to a file not matching the content! Operation aborted! Sel name:",%fileNameMustMatch,"Filename",fileBase(%filename));
			return;
		}
	} else
		%filename = %currentFile;

	// Save the Gui.
	if (%currentObject.getName() $= "EditorGui") {
		info("Calling the specific EditorGui save function");
		Lab.saveEditorGui(%filename);
		return;
	}

	if( isWriteableFileName( %filename ) ) {
		if (%currentObject.isMethod("onPreEditorSave"))
			%currentObject.onPreEditorSave();
 
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
		%currentObject.setFileName( makeRelativePath( %filename, getMainDotCsDir() ) );
		GuiEditorStatusBar.print( "Saved file '" @ %currentObject.getFileName() @ "'" );

		if (%currentObject.isMethod("onPostEditorSave"))
			%currentObject.onPostEditorSave();
	} else
		LabMsgOk( "Error writing to file", "There was an error writing to file '" @ %currentFile @ "'. The file may be read-only." );
}

function GuiEd::saveGuiToFile( %this, %currentObject, %filename ) {
	if( isWriteableFileName( %filename ) ) {
		if (%currentObject.isMethod("onPreEditorSave"))
			%currentObject.onPreEditorSave();
    
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
		%currentObject.setFileName( makeRelativePath( %filename, getMainDotCsDir() ) );
		GuiEditorStatusBar.print( "Saved file '" @ %currentObject.getFileName() @ "'" );

		if (%currentObject.isMethod("onPostEditorSave"))
			%currentObject.onPostEditorSave();
	} else
		LabMsgOk( "Error writing to file", "There was an error writing to file '" @ %currentFile @ "'. The file may be read-only." );
	
}
//---------------------------------------------------------------------------------------------

function GuiEditCanvas::append( %this ) {
	// Get filename.
	%openFileName = GuiBuilder::getOpenName();

	if( %openFileName $= ""
								|| ( !isFile( %openFileName )
									  && !isFile( %openFileName @ ".dso" ) ) )
		return;

	// Exec file.
	%oldRedefineBehavior = $Con::redefineBehavior;
	$Con::redefineBehavior = "renameNew";
	exec( %openFileName );
	$Con::redefineBehavior = %oldRedefineBehavior;

	// Find guiContent.

	if( !isObject( %guiContent ) ) {
		LabMsgOk( "Error loading GUI file", "The GUI content controls could not be found.  This function can only be used with files saved by the GUI editor." );
		return;
	}

	if( !GuiEditorContent.getCount() )
		GuiEditor.openForEditing( %guiContent );
	else {
		GuiEditor.getCurrentAddSet().add( %guiContent );
		GuiEditor.readGuides( %guiContent );
		GuiEditor.onAddNewCtrl( %guiContent );
		GuiEditor.onHierarchyChanged();
	}

	GuiEditorStatusBar.print( "Appended controls from '" @ %openFileName @ "'" );
}

//---------------------------------------------------------------------------------------------

function GuiEditCanvas::revert( %this ) {
	if( !GuiEditorContent.getCount() )
		return;

	%gui = GuiEditorContent.getObject( 0 );
	%filename = %gui.getFileName();

	if( %filename $= "" )
		return;

	if( LabMsgOkCancel( "Revert Gui", "Really revert the current Gui?  This cannot be undone.", "OkCancel", "Question" ) == $MROk )
		%this.load( %filename );
}

//---------------------------------------------------------------------------------------------