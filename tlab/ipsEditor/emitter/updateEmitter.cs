function IPSE_Editor::updateEmitterCtrl(%this,%ctrl,%onMouseUp) {
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
	devLog("updateEmitter %propertyField",%propertyField,"%value",%value);
	%this.updateEmitter(%propertyField,%value,%isSlider,%onMouseUp);
	
}

// Generic updateEmitter method
function IPSE_Editor::updateEmitter( %this, %propertyField, %value, %isSlider, %onMouseUp ) {
	IPSE_Editor.setEmitterDirty();
	%emitter = IPSE_Editor.currEmitter;
	
	%last = Editor.getUndoManager().getUndoAction(Editor.getUndoManager().getUndoCount() - 1);

	if( (%isSlider) && (%last.isSlider) && (!%last.onMouseUp) ) {
		%last.field = %propertyField;
		%last.isSlider = %isSlider;
		%last.onMouseUp = %onMouseUp;
		%last.newValue = %value;
	} else {
		%action = IpsEditor.createUndo(ActionUpdateActiveEmitter, "Update Active Emitter");
		%action.emitter = %emitter;
		%action.field = %propertyField;
		%action.isSlider = %isSlider;
		%action.onMouseUp = %onMouseUp;
		%action.newValue = %value;
		%action.oldValue = %emitter.getFieldValue( %propertyField );
		IpsEditor.submitUndo( %action );
	}

	%emitter.setFieldValue( %propertyField, %value );
	%emitter.reload();
}

//---------------------------------------------------------------------------------------------

// Special case updateEmitter methods
function IPSE_Editor::updateLifeFields( %this, %isRandom, %value, %isSlider, %onMouseUp ) {
	IPSE_Editor.setEmitterDirty();
	%emitter = IPSE_Editor.currEmitter;

	// Transfer values over to gui controls.

	if( %isRandom ) {
		if( %value > 0 )
			%value++;

		if( %value > IPSE_Editor-->lifetimeMS_slider.getValue() ) {
			IPSE_Editor-->lifetimeMS_edit.setText( %value );
			IPSE_Editor-->lifetimeMS_slider.setValue( %value );
		}
	} else {
		if( %value > 0 )
			%value --;

		if( %value < IPSE_Editor-->lifetimeVarianceMS_slider.getValue() ) {
			IPSE_Editor-->lifetimeVarianceMS_edit.setText( %value );
			IPSE_Editor-->lifetimeVarianceMS_slider.setValue( %value );
		}
	}

	// Submit undo.
	%last = Editor.getUndoManager().getUndoAction(Editor.getUndoManager().getUndoCount() - 1);

	if( (%isSlider) && (%last.isSlider) && (!%last.onMouseUp) ) {
		%last.isSlider = %isSlider;
		%last.onMouseUp = %onMouseUp;
		%last.newValueLifetimeMS = IPSE_Editor-->lifetimeMS_edit.getText();
		%last.newValueLifetimeVarianceMS = IPSE_Editor-->lifetimeVarianceMS_edit.getText();
	} else {
		%action = IpsEditor.createUndo(ActionUpdateActiveEmitterLifeFields, "Update Active Emitter");
		%action.emitter = %emitter;
		%action.isSlider = %isSlider;
		%action.onMouseUp = %onMouseUp;
		%action.newValueLifetimeMS = IPSE_Editor-->lifetimeMS_edit.getText();
		%action.oldValueLifetimeMS = %emitter.lifetimeMS;
		%action.newValueLifetimeVarianceMS = IPSE_Editor-->lifetimeVarianceMS_edit.getText();
		%action.oldValueLifetimeVarianceMS = %emitter.lifetimeVarianceMS;
		IpsEditor.submitUndo( %action );
	}

	// Set the values on the current emitter.
	%emitter.lifetimeMS = IPSE_Editor-->lifetimeMS_edit.getText();
	%emitter.lifetimeVarianceMS = IPSE_Editor-->lifetimeVarianceMS_edit.getText();
	%emitter.reload();
	// Keep the infiniteLoop checkbox up to date.
	IPSE_Editor-->infiniteLoop.setStateOn(
		%emitter.lifetimeMS == 0
	);
}

