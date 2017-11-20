// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:1,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:0,x:36267,y:32264,varname:node_0,prsc:2|diff-5958-OUT,spec-4970-OUT,gloss-5741-OUT,normal-3900-RGB,olwid-3754-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:37,x:34652,y:31725,varname:node_37,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:82,x:34310,y:32004,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:_Diffuse,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:8993b617f08498f43adcbd90697f1c5d,ntxv:0,isnm:False|UVIN-272-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:272,x:34105,y:31932,varname:node_272,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:5533,x:34542,y:32115,varname:node_5533,prsc:2|A-82-RGB,B-3403-OUT;n:type:ShaderForge.SFN_Slider,id:6291,x:34404,y:32256,ptovrint:False,ptlb:diffuse force,ptin:_diffuseforce,varname:node_6291,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0.8,cur:0.8,max:2;n:type:ShaderForge.SFN_Lerp,id:8955,x:34891,y:32078,varname:node_8955,prsc:2|A-2186-OUT,B-5533-OUT,T-6291-OUT;n:type:ShaderForge.SFN_Desaturate,id:2186,x:34530,y:31986,varname:node_2186,prsc:2|COL-82-RGB,DES-6363-OUT;n:type:ShaderForge.SFN_Slider,id:6363,x:34261,y:31843,ptovrint:False,ptlb:saturation,ptin:_saturation,varname:node_6363,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-10,cur:1,max:10;n:type:ShaderForge.SFN_Lerp,id:2801,x:35184,y:32079,varname:node_2801,prsc:2|A-8955-OUT,B-8166-RGB,T-8441-OUT;n:type:ShaderForge.SFN_Color,id:8166,x:34939,y:32268,ptovrint:False,ptlb:shadow_color,ptin:_shadow_color,varname:node_8166,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:609,x:34816,y:31778,varname:node_609,prsc:2|A-37-OUT,B-482-OUT;n:type:ShaderForge.SFN_OneMinus,id:3773,x:34977,y:31778,varname:node_3773,prsc:2|IN-609-OUT;n:type:ShaderForge.SFN_Posterize,id:6670,x:35274,y:31803,varname:node_6670,prsc:2|IN-3773-OUT,STPS-672-OUT;n:type:ShaderForge.SFN_Multiply,id:8441,x:35274,y:31946,varname:node_8441,prsc:2|A-6670-OUT,B-3433-OUT;n:type:ShaderForge.SFN_Lerp,id:9770,x:35612,y:32176,varname:node_9770,prsc:2|A-9164-OUT,B-2801-OUT,T-8441-OUT;n:type:ShaderForge.SFN_Vector1,id:3403,x:34202,y:32158,varname:node_3403,prsc:2,v1:1;n:type:ShaderForge.SFN_LightColor,id:7823,x:35142,y:32191,varname:node_7823,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9164,x:35379,y:32191,varname:node_9164,prsc:2|A-2801-OUT,B-7823-RGB;n:type:ShaderForge.SFN_ObjectPosition,id:1785,x:35613,y:32779,varname:node_1785,prsc:2;n:type:ShaderForge.SFN_ViewPosition,id:4088,x:35613,y:32908,varname:node_4088,prsc:2;n:type:ShaderForge.SFN_Distance,id:2020,x:35804,y:32798,varname:node_2020,prsc:2|A-4088-XYZ,B-1785-XYZ;n:type:ShaderForge.SFN_Multiply,id:1042,x:35977,y:32798,varname:node_1042,prsc:2|A-2020-OUT,B-7467-OUT;n:type:ShaderForge.SFN_Add,id:3754,x:36139,y:32720,varname:node_3754,prsc:2|A-2870-OUT,B-1042-OUT;n:type:ShaderForge.SFN_Vector1,id:482,x:34652,y:31858,varname:node_482,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:672,x:35093,y:31837,varname:node_672,prsc:2,v1:1.08;n:type:ShaderForge.SFN_Vector1,id:3433,x:34993,y:31924,varname:node_3433,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Lerp,id:5958,x:35954,y:32288,varname:node_5958,prsc:2|A-9770-OUT,B-6643-OUT,T-1824-OUT;n:type:ShaderForge.SFN_Multiply,id:6643,x:35612,y:32311,varname:node_6643,prsc:2|A-9770-OUT,B-1029-OUT;n:type:ShaderForge.SFN_Slider,id:1029,x:35222,y:32344,ptovrint:False,ptlb:gloss_color,ptin:_gloss_color,varname:node_1029,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:2;n:type:ShaderForge.SFN_LightVector,id:9235,x:34725,y:32555,varname:node_9235,prsc:2;n:type:ShaderForge.SFN_HalfVector,id:1031,x:34732,y:32870,varname:node_1031,prsc:2;n:type:ShaderForge.SFN_Dot,id:4887,x:34976,y:32763,varname:node_4887,prsc:2,dt:1|A-2141-OUT,B-1031-OUT;n:type:ShaderForge.SFN_Power,id:6054,x:35188,y:32763,varname:node_6054,prsc:2|VAL-4887-OUT,EXP-804-OUT;n:type:ShaderForge.SFN_Exp,id:804,x:34976,y:32935,varname:node_804,prsc:2,et:1|IN-6371-OUT;n:type:ShaderForge.SFN_ConstantLerp,id:6371,x:34732,y:33033,varname:node_6371,prsc:2,a:1,b:11|IN-9186-OUT;n:type:ShaderForge.SFN_Slider,id:9186,x:34375,y:33040,ptovrint:False,ptlb:gloss,ptin:_gloss,varname:node_9186,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_NormalVector,id:2141,x:34686,y:32726,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:8967,x:34976,y:32606,varname:node_8967,prsc:2,dt:1|A-9235-OUT,B-2141-OUT;n:type:ShaderForge.SFN_Multiply,id:1824,x:35331,y:32683,varname:node_1824,prsc:2|A-8967-OUT,B-6054-OUT;n:type:ShaderForge.SFN_Tex2d,id:3900,x:35923,y:32124,ptovrint:False,ptlb:NormalMap,ptin:_NormalMap,varname:node_3900,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:bbab0a6f7bae9cf42bf057d8ee2755f6,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Slider,id:4970,x:35788,y:32459,ptovrint:False,ptlb:Metalic,ptin:_Metalic,varname:node_4970,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:5741,x:35788,y:32561,ptovrint:False,ptlb:Roughness,ptin:_Roughness,varname:node_5741,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:2870,x:35613,y:32644,ptovrint:False,ptlb:Outline_basevalue,ptin:_Outline_basevalue,varname:node_2870,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.006,max:0.006;n:type:ShaderForge.SFN_Slider,id:7467,x:35804,y:32987,ptovrint:False,ptlb:Outline_ratiodistancescale,ptin:_Outline_ratiodistancescale,varname:node_7467,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.002384947,max:0.006;proporder:82-6291-6363-8166-1029-9186-3900-4970-5741-2870-7467;pass:END;sub:END;*/

