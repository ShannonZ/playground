#version 330 core

in vec3 pos;
uniform mat4 MVP;

void main(void)
{
    gl_Position = MVP*vec4(pos, 1.0);
}