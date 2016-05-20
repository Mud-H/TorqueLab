

// Generic updateEmitter method
function PE_EmitterEditor::updateEmitter( %this, %propertyField, %value, %isSlider, %onMouseUp ) {
	PE_EmitterEditor.setEmitterDirty();
	%emitter = PE_EmitterEditor.currEmitter;
	%last = Editor.getUndoManager().getUndoAction(Editor.getUndoManager().getUndoCount() - 1);

	if( (%isSlider) && (%last.isSlider) && (!%last.onMouseUp) ) {
		%last.field = %propertyField;
		%last.isSlider = %isSlider;
		%last.onMouseUp = %onMouseUp;
		%last.newValue = %value;
	} else {
		%action = ParticleEditor.createUndo(ActionUpdateActiveEmitter, "Update Active Emitter");
		%action.emitter = %emitter;
		%action.field = %propertyField;
		%action.isSlider = %isSlider;
		%action.onMouseUp = %onMouseUp;
		%action.newValue = %value;
		%action.oldValue = %emitter.getFieldValue( %propertyField );
		ParticleEditor.submitUndo( %action );
	}

	%emitter.setFieldValue( %propertyField, %value );
	%emitter.reload();
}

//---------------------------------------------------------------------------------------------

// Special case updateEmitter methods
function PE_EmitterEditor::updateLifeFields( %this, %isRandom, %value, %isSlider, %onMouseUp ) {
	PE_EmitterEditor.setEmitterDirty();
	%emitter = PE_EmitterEditor.currEmitter;

	// Transfer values over to gui controls.

	if( %isRandom ) {
		if( %value > 0 )
			%value++;

		if( %value > PE_EmitterEditor-->PEE_lifetimeMS_slider.getValue() ) {
			PE_EmitterEditor-->PEE_lifetimeMS_textEdit.setText( %value );
			PE_EmitterEditor-->PEE_lifetimeMS_slider.setValue( %value );
		}
	} else {
		if( %value > 0 )
			%value --;

		if( %value < PE_EmitterEditor-->PEE_lifetimeVarianceMS_slider.getValue() ) {
			PE_EmitterEditor-->PEE_lifetimeVarianceMS_textEdit.setText( %value );
			PE_EmitterEditor-->PEE_lifetimeVarianceMS_slider.setValue( %value );
		}
	}

	// Submit undo.
	%last = Editor.getUndoManager().getUndoAction(Editor.getUndoManager().getUndoCount() - 1);

	if( (%isSlider) && (%last.isSlider) && (!%last.onMouseUp) ) {
		%last.isSlider = %isSlider;
		%last.onMouseUp = %onMouseUp;
		%last.newValueLifetimeMS = PE_EmitterEditor-->PEE_lifetimeMS_textEdit.getText();
		%last.newValueLifetimeVarianceMS = PE_EmitterEditor-->PEE_lifetimeVarianceMS_textEdit.getText();
	} else {
		%action = ParticleEditor.createUndo(ActionUpdateActiveEmitterLifeFields, "Update Active Emitter");
		%action.emitter = %emitter;
		%action.isSlider = %isSlider;
		%action.onMouseUp = %onMouseUp;
		%action.newValueLifetimeMS = PE_EmitterEditor-->PEE_lifetimeMS_textEdit.getText();
		%action.oldValueLifetimeMS = %emitter.lifetimeMS;
		%action.newValueLifetimeVarianceMS = PE_EmitterEditor-->PEE_lifetimeVarianceMS_textEdit.getText();
		%action.oldValueLifetimeVarianceMS = %emitter.lifetimeVarianceMS;
		ParticleEditor.submitUndo( %action );
	}

	// Set the values on the current emitter.
	%emitter.lifetimeMS = PE_EmitterEditor-->PEE_lifetimeMS_textEdit.getText();
	%emitter.lifetimeVarianceMS = PE_EmitterEditor-->PEE_lifetimeVarianceMS_textEdit.getText();
	%emitter.reload();
	// Keep the infiniteLoop checkbox up to date.
	PE_EmitterEditor-->PEE_infiniteLoop.setStateOn(
		%emitter.lifetimeMS == 0
	);
}

//---------------------------------------------------------------------------------------------

