//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------


//=============================================================================================
//    IpsEditor.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function IpsEditor::createUndo( %this, %class, %desc ) {
	pushInstantGroup();
	%action = new UndoScriptAction() {
		class = %class;
		superClass = BaseIpsEdAction;
		actionName = %desc;
	};
	popInstantGroup();
	return %action;
}

//---------------------------------------------------------------------------------------------

function IpsEditor::submitUndo( %this, %action ) {
	%action.addToManager( Editor.getUndoManager() );
}

//=============================================================================================
//    BaseIpsEdAction.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function BaseIpsEdAction::sync( %this ) {
	// Sync particle state.
	if( isObject( %this.particle ) ) {
		%this.particle.reload();
		IPSP_Editor.guiSync();

		if( %this.particle.getId() == IPSP_Editor.currParticle.getId() )
			IPSP_Editor.setParticleDirty();
	}

	// Sync emitter state.

	if( isObject( %this.emitter ) ) {
		%this.emitter.reload();
		IPSE_Editor.guiSync();

		if( %this.emitter.getId() == IPSE_Editor.currEmitter.getId() )
			IPSE_Editor.setEmitterDirty();
	}
}

//---------------------------------------------------------------------------------------------

function BaseIpsEdAction::redo( %this ) {
	%this.sync();
}

//---------------------------------------------------------------------------------------------

function BaseIpsEdAction::undo( %this ) {
	%this.sync();
}

//=============================================================================================
//    ActionRenameEmitter.
//=============================================================================================

//---------------------------------------------------------------------------------------------

//TODO

//=============================================================================================
//    ActionCreateNewEmitter.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function ActionCreateNewEmitter::redo( %this ) {
	%emitter = %this.emitter;
	// Assign name.
	%emitter.name = %this.emitterName;
	// Remove from unlisted.
	IPS_UnlistedEmitters.remove( %emitter );
	// Drop it in the dropdown and select it.
	%popup = IPSE_Selector;
	%popup.add( %emitter.getName(), %emitter.getId() );
	%popup.sort();
	%popup.setSelected( %emitter.getId() );
	// Sync up.
	Parent::redo( %this );
}

//---------------------------------------------------------------------------------------------

function ActionCreateNewEmitter::undo( %this ) {
	%emitter = %this.emitter;

	// Prevent a save dialog coming up on the emitter.

	if( %emitter == IPSE_Editor.currEmitter )
		IPSE_Editor.setEmitterNotDirty();

	// Add to unlisted.
	IPS_UnlistedEmitters.add( %emitter );
	// Remove it from in the dropdown and select prev emitter.
	%popup = IPSE_Selector;

	if( isObject( %this.prevEmitter ) )
		%popup.setSelected( %this.prevEmitter.getId() );
	else
		%popup.setFirstSelected();

	%popup.clearEntry( %emitter.getId() );
	// Unassign name.
	%this.emitterName = %emitter.name;
	%emitter.name = "";
	// Sync up.
	Parent::undo( %this );
}

//=============================================================================================
//    ActionDeleteEmitter.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function ActionDeleteEmitter::redo( %this ) {
	%emitter = %this.emitter;
	// Unassign name.
	%this.emitterName = %emitter.name;
	%emitter.name = "";
	// Add to unlisted.
	IPS_UnlistedEmitters.add( %emitter );

	// Remove from file.

	if(    %emitter.getFileName() !$= ""
												 && %emitter.getFilename() !$= "tlab/IpsEditor/particleEmitterEditor.ed.cs" )
		IPS_EmitterSaver.removeObjectFromFile( %emitter );

	// Select DefaultEmitter or first in list.
	%popup = IPSE_Selector_Control-->PopUpMenu;
	%popup.setFirstSelected();
	// Remove from dropdown.
	%popup.clearEntry( %emitter );
	// Sync up.
	Parent::redo( %this );
}

//---------------------------------------------------------------------------------------------

function ActionDeleteEmitter::undo( %this ) {
	%emitter = %this.emitter;
	// Re-assign name.
	%emitter.name = %this.emitterName;
	// Remove from unlisted.
	IPS_UnlistedEmitters.remove( %emitter );

	// Resave to file.

	if(    %this.emitterFname !$= ""
											&& %this.emitterFname !$= "tlab/IpsEditor/particleEmitterEditor.ed.gui" ) {
		IPS_EmitterSaver.setDirty( %emitter, %this.emitterFname );
		IPS_EmitterSaver.saveDirty();
	}

	// Add it to the dropdown and selet it.
	%popup = IPSE_Selector_Control-->PopUpMenu;
	%popup.add( %emitter.getName(), %emitter.getId() );
	%popup.sort();
	%popup.setSelected( %emitter.getId() );
	// Sync up.
	Parent::undo( %this );
}

//=============================================================================================
//    ActionUpdateActiveEmitter.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function ActionUpdateActiveEmitter::redo( %this ) {
	%emitter = %this.emitter;
	%emitter.setFieldValue( %this.field, %this.newValue );
	Parent::redo( %this );
}

//---------------------------------------------------------------------------------------------

