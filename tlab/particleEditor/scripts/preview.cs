
function ParticleEditor::resetEmitterNode( %this ) {
	%tform = ServerConnection.getControlObject().getEyeTransform();
	%vec = VectorNormalize( ServerConnection.getControlObject().getForwardVector() );
	%vec = VectorScale( %vec, 4 );
	%tform = setWord( %tform, 0, getWord( %tform, 0 ) + getWord( %vec, 0 ) );
	%tform = setWord( %tform, 1, getWord( %tform, 1 ) + getWord( %vec, 1 ) );
	%tform = setWord( %tform, 2, getWord( %tform, 2 ) + getWord( %vec, 2 ) );

	if( !isObject( $ParticleEditor::emitterNode ) ) {
		if( !isObject( TestEmitterNodeData ) ) {
			datablock ParticleEmitterNodeData( TestEmitterNodeData ) {
				timeMultiple = 1;
			};
		}

		$ParticleEditor::emitterNode = new ParticleEmitterNode() {
			emitter = PEE_EmitterSelector.getText();
			velocity = 1;
			position = getWords( %tform, 0, 2 );
			rotation = getWords( %tform, 3, 6 );
			datablock = TestEmitterNodeData;
			parentGroup = MissionCleanup;
		};
	} else {
		$ParticleEditor::emitterNode.setTransform( %tform );
		%clientObject = $ParticleEditor::emitterNode.getClientObject();

		if( isObject( %clientObject ) )
			%clientObject.setTransform( %tform );

		ParticleEditor.updateEmitterNode();
	}
}

//---------------------------------------------------------------------------------------------
$PE_AlwaysLoop = false;
$PE_LoopMulti = 1;
function ParticleEditor::updateEmitterNode( %this ) {

		
	if( isObject( $ParticleEditor::emitterNode ) ) {
		%id = PEE_EmitterSelector_Control-->PopUpMenu.getSelected();
		%clientObject = $ParticleEditor::emitterNode.getClientObject();

		if( isObject( %clientObject ) )
			%clientObject.setEmitterDataBlock( %id );
		
		if (($PE_AlwaysLoop || $PE_AutoLoop) && %id.lifetimeMS > 0){
			%partLifeMs = 0;
			foreach$(%particle in %id.particles){
				if (%particle.lifetimeMS > %partLifeMs)
					%partLifeMs = %particle.lifetimeMS;
			}
			%delay = %id.lifetimeMS + %partLifeMs;
			
			%this.schedule(%delay * $PE_LoopMulti,"resetEmitterNode");
		}
		
	} else
		%this.resetEmitterNode();
}

function ParticleEditor::playEmitterFx( %this,%loop ) {
	if (%loop $= "" || !%loop)
		$PE_AutoLoop = false;
	else 
		$PE_AutoLoop = true;
	
	%this.updateEmitterNode();
	
}
//ParticleEditor.mountEmitterOnObjNode(LocalClientConnection.player,9);
function ParticleEditor::mountEmitterOnObjNode( %this,%targetObj,%mountNodeId,%offset ) {	
	if (!isObject(%targetObj))
		return;
		
	%targetObj.mountObject($ParticleEditor::emitterNode,%mountNodeId,%offset);
	%this.mountOn = %targetObj;
}
//ParticleEditor.unmountEmitter();
function ParticleEditor::unmountEmitter( %this ) {
	if (!isObject(%this.mountOn))
		return;
		
	%this.mountOn.unmountObject($ParticleEditor::emitterNode);
}
//ParticleEditor.mountOnControlObj();
function ParticleEditor::mountOnControlObj( %this,%mount,%node ) {
	if (%node $= "")
		%node = mROund(PE_ControlObjNode.getText());
	
	%offset = PE_ControlObjOffset.getText();
	if (!strIsNumeric(%node))
		return;
	
	%controlObj = LocalClientConnection.player;
	if (%mount)
		ParticleEditor.mountEmitterOnObjNode(%controlObj,%node,%offset);
	else
		ParticleEditor.unmountEmitter();
	
}