//---------------------------------------------------------------------------------------------

function IPSE_Editor::updateLifeFieldsInfiniteLoop( %this ) {
	%emitter = IPSE_Editor.currEmitter;
	%isEnabled = IPSE_Editor-->infiniteLoop.isStateOn();
	// Submit undo.
	%action = IpsEditor.createUndo( ActionUpdateActiveEmitterLifeFields, "Update Active Emitter" );
	%action.emitter = %emitter;

	if( %isEnabled ) {
		%action.newValueLifetimeMS = 0;
		%action.newvalueLifetimeVarianceMS = 0;
		%action.oldValueLifetimeMS = IPSE_Editor-->lifetimeMS_edit.getText();
		%action.oldValueLifetimeVarianceMS = IPSE_Editor-->lifetimeVarianceMS_edit.getText();
		//%this.defaultLifeTimeMS[%emitter.getId()] = %emitter.lifetimeMS;
		//%emitter.lifetimeMS = 0;
	} else {
		
		%action.newValueLifetimeMS = IPSE_Editor-->lifetimeMS_edit.getText();
		%action.newvalueLifetimeVarianceMS = IPSE_Editor-->lifetimeVarianceMS_edit.getText();
		%action.oldValueLifetimeMS = 0;
		%action.oldValueLifetimeVarianceMS = 0;		
		//%emitter.lifetimeMS = %this.defaultLifeTimeMS[%emitter.getId()];
		//IPSE_Editor-->lifetimeMS_edit.setText(%emitter.lifetimeMS);
		
	}

	IpsEditor.submitUndo( %action );
	// Execute action.
	%action.redo();
}

//---------------------------------------------------------------------------------------------

function IPSE_Editor::updateAmountFields( %this, %isRandom, %value, %isSlider, %onMouseUp ) {
	IPSE_Editor.setEmitterDirty();
	%emitter = IPSE_Editor.currEmitter;

	// Transfer values over to gui controls.

	if( %isRandom ) {
		%value ++;

		if( %value > IPSE_Editor-->ejectionPeriodMS_slider.getValue() ) {
			IPSE_Editor-->ejectionPeriodMS_edit.setText( %value );
			IPSE_Editor-->ejectionPeriodMS_slider.setValue( %value );
		}
	} else {
		%value --;

		if( %value < IPSE_Editor-->periodVarianceMS_slider.getValue() ) {
			IPSE_Editor-->periodVarianceMS_edit.setText( %value );
			IPSE_Editor-->periodVarianceMS_slider.setValue( %value );
		}
	}

	// Submit undo.
	%last = Editor.getUndoManager().getUndoAction(Editor.getUndoManager().getUndoCount() - 1);

	if( (%isSlider) && (%last.isSlider) && (!%last.onMouseUp) ) {
		%last.isSlider = %isSlider;
		%last.onMouseUp = %onMouseUp;
		%last.newValueEjectionPeriodMS = IPSE_Editor-->ejectionPeriodMS_edit.getText();
		%last.newValuePeriodVarianceMS = IPSE_Editor-->periodVarianceMS_edit.getText();
	} else {
		%action = IpsEditor.createUndo(ActionUpdateActiveEmitterAmountFields, "Update Active Emitter");
		%action.emitter = %emitter;
		%action.isSlider = %isSlider;
		%action.onMouseUp = %onMouseUp;
		%action.newValueEjectionPeriodMS = IPSE_Editor-->ejectionPeriodMS_edit.getText();
		%action.oldValueEjectionPeriodMS = %emitter.ejectionPeriodMS;
		%action.newValuePeriodVarianceMS = IPSE_Editor-->periodVarianceMS_edit.getText();
		%action.oldValuePeriodVarianceMS = %emitter.periodVarianceMS;
		IpsEditor.submitUndo( %action );
	}

	// Set the values on the current emitter.
	%emitter.ejectionPeriodMS = IPSE_Editor-->ejectionPeriodMS_edit.getText();
	%emitter.periodVarianceMS = IPSE_Editor-->periodVarianceMS_edit.getText();
	%emitter.reload();
}

