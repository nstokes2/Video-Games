//-----------------------------------------------------------------------------
// SkinnedModel.fx
//
// Microsoft Game Technology Group
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
//#include "Includes.inc"



struct Light 
{
    float4 color;
    float4 position;
    float falloff;
    float range;
};



shared float4x4 view;
shared float4x4 projection;
shared float3 cameraPosition;
shared float4 ambientLightColor;
shared float numLights = 1;

int numOfLights = 1;
Light lights[10];

float4 lcolor[10];
float4 lposition[10];
float lfalloff[10];
float lrange[10];

// Maximum number of bone matrices we can render using shader 2.0 in a single pass.
// If you change this, update SkinnedModelProcessor.cs to match.
#define MaxBones 72

bool isBlend = false;
// Input parameters.
float4x4 View;
float4x4 Projection;



float4x4 LightViewProj;
float3 LightDirection;
float3 LightColor = float3(255.0f, 255.0f, 0.0f);
bool outside = false;
float4 AmbientColor4 = float4(0.015, 0.015, 0.015, 0);
float DepthBias = 0.001f;
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



float4x4 Bones[MaxBones];

float3 Light1Direction = normalize(float3(1, 1, -2));
float3 Light1Color = float3(0.9, 0.8, 0.7);

float3 Light2Direction = normalize(float3(-1, -1, 1));
float3 Light2Color = float3(0.1, 0.3, 0.8);

float3 AmbientColor = 0.2;

float4 AmbientLightColor; 

texture Texture;


sampler Sampler = sampler_state
{
    Texture = (Texture);

    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
};


float4 CalculateSingleLight(Light light, float3 worldPosition, float3 worldNormal, 
                            float4 diffuseColor, float4 specularColor )
{
     float3 lightVector = light.position - worldPosition;
     float lightDist = length(lightVector);
     float3 directionToLight = normalize(lightVector);
     
     //calculate the intensity of the light with exponential falloff
     float baseIntensity = pow(saturate((light.range - lightDist) / light.range),
                                 light.falloff);
     
     
     //float diffuseIntensity = saturate( dot(directionToLight, worldNormal));
     float diffuseIntensity = saturate(dot(directionToLight, worldNormal));
     float4 diffuse = diffuseIntensity * light.color * diffuseColor;

     //calculate Phong components per-pixel
     float3 reflectionVector = normalize(reflect(-directionToLight, worldNormal));
     float3 directionToCamera = normalize(cameraPosition - worldPosition);
     
     //calculate specular component
     //float4 specular = saturate(light.color * specularColor * specularIntensity * 
       //                pow(saturate(dot(reflectionVector, directionToCamera)), 
         //                  specularPower));
                           
     return  baseIntensity * (diffuse);
}

struct CreateShadowMap_VSOut
{
    float4 Position : POSITION;
    float Depth     : TEXCOORD0;
};

// Vertex shader input structure.
struct shadow_INPUT
{
    float4 Position : POSITION0;
    
    
    float4 BoneIndices : BLENDINDICES0;
    float4 BoneWeights : BLENDWEIGHT0;
};
// Transforms the model into light space an renders out the depth of the object
CreateShadowMap_VSOut CreateShadowMap_VertexShader(shadow_INPUT input)
{
    CreateShadowMap_VSOut Out;
    
        // Blend between the weighted bone matrices.
    float4x4 skinTransform = 0;
    
    skinTransform += Bones[input.BoneIndices.x] * input.BoneWeights.x;
    skinTransform += Bones[input.BoneIndices.y] * input.BoneWeights.y;
    skinTransform += Bones[input.BoneIndices.z] * input.BoneWeights.z;
    skinTransform += Bones[input.BoneIndices.w] * input.BoneWeights.w;
    
    Out.Position = mul(input.Position, mul(skinTransform, LightViewProj)); 
    Out.Depth = Out.Position.z / Out.Position.w;    
    return Out;
}

// Saves the depth value out to the 32bit floating point texture
float4 CreateShadowMap_PixelShader(CreateShadowMap_VSOut input) : COLOR
{ 
    return float4(input.Depth, 0, 0, 0);
}