function PE_EmitterEditor::updateLifeFieldsInfiniteLoop( %this ) {
	%emitter = PE_EmitterEditor.currEmitter;
	%isEnabled = PE_EmitterEditor-->PEE_infiniteLoop.isStateOn();
	// Submit undo.
	%action = ParticleEditor.createUndo( ActionUpdateActiveEmitterLifeFields, "Update Active Emitter" );
	%action.emitter = %emitter;

	if( %isEnabled ) {
		%action.newValueLifetimeMS = 0;
		%action.newvalueLifetimeVarianceMS = 0;
		%action.oldValueLifetimeMS = PE_EmitterEditor-->PEE_lifetimeMS_textEdit.getText();
		%action.oldValueLifetimeVarianceMS = PE_EmitterEditor-->PEE_lifetimeVarianceMS_textEdit.getText();
		//%this.defaultLifeTimeMS[%emitter.getId()] = %emitter.lifetimeMS;
		//%emitter.lifetimeMS = 0;
	} else {
		
		%action.newValueLifetimeMS = PE_EmitterEditor-->PEE_lifetimeMS_textEdit.getText();
		%action.newvalueLifetimeVarianceMS = PE_EmitterEditor-->PEE_lifetimeVarianceMS_textEdit.getText();
		%action.oldValueLifetimeMS = 0;
		%action.oldValueLifetimeVarianceMS = 0;		
		//%emitter.lifetimeMS = %this.defaultLifeTimeMS[%emitter.getId()];
		//PE_EmitterEditor-->PEE_lifetimeMS_textEdit.setText(%emitter.lifetimeMS);
		
	}

	ParticleEditor.submitUndo( %action );
	// Execute action.
	%action.redo();
}

//---------------------------------------------------------------------------------------------

function PE_EmitterEditor::updateAmountFields( %this, %isRandom, %value, %isSlider, %onMouseUp ) {
	PE_EmitterEditor.setEmitterDirty();
	%emitter = PE_EmitterEditor.currEmitter;

	// Transfer values over to gui controls.

	if( %isRandom ) {
		%value ++;

		if( %value > PE_EmitterEditor-->PEE_ejectionPeriodMS_slider.getValue() ) {
			PE_EmitterEditor-->PEE_ejectionPeriodMS_textEdit.setText( %value );
			PE_EmitterEditor-->PEE_ejectionPeriodMS_slider.setValue( %value );
		}
	} else {
		%value --;

		if( %value < PE_EmitterEditor-->PEE_periodVarianceMS_slider.getValue() ) {
			PE_EmitterEditor-->PEE_periodVarianceMS_textEdit.setText( %value );
			PE_EmitterEditor-->PEE_periodVarianceMS_slider.setValue( %value );
		}
	}

	// Submit undo.
	%last = Editor.getUndoManager().getUndoAction(Editor.getUndoManager().getUndoCount() - 1);

	if( (%isSlider) && (%last.isSlider) && (!%last.onMouseUp) ) {
		%last.isSlider = %isSlider;
		%last.onMouseUp = %onMouseUp;
		%last.newValueEjectionPeriodMS = PE_EmitterEditor-->PEE_ejectionPeriodMS_textEdit.getText();
		%last.newValuePeriodVarianceMS = PE_EmitterEditor-->PEE_periodVarianceMS_textEdit.getText();
	} else {
		%action = ParticleEditor.createUndo(ActionUpdateActiveEmitterAmountFields, "Update Active Emitter");
		%action.emitter = %emitter;
		%action.isSlider = %isSlider;
		%action.onMouseUp = %onMouseUp;
		%action.newValueEjectionPeriodMS = PE_EmitterEditor-->PEE_ejectionPeriodMS_textEdit.getText();
		%action.oldValueEjectionPeriodMS = %emitter.ejectionPeriodMS;
		%action.newValuePeriodVarianceMS = PE_EmitterEditor-->PEE_periodVarianceMS_textEdit.getText();
		%action.oldValuePeriodVarianceMS = %emitter.periodVarianceMS;
		ParticleEditor.submitUndo( %action );
	}

	// Set the values on the current emitter.
	%emitter.ejectionPeriodMS = PE_EmitterEditor-->PEE_ejectionPeriodMS_textEdit.getText();
	%emitter.periodVarianceMS = PE_EmitterEditor-->PEE_periodVarianceMS_textEdit.getText();
	%emitter.reload();
}

