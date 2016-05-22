//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function SceneEditorTreeTabBook::onTabSelected( %this,%text, %index ) {
   SceneTreeWindow-->DeleteSelection.visible = false;
		SceneTreeWindow-->LockSelection.visible = false;
		SceneTreeWindow-->AddSimGroup.visible = false;
		   	%pageInt = %this.getObject(%index).internalName;

	if (%this.getObject(%index).isMethod("onPageSelected"))
	   %this.getObject(%index).onPageSelected(%this,%text,%index);
}
//------------------------------------------------------------------------------
