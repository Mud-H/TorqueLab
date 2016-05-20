//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamSlider( %pData )
{
	%mouseArea = %pData.pill-->mouse;

	if (isObject (%mouseArea))
	{
		%mouseArea.infoText = %tooltip;

		if (%pData.mouseAreaClass !$="")
			%mouseArea.superClass = %mouseSuperClass;
	}

	%pData.pill-->field.text = %pData.Title;
	//Slider ctrl update
	%slider = %pData.pill-->slider;
	%slider = paramSliderOptions(%pData,%slider);

	foreach$(%option in %pData.OptionList[%pData.Setting])
		eval(%slider@%pData.OptionCmd[%pData.Setting,%option]);

	%slider.command = %pData.Command;
	%slider.altCommand = %pData.AltCommand;
	%slider.internalName = %pData.InternalName;
	%slider.variable = %pData.Variable;

	if (%pData.Value !$= "")
		%slider.setValue(%pData.Value);

	%slider.tooltip = %tooltip;
	%slider.hovertime = %tooltipDelay;
	%pData.pill-->field.tooltip = %tooltip;
	%pData.pill-->field.hovertime = %tooltipDelay;
	return %slider;
}
//------------------------------------------------------------------------------
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamSliderOnly( %pData )
{
	%mouseArea = %pData.pill-->mouse;

	if (isObject (%mouseArea))
	{
		%mouseArea.infoText = %tooltip;

		if (%pData.mouseAreaClass !$="")
			%mouseArea.superClass = %mouseSuperClass;
	}

	%pData.pill-->field.text = %pData.Title;
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
	%slider.internalName = %pData.InternalName;
	%slider.variable = %pData.Variable;

	if (%pData.Value !$= "")
		%slider.setValue(%pData.Value);

	%slider.tooltip = %tooltip;
	%slider.hovertime = %tooltipDelay;
	%pData.pill-->field.tooltip = %tooltip;
	%pData.pill-->field.hovertime = %tooltipDelay;
}
//------------------------------------------------------------------------------
