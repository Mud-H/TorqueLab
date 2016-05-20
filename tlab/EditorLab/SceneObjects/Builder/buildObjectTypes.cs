//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function ObjectBuilderGui::buildBotSpawn(%this) {
	%this.objectClassName = "BotSpawnSphere";
	%this.addField("dataBlock",    "TypeDataBlock", "dataBlock",   "MissionMarkerData BotSpawnMarker");
	%this.addField("radius",       "TypeFloat",     "Radius",        1);
	%this.addField("sphereWeight", "TypeFloat",     "Sphere Weight", 1);
	%this.addField("spawnClass",     "TypeString",    "Spawn Class", "Player");
	%this.addField("spawnDatablock", "TypeDataBlock", "Spawn Data", "PlayerData DefaultPlayerData");

	if( Scene.getActiveSimGroup().getID() == MissionGroup.getID() ) {
		if( !isObject("BotStuff") )
			MissionGroup.add( new SimGroup("BotStuff") );

		if( !isObject("BotSpawns") )
			BotsStuff.add( new SimGroup("BotSpawns") );

		Scene.setActiveSimGroup("BotSpawns");
	}

	%this.process();
}

function ObjectBuilderGui::buildBotGoal(%this) {
	%this.objectClassName = "BotGoalPoint";
	%this.addField("dataBlock",    "TypeDataBlock", "dataBlock",   "MissionMarkerData BotGoalMarker");

	if( Scene.getActiveSimGroup().getID() == MissionGroup.getID() ) {
		if( !isObject("BotStuff") )
			MissionGroup.add( new SimGroup("BotStuff") );

		if( !isObject("BotGoals") )
			BotsStuff.add( new SimGroup("BotGoals") );

		Scene.setActiveSimGroup("BotSpawns");
	}

	%this.process();
}

function ObjectBuilderGui::buildPlayerDropPoint(%this) {
	%this.objectClassName = "SpawnSphere";
	%this.addField("dataBlock",    "TypeDataBlock", "dataBlock",   "MissionMarkerData SpawnSphereMarker");
	%this.addField("radius",       "TypeFloat",     "Radius",        1);
	%this.addField("sphereWeight", "TypeFloat",     "Sphere Weight", 1);
	%this.addField("spawnClass",     "TypeString",    "Spawn Class", "Player");
	%this.addField("spawnDatablock", "TypeDataBlock", "Spawn Data", "PlayerData DefaultPlayerData");

	if( Scene.getActiveSimGroup().getID() == MissionGroup.getID() ) {
		if( !isObject("Spawnpoints") )
			MissionGroup.add( new SimGroup("Spawnpoints") );

		Scene.setActiveSimGroup("Spawnpoints");
	}

	%this.process();
}

function ObjectBuilderGui::buildObserverDropPoint(%this) {
	%this.objectClassName = "SpawnSphere";
	%this.addField("dataBlock",    "TypeDataBlock", "dataBlock",   "MissionMarkerData SpawnSphereMarker");
	%this.addField("radius",       "TypeFloat",     "Radius",        1);
	%this.addField("sphereWeight", "TypeFloat",     "Sphere Weight", 1);
	%this.addField("spawnClass",     "TypeString",    "Spawn Class", "Camera");
	%this.addField("spawnDatablock", "TypeDataBlock", "Spawn Data", "CameraData Observer");

	if( Scene.getActiveSimGroup().getID() == MissionGroup.getID() ) {
		if( !isObject("ObserverDropPoints") )
			MissionGroup.add( new SimGroup("ObserverDropPoints") );

		Scene.setActiveSimGroup("ObserverDropPoints");
	}

	%this.process();
}
//------------------------------------------------------------------------------
// This function is used for objects that don't require any special
// fields/functionality when being built
//------------------------------------------------------------------------------
function ObjectBuilderGui::buildObject(%this, %className) {
	%this.objectClassName = %className;
	%this.process();
}

//------------------------------------------------------------------------------
// Environment
//------------------------------------------------------------------------------