/*
// Vertex shader input structure.
struct VS_INPUT
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TexCoords : TEXCOORD0;
    float4 BoneIndices : BLENDINDICES0;
    float4 BoneWeights : BLENDWEIGHT0;
};


// Vertex shader output structure.
struct VS_OUTPUT
{
   // float4 Position : POSITION0;
    //float4 Lighting : COLOR0;
    //float2 TexCoord : TEXCOORD0;
    
    // float3 WorldNormal : TEXCOORD1;
   //  float3 WorldPosition : TEXCOORD2;
   
     float3 Lighting : COLOR0;
     float4 World : POSITION0;
     float2 TexCoords : TEXCOORD0;
     float3 Normal : TEXCOORD1;
     float3 Position : TEXCOORD2;
};


// Vertex shader program.
VS_OUTPUT VertexShader(VS_INPUT input)
{
    VS_OUTPUT output;
    
    // Blend between the weighted bone matrices.
    float4x4 skinTransform = 0;
    
    skinTransform += Bones[input.BoneIndices.x] * input.BoneWeights.x;
    skinTransform += Bones[input.BoneIndices.y] * input.BoneWeights.y;
    skinTransform += Bones[input.BoneIndices.z] * input.BoneWeights.z;
    skinTransform += Bones[input.BoneIndices.w] * input.BoneWeights.w;
    
    // Skin the vertex position.
    float4 position = mul(input.Position, skinTransform);
    
    //output.Position = mul(mul(position, View), Projection);

    // Skin the vertex normal, then compute lighting.
    float3 normal = normalize((mul(input.Normal, skinTransform)));
    
    //float3 light1 = max(dot(normal, Light1Direction), 0) * Light1Color;
    //float3 light2 = max(dot(normal, Light2Direction), 0) * Light2Color;

   // output.Lighting = light1 + light2 + AmbientColor;
     //  float4    diffuseColor = tex2D(Sampler, input.TexCoord);

    	//float4 diffuse = tex2D(Sampler, input.TexCoord);
    //float3 sun=	max(dot(normal, LightDirection), 0) * LightColor;
	//float4 color = AmbientLightColor * diffuseColor;
	//float4 color = (sun, 0);
	
	
     //float4 worldNormal =  mul(normal, skinTransform);
     //float4 worldPosition =  mul(position, skinTransform);
    
     //output.WorldPosition = worldPosition / worldPosition.w;
//	    for(int i=0; i< numLights; i++)
 //   {
   //     color += CalculateSingleLight(lights[i], 
     //             position, normal,
       //           diffuseColor, diffuseColor );
   // }
   
	float3 light1 = max(dot(normal, Light1Direction), 0) * Light1Color;
    float3 light2 = max(dot(normal, Light2Direction), 0) * Light2Color;

    output.Lighting = light1 + light2 + AmbientColor;
    
    output.Normal = normal;
    output.Position = position/position.w;
    //output.Lighting = color;
   // output.TexCoord = input.TexCoord;
   output.World = position;
   output.TexCoords = input.TexCoords;
    
    return output;
}


// Pixel shader input structure.
struct PS_INPUT
{
    // float4 World : POSITION;
    float3 Lighting : COLOR0;
     float2 TexCoords : TEXCOORD0;
     float3 Normal : TEXCOORD1;
     float3 Position : TEXCOORD2;
};


// Pixel shader program.
float4 PixelShader(PS_INPUT input) : COLOR0
{
   // float4 color = tex2D(Sampler, input.TexCoords);

   // color.rgb *= input.Lighting;
 //  float3 sun1=	max(dot(input.Normal, Light1Direction), 0) * Light1Color;
 //  float3 sun2=	max(dot(input.Normal, Light2Direction), 0) * Light2Color;
   float4    diffuseColor = tex2D(Sampler, input.TexCoords);
   
  // float4 color = AmbientLightColor * diffuseColor  + (sun, 1);
  // 	    for(int i=0; i< numLights; i++)
   // {
     //   color += CalculateSingleLight(lights[i], 
       //           input.Position, input.Normal,
         //         diffuseColor, diffuseColor );
    //}
   // float3 color = sun1 + sun2 + AmbientColor;
  //  diffuseColor.rgb *= color;
   //diffuseColor.a = 255.0;
   
   diffuseColor.rgb * input.Lighting;
   
   
    

        
    
    return diffuseColor;
}
*/


