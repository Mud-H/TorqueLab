//==============================================================================
// TorqueLab -> IPS Editor
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//=============================================================================================
//    IpsEditor.
//=============================================================================================

//---------------------------------------------------------------------------------------------
//IpsEditor.initEditor
function IpsEditor::initEditor( %this ) {
	echo( "Initializing ParticleEmitterData and ParticleData DataBlocks..." );
	datablock ParticleEmitterData(IPSE_Editor_NotDirtyEmitter) {
		particles = "DefaultParticle";
	};
	datablock BillboardParticleData(IPSP_Editor_NotDirtyParticle) {
		textureName = "art/gfx/particles/defaultParticle";
	};
	datablock SphereEmitterData(IPSP_Editor_NotDirtySphereEmitter) {
		particles = "DefaultParticle";
	};
		datablock CompositeEmitterData(IPSC_Editor_NotDirtyComposite) {
		particles = "DefaultParticle";
	};
	IPS_UnlistedEmitters.add( IPSE_Editor_NotDirtyEmitter );
	IPS_UnlistedEmitters.add( IPSP_Editor_NotDirtyParticle );
	IPS_UnlistedEmitters.add( IPSP_Editor_NotDirtySphereEmitter );
	
   $IPS_ClassDirty["ParticleEmitterData"] = IPSE_Editor_NotDirtyEmitter;
	$IPS_ClassDirty["BillboardParticleData"] = IPSP_Editor_NotDirtyParticle;
	$IPS_ClassDirty["SphereEmitterData"] = IPSP_Editor_NotDirtySphereEmitter;
	$IPS_ClassDirty["CompositeEmitterData"] = IPSC_Editor_NotDirtyComposite;
	IpsEditor.createDataList();
	IPSE_EmitterParticleSelector2.add( "None", 0 );
	IPSE_EmitterParticleSelector3.add( "None", 0 );
	IPSE_EmitterParticleSelector4.add( "None", 0 );
	IPSE_EmitterParticleSelector1.sort();
	IPSE_EmitterParticleSelector2.sort();
	IPSE_EmitterParticleSelector3.sort();
	IPSE_EmitterParticleSelector4.sort();
	IPSE_Editor-->blendStyle.clear();
	IPSE_Editor-->blendStyle.add( "NORMAL", 0 );
	IPSE_Editor-->blendStyle.add( "ADDITIVE", 1 );
	IPSE_Editor-->blendStyle.add( "SUBTRACTIVE", 2 );
	IPSE_Editor-->blendStyle.add( "PREMULTALPHA", 3 );
	IPSE_Selector.setFirstSelected();
	IPS_Window-->EditorTabBook.selectPage( 0 );
}

function IpsEditor::createDataList( %this ) {
	// This function creates the list of all particles and particle emitters
	%emitterCount = 0;
	%particleCount = 0;
	%compositeCount = 0;
		IPSE_EmitterParticleSelector1.clear();
	IPSE_EmitterParticleSelector2.clear();
	IPSE_EmitterParticleSelector3.clear();
	IPSE_EmitterParticleSelector4.clear();
	
	IPSC_Emitter1_Selector.clear();
	IPSC_Emitter2_Selector.clear();
	
   IPSP_Selector.clear();
   IPSE_Selector.clear();
	foreach( %obj in DatablockGroup ) {  
	   
	 
		if( %obj.isMemberOfClass( "ParticleEmitterData" ) ) {
		   if( %obj.isMemberOfClass( "CompositeEmitterData" ) ) {
			

            %name = %obj.getName();
            %id = %obj.getId();

            if ( %name $= "DefaultCompositeData")
               continue;
         
            IPSC_Selector.add( %obj.getName(), %obj.getId() );
         
            
            %compositeCount ++;
            continue;
		   }
			// Filter out emitters on the IPS_UnlistedEmitters list.
			%unlistedFound = false;

			foreach( %unlisted in IPS_UnlistedEmitters )
				if( %unlisted.getId() == %obj.getId() ) {
					%unlistedFound = true;
					break;
				}

			if( %unlistedFound )
				continue;

			// To prevent our default emitters from getting changed,
			// prevent them from populating the list. Default emitters
			// should only be used as a template for creating new ones.
			if ( %obj.getName() $= "DefaultEmitter")
				continue;
         IPSC_Emitter1_Selector.add( %obj.getName(), %obj.getId() );
	      IPSC_Emitter2_Selector.add( %obj.getName(), %obj.getId() );
			IPSE_Selector.add( %obj.getName(), %obj.getId() );
			%emitterCount ++;
		} else if( %obj.isMemberOfClass( "ParticleData" ) || %obj.isMemberOfClass( "BillboardParticleData" ) ) {
			%unlistedFound = false;

			foreach( %unlisted in IPS_UnlistedParticles )
				if( %unlisted.getId() == %obj.getId() ) {
					%unlistedFound = true;
					break;
				}

			if( %unlistedFound )
				continue;

			%name = %obj.getName();
			%id = %obj.getId();

			if ( %name $= "DefaultParticle")
				continue;
      
         IPSP_Selector.add( %obj.getName(), %obj.getId() );
			IPS_AllParticles.add(%obj);
			// Add to particle dropdown selectors.
			IPSE_EmitterParticleSelector1.add( %name, %id );
			IPSE_EmitterParticleSelector2.add( %name, %id );
			IPSE_EmitterParticleSelector3.add( %name, %id );
			IPSE_EmitterParticleSelector4.add( %name, %id );
			%particleCount ++;
		}  
	}

	IPSE_Selector.sort();
	//IPSE_EmitterParticleSelector1.sort();
	//IPSE_EmitterParticleSelector2.sort();
	//IPSE_EmitterParticleSelector3.sort();
	//IPSE_EmitterParticleSelector4.sort();
	echo( "Found" SPC %emitterCount SPC "emitters," SPC %particleCount SPC "particles and" SPC %compositeCount SPC "composites.");
}

//---------------------------------------------------------------------------------------------

function IpsEditor::openEmitterPane( %this ) {
	//IPS_Window.text = "Particle Editor - Emitters";
	IPSE_Editor.guiSync();
	IpsEditor.activeEditor = IPSE_Editor;

	if( !IPSE_Editor.dirty )
		IPSE_Editor.setEmitterNotDirty();
}

//---------------------------------------------------------------------------------------------

function IpsEditor::openParticlePane( %this ) {
	//IPS_Window.text = "Particle Editor - Particles";
	IPSP_Editor.guiSync();
	IpsEditor.activeEditor = IPSP_Editor;

	if( !IPSP_Editor.dirty )
		IPSP_Editor.setParticleNotDirty();
}

//---------------------------------------------------------------------------------------------

//=============================================================================================
//    IPS_TabBook.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function IPS_TabBook::onTabSelected( %this, %text, %idx ) {
	if( %idx == 0 )
		IpsEditor.openEmitterPane();
	else
		IpsEditor.openParticlePane();
}
