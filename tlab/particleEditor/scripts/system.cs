//==============================================================================
// TorqueLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

$PE_PARTICLEEDITOR_DEFAULT_FILENAME = "art/gfx/particles/managedParticleData.cs";


//=============================================================================================
//    PE_ParticleEditor.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function PE_ParticleEditor::doSystemSave( %this ) {
	%this.doEmitterSave();
	%this.doParticleSave();
}

function PE_ParticleEditor::refreshParticleList( %this ) {
	
	PE_ParticleEditor.guiSync(true);
}
