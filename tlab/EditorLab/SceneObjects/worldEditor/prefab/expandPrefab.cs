//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================


function Lab::ExpandPrefab(%this,%prefab) {
	DevLog("Expanding prefab:",%prefab);
	%container = %prefab.parentGroup;

	if (!isObject(%container)) {
		devLog("Invalid container:",%container);
		%container = MissionGroup;
	}

	%simGroup = newSimGroup("",%container);
	%file = %prefab.fileName;
	%simGroup.prefabFile = %file;
	%simGroup.internalName = "fab_"@fileBase(%file);
	exec(%file);

	//$ThisPrefab.outputlog();
	foreach(%obj in $ThisPrefab) {
		%dataObj = %obj.deepClone();
		%dataObj.internalName = fileBase(%obj.shapeName);
		%dataObj.absPos = %dataObj.position;
		%dataObj.position = VectorAdd(%dataObj.position, %prefab.position);
		%simGroup.add(%dataObj);
	}

	delObj(%prefab);
	return true;
}
function Lab::CollapsePrefab(%this,%simGroup) {
	%file = %simGroup.prefabFile;

	if (%file $= "")
		return;

	%position = VectorSub(%simGroup.getObject(0).position,%simGroup.getObject(0).absPos);

	foreach(%obj in %simGroup)
		%obj.position = %obj.absPos;

	%simGroup.save(%file,false,"$ThisPrefab = ");
	%objId = new Prefab() {
		filename = %file;
		position = %position;
		parentGroup = %simGroup.parentGroup;
	};
	delObj(%simGroup);
}
/*
Example Prefab File:

//--- OBJECT WRITE BEGIN ---
$ThisPrefab = new SimGroup() {
   canSave = "1";
   canSaveDynamicFields = "1";

   new TSStatic() {
      shapeName = "art/models/EnemyTrenches/Buildings/Bunkers/BunkerSetA/ExtWall/WallA_4u.DAE";
      playAmbient = "1";
      meshCulling = "0";
      originSort = "0";
      collisionType = "Collision Mesh";
      decalType = "Collision Mesh";
      allowPlayerStep = "0";
      alphaFadeEnable = "0";
      alphaFadeStart = "100";
      alphaFadeEnd = "150";
      alphaFadeInverse = "0";
      renderNormals = "0";
      forceDetail = "-1";
      position = "7.99997 3.05176e-005 -1.6";
      rotation = "0 0 1 180";
      scale = "1 1 1";
      internalName = "WallA_4u";
      canSave = "1";
      canSaveDynamicFields = "1";
         byGroup = "0";
   };
   new TSStatic() {
      shapeName = "art/models/EnemyTrenches/Buildings/Bunkers/BunkerSetA/ExtWall/WallATopA_4u.DAE";
      playAmbient = "1";
      meshCulling = "0";
      originSort = "0";
      collisionType = "Collision Mesh";
      decalType = "Collision Mesh";
      allowPlayerStep = "0";
      alphaFadeEnable = "0";
      alphaFadeStart = "100";
      alphaFadeEnd = "150";
      alphaFadeInverse = "0";
      renderNormals = "0";
      forceDetail = "-1";
      position = "7.99997 3.05176e-005 1.6";
      rotation = "0 0 1 180";
      scale = "1 1 1";
      internalName = "WallATopA_4u";
      canSave = "1";
      canSaveDynamicFields = "1";
         byGroup = "0";
   };
   new TSStatic() {
      shapeName = "art/models/EnemyTrenches/Buildings/Bunkers/BunkerSetA/ExtWall/WallATopA_4u.DAE";
      playAmbient = "1";
      meshCulling = "0";
      originSort = "0";
      collisionType = "Collision Mesh";
      decalType = "Collision Mesh";
      allowPlayerStep = "0";
      alphaFadeEnable = "0";
      alphaFadeStart = "100";
      alphaFadeEnd = "150";
      alphaFadeInverse = "0";
      renderNormals = "0";
      forceDetail = "-1";
      position = "4.79996 3.05176e-005 1.6";
      rotation = "0 0 1 180";
      scale = "1 1 1";
      internalName = "WallATopA_4u";
      canSave = "1";
      canSaveDynamicFields = "1";
         byGroup = "0";
   };
   new TSStatic() {
      shapeName = "art/models/EnemyTrenches/Buildings/Bunkers/BunkerSetA/ExtWall/WallA_4u.DAE";
      playAmbient = "1";
      meshCulling = "0";
      originSort = "0";
      collisionType = "Collision Mesh";
      decalType = "Collision Mesh";
      allowPlayerStep = "0";
      alphaFadeEnable = "0";
      alphaFadeStart = "100";
      alphaFadeEnd = "150";
      alphaFadeInverse = "0";
      renderNormals = "0";
      forceDetail = "-1";
      position = "4.79996 3.05176e-005 -1.6";
      rotation = "0 0 1 180";
      scale = "1 1 1";
      internalName = "WallA_4u";
      canSave = "1";
      canSaveDynamicFields = "1";
         byGroup = "0";
   };
   new TSStatic() {
      shapeName = "art/models/EnemyTrenches/Buildings/Bunkers/BunkerSetA/ExtWall/WallA_4u.DAE";
      playAmbient = "1";
      meshCulling = "0";
      originSort = "0";
      collisionType = "Collision Mesh";
      decalType = "Collision Mesh";
      allowPlayerStep = "0";
      alphaFadeEnable = "0";
      alphaFadeStart = "100";
      alphaFadeEnd = "150";
      alphaFadeInverse = "0";
      renderNormals = "0";
      forceDetail = "-1";
      position = "1.59998 3.05176e-005 -1.6";
      rotation = "0 0 1 180";
      scale = "1 1 1";
      internalName = "WallA_4u";
      canSave = "1";
      canSaveDynamicFields = "1";
         byGroup = "0";
   };
   new TSStatic() {
      shapeName = "art/models/EnemyTrenches/Buildings/Bunkers/BunkerSetA/ExtWall/WallATopA_4u.DAE";
      playAmbient = "1";
      meshCulling = "0";
      originSort = "0";
      collisionType = "Collision Mesh";
      decalType = "Collision Mesh";
      allowPlayerStep = "0";
      alphaFadeEnable = "0";
      alphaFadeStart = "100";
      alphaFadeEnd = "150";
      alphaFadeInverse = "0";
      renderNormals = "0";
      forceDetail = "-1";
      position = "1.59998 3.05176e-005 1.6";
      rotation = "0 0 1 180";
      scale = "1 1 1";
      internalName = "WallATopA_4u";
      canSave = "1";
      canSaveDynamicFields = "1";
         byGroup = "0";
   };
   new TSStatic() {
      shapeName = "art/models/EnemyTrenches/Buildings/Bunkers/BunkerSetA/ExtWall/WallATopA_4u.DAE";
      playAmbient = "1";
      meshCulling = "0";
      originSort = "0";
      collisionType = "Collision Mesh";
      decalType = "Collision Mesh";
      allowPlayerStep = "0";
      alphaFadeEnable = "0";
      alphaFadeStart = "100";
      alphaFadeEnd = "150";
      alphaFadeInverse = "0";
      renderNormals = "0";
      forceDetail = "-1";
      position = "-1.60004 3.05176e-005 1.6";
      rotation = "0 0 1 180";
      scale = "1 1 1";
      internalName = "WallATopA_4u";
      canSave = "1";
      canSaveDynamicFields = "1";
         byGroup = "0";
   };
   new TSStatic() {
      shapeName = "art/models/EnemyTrenches/Buildings/Bunkers/BunkerSetA/ExtWall/WallA_4u.DAE";
      playAmbient = "1";
      meshCulling = "0";
      originSort = "0";
      collisionType = "Collision Mesh";
      decalType = "Collision Mesh";
      allowPlayerStep = "0";
      alphaFadeEnable = "0";
      alphaFadeStart = "100";
      alphaFadeEnd = "150";
      alphaFadeInverse = "0";
      renderNormals = "0";
      forceDetail = "-1";
      position = "-1.60004 3.05176e-005 -1.6";
      rotation = "0 0 1 180";
      scale = "1 1 1";
      internalName = "WallA_4u";
      canSave = "1";
      canSaveDynamicFields = "1";
         byGroup = "0";
   };
   new TSStatic() {
      shapeName = "art/models/EnemyTrenches/Buildings/Bunkers/BunkerSetA/ExtWall/WallA_4u.DAE";
      playAmbient = "1";
      meshCulling = "0";
      originSort = "0";
      collisionType = "Collision Mesh";
      decalType = "Collision Mesh";
      allowPlayerStep = "0";
      alphaFadeEnable = "0";
      alphaFadeStart = "100";
      alphaFadeEnd = "150";
      alphaFadeInverse = "0";
      renderNormals = "0";
      forceDetail = "-1";
      position = "-4.80005 3.05176e-005 -1.6";
      rotation = "0 0 1 180";
      scale = "1 1 1";
      internalName = "WallA_4u";
      canSave = "1";
      canSaveDynamicFields = "1";
         byGroup = "0";
   };
   new TSStatic() {
      shapeName = "art/models/EnemyTrenches/Buildings/Bunkers/BunkerSetA/ExtWall/WallATopA_4u.DAE";
      playAmbient = "1";
      meshCulling = "0";
      originSort = "0";
      collisionType = "Collision Mesh";
      decalType = "Collision Mesh";
      allowPlayerStep = "0";
      alphaFadeEnable = "0";
      alphaFadeStart = "100";
      alphaFadeEnd = "150";
      alphaFadeInverse = "0";
      renderNormals = "0";
      forceDetail = "-1";
      position = "-4.80005 3.05176e-005 1.6";
      rotation = "0 0 1 180";
      scale = "1 1 1";
      internalName = "WallATopA_4u";
      canSave = "1";
      canSaveDynamicFields = "1";
         byGroup = "0";
   };
   new TSStatic() {
      shapeName = "art/models/EnemyTrenches/Buildings/Bunkers/BunkerSetA/ExtWall/WallATopA_4u.DAE";
      playAmbient = "1";
      meshCulling = "0";
      originSort = "0";
      collisionType = "Collision Mesh";
      decalType = "Collision Mesh";
      allowPlayerStep = "0";
      alphaFadeEnable = "0";
      alphaFadeStart = "100";
      alphaFadeEnd = "150";
      alphaFadeInverse = "0";
      renderNormals = "0";
      forceDetail = "-1";
      position = "-8.00003 3.05176e-005 1.6";
      rotation = "0 0 1 180";
      scale = "1 1 1";
      internalName = "WallATopA_4u";
      canSave = "1";
      canSaveDynamicFields = "1";
         byGroup = "0";
   };
   new TSStatic() {
      shapeName = "art/models/EnemyTrenches/Buildings/Bunkers/BunkerSetA/ExtWall/WallA_4u.DAE";
      playAmbient = "1";
      meshCulling = "0";
      originSort = "0";
      collisionType = "Collision Mesh";
      decalType = "Collision Mesh";
      allowPlayerStep = "0";
      alphaFadeEnable = "0";
      alphaFadeStart = "100";
      alphaFadeEnd = "150";
      alphaFadeInverse = "0";
      renderNormals = "0";
      forceDetail = "-1";
      position = "-8.00003 3.05176e-005 -1.6";
      rotation = "0 0 1 180";
      scale = "1 1 1";
      internalName = "WallA_4u";
      canSave = "1";
      canSaveDynamicFields = "1";
         byGroup = "0";
   };
};
//--- OBJECT WRITE END ---
*/