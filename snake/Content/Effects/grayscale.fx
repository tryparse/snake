#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
    return float4(1, 0, 0, 1);
}

technique PostProcess
{
    pass P0
    {  
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}