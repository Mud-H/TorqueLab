//==============================================================================
// TorqueLab -> Editor Gui General Scripts
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================

function Scene::hideGroupChilds( %this, %simGroup ) {
	logd("Scene::hideGroupChilds",%simGroup);

	foreach( %child in %simGroup ) {
		if( %child.isMemberOfClass( "SimGroup" ) )
			%this.hideGroupChilds( %child );
		else if (!%child.hidden)
			EWorldEditor.hideObject(%child,true);

		//%child.setHidden( true );
	}
}
//------------------------------------------------------------------------------
function Scene::showGroupChilds( %this, %simGroup ) {
	logd("Scene::showGroupChilds",%simGroup);

	foreach( %child in %simGroup ) {
		if( %child.isMemberOfClass( "SimGroup" ) )
			%this.showGroupChilds( %child );
		else if (%child.hidden)
			EWorldEditor.hideObject(%child,false);

		//	%child.setHidden( false );
	}
}
//------------------------------------------------------------------------------

function Scene::toggleHideGroupChilds( %this, %simGroup ) {
	logd("Scene::toggleHideGroupChilds",%simGroup);

	foreach( %child in %simGroup ) {
		if( %child.isMemberOfClass( "SimGroup" ) )
			%this.toggleHideGroupChilds( %child );
		else
			%child.setHidden( !%child.hidden );
	}
}
//------------------------------------------------------------------------------