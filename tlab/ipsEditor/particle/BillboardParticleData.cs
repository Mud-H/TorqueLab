//==============================================================================
// TorqueLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

/*
spinSpeed spinRandomMin spinRandomMax
useInvAlpha 
animateTexture framesPerSec animTexTiling animTexFrames
textureCoords
HighResTexture MidResTexture LowResTexture textureName
colors times
 addField( "spinSpeed", TYPEID< F32 >(), Offset(spinSpeed, BillboardParticleData),
      "Speed at which to spin the particle." );
   addFieldV( "spinRandomMin", TYPEID< F32 >(), Offset(spinRandomMin, BillboardParticleData), &spinRandFValidator,
      "Minimum allowed spin speed of this particle, between -1000 and spinRandomMax." );
   addFieldV( "spinRandomMax", TYPEID< F32 >(), Offset(spinRandomMax, BillboardParticleData), &spinRandFValidator,
      "Maximum allowed spin speed of this particle, between spinRandomMin and 1000." );
   addField( "useInvAlpha", TYPEID< bool >(), Offset(useInvAlpha, BillboardParticleData),
      "@brief Controls how particles blend with the scene.\n\n"
      "If true, particles blend like ParticleBlendStyle NORMAL, if false, "
      "blend like ParticleBlendStyle ADDITIVE.\n"
      "@note If ParticleEmitterData::blendStyle is set, it will override this value." );
   addField( "animateTexture", TYPEID< bool >(), Offset(animateTexture, BillboardParticleData),
      "If true, allow the particle texture to be an animated sprite." );
   addField( "framesPerSec", TYPEID< S32 >(), Offset(framesPerSec, BillboardParticleData),
      "If animateTexture is true, this defines the frames per second of the "
      "sprite animation." );

   addField( "textureCoords", TYPEID< Point2F >(), Offset(texCoords, BillboardParticleData),  4,
      "@brief 4 element array defining the UV coords into textureName to use "
      "for this particle.\n\n"
      "Coords should be set for the first tile only when using animTexTiling; "
      "coordinates for other tiles will be calculated automatically. \"0 0\" is "
      "top left and \"1 1\" is bottom right." );
   addField( "animTexTiling", TYPEID< Point2I >(), Offset(animTexTiling, BillboardParticleData),
      "@brief The number of frames, in rows and columns stored in textureName "
      "(when animateTexture is true).\n\n"
      "A maximum of 256 frames can be stored in a single texture when using "
      "animTexTiling. Value should be \"NumColumns NumRows\", for example \"4 4\"." );
   addField( "animTexFrames", TYPEID< StringTableEntry >(), Offset(animTexFramesString, BillboardParticleData),
      "@brief A list of frames and/or frame ranges to use for particle "
      "animation if animateTexture is true.\n\n"
      "Each frame token must be separated by whitespace. A frame token must be "
      "a positive integer frame number or a range of frame numbers separated "
      "with a '-'. The range separator, '-', cannot have any whitspace around "
      "it.\n\n"
      "Ranges can be specified to move through the frames in reverse as well "
      "as forward (eg. 19-14). Frame numbers exceeding the number of tiles will "
      "wrap.\n"
      "@tsexample\n"
      "animTexFrames = \"0-16 20 19 18 17 31-21\";\n"
      "@endtsexample\n" );

   addField( "HighResTexture", TYPEID< StringTableEntry >(), Offset(hResTextureName, BillboardParticleData),
      "@brief Texture file to use for this particle." );
   addField( "MidResTexture", TYPEID< StringTableEntry >(), Offset(mResTextureName, BillboardParticleData),
      "@brief Texture file to use for this particle." );
   addField( "LowResTexture", TYPEID< StringTableEntry >(), Offset(lResTextureName, BillboardParticleData),
      "@brief Texture file to use for this particle." );
   addField( "textureName", TYPEID< StringTableEntry >(), Offset(hResTextureName, BillboardParticleData),
      "@brief Texture file to use for this particle.\n\n"
      "Deprecated. use HighResTexture instead." );
   addField( "animTexName", TYPEID< StringTableEntry >(), Offset(hResTextureName, BillboardParticleData),
      "@brief Texture file to use for this particle if animateTexture is true.\n\n"
      "Deprecated. Use textureName instead." );

   // Interpolation variables
   addField( "colors", TYPEID< ColorF >(), Offset(colors, BillboardParticleData), PDC_NUM_KEYS,
      "@brief Particle RGBA color keyframe values.\n\n"
      "The particle color will linearly interpolate between the color/time keys "
      "over the lifetime of the particle." );
   addProtectedField( "times", TYPEID< F32 >(), Offset(times, BillboardParticleData), &protectedSetTimes, 
      &defaultProtectedGetFn, PDC_NUM_KEYS,
      "@brief Time keys used with the colors and sizes keyframes.\n\n"
      "Values are from 0.0 (particle creation) to 1.0 (end of lifespace)." );
      
bool animateTexture = "0"
  string animTexName = "core/art/particles/sparkle"
  Point2I animTexTiling = "0 0" 
  ColorF colors[ 0 ] = "1 0.795276 0 0.795276"
  ColorF colors[ 1 ] = "1 0.692913 0 0.795276"
  ColorF colors[ 2 ] = "1 0 0 0.19685"
  ColorF colors[ 3 ] = "1 1 1 1"
  float constantAcceleration = "0"
  float dragCoefficient = "0"
  int framesPerSec = "1"
  float gravityCoefficient = "-0.048852"
  bool hidden = "0"
  string HighResTexture = "core/art/particles/sparkle"
  float inheritedVelFactor = "0"
  int lifetimeMS = "3000"
  int lifetimeVarianceMS = "0"
  bool locked = "0"
  string Name = "AxelTestParticle03"
  SimObject parentGroup = "DataBlockGroup"
  pid persistentId = ""
  float sizes[ 0 ] = "0.0488311"
  float sizes[ 1 ] = "0.0976622"
  float sizes[ 2 ] = "0.0488311"
  float sizes[ 3 ] = "1"
  float spinRandomMax = "90"
  float spinRandomMin = "-90"
  float spinSpeed = "0"
  Point2F textureCoords[ 0 ] = "0 0"
  Point2F textureCoords[ 1 ] = "0 1"
  Point2F textureCoords[ 2 ] = "1 1"
  Point2F textureCoords[ 3 ] = "1 0"
  string textureName = "core/art/particles/sparkle"
  float times[ 0 ] = "0"
  float times[ 1 ] = "0.498039"
  float times[ 2 ] = "1"
  float times[ 3 ] = "1"
  bool useInvAlpha = "0"
  float windCoefficient = "0"
  */