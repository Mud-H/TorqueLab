//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function SceneEd::doClone( %this ) {
	%copyCount = SETools_CloneCount.getValue();

	if (%copyCount > 100)%copyCount = 100;

	if (%copyCount <= 0) return;

	EWorldEditor.copySelection(EWorldEditor.lastMoveOffset,%copyCount);
	//ETools.hideTool(CloneDrag);
	//EditorMap.push();
	//%this.setVisible(false);
}
//------------------------------------------------------------------------------