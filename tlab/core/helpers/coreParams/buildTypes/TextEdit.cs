//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamTextEdit( %pData )
{
	%pData.pill-->field.text = %pData.Title;
	//TextEdit ctrl update
	%textEdit = %pData.pill-->textEdit;

	//OLD TEXTEDIT INTERNALNAME
	if (!isObject(%textEdit))
		%textEdit = %pData.pill-->edit;

	%textEdit.command = %pData.Command;
	%textEdit.altCommand = %pData.AltCommand;
	%textEdit.internalName = %pData.InternalName;
	%textEdit.variable = %pData.Variable;
	%precision =  %pData.Option[%pData.Setting,"precision"];

	if (%precision !$="" && %pData.Value !$= "")
	{
		%fixValue = setFloatPrecision(%pData.Value,%precision);
		%textEdit.text = %fixValue;
		%textEdit.variable = "";
	}
	else
		%textEdit.variable = %pData.Variable;

	if (%pData.Value !$= "")
		%textEdit.text = %pData.Value;

	return %textEdit;
}
//------------------------------------------------------------------------------
