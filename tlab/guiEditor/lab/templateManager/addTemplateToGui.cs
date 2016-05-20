//==============================================================================
// TorqueLab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function TplManager::addTemplateToGui(%this,%tplGui,%addTo) {
	GuiEd.closeTplPreview();
	%tplClone = %tplGui.getObject(0).deepClone();
	show(%tplClone);
	%tplClone.setName("");
	%tplClone.templateSrc = %tplGui.getName();

	if (!isObject(%addTo))
		%addTo = GuiEditor.getCurrentAddSet();

	%addTo.add(%tplClone);
}
//------------------------------------------------------------------------------
