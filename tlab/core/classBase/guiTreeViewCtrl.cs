//==============================================================================
// TorqueLab -> GuiTreeViewCtrl
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

// Common functions for filter text and clear button controls on tree views.
// The GuiTextEditCtrl having the filter text must have "treeView" dynamic field
// that has the ID of the associated GuiTreeViewCtrl.
// The button ctrl used to clear the text field must have a "textCtrl" dynamic field
// that has the ID of the associated filter GuiTextEditCtrl.

function EditorTreeView::onDefineIcons( %this ) {
	%icons = "tlab/art/icons/iconTables/TreeViewBase/default:" @
				"tlab/art/icons/iconTables/TreeViewBase/folderclosed:" @
				"tlab/art/icons/iconTables/TreeViewBase/groupclosed:" @
				"tlab/art/icons/iconTables/TreeViewBase/folderopen:" @
				"tlab/art/icons/iconTables/TreeViewBase/groupopen:" @
				"tlab/art/icons/iconTables/TreeViewBase/hidden:" @
				"tlab/art/icons/iconTables/TreeViewBase/shll_icon_passworded_hi:" @
				"tlab/art/icons/iconTables/TreeViewBase/shll_icon_passworded:" @
				"tlab/art/icons/iconTables/TreeViewBase/default";
	%this.buildIconTable(%icons);
}

function GuiTreeViewCtrl::handleRenameObject( %this, %name, %obj ) {
	logc("GuiTreeViewCtrl::handleRenameObject( %this, %name, %obj )", %this, %name, %obj );

	if (!isObject(%obj))
		return;

	%inspector = GuiInspector::findByObject( %obj ); // Changed from GuiInspector::findByObject( %obj );

	if( isObject( %inspector ) ) {
		%field = ( %this.renameInternal ) ? "internalName" : "name";
		%inspector.setObjectField( %field, %name );
		return true;
	}

	return false;
}

//---------------------------------------------------------------------------------------------

function GuiTreeViewFilterText::onWake( %this ) {
	//Mud-H add validation to prevent random crashes (to be removed)
	if (!isObject(%this.treeView)) {
		warnLog("Invalid treeview object for GuiTreeViewFilterText::onWake",%this.getName());
		return;
	}

	%filter = %this.treeView.getFilterText();

	if( %filter $= "" )
		%this.setText( "\c2Filter..." );
	else
		%this.setText( %filter );
}

//---------------------------------------------------------------------------------------------

function GuiTreeViewFilterText::onGainFirstResponder( %this ) {
	%this.selectAllText();
}

//---------------------------------------------------------------------------------------------

// When Enter is pressed in the filter text control, pass along the text of the control
// as the treeview's filter.
function GuiTreeViewFilterText::onReturn( %this ) {
	%text = %this.getText();

	if( %text $= "" )
		%this.reset();
	else
		%this.treeView.setFilterText( %text );
}

//---------------------------------------------------------------------------------------------

function GuiTreeViewFilterText::reset( %this ) {
	%this.setText( "\c2Filter..." );
	%this.treeView.clearFilterText();
}

//---------------------------------------------------------------------------------------------

function GuiTreeViewFilterClearButton::onClick( %this ) {
	%this.textCtrl.reset();
}
