shader_type canvas_item;

uniform vec3 replacement_color1:source_color=vec3(1.0,1.0,1.0);
uniform vec3 replacement_color2:source_color=vec3(1.0,1.0,1.0);

vec3 ChangeColor(vec3 color,vec3 pick_color,vec3 replacement_color,float offset){
	vec3 reverse_color=color.rgb-pick_color;
	float amount_reverse_color=length(reverse_color);
	float reverse_color_mask=step(offset,amount_reverse_color);
	vec3 pick_color_mask=(1.0-reverse_color_mask)*color.rgb;
	vec3 retained_reverse_color=color.rgb*reverse_color_mask;
	vec3 replacement_color_mask=replacement_color*pick_color_mask;
	return retained_reverse_color+replacement_color_mask;
}

void fragment() {
	vec3 replacement1=ChangeColor(COLOR.rgb,vec3(0.533,0.627,0.263),replacement_color1,0.15);
	vec3 replacement2=ChangeColor(replacement1,vec3(0.196,0.22,0.129),replacement_color1,0.1);
	vec3 replacement3=ChangeColor(replacement2,vec3(0.769,0.247,0.192),replacement_color2,0.1);

	COLOR.rgb=replacement3;

}
