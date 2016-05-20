//==============================================================================
// TorqueLab -> WorldEditor Grid and Snapping System
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Force selected object to fit exactly on the grid
//==============================================================================

//==============================================================================
//Force all selected object to the grid
function WorldEditor::forceToGrid( %this, %obj ) {
	for(%i = 0; %i < %this.getSelectionSize(); %i++) {
		%obj = %this.getSelectedObject(%i);
		
		%this.forceObjectToGrid(%obj);
	}
	if (%this.stickToGround){
		%this.dropType = "toTerrain";
		%this.dropSelection();
		%this.dropType = Scene.dropMode;		
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Force single object to the grid
function WorldEditor::forceObjectToGrid( %this, %obj ) {
	%transform = %obj.getTransform();
	%gridSize = %this.gridSize;
	%pos = getWords(%transform,0,2);
	%start = %pos;
	%multi = %gridSize;

	if (%multi > 1)
		%multi = 1;

	%pos.x = mRound(%pos.x/%multi)*%multi;
	%pos.y = mRound(%pos.y/%multi)*%multi;
	%pos.z = mRound(%pos.z/%multi)*%multi;
	%transform = setWord(%transform, 0, %pos.x);
	%transform = setWord(%transform, 1, %pos.y);

	if (!$Cfg_Common_Grid_forceToGridNoZ)
		%transform = setWord(%transform, 2, %pos.z);

	%obj.setTransform(%transform);
	
	Scene.doRefreshInspect();	

}
//------------------------------------------------------------------------------

//==============================================================================
// World Editor Grid Functions
//==============================================================================
//==============================================================================
//Lab.setGridSize($ThisControl.getValue());
function Lab::setGridSize( %this, %value,%isSnapping,%gizmoZ ) {
	if (!strIsNumeric(%value))
		return;

	EWorldEditor.gridSize = %value;
	%isSnapping = true; //For now, gridSnap and GridSize are always the same

	if (%isSnapping)
		%this.setGizmoGridSize(%value,%gizmoZ,true);

	Lab.syncGuiGridSnap();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::incGridSize(%this,%value) {
	%step = $WEditor::GridStep * %value;
	%current = EWorldEditor.gridSize;
	%newsize = %current + %step;
	Lab.setGridSize(%newsize,true);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::setGizmoGridSize( %this, %gridSize,%zSnap,%noGuiSync ) {
	%size = getWord(%gridSize,0);
	%gridSizeXYZ = %size SPC %size SPC %size;

	if (%zSnap !$= "" && strIsNumeric(%zSnap))
		%gridSizeXYZ.z = %zSnap;

	GlobalGizmoProfile.gridSize = %gridSizeXYZ;

	if (!%noGuiSync)
		Lab.syncGuiGridSnap();
}
//------------------------------------------------------------------------------
//==============================================================================
//Lab.setGridSize($ThisControl.getValue());
function Lab::toggleGridSnap( %this ) {
	%gridSnap = !EWorldEditor.gridSnap;
	devLog("toggle grid snap from:",EWorldEditor.gridSnap,"To",%gridSnap);
	Lab.setGridSnap(!EWorldEditor.gridSnap);
}
//------------------------------------------------------------------------------
//==============================================================================
//Lab.setGridSize($ThisControl.getValue());
function Lab::setGridSnap( %this, %gridSnapOn ) {
	//If nothing submitted, WorldEditor gridSnap is already set, just need to sync
	if (%gridSnapOn !$= "")
		EWorldEditor.gridSnap = %gridSnapOn;
	else
		%gridSnapOn = EWorldEditor.gridSnap;

	Lab.setGizmoGridSnap(%gridSnapOn);
	Lab.syncGuiGridSnap();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setGizmoGridSnap( %this, %gridSnapOn ) {
	GlobalGizmoProfile.snapToGrid = %gridSnapOn;
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync Grid and Snapping GUI related controls
//==============================================================================
//==============================================================================
//Lab.setGridSize($ThisControl.getValue());
function Lab::syncGuiGridSnap( %this ) {
	if (isObject(SceneEditorToolbar))
		return;
	SceneEditorToolbar-->objectSnapBtn.setStateOn( EWorldEditor.getSoftSnap() );
	//SceneEditorToolbar-->softSnapSizeTextEdit.setText( EWorldEditor.getSoftSnapSize() );
	SceneEditorToolbar-->WorldEditorGridSizeEdit.setText( EWorldEditor.gridSize );
	SceneEditorToolbar-->WorldEditorGridSizeEdit.setText( EWorldEditor.gridSize );
	SceneEditorToolbar-->objectGridSnapBtn.setStateOn( EWorldEditor.gridSnap );
	if (isObject(ESnapOptions))
		return;
	ESnapOptions-->SnapSize.setText( EWorldEditor.getSoftSnapSize() );
	ESnapOptions-->GridSize.setText(  EWorldEditor.gridSize  );
	ESnapOptions-->GridSnapButton.setStateOn( EWorldEditor.gridSnap );
	
	ESnapOptions-->NoSnapButton.setStateOn( !EWorldEditor.stickToGround && !EWorldEditor.getSoftSnap() && !EWorldEditor.gridSnap );
	
	//devLog("Ended with snap:",EWorldEditor.gridSnap);
}
//------------------------------------------------------------------------------
//==============================================================================
// World Editor Grid Functions
//==============================================================================

//==============================================================================

function EWorldEditor::getGridSnap( %this ) {
	return %this.gridSnap;
}
//------------------------------------------------------------------------------

//==============================================================================
function EWorldEditor::getGridSize( %this ) {
	return %this.gridSize;
}
//------------------------------------------------------------------------------

//==============================================================================
function toggleSnappingOptions( %var ) {
	if( SceneEditorToolbar->objectSnapDownBtn.getValue() && SceneEditorToolbar->objectSnapBtn.getValue() ) {
		if( %var $= "terrain" ) {
			EWorldEditor.stickToGround = 1;
			EWorldEditor.setSoftSnap(false);
			ESnapOptions_Book.selectPage(0);
			SceneEditorToolbar-->objectSnapBtn.setStateOn(0);
		} else {
			// soft snapping
			EWorldEditor.stickToGround = 0;
			EWorldEditor.setSoftSnap(true);
			ESnapOptions_Book.selectPage(1);
			SceneEditorToolbar-->objectSnapDownBtn.setStateOn(0);
		}
	} else if( %var $= "terrain" && EWorldEditor.stickToGround == 0 ) {
		// Terrain Snapping
		EWorldEditor.stickToGround = 1;
		EWorldEditor.setSoftSnap(false);
		ESnapOptions_Book.selectPage(0);
		SceneEditorToolbar-->objectSnapDownBtn.setStateOn(1);
		SceneEditorToolbar-->objectSnapBtn.setStateOn(0);
	} else if( %var $= "soft" && EWorldEditor.getSoftSnap() == false ) {
		// Object Snapping
		EWorldEditor.stickToGround = 0;
		EWorldEditor.setSoftSnap(true);
		ESnapOptions_Book.selectPage(1);
		SceneEditorToolbar-->objectSnapBtn.setStateOn(1);
		SceneEditorToolbar-->objectSnapDownBtn.setStateOn(0);
	} else if( %var $= "grid" ) {
		Lab.setGridSnap( !EWorldEditor.getGridSnap() );
	} else {
		// No snapping.
		EWorldEditor.stickToGround = false;
		Lab.setGridSnap( false );
		EWorldEditor.setSoftSnap( false );
		SceneEditorToolbar->objectSnapDownBtn.setStateOn(0);
		SceneEditorToolbar->objectSnapBtn.setStateOn(0);
	}

	EWorldEditor.syncGui();
}
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
/*

DefineEngineMethod( WorldEditor, getSoftSnap, bool, (), ,
	"Is soft snapping always on?"
	"@return True if soft snap is on, false if not.")
{
	return object->mSoftSnap;
}

DefineEngineMethod( WorldEditor, setSoftSnap, void, (bool softSnap), ,
	"Allow soft snapping all of the time."
	"@param softSnap True to turn soft snap on, false to turn it off.")
{
	object->mSoftSnap = softSnap;
}

DefineEngineMethod( WorldEditor, getSoftSnapSize, F32, (), ,
	"Get the absolute size to trigger a soft snap."
	"@return absolute size to trigger a soft snap.")
{
	return object->mSoftSnapSize;
}

DefineEngineMethod( WorldEditor, setSoftSnapSize, void, (F32 size), ,
	"Set the absolute size to trigger a soft snap."
	"@param size Absolute size to trigger a soft snap.")
{
	object->mSoftSnapSize = size;
}

DefineEngineMethod( WorldEditor, getSoftSnapAlignment, WorldEditor::AlignmentType, (),,
	"Get the soft snap alignment."
	"@return soft snap alignment.")
{
   return object->mSoftSnapAlignment;
}

DefineEngineMethod( WorldEditor, setSoftSnapAlignment, void, ( WorldEditor::AlignmentType type ),,
	"Set the soft snap alignment."
	"@param type Soft snap alignment type.")
{
   object->mSoftSnapAlignment = type;
}

DefineEngineMethod( WorldEditor, softSnapSizeByBounds, void, (bool useBounds), ,
	"Use selection bounds size as soft snap bounds."
	"@param useBounds True to use selection bounds size as soft snap bounds, false to not.")
{
	object->mSoftSnapSizeByBounds = useBounds;
}

DefineEngineMethod( WorldEditor, getSoftSnapBackfaceTolerance, F32, (),,
	"Get the fraction of the soft snap radius that backfaces may be included."
	"@return fraction of the soft snap radius that backfaces may be included.")
{
	return object->mSoftSnapBackfaceTolerance;
}

DefineEngineMethod( WorldEditor, setSoftSnapBackfaceTolerance, void, (F32 tolerance),,
	"Set the fraction of the soft snap radius that backfaces may be included."
	"@param tolerance Fraction of the soft snap radius that backfaces may be included (range of 0..1).")
{
	object->mSoftSnapBackfaceTolerance = tolerance;
}

DefineEngineMethod( WorldEditor, softSnapRender, void, (F32 render),,
	"Render the soft snapping bounds."
	"@param render True to render the soft snapping bounds, false to not.")
{
	object->mSoftSnapRender = render;
}

DefineEngineMethod( WorldEditor, softSnapRenderTriangle, void, (F32 renderTriangle),,
	"Render the soft snapped triangle."
	"@param renderTriangle True to render the soft snapped triangle, false to not.")
{
	object->mSoftSnapRenderTriangle = renderTriangle;
}

DefineEngineMethod( WorldEditor, softSnapDebugRender, void, (F32 debugRender),,
	"Toggle soft snapping debug rendering."
	"@param debugRender True to turn on soft snapping debug rendering, false to turn it off.")
{
	object->mSoftSnapDebugRender = debugRender;
}
*/