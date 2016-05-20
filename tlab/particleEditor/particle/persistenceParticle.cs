

function PE_ParticleEditor::doParticleSave( %this ) {
PE_ParticleEditor.saveParticle( PE_ParticleEditor.currParticle );
}
//---------------------------------------------------------------------------------------------

function PE_ParticleEditor::onNewParticle( %this ) {
	// Bail if the user selected the same particle.
	%id = PEP_ParticleSelector.getSelected();

	if( %id == PE_ParticleEditor.currParticle )
		return;

	// Load new particle if we're not in a dirty state
	if( PE_ParticleEditor.dirty ) {
		LabMsgYesNoCancel("Save Existing Particle?",
								"Do you want to save changes to <br><br>" @ PE_ParticleEditor.currParticle.getName(),
								"PE_ParticleEditor.saveParticle(" @ PE_ParticleEditor.currParticle @ ");",
								"PE_ParticleEditor.saveParticleDialogDontSave(" @ PE_ParticleEditor.currParticle @ "); PE_ParticleEditor.loadNewParticle();"
							  );
	} else {
		PE_ParticleEditor.loadNewParticle();
	}
}

//---------------------------------------------------------------------------------------------

function PE_ParticleEditor::loadNewParticle( %this, %particle ) {
	if( isObject( %particle ) )
		%particle = %particle.getId();
	else
		%particle = PEP_ParticleSelector.getSelected();

	PE_ParticleEditor.currParticle = %particle;
	%particle.reload();
	PE_ParticleEditor_NotDirtyParticle.assignFieldsFrom( %particle );
	PE_ParticleEditor_NotDirtyParticle.originalName = %particle.getName();
	PE_ParticleEditor.guiSync();
	PE_ParticleEditor.setParticleNotDirty();
}

//---------------------------------------------------------------------------------------------

function PE_ParticleEditor::setParticleDirty( %this ) {
	PE_ParticleEditor.text = "Particle *";
	PE_ParticleEditor.dirty = true;
	%particle = PE_ParticleEditor.currParticle;

	if( %particle.getFilename() $= "" || %particle.getFilename() $= "tlab/particleEditor/particleParticleEditor.ed.cs" )
		PE_ParticleSaver.setDirty( %particle, $PE_PARTICLEEDITOR_DEFAULT_FILENAME );
	else
		PE_ParticleSaver.setDirty( %particle );
}

//---------------------------------------------------------------------------------------------

function PE_ParticleEditor::setParticleNotDirty( %this ) {
	PE_ParticleEditor.text = "Particle";
	PE_ParticleEditor.dirty = false;
	PE_ParticleSaver.clearAll();
}

//---------------------------------------------------------------------------------------------

function PE_ParticleEditor::showNewDialog( %this, %replaceSlot ) {
	// Open a dialog if the current Particle is dirty
	if( PE_ParticleEditor.dirty ) {
		LabMsgYesNoCancel("Save Particle Changes?",
								"Do you wish to save the changes made to the <br>current particle before changing the particle?",
								"PE_ParticleEditor.saveParticle( " @ PE_ParticleEditor.currParticle.getName() @ " ); PE_ParticleEditor.createParticle( " @ %replaceSlot @ " );",
								"PE_ParticleEditor.saveParticleDialogDontSave( " @ PE_ParticleEditor.currParticle.getName() @ " ); PE_ParticleEditor.createParticle( " @ %replaceSlot @ " );"
							  );
	} else {
		PE_ParticleEditor.createParticle( %replaceSlot );
	}
}

//---------------------------------------------------------------------------------------------

function PE_ParticleEditor::createParticle( %this, %replaceSlot ) {
	// Make sure we have a spare slot on the current emitter.
	if( !%replaceSlot ) {
		%numExistingParticles = getWordCount( PE_EmitterEditor.currEmitter.particles );

		if( %numExistingParticles > 3 ) {
			LabMsgOK( "Error", "An emitter cannot have more than 4 particles assigned to it." );
			return;
		}

		%particleIndex = %numExistingParticles;
	} else
		%particleIndex = %replaceSlot - 1;

	// Create the particle datablock and add to the emitter.
	%newParticle = getUniqueName( "newParticle" );
	datablock ParticleData( %newParticle : DefaultParticle ) {
	};
	// Submit undo.
	%action = ParticleEditor.createUndo( ActionCreateNewParticle, "Create New Particle" );
	%action.particle = %newParticle.getId();
	%action.particleIndex = %particleIndex;
	%action.prevParticle = ( "PEE_EmitterParticleSelector" @ ( %particleIndex + 1 ) ).getSelected();
	%action.emitter = PE_EmitterEditor.currEmitter;
	ParticleEditor.submitUndo( %action );
	// Execute action.
	%action.redo();
}

//---------------------------------------------------------------------------------------------

function PE_ParticleEditor::showDeleteDialog( %this ) {
	// Don't allow deleting DefaultParticle.
	if( PE_ParticleEditor.currParticle.getName() $= "DefaultParticle" ) {
		LabMsgOK( "Error", "Cannot delete DefaultParticle");
		return;
	}

	// Check to see if the particle emitter has more than 1 particle on it.

	if( getWordCount( PE_EmitterEditor.currEmitter.particles ) == 1 ) {
		LabMsgOK( "Error", "At least one particle must remain on the particle emitter.");
		return;
	}

	// Bring up requester for confirmation.

	if( isObject( PE_ParticleEditor.currParticle ) ) {
		LabMsgYesNoCancel( "Delete Particle?",
								 "Are you sure you want to delete<br><br>" @ PE_ParticleEditor.currParticle.getName() @ "<br><br> Particle deletion won't take affect until the engine is quit.",
								 "PE_ParticleEditor.saveParticleDialogDontSave( " @ PE_ParticleEditor.currParticle.getName() @ " ); PE_ParticleEditor.deleteParticle();",
								 "",
								 ""
							  );
	}
}

//---------------------------------------------------------------------------------------------

function PE_ParticleEditor::deleteParticle( %this ) {
	%particle = PE_ParticleEditor.currParticle;
	// Submit undo.
	%action = ParticleEditor.createUndo( ActionDeleteParticle, "Delete Particle" );
	%action.particle = %particle;
	%action.emitter = PE_EmitterEditor.currEmitter;
	ParticleEditor.submitUndo( %action );
	// Execute action.
	%action.redo();
}

//---------------------------------------------------------------------------------------------

function PE_ParticleEditor::saveParticle( %this, %particle ) {
	%particle.setName( PEP_ParticleSelector.getText() );
	PE_ParticleEditor_NotDirtyParticle.assignFieldsFrom( %particle );
	PE_ParticleEditor_NotDirtyParticle.originalName = %particle.getName();
	PE_ParticleSaver.saveDirty();
	PE_ParticleEditor.setParticleNotDirty();
	ParticleEditor.createParticleList();
}

//---------------------------------------------------------------------------------------------

function PE_ParticleEditor::saveParticleDialogDontSave( %this, %particle ) {
	%particle.setName( PE_ParticleEditor_NotDirtyParticle.originalName );
	%particle.assignFieldsFrom( PE_ParticleEditor_NotDirtyParticle );
	PE_ParticleEditor.setParticleNotDirty();
}