//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================



//==============================================================================
function EditorGuiStatusBar::reset( %this ) {
	EWorldEditorStatusBarInfo.clearInfo();
}
//------------------------------------------------------------------------------
//==============================================================================
function EditorGuiStatusBar::getInfo( %this ) {
	return EWorldEditorStatusBarInfo.getValue();
}
//------------------------------------------------------------------------------
//==============================================================================
function EditorGuiStatusBar::setInfo( %this, %text ) {
	EWorldEditorStatusBarInfo.setText(%text);
}
//------------------------------------------------------------------------------
//==============================================================================
function EditorGuiStatusBar::clearInfo( %this ) {
	EWorldEditorStatusBarInfo.setText("");
}
//------------------------------------------------------------------------------
//==============================================================================
function EditorGuiStatusBar::getSelection( %this ) {
	return EWorldEditorStatusBarSelection.getValue();
}
//------------------------------------------------------------------------------
//==============================================================================
function EditorGuiStatusBar::setSelection( %this, %text ) {
	EWorldEditorStatusBarSelection.setText(%text);
}
//------------------------------------------------------------------------------
//==============================================================================
function EditorGuiStatusBar::setSelectionObjectsByCount( %this, %count ) {
	%text = " objects selected";

	if(%count == 1)
		%text = " object selected";

	EWorldEditorStatusBarSelection.setText(%count @ %text);
}
//------------------------------------------------------------------------------
//==============================================================================
function EditorGuiStatusBar::clearSelection( %this ) {
	EWorldEditorStatusBarSelection.setText("");
}
//------------------------------------------------------------------------------
//==============================================================================
function EditorGuiStatusBar::getCamera( %this ) {
	return EWorldEditorStatusBarCamera.getText();
}
//------------------------------------------------------------------------------
//==============================================================================
function EditorGuiStatusBar::setCamera( %this, %text,%doUpdate ) {
	if (%doUpdate $= "")
		%doUpdate = true;

	%id = EWorldEditorStatusBarCamera.findText( %text );

	if( %id != -1 ) {
		if ( EWorldEditorStatusBarCamera.getSelected() != %id )
			EWorldEditorStatusBarCamera.setSelected( %id, %doUpdate );
	}
}
//------------------------------------------------------------------------------
