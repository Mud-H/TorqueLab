//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function EditorIsActive() {
	return ( isObject(EditorGui) && Canvas.getContent() == EditorGui.getId() );
}

function GuiEditorIsActive() {
	return ( isObject(GuiEditorGui) && Canvas.getContent() == GuiEditorGui.getId() );
}

function startToolTime(%tool) {
	if($toolDataToolCount $= "")
		$toolDataToolCount = 0;

	if($toolDataToolEntry[%tool] !$= "true") {
		$toolDataToolEntry[%tool] = "true";
		$toolDataToolList[$toolDataToolCount] = %tool;
		$toolDataToolCount++;
		$toolDataClickCount[%tool] = 0;
	}

	$toolDataStartTime[%tool] = getSimTime();
	$toolDataClickCount[%tool]++;
}

function stopToolTime(%tool) {
	%startTime = 0;

	if($toolDataStartTime[%tool] !$= "")
		%startTime = $toolDataStartTime[%tool];

	if($toolDataTotalTime[%tool] $= "")
		$toolDataTotalTime[%tool] = 0;

	$toolDataTotalTime[%tool] += getSimTime() - %startTime;
}

function dumpToolData() {
	%count = $toolDataToolCount;

	for(%i=0; %i<%count; %i++) {
		%tool = $toolDataToolList[%i];
		%totalTime = $toolDataTotalTime[%tool];

		if(%totalTime $= "")
			%totalTime = 0;

		%clickCount = $toolDataClickCount[%tool];
		echo("---");
		echo("Tool: " @ %tool);
		echo("Time (seconds): " @ %totalTime / 1000);
		echo("Activated: " @ %clickCount);
		echo("---");
	}
}
