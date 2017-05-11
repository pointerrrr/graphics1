//based off of http://www.learnopengles.com/android-lesson-two-ambient-and-diffuse-lighting/
#version 330
uniform mat4 MVP;
uniform mat4 MV;
uniform vec3 LightPos;

in vec3 vPosition;
in vec4 vColor;
in vec3 vNormal;

out vec4 color;
 
void main()
{
 vec3 modelViewVertex = vec3(MVP * vec4( vPosition, 1.0 )); 
 vec3 modelViewNormal = vec3(MVP * vec4(vNormal, 0.0));
 float distance = length(LightPos - modelViewVertex);
 vec3 lightVector = normalize(LightPos - modelViewVertex);
 float diffuse = max(dot(modelViewNormal, lightVector), 0.1);
 diffuse = diffuse * (1.0 / (1.0 + (0.25 * distance * distance)));
 
 gl_Position = MVP * vec4( vPosition, 1.0 );
 color = vColor * diffuse;
}