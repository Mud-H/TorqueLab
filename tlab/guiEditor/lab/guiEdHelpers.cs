//==============================================================================
// TorqueLab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function GuiEd::AlignCtrlToParent(%this,%direction) {
	%selection =  GuiEditor.getSelection();

	while(isObject(%selection.getObject(%i))) {
		%control = %selection.getObject(%i);
		%parent = %control.parentGroup;

		if (!isObject(%parent)) {
			info(%control.getName()," have no parent to be forced inside");
			%i++;
			continue;
		}

		switch$(%direction) {
		case "right": //Set max right of parent
			%control.position.x = %parent.extent.x - %control.extent.x - $GuiEditorAlignMargin;

		case "left": //Set max left of parent
			%control.position.x = $GuiEditorAlignMargin;

		case "top": //Set max left of parent
			%control.position.y = $GuiEditorAlignMargin;

		case "bottom": //Set max right of parent
			%control.position.y = %parent.extent.y - %control.extent.y -$GuiEditorAlignMargin;
		}

		%i++;
	}
}
//------------------------------------------------------------------------------