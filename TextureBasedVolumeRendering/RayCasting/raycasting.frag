#version 400

in vec3 EntryPoint;

uniform sampler2D ExitPoints;
//uniform sampler3D VolumeTex;
//uniform float     StepSize;
uniform vec2      ScreenSize;
layout (location = 0) out vec4 FragColor;

void main()
{
    vec3 exitPoint = texture(ExitPoints, gl_FragCoord.st/ScreenSize).xyz;
    if (EntryPoint == exitPoint)
    	discard;
     FragColor = vec4(EntryPoint, 1.0);
   
}
