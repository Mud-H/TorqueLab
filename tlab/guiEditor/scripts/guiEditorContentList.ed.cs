//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

// Code for the drop-down that allows selecting a GUI to edit in the Gui Editor.


if( !isDefined( "$GuiEditor::GuiFilterList" ) ) {
	/// List of named controls that are filtered out from the
	/// control list dropdown.
	$GuiEditor::GuiFilterList =
		"GuiEditorGui" TAB
		"AL_ShadowVizOverlayCtrl" TAB
		"ToolsMsgBoxOKDlg" TAB
		"ToolsMsgBoxOKCancelDlg" TAB
		"ToolsMsgBoxOKCancelDetailsDlg" TAB
		"ToolsMsgBoxYesNoDlg" TAB
		"ToolsMsgBoxYesNoCancelDlg" TAB
		"MessagePopupDlg";
}


//------------------------------------------------------------------------------
//==============================================================================
function GuiEditorContentList::init( %this ) {
	%this.clear();
	%this.scanGroup( GuiGroup );
}
//------------------------------------------------------------------------------
//==============================================================================
function GuiEditorContentList::scanGroup( %this, %group ) {
	foreach( %obj in %group ) {
		if( %obj.isMemberOfClass( "GuiControl" ) ) {
			if(%obj.getClassName() $= "GuiCanvas") {
				%this.scanGroup( %obj );
			} else {
				if (%obj.getName() $= "" && !$pref::GuiEditor::ShowUnnamed)
					%skip = true;
				else if(%obj.getName() $= "")
					%name = "(unnamed) - " @ %obj;
				else
					%name = %obj.getName() @ " - " @ %obj;

				%skip = false;

				foreach$( %guiEntry in $GuiEditor::GuiFilterList )
					if( %obj.getName() $= %guiEntry ) {
						%skip = true;
						break;
					}

				if( !%skip )
					%this.add( %name, %obj );
			}
		} else if( %obj.isMemberOfClass( "SimGroup" )&&  ( %obj.internalName !$= "EditorGuiGroup"  || GuiEditor.showEditorGuis)  ) { // except if explicitly requested.
			// Scan nested SimGroups for GuiControls.
			%this.scanGroup( %obj );
		} else if( %obj.isMemberOfClass( "SimGroup" ) && %obj.internalName $= "EditorGuiGroup" && $pref::GuiEditor::ShowEditorsGui ) { // except if explicitly requested.
			// Scan nested SimGroups for GuiControls.
			%this.scanGroup( %obj );
		}
	}
}
//------------------------------------------------------------------------------

//=============================================================================================
//    Event Handlers.
//=============================================================================================


//==============================================================================
function GuiEditorContentList::onSelect( %this, %ctrl ) {
	GuiEditor.openForEditing( %ctrl );
}
//------------------------------------------------------------------------------