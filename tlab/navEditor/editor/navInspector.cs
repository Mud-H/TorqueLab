//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Plugin Object Params - Used set default settings and build plugins options GUI
//==============================================================================


function NavInspector::inspect(%this, %obj) {
	%name = "";

	if(isObject(%obj))
		%name = %obj.getName();
	else
		NavFieldInfoControl.setText("");

	Parent::inspect(%this, %obj);
}

function NavInspector::onInspectorFieldModified(%this, %object, %fieldName, %arrayIndex, %oldValue, %newValue) {
	// Same work to do as for the regular WorldEditor Inspector.
	Inspector::onInspectorFieldModified(%this, %object, %fieldName, %arrayIndex, %oldValue, %newValue);
}

function NavInspector::onFieldSelected(%this, %fieldName, %fieldTypeStr, %fieldDoc) {
	NavFieldInfoControl.setText("<font:ArialBold:14>" @ %fieldName @ "<font:ArialItalic:14> (" @ %fieldTypeStr @ ") " NL "<font:Arial:14>" @ %fieldDoc);
}

function NavTreeView::onInspect(%this, %obj) {
	NavInspector.inspect(%obj);
}

function NavTreeView::onSelect(%this, %obj) {
	NavInspector.inspect(%obj);
	NavEditorGui.onObjectSelected(%obj);
}
