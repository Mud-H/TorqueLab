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

$River::EditorOpen = false;
$River::wireframe = true;
$River::showSpline = true;
$River::showRiver = true;
$River::showWalls = true;

function RiverEditorGui::onEditorActivated( %this ) {
	%count = EWorldEditor.getSelectionSize();

	for ( %i = 0; %i < %count; %i++ ) {
		%obj = EWorldEditor.getSelectedObject(%i);

		if ( %obj.getClassName() !$= "River" )
			EWorldEditor.unselectObject( %obj );
		else
			%this.setSelectedRiver( %obj );
	}

	%this.onRiverSelected( %this.getSelectedRiver() );
	%this.onNodeSelected(-1);
}

function RiverEditorGui::onEditorDeactivated( %this ) {
}

function RiverEditorGui::createRiver( %this ) {
	%river = new River() {
		rippleDir[0] = "0.000000 1.000000";
		rippleDir[1] = "0.707000 0.707000";
		rippleDir[2] = "0.500000 0.860000";
		rippleSpeed[0] = "-0.065";
		rippleSpeed[1] = "0.09";
		rippleSpeed[2] = "0.04";
		rippleTexScale[0] = "7.140000 7.140000";
		rippleTexScale[1] = "6.250000 12.500000";
		rippleTexScale[2] = "50.000000 50.000000";
		waveDir[0] = "0.000000 1.000000";
		waveDir[1] = "0.707000 0.707000";
		waveDir[2] = "0.500000 0.860000";
		waveSpeed[0] = "1";
		waveSpeed[1] = "1";
		waveSpeed[2] = "1";
		waveMagnitude[0] = "0.2";
		waveMagnitude[1] = "0.2";
		waveMagnitude[2] = "0.2";
		baseColor = "45 108 171 255";
		rippleTex = "art/textures/water/ripple.dds";
		foamTex = "art/textures/water/foam";
		depthGradientTex = "art/textures/water/depthcolor_ramp";
	};
	return %river;
}

function RiverEditorGui::paletteSync( %this, %mode ) {
	%evalShortcut = "LabPaletteArray-->" @ %mode @ ".setStateOn(1);";
	eval(%evalShortcut);
}

function RiverEditorGui::onEscapePressed( %this ) {
	if( %this.getMode() $= "RiverEditorAddNodeMode" ) {
		%this.prepSelectionMode();
		return true;
	}

	return false;
}

function RiverEditorGui::onRiverSelected( %this, %river ) {
	%this.river = %river;
	RiverInspector.inspect( %river );
	RiverTreeView.buildVisibleTree(true);
	RiverManager.currentRiver = %river;

	if( RiverTreeView.getSelectedObject() != %river ) {
		RiverTreeView.clearSelection();
		%treeId = RiverTreeView.findItemByObjectId( %river );
		RiverTreeView.selectItem( %treeId );
	}

	RiverManager.updateRiverData();
}

function RiverEditorGui::onNodeSelected( %this, %nodeIdx ) {
	RiverEditorGui.selectedNode = %nodeIdx;

	if ( %nodeIdx == -1 ) {
		RiverEditorGui.selectedNode = %nodeIdx;
		RiverEditorOptionsWindow-->position.setActive( false );
		RiverEditorOptionsWindow-->position.setValue( "" );
		RiverEditorOptionsWindow-->rotation.setActive( false );
		RiverEditorOptionsWindow-->rotation.setValue( "" );
		RiverEditorOptionsWindow-->CurrentWidth.setActive( false );
		RiverEditorOptionsWindow-->CurrentWidth.setValue( "" );
		RiverEditorOptionsWindow-->CurrentWidthSlider.setActive( false );
		RiverEditorOptionsWindow-->CurrentWidthSlider.setValue( "" );
		RiverEditorOptionsWindow-->CurrentDepth.setActive( false );
		RiverEditorOptionsWindow-->CurrentDepth.setValue( "" );
		RiverEditorOptionsWindow-->CurrentDepthSlider.setActive( false );
		RiverEditorOptionsWindow-->CurrentDepthSlider.setValue( "" );
	} else {
		RiverEditorOptionsWindow-->position.setActive( true );
		RiverEditorOptionsWindow-->position.setValue( %this.getNodePosition() );
		RiverEditorOptionsWindow-->rotation.setActive( true );
		RiverEditorOptionsWindow-->rotation.setValue( %this.getNodeNormal() );
		RiverEditorOptionsWindow-->CurrentWidth.setActive( true );
		RiverEditorOptionsWindow-->CurrentWidth.setValue( %this.getNodeWidth() );
		RiverEditorOptionsWindow-->CurrentWidthSlider.setActive( true );
		RiverEditorOptionsWindow-->CurrentWidthSlider.setValue( %this.getNodeWidth() );
		RiverEditorOptionsWindow-->CurrentDepth.setActive( true );
		RiverEditorOptionsWindow-->CurrentDepth.setValue( %this.getNodeDepth() );
		RiverEditorOptionsWindow-->CurrentDepthSlider.setActive( true );
		RiverEditorOptionsWindow-->CurrentDepthSlider.setValue( %this.getNodeDepth() );
	}

	RiverManager.onNodeSelected(%nodeIdx,true);
}

