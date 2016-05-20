//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamSliderText( %pData )
{
	%pData.pill-->field.text = %pData.Title;
	//TextEdit ctrl update
	%textValue = %pData.pill-->textValue;
	%textValue.internalName = %pData.InternalName;
	//Slider ctrl update
	%slider = %pData.pill-->slider;
	%range = %pData.Option[%pData.Setting,"range"];

	if (%range $="") %range = "0 1";

	%slider.range = %range;

	if (isObject(%pData.pill-->minRange))
	{
		%pData.pill-->minRange.text = %range.x;
		%pData.pill-->minRange.internalName = "";
	}

	if (isObject(%pData.pill-->maxRange))
	{
		%pData.pill-->maxRange.text = %range.y;
		%pData.pill-->maxRange.internalName = "";
	}

	foreach$(%option in %pData.OptionList[%pData.Setting])
		eval(%slider@%pData.OptionCmd[%pData.Setting,%option]);

	%slider.skipPrecision = true;
	%slider.command = %pData.Command;
	%slider.altCommand = %pData.AltCommand;
	%slider.internalName = %pData.InternalName@"__slider";
	%slider.variable = %pData.Variable;

	if (%pData.Value !$= "")
		%slider.setValue(%pData.Value);
}
//------------------------------------------------------------------------------