function ActionUpdateActiveEmitter::undo( %this ) {
	%emitter = %this.emitter;
	%emitter.setFieldValue( %this.field, %this.oldValue );
	Parent::undo( %this );
}

//=============================================================================================
//    ActionUpdateActiveEmitterLifeFields.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function ActionUpdateActiveEmitterLifeFields::redo( %this ) {
	%emitter = %this.emitter;
	%emitter.lifetimeMS = %this.newValueLifetimeMS;
	%emitter.lifetimeVarianceMS = %this.newValueLifetimeVarianceMS;
	Parent::redo( %this );
}

//---------------------------------------------------------------------------------------------

function ActionUpdateActiveEmitterLifeFields::undo( %this ) {
	%emitter = %this.emitter;
	%emitter.lifetimeMS = %this.oldValueLifetimeMS;
	%emitter.lifetimeVarianceMS = %this.oldValueLifetimeVarianceMS;
	Parent::undo( %this );
}

//=============================================================================================
//    ActionUpdateActiveEmitterAmountFields.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function ActionUpdateActiveEmitterAmountFields::redo( %this ) {
	%emitter = %this.emitter;
	%emitter.ejectionPeriodMS = %this.newValueEjectionPeriodMS;
	%emitter.periodVarianceMS = %this.newValuePeriodVarianceMS;
	Parent::redo( %this );
}

//---------------------------------------------------------------------------------------------

function ActionUpdateActiveEmitterAmountFields::undo( %this ) {
	%emitter = %this.emitter;
	%emitter.ejectionPeriodMS = %this.oldValueEjectionPeriodMS;
	%emitter.periodVarianceMS = %this.oldValuePeriodVarianceMS;
	Parent::undo( %this );
}

//=============================================================================================
//    ActionUpdateActiveEmitterSpeedFields.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function ActionUpdateActiveEmitterSpeedFields::redo( %this ) {
	%emitter = %this.emitter;
	%emitter.ejectionVelocity = %this.newValueEjectionVelocity;
	%emitter.velocityVariance = %this.newValueVelocityVariance;
	Parent::redo( %this );
}

//---------------------------------------------------------------------------------------------

function ActionUpdateActiveEmitterSpeedFields::undo( %this ) {
	%emitter = %this.emitter;
	%emitter.ejectionVelocity = %this.oldValueEjectionVelocity;
	%emitter.velocityVariance = %this.oldValueVelocityVariance;
	Parent::undo( %this );
}

//=============================================================================================
//    ActionCreateNewParticle.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function ActionCreateNewParticle::redo( %this ) {
	%particle = %this.particle.getName();
	%particleId = %this.particle.getId();
	%particleIndex = %this.particleIndex;
	%emitter = %this.emitter;
	// Remove from unlisted.
	IPS_UnlistedParticles.remove( %particleId );
	// Add it to the dropdown.
	IPSP_Selector.add( %particle, %particleId );
	IPSP_Selector.sort();
	IPSP_Selector.setSelected( %particleId, false );

	// Add particle to dropdowns in the emitter editor.

	for( %i = 1; %i < 5; %i ++ ) {
		%emitterParticle = "IPSE_EmitterParticle" @ %i;
		%popup = %emitterParticle-->PopupMenu;
		%popup.add( %particle, %particleId );
		%popup.sort();

		if( %i == %particleIndex + 1 )
			%popup.setSelected( %particleId );
	}

	// Sync up.
	IPSP_Editor.loadNewParticle();
	Parent::redo( %this );
}

//---------------------------------------------------------------------------------------------

function ActionCreateNewParticle::undo( %this ) {
	%particle = %this.particle.getName();
	%particleId = %this.particle.getId();
	%emitter = %this.emitter;
	// Add to unlisted.
	IPS_UnlistedParticles.add( %particleId );
	// Remove from dropdown.
	IPSP_Selector.clearEntry( %particleId );
	IPSP_Selector.setFirstSelected( false );

	// Remove from particle dropdowns in emitter editor.

	for( %i = 1; %i < 5; %i ++ ) {
		%emitterParticle = "IPSE_EmitterParticle" @ %i;
		%popup = %emitterParticle-->PopUpMenu;

		if( %popup.getSelected() == %particleId )
			%popup.setSelected( %this.prevParticle );

		%popup.clearEntry( %particleId );
	}

	// Sync up.
	IPSP_Editor.loadNewParticle();
	Parent::undo( %this );
}

//=============================================================================================
//    ActionDeleteParticle.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function ActionDeleteParticle::redo( %this ) {
	%particle = %this.particle.getName();
	%particleId = %this.particle.getId();
	%emitter = %this.emitter;
	// Add to unlisted.
	IPS_UnlistedParticles.add( %particleId );

	// Remove from file.

	if(    %particle.getFileName() !$= ""
												  && %particle.getFilename() !$= "tlab/IpsEditor/particleIpsEditor.ed.cs" )
		IPS_ParticleSaver.removeObjectFromFile( %particleId );

	// Remove from dropdown.
	IPSP_Selector.clearEntry( %particleId );
	IPSP_Selector.setFirstSelected();

	// Remove from particle selectors in emitter.

	for( %i = 1; %i < 5; %i ++ ) {
		%emitterParticle = "IPSE_EmitterParticle" @ %i;
		%popup = %emitterParticle-->PopUpMenu;

		if( %popup.getSelected() == %particleId ) {
			%this.particleIndex = %i - 1;
			%popup.setSelected( 0 ); // Select "None".
		}

		%popup.clearEntry( %particleId );
	}

	// Sync up.
	IPSP_Editor.loadNewParticle();
	Parent::redo( %this );
}

