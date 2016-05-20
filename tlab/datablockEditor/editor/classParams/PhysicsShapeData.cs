//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_ScatterSkyManager.buildParams();
function DbEd::buildPhysicsShapeDataParams( %this ) {
	%arCfg = Lab.createBaseParamsArray("DbEd_PhysicsShapeData",DbEd_EditorStack);
	%arCfg.updateFunc = "DbEd.updateParam";
	%arCfg.style = "StyleA";
	%arCfg.useNewSystem = true;
	%arCfg.noDefaults = true;
	%arCfg.noDirectSync = true;
	%arCfg.group[%gid++] = "Physics";
	%arCfg.setVal("mass",  "mass" TAB "SliderEdit" TAB "range>>0 3;;tickAt>>0.01" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("friction",  "friction" TAB "SliderEdit" TAB "range>>0 1.5;;tickAt>>0.01" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("staticFriction",  "staticFriction" TAB "SliderEdit" TAB "range>>0 1.5;;tickAt>>0.01" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("restitution",  "restitution" TAB "SliderEdit" TAB "range>>0 0.95;;tickAt>>0.01" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("linearDamping",  "linearDamping" TAB "SliderEdit" TAB "range>>0 3;;tickAt>>0.01" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("angularDamping",  "angularDamping" TAB "SliderEdit" TAB "range>>0 1.5;;tickAt>>0.01" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("linearSleepThreshold",  "linearSleepThreshold" TAB "SliderEdit" TAB "range>>0 1.5;;tickAt>>0.01" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("angularSleepThreshold",  "angularSleepThreshold" TAB "SliderEdit" TAB "range>>0 0.95;;tickAt>>0.01" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("waterDampingScale",  "waterDampingScale" TAB "SliderEdit" TAB "range>>0 3;;tickAt>>0.01" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("buoyancyDensity",  "friction" TAB "SliderEdit" TAB "range>>0 1.5;;tickAt>>0.01" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("staticFriction",  "staticFriction" TAB "SliderEdit" TAB "range>>0 1.5;;tickAt>>0.01" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("restitution",  "restitution" TAB "SliderEdit" TAB "range>>0 0.95;;tickAt>>0.01" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.group[%gid++] = "Media";
	%arCfg.setVal("shapeName",  "shapeName" TAB "FileSelect" TAB "" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("debris",  "debris" TAB "TextEdit" TAB "" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("explosion",  "explosion" TAB "TextEdit" TAB "" TAB "DbEd.activeDatablock" TAB %gid);
	%arCfg.setVal("destroyedShape",  "destroyedShape" TAB "TextEdit" TAB "" TAB "DbEd.activeDatablock" TAB %gid);
	$DbEd_PhysicsShapeDataSimTypes = "ClientOnly ServerOnly ClientServer";
	%arCfg.setVal("simType",  "simType" TAB "Dropdown" TAB "itemList>>$DbEd_PhysicsShapeDataSimTypes" TAB "DbEd.activeDatablock" TAB %gid);
	buildParamsArray(%arCfg,true);
	DbEd.currentParam = %arCfg;
}
//------------------------------------------------------------------------------


/*
addField( "mass", TypeF32, Offset( mass, PhysicsShapeData ),
      "@brief Value representing the mass of the shape.\n\n"
      "A shape's mass influences the magnitude of any force exerted on it. "
      "For example, a PhysicsShape with a large mass requires a much larger force to move than "
      "the same shape with a smaller mass.\n"
      "@note A mass of zero will create a kinematic shape while anything greater will create a dynamic shape.");

   addField( "friction", TypeF32, Offset( dynamicFriction, PhysicsShapeData ),
      "@brief Coefficient of kinetic %friction to be applied to the shape.\n\n"
      "Kinetic %friction reduces the velocity of a moving object while it is in contact with a surface. "
      "A higher coefficient will result in a larger velocity reduction. "
      "A shape's friction should be lower than it's staticFriction, but larger than 0.\n\n"
      "@note This value is only applied while an object is in motion. For an object starting at rest, see PhysicsShape::staticFriction");

   addField( "staticFriction", TypeF32, Offset( staticFriction, PhysicsShapeData ),
      "@brief Coefficient of static %friction to be applied to the shape.\n\n"
      "Static %friction determines the force needed to start moving an at-rest object in contact with a surface. "
      "If the force applied onto shape cannot overcome the force of static %friction, the shape will remain at rest. "
      "A larger coefficient will require a larger force to start motion. "
      "This value should be larger than zero and the physicsShape's friction.\n\n"
      "@note This value is only applied while an object is at rest. For an object in motion, see PhysicsShape::friction");

   addField( "restitution", TypeF32, Offset( restitution, PhysicsShapeData ),
      "@brief Coeffecient of a bounce applied to the shape in response to a collision.\n\n"
      "Restitution is a ratio of a shape's velocity before and after a collision. "
      "A value of 0 will zero out a shape's post-collision velocity, making it stop on contact. "
      "Larger values will remove less velocity after a collision, making it \'bounce\' with a greater force. "
      "Normal %restitution values range between 0 and 1.0."
      "@note Values near or equaling 1.0 are likely to cause undesirable results in the physics simulation."
      " Because of this it is reccomended to avoid values close to 1.0");

       addField( "linearDamping", TypeF32, Offset( linearDamping, PhysicsShapeData ),
      "@brief Value that reduces an object's linear velocity over time.\n\n"
      "Larger values will cause velocity to decay quicker.\n\n" );

   addField( "angularDamping", TypeF32, Offset( angularDamping, PhysicsShapeData ),
      "@brief Value that reduces an object's rotational velocity over time.\n\n"
      "Larger values will cause velocity to decay quicker.\n\n" );

   addField( "linearSleepThreshold", TypeF32, Offset( linearSleepThreshold, PhysicsShapeData ),
      "@brief Minimum linear velocity before the shape can be put to sleep.\n\n"
      "This should be a positive value. Shapes put to sleep will not be simulated in order to save system resources.\n\n"
      "@note The shape must be dynamic.");

   addField( "angularSleepThreshold", TypeF32, Offset( angularSleepThreshold, PhysicsShapeData ),
      "@brief Minimum rotational velocity before the shape can be put to sleep.\n\n"
      "This should be a positive value. Shapes put to sleep will not be simulated in order to save system resources.\n\n"
      "@note The shape must be dynamic.");

   addField( "waterDampingScale", TypeF32, Offset( waterDampingScale, PhysicsShapeData ),
      "@brief Scale to apply to linear and angular dampening while underwater.\n\n "
      "Used with the waterViscosity of the  "
      "@see angularDamping linearDamping" );

   addField( "buoyancyDensity", TypeF32, Offset( buoyancyDensity, PhysicsShapeData ),
      "@brief The density of the shape for calculating buoyant forces.\n\n"
      "The result of the calculated buoyancy is relative to the density of the WaterObject the PhysicsShape is within.\n\n"
      "@see WaterObject::density");

addGroup("Media");

   addField( "shapeName", TypeShapeFilename, Offset( shapeName, PhysicsShapeData ),
      "@brief Path to the .DAE or .DTS file to use for this shape.\n\n"
      "Compatable with Live-Asset Reloading. ");

   addField( "debris", TYPEID< SimObjectRef<PhysicsDebrisData> >(), Offset( debris, PhysicsShapeData ),
      "@brief Name of a PhysicsDebrisData to spawn when this shape is destroyed (optional)." );

   addField( "explosion", TYPEID< SimObjectRef<ExplosionData> >(), Offset( explosion, PhysicsShapeData ),
      "@brief Name of an ExplosionData to spawn when this shape is destroyed (optional)." );

   addField( "destroyedShape", TYPEID< SimObjectRef<PhysicsShapeData> >(), Offset( destroyedShape, PhysicsShapeData ),
      "@brief Name of a PhysicsShapeData to spawn when this shape is destroyed (optional)." );

endGroup("Media");


addGroup( "Networking" );

   addField( "simType", TYPEID< PhysicsShapeData::SimType >(), Offset( simType, PhysicsShapeData ),
      "@brief Controls whether this shape is simulated on the server, client, or both physics simulations.\n\n" );

endGroup( "Networking" );
*/