// Vertex shader input structure.
struct VS_INPUT
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;
    float4 BoneIndices : BLENDINDICES0;
    float4 BoneWeights : BLENDWEIGHT0;
};


// Vertex shader output structure.
struct VS_OUTPUT
{
    float4 Position : POSITION0;
    float3 Lighting : COLOR0;
    float2 TexCoord : TEXCOORD0;
    
    float3 Normal : TEXCOORD1;
    float3 Position3 : TEXCOORD2;
};


// Vertex shader program.
VS_OUTPUT VertexShader(VS_INPUT input)
{
    VS_OUTPUT output;
    
    // Blend between the weighted bone matrices.
    float4x4 skinTransform = 0;
    
    skinTransform += Bones[input.BoneIndices.x] * input.BoneWeights.x;
    skinTransform += Bones[input.BoneIndices.y] * input.BoneWeights.y;
    skinTransform += Bones[input.BoneIndices.z] * input.BoneWeights.z;
    skinTransform += Bones[input.BoneIndices.w] * input.BoneWeights.w;
    
    // Skin the vertex position.
    float4 position = mul(input.Position, skinTransform);
    
    output.Position = mul(mul(position, View), Projection);

    // Skin the vertex normal, then compute lighting.
    float3 normal = normalize(mul(input.Normal, skinTransform));
    
    float3 light1 = max(dot(normal, Light1Direction), 0) * Light1Color;
    float3 light2 = max(dot(normal, Light2Direction), 0) * Light2Color;

    output.Lighting = light1 + light2 + AmbientColor;

    output.TexCoord = input.TexCoord;
    output.Normal = normal;
    output.Position3 = position/position.w;
    
    return output;
}


// Pixel shader input structure.
struct PS_INPUT
{
    float3 Lighting : COLOR0;
    float2 TexCoord : TEXCOORD0;
    float3 Normal : TEXCOORD1;
    float3 Position : TEXCOORD2;
};


// Pixel shader program.
float4 PixelShader(PS_INPUT input) : COLOR0
{
    float4 diffuseColor = tex2D(Sampler, input.TexCoord);
    float4 color;
float3 light1 = max(dot(input.Normal, Light1Direction), 0) * Light1Color;
float3 light2 = max(dot(input.Normal, Light2Direction), 0) * Light2Color;
float3 combinedLights;
combinedLights = light1 + light2 + AmbientColor;
   // color.rgb *= (light1 + light2 + AmbientColor);
   // color *= (AmbientColor, 1);
    
    float3 lightVector;
    float lightDist;
    float3 directionToLight;
    float baseIntensity;
    float diffuseIntensity;
    float4 diffuse;
      	    for(int i=0; i< numLights; i++)
    {
    Light alight;
    alight.color = lcolor[i];
    alight.position = lposition[i];
    alight.falloff =  lfalloff[i];
    alight.range =  lrange[i];
    
       lightVector = (lposition[i]/lposition[i].w) - input.Position;
      lightDist = length(lightVector);
      directionToLight = normalize(lightVector);
           //calculate the intensity of the light with exponential falloff
      baseIntensity = pow(saturate((lrange[i] - lightDist) / lrange[i]),
                                 lfalloff[i]);
     
     
      diffuseIntensity = saturate( dot(directionToLight, input.Normal));
      diffuse = diffuseIntensity * lcolor[i] * diffuseColor;
      
      
      
     
    
        //combinedLights += CalculateSingleLight(alight, 
          //        input.Position, input.Normal,
            //      diffuseColor, diffuseColor ).rgb;
             
         combinedLights += diffuse.rgb;    
    }
    diffuseColor.rgb *= combinedLights;
    
    diffuseColor.a = 1;
    return diffuseColor;
}



technique SkinnedModelTechnique
{
    pass SkinnedModelPass
    {
        VertexShader = compile vs_3_0 VertexShader();
        PixelShader = compile ps_3_0 PixelShader();
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