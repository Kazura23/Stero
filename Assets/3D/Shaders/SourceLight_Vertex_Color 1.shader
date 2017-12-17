// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:1,cgin:,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:1,uamb:True,mssp:True,bkdf:True,hqlp:True,rprd:True,enco:False,rmgx:True,imps:False,rpth:0,vtps:0,hqsc:False,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:33773,y:32756,varname:node_2865,prsc:2|diff-3422-OUT,spec-1109-OUT,gloss-1319-OUT,normal-4740-OUT,emission-3853-OUT,alpha-6709-OUT;n:type:ShaderForge.SFN_VertexColor,id:1312,x:31689,y:33763,varname:node_1312,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:3309,x:31591,y:32147,varname:node_3309,prsc:2,tex:2325e7a2f6ef2504e8d37a3763f19b52,ntxv:0,isnm:False|UVIN-5636-UVOUT,TEX-8193-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:8193,x:31267,y:32145,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_8193,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:2325e7a2f6ef2504e8d37a3763f19b52,ntxv:0,isnm:False;n:type:ShaderForge.SFN_TexCoord,id:179,x:31079,y:32694,varname:node_179,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Rotator,id:5636,x:31301,y:32704,varname:node_5636,prsc:2|UVIN-179-UVOUT,ANG-1585-OUT;n:type:ShaderForge.SFN_Slider,id:1585,x:31015,y:32894,ptovrint:False,ptlb:Rotation,ptin:_Rotation,varname:node_1585,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-10,cur:0,max:1;n:type:ShaderForge.SFN_Tex2d,id:2958,x:31591,y:32469,ptovrint:False,ptlb:MainRM,ptin:_MainRM,varname:node_2958,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5636-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:8214,x:31588,y:32291,ptovrint:False,ptlb:MainNormal,ptin:_MainNormal,varname:node_8214,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True|UVIN-5636-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:2562,x:31757,y:33109,ptovrint:False,ptlb:Detail1_color,ptin:_Detail1_color,varname:node_2562,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-6782-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:2887,x:31757,y:33283,ptovrint:False,ptlb:Detail1_RM,ptin:_Detail1_RM,varname:node_2887,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-6782-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:3089,x:31757,y:33456,ptovrint:False,ptlb:Detail1_Normal,ptin:_Detail1_Normal,varname:node_3089,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:True|UVIN-6782-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:3528,x:31138,y:33174,varname:node_3528,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Rotator,id:6782,x:31540,y:33235,varname:node_6782,prsc:2|UVIN-3528-UVOUT,ANG-8761-OUT;n:type:ShaderForge.SFN_Slider,id:8761,x:31027,y:33459,ptovrint:False,ptlb:Detail1_rotation,ptin:_Detail1_rotation,varname:node_8761,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-10,cur:0,max:10;n:type:ShaderForge.SFN_Lerp,id:7070,x:32291,y:32907,varname:node_7070,prsc:2|A-2562-RGB,B-3309-RGB,T-7746-OUT;n:type:ShaderForge.SFN_Lerp,id:8428,x:32296,y:33207,varname:node_8428,prsc:2|A-2887-RGB,B-8214-RGB,T-7746-OUT;n:type:ShaderForge.SFN_Lerp,id:4740,x:32556,y:33450,varname:node_4740,prsc:2|A-3089-RGB,B-2958-RGB,T-7746-OUT;n:type:ShaderForge.SFN_Tex2d,id:1255,x:31945,y:33747,ptovrint:False,ptlb:Alpha_clip_detail1,ptin:_Alpha_clip_detail1,varname:node_1255,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:7746,x:32089,y:33523,varname:node_7746,prsc:2|A-1312-R,B-1255-R;n:type:ShaderForge.SFN_ComponentMask,id:1319,x:32577,y:33167,varname:node_1319,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-8428-OUT;n:type:ShaderForge.SFN_ComponentMask,id:1109,x:32567,y:32957,varname:node_1109,prsc:2,cc1:1,cc2:-1,cc3:-1,cc4:-1|IN-8428-OUT;n:type:ShaderForge.SFN_Color,id:3156,x:32262,y:32546,ptovrint:False,ptlb:MAinColor,ptin:_MAinColor,varname:node_3156,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:3422,x:32567,y:32755,varname:node_3422,prsc:2|A-3156-RGB,B-7070-OUT,C-5678-OUT;n:type:ShaderForge.SFN_Slider,id:5678,x:32163,y:32445,ptovrint:False,ptlb:COlorforce,ptin:_COlorforce,varname:node_5678,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.506115,max:1;n:type:ShaderForge.SFN_ComponentMask,id:1862,x:32163,y:32250,varname:node_1862,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-3309-R;n:type:ShaderForge.SFN_OneMinus,id:1709,x:32320,y:32260,varname:node_1709,prsc:2|IN-1862-OUT;n:type:ShaderForge.SFN_Slider,id:8902,x:32491,y:32218,ptovrint:False,ptlb:Windows opacity,ptin:_Windowsopacity,varname:node_8902,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5713308,max:1;n:type:ShaderForge.SFN_Add,id:5,x:32815,y:32298,varname:node_5,prsc:2|A-8902-OUT,B-1709-OUT;n:type:ShaderForge.SFN_Clamp01,id:7248,x:32983,y:32321,varname:node_7248,prsc:2|IN-5-OUT;n:type:ShaderForge.SFN_ComponentMask,id:8659,x:32086,y:32709,varname:node_8659,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-2562-R;n:type:ShaderForge.SFN_OneMinus,id:3000,x:32262,y:32719,varname:node_3000,prsc:2|IN-8659-OUT;n:type:ShaderForge.SFN_Add,id:3587,x:32808,y:32442,varname:node_3587,prsc:2|A-8902-OUT,B-3000-OUT;n:type:ShaderForge.SFN_Clamp01,id:9414,x:32997,y:32442,varname:node_9414,prsc:2|IN-3587-OUT;n:type:ShaderForge.SFN_Lerp,id:6709,x:33037,y:32591,varname:node_6709,prsc:2|A-9414-OUT,B-7248-OUT,T-1312-R;n:type:ShaderForge.SFN_OneMinus,id:9033,x:32932,y:33279,varname:node_9033,prsc:2|IN-6709-OUT;n:type:ShaderForge.SFN_Color,id:6554,x:32944,y:33450,ptovrint:False,ptlb:window emission,ptin:_windowemission,varname:node_6554,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.8312373,c3:0.6176471,c4:1;n:type:ShaderForge.SFN_Blend,id:2019,x:33117,y:33279,varname:node_2019,prsc:2,blmd:14,clmp:True|SRC-9033-OUT,DST-6554-RGB;n:type:ShaderForge.SFN_Power,id:3853,x:33312,y:33279,varname:node_3853,prsc:2|VAL-2019-OUT,EXP-5066-OUT;n:type:ShaderForge.SFN_Slider,id:5066,x:33157,y:33578,ptovrint:False,ptlb:EmissionPower,ptin:_EmissionPower,varname:node_5066,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:0;proporder:3156-8193-5678-1585-2958-8214-1255-2562-3089-2887-8761-8902-6554-5066;pass:END;sub:END;*/

