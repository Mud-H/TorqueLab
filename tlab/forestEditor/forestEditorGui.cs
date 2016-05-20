//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------


// ForestEditorGui Script Methods

function ForestEditorGui::setActiveTool( %this, %tool ) {
	if ( %tool == ForestTools->BrushTool )
		ForestEditTabBook.selectPage(0);

	Parent::setActiveTool( %this, %tool );
}

/// This is called by the editor when the active forest has
/// changed giving us a chance to update the GUI.
function ForestEditorGui::onActiveForestUpdated( %this, %forest, %createNew ) {
	%gotForest = isObject( %forest );

	// Give the user a chance to add a forest.
	if ( !%gotForest && %createNew ) {
		LabMsgYesNo(  "Forest",
						  "There is not a Forest in this mission.  Do you want to add one?",
						  %this @ ".createForest();", "" );
		return;
	}
}

/// Called from a message box when a forest is not found.
function ForestEditorGui::createForest( %this ) {
	if ( isObject( theForest ) ) {
		error( "Cannot create a second 'theForest' Forest!" );
		return;
	}

	// Allocate the Forest and make it undoable.
	%file = strreplace(MissionGroup.getFilename(),".mis",".forest");
	//%file = filePath(MissionGroup.getFilename())@"/data.forest";
	%file = strreplace(MissionGroup.getFilename(),".mis","a.forest");
	new Forest( theForest ) {
		//dataFile = %file;
		parentGroup = "MissionGroup";
	};
	MECreateUndoAction::submit( theForest );
	ForestEditorGui.setActiveForest( theForest );
	//Re-initialize the editor settings so we can start using it immediately.
	%tool = ForestEditorGui.getActiveTool();

	if ( isObject( %tool ) )
		%tool.onActivated();

	if ( %tool == ForestTools->SelectionTool ) {
		%mode = GlobalGizmoProfile.mode;

		switch$ (%mode) {
		case "None":
			ForestEditorSelectModeBtn.performClick();

		case "Move":
			ForestEditorMoveModeBtn.performClick();

		case "Rotate":
			ForestEditorRotateModeBtn.performClick();

		case "Scale":
			ForestEditorScaleModeBtn.performClick();
		}
	} else if ( %tool == ForestTools->BrushTool ) {
		%mode = ForestTools->BrushTool.mode;

		switch$ (%mode) {
		case "Paint":
			ForestEditorPaintModeBtn.performClick();

		case "Erase":
			ForestEditorEraseModeBtn.performClick();

		case "EraseSelected":
			ForestEditorEraseSelectedModeBtn.performClick();
		}
	}

	EWorldEditor.isDirty = true;
//	ForestEditorInspector.inspect( theForest );
	//EWorldEditor.isDirty = true;
}


function ForestEditorGui::deleteBrushOrElement( %this ) {
	ForestEditBrushTree.deleteSelection();
	ForestEditorPlugin.dirty = true;
}




// Child-control Script Methods


function ForestEditMeshTree::onSelect( %this, %obj ) {
	ForestEditorInspector.inspect( %obj );
}


function ForestEditTabBook::onTabSelected( %this, %text, %idx ) {
	%bbg = ForestEditorPalleteWindow.findObjectByInternalName("BrushButtonGroup");
	%mbg = ForestEditorPalleteWindow.findObjectByInternalName("MeshButtonGroup");
	%bbg.setVisible( false );
	%mbg.setVisible( false );

	if ( %text $= "Brushes" ) {
		%bbg.setVisible( true );
		%obj = ForestEditBrushTree.getSelectedObject();
		ForestEditorInspector.inspect( %obj );
	} else if ( %text $= "Meshes" ) {
		%mbg.setVisible( true );
		%obj = ForestEditMeshTree.getSelectedObject();
		ForestEditorInspector.inspect( %obj );
	}
}



function ForestEditorInspector::inspect( %this, %obj ) {
	if ( isObject( %obj ) )
		%class = %obj.getClassName();

	%this.showObjectName = false;
	%this.showCustomFields = false;

	switch$ ( %class ) {
	case "ForestBrush":
		%this.groupFilters = "+NOTHING,-Ungrouped";

	case "ForestBrushElement":
		%this.groupFilters = "+ForestBrushElement,-Ungrouped";

	case "TSForestItemData":
		%this.groupFilters = "-Media,+Wind";

	default:
		%this.groupFilters = "";
	}

	Parent::inspect( %this, %obj );
}

function ForestEditorInspector::onInspectorFieldModified( %this, %object, %fieldName, %oldValue, %newValue ) {
	// The instant group will try to add our
	// UndoAction if we don't disable it.
	%instantGroup = $InstantGroup;
	$InstantGroup = 0;
	%nameOrClass = %object.getName();

	if ( %nameOrClass $= "" )
		%nameOrClass = %object.getClassname();

	%action = new InspectorFieldUndoAction() {
		actionName = %nameOrClass @ "." @ %fieldName @ " Change";
		objectId = %object.getId();
		fieldName = %fieldName;
		fieldValue = %oldValue;
		inspectorGui = %this;
	};
	// Restore the instant group.
	$InstantGroup = %instantGroup;
	%action.addToManager( Editor.getUndoManager() );

	if ( %object.getClassName() $= "TSForestItemData" )
		ForestDataManager.setDirty( %object );

	ForestEditorPlugin.dirty = true;
}

function ForestEditorInspector::onFieldSelected( %this, %fieldName, %fieldTypeStr, %fieldDoc ) {
	//FieldInfoControl.setText( "<font:ArialBold:14>" @ %fieldName @ "<font:ArialItalic:14> (" @ %fieldTypeStr @ ") " NL "<font:Arial:14>" @ %fieldDoc );
}

function ForestBrushSizeSliderCtrlContainer::onWake(%this) {
	%this-->slider.range = "1" SPC getWord(ETerrainEditor.maxBrushSize, 0);
	%this-->slider.setValue(ForestBrushSizeTextEditContainer-->textEdit.getValue());
}