function ObjectBuilderGui::buildScatterSky( %this, %dontWarnAboutSun ) {
	if( !%dontWarnAboutSun ) {
		// Check for sun object already in the level.  If there is one,
		// warn the user.
		initContainerTypeSearch( $TypeMasks::EnvironmentObjectType );

		while( 1 ) {
			%object = containerSearchNext();

			if( !%object )
				break;

			if( %object.isMemberOfClass( "Sun" ) ) {
				LabMsgYesNo( "Warning",
								 "A ScatterSky object will conflict with the Sun object that is already in the level." SPC
								 "Do you still want to create a ScatterSky object?",
								 %this @ ".buildScatterSky( true );" );
				return;
			}
		}
	}

	%this.objectClassName = "ScatterSky";
	%this.addField("rayleighScattering", "TypeFloat", "Rayleigh Scattering",  "0.0035");
	%this.addField("mieScattering", "TypeFloat", "Mie Scattering", "0.0045");
	%this.addField("skyBrightness", "TypeFloat", "Sky Brightness", "25");
	%this.process();
	// This is a trick... any fields added after process won't show
	// up as controls, but will be applied to the created object.
	%this.addField( "flareType", "TypeLightFlareDataPtr", "Flare", "ScatterSkyFlareExample" );
	%this.addField( "moonMat", "TypeMaterialName", "Moon Material", "Moon_Glow_Mat" );
	%this.addField( "nightCubemap", "TypeCubemapName", "Night Cubemap", "NightCubemap" );
	%this.addField( "useNightCubemap", "TypeBool", "Use Night Cubemap", "true" );
}

function ObjectBuilderGui::buildCloudLayer(%this) {
	OBObjectName.setValue( "" );
	%this.objectClassName = "CloudLayer";
	%this.addField( "texture", "TypeImageFilename", "Texture", "art/textures/skies/clouds/clouds_normal_displacement" );
	%this.process();
}

function ObjectBuilderGui::buildBasicClouds(%this) {
	OBObjectName.setValue( "" );
	%this.objectClassName = "BasicClouds";
	%this.process();
	// This is a trick... any fields added after process won't show
	// up as controls, but will be applied to the created object.
	%this.addField( "texture[0]", "TypeImageFilename", "Texture", "art/textures/skies/clouds/cloud1" );
	%this.addField( "texture[1]", "TypeImageFilename", "Texture", "art/textures/skies/clouds/cloud2" );
	%this.addField( "texture[2]", "TypeImageFilename", "Texture", "art/textures/skies/clouds/cloud3" );
}

function ObjectBuilderGui::checkExists( %this, %classname ) {
	for ( %i = 0; %i < SceneEd.getActiveSimGroup().getCount(); %i++ ) {
		%obj = SceneEd.getActiveSimGroup().getObject( %i );

		if ( %obj.getClassName() $= %classname )
			return true;
	}

	return false;
}

function ObjectBuilderGui::buildsgMissionLightingFilter(%this) {
	%this.objectClassName = "sgMissionLightingFilter";
	%this.addField("dataBlock", "TypeDataBlock", "sgMissionLightingFilter Data", "sgMissionFilterData");
	%this.process();
}

function ObjectBuilderGui::buildsgDecalProjector(%this) {
	%this.objectClassName = "sgDecalProjector";
	%this.addField("dataBlock", "TypeDataBlock", "DecalData Data", "DecalData");
	%this.process();
}

function ObjectBuilderGui::buildsgLightObject(%this) {
	%this.objectClassName = "sgLightObject";
	%this.addField("dataBlock", "TypeDataBlock", "LightObject Data", "sgLightObjectData");
	%this.process();
}

function ObjectBuilderGui::buildSun( %this, %dontWarnAboutScatterSky ) {
	if( !%dontWarnAboutScatterSky ) {
		// Check for scattersky object already in the level.  If there is one,
		// warn the user.
		initContainerTypeSearch( $TypeMasks::EnvironmentObjectType );

		while( 1 ) {
			%object = containerSearchNext();

			if( !%object )
				break;

			if( %object.isMemberOfClass( "ScatterSky" ) ) {
				LabMsgYesNo( "Warning",
								 "A Sun object will conflict with the ScatterSky object that is already in the level." SPC
								 "Do you still want to create a Sun object?",
								 %this @ ".buildSun( true );" );
				return;
			}
		}
	}

	%this.objectClassName = "Sun";
	%this.addField("direction", "TypeVector", "Direction", "1 1 -1");
	%this.addField("color", "TypeColor", "Sun color", "0.8 0.8 0.8");
	%this.addField("ambient", "TypeColor", "Ambient color", "0.2 0.2 0.2");
	%this.process();
	// This is a trick... any fields added after process won't show
	// up as controls, but will be applied to the created object.
	%this.addField( "coronaMaterial", "TypeMaterialName", "Corona Material", "Corona_Mat" );
	%this.addField( "flareType", "TypeLightFlareDataPtr", "Flare", "SunFlareExample" );
}