Shader "Shader Forge/SourceLight_Vertex_Color " {
    Properties {
        _MAinColor ("MAinColor", Color) = (0.5,0.5,0.5,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _COlorforce ("COlorforce", Range(0, 1)) = 0.506115
        _Rotation ("Rotation", Range(-10, 1)) = 0
        _MainRM ("MainRM", 2D) = "white" {}
        _MainNormal ("MainNormal", 2D) = "bump" {}
        _Alpha_clip_detail1 ("Alpha_clip_detail1", 2D) = "white" {}
        _Detail1_color ("Detail1_color", 2D) = "white" {}
        _Detail1_Normal ("Detail1_Normal", 2D) = "white" {}
        _Detail1_RM ("Detail1_RM", 2D) = "white" {}
        _Detail1_rotation ("Detail1_rotation", Range(-10, 10)) = 0
        _Windowsopacity ("Windows opacity", Range(0, 1)) = 0.5713308
        _windowemission ("window emission", Color) = (1,0.8312373,0.6176471,1)
        _EmissionPower ("EmissionPower", Range(1, 0)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
            "DisableBatching"="True"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _Rotation;
            uniform sampler2D _MainRM; uniform float4 _MainRM_ST;
            uniform sampler2D _MainNormal; uniform float4 _MainNormal_ST;
            uniform sampler2D _Detail1_color; uniform float4 _Detail1_color_ST;
            uniform sampler2D _Detail1_RM; uniform float4 _Detail1_RM_ST;
            uniform sampler2D _Detail1_Normal; uniform float4 _Detail1_Normal_ST;
            uniform float _Detail1_rotation;
            uniform sampler2D _Alpha_clip_detail1; uniform float4 _Alpha_clip_detail1_ST;
            uniform float4 _MAinColor;
            uniform float _COlorforce;
            uniform float _Windowsopacity;
            uniform float4 _windowemission;
            uniform float _EmissionPower;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(7,8)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD9;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float node_6782_ang = _Detail1_rotation;
                float node_6782_spd = 1.0;
                float node_6782_cos = cos(node_6782_spd*node_6782_ang);
                float node_6782_sin = sin(node_6782_spd*node_6782_ang);
                float2 node_6782_piv = float2(0.5,0.5);
                float2 node_6782 = (mul(i.uv0-node_6782_piv,float2x2( node_6782_cos, -node_6782_sin, node_6782_sin, node_6782_cos))+node_6782_piv);
                float3 _Detail1_Normal_var = UnpackNormal(tex2D(_Detail1_Normal,TRANSFORM_TEX(node_6782, _Detail1_Normal)));
                float node_5636_ang = _Rotation;
                float node_5636_spd = 1.0;
                float node_5636_cos = cos(node_5636_spd*node_5636_ang);
                float node_5636_sin = sin(node_5636_spd*node_5636_ang);
                float2 node_5636_piv = float2(0.5,0.5);
                float2 node_5636 = (mul(i.uv0-node_5636_piv,float2x2( node_5636_cos, -node_5636_sin, node_5636_sin, node_5636_cos))+node_5636_piv);
                float4 _MainRM_var = tex2D(_MainRM,TRANSFORM_TEX(node_5636, _MainRM));
                float4 _Alpha_clip_detail1_var = tex2D(_Alpha_clip_detail1,TRANSFORM_TEX(i.uv0, _Alpha_clip_detail1));
                float node_7746 = (i.vertexColor.r*_Alpha_clip_detail1_var.r);
                float3 normalLocal = lerp(_Detail1_Normal_var.rgb,_MainRM_var.rgb,node_7746);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float4 _Detail1_RM_var = tex2D(_Detail1_RM,TRANSFORM_TEX(node_6782, _Detail1_RM));
                float3 _MainNormal_var = UnpackNormal(tex2D(_MainNormal,TRANSFORM_TEX(node_5636, _MainNormal)));
                float3 node_8428 = lerp(_Detail1_RM_var.rgb,_MainNormal_var.rgb,node_7746);
                float gloss = 1.0 - node_8428.r; // Convert roughness to gloss
                float perceptualRoughness = node_8428.r;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                #if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMin[0] = unity_SpecCube0_BoxMin;
                    d.boxMin[1] = unity_SpecCube1_BoxMin;
                #endif
                #if UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMax[0] = unity_SpecCube0_BoxMax;
                    d.boxMax[1] = unity_SpecCube1_BoxMax;
                    d.probePosition[0] = unity_SpecCube0_ProbePosition;
                    d.probePosition[1] = unity_SpecCube1_ProbePosition;
                #endif
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = node_8428.g;
                float specularMonochrome;
                float4 _Detail1_color_var = tex2D(_Detail1_color,TRANSFORM_TEX(node_6782, _Detail1_color));
                float4 node_3309 = tex2D(_MainTex,TRANSFORM_TEX(node_5636, _MainTex));
                float3 diffuseColor = (_MAinColor.rgb*lerp(_Detail1_color_var.rgb,node_3309.rgb,node_7746)*_COlorforce); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                half surfaceReduction;
                #ifdef UNITY_COLORSPACE_GAMMA
                    surfaceReduction = 1.0-0.28*roughness*perceptualRoughness;
                #else
                    surfaceReduction = 1.0/(roughness*roughness + 1.0);
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular);
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                indirectSpecular *= surfaceReduction;
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float node_7248 = saturate((_Windowsopacity+(1.0 - node_3309.r.r)));
                float node_6709 = lerp(saturate((_Windowsopacity+(1.0 - _Detail1_color_var.r.r))),node_7248,i.vertexColor.r);
                float3 emissive = pow(saturate(( (1.0 - node_6709) > 0.5 ? (_windowemission.rgb + 2.0*(1.0 - node_6709) -1.0) : (_windowemission.rgb + 2.0*((1.0 - node_6709)-0.5)))),_EmissionPower);
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                return fixed4(finalColor,node_6709);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _Rotation;
            uniform sampler2D _MainRM; uniform float4 _MainRM_ST;
            uniform sampler2D _MainNormal; uniform float4 _MainNormal_ST;
            uniform sampler2D _Detail1_color; uniform float4 _Detail1_color_ST;
            uniform sampler2D _Detail1_RM; uniform float4 _Detail1_RM_ST;
            uniform sampler2D _Detail1_Normal; uniform float4 _Detail1_Normal_ST;
            uniform float _Detail1_rotation;
            uniform sampler2D _Alpha_clip_detail1; uniform float4 _Alpha_clip_detail1_ST;
            uniform float4 _MAinColor;
            uniform float _COlorforce;
            uniform float _Windowsopacity;
            uniform float4 _windowemission;
            uniform float _EmissionPower;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(7,8)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float node_6782_ang = _Detail1_rotation;
                float node_6782_spd = 1.0;
                float node_6782_cos = cos(node_6782_spd*node_6782_ang);
                float node_6782_sin = sin(node_6782_spd*node_6782_ang);
                float2 node_6782_piv = float2(0.5,0.5);
                float2 node_6782 = (mul(i.uv0-node_6782_piv,float2x2( node_6782_cos, -node_6782_sin, node_6782_sin, node_6782_cos))+node_6782_piv);
                float3 _Detail1_Normal_var = UnpackNormal(tex2D(_Detail1_Normal,TRANSFORM_TEX(node_6782, _Detail1_Normal)));
                float node_5636_ang = _Rotation;
                float node_5636_spd = 1.0;
                float node_5636_cos = cos(node_5636_spd*node_5636_ang);
                float node_5636_sin = sin(node_5636_spd*node_5636_ang);
                float2 node_5636_piv = float2(0.5,0.5);
                float2 node_5636 = (mul(i.uv0-node_5636_piv,float2x2( node_5636_cos, -node_5636_sin, node_5636_sin, node_5636_cos))+node_5636_piv);
                float4 _MainRM_var = tex2D(_MainRM,TRANSFORM_TEX(node_5636, _MainRM));
                float4 _Alpha_clip_detail1_var = tex2D(_Alpha_clip_detail1,TRANSFORM_TEX(i.uv0, _Alpha_clip_detail1));
                float node_7746 = (i.vertexColor.r*_Alpha_clip_detail1_var.r);
                float3 normalLocal = lerp(_Detail1_Normal_var.rgb,_MainRM_var.rgb,node_7746);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float4 _Detail1_RM_var = tex2D(_Detail1_RM,TRANSFORM_TEX(node_6782, _Detail1_RM));
                float3 _MainNormal_var = UnpackNormal(tex2D(_MainNormal,TRANSFORM_TEX(node_5636, _MainNormal)));
                float3 node_8428 = lerp(_Detail1_RM_var.rgb,_MainNormal_var.rgb,node_7746);
                float gloss = 1.0 - node_8428.r; // Convert roughness to gloss
                float perceptualRoughness = node_8428.r;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = node_8428.g;
                float specularMonochrome;
                float4 _Detail1_color_var = tex2D(_Detail1_color,TRANSFORM_TEX(node_6782, _Detail1_color));
                float4 node_3309 = tex2D(_MainTex,TRANSFORM_TEX(node_5636, _MainTex));
                float3 diffuseColor = (_MAinColor.rgb*lerp(_Detail1_color_var.rgb,node_3309.rgb,node_7746)*_COlorforce); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                float node_7248 = saturate((_Windowsopacity+(1.0 - node_3309.r.r)));
                float node_6709 = lerp(saturate((_Windowsopacity+(1.0 - _Detail1_color_var.r.r))),node_7248,i.vertexColor.r);
                return fixed4(finalColor * node_6709,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
