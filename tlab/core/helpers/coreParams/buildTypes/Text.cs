//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamText( %pData )
{
	%pData.pill-->field.text = %pData.Title;
	//TextEdit ctrl update
	%textValue = %pData.pill-->value;

	%textValue.text = %pData.Default;
	
	%textValue.internalName = %pData.InternalName;
}
//------------------------------------------------------------------------------
