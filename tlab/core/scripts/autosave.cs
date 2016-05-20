//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
// Handle the escape bind
function scSceneAutoSaveDelayEdit::onValidate( %this ) {
	%time = %this.getText();

	if (!strIsNumeric(%time))
		return;

	%newDelay = mFloatLength(%time,1);

	if (%newDelay $= $Cfg_TLab_AutoSaveDelay)
		return;

	$Cfg_TLab_AutoSaveDelay = %newDelay;
	info("Autosaving delay changed to",$Cfg_TLab_AutoSaveDelay,"minutes");
	Lab.SetAutoSave();
}
//------------------------------------------------------------------------------

//==============================================================================
// Handle the escape bind
function scSceneAutoSaveCheck::onClick( %this ) {	
	Lab.SetAutoSave();
	if ($Cfg_TLab_AutoSaveEnabled)
		info("Autosaving enabled! Saving each",$Cfg_TLab_AutoSaveDelay,"minutes!");	
}
//------------------------------------------------------------------------------
//==============================================================================
// Handle the escape bind
function Lab::SetAutoSave( %this ) {
	if ($Cfg_TLab_AutoSaveEnabled) {
		if ($Cfg_TLab_AutoSaveDelay $= "" || !strIsNumeric($Cfg_TLab_AutoSaveDelay))
			$Cfg_TLab_AutoSaveDelay = "1";

		cancel($LabAutoSaveSchedule);
		$LabAutoSaveSchedule = Lab.schedule($Cfg_TLab_AutoSaveDelay * 60000,"AutoSaveScene");
		//$LabAutoSaveSchedule = Lab.schedule(Lab.autoSaveDelay * 60000,"AutoSaveScene");
		info("Autosaving enabled! Saving each",$Cfg_TLab_AutoSaveDelay,"minutes!");
	} else {
		//cancel($LabAutoSaveSchedule);
		//delObj($LabAutoSaveSchedule);
		info("Autosaving disabled");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Handle the escape bind
function Lab::AutoSaveScene( %this ) {
	if (!EditorGui.isAwake())
		return;

	%file = $Server::MissionFile@".bak";
	
	MissionGroup.save(%file);
//	Lab.SaveMission(true);
	info("Scene has been autosaved to:",%file);
	Lab.SetAutoSave();
}
//------------------------------------------------------------------------------