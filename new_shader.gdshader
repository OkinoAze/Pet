shader_type canvas_item;
uniform vec3 pick_color:source_color=vec3(1.0,1.0,1.0);
uniform float offset=0.3;
uniform vec3 replacement_color:source_color=vec3(1.0,1.0,1.0);
ChageColor

void fragment() {
	vec3 reverse_color=COLOR.rgb-pick_color;
	float amount_reverse_color=length(reverse_color);
	float reverse_color_mask=step(offset,amount_reverse_color);
	vec3 pick_color_mask=(1.0-reverse_color_mask)*COLOR.rgb;
	vec3 retained_reverse_color=COLOR.rgb*reverse_color_mask;
	vec3 replacement_color_mask=replacement_color*pick_color_mask;
	vec3 final_color=retained_reverse_color+replacement_color_mask;
	COLOR.rgb=final_color;
}
