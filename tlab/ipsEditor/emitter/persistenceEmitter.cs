
function IPSP_Editor::doEmitterSave( %this ) {
	IPSE_Editor.saveEmitter(); 
	//IPSP_Editor.saveParticle( IPSP_Editor.currParticle );
}
//---------------------------------------------------------------------------------------------

function IPSE_Editor::onNewEmitter( %this ) {
	if(    isObject( IPSE_Editor.currEmitter )
			 && IPSE_Editor.currEmitter $= IPSE_Selector.getSelected() )
		return;

	//FIXME: disregards particle tab dirty state

	if( IPSE_Editor.dirty ) {
		if( IPSP_Editor.dirty ) {
			MessageBoxYesNo("Save Existing Particle?",
								 "Do you want to save changes to <br><br>" @ IPSP_Editor.currParticle.getName(),
								 "IPSP_Editor.saveParticle(" @ IPSP_Editor.currParticle @ ");"
								);
		}

		%savedEmitter = IPSE_Editor.currEmitter;
		MessageBoxYesNoCancel("Save Existing Emitter?",
									 "Do you want to save changes to <br><br>" @ %savedEmitter.getName(),
									 "IPSE_Editor.saveEmitter(" @ %savedEmitter@ "); IPSE_Editor.loadNewEmitter();",
									 "IPSE_Editor.saveEmitterDialogDontSave(" @ %savedEmitter @ "); IPSE_Editor.loadNewEmitter();"
									);
	} else {
		IPSE_Editor.loadNewEmitter();
	}
}

//---------------------------------------------------------------------------------------------

function IPSE_Editor::loadNewEmitter( %this, %emitter ) {
	if( isObject( %emitter ) )
		%current = %emitter.getId();
	else
		%current = IPSE_Selector.getSelected();
	
	$IPS_AutoLoop = false;
	IPSE_Editor-->infiniteLoop.setStateOn( %current.lifetimeMS == 0 );
	IPSE_Editor.currEmitter = %current;
	%notDirtyData = $IPS_ClassDirty[%current.getClassName()];
	%notDirtyData.assignFieldsFrom( %current );
	%notDirtyData.originalName = %current.name;
	IPSE_Editor.guiSync();
	IPSE_Editor.setEmitterNotDirty();
	IPSP_Editor.loadNewParticle( getWord( %current.particles, 0 ) );
	$IPS_NodeMode = "Emitter";
	IpsEditor.updateEmitterNode();
	
}

//---------------------------------------------------------------------------------------------

function IPSE_Editor::setEmitterDirty( %this ) {
	IPSE_Editor.text = "Emitter *";
	IPSE_Editor.dirty = true;
	%emitter = IPSE_Editor.currEmitter;

	if( %emitter.getFilename() $= "" || %emitter.getFilename() $= "tlab/IpsEditor/particleEmitterEditor.ed.cs" )
		IPS_EmitterSaver.setDirty( %emitter, $IPSE_Editor_DEFAULT_FILENAME );
	else
		IPS_EmitterSaver.setDirty( %emitter );
}

//---------------------------------------------------------------------------------------------

function IPSE_Editor::setEmitterNotDirty( %this ) {
	IPSE_Editor.text = "Emitter";
	IPSE_Editor.dirty = false;
	IPS_EmitterSaver.clearAll();
}

//---------------------------------------------------------------------------------------------

// Create Functionality
function IPSE_Editor::showNewDialog( %this ) {
	//FIXME: disregards particle tab dirty state

	// Open a dialog if the current emitter is dirty.
	if( IPSP_Editor.dirty ) {
		MessageBoxYesNo("Save Existing Particle?",
							 "Do you want to save changes to <br><br>" @ IPSP_Editor.currParticle.getName(),
							 "IPSP_Editor.saveParticle(" @ IPSP_Editor.currParticle @ ");"
							);
	}

	if( IPSE_Editor.dirty ) {
		MessageBoxYesNoCancel("Save Emitter Changes?",
									 "Do you wish to save the changes made to the <br>current emitter before changing the emitter?",
									 "IPSE_Editor.saveEmitter( " @ IPSE_Editor.currEmitter.getName() @ " ); IPSE_Editor.createEmitter();",
									 "IPSE_Editor.saveEmitterDialogDontSave( " @ IPSE_Editor.currEmitter.getName() @ " ); IPSE_Editor.createEmitter();"
									);
	} else {
		IPSE_Editor.createEmitter();
	}
}

//---------------------------------------------------------------------------------------------

function IPSE_Editor::createEmitter( %this ) {
	// Create a new emitter.
	%emitter = getUniqueName( "newEmitter" );
	datablock ParticleEmitterData( %emitter : DefaultEmitter ) {
	};
	// Submit undo.
	%action = IpsEditor.createUndo( ActionCreateNewEmitter, "Create New Emitter" );
	%action.prevEmitter = IPSE_Editor.currEmitter;
	%action.emitter = %emitter.getId();
	%action.emitterName = %emitter;
	IpsEditor.submitUndo( %action );
	// Execute action.
	%action.redo();
	IPSP_Editor.createParticle(false);
}

//---------------------------------------------------------------------------------------------

function IPSE_Editor::showDeleteDialog( %this ) {
	if( IPSE_Editor.currEmitter.getName() $= "DefaultEmitter" ) {
		MessageBoxOK( "Error", "Cannot delete DefaultEmitter");
		return;
	}

	if( isObject( IPSE_Editor.currEmitter ) ) {
		MessageBoxYesNoCancel("Delete Emitter?",
									 "Are you sure you want to delete<br><br>" @ IPSE_Editor.currEmitter.getName() @ "<br><br> Emitter deletion won't take affect until the level is exited.",
									 "IPSE_Editor.saveEmitterDialogDontSave( " @ IPSE_Editor.currEmitter.getName() @ " ); IPSE_Editor.deleteEmitter();"
									);
	}
}

//---------------------------------------------------------------------------------------------

function IPSE_Editor::deleteEmitter( %this ) {
	%emitter = IPSE_Editor.currEmitter;
	// Create undo.
	%action = IpsEditor.createUndo( ActionDeleteEmitter, "Delete Emitter" );
	%action.emitter = %emitter;
	%action.emitterFname = %emitter.getFilename();
	IpsEditor.submitUndo( %action );
	// Execute action.
	%action.redo();
}

//---------------------------------------------------------------------------------------------

function IPSE_Editor::saveEmitter( %this, %emitter ) {
	if ( %emitter $= "" )
		%newName = IPSE_Selector_Control->TextEdit.getText();
	else
		%newName = %emitter.getName();

   
	IPSE_Editor.currEmitter.setName( %newName );
	%notDirtyData = $IPS_ClassDirty[IPSE_Editor.currEmitter.getClassName()];
	%notDirtyData.assignFieldsFrom( %emitter );
	%notDirtyData.originalName = %newName;
	IPS_EmitterSaver.saveDirty();
	IPSE_Editor.currEmitter = %newName.getId();
	IPSE_Editor.setEmitterNotDirty();
	IpsEditor.createDataList();
}

//---------------------------------------------------------------------------------------------

function IPSE_Editor::saveEmitterDialogDontSave( %this, %emitter) {
   	%notDirtyData = $IPS_ClassDirty[%emitter.getClassName()];
	%emitter.setName( %notDirtyData.originalName );
	%emitter.assignFieldsFrom( %notDirtyData );
	IPSE_Editor.setEmitterNotDirty();
}
