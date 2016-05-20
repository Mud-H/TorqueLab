//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================

function EditorExportToCollada() {
	if ( !$Pref::disableSaving && !isWebDemo() ) {
		%dlg = new SaveFileDialog() {
			Filters        = "COLLADA Files (*.dae)|*.dae|";
			DefaultPath    = $Pref::WorldEditor::LastPath;
			DefaultFile    = "";
			ChangePath     = false;
			OverwritePrompt   = true;
		};
		%ret = %dlg.Execute();

		if ( %ret ) {
			$Pref::WorldEditor::LastPath = filePath( %dlg.FileName );
			%exportFile = %dlg.FileName;
		}

		if( fileExt( %exportFile ) !$= ".dae" )
			%exportFile = %exportFile @ ".dae";

		%dlg.delete();

		if ( !%ret )
			return;

		if ( Lab.currentEditor.getId() == ShapeLabPlugin.getId() )
			ShapeLabShapeView.exportToCollada( %exportFile );
		else
			EWorldEditor.colladaExportSelection( %exportFile );
	}
}

//------------------------------------------------------------------------------
