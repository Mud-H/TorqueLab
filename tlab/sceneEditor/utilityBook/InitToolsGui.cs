//==============================================================================
// TorqueLab -> SceneEditor Inspector script
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================

//==============================================================================
// Prefabs
//==============================================================================
function SceneEd::initToolsGui( %this ) {
	SETools_CreateOptClassMenu.clear();
	SETools_CreateOptClassMenu.add("Zone",0);
	SETools_CreateOptClassMenu.add("Portal",1);
	SETools_CreateOptClassMenu.add("OcclusionVolume",2);
	SETools_CreateOptClassMenu.setText("Zone");
	
	%transformStack = SEP_TransformRollout-->toolsStack;
	if (!isObject(%transformStack))
		return;
	%stackList = %transformStack.getId() SPC "SEP_PageSetupStack SEP_PageUtilitypStack";
	foreach$(%stack in %stackList){
		foreach(%gui in %stack){
			%gui.expanded = false;
		}
	}
}

