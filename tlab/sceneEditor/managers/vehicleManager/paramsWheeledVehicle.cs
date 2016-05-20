//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_ScatterSkyManager.buildParams();
function sepVM::buildWheeledParams( %this ) {
	%arCfg = Lab.createBaseParamsArray("sepVM_WheeledVehicle",sepVM_WheeledVehicleData);
	%arCfg.updateFunc = "sepVM.updateParam";	
	%arCfg.useNewSystem = true;
	%arCfg.noDefaults = true;
	%arCfg.manualObj = Wheeled_Vehicle_vm_clone;
	%arCfg.noDirectSync = true;
	%arCfg.group[%gid++] = "General" TAB "Stack StackA";
	%arCfg.setVal("shapeFile",  "shapeFile" TAB "FileSelect" TAB "" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("superClass",  "superClass" TAB "TextEdit" TAB "" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("wheelDrive",  "wheelDrive" TAB "TextEdit" TAB "" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.group[%gid++] = "Performance And Driving" TAB "Stack StackA";
	%arCfg.setVal("engineTorque",  "engineTorque" TAB "SliderEdit" TAB "range>>1000 20000;;tickAt>>10" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("engineBrake",  "engineBrake" TAB "SliderEdit" TAB "range>>0 5000;;tickAt>>10" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("maxWheelSpeed",  "maxWheelSpeed" TAB "SliderEdit" TAB "range>>0 200;;tickAt>>1" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("brakeTorque",  "brakeTorque" TAB "SliderEdit" TAB "range>>0 5000;;tickAt>>10" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("maxSteeringAngle",  "jetEnergyDrain" TAB "SliderEdit" TAB "range>>0 1;;tickAt>>0.001" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.group[%gid++] = "Nitro boost (Turbo)" TAB "Stack StackA";
	%arCfg.setVal("jetForce",  "Boost force" TAB "SliderEdit" TAB "range>>0 10000;;tickAt>>10" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("jetEnergyDrain",  "Nitro draining" TAB "SliderEdit" TAB "range>>0 10;;tickAt>>0.1" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("minJetEnergy",  "Min. Nitro" TAB "SliderEdit" TAB "range>>0 100;;tickAt>>0.1" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("maxEnergy",  "Nitro tank capacity" TAB "SliderEdit" TAB "range>>0 200;;tickAt>>1" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.group[%gid++] = "Physic" TAB "Stack StackA";
	%arCfg.setVal("massCenter",  "massCenter" TAB "TextEdit" TAB "" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("bodyRestitution",  "bodyRestitution" TAB "SliderEdit" TAB "range>>0 1;;tickAt>>0.01" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("bodyFriction",  "bodyFriction" TAB "SliderEdit" TAB "range>>0 1;;tickAt>>0.01" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("integration",  "integration" TAB "SliderEdit" TAB "range>>1 128;;tickAt>>1" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("mass",  "mass" TAB "SliderEdit" TAB "range>>0 5000;;tickAt>>10" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("drag",  "drag" TAB "SliderEdit" TAB "range>>0 5;;tickAt>>0.05" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.group[%gid++] = "Impact and damage" TAB "Stack StackB";
	%arCfg.setVal("minImpactSpeed",  "minImpactSpeed" TAB "SliderEdit" TAB "range>>0 200;;tickAt>>1" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("softImpactSpeed",  "softImpactSpeed" TAB "SliderEdit" TAB "range>>0 200;;tickAt>>1" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("hardImpactSpeed",  "hardImpactSpeed" TAB "SliderEdit" TAB "range>>0 200;;tickAt>>1" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("contactTol",  "contactTol" TAB "SliderEdit" TAB "range>>0 2;;tickAt>>0.01" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	$sepVM_EngineSFXList = "cheetahEngine";
	$sepVM_ImpactSFXList = "softImpact hardImpact";
	$sepVM_EmitterList = "Bullet350TireEmitter CheetahTireEmitter";
	%arCfg.group[%gid++] = "Sounds and FX" TAB "Stack StackB";
	%arCfg.setVal("engineSound",  "engineSound" TAB "Dropdown" TAB "itemList>>$sepVM_EngineSFXList" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("tireEmitter",  "tireEmitter" TAB "Dropdown" TAB "itemList>>$sepVM_EmitterList" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("dustEmitter",  "dustEmitter" TAB "Dropdown" TAB "itemList>>$sepVM_EmitterList" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("softImpactSound",  "softImpactSound" TAB "Dropdown" TAB "itemList>>$sepVM_ImpactSFXList" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("hardImpactSound",  "hardImpactSound" TAB "Dropdown" TAB "itemList>>$sepVM_ImpactSFXList" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.group[%gid++] = "Camera" TAB "Stack StackB";
	%arCfg.setVal("cameraRoll",  "cameraRoll" TAB "SliderEdit" TAB "range>>0 5;;tickAt>>0.1" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("cameraLag",  "cameraLag" TAB "SliderEdit" TAB "range>>0 5;;tickAt>>0.1" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("cameraDecay",  "cameraDecay" TAB "SliderEdit" TAB "range>>0 5;;tickAt>>0.1" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("cameraOffset",  "cameraOffset" TAB "SliderEdit" TAB "range>>0 20;;tickAt>>0.1" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("cameraMaxDist",  "cameraMaxDist" TAB "SliderEdit" TAB "range>>0 20;;tickAt>>0.1" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("cameraMinDist",  "cameraMinDist" TAB "SliderEdit" TAB "range>>0 20;;tickAt>>0.1" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	buildParamsArray(%arCfg,false);
	sepVM.wheeledParamVehicle = %arCfg;
	%arCfg = Lab.createBaseParamsArray("sepVM_WheeledWheels1",sepVM_WheelAndSpringData1);
	%arCfg.updateFunc = "sepVM.updateParam";
	%arCfg.style = "StyleA";
	%arCfg.useNewSystem = true;
	%arCfg.noDefaults = true;
	%arCfg.noDirectSync = true;
	%gid = 0;
	%arCfg.group[%gid++] = "Spring1" TAB "StackB";
	%arCfg.setVal("length",  "length" TAB "SliderEdit" TAB "range>>0 2;;tickAt>>0.05" TAB "Wheeled_Spring1_vm_clone" TAB %gid);
	%arCfg.setVal("force",  "force" TAB "SliderEdit" TAB "range>>0 20000;;tickAt>>10" TAB "Wheeled_Spring1_vm_clone" TAB %gid);
	%arCfg.setVal("damping",  "damping" TAB "SliderEdit" TAB "range>>0 20000;;tickAt>>10" TAB "Wheeled_Spring1_vm_clone" TAB %gid);
	%arCfg.setVal("antiSwayForce",  "antiSwayForce" TAB "SliderEdit" TAB "range>>0 10;;tickAt>>1" TAB "Wheeled_Spring1_vm_clone" TAB %gid);
	%arCfg.group[%gid++] = "Tire1" TAB "StackA";
	%arCfg.setVal("shapeFile",  "shapeFile" TAB "FileSelect" TAB "" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("staticFriction",  "staticFriction" TAB "SliderEdit" TAB "range>>0 10;;tickAt>>0.05" TAB "Wheeled_Tire1_vm_clone" TAB %gid);
	%arCfg.setVal("kineticFriction",  "kineticFriction" TAB "SliderEdit" TAB "range>>0 10;;tickAt>>0.05" TAB "Wheeled_Tire1_vm_clone" TAB %gid);
	%arCfg.setVal("lateralForce",  "lateralForce" TAB "SliderEdit" TAB "range>>0 50000;;tickAt>>10" TAB "Wheeled_Tire1_vm_clone" TAB %gid);
	%arCfg.setVal("lateralDamping",  "lateralDamping" TAB "SliderEdit" TAB "range>>0 25000;;tickAt>>10" TAB "Wheeled_Tire1_vm_clone" TAB %gid);
	%arCfg.setVal("lateralRelaxation",  "lateralRelaxation" TAB "SliderEdit" TAB "range>>0 10;;tickAt>>1" TAB "Wheeled_Tire1_vm_clone" TAB %gid);
	%arCfg.setVal("longitudinalForce",  "longitudinalForce" TAB "SliderEdit" TAB "range>>0 50000;;tickAt>>10" TAB "Wheeled_Tire1_vm_clone" TAB %gid);
	%arCfg.setVal("longitudinalDamping",  "longitudinalDamping" TAB "SliderEdit" TAB "range>>0 25000;;tickAt>>10" TAB "Wheeled_Tire1_vm_clone" TAB %gid);
	%arCfg.setVal("longitudinalRelaxation",  "longitudinalRelaxation" TAB "SliderEdit" TAB "range>>0 10;;tickAt>>1" TAB "Wheeled_Tire1_vm_clone" TAB %gid);
	buildParamsArray(%arCfg,false);
	sepVM.wheeledParamWheel1 = %arCfg;
	%arCfg = Lab.createBaseParamsArray("sepVM_WheeledWheels2",sepVM_WheelAndSpringData2);
	%arCfg.updateFunc = "sepVM.updateParam";
	%arCfg.style = "StyleA";
	%arCfg.useNewSystem = true;
	%arCfg.noDefaults = true;
	%arCfg.noDirectSync = true;
	%gid = 0;
	%arCfg.group[%gid++] = "Spring2" TAB "StackB";
	%arCfg.setVal("length",  "length" TAB "SliderEdit" TAB "range>>0 2;;tickAt>>0.05" TAB "Wheeled_Spring2_vm_clone" TAB %gid);
	%arCfg.setVal("force",  "force" TAB "SliderEdit" TAB "range>>0 20000;;tickAt>>10" TAB "Wheeled_Spring2_vm_clone" TAB %gid);
	%arCfg.setVal("damping",  "damping" TAB "SliderEdit" TAB "range>>0 20000;;tickAt>>10" TAB "Wheeled_Spring2_vm_clone" TAB %gid);
	%arCfg.setVal("antiSwayForce",  "antiSwayForce" TAB "SliderEdit" TAB "range>>0 10;;tickAt>>1" TAB "Wheeled_Spring2_vm_clone" TAB %gid);
	%arCfg.group[%gid++] = "Tire2" TAB "StackA";
	%arCfg.setVal("shapeFile",  "shapeFile" TAB "FileSelect" TAB "" TAB "Wheeled_Vehicle_vm_clone" TAB %gid);
	%arCfg.setVal("staticFriction",  "staticFriction" TAB "SliderEdit" TAB "range>>0 10;;tickAt>>0.05" TAB "Wheeled_Tire2_vm_clone" TAB %gid);
	%arCfg.setVal("kineticFriction",  "kineticFriction" TAB "SliderEdit" TAB "range>>0 10;;tickAt>>0.05" TAB "Wheeled_Tire2_vm_clone" TAB %gid);
	%arCfg.setVal("lateralForce",  "lateralForce" TAB "SliderEdit" TAB "range>>0 50000;;tickAt>>10" TAB "Wheeled_Tire2_vm_clone" TAB %gid);
	%arCfg.setVal("lateralDamping",  "lateralDamping" TAB "SliderEdit" TAB "range>>0 25000;;tickAt>>10" TAB "Wheeled_Tire2_vm_clone" TAB %gid);
	%arCfg.setVal("lateralRelaxation",  "lateralRelaxation" TAB "SliderEdit" TAB "range>>0 10;;tickAt>>1" TAB "Wheeled_Tire2_vm_clone" TAB %gid);
	%arCfg.setVal("longitudinalForce",  "longitudinalForce" TAB "SliderEdit" TAB "range>>0 50000;;tickAt>>10" TAB "Wheeled_Tire2_vm_clone" TAB %gid);
	%arCfg.setVal("longitudinalDamping",  "longitudinalDamping" TAB "SliderEdit" TAB "range>>0 25000;;tickAt>>10" TAB "Wheeled_Tire2_vm_clone" TAB %gid);
	%arCfg.setVal("longitudinalRelaxation",  "longitudinalRelaxation" TAB "SliderEdit" TAB "range>>0 10;;tickAt>>1" TAB "Wheeled_Tire2_vm_clone" TAB %gid);
	buildParamsArray(%arCfg,false);
	sepVM.wheeledParamWheel2 = %arCfg;
}
//------------------------------------------------------------------------------
