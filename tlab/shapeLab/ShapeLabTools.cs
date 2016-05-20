//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


$Cfg_Plugins_ShapeLab_UseSimplifiedSystem = true;
//==============================================================================
// SceneEditorTools Frame Set Scripts
//==============================================================================
function ShapeLabTools::sePanelVisibility( %this ) {
	devLog("ShapeLabTools::togglePanelVisibility( %this,%checkbox )",%this,%checkbox);
	%panel = %checkbox.panel;
}

function ShapeLabTools::updatePanels( %this ) {
	devLog("ShapeLabTools::updatePanels:",%this);	

	
	%extent = 	ShapeLabTools.extent.y;
	%mainPanelSize = getWord(ShapeLabTools.rows,1);
	%rows = "0";
	%this.hideCtrl(ShapeLabSubPanel);
	%currentIndex = 0;
	%this.showCtrl(ShapeLabPropWindow,%currentIndex++);



	devLog("MainPanelSize=",%mainPanelSize);

	foreach(%ctrl in ShapeLabTools) {
		if (%ctrl.internalName $= "MainPanel" || !%ctrl.visible)
			continue;

		%subPanelCount++;
		devLog("Updating sub panel:",%ctrl.getName(),"As sub panel id:",%subPanelCount);
	}

	%subExtent = %extent - %mainPanelSize;
	%subPanelSize = %subExtent/%subPanelCount;
	%subPanelPos = %mainPanelSize;

	for(%i=1; %i<=%subPanelCount; %i++) {
		%rows = strAddWord(%rows,%subPanelPos);
		%subPanelPos += %subPanelSize;
	}

	devLog("Panel Set rows set to:",%rows);
	%this.setRows(%rows,true);
}

//==============================================================================
function ShapeLabTools::onPreEditorSave( %this ) {
	ShapeLab_SeqPillStack.clear();
}
//------------------------------------------------------------------------------
//==============================================================================
function ShapeLabTools::onPostEditorSave( %this ) {
	//EPostFxManager.moveToGui(SEP_PostFXManager_Clone);
}
//------------------------------------------------------------------------------
