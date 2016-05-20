
function PE_ParticleEditor::doEmitterSave( %this ) {
	PE_EmitterEditor.saveEmitter(); 
	//PE_ParticleEditor.saveParticle( PE_ParticleEditor.currParticle );
}
//---------------------------------------------------------------------------------------------

function PE_EmitterEditor::onNewEmitter( %this ) {
	if(    isObject( PE_EmitterEditor.currEmitter )
			 && PE_EmitterEditor.currEmitter $= PEE_EmitterSelector.getSelected() )
		return;

	//FIXME: disregards particle tab dirty state

	if( PE_EmitterEditor.dirty ) {
		if( PE_ParticleEditor.dirty ) {
			MessageBoxYesNo("Save Existing Particle?",
								 "Do you want to save changes to <br><br>" @ PE_ParticleEditor.currParticle.getName(),
								 "PE_ParticleEditor.saveParticle(" @ PE_ParticleEditor.currParticle @ ");"
								);
		}

		%savedEmitter = PE_EmitterEditor.currEmitter;
		MessageBoxYesNoCancel("Save Existing Emitter?",
									 "Do you want to save changes to <br><br>" @ %savedEmitter.getName(),
									 "PE_EmitterEditor.saveEmitter(" @ %savedEmitter@ "); PE_EmitterEditor.loadNewEmitter();",
									 "PE_EmitterEditor.saveEmitterDialogDontSave(" @ %savedEmitter @ "); PE_EmitterEditor.loadNewEmitter();"
									);
	} else {
		PE_EmitterEditor.loadNewEmitter();
	}
}

//---------------------------------------------------------------------------------------------

function PE_EmitterEditor::loadNewEmitter( %this, %emitter ) {
	if( isObject( %emitter ) )
		%current = %emitter.getId();
	else
		%current = PEE_EmitterSelector.getSelected();
	
	$PE_AutoLoop = false;
	PE_EmitterEditor-->PEE_infiniteLoop.setStateOn( %current.lifetimeMS == 0 );
	PE_EmitterEditor.currEmitter = %current;
	PE_EmitterEditor_NotDirtyEmitter.assignFieldsFrom( %current );
	PE_EmitterEditor_NotDirtyEmitter.originalName = %current.name;
	PE_EmitterEditor.guiSync();
	PE_EmitterEditor.setEmitterNotDirty();
	PE_ParticleEditor.loadNewParticle( getWord( %current.particles, 0 ) );
	ParticleEditor.updateEmitterNode();
	
}

//---------------------------------------------------------------------------------------------

function PE_EmitterEditor::setEmitterDirty( %this ) {
	PE_EmitterEditor.text = "Emitter *";
	PE_EmitterEditor.dirty = true;
	%emitter = PE_EmitterEditor.currEmitter;

	if( %emitter.getFilename() $= "" || %emitter.getFilename() $= "tlab/particleEditor/particleEmitterEditor.ed.cs" )
		PE_EmitterSaver.setDirty( %emitter, $PE_EMITTEREDITOR_DEFAULT_FILENAME );
	else
		PE_EmitterSaver.setDirty( %emitter );
}

//---------------------------------------------------------------------------------------------

function PE_EmitterEditor::setEmitterNotDirty( %this ) {
	PE_EmitterEditor.text = "Emitter";
	PE_EmitterEditor.dirty = false;
	PE_EmitterSaver.clearAll();
}

//---------------------------------------------------------------------------------------------

// Create Functionality
function PE_EmitterEditor::showNewDialog( %this ) {
	//FIXME: disregards particle tab dirty state

	// Open a dialog if the current emitter is dirty.
	if( PE_ParticleEditor.dirty ) {
		MessageBoxYesNo("Save Existing Particle?",
							 "Do you want to save changes to <br><br>" @ PE_ParticleEditor.currParticle.getName(),
							 "PE_ParticleEditor.saveParticle(" @ PE_ParticleEditor.currParticle @ ");"
							);
	}

	if( PE_EmitterEditor.dirty ) {
		MessageBoxYesNoCancel("Save Emitter Changes?",
									 "Do you wish to save the changes made to the <br>current emitter before changing the emitter?",
									 "PE_EmitterEditor.saveEmitter( " @ PE_EmitterEditor.currEmitter.getName() @ " ); PE_EmitterEditor.createEmitter();",
									 "PE_EmitterEditor.saveEmitterDialogDontSave( " @ PE_EmitterEditor.currEmitter.getName() @ " ); PE_EmitterEditor.createEmitter();"
									);
	} else {
		PE_EmitterEditor.createEmitter();
	}
}

//---------------------------------------------------------------------------------------------

function PE_EmitterEditor::createEmitter( %this ) {
	// Create a new emitter.
	%emitter = getUniqueName( "newEmitter" );
	datablock ParticleEmitterData( %emitter : DefaultEmitter ) {
	};
	// Submit undo.
	%action = ParticleEditor.createUndo( ActionCreateNewEmitter, "Create New Emitter" );
	%action.prevEmitter = PE_EmitterEditor.currEmitter;
	%action.emitter = %emitter.getId();
	%action.emitterName = %emitter;
	ParticleEditor.submitUndo( %action );
	// Execute action.
	%action.redo();
	PE_ParticleEditor.createParticle(false);
}

//---------------------------------------------------------------------------------------------

function PE_EmitterEditor::showDeleteDialog( %this ) {
	if( PE_EmitterEditor.currEmitter.getName() $= "DefaultEmitter" ) {
		MessageBoxOK( "Error", "Cannot delete DefaultEmitter");
		return;
	}

	if( isObject( PE_EmitterEditor.currEmitter ) ) {
		MessageBoxYesNoCancel("Delete Emitter?",
									 "Are you sure you want to delete<br><br>" @ PE_EmitterEditor.currEmitter.getName() @ "<br><br> Emitter deletion won't take affect until the level is exited.",
									 "PE_EmitterEditor.saveEmitterDialogDontSave( " @ PE_EmitterEditor.currEmitter.getName() @ " ); PE_EmitterEditor.deleteEmitter();"
									);
	}
}

//---------------------------------------------------------------------------------------------

function PE_EmitterEditor::deleteEmitter( %this ) {
	%emitter = PE_EmitterEditor.currEmitter;
	// Create undo.
	%action = ParticleEditor.createUndo( ActionDeleteEmitter, "Delete Emitter" );
	%action.emitter = %emitter;
	%action.emitterFname = %emitter.getFilename();
	ParticleEditor.submitUndo( %action );
	// Execute action.
	%action.redo();
}

//---------------------------------------------------------------------------------------------

function PE_EmitterEditor::saveEmitter( %this, %emitter ) {
	if ( %emitter $= "" )
		%newName = PEE_EmitterSelector_Control->TextEdit.getText();
	else
		%newName = %emitter.getName();

	PE_EmitterEditor.currEmitter.setName( %newName );
	PE_EmitterEditor_NotDirtyEmitter.assignFieldsFrom( %emitter );
	PE_EmitterEditor_NotDirtyEmitter.originalName = %newName;
	PE_EmitterSaver.saveDirty();
	PE_EmitterEditor.currEmitter = %newName.getId();
	PE_EmitterEditor.setEmitterNotDirty();
	ParticleEditor.createParticleList();
}

//---------------------------------------------------------------------------------------------

function PE_EmitterEditor::saveEmitterDialogDontSave( %this, %emitter) {
	%emitter.setName( PE_EmitterEditor_NotDirtyEmitter.originalName );
	%emitter.assignFieldsFrom( PE_EmitterEditor_NotDirtyEmitter );
	PE_EmitterEditor.setEmitterNotDirty();
}