//---------------------------------------------------------------------------------------------

function IPSE_Editor::updateSpeedFields( %this, %isRandom, %value, %isSlider, %onMouseUp ) {
	IPSE_Editor.setEmitterDirty();
	%emitter = IPSE_Editor.currEmitter;

	// Transfer values over to gui controls.

	if( %isRandom ) {
		if( %value > IPSE_Editor-->ejectionVelocity_slider.getValue() ) {
			IPSE_Editor-->ejectionVelocity_edit.setText( %value );
			IPSE_Editor-->ejectionVelocity_slider.setValue( %value );
		}
	} else {
		if( %value < IPSE_Editor-->velocityVariance_slider.getValue() ) {
			IPSE_Editor-->velocityVariance_edit.setText( %value );
			IPSE_Editor-->velocityVariance_slider.setValue( %value );
		}
	}

	// Submit undo.
	%last = Editor.getUndoManager().getUndoAction(Editor.getUndoManager().getUndoCount() - 1);

	if( (%isSlider) && (%last.isSlider) && (!%last.onMouseUp) ) {
		%last.isSlider = %isSlider;
		%last.onMouseUp = %onMouseUp;
		%last.newValueEjectionVelocity = IPSE_Editor-->ejectionVelocity_edit.getText();
		%last.newValueVelocityVariance = IPSE_Editor-->velocityVariance_edit.getText();
	} else {
		%action = IpsEditor.createUndo(ActionUpdateActiveEmitterSpeedFields, "Update Active Emitter");
		%action.emitter = %emitter;
		%action.isSlider = %isSlider;
		%action.onMouseUp = %onMouseUp;
		%action.newValueEjectionVelocity = IPSE_Editor-->ejectionVelocity_edit.getText();
		%action.oldValueEjectionVelocity = %emitter.ejectionVelocity;
		%action.newValueVelocityVariance = IPSE_Editor-->velocityVariance_edit.getText();
		%action.oldValueVelocityVariance = %emitter.velocityVariance;
		IpsEditor.submitUndo( %action );
	}

	// Set the values on the current emitter.
	%emitter.ejectionVelocity = IPSE_Editor-->ejectionVelocity_edit.getText();
	%emitter.velocityVariance = IPSE_Editor-->velocityVariance_edit.getText();
	%emitter.reload();
}

//---------------------------------------------------------------------------------------------

function IPSE_Editor::updateParticlesFields( %this ) {
	%particles = "";

	for( %i = 1; %i < 5; %i ++ ) {
		%emitterParticle = "IPSE_EmitterParticle" @ %i;
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
	%currParticle = IPSP_Editor.currParticle.getName();

	foreach$( %particleName in %particles ) {
		if( %particleName $= %currParticle ) {
			%changedEditParticle = 0;
			break;
		}
	}

	// True only if the currently edited particle has not been found and the
	// IpsEditor is dirty.

	if( %changedEditParticle && IPSP_Editor.dirty ) {
		MessageBoxYesNoCancel("Save Particle Changes?",
									 "Do you wish to save the changes made to the <br>current particle before changing the particle?",
									 "IPSP_Editor.saveParticle( " @ IPSP_Editor.currParticle.getName() @ " ); IPSE_Editor.updateEmitter( \"particles\"," @ %particles @ ");",
									 "IPSP_Editor.saveParticleDialogDontSave( " @ IPSP_Editor.currParticle.getName() @ " ); IPSE_Editor.updateEmitter( \"particles\"," @ %particles @ ");",
									 "IPSE_Editor.guiSync();" );
	} else {
		IPSE_Editor.updateEmitter( "particles", %particles );
	}
}
