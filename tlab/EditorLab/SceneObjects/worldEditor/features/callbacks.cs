//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Objects Selection Callback - Group of function called when selecting objects
//==============================================================================
//==============================================================================
// Add a function to call when selecting an object
function Lab::AddSelectionCallback(%this,%function,%type) {
	Lab.selectionCallback[%type] = strAddField(Lab.selectionCallback[%type],%function);
}
//------------------------------------------------------------------------------
//==============================================================================
// Call all the selection callbacks when selection changed
function Lab::DoSelectionCallback(%this,%type,%object) {
	%fields = Lab.selectionCallback[%type];
	%count = getFieldCount(	%fields);

	for(%i=0; %i<%count; %i++) {
		%evalFunc = getField(%fields,%i)@"(%object);";
		eval(getField(%fields,%i)@"(%object);");
	}
}
//------------------------------------------------------------------------------