//---------------------------------------------------------------------------------------------

function PE_EmitterEditor::updateSpeedFields( %this, %isRandom, %value, %isSlider, %onMouseUp ) {
	PE_EmitterEditor.setEmitterDirty();
	%emitter = PE_EmitterEditor.currEmitter;

	// Transfer values over to gui controls.

	if( %isRandom ) {
		if( %value > PE_EmitterEditor-->PEE_ejectionVelocity_slider.getValue() ) {
			PE_EmitterEditor-->PEE_ejectionVelocity_textEdit.setText( %value );
			PE_EmitterEditor-->PEE_ejectionVelocity_slider.setValue( %value );
		}
	} else {
		if( %value < PE_EmitterEditor-->PEE_velocityVariance_slider.getValue() ) {
			PE_EmitterEditor-->PEE_velocityVariance_textEdit.setText( %value );
			PE_EmitterEditor-->PEE_velocityVariance_slider.setValue( %value );
		}
	}

	// Submit undo.
	%last = Editor.getUndoManager().getUndoAction(Editor.getUndoManager().getUndoCount() - 1);

	if( (%isSlider) && (%last.isSlider) && (!%last.onMouseUp) ) {
		%last.isSlider = %isSlider;
		%last.onMouseUp = %onMouseUp;
		%last.newValueEjectionVelocity = PE_EmitterEditor-->PEE_ejectionVelocity_textEdit.getText();
		%last.newValueVelocityVariance = PE_EmitterEditor-->PEE_velocityVariance_textEdit.getText();
	} else {
		%action = ParticleEditor.createUndo(ActionUpdateActiveEmitterSpeedFields, "Update Active Emitter");
		%action.emitter = %emitter;
		%action.isSlider = %isSlider;
		%action.onMouseUp = %onMouseUp;
		%action.newValueEjectionVelocity = PE_EmitterEditor-->PEE_ejectionVelocity_textEdit.getText();
		%action.oldValueEjectionVelocity = %emitter.ejectionVelocity;
		%action.newValueVelocityVariance = PE_EmitterEditor-->PEE_velocityVariance_textEdit.getText();
		%action.oldValueVelocityVariance = %emitter.velocityVariance;
		ParticleEditor.submitUndo( %action );
	}

	// Set the values on the current emitter.
	%emitter.ejectionVelocity = PE_EmitterEditor-->PEE_ejectionVelocity_textEdit.getText();
	%emitter.velocityVariance = PE_EmitterEditor-->PEE_velocityVariance_textEdit.getText();
	%emitter.reload();
}

//---------------------------------------------------------------------------------------------

function PE_EmitterEditor::updateParticlesFields( %this ) {
	%particles = "";

	for( %i = 1; %i < 5; %i ++ ) {
		%emitterParticle = "PEE_EmitterParticle" @ %i;
		%popup = %emitterParticle-->PopUpMenu;
		%text = %popup.getText();

		if( %text $= "" || %text $= "None" )
			continue;

		if( %particles $= "" )
			%particles = %text;
		else
			%particles = %particles SPC %text;
	}

	%changedEditParticle = 1;
	%currParticle = PE_ParticleEditor.currParticle.getName();

	foreach$( %particleName in %particles ) {
		if( %particleName $= %currParticle ) {
			%changedEditParticle = 0;
			break;
		}
	}

	// True only if the currently edited particle has not been found and the
	// ParticleEditor is dirty.

	if( %changedEditParticle && PE_ParticleEditor.dirty ) {
		MessageBoxYesNoCancel("Save Particle Changes?",
									 "Do you wish to save the changes made to the <br>current particle before changing the particle?",
									 "PE_ParticleEditor.saveParticle( " @ PE_ParticleEditor.currParticle.getName() @ " ); PE_EmitterEditor.updateEmitter( \"particles\"," @ %particles @ ");",
									 "PE_ParticleEditor.saveParticleDialogDontSave( " @ PE_ParticleEditor.currParticle.getName() @ " ); PE_EmitterEditor.updateEmitter( \"particles\"," @ %particles @ ");",
									 "PE_EmitterEditor.guiSync();" );
	} else {
		PE_EmitterEditor.updateEmitter( "particles", %particles );
	}
}