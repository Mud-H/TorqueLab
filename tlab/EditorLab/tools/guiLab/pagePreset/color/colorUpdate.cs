//==============================================================================
// Lab GuiManager -> Profile Color Management System
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Update the Colors preset preview from current presets
//==============================================================================
//==============================================================================
// Update all the color sets
function GLab::updateColors( %this ) {
	//First reset the color list for each color sets type
	foreach$(%colorGroup in $GLab_ColorGroups)
		$GLab_ColorGroupSetList[%colorGroup] = "None";

	//Check each object in Color group and update the associated colorPickerCtrl
	//Colors are store in 2 ways: SimGroup and ScriptObject
	if (!isObject(GuiColor_Group))
		return;

	foreach(%obj in GuiColor_Group) {
		if (%obj.getClassName() $= "SimGroup")
			%this.updateColorFontGroup(%obj);
		else if (%obj.getClassName() $= "ScriptObject")
			%this.updateColorFillObj(%obj);
	}
}
//------------------------------------------------------------------------------
$GLab_FontRole[0] = "Normal";
$GLab_FontRole[1] = "Hover";
$GLab_FontRole[2] = "N/A";
$GLab_FontRole[3] = "Sel.";
//==============================================================================
// Update a color preset using SimGroup
function GLab::updateColorFontGroup( %this,%group ) {
	%colorGroup = "colorFont";

	foreach(%obj in %group) {
		%colorSet = %obj.internalName;
		//Make sure the color is in the color sets list
		$GLab_ColorGroupSetList[%colorGroup] = strAddWord($GLab_ColorGroupSetList[%colorGroup],%colorSet,true);
		%colorsGroup = GLab_ColorSetStack.findObjectByInternalName(%colorSet,true);
		%groupHeader = GLab_ColorSetStack.findObjectByInternalName(%colorSet@"Text",true);
		%groupHeader.text = %colorSet SPC "Colors:" @ "\c0 c0\c1 c1\c2 c2\c3 c3\c4 c4\c5 c5\c6 c6\c7 c7\c8 c8\c9 c9";
		%count = %obj.getDynamicFieldCount();

		for(%i=0 ; %i<%count; %i++) {
			%fieldFull = %obj.getDynamicField(%i);
			%field = getWord(%fieldFull,0);
			%color = removeWord(%fieldFull,0);
			%len = %field;
			%colorId = strLastChars(%field,1);
			%textCtrl = %colorsGroup.findObjectByInternalName("c"@%colorId,true);

			if ($GLab_FontRole[%colorId] !$= "")
				%taggedText = $GLab_FontRole[%colorId];
			else
				%taggedText = "Font["@%colorId@"]";

			//%taggedText = '\c'@%colorId@'Me';
			//eval("%taggedText = \\\"c"@%colorId@%text@";");
			if (!isObject(%textCtrl)) {
				warnLog("Can't find the text ctrl for color:",%field,"ID:",%colorId,"GroupID:",%colorsGroup.getId());
			} else
				%textCtrl.setText(%taggedText);

			%colorPick = %colorsGroup.findObjectByInternalName(%field,true);
			%this.updateColorPickerCtrl(%colorPick,%field,%color,%obj);
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Update a color preset using ScriptObject
function GLab::updateColorFillObj( %this,%obj,%colorGroup ) {
	%colorGroup = "colorFill";
	%count = %obj.getDynamicFieldCount();

	for(%i=0 ; %i<%count; %i++) {
		%fieldFull = %obj.getDynamicField(%i);
		%field = getWord(%fieldFull,0);
		%color = removeWord(%fieldFull,0);
		//Make sure the color is in the color sets list
		$GLab_ColorGroupSetList[%colorGroup] = strAddWord($GLab_ColorGroupSetList[%colorGroup],%field,true);
		%fieldlen = strlen(%field);
		%fillType = getSubStr(%field,0,%fieldlen-1);
		%fillId = getSubStr(%field,%fieldlen-1);
		%typeStack  = GLab_ColorSetStack.findObjectByInternalName(%fillType,true);
		%idCtrl = %typeStack.findObjectByInternalName(%fillId,true);
		%colorPick = %idCtrl->picker;
		%this.updateColorPickerCtrl(%colorPick,%field,%color,%obj);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Update a color preset using ScriptObject
function GLab::updateColorPickerCtrl( %this,%colorPick,%field,%color,%srcObj ) {
	%colorPick.canSaveDynamicFields = "1";
	%colorPick.baseColor = ColorIntToFloat(%color);
	%colorPick.pickColor = ColorIntToFloat(%color);
	%colorPick.superClass = "GuiColorDefaultPicker";
	%colorPick.sourceObject = %srcObj;
	%colorPick.sourceField = %field;
	%colorPick.command = "$ThisControl.pickColorI();";
}
//------------------------------------------------------------------------------
