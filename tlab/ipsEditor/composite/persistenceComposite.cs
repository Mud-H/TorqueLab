

function IPSC_Editor::doSave( %this ) {
IPSC_Editor.saveComposite( IPSC_Editor.currComposite );
}
//---------------------------------------------------------------------------------------------

function IPSC_Editor::onNewComposite( %this ) {
	// Bail if the user selected the same particle.
	%id = IPSC_Selector.getSelected();
	
	
	

//	if( %id == IPSC_Editor.currComposite )
	//	return;

	// Load new particle if we're not in a dirty state
	if( IPSC_Editor.dirty && %id != IPSC_Editor.currComposite ) {
		LabMsgYesNoCancel("Save Existing Composite?",
								"Do you want to save changes to <br><br>" @ IPSC_Editor.currComposite.getName(),
								"IPSC_Editor.saveComposite(" @ IPSC_Editor.currComposite @ ");",
								"IPSC_Editor.saveCompositeDialogDontSave(" @ IPSC_Editor.currComposite @ "); IPSC_Editor.loadNewComposite();"
							  );
	} else {
		IPSC_Editor.loadNewComposite();
	}
}
function IPSC_Editor::onNewEmitter( %this,%slot ) {
   devLog("onNewEmitter","Slot",%slot);
   %ctrl = "IPSC_Emitter"@%slot@"_Control";
   %menu = %ctrl-->PopupMenu;
   %edit = %ctrl-->TextEdit;
   if (!isObject(%menu))
      return;
   
   %emitter = %menu.getSelected();
   
	IPSC_Editor.setCompositeDirty();
	%composite = IPSC_Editor.currComposite;
	devLog(%composite.getName(),"Emitter",%emitter.getName(),"Slot",%slot);
   
   %menu.getParent().updateFromChild(%menu);
	%composite.setFieldValue( "emitter"@%slot, %emitter.getName() );
	%edit.setText(%emitter.getName());
	%composite.reload();
}
//---------------------------------------------------------------------------------------------

function IPSC_Editor::loadNewComposite( %this, %composite ) {
	if( isObject( %composite ) )
		%composite = %composite.getId();
	else
		%composite = IPSC_Selector.getSelected();
	if (!isObject(	%composite))
	   %composite = IPSC_Editor.currComposite;
	%emitter1 = "";
	%emitter2 = "";	
   if (isObject(%composite.emitter1))
      %emitter1 = %composite.emitter1.getName();
   if (isObject(%composite.emitter2))
      %emitter2 = %composite.emitter2.getName();
  IPSC_Emitter1_Control-->PopupMenu.setText(%emitter1);
   IPSC_Emitter2_Control-->PopupMenu.setText(%emitter2);
IPSC_Emitter1_Control-->TextEdit.setText(%emitter1);
   IPSC_Emitter2_Control-->TextEdit.setText(%emitter2);
   
	IPSC_Editor.currComposite = %composite;
	%composite.reload();
	
	IPSC_Editor_NotDirtyComposite.assignFieldsFrom( %composite );
	IPSC_Editor_NotDirtyComposite.originalName = %composite.getName();
	
	IPSC_Editor.guiSync();
	IPSC_Editor.setCompositeNotDirty();
	IPSC_Selector_Control-->TextEdit.setText(%composite.getName());
	
	$IPS_NodeMode = "Composite";
	IpsEditor.updateEmitterNode();
}

//---------------------------------------------------------------------------------------------

function IPSC_Editor::setCompositeDirty( %this ) {
	IPSC_Editor.text = "Composite *";
	IPSC_Editor.dirty = true;
	%particle = IPSC_Editor.currComposite;

	if( %particle.getFilename() $= "" || %particle.getFilename() $= "tlab/IpsEditor/particleIpsEditor.ed.cs" )
		IPS_CompositeSaver.setDirty( %particle, $IPSC_Editor_DEFAULT_FILENAME );
	else
		IPS_CompositeSaver.setDirty( %particle );
}

//---------------------------------------------------------------------------------------------

function IPSC_Editor::setCompositeNotDirty( %this ) {
	IPSC_Editor.text = "Composite";
	IPSC_Editor.dirty = false;
	IPS_CompositeSaver.clearAll();
}

//---------------------------------------------------------------------------------------------

