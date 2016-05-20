//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Add Objects at Reference Nodes
//==============================================================================

function SEPutil_AddObjAtNode_getSrcfile() {
	%current = $ActiveSplatmap;
	if (!isFile(%current))
		%current = $Server::MissionFile;
	%filter = "Model Files (*.DAE, *.dts)|*.DAE;*.dts;|All Files (*.*)|*.*|";
	getLoadFilename(%filter,"SEPutil_AddObjAtNode_setSrcfile",%current);
}
//==============================================================================
function SEPutil_AddObjAtNode_setSrcfile( %file) {
	%file = validatePath(%file,true);
	SEPutil_AddObjAtNode_Srcfile.setText(%file);
	
}

function SceneEd::doAddObjectsToRefNode( %this,%copyObject ) {
	if (%copyObject){
		%fileSrc = SEPutil_AddObjAtNode_CopyObj.text;
		if (!isObject(%fileSrc)){
			warnLog("Invalid Source Object");
			return;
		}
	}
	else {
		%fileSrc = SEPutil_AddObjAtNode_Srcfile.text;
		if (!isFile(%fileSrc)){
			SEPutil_AddObjAtNode_getSrcfile();
			return;
		}
	}
	%parentNode = SEPutil_AddObjAtNode_SrcNode.getText();
	if (%parentNode $= ""){
		warnLog("Invalid parent name");
		return;
	}
	%srcObj = SEPutil_AddObjAtNode_SrcObj.text;
	if (!isObject(%srcObj)){
		warnLog("Invalid Source Object");
		return;
	}
	
	addObjectAtShapeNodeChildPos(%fileSrc,%srcObj,%parentNode);
}