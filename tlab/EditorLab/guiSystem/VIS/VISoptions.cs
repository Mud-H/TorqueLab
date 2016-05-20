//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Handle the escape bind
function SideBarVIS::initVisualOptions( %this ) {
	
}
//------------------------------------------------------------------------------

function SideBarVIS::initOptionsArray( %this ) {
	ar_SideBarVIS.empty();
// Expose stock visibility/debug options.
%this.addOption( "Render: Zones", "$Zone::isRenderable", "" );
	%this.addOption( "Render: Zones", "$Zone::isRenderable", "" );
	%this.addOption( "Render: Portals", "$Portal::isRenderable", "" );
	%this.addOption( "Render: Occlusion Volumes", "$OcclusionVolume::isRenderable", "" );
	%this.addOption( "Render: Triggers", "$Trigger::renderTriggers", "" );
	%this.addOption( "Render: PhysicalZones", "$PhysicalZone::renderZones", "" );
	%this.addOption( "Render: Sound Emitters", "$SFXEmitter::renderEmitters", "" );
	%this.addOption( "Render: Mission Area", "EWorldEditor.renderMissionArea", "" );
	%this.addOption( "Render: Sound Spaces", "$SFXSpace::isRenderable", "" );
	%this.addOption( "Wireframe Mode", "$gfx::wireframe", "" );
	%this.addOption( "Debug Render: Player Collision", "$Player::renderCollision", "" );
	%this.addOption( "Debug Render: Terrain", "$TerrainBlock::debugRender", "" );
	%this.addOption( "Debug Render: Decals", "$Decals::debugRender", "" );
	%this.addOption( "Debug Render: Light Frustums", "$Light::renderLightFrustums", "" );
	%this.addOption( "Debug Render: Bounding Boxes", "$Scene::renderBoundingBoxes", "" );
	%this.addOption( "AL: Disable Shadows", "$Shadows::disable", "" );
	%this.addOption( "AL: Light Color Viz", "$AL_LightColorVisualizeVar", "toggleLightColorViz" );
	%this.addOption( "AL: Light Specular Viz", "$AL_LightSpecularVisualizeVar", "toggleLightSpecularViz" );
	%this.addOption( "AL: Normals Viz", "$AL_NormalsVisualizeVar", "toggleNormalsViz" );
	%this.addOption( "AL: Depth Viz", "$AL_DepthVisualizeVar", "toggleDepthViz" );
	%this.addOption( "Frustum Lock", "$Scene::lockCull", "" );
	%this.addOption( "Disable Zone Culling", "$Scene::disableZoneCulling", "" );
	%this.addOption( "Disable Terrain Occlusion", "$Scene::disableTerrainOcclusion", "" );
	//PBR Script
	%this.addOption( "Debug Render: Physics World", "$PhysicsWorld::render", "togglePhysicsDebugViz" );
	%this.addOption( "AL: Environment Light", "$AL_LightMapShaderVar", "toggleLightMapViz" );
	%this.addOption( "AL: Color Buffer", "$AL_ColorBufferShaderVar", "toggleColorBufferViz" );
	%this.addOption( "AL: Spec Map(Rough)", "$AL_RoughMapShaderVar", "toggleRoughMapViz");
	%this.addOption( "AL: Spec Map(Metal)", "$AL_MetalMapShaderVar", "toggleMetalMapViz");
	%this.addOption( "AL: Backbuffer", "$AL_BackbufferVisualizeVar", "toggleBackbufferViz" );
	//PBR Script End
	$VisibilityOptionsLoaded = true;
}
function SideBarVIS::initClassArray( %this ) {
	ar_SideBarVISClass.empty();
%classList = enumerateConsoleClasses( "SceneObject" );
	%classCount = getFieldCount( %classList );

	for ( %i = 0; %i < %classCount; %i++ ) {
		%className = getField( %classList, %i );
		%this.classArray.push_back( %className );
	}

	// Remove duplicates and sort by key.
	%this.classArray.uniqueKey();
	%this.classArray.sortka();
	$VisibilityClassLoaded = true;
}
function SideBarVIS::updateOptions( %this,%arrayOnly ) {
	// First clear the stack control.
	%this-->theVisOptionsList.clear();

	// Go through all the
	// parameters in our array and
	// create a check box for each.
	for ( %i = 0; %i < %this.array.count(); %i++ ) {
		%text = "  " @ %this.array.getValue( %i );
		%val = %this.array.getKey( %i );
		%var = getWord( %val, 0 );
		%toggleFunction = getWord( %val, 1 );
		%textLength = strlen( %text );
		%cmd = "";

		if ( %toggleFunction !$= "" )
			%cmd = %toggleFunction @ "( $thisControl.getValue() );";
		
		%visPill = cloneObject(SideBarVIS_CheckPill);
		%visCheck = %visPill.getObject(0);
		%visCheck.variable = %var;
		%visCheck.command = %cmd;
		%visCheck.text = %text;
		%visCheck.tooltip = "Visual and rendering options";
		//%visCheck.extent = (%textLength * 4) @ " 18";
		
		%this-->theVisOptionsList.addGuiControl( %visPill );		
	}
	%this-->theVisOptionsList.updateStack();
}

