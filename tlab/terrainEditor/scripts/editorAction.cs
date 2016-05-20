//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function TerrainEditor::getActionDescription( %this, %action )
{
   %isPaintMode = false;
    switch$( %action )
    {
    case "brushAdjustHeight":
         %msg = "Adjust terrain height up or down.";

    case "adjustHeight":
         %msg = "Adjust terrain height up or down.";

    case "clear":
         %msg = "Add clear terrain collision.";

    case "clearEmpty":
         %msg = "Add back terrain collision.";

    case "raiseHeight":
         %msg = "Raise terrain height.";

    case "lowerHeight":
         %msg = "Lower terrain height.";

    case "smoothHeight":
         %msg = "Smooth terrain.";
    case "smoothSlope":
         %msg = "Smooth slope terrain.";

    case "paintNoise":
         %msg = "Modify terrain height using noise.";

    case "flattenHeight":
         %msg = "Flatten terrain.";

    case "setHeight":
         %msg = "Set terrain height to defined value.";

    case "setEmpty":
         %msg = "Remove terrain collision.";

    case "outlineSelect":
         %msg = "Remove terrain collision.";

    case "paintMaterial":
         %isPaintMode = true;
         %msg = "Remove terrain collision.";
    case "scaleHeight":
         %msg = "scaleHeight terrain collision.";
    case "select":
         %msg = "Select avtion.";


    default:
        %msg = "";
    }
   TEPainter.setPaintMode(%isPaintMode);
 return %msg;   
}

/// This is only ment for terrain editing actions and not
/// processed actions or the terrain material painting action.
function TerrainEditor::switchAction( %this, %action )
{
    %actionDesc = %this.getActionDescription(%action);
    %this.currentMode = "paint";
    %this.selectionHidden = true;
    %this.currentAction = %action;
    %this.currentActionDesc = %actionDesc;
    %this.savedAction = %action;
    %this.savedActionDesc = %actionDesc;

    if (  %action $= "setEmpty" ||
                     %action $= "clearEmpty" ||
                                %action $= "setHeight" )
        %this.renderSolidBrush = true;
    else
        %this.renderSolidBrush = false;

    EditorGuiStatusBar.setInfo(%actionDesc);
    %this.setAction( %this.currentAction );
}

function TerrainEditor::onSmoothHeightmap( %this )
{
    if ( !%this.getActiveTerrain() )
        return;

    // Show the dialog first and let the user
    // set the smoothing parameters.
    // Now create the terrain smoothing action to
    // get the work done and perform later undos.
    %action = new TerrainSmoothAction();
    %action.smooth( %this.getActiveTerrain(), 1.0, 1 );
    %action.addToManager( Editor.getUndoManager() );
}

function TerrainEditor::onMaterialUndo( %this )
{
    // Update the gui to reflect the current materials.
    EPainter.updateLayers();
}


function TESettingsApplyButton::onAction(%this)
{
    ETerrainEditor.softSelectFilter = TESoftSelectFilter.getValue();
    ETerrainEditor.resetSelWeights(true);
    ETerrainEditor.processAction("softSelect");
}