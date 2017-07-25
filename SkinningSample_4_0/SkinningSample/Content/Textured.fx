float Alpha;


//bool isBlend;


float3 Light1Direction = normalize(float3(1, 1, -2));
float3 Light1Color = float3(0.9, 0.8, 0.7);
float3 Light2Direction = normalize(float3(-1, -1, 1));
float3 Light2Color = float3(0.1, 0.3, 0.8);

float4x4 View;

float4x4 Projection;
float4x4 World;


float TexAmbientColor = 0.2;


float4x4 LightViewProj;

float3 LightDirection;
//float4 AmbientColor = float4(0.15, 0.15, 0.15, 0);

float4 AmbientColor = float4(0.08, 0.08, 0.08, 0);

//float3 TexAmbientColor = 0.2;

float DepthBias = 0.001f;


//------- Texture Samplers --------
Texture Texture;


sampler Sampler = sampler_state
{
    Texture = (Texture);

    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = wrap;
    AddressV = wrap;
};



// Vertex shader input structure.
struct shadow_INPUT
{
    float4 Position : POSITION0;
    
    
    float4 BoneIndices : BLENDINDICES0;
    float4 BoneWeights : BLENDWEIGHT0;
};

struct CreateShadowMap_VSOut
{
    float4 Position : POSITION;
    float Depth     : TEXCOORD0;
};

// Transforms the model into light space an renders out the depth of the object
CreateShadowMap_VSOut CreateShadowMap_VertexShader(shadow_INPUT input)
{
    CreateShadowMap_VSOut Out;
    
    
    Out.Position = mul(input.Position, mul(World, LightViewProj)); 
    Out.Depth = Out.Position.z / Out.Position.w;    
    return Out;
}

// Saves the depth value out to the 32bit floating point texture
float4 CreateShadowMap_PixelShader(CreateShadowMap_VSOut input) : COLOR
{ 
    return float4(input.Depth, 0, 0, 0);
}



//------- Technique: Textured --------
struct TexVertexToPixel
{
    float4 Position   	: POSITION;    
    float4 Color		: COLOR0;
    float2 TextureCoords: TEXCOORD0;
};

struct TexPixelToFrame
{
    float4 Color : COLOR0;
   // float2 TextureCoords : TEXCOORD0;
};


// Vertex shader input structure.
struct VS_INPUT
{
    float4 Position : POSITION0;
   // float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;
        float4 Color		: COLOR0;
    //float4 BoneIndices : BLENDINDICES0;
   // float4 BoneWeights : BLENDWEIGHT0;
};


// Vertex shader output structure.
struct VS_OUTPUT
{
    float4 Position : POSITION0;
    float3 Lighting : COLOR0;
    float2 TexCoord : TEXCOORD0;
};


VS_OUTPUT VertexShader(VS_INPUT input)
{
    VS_OUTPUT output;
    
    
    // Skin the vertex position.
    //float4 position = mul(input.Position, skinTransform);
    
    //output.Position = mul(mul(position, View), Projection);
    float4x4 ViewProjection = mul( View, Projection);
    float4x4 WorldViewProjection = mul(World, ViewProjection);
    output.Position = mul(input.Position, WorldViewProjection);
    

    // Skin the vertex normal, then compute lighting.
    //float3 normal = normalize(mul(input.Normal, skinTransform));
    
   float3 normal = float3( 0.0, 1.0, 0.0);
    float3 light1 = max(dot(normal, Light1Direction), 0) * Light1Color;
    float3 light2 = max(dot(normal, Light2Direction), 0) * Light2Color;

    output.Lighting = light1 + light2 + TexAmbientColor;

    output.TexCoord = input.TexCoord;
    
    return output;






}

TexVertexToPixel TexturedVS( float4 inPos : POSITION, float3 inNormal: NORMAL, float2 inTexCoords: TEXCOORD0)
{	
	TexVertexToPixel Output = (TexVertexToPixel)0;
	float4x4 ViewProjection = mul (View, Projection);
	float4x4 WorldViewProjection = mul (World, ViewProjection);
    
	Output.Position = mul(inPos, WorldViewProjection);	
	Output.TextureCoords = inTexCoords;
	
	float3 normal = normalize(mul(normalize(inNormal), World));	
	    
    float3 light1 = max(dot(normal, Light1Direction), 0) * Light1Color;
    float3 light2 = max(dot(normal, Light2Direction), 0) * Light2Color;

   Output.Color.rgb = light1 + light2 + TexAmbientColor;
   
  // if(isBlend)
   Output.Color.a = 1;
   Output.TextureCoords = inTexCoords;
	return Output;     
}

// Pixel shader input structure.
struct PS_INPUT
{
    float3 Lighting : COLOR0;
    float2 TexCoord : TEXCOORD0;
};


// Pixel shader program.
float4 PixelShader(PS_INPUT input) : COLOR0
{
    float4 color = tex2D(Sampler, input.TexCoord);

    color.rgb *= input.Lighting;
    
    return color;
}

TexPixelToFrame TexturedPS(TexVertexToPixel PSIn) 
{
	TexPixelToFrame Output = (TexPixelToFrame)0;		
    
	Output.Color = tex2D(Sampler, PSIn.TextureCoords);
	Output.Color.rgb *= PSIn.Color.rgb;
	//if(!isBlend) //if not blending go to 1 else keep the color
	//Output.Color.a = PSIn.Color.a;

	return Output;
}

texture ShadowMap;
sampler ShadowMapSampler = sampler_state
{
    Texture = <ShadowMap>;
    MinFilter = POINT;
    MagFilter = POINT;
    MipFilter = NONE;
    AddressU = Clamp;
    AddressV = Clamp;
};



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
    float4 diffuseColor = tex2D(Sampler, input.TexCoord);
    // Intensity based on the direction of the light
    float diffuseIntensity = saturate(dot(LightDirection, input.Normal));
    
    diffuseIntensity = max(dot(input.Normal, LightDirection), 1);
    float3 light1 = max(dot(input.Normal, Light1Direction), 1) * Light1Color;
    float3 light2 = max(dot(input.Normal, Light2Direction), 1) * Light2Color;
    // Final diffuse color with ambient color added
    //float4 diffuse = diffuseIntensity * diffuseColor + (light1,0)+ AmbientColor;
    
    
    
    float4 diffuse = (diffuseIntensity   + (light1,0) + (light2, 0) ) * diffuseColor;
    
    //diffuse *= diffuseColor;
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
    
    // Check to see if this pixel is in front or behind the value in the shadow map
    if (shadowdepth < ourdepth)
    {
        // Shadow the pixel by lowering the intensity
        diffuse *= float4(0.5,0.5,0.5,0);
    };
    
    return diffuse;
}

technique Textured
{
	//pass Pass0
    //{   
    //	VertexShader = compile vs_1_1 TexturedVS();
      //  PixelShader  = compile ps_1_1 TexturedPS();
   // }
   
    pass SkinnedModelPass
    {
        VertexShader = compile vs_2_0 VertexShader();
        PixelShader = compile ps_2_0 PixelShader();
    }
}

// Technique for creating the shadow map
technique CreateShadowMap
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 CreateShadowMap_VertexShader();
        PixelShader = compile ps_2_0 CreateShadowMap_PixelShader();
    }
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
