//==============================================================================
// TorqueLab -> SimViewer - Allow to have a full objects hierachical view
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function LabSimViewer::open(%this,%group)
{ 
   if (!isObject(SView))
      new ScriptObject(SView);
   
	if (!%this.isAwake())
		popDlg(%this);
		
	if (isObject(%group))
		%viewGroup = %group;
	else
		%viewGroup = RootGroup;
		
	
   Canvas.pushDialog("LabSimViewer", 20);
   SView_TreeInspector.open(%viewGroup);
}
//------------------------------------------------------------------------------
//==============================================================================
function SView::Inspect(%this,%obj)
{ 
   // Don't inspect the root group.
   if( %obj == -1 )
      return;
	
	SimViewInspectButton.visible = 0;
	if (%obj.isMemberOfClass("SimSet"))
		SimViewInspectButton.visible = 1;
   
   LabInspect.inspect(%obj);
   
   // Update selected object properties
   SView_InspectObjectName.setValue(%obj.getName());
   SView_InspectObjectInternal.setValue( %obj.getInternalName() );
   SView_InspectObjectID.setValue( %obj.getId() );
   
   // Store Object Reference
   SView_InspectObjectName.refObj = %obj;

}
//------------------------------------------------------------------------------
//==============================================================================
function SView::OpenSelGroup(%this)
{ 
	 %group = SView_InspectObjectName.refObj;
	if (!%group.isMemberOfClass("SimSet")){
		SimViewInspectButton.visible = 0;
		return;
	}
	
  LabSimViewer.open(%group);

}
//------------------------------------------------------------------------------
//==============================================================================
function SView::InspectApply(%this)
{
   %obj = SView_InspectObjectName.refObj;
   if( !isObject( %obj ) )
      return;
       
   // Update name and internal name
   %obj.setName( SView_InspectObjectName.getValue() );
   %obj.setInternalName( SView_InspectObjectInternal.getValue() );
   
   // Update inspected object information.
   LabInspect.inspect( %obj );
}
//------------------------------------------------------------------------------
//==============================================================================
function SView::InspectDelete(%this)
{
   %obj = SView_InspectObjectName.refObj;
   if( !isObject( %obj ) )
      return;

   %obj.delete();       
   
   // Update inspected object information.
   LabInspect.inspect( 0 );
   
   // Update selected object properties
   SView_InspectObjectName.setValue("");
   SView_InspectObjectInternal.setValue( "" );
   SView_InspectObjectID.setValue( 0 );
   

}

//------------------------------------------------------------------------------
//==============================================================================
function SView_TreeInspector::onSelect(%this, %obj)
{
   Inspect(%obj);
}
//------------------------------------------------------------------------------
//==============================================================================
function SView::Tree(%this,%obj)
{
   devLog("#########################","$$$$$$$$$$$$$$$$$$$$$$$$$$$$$"," If reading this, it's actualy called","SView::Tree",%obj);
   Canvas.popDialog("LabSimViewer");
   Canvas.pushDialog("LabSimViewer", 20);
   SView_TreeInspector.open(%obj);
}
//------------------------------------------------------------------------------
//==============================================================================
// MM: Added Dynamic group toggle support.
function GuiInspector::toggleDynamicGroupScript(%this, %obj)
{
   %this.toggleDynamicGroupExpand();
   %this.inspect(%obj);
}
// MM: Added group toggle support.
function GuiInspector::toggleGroupScript(%this, %obj, %fieldName)
{
   %this.toggleGroupExpand(%obj, %fieldName);
   %this.inspect(%obj);
}

// MM: Set All Group State support.
function GuiInspector::setAllGroupStateScript(%this, %obj, %groupState)
{
   %this.setAllGroupState(%groupState);
   %this.inspect(%obj);
}
