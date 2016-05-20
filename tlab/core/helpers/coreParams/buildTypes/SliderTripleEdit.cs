//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamSliderTripleEdit( %pData )
{
	%pData.pill-->field.text = %pData.Title;
	//TextEdit ctrl update
	%textEdit = %pData.pill-->textEdit;
	%textEdit.command = %pData.Command;
	%textEdit.altCommand = %pData.AltCommand;
	%textEdit.internalName = %pData.InternalName;

	if (%pData.Value !$= "")
		%textEdit.text = %pData.Value;

	%precision =  %pData.Option[%pData.Setting,"precision"];

	if (%precision !$="" && %pData.Value !$= "")
	{
		%fixValue = setFloatPrecision(%pData.Value,%precision);
		%textEdit.text = %fixValue;
		%textEdit.variable = "";
	}
	else
		%textEdit.variable = %pData.Variable;

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

	%slider.command = %pData.Command;
	%slider.altCommand = %pData.AltCommand;
	%slider.internalName = %pData.InternalName@"__slider";
	%slider.variable = %pData.Variable;

	if (%pData.Value !$= "")
		%slider.setValue(%pData.Value);

	//Slider1 ctrl update
	%slider1 = %pData.pill-->slider1;
	%range1 = %pData.Option[%pData.Setting,"range1"];

	if (%range1 $="") %range1 = "0 1";

	if (isObject(%pData.pill-->minRange1))
	{
		%pData.pill-->minRange1.text = %range1.x;
		%pData.pill-->minRange1.internalName = "";
	}

	if (isObject(%pData.pill-->maxRange1))
	{
		%pData.pill-->maxRange1.text = %range1.y;
		%pData.pill-->maxRange1.internalName = "";
	}

	foreach$(%option in %pData.OptionList[%pData.Setting])
		eval(%slider1@%pData.OptionCmd[%pData.Setting,%option]);

	%slider1.range = %range1;
	%slider1.command = %pData.Command;
	%slider1.altCommand = %pData.AltCommand;
	%slider1.internalName = %pData.InternalName@"__slider1";
	%slider1.variable = %pData.Variable;

	if (%pData.Value !$= "")
		%slider1.setValue(%pData.Value);

	//Slider2 ctrl update
	%slider2 = %pData.pill-->slider2;
	%range2 = %pData.Option[%pData.Setting,"range2"];

	if (%range2 $="") %range2 = "0 1";

	%slider2.range = %range2;

	if (isObject(%pData.pill-->minRange2))
	{
		%pData.pill-->minRange2.text = %range2.x;
		%pData.pill-->minRange2.internalName = "";
	}

	if (isObject(%pData.pill-->maxRange2))
	{
		%pData.pill-->maxRange2.text = %range2.y;
		%pData.pill-->maxRange2.internalName = "";
	}

	foreach$(%option in %pData.OptionList[%pData.Setting])
		eval(%slider2@%pData.OptionCmd[%pData.Setting,%option]);

	%slider2.range = %range2;
	%slider2.command = %pData.Command;
	%slider2.altCommand = %pData.AltCommand;
	%slider2.internalName = %pData.InternalName@"__slider2";
	%slider2.variable = %pData.Variable;

	if (%pData.Value !$= "")
		%slider2.setValue(%pData.Value);
}
//------------------------------------------------------------------------------
