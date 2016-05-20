//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function LabParamsDlg::onWake( %this ) {
	if (!$LabParamsTreeBuilt)
		Lab.buildSettingTree(true);
	
	foreach(%gui in LP_SettingsContainer)
		hide(%gui);	

	hide(%this-->ParamStyles);
	LabParamsTree.expandAllGroups(true);
	//Get the predefined config files and add to menu
	LabCfg.getAllConfigs();
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::buildSettingTree( %this,%keepExisting ) {
	foreach(%paramArray in LabParamsGroup) {
		%name = %paramArray.internalName;
		%group = %paramArray.group;
		%containerName = "lpPage_"@%group@"_"@%name;
		
		if (!%keepExisting)
			delObj(%containerName);
		//%paramArray.groupLink = %group@"_"@%name; //TMP
		if (!isObject(%containerName)) {
			
			%newContainer = cloneObject(LP_SampleContainer,%containerName,%group@"_"@%name,LP_SettingsContainer);
		}
      
		%paramArray.optContainer = %containerName;
		LabParamsTree.addParam(%paramArray);
		%paramArray.container = %containerName-->Params_Stack;
		%paramArray.style = $LabParamsStyle;
		buildParamsArray(%paramArray);
		if (isObject(%paramArray.extraStack))
		   %this.buildParamsExtra(%paramArray,%containerName);
		LabParams.syncArray(%paramArray,true);
	}
	$LabParamsTreeBuilt = true;
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::buildAllParamsExtra( %this ) {
	foreach(%paramArray in LabParamsGroup) {
	   if (isObject(%paramArray.extraStack))
		   %this.buildParamsExtra(%paramArray,%paramArray.optContainer);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::buildParamsExtra( %this,%paramArray,%container ) {
	%stackSrc = %paramArray.extraStack;
	%stackTgt = %container-->Params_Stack;
	devLog("buildParams Extra:",%stackSrc,"Add to",%container,"Stack",%stackTgt);
   foreach(%ctrl in %stackSrc)
   {
      %clone =  cloneObject(%ctrl,"","",%stackTgt);      
   }	
}
//------------------------------------------------------------------------------
//==============================================================================
function LabParamsDlg::regenerate( %this ) {
	LabParamsTree.clear();
	%this.clearSettingsContainer();
	Lab.buildSettingTree();
	LabParamsTree.buildVisibleTree();
}
//------------------------------------------------------------------------------

//==============================================================================
function LabParamsDlg::onPreEditorSave( %this ) {
	//%this.clearSettingsContainer();
}
//------------------------------------------------------------------------------

//==============================================================================
function LabParamsDlg::onPostEditorSave( %this ) {
}
//------------------------------------------------------------------------------

//==============================================================================
// Add default setting (Must set beginGroup and endGroup from caller)
function LPD_ConfigNameMenu::onSelect( %this,%id,%text ) {
	%fileText = %text;
	%filename = fileBase(%fileText)@".cfg.cs";
	%cfg = "tlab/core/settings/cfgs/"@%filename;

	if (!isFile(%cfg))
		return;

	LPD_ConfigNameEdit.setText(%text);
	LabCfg.file = %cfg;
}
//------------------------------------------------------------------------------
//==============================================================================
// Unreviewed
//==============================================================================


//==============================================================================
function LabParamsDlg::toggleSettings( %this,%text) {
	%id = LabParamsTree.findItemByName(%text);

	if (%id <= 0)
		return;

	%selected = false;

	if (%id $= LabParamsTree.getSelectedItem())
		%selected = true;

	if (!%this.isAwake()) {
		toggleDlg(LabParamsDlg);
		LabParamsTree.clearSelection();
		LabParamsTree.selectItem(%id);
	} else if (%id $= LabParamsTree.getSelectedItem()) {
		toggleDlg(LabParamsDlg);
	} else {
		LabParamsTree.clearSelection();
		LabParamsTree.selectItem(%id);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function LabParamsDlg::setSelectedSettings( %this,%treeItemObj ) {
	if (!isObject(%treeItemObj)) {
		warnLog("Invalid settings item objects selected:",%treeItemObj);
		return;
	}

	foreach(%gui in LP_SettingsContainer)
		hide(%gui);

	show(%treeItemObj.itemContainer);
}

//------------------------------------------------------------------------------
//==============================================================================
function LabParamsDlg::createSettingContainer( %this,%group,%subgroup ) {
	%containerName = "lpPage_"@%group@"_"@%subgroup;

	if (isObject(%containerName)) {
		warnLog("There's already an object using that name:",%containerName.getName(),"ObjId=",%containerName.getId());
		return;
	}

	%newContainer = cloneObject(LP_SampleContainer);
	%newContainer.setName(%containerName);
	%newContainer.internalName = %group@"_"@%subgroup;
	LP_SettingsContainer.add(%newContainer);
	return %newContainer;
}

//==============================================================================
//LabParamsDlg.clearSettingsContainer();
function LabParamsDlg::clearSettingsContainer( %this ) {
	foreach(%gui in LP_SettingsContainer) {
		if (%gui.internalName $= "core")
			continue;

		%delList = strAddWord(%delList,%gui.getId());
	}
	foreach$(%id in %delList)
		delObj(%id);

	LabParamsDlg.cleared = true;
}
//------------------------------------------------------------------------------

//==============================================================================
// Tree Builder functions
//==============================================================================


//==============================================================================
// LabParamsTree Callbacks
//==============================================================================
//==============================================================================
function LabParamsTree::expandAllGroups( %this,%buildTree ) {
	foreach$(%id in  LabParamsTree.groupList)
		LabParamsTree.expandItem(%id);

	if (%buildTree)
		%this.buildVisibleTree();
}
//------------------------------------------------------------------------------
//==============================================================================
function LabParamsTree::onSelect( %this,%itemId ) {
	%text = %this.getItemText(%itemId);
	%value = %this.getItemValue(%itemId);
	%itemObj = $LabParamsItemObj[%itemId];

	if (isObject(%itemObj)) {
		LabParamsDlg.setSelectedSettings(%itemObj);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function LabParamsTree::onMouseUp( %this,%itemId,%clicks ) {
	%itemObj = $LabParamsItemObj[%itemId];
	%text = %this.getItemText(%itemId);
	%value = %this.getItemValue(%itemId);
	return;
}
//------------------------------------------------------------------------------


//==============================================================================
function LabParamsTree::addParam( %this,%paramArray) {
	%group = %paramArray.group;
	%link = %paramArray.groupLink;
	%name = %paramArray.displayName;
	%parentId = LabParamsTree.addSettingGroup(%group);
	%itemId = %this.findChildItemByName( %parentID,%name);

	if( !%itemId ) {
		%itemId = %this.insertItem( %parentID, %name,%link );
	}

	%itemName = "soSettingItem_"@%link;
	%itemObj = newScriptObject(%itemName);
	%itemObj.groupParent = %group;
	%itemObj.groupItem = %name;
	$LabParamsItemObj[%itemId] = %itemObj;
	%itemObj.itemContainer = %paramArray.optContainer;
}
//------------------------------------------------------------------------------
//==============================================================================
function LabParamsTree::addSettingGroup( %this,%group) {
	%tree = LabParamsTree;
	%groupTitle = $LabParamsGroupName[%group];

	if (%groupTitle $="") %groupTitle = %group;

	%parentName = %tree.findItemByName( %group );
	%groupId = %tree.findItemByValue( %group );

	if( %groupId == 0 ) {
		%groupId = %tree.insertItem( 0, %groupTitle,%group );
		LabParamsTree.groupList = strAddWord(LabParamsTree.groupList,%groupId,true);
	}

	return %groupId;
}
//------------------------------------------------------------------------------