function ObjectBuilderGui::buildLightning(%this) {
	%this.objectClassName = "Lightning";
	%this.addField("dataBlock", "TypeDataBlock", "Data block", "LightningData DefaultStorm");
	%this.process();
}

function ObjectBuilderGui::addWaterObjectFields(%this) {
	%this.addField("rippleDir[0]", "TypePoint2", "Ripple Direction", "0.000000 1.000000");
	%this.addField("rippleDir[1]", "TypePoint2", "Ripple Direction", "0.707000 0.707000");
	%this.addField("rippleDir[2]", "TypePoint2", "Ripple Direction", "0.500000 0.860000");
	%this.addField("rippleTexScale[0]", "TypePoint2", "Ripple Texture Scale", "7.140000 7.140000");
	%this.addField("rippleTexScale[1]", "TypePoint2", "Ripple Texture Scale", "6.250000 12.500000");
	%this.addField("rippleTexScale[2]", "TypePoint2", "Ripple Texture Scale", "50.000000 50.000000");
	%this.addField("rippleSpeed[0]", "TypeFloat", "Ripple Speed", "0.065");
	%this.addField("rippleSpeed[1]", "TypeFloat", "Ripple Speed", "0.09");
	%this.addField("rippleSpeed[2]", "TypeFloat", "Ripple Speed", "0.04");
	%this.addField("rippleMagnitude[0]", "TypeFloat", "Ripple Magnitude", "1.0");
	%this.addField("rippleMagnitude[1]", "TypeFloat", "Ripple Magnitude", "1.0");
	%this.addField("rippleMagnitude[2]", "TypeFloat", "Ripple Magnitude", "0.3");
	%this.addField("overallRippleMagnitude", "TypeFloat", "Overall Ripple Magnitude", "1.0");
	%this.addField("waveDir[0]", "TypePoint2", "Wave Direction", "0.000000 1.000000");
	%this.addField("waveDir[1]", "TypePoint2", "Wave Direction", "0.707000 0.707000");
	%this.addField("waveDir[2]", "TypePoint2", "Wave Direction", "0.500000 0.860000");
	%this.addField("waveMagnitude[0]", "TypePoint2", "Wave Magnitude", "0.2");
	%this.addField("waveMagnitude[1]", "TypePoint2", "Wave Magnitude", "0.2");
	%this.addField("waveMagnitude[2]", "TypePoint2", "Wave Magnitude", "0.2");
	%this.addField("waveSpeed[0]", "TypeFloat", "Wave Speed", "1");
	%this.addField("waveSpeed[1]", "TypeFloat", "Wave Speed", "1");
	%this.addField("waveSpeed[2]", "TypeFloat", "Wave Speed", "1");
	%this.addField("overallWaveMagnitude", "TypeFloat", "Overall Wave Magnitude", "1.0");
	%this.addField("rippleTex", "TypeImageFilename", "Ripple Texture", "core/art/textures/water/ripple" );
	%this.addField("depthGradientTex", "TypeImageFilename", "Depth Gradient Texture", "core/art/textures/water/depthcolor_ramp" );
	%this.addField("foamTex", "TypeImageFilename", "Foam Texture", "core/art/textures/water/foam" );
}

function ObjectBuilderGui::buildWaterBlock(%this) {
	%this.objectClassName = "WaterBlock";
	%this.addField( "baseColor", "TypeColorI", "Base Color", "45 108 171 255" );
	%this.process();
	// This is a trick... any fields added after process won't show
	// up as controls, but will be applied to the created object.
	%this.addWaterObjectFields();
}

function ObjectBuilderGui::buildWaterPlane(%this) {
	%this.objectClassName = "WaterPlane";
	%this.addField( "baseColor", "TypeColorI", "Base Color", "45 108 171 255" );
	%this.process();
	// This is a trick... any fields added after process won't show
	// up as controls, but will be applied to the created object.
	%this.addWaterObjectFields();
}

