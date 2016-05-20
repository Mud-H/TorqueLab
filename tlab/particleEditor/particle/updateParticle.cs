
// Generic updateParticle method
function PE_ParticleEditor::updateParticle(%this, %propertyField, %value, %isSlider, %onMouseUp) {
	PE_ParticleEditor.setParticleDirty();
	%particle = PE_ParticleEditor.currParticle;
	%last = Editor.getUndoManager().getUndoAction(Editor.getUndoManager().getUndoCount() - 1);

	if( (%isSlider) && (%last.isSlider) && (!%last.onMouseUp) ) {
		%last.field = %propertyField;
		%last.isSlider = %isSlider;
		%last.onMouseUp = %onMouseUp;
		%last.newValue = %value;
	} else {
		%action = ParticleEditor.createUndo(ActionUpdateActiveParticle, "Update Active Particle");
		%action.particle = %particle;
		%action.field = %propertyField;
		%action.isSlider = %isSlider;
		%action.onMouseUp = %onMouseUp;
		%action.newValue = %value;
		%action.oldValue = %particle.getFieldValue( %propertyField );
		ParticleEditor.submitUndo( %action );
	}

	%particle.setFieldValue( %propertyField, %value );
	%particle.reload();
}

//---------------------------------------------------------------------------------------------

// Special case updateEmitter methods
function PE_ParticleEditor::updateParticleTexture( %this, %action ) {
	if( %action ) {
		%texture = MaterialEditorTools.openFile("texture");

		if( %texture !$= "" ) {
			PE_ParticleEditor-->PEP_previewImage.setBitmap(%texture);
			PE_ParticleEditor-->PEP_previewImageName.setText(%texture);
			PE_ParticleEditor-->PEP_previewImageName.tooltip = %texture;
			PE_ParticleEditor.updateParticle( "textureName", %texture );
		}
	} else {
		PE_ParticleEditor-->PEP_previewImage.setBitmap("");
		PE_ParticleEditor-->PEP_previewImageName.setText("");
		PE_ParticleEditor-->PEP_previewImageName.tooltip = "";
		PE_ParticleEditor.updateParticle( "textureName", "" );
	}
}

//---------------------------------------------------------------------------------------------

function PE_ParticleEditor::updateLifeFields( %this, %isRandom, %value, %isSlider, %onMouseUp ) {
	PE_ParticleEditor.setParticleDirty();
	%particle = PE_ParticleEditor.currParticle;

	//Transfer values over to gui controls.

	if( %isRandom ) {
		%value ++;

		if( %value > PE_ParticleEditor-->PEP_lifetimeMS_slider.getValue() ) {
			PE_ParticleEditor-->PEP_lifetimeMS_textEdit.setText( %value );
			PE_ParticleEditor-->PEP_lifetimeMS_slider.setValue( %value );
		}
	} else {
		%value --;

		if( %value < PE_ParticleEditor-->PEP_lifetimeVarianceMS_slider.getValue() ) {
			PE_ParticleEditor-->PEP_lifetimeVarianceMS_textEdit.setText( %value );
			PE_ParticleEditor-->PEP_lifetimeVarianceMS_slider.setValue( %value );
		}
	}

	// Submit undo.
	%last = Editor.getUndoManager().getUndoAction(Editor.getUndoManager().getUndoCount() - 1);

	if( (%isSlider) && (%last.isSlider) && (!%last.onMouseUp) ) {
		%last.isSlider = %isSlider;
		%last.onMouseUp = %onMouseUp;
		%last.newValueLifetimeMS = PE_ParticleEditor-->PEP_lifetimeMS_textEdit.getText();
		%last.newValueLifetimeVarianceMS = PE_ParticleEditor-->PEP_lifetimeVarianceMS_textEdit.getText();
	} else {
		%action = ParticleEditor.createUndo(ActionUpdateActiveParticleLifeFields, "Update Active Particle");
		%action.particle = %particle;
		%action.isSlider = %isSlider;
		%action.onMouseUp = %onMouseUp;
		%action.newValueLifetimeMS = PE_ParticleEditor-->PEP_lifetimeMS_textEdit.getText();
		%action.oldValueLifetimeMS = %particle.lifetimeMS;
		%action.newValueLifetimeVarianceMS = PE_ParticleEditor-->PEP_lifetimeVarianceMS_textEdit.getText();
		%action.oldValueLifetimeVarianceMS = %particle.lifetimeVarianceMS;
		ParticleEditor.submitUndo( %action );
	}

	%particle.lifetimeMS = PE_ParticleEditor-->PEP_lifetimeMS_textEdit.getText();
	%particle.lifetimeVarianceMS = PE_ParticleEditor-->PEP_lifetimeVarianceMS_textEdit.getText();
	%particle.reload();
}

//---------------------------------------------------------------------------------------------

function PE_ParticleEditor::updateSpinFields( %this, %isMax, %value, %isSlider, %onMouseUp ) {
	PE_ParticleEditor.setParticleDirty();
	%particle = PE_ParticleEditor.currParticle;

	// Transfer values over to gui controls.
	if( %isMax ) {
		%value ++;

		if( %value > PE_ParticleEditor-->PEP_spinRandomMax_slider.getValue() ) {
			PE_ParticleEditor-->PEP_spinRandomMax_textEdit.setText( %value );
			PE_ParticleEditor-->PEP_spinRandomMax_slider.setValue( %value );
		}
	} else {
		%value --;

		if( %value < PE_ParticleEditor-->PEP_spinRandomMin_slider.getValue() ) {
			PE_ParticleEditor-->PEP_spinRandomMin_textEdit.setText( %value );
			PE_ParticleEditor-->PEP_spinRandomMin_slider.setValue( %value );
		}
	}

	// Submit undo.
	%last = Editor.getUndoManager().getUndoAction(Editor.getUndoManager().getUndoCount() - 1);

	if( (%isSlider) && (%last.isSlider) && (!%last.onMouseUp) ) {
		%last.isSlider = %isSlider;
		%last.onMouseUp = %onMouseUp;
		%last.newValueSpinRandomMax = PE_ParticleEditor-->PEP_spinRandomMax_textEdit.getText();
		%last.newValueSpinRandomMin = PE_ParticleEditor-->PEP_spinRandomMin_textEdit.getText();
	} else {
		%action = ParticleEditor.createUndo(ActionUpdateActiveParticleSpinFields, "Update Active Particle");
		%action.particle = %particle;
		%action.isSlider = %isSlider;
		%action.onMouseUp = %onMouseUp;
		%action.newValueSpinRandomMax = PE_ParticleEditor-->PEP_spinRandomMax_textEdit.getText();
		%action.oldValueSpinRandomMax = %particle.spinRandomMax;
		%action.newValueSpinRandomMin = PE_ParticleEditor-->PEP_spinRandomMin_textEdit.getText();
		%action.oldValueSpinRandomMin = %particle.spinRandomMin;
		ParticleEditor.submitUndo( %action );
	}

	%particle.spinRandomMax = PE_ParticleEditor-->PEP_spinRandomMax_textEdit.getText();
	%particle.spinRandomMin = PE_ParticleEditor-->PEP_spinRandomMin_textEdit.getText();
	%particle.reload();
}

//---------------------------------------------------------------------------------------------