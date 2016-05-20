
//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_ScatterSkyManager.buildParams();
function DbEd::buildPlayerDataParams( %this ) {
	%arCfg = Lab.createBaseParamsArray("DbEd_PlayerDataData",DbEd_EditorStack);
	%arCfg.updateFunc = "DbEd.updateParam";
	%arCfg.style = "StyleA";
	%arCfg.useNewSystem = true;
	%arCfg.noDefaults = true;
	%arCfg.noDirectSync = true;
	%arCfg.group[%gid++] = "Movement";
	%arCfg.setVal("runForce",  "runForce" TAB "SliderEdit" TAB "range>>0 1000;;tickAt>>1" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("maxForwardSpeed",  "maxForwardSpeed" TAB "SliderEdit" TAB "range>>0 10;;tickAt>>0.1" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("maxBackwardSpeed",  "maxBackwardSpeed" TAB "SliderEdit" TAB "range>>0 10;;tickAt>>0.1" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("maxSideSpeed",  "maxSideSpeed" TAB "SliderEdit" TAB "range>>0 10;;tickAt>>0.1" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("runSurfaceAngle",  "runSurfaceAngle" TAB "SliderEdit" TAB "range>>0 90;;tickAt>>1" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("sprintForce",  "sprintForce" TAB "SliderEdit" TAB "range>>0 1000;;tickAt>>1" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("maxSprintForwardSpeed",  "maxSprintForwardSpeed" TAB "SliderEdit" TAB "range>>0 20;;tickAt>>0.1" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("maxSprintBackwardSpeed",  "maxSprintBackwardSpeed" TAB "SliderEdit" TAB "range>>0 20;;tickAt>>0.1" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("maxSprintSideSpeed",  "maxSprintSideSpeed" TAB "SliderEdit" TAB "range>>0 20;;tickAt>>0.1" TAB "DbEd.activeDatablock" TAB %gid);
	buildParamsArray(%arCfg,true);
	DbEd.currentParam = %arCfg;
}
//------------------------------------------------------------------------------
/*


	 runForce = "4320";
   maxForwardSpeed = "8";
   maxBackwardSpeed = "6";
   maxSideSpeed = "6";
   runSurfaceAngle = "38";

    sprintForce = "4320";
   maxSprintForwardSpeed = "14";
   maxSprintBackwardSpeed = "8";
   maxSprintSideSpeed = "6";

airControl = "0.7";
 horizMaxSpeed = "68";
   horizResistSpeed = "33";
   horizResistFactor = "0.35";
   upResistSpeed = "25";
   upResistFactor = "0.3";
   jumpForce = "1400";
   minJumpSpeed = "20";
   maxJumpSpeed = "50";
   jumpSurfaceAngle = "80";
   jumpDelay = "5";

pickupRadius = "1";
   maxTimeScale = "1";
   renderFirstPerson = "0";
   firstPersonShadows = "1";
   maxLookAngle = "0.9";
   maxStepHeight = "0.35";

   minImpactSpeed = "10";
   minLateralImpactSpeed = "20";



   swimForce = "4320";
   maxUnderwaterForwardSpeed = "8.4";
   maxUnderwaterBackwardSpeed = "7.8";
   maxUnderwaterSideSpeed = "4";
   maxCrouchBackwardSpeed = "2";
   maxCrouchSideSpeed = "2";
   fallingSpeedThreshold = "-6";
   recoverDelay = "0";
   recoverRunForceScale = "0";
   landSequenceTime = "0.33";
   boundingBox = "0.65 0.75 1.85";
   crouchBoundingBox = "0.65 0.75 1.3";
   swimBoundingBox = "1 2 2";
   boxHeadPercentage = "0.83";
   boxTorsoPercentage = "0.49";
   boxHeadLeftPercentage = "0.3";
   boxHeadRightPercentage = "0.6";
   boxHeadBackPercentage = "0.3";
   boxHeadFrontPercentage = "0.6";
   footPuffEmitter = "LightPuffEmitter";
   footPuffNumParts = "10";
   dustEmitter = "LightPuffEmitter";
   decalOffset = "0.25";
   FootSoftSound = "FootLightSoftSound";
   FootHardSound = "FootLightHardSound";
   FootMetalSound = "FootLightMetalSound";
   FootSnowSound = "FootLightSnowSound";
   FootShallowSound = "FootLightShallowSplashSound";
   FootWadingSound = "FootLightWadingSound";
   FootUnderwaterSound = "FootLightUnderwaterSound";
   Splash = "PlayerSplash";
   splashVelocity = "4";
   splashAngle = "67";
   splashVelEpsilon = "0.6";
   splashEmitter[0] = "PlayerWakeEmitter";
   splashEmitter[1] = "PlayerFoamEmitter";
   splashEmitter[2] = "PlayerBubbleEmitter";
   footstepSplashHeight = "0.35";
   mediumSplashSoundVelocity = "10";
   hardSplashSoundVelocity = "20";
   exitSplashSoundVelocity = "5";
   groundImpactMinSpeed = "45";
   groundImpactShakeFreq = "4 4 4";
   groundImpactShakeAmp = "1 1 1";
   groundImpactShakeDuration = "0.8";
   imageAnimPrefixFP = "soldier";
   shapeNameFP[0] = "art/shapes/actors/Soldier/FP/FP_SoldierArms.DAE";
   allowImageStateAnimation = "1";
   shapeFile = "art/models/Lab/vehicles/SciFi/TankCyber/TankCyberA.DAE";
   Debris = "PlayerDebris";
   debrisShapeName = "art/models/Lab/vehicles/SciFi/TankCyber/TankCyberA.DAE";
   mass = "120";
   drag = "1.3";
   maxDamage = "100";
   repairRate = "0.33";
   cameraMaxDist = "3";
   cameraMinDist = "0";
   cameraDefaultFov = "55";
   cameraMaxFov = "65";
   superClass = "PlayerDataFPS";
   maxInvLurkerGrenadeLauncher = "1";
   observeParameters = "0.5 4.5 4.5";
   mainWeapon = "Ryder";
   canObserve = "1";
   maxInvRyderClip = "10";
   speedDamageScale = "0.4";
   maxInvShotgun = "1";
   maxDrag = "0.4";
   maxInvLurker = "1";
   cmdCategory = "Clients";
   maxInvProxMine = "5";
   maxInvLurkerGrenadeAmmo = "20";
   maxInvShotgunClip = "8";
   maxInvLurkerClip = "20";
   rechargeRate = "0.256";
   energyPerDamagePoint = "75";
   throwForce = "30";
   maxInvDeployableTurret = "5";
   availableSkins = "base	DarkBlue";
   maxInvRyder = "1";
   */
