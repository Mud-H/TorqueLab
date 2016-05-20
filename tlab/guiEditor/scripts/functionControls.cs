//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function Lab::forceCtrlInsideParent(%this) {
	%selection =  GuiEditor.getSelection();

	while(isObject(%selection.getObject(%i))) {
		%control = %selection.getObject(%i);
		%parent = %control.parentGroup;

		if (!isObject(%parent)) {
			info(%control.getName()," have no parent to be forced inside");
			%i++;
			continue;
		}

		%realExtentX = %control.position.x + %control.extent.x;

		if (%realExtentX > %parent.extent.x)
			%control.extent.x = %parent.extent.x - %control.position.x;

		%realExtentY = %control.position.y + %control.extent.y;

		if (%realExtentY > %parent.extent.y)
			%control.extent.y = %parent.extent.y - %control.position.y;

		%i++;
	}
}
//------------------------------------------------------------------------------
$GuiEditorAlignMargin = 0;


//==============================================================================
function Lab::setSelectedControlAsReference(%this) {
	%selection =  GuiEditor.getSelection();
	%control = %selection.getObject(0);
	$GuiEditor_ReferenceControl = %control;
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::setControlReferenceField(%this,%field) {
	%obj = $GuiEditor_ReferenceControl;

	if (!isObject(%obj)) {
		warnLog("No reference object setted");
		return;
	}

	%field = trim(%field);

	if (%field $= "name")
		%value = ""; //Referenced name always empty since they must be unique
	else
		%value = %obj.getFieldValue(%field);

	%selection =  GuiEditor.getSelection();
	%i = 0;

	while(isObject(%selection.getObject(%i))) {
		%control = %selection.getObject(%i);
		%control.setFieldValue(%field,%value);
		%i++;
	}
}
//------------------------------------------------------------------------------
