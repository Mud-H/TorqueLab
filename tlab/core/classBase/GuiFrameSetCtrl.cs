//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TLab::DefaultPlugins = "SceneEditor";

//==============================================================================
// Make sure all GUIs are fine once the editor is launched
function GuiFrameSetCtrl::togglePanel(%this,%panelIntName) {
	%panel = %this.findObjectByInternalName(%panelIntName);

	if (!isObject(%panel)) {
		devlog("Trying to toggle a FrameSet panel which don't exist:",%panelIntName);
		return;
	}

	if (%panel.visible) {
		//Hide the panel
		%index = %this.getObjectIndex(%panel);
		%rows = %this.rows;
		%panel.myIndex = %index;
		%panel.myRow = %rows;
		%this.lastRows[getWordCount(%rows)] = %rows;
		%newRows = removeWord(%rows,getWordCount(%rows)-1);
		hide(%panel);
		%this.pushToBack(%panel);
		%this.rows = %newRows;
		devLog("Hiding:",%panelIntName,"Rows was",%rows,"New Rows = ",%newRows);
	} else {
		%index = %panel.myIndex;
		%newRows = %this.lastRows[getWordCount(%this.rows)+1];
		devLog("Showing:",%panelIntName,"Rows was",%this.rows,"New Rows = ",%newRows);
		%this.rows = %newRows;
		show(%panel);
		%this.reorderChild(%panel,%this.getObject(%index));
	}

	%this.updateSizes();
}
//------------------------------------------------------------------------------
