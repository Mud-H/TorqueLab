//==============================================================================
// TorqueLab -> AlterVerse Tools adaptation script
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$Cfg_TerrainEditor_painter_AutoPaintPanel = 1;
//==============================================================================
function TEPainter::setPaintMode(%this,%enabled) {
   if (%enabled)
   {
    %this.activatePaintMode();  
   return;
   }
   %this.deactivatePaintMode();  
   
}
//------------------------------------------------------------------------------


//==============================================================================
function TEPainter::activatePaintMode(%this,%enabled) {
   if (ETerrainEditor.currentAction $= paintMaterial )
   {
         //warnLog("Already in Paint mode!");
         return;
   }   
      
   ETerrainEditor.previousAction = ETerrainEditor.currentAction;
  ETerrainEditor.currentMode = "paint";
	ETerrainEditor.selectionHidden = true;
	ETerrainEditor.currentAction = paintMaterial;
	ETerrainEditor.currentActionDesc = "Paint material on terrain";
	ETerrainEditor.setAction( ETerrainEditor.currentAction );
	EditorGuiStatusBar.setInfo(ETerrainEditor.currentActionDesc);
	ETerrainEditor.renderVertexSelection = true;
	
	if (!$Cfg_TerrainEditor_painter_AutoPaintPanel)
	   return;
	TEPainter.toolsInitialState = false;
   if (Lab.isShownPluginTools())
      TEPainter.toolsInitialState = true;
	Lab.showPluginTools();
}
//------------------------------------------------------------------------------

//==============================================================================
function TEPainter::deactivatePaintMode(%this) {
   if (ETerrainEditor.currentAction !$= paintMaterial )
   {
       if (!$Cfg_TerrainEditor_painter_AutoPaintPanel)
	   return;
       if ( !TEPainter.toolsInitialState )
          Lab.hidePluginTools();
         //warnLog("Already NOT in Paint mode!");
         return;
   }  
   ETerrainEditor.currentAction = ETerrainEditor.previousAction;
	ETerrainEditor.currentActionDesc = "Depend of what you were using";
	ETerrainEditor.setAction( ETerrainEditor.currentAction );
	EditorGuiStatusBar.setInfo(ETerrainEditor.currentActionDesc);
	ETerrainEditor.renderVertexSelection = true;
   if (!$Cfg_TerrainEditor_painter_AutoPaintPanel)
	   return;
   if ( !TEPainter.toolsInitialState )
      Lab.hidePluginTools();
      
	
}
//------------------------------------------------------------------------------


//==============================================================================
function TEPainter::setPaintToolsActive(%this,%active) {  
   Lab.hidePluginTools();

}
//------------------------------------------------------------------------------