function ObjectBuilderGui::buildTerrainBlock(%this) {
	%this.objectClassName = "TerrainBlock";
	%this.createCallback = "ETerrainEditor.attachTerrain();";
	%this.addField("terrainFile", "TypeFile", "Terrain file", "", "*.ter");
	%this.addField("squareSize", "TypeInt", "Square size", "8");
	%this.process();
}

function ObjectBuilderGui::buildGroundCover( %this ) {
	%this.objectClassName = "GroundCover";
	%this.addField( "material", "TypeMaterialName", "Material Name", "" );
	%this.addField( "shapeFilename[0]", "TypeFile", "Shape File [Optional]", "", "*.*");
	%this.process();
	// This is a trick... any fields added after process won't show
	// up as controls, but will be applied to the created object.
	%this.addField( "probability[0]", "TypeFloat", "Probability", "1" );
}

function ObjectBuilderGui::buildPrecipitation(%this) {
	%this.objectClassName = "Precipitation";
	%this.addField("dataBlock", "TypeDataBlock", "Precipitation data", "PrecipitationData");
	%this.process();
}

function ObjectBuilderGui::buildParticleEmitterNode(%this) {
	%this.objectClassName = "ParticleEmitterNode";
	%this.addField("dataBlock", "TypeDataBlock", "datablock", "ParticleEmitterNodeData");
	%this.addField("emitter",   "TypeDataBlock", "Particle data", "ParticleEmitterData");
	%this.process();
}

function ObjectBuilderGui::buildParticleSimulation(%this) {
	%this.objectClassName = "ParticleSimulation";
	%this.addField("datablock", "TypeDataBlock", "datablock", "ParticleSimulationData");
	%this.process();
}

//------------------------------------------------------------------------------
// Mission
//------------------------------------------------------------------------------

function ObjectBuilderGui::buildTrigger(%this) {
	%this.objectClassName = "Trigger";
	%this.addField("dataBlock", "TypeDataBlock", "Data Block", "TriggerData defaultTrigger");
	%this.addField("polyhedron", "TypeTriggerPolyhedron", "Polyhedron", "-0.5 0.5 0.0 1.0 0.0 0.0 0.0 -1.0 0.0 0.0 0.0 1.0");
	%this.process();
}

function ObjectBuilderGui::buildPhysicalZone(%this) {
	%this.objectClassName = "PhysicalZone";
	%this.addField("polyhedron", "TypeTriggerPolyhedron", "Polyhedron", "-0.5 0.5 0.0 1.0 0.0 0.0 0.0 -1.0 0.0 0.0 0.0 1.0");
	%this.process();
}

function ObjectBuilderGui::buildCamera(%this) {
	%this.objectClassName = "Camera";
	%this.addField("position", "TypePoint3", "Position", "0 0 0");
	%this.addField("rotation", "TypePoint4", "Rotation", "1 0 0 0");
	%this.addField("dataBlock", "TypeDataBlock", "Data block", "CameraData Observer");
	%this.addField("team", "TypeInt", "Team", "0");
	%this.process();
}

function ObjectBuilderGui::buildLevelInfo(%this) {
	if ( %this.checkExists( "LevelInfo" ) ) {
		GenericPromptDialog-->GenericPromptWindow.text = "Warning";
		GenericPromptDialog-->GenericPromptText.text   = "There is already an existing LevelInfo in the scene.";
		Canvas.pushDialog( GenericPromptDialog );
		return;
	}

	OBObjectName.setValue( "theLevelInfo" );
	%this.objectClassName = "LevelInfo";
	%this.process();
}

function ObjectBuilderGui::buildTimeOfDay(%this) {
	if ( %this.checkExists( "TimeOfDay" ) ) {
		GenericPromptDialog-->GenericPromptWindow.text = "Warning";
		GenericPromptDialog-->GenericPromptText.text   = "There is already an existing TimeOfDay in the scene.";
		Canvas.pushDialog( GenericPromptDialog );
		return;
	}

	%this.objectClassName = "TimeOfDay";
	%this.process();
}


//------------------------------------------------------------------------------
// System
//------------------------------------------------------------------------------