function SideBarVIS::addOption( %this, %text, %varName, %toggleFunction ) {
	// Create the array if it
	// doesn't already exist.
	if ( !isObject( %this.array ) )
		%this.array = new ArrayObject();

	%this.array.push_back( %varName @ " " @ %toggleFunction, %text );
	%this.array.uniqueKey();
	%this.array.sortd();
	//%this.updateOptions();
}
//SideBarVIS.addClassOptions

//==============================================================================
function SideBarVIS::updateClass( %this,%arrayOnly ) {
	%visArray = %this-->theClassVisArray;	
	%selArray = %this-->theClassSelArray;
	

	%visArray.clear();
	%selArray.clear();
	

	%visArray.visible = true;
	%selArray.visible = true;
	
	
	

	

	// First clear the stack control.
	
	

	// Go through all the
	// parameters in our array and
	// create a check box for each.

	for ( %i = 0; %i < %this.classArray.count(); %i++ ) {
		%class = %this.classArray.getKey( %i );
		%visVar = "$" @ %class @ "::isRenderable";
		%selVar = "$" @ %class @ "::isSelectable";
		%textLength = strlen( %class );
		%text = "  " @ %class;
		
		%visPill = cloneObject(SideBarVIS_CheckPill);
		%visCheck = %visPill.getObject(0);
		%visPill.setName("Vis_Visible_"@%class);
		%visCheck.variable = %visVar;
		%visCheck.command = "EVisibilityLayers.toggleRenderable(\""@%class@"\");";
		%visCheck.text = %text;
		%visCheck.tooltip = "Show/hide all " @ %class @ " objects.";
		%visArray.addGuiControl(%visPill);
		
		%selPill = cloneObject(SideBarVIS_CheckPill);
		%selPill.setName("Vis_Select_"@%class);
		%selCheck = %selPill.getObject(0);
		%selCheck.variable = %selVar;
		%selCheck.command = "EVisibilityLayers.toggleSelectable(\""@%class@"\");";
		%selCheck.text = %text;
		%selCheck.tooltip = "Show/hide all " @ %class @ " objects.";
		%selArray.addGuiControl(%selPill);
		
		
	}
	if ($EVisibilityArrayMode){
		%visArray.refresh();
		%selArray.refresh();
	}
	$VisibilityClassLoaded = true;
}
//==============================================================================
function SideBarVIS::toggleRenderable( %this,%class ) {
	//eval("$" @ %class @ "::isRenderable = !$"@ %class @ "::isRenderable;");
	%visVar = "$" @ %class @ "::isRenderable";

	//%selVar = "$" @ %class @ "::isSelectable";
	if (!%visVar)
		eval("$" @ %class @ "::isSelectable = \"0\";");
	else
		eval("$" @ %class @ "::isSelectable = \"1\";");
}
function SideBarVIS::toggleSelectable( %this,%class ) {
	//eval("$" @ %class @ "::isRenderable = !$"@ %class @ "::isRenderable;");
	%selVar = "$" @ %class @ "::isSelectable";

	if (%selVar)
		eval("$" @ %class @ "::isRenderable = \"1\";");
}
