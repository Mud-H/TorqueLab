//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function SceneEd::setDatablockPage( %this,%ac ) {
SceneEd.populateTrees();
}
//==============================================================================
function SceneEd::showDatablockPage( %this ) {   
   
   SceneEd.populateTrees();
   
}
//==============================================================================
function SceneEd::canBeClientSideDatablock( %className ) {
	switch$( %className ) {
	case "SFXProfile" or
			"SFXPlayList" or
			"SFXAmbience" or
			"SFXEnvironment" or
			"SFXState" or
			"SFXDescription" or
			"SFXFMODProject":
		return true;

	default:
		return false;
	}
}
//------------------------------------------------------------------------------