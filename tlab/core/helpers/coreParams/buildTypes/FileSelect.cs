//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamFileSelect( %pData )
{
	%pData.pill-->field.text = %pData.Title;
	%selButton = %pData.pill-->SelectButton;

	//TextEdit ctrl update
	if (%pData.Option[%pData.Setting,"callBack"] !$= "")
		%selButton.callCommand = %pData.Option[%pData.Setting,"callBack"];

	if (%pData.Option[%pData.Setting,"file"] !$= "")
		%selButton.baseFile = %pData.Option[%pData.Setting,"file"];

	if (%pData.Option[%pData.Setting,"filter"] !$= "")
		%selButton.filter = %pData.Option[%pData.Setting,"filter"];

	%selButton.superClass = "ParamFileSelectBtn";
	%selButton.pill = %pData.pill;
	%textEdit = %pData.pill-->textEdit;
	%textCommand = strreplace(%pData.Command,"$ThisControl",%textEdit.getId());
	%textAltCommand = strreplace(%pData.AltCommand,"$ThisControl",%textEdit.getId());
	%textEdit.command = %textCommand;
	%textEdit.altCommand = %textAltCommand;
	%textEdit.internalName = %pData.InternalName;
	%textEdit.variable = %pData.Variable;
	%selButton.textEdit = %textEdit;
	%textEdit.pill = %pData.pill;
}
//------------------------------------------------------------------------------
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function ParamFileSelectBtn::onClick( %this )
{

	if (%this.callCommand !$= "")
	{
		eval(%this.callCommand);
		return;
	}

	if (%this.filter $= "")
		%this.filter = "*.*";

	%baseFile = "main.cs";

	if (%this.textEdit.getText() !$="")
		%baseFile = %this.textEdit.getText();
	else if (%this.baseFile !$= "")
		%baseFile = %this.baseFile;

	getLoadFilename(%this.filter,"paramFileSelectCallback",%this.baseFile,%this,%this.textEdit);
}
//------------------------------------------------------------------------------
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function paramFileSelectCallback( %file,%button,%textEdit )
{
	%relFile = makeRelativePath(%file);
	%textEdit.setText(%relFile);
	%paramArray = %button.pill.paramObj;
	syncParamArrayCtrlData(%textEdit,"",%paramArray);
	//eval(%textEdit.command);
}
//------------------------------------------------------------------------------