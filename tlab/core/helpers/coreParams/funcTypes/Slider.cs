//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function paramSliderOptions( %pData,%slider )
{
	//Check if there's a range settings
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

	//TickAt will force a tick at the specified step
	%tickAt = %pData.Option[%pData.Setting,"tickAt"];

	if (%tickAt !$= "")
	{
		%totalRange = %slider.range.y - %slider.range.x;
		%totalTicks = (%totalRange / %tickAt) - 1;
		%slider.ticks = %totalTicks;
		%slider.snap = true;
	}

	return %slider;
}
//------------------------------------------------------------------------------
