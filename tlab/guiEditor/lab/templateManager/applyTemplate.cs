//==============================================================================
// TorqueLab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function TplManager::applyTemplateOnControl(%this,%tplGui,%ctrl) {
//	%this.testCtrl(%tplGui);
	%this.applyTemplate(%tplGui,%ctrl);
	return;
	//-----------------------------------------------------
	// Root template setup
	%cId_1 = 0;

	foreach(%ctrl_1 in %tplGui) {
		//-----------------------------------------------------
		// Apply Child Level 1
		if (%cId_1 >= %ctrl.getCount()) {
			devLog("There's no Target Ctrl for Src Child:",%cId_1);
			break;
		}

		%tgtCtrl = %ctrl.getObject(%cId_1);
		devLog("Applying template on Level 1 Child:",%ctrl_1,"Target is:",%tgtCtrl);
		%cId_1++;
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function TplManager::applyTemplate(%this,%sourceCtrl,%tgtCtrl,%level) {
	if (%level $= "")
		%level = 1;

	%childId = 0;

	foreach(%child in %sourceCtrl) {
		if (%childId >= %tgtCtrl.getCount()) {
			devLog("There's no Target Ctrl for Src Child:",%childId);
			return;
		}

		%tgtChild = %tgtCtrl.getObject(%childId);
		devLog("Sharing fields",%child.shareFields);
		%this.applySharedFields(%child,%tgtChild);
		devLog("Child level:",%level,"Found:",%child.getClassName(),"Targtet",%tgtChild.getClassName());
		%this.applyTemplate(%child,%tgtChild,%level+1);
		%childId++;
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function TplManager::applySharedFields(%this,%source,%target) {
	%fields = %source.shareFields;

	foreach$(%field in %fields) {
		%value = %source.getFieldValue(%field);
		%tgtValue = %target.getFieldValue(%field);

		if (%tgtValue $= "")
			%tgtValue = "[EMPTY]";

		devLog(%target,"Applying field:",%field,"Was",%tgtValue,"Setted to:",%value);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function TplManager::testCtrl(%this,%tplGui,%level) {
	if (%level $= "")
		%level = 1;

	foreach(%ctrl in %tplGui) {
		devLog("Child level:",%level,"Found:",%ctrl);
		%this.testCtrl(%ctrl,%level+1);
	}
}
//------------------------------------------------------------------------------