//---------------------------------------------------------------------------------------------

function ActionDeleteParticle::undo( %this ) {
	%particle = %this.particle.getName();
	%particleId = %this.particle.getId();
	%particleIndex = %this.particleIndex;
	%emitter = %this.emitter;
	// Remove from unlisted.
	IPS_UnlistedParticles.remove( %particleId );

	// Resave to file.

	if(    %particle.getFilename() !$= ""
												  && %particle.getFilename() !$= "tlab/IpsEditor/particleIpsEditor.ed.gui" ) {
		IPS_ParticleSaver.setDirty( %particle );
		IPS_ParticleSaver.saveDirty();
	}

	// Add to dropdown.
	IPSP_Selector.add( %particle, %particleId );
	IPSP_Selector.sort();
	IPSP_Selector.setSelected( %particleId );

	// Add particle to dropdowns in the emitter editor.

	for( %i = 1; %i < 5; %i ++ ) {
		%emitterParticle = "IPSE_EmitterParticle" @ %i;
		%popup = %emitterParticle-->PopUpMenu;
		%popup.add( %particle, %particleId );
		%popup.sort();

		if( %i == %particleIndex + 1 )
			%popup.setSelected( %particleId );
	}

	// Sync up.
	IPSP_Editor.loadNewParticle();
	Parent::undo( %This );
}

//=============================================================================================
//    ActionUpdateActiveParticle.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function ActionUpdateActiveParticle::redo( %this ) {
	%particle = %this.particle;
	%particle.setFieldValue( %this.field, %this.newValue );
	Parent::redo( %this );
}

function ActionUpdateActiveParticle::undo( %this ) {
	%particle = %this.particle;
	%particle.setFieldValue( %this.field, %this.oldValue );
	Parent::undo( %this );
}

//=============================================================================================
//    ActionUpdateActiveParticleLifeFields.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function ActionUpdateActiveParticleLifeFields::redo( %this ) {
	%particle = %this.particle;
	%particle.lifetimeMS = %this.newValueLifetimeMS;
	%particle.lifetimeVarianceMS = %this.newValueLifetimeVarianceMS;
	Parent::redo( %this );
}

//---------------------------------------------------------------------------------------------

function ActionUpdateActiveParticleLifeFields::undo( %this ) {
	%particle = %this.particle;
	%particle.lifetimeMS = %this.oldValueLifetimeMS;
	%particle.lifetimeVarianceMS = %this.oldValueLifetimeVarianceMS;
	Parent::undo( %this );
}

//=============================================================================================
//    ActionUpdateActiveParticleSpinFields.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function ActionUpdateActiveParticleSpinFields::redo( %this ) {
	%particle = %this.particle;
	%particle.spinRandomMax = %this.newValueSpinRandomMax;
	%particle.spinRandomMin = %this.newValueSpinRandomMin;
	Parent::redo( %this );
}

//---------------------------------------------------------------------------------------------

function ActionUpdateActiveParticleSpinFields::undo( %this ) {
	%particle = %this.particle;
	%particle.spinRandomMax = %this.oldValueSpinRandomMax;
	%particle.spinRandomMin = %this.oldValueSpinRandomMin;
	Parent::undo( %this );
}

//=============================================================================================
//    ActionCreateNewEmitter.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function ActionCreateNewComposite::redo( %this ) {
	%composite = %this.composite;
	// Assign name.
	%composite.name = %this.compositeName;
	// Remove from unlisted.
	IPS_UnlistedComposites.remove( %composite );
	// Drop it in the dropdown and select it.
	%popup = IPSC_Selector;
	%popup.add( %composite.getName(), %composite.getId() );
	%popup.sort();
	%popup.setSelected( %composite.getId() );
	// Sync up.
	Parent::redo( %this );
}

//---------------------------------------------------------------------------------------------

function ActionCreateNewComposite::undo( %this ) {
	%composite = %this.composite;

	// Prevent a save dialog coming up on the emitter.

	if( %composite == IPSC_Editor.currComposite )
		IPSC_Editor.setCompositeNotDirty();

	// Add to unlisted.
	IPS_UnlistedComposites.add( %composite );
	// Remove it from in the dropdown and select prev emitter.
	%popup = IPSC_Selector;

	if( isObject( %this.prevComposite ) )
		%popup.setSelected( %this.prevComposite.getId() );
	else
		%popup.setFirstSelected();

	%popup.clearEntry( %composite.getId() );
	// Unassign name.
	%this.compositeName = %composite.name;
	%composite.name = "";
	// Sync up.
	Parent::undo( %this );
}
