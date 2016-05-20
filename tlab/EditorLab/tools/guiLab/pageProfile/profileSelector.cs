//==============================================================================
// GameLab -> Interface Development Gui
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function GLab::showProfileFieldOptions( %this,%field ) {
	%profile = $GLab_SelectedObject;

	if (%field $= "" || !isObject(%profile)) {
		GLab_ActiveFieldRollout.visible = false;
		GLab_ActiveFieldRollout.expanded = false;
		$GLab_SelectedField = "";
	}

	GLab_ActiveFieldRollout.visible = true;
	$GLab_SelectedField = %field;
	%value = %profile.getFieldValue(%field);
	%profileName = %profile.getName();
	%parent = %this.findParentFieldSource(%profileName,%field);
	%parentValue = %parent.getFieldValue(%field);
	%isOwner = %this.profileIsFieldOwner(%profileName,%field);

	if (!%isOwner)
		%owner = %parent;
	else
		%owner = %profileName;

	%showParentInfo = true;

	if (%parent $= "0")
		%showParentInfo = false;

	$GLab_FieldValueInfo = "Value:\c2" SPC %value;
	$GLab_FieldSource = "Source:\c2 "@%owner;
	$GLab_FieldOpt_Owner =  %owner;
	$GLab_FieldOpt_Title = %field SPC "\c2" SPC %value;
	$GLab_FieldOpt_ParentValue = "Value \c2" SPC %parentValue;
	$GLab_FieldOpt_Parent = "Profile \c2" SPC %parent;
	GLab_FieldOptions-->editProfile.parentProfile = %parent;
	GLab_ActiveFieldRollout.caption = $GLab_Caption_NoFieldSelected;
	GLab_ActiveFieldRollout.caption = %field SPC"\c6info";
	GLab_ActiveFieldRollout.expanded = true;
	%parentInfo = GLab_FieldOptions-->parentInfo;
	GLab_FieldOptions-->isNotOwnerInfo.setVisible(!%isOwner);
	GLab_FieldOptions-->isOwnerInfo.setVisible(%isOwner);
}
//------------------------------------------------------------------------------

function GLab_FieldInfoMouse::onMouseDown( %this,%arg1,%arg2 ) {
	loga("GLab_FieldInfoMouse::onMouseDown( %this,%arg1,%arg2 )",%this,%arg1,%arg2);
	GLab.showProfileFieldOptions(%this.linkedField);
}
//==============================================================================
// Load the GuiManager scripts and guis if specified
function GLab::CreateFieldContextMenu() {
	if( !isObject( GLab.contextMenuField ) )
		GLab.contextMenuField = new PopupMenu() {
		superClass = "MenuBuilder";
		isPopup = true;
		item[ 0 ] = "\c2-- Selected Field Options --";
		item[ 1 ] = "Remove from profile" TAB "" TAB "removeProfileField($GLab_SelectedObject,$GLab_ProfileFieldContext);";
		item[ 2 ] = "\c2-- Selected Profile Options --";
		item[ 3 ] = "Remove all fonts colors from profile" TAB "" TAB "GLab.ClearProfileColorType(\"All\",$GLab_SelectedObject);";
		object = -1;
	};
}
//------------------------------------------------------------------------------
function GLab_FieldInfoMouse::onRightMouseDown( %this,%mousePoint,%clicks ) {
	loga("GLab_FieldInfoMouse::onRightMouseDown( %this,%mousePoint,%clicks )",%this,%mousePoint,%clicks);

	if( !isObject( GLab.contextMenuField ))
		GLab.CreateFieldContextMenu();

	GLab.contextMenuField.setItem(0,"-->"@%this.linkedField@"<-- Field Options","");
	GLab.contextMenuField.setItem(2,"-->"@$GLab_SelectedProfile@"<-- Profile Options","");
	$GLab_ProfileFieldContext = %this.linkedField;
	GLab.contextMenuField.showPopup( Canvas );
}


function GLab::selectProfile( %this,%profile ) {
	%id = GLab_ProfilesTree.findItemByName(%profile.getName());
	GLab_ProfilesTree.selectItem(%id);
}
//==============================================================================
function GLab::setSelectedProfile( %this,%profile ) {
	if (%profile $= "") {
		%id = GLab_ProfilesTree.getSelectedItem();
		%profile = GLab_ProfilesTree.getItemValue( %id );
	}

	if (!isObject(%profile)) {
		$GLab_SelectedProfile = "";
		$GLab_SelectedObject = "";
		return;
	}

	$GLab_SelectedProfile = %profile.getName();
	$GLab_SelectedObject = %profile;
	$GLab_SelectedField = "";
	%this.syncProfileParamArray();

	if( !isObject( "GuiEditorProfilesPM" ) )
		new PersistenceManager( GuiEditorProfilesPM );

	GLab_ProfileInspector.inspect(%profile);
	GLab_ActiveFieldRollout.caption =  $GLab_Caption_NoFieldSelected;
	GLab_ActiveFieldRollout.expanded = false;
	%active = strFind($DirtyList,%profile.getName());
	GLab_SaveSelectProfileButton.setActive(%active);
}
//------------------------------------------------------------------------------

//==============================================================================
function GLab::toggleActiveFieldRollout( %this ) {
	GLab_ActiveFieldRollout.setVisible(!GLab_ActiveFieldRollout.visible);
	$GLab::ShowActiveFieldRollout = GLab_ActiveFieldRollout.visible;
}
//------------------------------------------------------------------------------