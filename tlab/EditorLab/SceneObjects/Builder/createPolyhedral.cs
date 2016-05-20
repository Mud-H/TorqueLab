//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================

//==============================================================================
//SceneTrees - MarkObject/UnMarkObject (Used for active SimGroup)
//==============================================================================
//==============================================================================

//------------------------------------------------------------------------------
//Scene.createPolyhedralFromCoord("","-10 -10 -10","10 10 10","");
function Scene::createPolyhedralFromCoord(%this, %polyhedralClass,%coordMin,%coordMax,%dropAtPos) {
//This function will create a temporary zone to send to code so it can convert it 
	if (%dropAtPos $= "")
		%dropAtPos = Scene.getCreateObjectPosition();
		
	%vectorDiff = VectorSub(%coordMax,	%coordMin);
	
	%dropAtPos.z += %vectorDiff.z/2;
	
	%this.createPolyhedral(%polyhedralClass,%dropAtPos,%vectorDiff);
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
//Scene.createPolyhedralFromSelection(SETools_CreateOptClassMenu.getText());
function Scene::createPolyhedralFromSelection(%this,%class,%getDropPos) {
	if (EWorldEditor.getSelectionSize() <= 0){
		warnLog("Nothing is selected");
		return;
	}
	
	%useBoxCenter =  EWorldEditor.objectsUseBoxCenter;
	
	 EWorldEditor.objectsUseBoxCenter = true;
	%dropAtPos = EWorldEditor.getSelectionCentroid();
	 EWorldEditor.objectsUseBoxCenter = %useBoxCenter;

//This function will create a temporary zone to send to code so it can convert it 
	if (%getDropPos )
		%dropAtPos = Scene.getCreateObjectPosition();
		
	%vectorDiff = EWorldEditor.getSelectionExtent();
	
	%text = SETools_CreateOptClassMargin.getText();
	if (!strIsNumeric(%text.x))
		%failed = true;
	if (!strIsNumeric(%text.y))
		%failed = true;
	if (!strIsNumeric(%text.z))
		%failed = true;
	
	if (%failed)
		%margin = "0 0 0";		
	else 
		%margin = %text.x SPC %text.y SPC %text.z;	
		
	%marginVect = VectorScale(%margin,2);
	
	%vectorDiff = VectorSub(%vectorDiff,%marginVect);
	
	if (%vectorDiff.x <= "0.01")
		%vectorDiff.x = "0.01";
	if (%vectorDiff.y <= "0.01")
		%vectorDiff.y = "0.01";	
	if (%vectorDiff.z <= "0.01")
		%vectorDiff.z = "0.01";
	%this.createPolyhedral(%class,%dropAtPos,%vectorDiff);
	
	
	
}


function SETools_CreateOptClassMargin::onValidate( %this ) {
	%text = SETools_CreateOptClassMargin.getText();
	if (!strIsNumeric(%text.x))
		%failed = true;
	if (!strIsNumeric(%text.y))
		%failed = true;
	if (!strIsNumeric(%text.z))
		%failed = true;
	
	if (%failed){
		SETools_CreateOptClassMargin.setText("0 0 0");
		return;
	}
	SETools_CreateOptClassMargin.setText(%text.x SPC %text.y SPC %text.z);
	
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
//Scene.createPolyhedralFromCoord("","-10 -10 -10","10 10 10","");
function Scene::createPolyhedral(%this,%class,%position,%scale) {
	if (%class $= "")
		%class = "Zone";
	%name = getUniqueName("Auto"@%class);
	switch$(%class){
		case "Zone":
			%newObj = new Zone( %name )
			{
				position = %position;
				rotation = "1 0 0 0";
				scale = %scale;
			};
		case "Portal":
			%newObj = new Portal( %name )
			{
				position = %position;
				rotation = "1 0 0 0";
				scale = %scale;
			};
		case "OcclusionVolume":
			%newObj = new OcclusionVolume( %name )
			{
				position = %position;
				rotation = "1 0 0 0";
				scale = %scale;
			};
	}
	if (!isObject(%newObj))
		return;
		
	%activeGroup = Scene.getActiveSimGroup();
	%activeGroup.add(%newObj);
}