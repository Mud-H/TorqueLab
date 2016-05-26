//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//ToolsContainer to top right
//SideBarContainer Height - 100 bottom left

$FWPackage = FWPackage_Default;
package FWPackage_Default {	
   //==============================================================================

//------------------------------------------------------------------------------
	//==============================================================================
//Called from Toolbar and TerrainManager
function FW::setToolsToggleButton(%this,%toolCtrl) {
   %container = %toolCtrl.getParent();
	%button = EditorGuiMain-->ToolsToggle;
	EditorGuiMain-->ToolsExpander.visible = 0;
	if (!isObject(%button))
	{
	   %button = %this.getToolsToggleButton();
	  
	}
	 %container.add(%button);
	%button.docking = "right";
	%container.pushToBack(%button);
	//%button.AlignCtrlToParent("right","4");
	//%button.AlignCtrlToParent("top","4");
	
	%button.visible = !%toolCtrl.noToggleButton;
 
}
//------------------------------------------------------------------------------
//==============================================================================
//Called from Toolbar and TerrainManager
function FW::setToolsExpandButton(%this,%toolCtrl) {
	%container = %toolCtrl.getParent();
	%button = EditorGuiMain-->ToolsExpander;
	EditorGuiMain-->ToolsToggle.visible = 0;
	if (!isObject(%button))
	{
	   %button = %this.getToolsExpandButton();
	   %container.add(%button);
	}
	%button.docking = "right";
	%container.pushToBack(%button);
	//%button.AlignCtrlToParent("right","4");
	//%button.AlignCtrlToParent("top","4");
	%button.visible = !%toolCtrl.visible;
}
//------------------------------------------------------------------------------


};

activatePackage( FWPackage_Default );
