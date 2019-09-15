#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler s0;

struct VertexShaderOutput
{  
    float4 Position : SV_POSITION;  
    float4 Color : COLOR0;  
    float2 TextureCoordinates : TEXCOORD0;
};

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 color = tex2D(s0, input.TextureCoordinates);
    color.rgb = color.r;
    return color;
}

technique PostProcess
{
    pass P0
    {  
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}