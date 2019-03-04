#version 330 core

layout(location = 0) in vec3 aPosition;

//...However, they aren't needed for the vertex shader itself.
//Instead, we create an output variable so we can send that data to the fragment shader.
out vec3 texCoord;

uniform mat4 transform;

void main(void)
{
	//Then, we further the input texture coordinate to the output one.
	//texCoord can now be used in the fragment shader.
	texCoord = aPosition+vec3(0.5f,0.5f,0.5f);

    gl_Position = vec4(aPosition, 1.0)*transform;
}