function IPSC_Editor::showNewDialog( %this, %replaceSlot ) {
	// Open a dialog if the current Particle is dirty
	if( IPSC_Editor.dirty ) {
		LabMsgYesNoCancel("Save Composite Changes?",
								"Do you wish to save the changes made to the <br>current particle before changing the particle?",
								"IPSC_Editor.saveComposite( " @ IPSC_Editor.currComposite.getName() @ " ); IPSC_Editor.createComposite( " @ %replaceSlot @ " );",
								"IPSC_Editor.saveCompositeDialogDontSave( " @ IPSC_Editor.currComposite.getName() @ " ); IPSC_Editor.createComposite( " @ %replaceSlot @ " );"
							  );
	} else {
		IPSC_Editor.createComposite( %replaceSlot );
	}
}

//---------------------------------------------------------------------------------------------

function IPSC_Editor::createComposite( %this, %replaceSlot ) {
	// Make sure we have a spare slot on the current emitter.
	/*if( !%replaceSlot ) {
		%numExistingParticles = getWordCount( IPSE_Editor.currComposite.particles );

		if( %numExistingParticles > 3 ) {
			LabMsgOK( "Error", "An emitter cannot have more than 4 particles assigned to it." );
			return;
		}

		%particleIndex = %numExistingParticles;
	} else
		%particleIndex = %replaceSlot - 1;
*/
%particleIndex = 1;
	// Create the particle datablock and add to the emitter.
	%newComposite = getUniqueName( "newComposite" );
	datablock CompositeEmitterData( %newComposite : DefaultComposite ) {
	    Particles = AxelTestParticle03; 
      Emitter1 = LavaBallEmitter;
      Emitter2 = LavaDisturbanceEmitter;
	};
	devLog("Composite created:",%newComposite,"Slot",%replaceSlot);
	// Submit undo.
	%action = IpsEditor.createUndo( ActionCreateNewComposite, "Create New Composite" );
	%action.composite = %newComposite.getId();
	%action.compositeName = %newComposite;
	%action.compositeIndex = %replaceSlot;
	//%action.prevComposite = ( "IPSE_EmitterParticleSelector" @ ( %particleIndex + 1 ) ).getSelected();
	//%action.emitter = IPSC_Editor.currComposite;
	IpsEditor.submitUndo( %action );
	// Execute action.
	%action.redo();
	%newComposite.setFileName(DefaultComposite.getFilename());
	IPSC_Selector.setSelected(%newComposite.getId());
	IPSC_Editor.currComposite = %newComposite.getId();
}

//---------------------------------------------------------------------------------------------

function IPSC_Editor::showDeleteDialog( %this ) {
	// Don't allow deleting DefaultParticle.
	if( IPSC_Editor.currComposite.getName() $= "DefaultComposite" ) {
		LabMsgOK( "Error", "Cannot delete DefaultParticle");
		return;
	}

	

	// Bring up requester for confirmation.

	if( isObject( IPSC_Editor.currComposite ) ) {
		LabMsgYesNoCancel( "Delete Composite?",
								 "Are you sure you want to delete<br><br>" @ IPSC_Editor.Composite.getName() @ "<br><br> Particle deletion won't take affect until the engine is quit.",
								 "IPSC_Editor.saveCompositeDialogDontSave( " @ IPSC_Editor.Composite.getName() @ " ); IPSC_Editor.deleteComposite();",
								 "",
								 ""
							  );
	}
}

//---------------------------------------------------------------------------------------------

function IPSC_Editor::deleteComposite( %this ) {
	%particle = IPSC_Editor.currComposite;
	// Submit undo.
	%action = IpsEditor.createUndo( ActionDeleteComposite, "Delete Composite" );
	%action.composite = %particle;
	//%action.emitter = IPSE_Editor.currEmitter;
	IpsEditor.submitUndo( %action );
	// Execute action.
	%action.redo();
}

//---------------------------------------------------------------------------------------------

function IPSC_Editor::saveComposite( %this, %composite ) {
	%composite.setName( IPSC_Selector_Control-->TextEdit.getText() );
	IPSC_Editor_NotDirtyComposite.assignFieldsFrom( %composite );
	IPSC_Editor_NotDirtyComposite.originalName = %composite.getName();
	IPS_CompositeSaver.saveDirty();
	IPSC_Editor.setCompositeNotDirty();
	IpsEditor.createDataList();
}

//---------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------

function IPSC_Editor::saveCompositeDialogDontSave( %this, %particle ) {
	%particle.setName( IPSC_Editor_NotDirtyComposite.originalName );
	%particle.assignFieldsFrom( IPSC_Editor_NotDirtyComposite );
	IPSC_Editor.setCompositeNotDirty();
}



datablock CompositeEmitterData(newComposite2 : DefaultComposite)
{
   Emitter1 = "LightningFlashData";
};
