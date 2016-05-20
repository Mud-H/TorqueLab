//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Clear and Refresh Material
function MaterialEditorTools::onPreEditorSave(%this) {
	MaterialEditorTools.currentLayer = "0";
   MaterialEditorTools.currentMaterial = "noshape_NoShape";
}
//------------------------------------------------------------------------------
//==============================================================================
// Clear and Refresh Material
function MaterialEditorTools::onPostEditorSave(%this) {
	
}
//------------------------------------------------------------------------------
