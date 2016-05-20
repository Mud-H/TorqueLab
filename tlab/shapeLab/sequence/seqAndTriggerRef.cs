//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// ShapeLab Sequence Functions References
//==============================================================================
/*
//==============================================================================
// TSShapeConstructor - SEQUENCES
//==============================================================================

//==============================================================================
TSShapeConstructor.getSequenceCount();
//-----------------------------------------------------------------------------
Get the total number of sequences in the shape.
@return the number of sequences in the shape
//-----------------------------------------------------------------------------

//==============================================================================
TSShapeConstructor.getSequenceIndex(name);
//-----------------------------------------------------------------------------
Find the index of the sequence with the given name.
   @param name name of the sequence to lookup
   @return index of the sequence with matching name, or -1 if not found
example:
   // Check if a given sequence exists in the shape
   if ( %this.getSequenceIndex( \"walk\" ) == -1 )
      echo( \"Could not find 'walk' sequence\" );
//-----------------------------------------------------------------------------

//==============================================================================
TSShapeConstructor.getSequenceName(name);
//------------------------------------------------------------------------------
Get the name of the indexed sequence.
   @param index index of the sequence to query (valid range is 0 - getSequenceCount()-1)
   @return the name of the sequence
   Example:
   // print the name of all sequences in the shape
   %count = %this.getSequenceCount();
   for ( %i = 0; %i < %count; %i++ )
      echo( %i SPC %this.getSequenceName( %i ) );

//------------------------------------------------------------------------------

//==============================================================================
TSShapeConstructor.getSequenceSource(name);
//------------------------------------------------------------------------------
"Get information about where the sequence data came from.
   "For example, whether it was loaded from an external DSQ file.
   @param name name of the sequence to query
   @return TAB delimited string of the form: \"from reserved start end total\", where:"
   "<dl>"
      "<dt>from</dt><dd>the source of the animation data, such as the path to "
      "a DSQ file, or the name of an existing sequence in the shape. This field "
      "will be empty for sequences already embedded in the DTS or DAE file.</dd>"
      "<dt>reserved</dt><dd>reserved value</dd>"
      "<dt>start</dt><dd>the first frame in the source sequence used to create this sequence</dd>"
      "<dt>end</dt><dd>the last frame in the source sequence used to create this sequence</dd>"
      "<dt>total</dt><dd>the total number of frames in the source sequence</dd>"
   "</dl>
   Example:
   // print the source for the walk animation
   "echo( \"walk source:\" SPC getField( %this.getSequenceSource( \"walk\" ), 0 ) );

//------------------------------------------------------------------------------

//==============================================================================
TSShapeConstructor.getSequenceFrameCount(name);
//------------------------------------------------------------------------------
"Get the number of keyframes in the sequence.
   @param name name of the sequence to query
   @return number of keyframes in the sequence
   Example:
   "echo( \"Run has \" @ %this.getSequenceFrameCount( \"run\" ) @ \" keyframes\" );

//------------------------------------------------------------------------------

//==============================================================================
TSShapeConstructor.getSequencePriority(name);
//------------------------------------------------------------------------------
 "Get the priority setting of the sequence.
   @param name name of the sequence to query
   @return priority value of the sequence )

//------------------------------------------------------------------------------

//==============================================================================
TSShapeConstructor.setSequencePriority(name,priority);
//------------------------------------------------------------------------------
"Set the sequence priority.
   @param name name of the sequence to modify
   @param priority new priority value
   @return true if successful, false otherwise )

//------------------------------------------------------------------------------

//==============================================================================
TSShapeConstructor.getSequenceGroundSpeed(name);
//------------------------------------------------------------------------------
 "Get the ground speed of the sequence.
   @note Note that only the first 2 ground frames of the sequence are "
   "examined; the speed is assumed to be constant throughout the sequence.
   @param name name of the sequence to query
   @return string of the form: \"trans.x trans.y trans.z rot.x rot.y rot.z\"
   Example:
   %speed = VectorLen( getWords( %this.getSequenceGroundSpeed( \"run\" ), 0, 2 ) );
   "   echo( \"Run moves at \" @ %speed @ \" units per frame\" );

//------------------------------------------------------------------------------

//==============================================================================
TSShapeConstructor.setSequenceGroundSpeed(name,transSpeed,rotSpeed);
//------------------------------------------------------------------------------
"Set the translation and rotation ground speed of the sequence.
   "The ground speed of the sequence is set by generating ground transform "
   "keyframes. The ground translational and rotational speed is assumed to "
   "be constant for the duration of the sequence. Existing ground frames for "
   "the sequence (if any) will be replaced.
   @param name name of the sequence to modify
   @param transSpeed translational speed (trans.x trans.y trans.z) in "
   "Torque units per frame
   @param rotSpeed (optional) rotational speed (rot.x rot.y rot.z) in "
   "radians per frame. Default is \"0 0 0\"
   @return true if successful, false otherwise
   Example:
   %this.setSequenceGroundSpeed( \"run\", \"5 0 0\" );
   %this.setSequenceGroundSpeed( \"spin\", \"0 0 0\", \"4 0 0\" );

//------------------------------------------------------------------------------
//==============================================================================
TSShapeConstructor.getSequenceCyclic(name);
//------------------------------------------------------------------------------
"Check if this sequence is cyclic (looping).
   @param name name of the sequence to query
   @return true if this sequence is cyclic, false if not
   Example:
   "if ( !%this.getSequenceCyclic( \"ambient\" ) )
   "   error( \"ambient sequence is not cyclic!\" );

//------------------------------------------------------------------------------

//==============================================================================
TSShapeConstructor.setSequenceCyclic(name,cyclic);
//------------------------------------------------------------------------------
"Mark a sequence as cyclic or non-cyclic.
   @param name name of the sequence to modify
   @param cyclic true to make the sequence cyclic, false for non-cyclic
   @return true if successful, false otherwise
   Example:
   %this.setSequenceCyclic( \"ambient\", true );
   %this.setSequenceCyclic( \"shoot\", false );

//------------------------------------------------------------------------------

//==============================================================================
TSShapeConstructor.getSequenceBlend(name);
//------------------------------------------------------------------------------
"Get information about blended sequences.
   @param name name of the sequence to query
   @return TAB delimited string of the form: \"isBlend blendSeq blendFrame\", where:"
   "<dl>"
   "<dt>blend_flag</dt><dd>a boolean flag indicating whether this sequence is a blend</dd>"
   "<dt>blend_seq_name</dt><dd>the name of the sequence that contains the reference "
   "frame (empty for blend sequences embedded in DTS files)</dd>"
   "<dt>blend_seq_frame</dt><dd>the blend reference frame (empty for blend sequences "
   "embedded in DTS files)</dd>"
   "</dl>
   @note Note that only sequences set to be blends using the setSequenceBlend "
   "command will contain the blendSeq and blendFrame information.
   Example:
   %blendData = %this.getSequenceBlend( \"look\" );
   "if ( getField( %blendData, 0 ) )
   "   echo( \"look is a blend, reference: \" @ getField( %blendData, 1 ) );

//------------------------------------------------------------------------------

//==============================================================================
TSShapeConstructor.setSequenceBlend(name,blend,blendSeq,blendFrame);
//------------------------------------------------------------------------------
"Mark a sequence as a blend or non-blend.
   "A blend sequence is one that will be added on top of any other playing "
   "sequences. This is done by storing the animated node transforms relative "
   "to a reference frame, rather than as absolute transforms.
   @param name name of the sequence to modify
   @param blend true to make the sequence a blend, false for a non-blend
   @param blendSeq the name of the sequence that contains the blend reference frame
   @param blendFrame the reference frame in the blendSeq sequence
   @return true if successful, false otherwise
   Example:
   %this.setSequenceBlend( \"look\", true, \"root\", 0 );

//------------------------------------------------------------------------------

//==============================================================================
TSShapeConstructor.renameSequence(oldName,newName);
//------------------------------------------------------------------------------
"Rename a sequence.
   @note Note that sequence names must be unique, so this command will fail "
   "if there is already a sequence with the desired name
   @param oldName current name of the sequence
   @param newName new name of the sequence
   @return true if successful, false otherwise
   Example:
   %this.renameSequence( \"walking\", \"walk\" );
//------------------------------------------------------------------------------

//==============================================================================
TSShapeConstructor.addSequence(source,name,start,end,padRot,padTrans);
//------------------------------------------------------------------------------
 "Add a new sequence to the shape.
   @param source the name of an existing sequence, or the name of a DTS or DAE "
   "shape or DSQ sequence file. When the shape file contains more than one "
   "sequence, the desired sequence can be specified by appending the name to the "
   "end of the shape file. eg. \"myShape.dts run\" would select the \"run\" "
   "sequence from the \"myShape.dts\" file.
   @param name name of the new sequence
   @param start (optional) first frame to copy. Defaults to 0, the first frame in the sequence.
   @param end (optional) last frame to copy. Defaults to -1, the last frame in the sequence.
   @param padRot (optional) copy root-pose rotation keys for non-animated nodes. This is useful if "
   "the source sequence data has a different root-pose to the target shape, such as if one character was "
   "in the T pose, and the other had arms at the side. Normally only nodes that are actually rotated by "
   "the source sequence have keyframes added, but setting this flag will also add keyframes for nodes "
   "that are not animated, but have a different root-pose rotation to the target shape root pose.
   @param padTrans (optional) copy root-pose translation keys for non-animated nodes.  This is useful if "
   "the source sequence data has a different root-pose to the target shape, such as if one character was "
   "in the T pose, and the other had arms at the side. Normally only nodes that are actually moved by "
   "the source sequence have keyframes added, but setting this flag will also add keyframes for nodes "
   "that are not animated, but have a different root-pose position to the target shape root pose.
   @return true if successful, false otherwise
   Example:
   %this.addSequence( \"./testShape.dts ambient\", \"ambient\" );
   %this.addSequence( \"./myPlayer.dae run\", \"run\" );
   %this.addSequence( \"./player_look.dsq\", \"look\", 0, -1 );     // start to end
   %this.addSequence( \"walk\", \"walk_shortA\", 0, 4 );            // start to frame 4
   %this.addSequence( \"walk\", \"walk_shortB\", 4, -1 );           // frame 4 to end

//------------------------------------------------------------------------------

//==============================================================================
TSShapeConstructor.removeSequence(name);
//------------------------------------------------------------------------------
"Remove the sequence from the shape.
   @param name name of the sequence to remove
   @return true if successful, false otherwise )

//------------------------------------------------------------------------------



//-----------------------------------------------------------------------------
// TRIGGERS
DefineTSShapeConstructorMethod( getTriggerCount, S32, ( const char* name ),,
   ( name ), 0,
   "Get the number of triggers in the specified sequence.
   @param name name of the sequence to query
   @return number of triggers in the sequence )
{
   GET_SEQUENCE( getTriggerCount, seq, name, 0 );
   return seq->numTriggers;
}}

DefineTSShapeConstructorMethod( getTrigger, const char*, ( const char* name, S32 index ),,
   ( name, index ), "",
   "Get information about the indexed trigger
   @param name name of the sequence to query
   @param index index of the trigger (valid range is 0 - getTriggerCount()-1)
   @return string of the form \"frame state\"
   Example:
   // print all triggers in the sequence
   %count = %this.getTriggerCount( \"back\" );
   "for ( %i = 0; %i < %count; %i++ )
   "   echo( %i SPC %this.getTrigger( \"back\", %i ) );

{
   // Find the sequence and return the indexed trigger (frame and state)
   GET_SEQUENCE( getTrigger, seq, name, "" );

   CHECK_INDEX_IN_RANGE( getTrigger, index, seq->numTriggers, "" );

   const TSShape::Trigger& trig = mShape->triggers[seq->firstTrigger + index];
   S32 frame = trig.pos * seq->numKeyframes;
   S32 state = getBinLog2(trig.state & TSShape::Trigger::StateMask) + 1;
   if (!(trig.state & TSShape::Trigger::StateOn))
      state = -state;

   static const U32 bufSize = 32;
   char* returnBuffer = Con::getReturnBuffer(bufSize);
   dSprintf(returnBuffer, bufSize, %d %d", frame, state);
   return returnBuffer;
}}

DefineTSShapeConstructorMethod( addTrigger, bool, ( const char* name, S32 keyframe, S32 state ),,
   ( name, keyframe, state ), false,
   "Add a new trigger to the sequence.
   @param name name of the sequence to modify
   @param keyframe keyframe of the new trigger
   @param state of the new trigger
   @return true if successful, false otherwise
   Example:
   %this.addTrigger( \"walk\", 3, 1 );
   %this.addTrigger( \"walk\", 5, -1 );

{
   if ( !mShape->addTrigger( name, keyframe, state ) )
      return false;

   ADD_TO_CHANGE_SET();
   return true;
}}

DefineTSShapeConstructorMethod( removeTrigger, bool, ( const char* name, S32 keyframe, S32 state ),,
   ( name, keyframe, state ), false,
   "Remove a trigger from the sequence.
   @param name name of the sequence to modify
   @param keyframe keyframe of the trigger to remove
   @param state of the trigger to remove
   @return true if successful, false otherwise
   Example:
   %this.removeTrigger( \"walk\", 3, 1 );

{
   if ( !mShape->removeTrigger( name, keyframe, state ) )
      return false;

   ADD_TO_CHANGE_SET();
   return true;
}}





































*/
