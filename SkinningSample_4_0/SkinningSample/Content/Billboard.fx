//-----------------------------------------------------------------------------
// Billboard.fx
//
// Microsoft Game Technology Group
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------


// Camera parameters.
float4x4 View;
float4x4 Projection;

//Shadowstuff for some reason u werent using worldinbillboard.fx
float4x4 World;
float4x4 LightViewProj;
float DepthBias = 0.001f;


// Lighting parameters.
float3 LightDirection;
float3 LightColor = 0.8;
float3 AmbientColor = 0.4;
texture ShadowMap;
sampler ShadowMapSampler = sampler_state
{
    Texture = <ShadowMap>;
};


// Parameters controlling the wind effect.
float3 WindDirection = float3(1, 0, 0);
float WindWaveSize = 0.1;
float WindRandomness = 1;
float WindSpeed = 4;
float WindAmount;
float WindTime;


// 1 means we should only accept opaque pixels.
// -1 means only accept transparent pixels.
float AlphaTestDirection = 1;
float AlphaTestThreshold = 0.95;


// Parameters describing the billboard itself.
float BillboardWidth;
float BillboardHeight;

texture Texture;


struct VS_INPUT
{
    float3 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;
    float Random : TEXCOORD1;
};


struct VS_OUTPUT
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
    float4 Color : COLOR0;
};


VS_OUTPUT VertexShaderFunction(VS_INPUT input)
{
    VS_OUTPUT output;

    // Apply a scaling factor to make some of the billboards
    // shorter and fatter while others are taller and thinner.
    float squishFactor = 0.75 + abs(input.Random) / 2;

    float width = BillboardWidth * squishFactor;
    float height = BillboardHeight / squishFactor;

    // Flip half of the billboards from left to right. This gives visual variety
    // even though we are actually just repeating the same texture over and over.
    if (input.Random < 0)
        width = -width;

    // Work out what direction we are viewing the billboard from.
    float3 viewDirection = View._m02_m12_m22;

    float3 rightVector = normalize(cross(viewDirection, input.Normal));

    // Calculate the position of this billboard vertex.
    float3 position = input.Position;

    // Offset to the left or right.
    position += rightVector * (input.TexCoord.x - 0.5) * width;
    
    // Offset upward if we are one of the top two vertices.
    position += input.Normal * (1 - input.TexCoord.y) * height;

    // Work out how this vertex should be affected by the wind effect.
    float waveOffset = dot(position, WindDirection) * WindWaveSize;
    
    waveOffset += input.Random * WindRandomness;
    
    // Wind makes things wave back and forth in a sine wave pattern.
    float wind = sin(WindTime * WindSpeed + waveOffset) * WindAmount;
    
    // But it should only affect the top two vertices of the billboard!
    wind *= (1 - input.TexCoord.y);
    
    position += WindDirection * wind;

    // Apply the camera transform.
    float4 viewPosition = mul(float4(position, 1), View);

    output.Position = mul(viewPosition, Projection);

    output.TexCoord = input.TexCoord;
    
    // Compute lighting.
    float diffuseLight = max(-dot(input.Normal, LightDirection), 0);
    
    output.Color.rgb = diffuseLight * LightColor + AmbientColor;
    output.Color.a = 1;
    
    return output;
}


sampler TextureSampler = sampler_state
{
    Texture = (Texture);
};


float4 PixelShaderFunction(float2 texCoord : TEXCOORD0, float4 color : COLOR0) : COLOR0
{
    color *= tex2D(TextureSampler, texCoord);

    // Apply the alpha test.
    clip((color.a - AlphaTestThreshold) * AlphaTestDirection);

    return color;
}
struct DrawWithShadowMap_VSIn
{
    float4 Position : POSITION0;
    float3 Normal   : NORMAL0;
    float2 TexCoord : TEXCOORD0;
};

struct DrawWithShadowMap_VSOut
{
    float4 Position : POSITION0;
    float3 Normal   : TEXCOORD0;
    float2 TexCoord : TEXCOORD1;
    float4 WorldPos : TEXCOORD2;
};
// Draws the model with shadows
DrawWithShadowMap_VSOut DrawWithShadowMap_VertexShader(DrawWithShadowMap_VSIn input)
{
    DrawWithShadowMap_VSOut Output;

    float4x4 WorldViewProj = mul(mul(World, View), Projection);
    
    // Transform the models verticies and normal
    Output.Position = mul(input.Position, WorldViewProj);
    Output.Normal =  normalize(mul(input.Normal, World));
    Output.TexCoord = input.TexCoord;
    
    // Save the vertices postion in world space
    Output.WorldPos = mul(input.Position, World);
    
    return Output;
}
// Determines the depth of the pixel for the model and checks to see 
// if it is in shadow or not
float4 DrawWithShadowMap_PixelShader(DrawWithShadowMap_VSOut input) : COLOR
{ 
    // Color of the model
    float4 diffuseColor = tex2D(TextureSampler, input.TexCoord);
    // Intensity based on the direction of the light
    float diffuseIntensity = saturate(dot(LightDirection, input.Normal));
    // Final diffuse color with ambient color added
    float4 diffuse = diffuseIntensity * diffuseColor + (AmbientColor,1);
    
    // Find the position of this pixel in light space
    float4 lightingPosition = mul(input.WorldPos, LightViewProj);
    
    // Find the position in the shadow map for this pixel
    float2 ShadowTexCoord = 0.5 * lightingPosition.xy / 
                            lightingPosition.w + float2( 0.5, 0.5 );
    ShadowTexCoord.y = 1.0f - ShadowTexCoord.y;

    // Get the current depth stored in the shadow map
    float shadowdepth = tex2D(ShadowMapSampler, ShadowTexCoord).r;    
    
    // Calculate the current pixel depth
    // The bias is used to prevent folating point errors that occur when
    // the pixel of the occluder is being drawn
    float ourdepth = (lightingPosition.z / lightingPosition.w) - DepthBias;
    

	diffuse = (1.0, 1.0, 1.0, 1.0);
    // Check to see if this pixel is in front or behind the value in the shadow map
    if (shadowdepth < ourdepth)
    {
        // Shadow the pixel by lowering the intensity
        diffuse *= float4(0.5,0.5,0.5,0);
    };
    
    return diffuse;
}

// Technique for drawing with the shadow map
technique DrawWithShadowMap
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 DrawWithShadowMap_VertexShader();
        PixelShader = compile ps_2_0 DrawWithShadowMap_PixelShader();
    }
}

technique Billboards
{
    pass
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
