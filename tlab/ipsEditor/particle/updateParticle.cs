function IPSP_Editor::updateParticleCtrl(%this,%ctrl,%onMouseUp) {
   %intWords = strreplace(%ctrl.internalName,"_"," ");
   %propertyField = getWord(%intWords,0);
   %isSlider = false;
   if (%ctrl.isMemberOfClass("GuiSliderCtrl")){
      %isSlider = true;
      %value = %ctrl.getValue();
   } else  if (%ctrl.isMemberOfClass("GuiTextEditCtrl")){
      %isSlider = false;
      %value = %ctrl.getText();
   }
	devLog("updateParticle %propertyField",%propertyField,"%value",%value);
	%this.updateParticle(%propertyField,%value,%isSlider,%onMouseUp);
	
}
// Generic updateParticle method
function IPSP_Editor::updateParticle(%this, %propertyField, %value, %isSlider, %onMouseUp) {
	IPSP_Editor.setParticleDirty();
	%particle = IPSP_Editor.currParticle;
	%last = Editor.getUndoManager().getUndoAction(Editor.getUndoManager().getUndoCount() - 1);

	if( (%isSlider) && (%last.isSlider) && (!%last.onMouseUp) ) {
		%last.field = %propertyField;
		%last.isSlider = %isSlider;
		%last.onMouseUp = %onMouseUp;
		%last.newValue = %value;
	} else {
		%action = IpsEditor.createUndo(ActionUpdateActiveParticle, "Update Active Particle");
		%action.particle = %particle;
		%action.field = %propertyField;
		%action.isSlider = %isSlider;
		%action.onMouseUp = %onMouseUp;
		%action.newValue = %value;
		%action.oldValue = %particle.getFieldValue( %propertyField );
		IpsEditor.submitUndo( %action );
	}

	%particle.setFieldValue( %propertyField, %value );
	%particle.reload();
}

//---------------------------------------------------------------------------------------------

// Special case updateEmitter methods
function IPSP_Editor::updateParticleTexture( %this, %action ) {
	if( %action ) {
		%texture = MaterialEditorTools.openFile("texture");

		if( %texture !$= "" ) {
			IPSP_Editor-->textureName_preview.setBitmap(%texture);
			IPSP_Editor-->textureName.setText(%texture);
			IPSP_Editor-->textureName.tooltip = %texture;
			IPSP_Editor.updateParticle( "textureName", %texture );
		}
	} else {
		IPSP_Editor-->textureName_preview.setBitmap("");
		IPSP_Editor-->textureName.setText("");
		IPSP_Editor-->textureName.tooltip = "";
		IPSP_Editor.updateParticle( "textureName", "" );
	}
}

//---------------------------------------------------------------------------------------------

function IPSP_Editor::updateLifeFields( %this, %isRandom, %value, %isSlider, %onMouseUp ) {
	IPSP_Editor.setParticleDirty();
	%particle = IPSP_Editor.currParticle;

	//Transfer values over to gui controls.

	if( %isRandom ) {
		%value ++;

		if( %value > IPSP_Editor-->lifetimeMS_slider.getValue() ) {
			IPSP_Editor-->lifetimeMS_edit.setText( %value );
			IPSP_Editor-->lifetimeMS_slider.setValue( %value );
		}
	} else {
		%value --;

		if( %value < IPSP_Editor-->lifetimeVarianceMS_slider.getValue() ) {
			IPSP_Editor-->lifetimeVarianceMS_edit.setText( %value );
			IPSP_Editor-->lifetimeVarianceMS_slider.setValue( %value );
		}
	}

	// Submit undo.
	%last = Editor.getUndoManager().getUndoAction(Editor.getUndoManager().getUndoCount() - 1);

	if( (%isSlider) && (%last.isSlider) && (!%last.onMouseUp) ) {
		%last.isSlider = %isSlider;
		%last.onMouseUp = %onMouseUp;
		%last.newValueLifetimeMS = IPSP_Editor-->lifetimeMS_edit.getText();
		%last.newValueLifetimeVarianceMS = IPSP_Editor-->lifetimeVarianceMS_edit.getText();
	} else {
		%action = IpsEditor.createUndo(ActionUpdateActiveParticleLifeFields, "Update Active Particle");
		%action.particle = %particle;
		%action.isSlider = %isSlider;
		%action.onMouseUp = %onMouseUp;
		%action.newValueLifetimeMS = IPSP_Editor-->lifetimeMS_edit.getText();
		%action.oldValueLifetimeMS = %particle.lifetimeMS;
		%action.newValueLifetimeVarianceMS = IPSP_Editor-->lifetimeVarianceMS_edit.getText();
		%action.oldValueLifetimeVarianceMS = %particle.lifetimeVarianceMS;
		IpsEditor.submitUndo( %action );
	}

	%particle.lifetimeMS = IPSP_Editor-->lifetimeMS_edit.getText();
	%particle.lifetimeVarianceMS = IPSP_Editor-->lifetimeVarianceMS_edit.getText();
	%particle.reload();
}

//---------------------------------------------------------------------------------------------

function IPSP_Editor::updateSpinFields( %this, %isMax, %value, %isSlider, %onMouseUp ) {
	IPSP_Editor.setParticleDirty();
	%particle = IPSP_Editor.currParticle;

	// Transfer values over to gui controls.
	if( %isMax ) {
		%value ++;

		if( %value > IPSP_Editor-->spinRandomMax_slider.getValue() ) {
			IPSP_Editor-->spinRandomMax_edit.setText( %value );
			IPSP_Editor-->spinRandomMax_slider.setValue( %value );
		}
	} else {
		%value --;

		if( %value < IPSP_Editor-->spinRandomMin_slider.getValue() ) {
			IPSP_Editor-->spinRandomMin_edit.setText( %value );
			IPSP_Editor-->spinRandomMin_slider.setValue( %value );
		}
	}

	// Submit undo.
	%last = Editor.getUndoManager().getUndoAction(Editor.getUndoManager().getUndoCount() - 1);

	if( (%isSlider) && (%last.isSlider) && (!%last.onMouseUp) ) {
		%last.isSlider = %isSlider;
		%last.onMouseUp = %onMouseUp;
		%last.newValueSpinRandomMax = IPSP_Editor-->spinRandomMax_edit.getText();
		%last.newValueSpinRandomMin = IPSP_Editor-->spinRandomMin_edit.getText();
	} else {
		%action = IpsEditor.createUndo(ActionUpdateActiveParticleSpinFields, "Update Active Particle");
		%action.particle = %particle;
		%action.isSlider = %isSlider;
		%action.onMouseUp = %onMouseUp;
		%action.newValueSpinRandomMax = IPSP_Editor-->spinRandomMax_edit.getText();
		%action.oldValueSpinRandomMax = %particle.spinRandomMax;
		%action.newValueSpinRandomMin = IPSP_Editor-->spinRandomMin_edit.getText();
		%action.oldValueSpinRandomMin = %particle.spinRandomMin;
		IpsEditor.submitUndo( %action );
	}

	%particle.spinRandomMax = IPSP_Editor-->spinRandomMax_edit.getText();
	%particle.spinRandomMin = IPSP_Editor-->spinRandomMin_edit.getText();
	%particle.reload();
}

//---------------------------------------------------------------------------------------------
