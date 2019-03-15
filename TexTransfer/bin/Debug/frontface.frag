#version 400

in vec3 EntryPoint;

uniform sampler2D ExitPoints;
uniform vec2      ScreenSize;
layout (location = 0) out vec4 FragColor;

void main()
{

    vec3 exitPoint = texture(ExitPoints, gl_FragCoord.st/ScreenSize).xyz;
   
    if (EntryPoint == exitPoint)
    	discard;
   
   //FragColor = vec4(EntryPoint,1.0);
   FragColor = vec4(exitPoint,1.0);
}