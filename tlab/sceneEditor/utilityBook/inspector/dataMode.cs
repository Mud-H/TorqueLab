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
function SceneEd::setInspectorDataMode( %this,%mode ) {
	SceneInspector.dataMode = %mode;
	devLog("Inspectordata mode changed to:",%mode);
	if (%mode $= "Datablock")
	   %this.setInspectorDatablockMode();
   else
      %this.setInspectorObjectMode();
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEd::setInspectorDatablockMode( %this,%mode ) {
	SceneEditorUtilityBook-->InspectorIcons_Datablock.visible = true;
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEd::setInspectorObjectMode( %this,%mode ) {
	SceneEditorUtilityBook-->InspectorIcons_Datablock.visible = false;
}
//------------------------------------------------------------------------------