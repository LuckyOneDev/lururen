#version 400
out vec4 frag_colour;

uniform vec4 ourColor;

void main() {
  frag_colour = ourColor;
}