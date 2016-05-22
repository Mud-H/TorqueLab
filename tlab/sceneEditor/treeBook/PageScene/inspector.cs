//==============================================================================
// TorqueLab -> SceneEditor Inspector script
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================

//==============================================================================
// Hack to force LevelInfo update after Cubemap change...
//==============================================================================
//==============================================================================
function SceneInspector::envHack( %this ) {
	envHack(true);
}
function SceneInspector::envDel( %this ) {
	envDel();
}
//------------------------------------------------------------------------------
function envHack(%autodelete ) {
	%tmpObj = new EnvVolume("tmpEnvVolume") {
		AreaEnvMap = "MipCubemap";
		cubeReflectorDesc = "DefaultCubeDesc";
		position = "8.21068 19.3464 241.855";
		rotation = "1 0 0 0";
		scale = "10 10 10";
		canSave = "1";
		canSaveDynamicFields = "1";
	};

	if (%autodelete)
		SceneInspector.schedule(500,"envDel");
}
//------------------------------------------------------------------------------
//==============================================================================
function envDel( ) {
	delObj(tmpEnvVolume);
}
//------------------------------------------------------------------------------

