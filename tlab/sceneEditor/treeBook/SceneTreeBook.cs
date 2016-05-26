//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$SceneEd_TreePage = 0;
$SceneEd_TreeMode = "Scene";
function SceneEditorTreeTabBook::init( %this ) {
	%this.selectPage($SceneEd_TreePage);
}



//==============================================================================
function SceneEditorTreeTabBook::onTabSelected( %this,%text, %index ) {
   $SceneEd_TreePage = %index;
   SceneTreeWindow-->DeleteSelection.visible = false;
		SceneTreeWindow-->LockSelection.visible = false;
		SceneTreeWindow-->AddSimGroup.visible = false;
		   	%pageInt = %this.getObject(%index).internalName;
   
   $SceneEd_TreeMode = %pageInt;
	if (%this.getObject(%index).isMethod("onPageSelected"))
	   %this.getObject(%index).onPageSelected(%this,%text,%index);
}
//------------------------------------------------------------------------------
