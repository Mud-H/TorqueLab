//==============================================================================
// GameLab -> Interface Development Gui
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Profile fields update functions
//==============================================================================
//==============================================================================
// Update a single profile field and set profile dirty if changed
function GLab::profileIsFieldOwner(%this,%profileName,%field ) {
	%field = strreplace(%field,"[","");
	%field = strreplace(%field,"]","");
	%ownList = $ProfOwnedFields[%profileName];
	%ownField = strFind(%ownList,%field);
	return %ownField;
}
//------------------------------------------------------------------------------

//==============================================================================
// Update a single profile field and set profile dirty if changed
function GLab::findParentFieldSource(%this,%profileName,%field ) {
	%parentName = $ProfParent[%profileName];
	%field = strreplace(%field,"[","");
	%field = strreplace(%field,"]","");

	while(%parentName !$= "" ) {
		%profileName = %parentName;

		if (%profileName $= "") {
			return false;
		}

		%ownList = $ProfOwnedFields[%profileName];
		%ownField = strFind(%ownList,%field);

		if (%ownField)
			return %profileName;

		%parentName = $ProfParent[%profileName];

		if (%parentName $= %profileName) {
			return false;
		}
	}

	return false;
}
//------------------------------------------------------------------------------