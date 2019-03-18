#version 400

in vec3 EntryPoint;

uniform sampler3D VolumeTex;
layout (location = 0) out vec4 FragColor;


void main()
{
	FragColor = texture(VolumeTex,EntryPoint);

	//FragColor = vec4(EntryPoint,1.0);
}