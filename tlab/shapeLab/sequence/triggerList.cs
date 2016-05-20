//==============================================================================
// TorqueLab -> ShapeLab -> Threads and Animation
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


	
function ShapeLab::initTriggerList( %this ) {
	ShapeLab_TriggerList.triggerId = 1;
	ShapeLab_TriggerList.clear();
	ShapeLab_TriggerList.addRow( -1, "-1" TAB "Frame" TAB "Trigger" TAB "State" );
	ShapeLab_TriggerList.setRowActive( -1, false );
}	
	
function ShapeLab_TriggerList::getTriggerText( %this, %frame, %state ) {
	// First column is invisible and used only for sorting
	%sortKey = ( %frame * 1000 ) + ( mAbs( %state ) * 10 ) + ( ( %state > 0 ) ? 1 : 0 );
	return %sortKey TAB %frame TAB mAbs( %state ) TAB ( ( %state > 0 ) ? "on" : "off" );
}

function ShapeLab_TriggerList::addItem( %this, %frame, %state ) {
	// Add to text list
	%row = %this.addRow( %this.triggerId, %this.getTriggerText( %frame, %state ) );
	%this.sortNumerical( 0, true );
	// Add marker to animation timeline
	%pos = ShapeLabPreview.getTimelineBitmapPos( ShapeLabPreview-->seqIn.getText() + %frame, 2 );
	%ctrl = new GuiBitmapCtrl() {
		internalName = "trigger" @ %this.triggerId;
		Profile = "ToolsDefaultProfile";
		HorizSizing = "right";
		VertSizing = "bottom";
		position = %pos SPC "0";
		Extent = "2 12";
		bitmap = "tlab/shapeLab/images/trigger_marker";
	};
	ShapeLabPreview_TriggerCont.addGuiControl( %ctrl );
	%this.triggerId++;
}

function ShapeLab_TriggerList::removeItem( %this, %frame, %state ) {
	// Remove from text list
	%row = %this.findTextIndex( %this.getTriggerText( %frame, %state ) );

	if ( %row > 0 ) {
		devLog("Row:",%row,"Deleting trigger:",%this.getRowId( %row ));
		eval( "ShapeLabPreview_TriggerCont-->trigger" @ %this.getRowId( %row ) @ ".delete();" );
		%this.removeRow( %row );
	}
}

function ShapeLab_TriggerList::removeAll( %this ) {
	%count = %this.rowCount();

	for ( %row = %count-1; %row > 0; %row-- ) {
		devLog("Row:",%row,"Deleting trigger:",%this.getRowId( %row ));
		eval( "ShapeLabPreview_TriggerCont-->trigger" @ %this.getRowId( %row ) @ ".delete();" );
		%this.removeRow( %row );
	}
}

function ShapeLab_TriggerList::updateItem( %this, %oldFrame, %oldState, %frame, %state ) {
	// Update text list entry
	%oldText = %this.getTriggerText( %oldFrame, %oldState );
	%row = %this.getSelectedRow();

	if ( ( %row <= 0 ) || ( %this.getRowText( %row ) !$= %oldText ) )
		%row = %this.findTextIndex( %oldText );

	if ( %row > 0 ) {
		%updatedId = %this.getRowId( %row );
		%newText = %this.getTriggerText( %frame, %state );
		%this.setRowById( %updatedId, %newText );
		// keep selected row the same
		%selectedId = %this.getSelectedId();
		%this.sortNumerical( 0, true );
		%this.setSelectedById( %selectedId );

		// Update animation timeline marker
		if ( %frame != %oldFrame ) {
			%pos = ShapeLabPreview.getTimelineBitmapPos( ShapeLabPreview-->seqIn.getText() + %frame, 2 );
			eval( "%ctrl = ShapeLabPreview_TriggerCont-->trigger" @ %updatedId @ ";" );
			%ctrl.position = %pos SPC "0";
		}
	}
}

