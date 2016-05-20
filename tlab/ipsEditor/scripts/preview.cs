
function IpsEditor::resetEmitterNode( %this ) {
	%tform = ServerConnection.getControlObject().getEyeTransform();
	%vec = VectorNormalize( ServerConnection.getControlObject().getForwardVector() );
	%vec = VectorScale( %vec, 4 );
	%tform = setWord( %tform, 0, getWord( %tform, 0 ) + getWord( %vec, 0 ) );
	%tform = setWord( %tform, 1, getWord( %tform, 1 ) + getWord( %vec, 1 ) );
	%tform = setWord( %tform, 2, getWord( %tform, 2 ) + getWord( %vec, 2 ) );

	if( !isObject( $IpsEditor::emitterNode ) ) {
		if( !isObject( TestEmitterNodeData ) ) {
			datablock ParticleEmitterNodeData( TestEmitterNodeData ) {
				timeMultiple = 1;
			};
		}

		$IpsEditor::emitterNode = new ParticleEmitterNode() {
			emitter = IPSE_Selector.getText();
			velocity = 1;
			position = getWords( %tform, 0, 2 );
			rotation = getWords( %tform, 3, 6 );
			datablock = TestEmitterNodeData;
			parentGroup = MissionCleanup;
		};
	} else {
		$IpsEditor::emitterNode.setTransform( %tform );
		%clientObject = $IpsEditor::emitterNode.getClientObject();

		if( isObject( %clientObject ) )
			%clientObject.setTransform( %tform );

		IpsEditor.updateEmitterNode();
	}
}

//---------------------------------------------------------------------------------------------
$IPS_AlwaysLoop = false;
$IPS_LoopMulti = 1;
$IPS_NodeMode = "Emitter";
function IpsEditor::updateEmitterNode( %this ) {

		
	if( isObject( $IpsEditor::emitterNode ) ) {
	   if ($IPS_NodeMode $= "Emitter")
		   %id = IPSE_Selector_Control-->PopUpMenu.getSelected();
      else
          %id = IPSC_Selector_Control-->PopUpMenu.getSelected();
		%clientObject = $IpsEditor::emitterNode.getClientObject();

		if( isObject( %clientObject ) )
			%clientObject.setEmitterDataBlock( %id );
		
		if (($IPS_AlwaysLoop || $IPS_AutoLoop) && %id.lifetimeMS > 0){
			%partLifeMs = 0;
			foreach$(%particle in %id.particles){
				if (%particle.lifetimeMS > %partLifeMs)
					%partLifeMs = %particle.lifetimeMS;
			}
			%delay = %id.lifetimeMS + %partLifeMs;
			
			%this.schedule(%delay * $IPS_LoopMulti,"resetEmitterNode");
		}
		
	} else
		%this.resetEmitterNode();
}

function IpsEditor::playEmitterFx( %this,%loop ) {
	if (%loop $= "" || !%loop)
		$IPS_AutoLoop = false;
	else 
		$IPS_AutoLoop = true;
	
	%this.updateEmitterNode();
	
}
//IpsEditor.mountEmitterOnObjNode(LocalClientConnection.player,9);
function IpsEditor::mountEmitterOnObjNode( %this,%targetObj,%mountNodeId,%offset ) {	
	if (!isObject(%targetObj))
		return;
		
	%targetObj.mountObject($IpsEditor::emitterNode,%mountNodeId,%offset);
	%this.mountOn = %targetObj;
}
//IpsEditor.unmountEmitter();
function IpsEditor::unmountEmitter( %this ) {
	if (!isObject(%this.mountOn))
		return;
		
	%this.mountOn.unmountObject($IpsEditor::emitterNode);
}
//IpsEditor.mountOnControlObj();
function IpsEditor::mountOnControlObj( %this,%mount,%node ) {
	if (%node $= "")
		%node = mROund(IPS_ControlObjNode.getText());
	
	%offset = IPS_ControlObjOffset.getText();
	if (!strIsNumeric(%node))
		return;
	
	%controlObj = LocalClientConnection.player;
	if (%mount)
		IpsEditor.mountEmitterOnObjNode(%controlObj,%node,%offset);
	else
		IpsEditor.unmountEmitter();
	
}
