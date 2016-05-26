//==============================================================================
// Boost! -> GuiControl Functions Helpers
// Copyright NordikLab Studio, 2013
//==============================================================================
//==============================================================================
// Schedule global on-off - Used to limit output of fast logs
//==============================================================================

function ForestEd::initQuickGenerator( %this ) {
	FEP_ToolsQuickGenerator-->groupName.setText("");
	FEP_ToolsQuickGenerator-->sourceFolder.setText("");
	FEP_ToolsQuickGenerator-->prefix.setText("");
}
//==============================================================================
function ForestEd::generateForestData( %this, %quick ) {
	%baseFolder = FEP_ToolsQuickGenerator-->sourceFolder.getText();
	%name = FEP_ToolsQuickGenerator-->groupName.getText();
	%prefix = FEP_ToolsQuickGenerator-->prefix.getText();
	if (%baseFolder $= "" || %name $= "" || %prefix $= "" )
	   return;

	if(%quick)
		%prefix = "Qck";

	%settingContainer =FEP_ToolsSettingsGenerator;
	buildForestDataFromFolder(%baseFolder,%name,%prefix,%settingContainer,$FEP_GeneratorDeleteExistBrush,$FEP_GeneratorLevelItemsMode);
	FEP_Manager.updateBrushData();
	FEP_Manager.setDirty(true);
}
//------------------------------------------------------------------------------
