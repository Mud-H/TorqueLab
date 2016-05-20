//==============================================================================
// TorqueLab -> Terrain Paint Generator System
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Terrain Paint Generator Globals Define
//------------------------------------------------------------------------------

$TPG_ValidateFields = "heightMin heightMax slopeMin slopeMax coverage";
//------------------------------------------------------------------------------

//==============================================================================
// Initialize the Terrain Paint Generator
function TPG::init(%this) {
	if ( !isObject( TPG_LayerGroup ) ) {
		new SimGroup( TPG_LayerGroup  );
	}

	TPG_Window-->saveGroupButton.active = false;
	TPG_StoredValues-->v1.setText("");
	TPG_StoredValues-->v2.setText("");
	TPG_StoredValues-->v3.setText("");
	TPG_StoredValues-->v4.setText("");
	TPG_StackLayers.clear();
	$TerrainPaintGeneratorGui_Initialized = true;
	TPG_StepModeCheckbox.setStateOn(false);
}
//------------------------------------------------------------------------------
//==============================================================================
// Initialize the Terrain Paint Generator
function TPG::exec(%this) {
	exec("tlab/terrainEditor/gui/TerrainPaintGeneratorGui.cs");
	exec("tlab/terrainEditor/scripts/paintGenerator.cs");
}
//------------------------------------------------------------------------------
//==============================================================================
// Make sure the stack and group have valid object and are synced
function TPG::checkLayersStackGroup(%this) {
	foreach(%pill in TPG_StackLayers) {
		if(isObject(%pill.layerObj))
			%addList = strAddWord(%addList,%pill.layerObj);
		else
			%badPill = strAddWord(%badPill,%pill);
	}

	foreach(%layer in TPG_LayerGroup) {
		%inStack = strFind(%addList,%layer.getId());

		if (!strFind(%addList,%layer.getId()))
			%deleteLayers = strAddWord(%deleteLayers,%layer);
	}

	foreach$(%pill in %badPill)
		%pill.delete();

	foreach$(%layer in %deleteLayers)
		%layer.delete();
}
//------------------------------------------------------------------------------

