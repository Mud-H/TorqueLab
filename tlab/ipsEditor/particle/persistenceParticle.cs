

function IPSP_Editor::doParticleSave( %this ) {
IPSP_Editor.saveParticle( IPSP_Editor.currParticle );
}
//---------------------------------------------------------------------------------------------

function IPSP_Editor::onNewParticle( %this ) {
	// Bail if the user selected the same particle.
	%id = IPSP_Selector.getSelected();

	if( %id == IPSP_Editor.currParticle )
		return;

	// Load new particle if we're not in a dirty state
	if( IPSP_Editor.dirty ) {
		LabMsgYesNoCancel("Save Existing Particle?",
								"Do you want to save changes to <br><br>" @ IPSP_Editor.currParticle.getName(),
								"IPSP_Editor.saveParticle(" @ IPSP_Editor.currParticle @ ");",
								"IPSP_Editor.saveParticleDialogDontSave(" @ IPSP_Editor.currParticle @ "); IPSP_Editor.loadNewParticle();"
							  );
	} else {
		IPSP_Editor.loadNewParticle();
	}
}

//---------------------------------------------------------------------------------------------

function IPSP_Editor::loadNewParticle( %this, %particle ) {
	if( isObject( %particle ) )
		%particle = %particle.getId();
	else
		%particle = IPSP_Selector.getSelected();

	IPSP_Editor.currParticle = %particle;
	%particle.reload();
	IPSP_Editor_NotDirtyParticle.assignFieldsFrom( %particle );
	IPSP_Editor_NotDirtyParticle.originalName = %particle.getName();
	IPSP_Editor.guiSync();
	IPSP_Editor.setParticleNotDirty();
}

//---------------------------------------------------------------------------------------------

function IPSP_Editor::setParticleDirty( %this ) {
	IPSP_Editor.text = "Particle *";
	IPSP_Editor.dirty = true;
	%particle = IPSP_Editor.currParticle;

	if( %particle.getFilename() $= "" || %particle.getFilename() $= "tlab/IpsEditor/particleIpsEditor.ed.cs" )
		IPS_ParticleSaver.setDirty( %particle, $IPSP_Editor_DEFAULT_FILENAME );
	else
		IPS_ParticleSaver.setDirty( %particle );
}

//---------------------------------------------------------------------------------------------

function IPSP_Editor::setParticleNotDirty( %this ) {
	IPSP_Editor.text = "Particle";
	IPSP_Editor.dirty = false;
	IPS_ParticleSaver.clearAll();
}

//---------------------------------------------------------------------------------------------

function IPSP_Editor::showNewDialog( %this, %replaceSlot ) {
	// Open a dialog if the current Particle is dirty
	if( IPSP_Editor.dirty ) {
		LabMsgYesNoCancel("Save Particle Changes?",
								"Do you wish to save the changes made to the <br>current particle before changing the particle?",
								"IPSP_Editor.saveParticle( " @ IPSP_Editor.currParticle.getName() @ " ); IPSP_Editor.createParticle( " @ %replaceSlot @ " );",
								"IPSP_Editor.saveParticleDialogDontSave( " @ IPSP_Editor.currParticle.getName() @ " ); IPSP_Editor.createParticle( " @ %replaceSlot @ " );"
							  );
	} else {
		IPSP_Editor.createParticle( %replaceSlot );
	}
}

//---------------------------------------------------------------------------------------------

function IPSP_Editor::createParticle( %this, %replaceSlot ) {
	// Make sure we have a spare slot on the current emitter.
	if( !%replaceSlot ) {
		%numExistingParticles = getWordCount( IPSE_Editor.currEmitter.particles );

		if( %numExistingParticles > 3 ) {
			LabMsgOK( "Error", "An emitter cannot have more than 4 particles assigned to it." );
			return;
		}

		%particleIndex = %numExistingParticles;
	} else
		%particleIndex = %replaceSlot - 1;

	// Create the particle datablock and add to the emitter.
	%newParticle = getUniqueName( "newParticle" );
	datablock BillboardParticleData( %newParticle : DefaultParticle ) {
	};
	// Submit undo.
	%action = IpsEditor.createUndo( ActionCreateNewParticle, "Create New Particle" );
	%action.particle = %newParticle.getId();
	%action.particleIndex = %particleIndex;
	%action.prevParticle = ( "IPSE_EmitterParticleSelector" @ ( %particleIndex + 1 ) ).getSelected();
	%action.emitter = IPSE_Editor.currEmitter;
	IpsEditor.submitUndo( %action );
	// Execute action.
	%action.redo();
}

//---------------------------------------------------------------------------------------------

function IPSP_Editor::showDeleteDialog( %this ) {
	// Don't allow deleting DefaultParticle.
	if( IPSP_Editor.currParticle.getName() $= "DefaultParticle" ) {
		LabMsgOK( "Error", "Cannot delete DefaultParticle");
		return;
	}

	// Check to see if the particle emitter has more than 1 particle on it.

	if( getWordCount( IPSE_Editor.currEmitter.particles ) == 1 ) {
		LabMsgOK( "Error", "At least one particle must remain on the particle emitter.");
		return;
	}

	// Bring up requester for confirmation.

	if( isObject( IPSP_Editor.currParticle ) ) {
		LabMsgYesNoCancel( "Delete Particle?",
								 "Are you sure you want to delete<br><br>" @ IPSP_Editor.currParticle.getName() @ "<br><br> Particle deletion won't take affect until the engine is quit.",
								 "IPSP_Editor.saveParticleDialogDontSave( " @ IPSP_Editor.currParticle.getName() @ " ); IPSP_Editor.deleteParticle();",
								 "",
								 ""
							  );
	}
}

//---------------------------------------------------------------------------------------------

function IPSP_Editor::deleteParticle( %this ) {
	%particle = IPSP_Editor.currParticle;
	// Submit undo.
	%action = IpsEditor.createUndo( ActionDeleteParticle, "Delete Particle" );
	%action.particle = %particle;
	%action.emitter = IPSE_Editor.currEmitter;
	IpsEditor.submitUndo( %action );
	// Execute action.
	%action.redo();
}

//---------------------------------------------------------------------------------------------

function IPSP_Editor::saveParticle( %this, %particle ) {
	%particle.setName( IPSP_Selector.getText() );
	IPSP_Editor_NotDirtyParticle.assignFieldsFrom( %particle );
	IPSP_Editor_NotDirtyParticle.originalName = %particle.getName();
	IPS_ParticleSaver.saveDirty();
	IPSP_Editor.setParticleNotDirty();
	IpsEditor.createParticleList();
}

//---------------------------------------------------------------------------------------------

function IPSP_Editor::saveParticleDialogDontSave( %this, %particle ) {
	%particle.setName( IPSP_Editor_NotDirtyParticle.originalName );
	%particle.assignFieldsFrom( IPSP_Editor_NotDirtyParticle );
	IPSP_Editor.setParticleNotDirty();
}
