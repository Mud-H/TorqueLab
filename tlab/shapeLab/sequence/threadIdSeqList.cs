//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// ShapeLab -> First Initialization
//==============================================================================

function ShapeLabThreadViewer::onAddThread( %this ) {
	ShapeLabShapeView.addThread();
	ShapeLab_ThreadIdList.addRow( %this.threadID++, ShapeLab_ThreadIdList.rowCount() );
	//ShapeLab_ThreadIdList.setSelectedRow( ShapeLab_ThreadIdList.rowCount()-1 );
}

function ShapeLabThreadViewer::onRemoveThread( %this ) {
	if ( ShapeLab_ThreadIdList.rowCount() > 1 ) {
		// Remove the selected thread
		%row = ShapeLab_ThreadIdList.getSelectedRow();
		ShapeLabShapeView.removeThread( %row );
		ShapeLab_ThreadIdList.removeRow( %row );
		// Update list (threads are always numbered 0-N)
		%rowCount = ShapeLab_ThreadIdList.rowCount();

		for ( %i = %row; %i < %rowCount; %i++ )
			ShapeLab_ThreadIdList.setRowById( ShapeLab_ThreadSeqList.getRowId( %i ), %i );

		// Select the next thread
		if ( %row >= %rowCount )
			%row = %rowCount - 1;

		ShapeLab_ThreadIdList.setSelectedRow( %row );
	}
}



//==============================================================================
// Thread ID List
//==============================================================================
function ShapeLab_ThreadIdList::onSelect( %this, %row, %text ) {
	devLog("ShapeLab_ThreadIdList::onSelect", %row, %text);
	
	
	

	
	if (ShapeLabShapeView.activeThread $= ShapeLab_ThreadIdList.getSelectedRow())
	{
		devLog("Sequence Thread ID already selected!");
		return;
	}
	ShapeLab.threadIdSeq[ShapeLabShapeView.activeThread] = %seqName;
	if (ShapeLabShapeView.activeThread !$= ShapeLab_ThreadIdList.getSelectedRow())
		ShapeLabShapeView.activeThread = ShapeLab_ThreadIdList.getSelectedRow();
		
	// Select the active thread's sequence in the list
	%seqName = ShapeLabShapeView.getThreadSequence();

	if ( %seqName $= "" )
		%seqName = "<rootpose>";
	else if ( startswith( %seqName, "__proxy__" ) )
		%seqName = ShapeLab.getUnproxyName( %seqName );

	%this.threadSeq[%row] = %seqName;
	%seqIndex = ShapeLab_ThreadSeqList.findTextIndex( %seqName );
	//if (ShapeLab_ThreadSeqList.getSelectedRow() !$= %seqIndex)
		ShapeLab_ThreadSeqList.setSelectedRow( %seqIndex );
	
	
	SL_ActiveThreadInfo-->activeThreadText.setText("Selected Thread Slot:\c1 "@ShapeLabShapeView.activeThread);

	// Update the playback controls
	switch ( ShapeLabShapeView.threadDirection ) {
	case -1:
		ShapeLabPreview-->playBkwdBtn.performClick();

	case 0:
		ShapeLabPreview-->pauseBtn.performClick();

	case 1:
		ShapeLabPreview-->playFwdBtn.performClick();
	}

	ShapeLabToggleButtonValue( ShapeLabPreview-->pingpong, ShapeLabShapeView.threadPingPong );
}

function SLE_ThreadTransToMenu::onSelect( %this, %id, %text ) {
	logd("SLE_ThreadTransToMenu::onSelect", %id, %text);
}
function SLE_ThreadTransStateMenu::onSelect( %this, %id, %text ) {
	logd("SLE_ThreadTransStateMenu::onSelect", %id, %text);
}

//==============================================================================
// Thread ID Sequence List
//==============================================================================
function ShapeLab_ThreadSeqList::onSelect( %this, %row, %text ) {
	devLog("ShapeLab_ThreadSeqList::onSelect", %row, %text);
	devLog("GetSelectedId::onSelect", %this.getSelectedId());
	
	%duration = SLE_ThreadSettings-->transitionTime.getText();
	SL_ActiveThreadInfo-->activeThreadText.setText("Selected Thread Slot:\c1 "@ShapeLabShapeView.activeThread);
	ShapeLabShapeView.setThreadSequence( getField(%text,0), %duration, ShapeLabShapeView.threadPos, 0 );
	ShapeLab.setActiveSequence( getField(%text,0));
	
	if (%this.threadSeq[ShapeLabShapeView.activeThread] !$=  %text){
		devLog(%text,"This is not the sequence set for ActiveThread:",ShapeLabShapeView.activeThread,"Seq",ShapeLabShapeView.getThreadSequence());
	//if (ShapeLab_ThreadIdList.getSelectedId()
	//ShapeLab_ThreadIdList.setSelectedById( %this.getSelectedId() );
	}
	
	
	//ShapeLabPreview.setSequence(%text);
}