Shader "custom/enviro_toonshader" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _diffuseforce ("diffuse force", Range(0.8, 2)) = 0.8
        _saturation ("saturation", Range(-10, 10)) = 1
        _shadow_color ("shadow_color", Color) = (0.5,0.5,0.5,1)
        _gloss_color ("gloss_color", Range(1, 2)) = 1
        _gloss ("gloss", Range(0, 1)) = 0
        _NormalMap ("NormalMap", 2D) = "bump" {}
        _Metalic ("Metalic", Range(0, 1)) = 0
        _Roughness ("Roughness", Range(0, 1)) = 0
        _Outline_basevalue ("Outline_basevalue", Range(0, 0.006)) = 0.006
        _Outline_ratiodistancescale ("Outline_ratiodistancescale", Range(0, 0.006)) = 0.002384947
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma only_renderers d3d11 glcore gles3 d3d11_9x 
            #pragma target 3.0
            uniform float _Outline_basevalue;
            uniform float _Outline_ratiodistancescale;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*(_Outline_basevalue+(distance(_WorldSpaceCameraPos,objPos.rgb)*_Outline_ratiodistancescale)),1) );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                return fixed4(float3(0,0,0),0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma only_renderers d3d11 glcore gles3 d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _diffuseforce;
            uniform float _saturation;
            uniform float4 _shadow_color;
            uniform float _gloss_color;
            uniform float _gloss;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _Metalic;
            uniform float _Roughness;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
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
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = mul( unity_WorldToObject, float4(normalLocal,0)) / recipObjScale;
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
                float gloss = _Roughness;
                float perceptualRoughness = 1.0 - _Roughness;
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
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = _Metalic;
                float specularMonochrome;
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float node_672 = 1.08;
                float node_8441 = (floor((1.0 - (attenuation*1.0)) * node_672) / (node_672 - 1)*0.1);
                float3 node_2801 = lerp(lerp(lerp(_Diffuse_var.rgb,dot(_Diffuse_var.rgb,float3(0.3,0.59,0.11)),_saturation),(_Diffuse_var.rgb*1.0),_diffuseforce),_shadow_color.rgb,node_8441);
                float3 node_9770 = lerp((node_2801*_LightColor0.rgb),node_2801,node_8441);
                float3 diffuseColor = lerp(node_9770,(node_9770*_gloss_color),(max(0,dot(lightDirection,i.normalDir))*pow(max(0,dot(i.normalDir,halfDirection)),exp2(lerp(1,11,_gloss))))); // Need this for specular when using metallic
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
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma only_renderers d3d11 glcore gles3 d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _diffuseforce;
            uniform float _saturation;
            uniform float4 _shadow_color;
            uniform float _gloss_color;
            uniform float _gloss;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _Metalic;
            uniform float _Roughness;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
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
                LIGHTING_COORDS(7,8)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = mul( unity_WorldToObject, float4(normalLocal,0)) / recipObjScale;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Roughness;
                float perceptualRoughness = 1.0 - _Roughness;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = _Metalic;
                float specularMonochrome;
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float node_672 = 1.08;
                float node_8441 = (floor((1.0 - (attenuation*1.0)) * node_672) / (node_672 - 1)*0.1);
                float3 node_2801 = lerp(lerp(lerp(_Diffuse_var.rgb,dot(_Diffuse_var.rgb,float3(0.3,0.59,0.11)),_saturation),(_Diffuse_var.rgb*1.0),_diffuseforce),_shadow_color.rgb,node_8441);
                float3 node_9770 = lerp((node_2801*_LightColor0.rgb),node_2801,node_8441);
                float3 diffuseColor = lerp(node_9770,(node_9770*_gloss_color),(max(0,dot(lightDirection,i.normalDir))*pow(max(0,dot(i.normalDir,halfDirection)),exp2(lerp(1,11,_gloss))))); // Need this for specular when using metallic
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
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma only_renderers d3d11 glcore gles3 d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _diffuseforce;
            uniform float _saturation;
            uniform float4 _shadow_color;
            uniform float _gloss_color;
            uniform float _gloss;
            uniform float _Metalic;
            uniform float _Roughness;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                o.Emission = 0;
                
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float node_672 = 1.08;
                float node_8441 = (floor((1.0 - (attenuation*1.0)) * node_672) / (node_672 - 1)*0.1);
                float3 node_2801 = lerp(lerp(lerp(_Diffuse_var.rgb,dot(_Diffuse_var.rgb,float3(0.3,0.59,0.11)),_saturation),(_Diffuse_var.rgb*1.0),_diffuseforce),_shadow_color.rgb,node_8441);
                float3 node_9770 = lerp((node_2801*_LightColor0.rgb),node_2801,node_8441);
                float3 diffColor = lerp(node_9770,(node_9770*_gloss_color),(max(0,dot(lightDirection,i.normalDir))*pow(max(0,dot(i.normalDir,halfDirection)),exp2(lerp(1,11,_gloss)))));
                float specularMonochrome;
                float3 specColor;
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, _Metalic, specColor, specularMonochrome );
                float roughness = 1.0 - _Roughness;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
