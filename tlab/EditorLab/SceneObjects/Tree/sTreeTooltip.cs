//==============================================================================
// TorqueLab -> Editor Gui General Scripts
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//------------------------------------------------------------------------------
//==============================================================================
// Tooltip for TSStatic
function SceneObjectsTree::GetTooltipTSStatic( %this, %obj ) {
	return "Shape: " @ %obj.shapeName;
}
//------------------------------------------------------------------------------
//==============================================================================
// Tooltip for ShapeBase
function SceneObjectsTree::GetTooltipShapeBase( %this, %obj ) {
	return "Datablock: " @ %obj.dataBlock;
}
//------------------------------------------------------------------------------
//==============================================================================
// Tooltip for StaticShape
function SceneObjectsTree::GetTooltipStaticShape( %this, %obj ) {
	return "Datablock: " @ %obj.dataBlock;
}
//------------------------------------------------------------------------------
//==============================================================================
// Tooltip for Item
function SceneObjectsTree::GetTooltipItem( %this, %obj ) {
	return "Datablock: " @ %obj.dataBlock;
}
//------------------------------------------------------------------------------
//==============================================================================
// Tooltip for RigidShape
function SceneObjectsTree::GetTooltipRigidShape( %this, %obj ) {
	return "Datablock: " @ %obj.dataBlock;
}
//------------------------------------------------------------------------------
//==============================================================================
// Tooltip for Prefab
function SceneObjectsTree::GetTooltipPrefab( %this, %obj ) {
	return "File: " @ %obj.filename;
}
//------------------------------------------------------------------------------
//==============================================================================
// Tooltip for GroundCover
function SceneObjectsTree::GetTooltipGroundCover( %this, %obj ) {
	%text = "Material: " @ %obj.material;

	for(%i=0; %i<8; %i++) {
		if(%obj.probability[%i] > 0 && %obj.shapeFilename[%i] !$= "") {
			%text = %text NL "Shape " @ %i @ ": " @ %obj.shapeFilename[%i];
		}
	}

	return %text;
}
//------------------------------------------------------------------------------
//==============================================================================
// Tooltip for SFXEmitter
function SceneObjectsTree::GetTooltipSFXEmitter( %this, %obj ) {
	if(%obj.fileName $= "")
		return "Track: " @ %obj.track;
	else
		return "File: " @ %obj.fileName;
}
//------------------------------------------------------------------------------
//==============================================================================
// Tooltip for ParticleEmitterNode
function SceneObjectsTree::GetTooltipParticleEmitterNode( %this, %obj ) {
	%text = "Datablock: " @ %obj.dataBlock;
	%text = %text NL "Emitter: " @ %obj.emitter;
	return %text;
}
//------------------------------------------------------------------------------
//==============================================================================
// Tooltip for WorldEditorSelection
function SceneObjectsTree::GetTooltipWorldEditorSelection( %this, %obj ) {
	%text = "Objects: " @ %obj.getCount();

	if( !%obj.getCanSave() )
		%text = %text NL "Persistent: No";
	else
		%text = %text NL "Persistent: Yes";

	return %text;
}
//------------------------------------------------------------------------------
