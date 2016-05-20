//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function TMG::getCurrentHeight(%this,%id) {
	%avgHeight = ETerrainEditor.lastAverageHeight;
		%value = mFloatLength(%avgHeight,2);
	ETerrainEditor.setHeightVal = %value;
	
	EWTerrainEditToolbar-->setHeight.text = %value;	
}
//------------------------------------------------------------------------------