function RiverEditorGui::onNodeModified( %this, %nodeIdx ) {
	devLog("RiverEditorGui::onNodeModified( %this, %nodeIdx )",%nodeIdx);
	RiverEditorOptionsWindow-->position.setValue( %this.getNodePosition() );
	RiverEditorOptionsWindow-->rotation.setValue( %this.getNodeNormal() );
	RiverEditorOptionsWindow-->width.setValue( %this.getNodeWidth() );
	RiverEditorOptionsWindow-->depth.setValue( %this.getNodeDepth() );
	RiverManager.onNodeModified(%nodeIdx);
}

function RiverEditorGui::editNodeDetails( %this ) {
	%this.setNodePosition( RiverEditorOptionsWindow-->position.getText() );
	%this.setNodeNormal( RiverEditorOptionsWindow-->rotation.getText() );
	%this.setNodeWidth( RiverEditorOptionsWindow-->width.getText() );
	%this.setNodeDepth( RiverEditorOptionsWindow-->depth.getText() );
}



function RiverTreeView::onSelect(%this, %obj) {
	RiverEditorGui.road = %obj;
	RiverInspector.inspect( %obj );

	if(%obj != RiverEditorGui.getSelectedRiver()) {
		RiverEditorGui.setSelectedRiver( %obj );
	}
}

function RiverEditorGui::prepSelectionMode( %this ) {
	%mode = %this.getMode();

	if ( %mode $= "RiverEditorAddNodeMode"  ) {
		if ( isObject( %this.getSelectedRiver() ) )
			%this.deleteNode();
	}

	%this.setMode( "RiverEditorSelectMode" );
	LabPaletteArray-->RiverEditorSelectMode.setStateOn(1);
}

//------------------------------------------------------------------------------
function ERiverEditorSelectModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
	RiverManager.toolMode = "Select";
}

function ERiverEditorAddModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
	RiverManager.toolMode = "Add";
}

function ERiverEditorMoveModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
	RiverManager.toolMode = "Move";
}

function ERiverEditorRotateModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
	RiverManager.toolMode = "Rotate";
}

function ERiverEditorScaleModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
	RiverManager.toolMode = "Scale";
}

function ERiverEditorInsertModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
	RiverManager.toolMode = "Insert";
}

function ERiverEditorRemoveModeBtn::onClick(%this) {
	EditorGuiStatusBar.setInfo(%this.ToolTip);
	RiverManager.toolMode = "Remove";
}

function RiverDefaultWidthSliderCtrlContainer::onWake(%this) {
	RiverDefaultWidthSliderCtrlContainer-->slider.setValue(RiverDefaultWidthTextEditContainer-->textEdit.getText());
}

function RiverDefaultDepthSliderCtrlContainer::onWake(%this) {
	RiverDefaultDepthSliderCtrlContainer-->slider.setValue(RiverDefaultDepthTextEditContainer-->textEdit.getText());
}