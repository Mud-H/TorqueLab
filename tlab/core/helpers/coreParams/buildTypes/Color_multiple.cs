//==============================================================================
// HelpersLab -> Build Params - Color Controls
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamColor( %pData )
{
	%pData.pill-->field.text = %pData.Title;
	//ColorPicker ctrl update
	%colorPicker = %pData.pill-->colorPicker;
	%colorPicker.command = %pData.Command;
	%colorPicker.altCommand = %pData.AltCommand;
	%colorPicker.internalName = %pData.InternalName;
	%command = %pData.altCommand;
	%command = strreplace(%command,"$ThisControl",%colorPicker.getId());
	%command = strreplace(%command,"syncParamArrayCtrl","syncParamArrayCtrlData");
	%colorPicker.updateCommand = %command;
	%noAlpha = %pData.Option[%pData.Setting,"noalpha"];
	%colorPicker.lockedAlpha = %noAlpha;
	%colorPicker.noAlpha = %noAlpha;

	if(%pData.Option[%pData.Setting,"mode"] $= "int")
		%colorPicker.isIntColor = true;

	if(%pData.Option[%pData.Setting,"auto"] !$= "")
		%colorPicker.autoColor = true;

	%floatLength = %pData.Option[%pData.Setting,"flen"];

	if (%floatLength > 0)
	{
		%colorPicker.floatLength = %floatLength;
	}

	return %colorPicker;
}
//------------------------------------------------------------------------------
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamColorEdit( %pData )
{
	%colorPicker = buildParamColor(%pData);
	%textEdit = %pData.pill-->TextEdit;
	%textEdit.command = %pData.Command;
	%textEdit.altCommand = %pData.AltCommand;
	%textEdit.internalName = %pData.Setting@"__ColorEdit";
	%textEdit.setting = %pData.Setting;
	%textEdit.colorPickerCtrl = %colorPicker;
	%textEdit.superClass = "GuiColorEditCtrl";
	%textEdit.isIntColor = %colorPicker.isIntColor;
	%textEdit.noAlpha = %colorPicker.noAlpha;
	%colorPicker.colorEditCtrl = %textEdit;
	%colorPicker.syncCtrls = %textEdit;
	%textEdit.floatLength = %colorPicker.floatLength;
	return %colorPicker;
}
//------------------------------------------------------------------------------

//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamColorInt( %pData )
{
	%pData.pill-->field.text = %pData.Title;
	%pData.pill-->field.internalName = "";
	//ColorPicker ctrl update
	%colorPicker = %pData.pill-->colorPicker;
	%colorPicker.command = %pData.Command;
	%colorPicker.altCommand = %pData.AltCommand;
	%colorPicker.internalName = %pData.InternalName;
	%checkbox.variable = %pData.Variable;
	%noAlpha = %pData.Option[%pData.Setting,"noalpha"];
	%colorPicker.lockedAlpha = %noAlpha;
	%colorPicker.isIntColor = true;
	return %colorPicker;
}
//------------------------------------------------------------------------------
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamColorAlpha( %pData )
{
	%pData.pill-->field.text = %pData.Title;
	%pData.pill-->field.internalName = "NoUpdate";
	//ColorPicker ctrl update
	%colorPicker = %pData.pill-->colorPicker;
	%colorPicker.command = %pData.Command;
	%colorPicker.altCommand = %pData.AltCommand;
	%colorPicker.internalName = %pData.InternalName;
	%colorPicker.variable = %pData.Variable;
	%colorPicker.alpha = "alphaSlider";
	%alphaSlider = %pData.pill-->slider;
	%alphaSlider.command = "updateParamColorAlpha($Me);";
	%alphaSlider.altCommand = "updateParamColorAlpha($Me);";
	%alphaSlider.internalName = "alphaSlider";
	%alphaSlider.dataType = %setting;
	%alphaSlider.range = "0 1";
	%alphaSlider.noFriends = true;
	%alphaSlider.variable = %pData.Variable;
	return %colorPicker;
}
//------------------------------------------------------------------------------
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamColorSlider( %pData )
{
	%isIntColor = false;

	if(%pData.Option[%pData.Setting,"mode"] $= "int")
		%isIntColor = true;

	%pData.pill-->field.text = %pData.Title;
	%alphaSlider = %pData.pill-->slider;
	//ColorPicker ctrl update
	%colorPicker = %pData.pill-->colorPicker;
	%colorPicker.command = %pData.Command;
	%colorPicker.altCommand = %pData.AltCommand;
	%colorPicker.internalName = %pData.InternalName;
	%colorPicker.alphaSlider = %alphaSlider;
	%checkbox.variable = %pData.Variable;
	%noAlpha = %pData.Option[%pData.Setting,"noalpha"];
	%colorPicker.lockedAlpha = %noAlpha;

	if(%pData.Option[%pData.Setting,"mode"] $= "int")
		%colorPicker.isIntColor = true;

	%alphaSlider.colorPicker = %colorPicker;
	%alphaSlider.fieldSource = %pData.Setting;
	%alphaSlider.command = %colorPicker@".AlphaChanged($ThisControl);";
	%alphaSlider.altCommand = %colorPicker@".AlphaChanged($ThisControl);";

	if (%isIntColor)
		%alphaSlider.range = "0 255";
	else
		%alphaSlider.range = "0 1";

	return %colorPicker;
}
//------------------------------------------------------------------------------
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamColorSliderEdit( %pData )
{
	%colorPicker = buildParamColor(%pData);
	%alphaSlider = %pData.pill-->slider;
	//ColorPicker ctrl update
	%colorPicker.command = %pData.Command;
	%colorPicker.altCommand = %pData.AltCommand;
	%colorPicker.internalName = %pData.InternalName;
	%colorPicker.alphaSlider = %alphaSlider;
	%checkbox.variable = %pData.Variable;
	%alphaSlider.colorPicker = %colorPicker;
	%alphaSlider.fieldSource = %pData.Setting;
	%alphaSlider.command = %colorPicker@".AlphaChanged($ThisControl);";
	%alphaSlider.altCommand = %colorPicker@".AlphaChanged($ThisControl);";

	if (%isIntColor)
		%alphaSlider.range = "0 255";
	else
		%alphaSlider.range = "0 1";

	%textEdit = %pData.pill-->TextEdit;
	%textEdit.command = %pData.Command;
	%textEdit.altCommand = %pData.AltCommand;
	%textEdit.internalName = %pData.Setting@"__ColorEdit";
	%textEdit.colorPickerCtrl = %colorPicker;
	%textEdit.superClass = "GuiColorEditCtrl";
	%textEdit.isIntColor = %isIntColor;
	%textEdit.alphaSlider = %alphaSlider;
	%textEdit.floatLength = %colorPicker.floatLength;
	%colorPicker.colorEditCtrl = %textEdit;
	%colorPicker.syncCtrls = %textEdit;
	return %colorPicker;
}
//------------------------------------------------------------------------------
