//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------


function Lab::validateObjectName( %name, %mustHaveName ) {
	if( %mustHaveName && %name $= "" ) {
		LabMsgOK( "Missing Object Name", "No name given for object.  Please enter a valid object name." );
		return false;
	}

	if( !isValidObjectName( %name ) ) {
		LabMsgOK( "Invalid Object Name", "'" @ %name @ "' is not a valid object name." NL
					 "" NL
					 "Please choose a name that begins with a letter or underscore and is otherwise comprised " @
					 "exclusively of letters, digits, and/or underscores."
				  );
		return false;
	}

	if( isObject( %name ) ) {
		%filename = %name.getFilename();

		if ( %filename $= "" )
			%filename = "an unknown file";

		LabMsgOK( "Invalid Object Name", "Object names must be unique, and there is an " @
					 "existing " @ %name.getClassName() @ " object with the name '" @ %name @ "' (defined " @
					 "in " @ %filename @ ").  Please choose another name." );
		return false;
	}

	if( isClass( %name ) ) {
		LabMsgOK( "Invalid Object Name", "'" @ %name @ "' is the name of an existing TorqueScript " @
					 "class.  Please choose another name." );
		return false;
	}

	return true;
}
//==============================================================================
// still needs to be optimized further
function searchForTexture(%material, %texture) {
	if( %texture !$= "" ) {
		// set the find signal as false to start out with
		%isFile= false;
		// sete the formats we're going to be looping through if need be
		%formats = ".png .jpg .dds .bmp .gif .jng .tga";

		// if the texture contains the correct filepath and name right off the bat, lets use it
		if( isFile(%texture) )
			%isFile = true;
		else {
			for( %i = 0; %i < getWordCount(%formats); %i++) {
				%testFileName = %texture @ getWord( %formats, %i );

				if(isFile(%testFileName)) {
					%isFile = true;
					break;
				}
			}
		}

		// if we didn't grab a proper name, lets use a string logarithm
		if( !%isFile ) {
			%materialDiffuse = %texture;
			%materialDiffuse2 = %texture;
			%materialPath = %material.getFilename();

			if( strchr( %materialDiffuse, "/") $= "" ) {
				%k = 0;

				while( strpos( %materialPath, "/", %k ) != -1 ) {
					%count = strpos( %materialPath, "/", %k );
					%k = %count + 1;
				}

				%materialsCs = getSubStr( %materialPath , %k , 99 );
				%texture =  strreplace( %materialPath, %materialsCs, %texture );
			} else
				%texture =  strreplace( %materialPath, %materialPath, %texture );

			// lets test the pathing we came up with
			if( isFile(%texture) )
				%isFile = true;
			else {
				for( %i = 0; %i < getWordCount(%formats); %i++) {
					%testFileName = %texture @ getWord( %formats, %i );

					if(isFile(%testFileName)) {
						%isFile = true;
						break;
					}
				}
			}

			// as a last resort to find the proper name
			// we have to resolve using find first file functions very very slow
			if( !%isFile ) {
				%k = 0;

				while( strpos( %materialDiffuse2, "/", %k ) != -1 ) {
					%count = strpos( %materialDiffuse2, "/", %k );
					%k = %count + 1;
				}

				%texture =  getSubStr( %materialDiffuse2 , %k , 99 );

				for( %i = 0; %i < getWordCount(%formats); %i++) {
					%searchString = "*" @ %texture @ getWord( %formats, %i );
					%testFileName = findFirstFile( %searchString );

					if( isFile(%testFileName) ) {
						%texture = %testFileName;
						%isFile = true;
						break;
					}
				}
			}

			return %texture;
		} else
			return %texture; //Texture exists and can be found - just return the input argument.
	}

	return ""; //No texture associated with this property.
}
//------------------------------------------------------------------------------