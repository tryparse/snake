#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler inputTexture;
float2 pov;

struct VertexShaderOutput
{  
    float4 Position : SV_POSITION;  
    float4 Color : COLOR0;  
    float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 pixel = tex2D(inputTexture, input.TextureCoordinates);

	if (input.TextureCoordinates.x > pov.x)
	{
		pixel.r = pixel.r * 0.7f;
		pixel.g = pixel.g * 0.7f;
		pixel.b = pixel.b * 0.7f;
	}

	return pixel;
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};