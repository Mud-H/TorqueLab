//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$FEP_GeneratorDeleteExistBrush = true;
$FEP_GeneratorLevelItemsMode = false;
$FEP_RegenerateExistingData = false;
//==============================================================================
function FEP_Manager::initDataGenerator( %this ) {
	if (FEP_ForestDataGenerator-->sourceFolder.getText() $= "")
		FEP_ForestDataGenerator-->sourceFolder.setText("[Select model folder to generate from]");

	FEP_ForestDataGenerator-->groupName.setText("[Brush group name]");
	FEP_ForestDataGenerator-->prefix.setText("[Data prefix]");
	%settingContainer = FEP_ForestDataGenerator-->settings;
	%settingContainer-->scaleMin.setText("1");
	%settingContainer-->scaleMax.setText("1");
	%settingContainer-->scaleExponent.setText("1");
	%settingContainer-->sinkMin.setText("0");
	%settingContainer-->sinkMax.setText("0");
	%settingContainer-->sinkRadius.setText("0");
	%settingContainer-->slopeMin.setText("0");
	%settingContainer-->slopeMax.setText("90");
	%settingContainer-->elevationMin.setText("-1000");
	%settingContainer-->elevationMax.setText("1000");
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::dataGenFolder( %this, %path ) {
	%path = makeRelativePath(%path );
	logd("Generate forest data from folder:",%path);

	foreach$(%cont in "FEP_ToolsQuickGenerator FEP_ForestDataGenerator") {
		%cont.findObjectByInternalName(%field,true);
		%cont-->sourceFolder.setText(%path);
	}

	//FEP_ForestDataGenerator-->sourceFolder.setText(%path);
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::generateForestData( %this, %quick ) {
	%baseFolder = FEP_ForestDataGenerator-->sourceFolder.getText();
	%name = FEP_ForestDataGenerator-->groupName.getText();
	%prefix = FEP_ForestDataGenerator-->prefix.getText();

	if(%quick)
		%prefix = "Qck";

	%settingContainer = FEP_ForestDataGenerator-->settings;
	buildForestDataFromFolder(%baseFolder,%name,%prefix,%settingContainer,$FEP_GeneratorDeleteExistBrush,$FEP_GeneratorLevelItemsMode);
	%this.updateBrushData();
	%this.setDirty(true);
}
//------------------------------------------------------------------------------

//==============================================================================
function FEP_Manager::deleteAllBrushes( %this ) {
	ForestBrushGroup.deleteAllObjects();
}
//------------------------------------------------------------------------------

//==============================================================================
function FEP_Manager::removeInvalidBrushes( %this ) {
	warnLog("Sooooon");
}
//------------------------------------------------------------------------------

//==============================================================================
function FEP_Manager::removeInvalidItems( %this ) {
	warnLog("Sooooon");
}
//------------------------------------------------------------------------------


//==============================================================================
function FEP_Manager::doOpenDialog( %this, %filter, %callback ) {
	%currentFolder = FEP_ForestDataGenerator-->sourceFolder.getText()@"/";
	%currentFolder = strreplace(%currentFolder,"//","/");
	if (!isDirectory(%currentFolder))
		%currentFolder = "art/shapes/";
	%dlg = new OpenFolderDialog() {
		Title = "Select Export Folder";
		Filters = %filter;
		DefaultFile = %currentFolder;
		ChangePath = false;
		MustExist = true;
		MultipleFiles = false;
	};

	if(%dlg.Execute())
		FEP_Manager.dataGenFolder(%dlg.FileName);

	%dlg.delete();
}
//------------------------------------------------------------------------------

//==============================================================================
function FEP_GeneratorEdit::onValidate( %this ) {
	%field = %this.internalName;

	foreach$(%cont in "FEP_ToolsQuickGenerator FEP_ForestDataGenerator") {
		%ctrl = %cont.findObjectByInternalName(%field,true);

		if (!isObject(%ctrl))
			continue;

		%ctrl.setText(%this.getText());
	}
}
//------------------------------------------------------------------------------
