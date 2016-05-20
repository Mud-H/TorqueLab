//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamSliderEdit( %pData )
{
	%slider = buildParamSlider(%pData);
	//TextEdit ctrl update
	%textEdit = buildParamTextEdit(%pData);
	//Slider ctrl update
	%slider = paramSliderOptions(%pData,%slider);

	foreach$(%option in %pData.OptionList[%pData.Setting])
	{
		eval(%slider@%pData.OptionCmd[%pData.Setting,%option]);
	}

	%slider.internalName = %pData.InternalName@"__slider";
	%slider.extent.y = %pData.Widget-->Slider.extent.y;
	%textEdit.friend = %slider;
	%slider.friend = %textEdit;
	return %textEdit;
}
//------------------------------------------------------------------------------
