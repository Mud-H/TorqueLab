//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function FEP_Manager::onWake( %this ) {
	%this-->TabBook.selectPage(0);
	%this.loadData();
	%this-->saveChangesButton.active = ForestDataManager.dirty;
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::onShow( %this ) {
	%contentId = %this.contentId;
	if (%contentId $= "")
		%contentId = 0;
	%this-->TabBook.selectPage(%contentId);
	%this.loadData();
	%this-->saveChangesButton.active = ForestDataManager.dirty;
}
//------------------------------------------------------------------------------


//==============================================================================
function FEP_Manager::init( %this ) {
	FEP_Manager.initDataGenerator();
	FEP_Manager.buildBrushParams();
	FEP_Manager.initBrushData(true);
	%this-->brush_filters.setText("Filters...");
	%this.filters["brush"] = "";
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::loadData( %this ) {
	ForestEditBrushTree.initTree();
	ForestEditMeshTree.initTree();
	%this.dataTree["brush"] = ForestManagerBrushTree;
	%this.dataTree["item"] = ForestManagerItemTree;
}
//------------------------------------------------------------------------------

//==============================================================================
function FEP_Manager::saveData( %this ) {
	ForestDataManager.saveDirty();
	ForestDataManager.dirty = false;
	%this-->saveChangesButton.active = false;
}
//------------------------------------------------------------------------------

//==============================================================================
function FEP_Manager::setDirty( %this,%isDirty ) {
	FEP_Manager.dirty = %isDirty;
	ForestDataManager.dirty = %isDirty;
	%this-->saveChangesButton.active = %isDirty;
}
//------------------------------------------------------------------------------

//==============================================================================
function FEP_TreeFilters::onValidate( %this ) {
	%typeData = strreplace(%this.internalName,"_"," ");
	%type = getWord(%typeData,0);
	FEP_Manager.filters[%type] = %this.getText();

	if (strFind(%this.filters[%type],"Filters..."))
		FEP_Manager.filters[%type] = "";

	FEP_Manager.dataTree[%type].setFilterText(FEP_Manager.filters[%type]);
}
//------------------------------------------------------------------------------

