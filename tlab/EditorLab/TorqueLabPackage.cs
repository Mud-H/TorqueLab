//==============================================================================
// TorqueLab -> TorqueLabPackage Functions Overide
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// The TorqueLab Package is activated when Editor is open and deactivated when closed. 
// You can add any function you want to behave differently while in the editor
//==============================================================================

package TorqueLabPackage {	
	function AIPlayer::npcThink(%this, %obj)
	{	
		if (!$TLPack_npcThinkEchoed)
			info("AIPlayer::npcThink Stopped by TorqueLab. To disable overwrite, comment npcThink function in tlab/EditorLab/TorqueLabPackage.cs");
		$TLPack_npcThinkEchoed = true;
		Lab.npcThinkList = strAddWord(Lab.npcThinkList,%obj.getId(), true);
	}
	function AIPlayer::Think(%this, %obj)
	{
		if (!$TLPack_ThinkEchoed)
			info("AIPlayer::Think Stopped by TorqueLab. To disable overwrite, comment Think function in tlab/EditorLab/TorqueLabPackage.cs");
		$TLPack_ThinkEchoed = true;
		info("AIPlayer::Think Stopped by TorqueLab");
		Lab.ThinkList = strAddWord(Lab.ThinkList,%obj.getId(), true);
	}
	function AIPlayer::openFire(%this, %obj, %tgt, %rtt)
	{
		if (LocalClientConnection.player !$= %tgt)
			Parent::openFire( %obj, %tgt, %rtt);
	}
	
	function PlayerData::damage(%this, %obj, %sourceObject, %position, %damage, %damageType)
	{
		if (isObject(LocalClientConnection.player))
			if (%obj.getId() $= LocalClientConnection.player.getId())
				return;
				
		Parent::damage(%obj, %sourceObject, %position, %damage, %damageType);
	}
	
	function serverCmdTogglePathCamera(%client, %val)
	{
		Lab.TogglePathCamera(%client);		
	}
	function serverCmdToggleCamera(%client)
	{
		Lab.ToggleCamera(%client);		
	}

	function serverCmdSetEditorCameraPlayer(%client)
	{
		Lab.SetEditorCameraPlayer(%client);		
	}

	function serverCmdSetEditorCameraPlayerThird(%client)
	{
		Lab.SetEditorCameraPlayerThird(%client);		
	}

	function serverCmdDropPlayerAtCamera(%client)
	{
		Lab.DropPlayerAtCamera(%client);		
	}

	function serverCmdDropCameraAtPlayer(%client)
	{
		Lab.DropCameraAtPlayer(%client);		
	}

	function serverCmdCycleCameraFlyType(%client)
	{
		Lab.CycleCameraFlyType(%client);
		
	}

	function serverCmdSetEditorCameraStandard(%client)
	{
		Lab.SetEditorCameraStandard(%client);
	}

	function serverCmdSetEditorCameraNewton(%client)
	{
		Lab.SetEditorCameraNewton(%client);
	}

	function serverCmdSetEditorCameraNewtonDamped(%client)
	{
		Lab.SetEditorCameraNewtonDamped(%client);
	}

	function serverCmdSetEditorOrbitCamera(%client)
	{
		Lab.SetEditorOrbitCamera(%client);
	}

	function serverCmdSetEditorFlyCamera(%client)
	{
		Lab.SetEditorFlyCamera(%client);
	}

	function serverCmdEditorOrbitCameraSelectChange(%client, %size, %center)
	{
		Lab.EditorOrbitCameraChange(%client, %size, %center);
	}

	function serverCmdEditorCameraAutoFit(%client, %radius)
	{
		Lab.CameraAutoFit(%client,%radius);
	}
	
	function EditorDoExitMission(%saveFirst) {
		Lab.DoExitMission(%saveFirst);
	}

};

package EditorDisconnectOverride {
	function disconnect() {
		if ( isObject( Editor ) && Editor.isEditorEnabled() ) {
			if (isObject( $Cfg_TLab_defaultGui ))
				Editor.close($Cfg_TLab_defaultGui);
		}

		Parent::disconnect();
	}
};
activatePackage( EditorDisconnectOverride );
