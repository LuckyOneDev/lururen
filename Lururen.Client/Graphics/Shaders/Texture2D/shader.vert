﻿#version 400

layout(location = 0) in vec2 aPosition;
layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 pivot;

uniform float layer = 0;

void main(void)
{
    texCoord = aTexCoord;
    gl_Position =  vec4(aPosition, layer, 1.0) * pivot * model * view * projection;
}   