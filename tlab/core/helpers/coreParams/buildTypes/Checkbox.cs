//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamCheckbox( %pData )
{
	//Checkbox ctrl update
	%checkbox = %pData.pill-->checkbox;
	%checkbox.text = "";
	%checkbox.command = %pData.Command;
	%checkbox.altCommand = %pData.AltCommand;
	%checkbox.internalName = %pData.InternalName;
	%checkbox.variable = %pData.Variable;

	//If no field ctrl, set the title on checkbox
	if (isObject(%pData.pill-->field))
		%pData.pill-->field.text = %pData.Title;
	else
		%checkbox.text = %pData.Title;
}
//------------------------------------------------------------------------------