function ObjectBuilderGui::buildPhysicsEntity(%this) {
	%this.objectClassName = "PhysicsEntity";
	%this.addField("dataBlock", "TypeDataBlock", "Data block", "PhysicsEntityData");
	%this.process();
}

//------------------------------------------------------------------------------
// Functions to allow scripted/datablock objects to be instantiated
//------------------------------------------------------------------------------

function PhysicsEntityData::create(%data) {
	%obj = new PhysicsEntity() {
		dataBlock = %data;
		parentGroup = Scene.getActiveSimGroup();
	};
	return %obj;
}

function StaticShapeData::create(%data) {
	%obj = new StaticShape() {
		dataBlock = %data;
		parentGroup = Scene.getActiveSimGroup();
	};
	return %obj;
}

function MissionMarkerData::create(%block) {
	switch$(%block) {
	case "WayPointMarker":
		%obj = new WayPoint() {
			dataBlock = %block;
			parentGroup = Scene.getActiveSimGroup();
		};

		return(%obj);
	case "SpawnSphereMarker":
		%obj = new SpawnSphere() {
			datablock = %block;
			parentGroup = Scene.getActiveSimGroup();
		};

		return(%obj);
	default:
		%obj = new WayPoint() {
			dataBlock = %block;
			parentGroup = Scene.getActiveSimGroup();
		};

		return(%obj);
	}

	return(-1);
}

function ItemData::create(%data) {
	%obj = new Item([%data.getName()]) {
		dataBlock = %data;
		parentGroup = Scene.getActiveSimGroup();
		static = true;
		rotate = true;
	};
	return %obj;
}

function TurretShapeData::create(%block) {
	%obj = new TurretShape() {
		dataBlock = %block;
		static = true;
		respawn = true;
		parentGroup = Scene.getActiveSimGroup();
	};
	return %obj;
}

function AITurretShapeData::create(%block) {
	%obj = new AITurretShape() {
		dataBlock = %block;
		static = true;
		respawn = true;
		parentGroup = Scene.getActiveSimGroup();
	};
	return %obj;
}

function WheeledVehicleData::create(%block) {
	%obj = new WheeledVehicle() {
		dataBlock = %block;
		parentGroup = Scene.getActiveSimGroup();
	};
	return %obj;
}

function FlyingVehicleData::create(%block) {
	%obj = new FlyingVehicle() {
		dataBlock = %block;
		parentGroup = Scene.getActiveSimGroup();
	};
	return(%obj);
}

function HoverVehicleData::create(%block) {
	%obj = new HoverVehicle() {
		dataBlock = %block;
		parentGroup = Scene.getActiveSimGroup();
	};
	return(%obj);
}

function RigidShapeData::create(%data) {
	%obj = new RigidShape() {
		dataBlock = %data;
		parentGroup = Scene.getActiveSimGroup();
	};
	return %obj;
}

function PhysicsShapeData::create( %datablock )
{
   %obj = new Px3Shape()
   {
		dataBlock = %datablock;
		parentGroup = EWCreatorWindow.objectGroup;
		
      invulnerable = false;
      damageRadius = 0;
      areaImpulse = 0;
      radiusDamage = 0;
      minDamageAmount = 0;         
   };

   return %obj;
}

function Px3ShapeData::create( %datablock )
{
   %obj = new Px3Shape(){
		dataBlock = %datablock;
		parentGroup = Scene.getActiveSimGroup();
		invulnerable = false;
		damageRadius = 0;
		areaImpulse = 0;
		radiusDamage = 0;
		minDamageAmount = 0;
	};
	return %obj;
}
function RigidPhysicsShapeData::create( %datablock ) {
	%obj = new RigidPhysicsShape() {
		dataBlock = %datablock;
		parentGroup = Scene.getActiveSimGroup();
		invulnerable = false;
		damageRadius = 0;
		areaImpulse = 0;
		radiusDamage = 0;
		minDamageAmount = 0;
	};
	return %obj;
}

function ProximityMineData::create( %datablock ) {
	%obj = new ProximityMine() {
		dataBlock = %dataBlock;
		parentGroup = Scene.getActiveSimGroup();
		static = true;    // mines created by the editor are static, and armed immediately
	};
	return %obj;
}
