//==============================================================================
// TorqueLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

$IPSC_Editor_DEFAULT_FILENAME = "art/gfx/particles/managedParticleData.cs";


//=============================================================================================
//    IPSC_Editor.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function IPSC_Editor::guiSync( %this,%listOnly ) {
	// Populate the selector with the particles assigned
	// to the current emitter.
	//%containsCurrParticle = false;
	//%popup = IPSC_Selector;
	//%popup.clear();

  
}


//=============================================================================================
//    IPS_ColorTintSwatch.
//=============================================================================================
function IPSC_Editor::selectEmitter( %this,%slot ) {
   eval("%emitter = IPSC_Editor.currComposite.emitter"@%slot@";");
	IPSE_Editor.loadNewEmitter(%emitter);

  
}

//=============================================================================================
//    IPSC_Selector_Control.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function IPSC_Selector_Control::onRenameItem( %this ) {
	Parent::onRenameItem( %this );
	//FIXME: need to check for validity of name and name clashes
	IPSC_Editor.setParticleDirty();
	// Resort menu.
	%this-->PopupMenu.sort();
}

//=============================================================================================
//    IPSC_NewParticleButton.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function IPSC_NewButton::onDefaultClick( %this ) {
	IPSC_Editor.showNewDialog();
}

//---------------------------------------------------------------------------------------------

function IPSC_NewParticleButton::onCtrlClick( %this ) {
	for( %i = 1; %i < 5; %i ++ ) {
		%popup = "IPSE_EmitterParticleSelector" @ %i;

		if( %popup.getSelected() == IPSC_Selector.getSelected() ) {
			%replaceSlot = %i;
			break;
		}
	}

	IPSC_Editor.showNewDialog( %replaceSlot );
}
