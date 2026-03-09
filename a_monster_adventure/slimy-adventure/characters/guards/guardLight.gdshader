shader_type canvas_item;

uniform float arcAngle = 3.1415;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec2 nUV = (UV - vec2(0.5,0.5)) / sqrt(pow(UV.x - 0.5,2)+pow(UV.y - 0.5,2));
	if (abs(atan(nUV.x,nUV.y)) < arcAngle/2.0){
		COLOR = COLOR;
	}
	else{
		COLOR = vec4(0.0,0.0,0.0,1.0);
	}
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
