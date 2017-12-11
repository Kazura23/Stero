// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:2,cusa:False,bamd:0,cgin:,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:False,qofs:0,qpre:1,rntp:0,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:True,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:3554,x:32480,y:32959,varname:node_3554,prsc:2|emission-7568-OUT;n:type:ShaderForge.SFN_Color,id:8306,x:31116,y:32564,ptovrint:False,ptlb:Sky Color,ptin:_SkyColor,varname:node_8306,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.9852941,c2:0,c3:0.5911764,c4:1;n:type:ShaderForge.SFN_ViewVector,id:2265,x:31161,y:32872,varname:node_2265,prsc:2;n:type:ShaderForge.SFN_Dot,id:7606,x:31419,y:32953,varname:node_7606,prsc:2,dt:1|A-2265-OUT,B-3211-OUT;n:type:ShaderForge.SFN_Vector3,id:3211,x:31161,y:32997,varname:node_3211,prsc:2,v1:0,v2:-1,v3:0;n:type:ShaderForge.SFN_Color,id:3839,x:31772,y:32848,ptovrint:False,ptlb:Horizon Color,ptin:_HorizonColor,varname:_GroundColor_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.06617647,c2:0.5468207,c3:1,c4:1;n:type:ShaderForge.SFN_Power,id:4050,x:31772,y:32995,varname:node_4050,prsc:2|VAL-6125-OUT,EXP-7609-OUT;n:type:ShaderForge.SFN_Vector1,id:7609,x:31587,y:33095,varname:node_7609,prsc:2,v1:8;n:type:ShaderForge.SFN_OneMinus,id:6125,x:31587,y:32953,varname:node_6125,prsc:2|IN-7606-OUT;n:type:ShaderForge.SFN_Lerp,id:2737,x:31999,y:32869,cmnt:Sky,varname:node_2737,prsc:2|A-8695-OUT,B-3839-RGB,T-4050-OUT;n:type:ShaderForge.SFN_LightVector,id:3559,x:30723,y:33040,cmnt:Auto-adapts to your directional light,varname:node_3559,prsc:2;n:type:ShaderForge.SFN_Dot,id:1472,x:31082,y:33150,cmnt:Linear falloff to sun angle,varname:node_1472,prsc:2,dt:1|A-8269-OUT,B-8750-OUT;n:type:ShaderForge.SFN_ViewVector,id:8750,x:30895,y:33160,varname:node_8750,prsc:2;n:type:ShaderForge.SFN_Add,id:7568,x:32262,y:33059,cmnt:Sky plus Sun,varname:node_7568,prsc:2|A-2737-OUT,B-5855-OUT;n:type:ShaderForge.SFN_Negate,id:8269,x:30895,y:33040,varname:node_8269,prsc:2|IN-3559-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:3001,x:31383,y:33282,cmnt:Modify radius of falloff,varname:node_3001,prsc:2|IN-1472-OUT,IMIN-1476-OUT,IMAX-1574-OUT,OMIN-9430-OUT,OMAX-6262-OUT;n:type:ShaderForge.SFN_Slider,id:2435,x:30320,y:33466,ptovrint:False,ptlb:Sun Radius B,ptin:_SunRadiusB,varname:node_2435,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1,max:0.1;n:type:ShaderForge.SFN_Slider,id:3144,x:30320,y:33360,ptovrint:False,ptlb:Sun Radius A,ptin:_SunRadiusA,varname:_SunOuterRadius_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:0.1;n:type:ShaderForge.SFN_Vector1,id:9430,x:31082,y:33610,varname:node_9430,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:6262,x:31082,y:33668,varname:node_6262,prsc:2,v1:0;n:type:ShaderForge.SFN_Clamp01,id:7022,x:31556,y:33282,varname:node_7022,prsc:2|IN-3001-OUT;n:type:ShaderForge.SFN_OneMinus,id:1574,x:31082,y:33464,varname:node_1574,prsc:2|IN-8889-OUT;n:type:ShaderForge.SFN_OneMinus,id:1476,x:31082,y:33315,varname:node_1476,prsc:2|IN-3432-OUT;n:type:ShaderForge.SFN_Multiply,id:8889,x:30893,y:33464,varname:node_8889,prsc:2|A-9367-OUT,B-9367-OUT;n:type:ShaderForge.SFN_Multiply,id:3432,x:30893,y:33315,varname:node_3432,prsc:2|A-7933-OUT,B-7933-OUT;n:type:ShaderForge.SFN_Max,id:9367,x:30681,y:33464,varname:node_9367,prsc:2|A-3144-OUT,B-2435-OUT;n:type:ShaderForge.SFN_Min,id:7933,x:30681,y:33315,varname:node_7933,prsc:2|A-3144-OUT,B-2435-OUT;n:type:ShaderForge.SFN_Power,id:754,x:31772,y:33336,varname:node_754,prsc:2|VAL-7022-OUT,EXP-5929-OUT;n:type:ShaderForge.SFN_Vector1,id:5929,x:31556,y:33412,varname:node_5929,prsc:2,v1:5;n:type:ShaderForge.SFN_Multiply,id:5855,x:31957,y:33257,cmnt:Sun,varname:node_5855,prsc:2|A-2359-RGB,B-754-OUT,C-7055-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7055,x:31772,y:33484,ptovrint:False,ptlb:Sun Intensity,ptin:_SunIntensity,varname:node_7055,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_LightColor,id:2359,x:31772,y:33210,cmnt:Get color from directional light,varname:node_2359,prsc:2;n:type:ShaderForge.SFN_Lerp,id:8695,x:32217,y:32476,varname:node_8695,prsc:2|A-572-RGB,B-8306-RGB,T-6976-OUT;n:type:ShaderForge.SFN_Color,id:572,x:30916,y:32122,ptovrint:False,ptlb:node_572,ptin:_node_572,varname:node_572,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.8235294,c3:0.2101419,c4:1;n:type:ShaderForge.SFN_Tex2d,id:1485,x:31147,y:32297,ptovrint:False,ptlb:node_1485,ptin:_node_1485,varname:node_1485,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-716-UVOUT;n:type:ShaderForge.SFN_Time,id:2520,x:31102,y:32032,varname:node_2520,prsc:2;n:type:ShaderForge.SFN_Cos,id:453,x:31513,y:32070,varname:node_453,prsc:2|IN-3716-OUT;n:type:ShaderForge.SFN_Multiply,id:7802,x:31354,y:32297,varname:node_7802,prsc:2|A-1485-R,B-6483-OUT;n:type:ShaderForge.SFN_TexCoord,id:9558,x:30616,y:32270,varname:node_9558,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Rotator,id:716,x:30916,y:32275,varname:node_716,prsc:2|UVIN-9558-UVOUT,SPD-9294-OUT;n:type:ShaderForge.SFN_Multiply,id:9294,x:30823,y:32507,varname:node_9294,prsc:2|A-453-OUT,B-359-OUT;n:type:ShaderForge.SFN_Vector1,id:359,x:30572,y:32572,varname:node_359,prsc:2,v1:0.001;n:type:ShaderForge.SFN_Tex2d,id:2965,x:31087,y:32738,ptovrint:False,ptlb:node_2965,ptin:_node_2965,varname:node_2965,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-716-UVOUT;n:type:ShaderForge.SFN_Time,id:9730,x:30862,y:31924,varname:node_9730,prsc:2;n:type:ShaderForge.SFN_Sin,id:5369,x:31513,y:31927,varname:node_5369,prsc:2|IN-4884-OUT;n:type:ShaderForge.SFN_Multiply,id:3028,x:31537,y:32415,varname:node_3028,prsc:2|A-2965-R,B-7690-OUT;n:type:ShaderForge.SFN_Multiply,id:3844,x:31707,y:32359,varname:node_3844,prsc:2|A-7802-OUT,B-3028-OUT;n:type:ShaderForge.SFN_Add,id:9632,x:31850,y:32006,varname:node_9632,prsc:2|A-5369-OUT,B-5327-OUT;n:type:ShaderForge.SFN_Add,id:1601,x:31850,y:32139,varname:node_1601,prsc:2|A-453-OUT,B-5327-OUT;n:type:ShaderForge.SFN_Vector1,id:5327,x:31650,y:32091,varname:node_5327,prsc:2,v1:1;n:type:ShaderForge.SFN_Divide,id:7690,x:32056,y:32026,varname:node_7690,prsc:2|A-9632-OUT,B-4683-OUT;n:type:ShaderForge.SFN_Divide,id:6483,x:32045,y:32155,varname:node_6483,prsc:2|A-1601-OUT,B-4683-OUT;n:type:ShaderForge.SFN_Vector1,id:4683,x:31811,y:32262,varname:node_4683,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:4884,x:31334,y:31883,varname:node_4884,prsc:2|A-3318-OUT,B-8274-OUT;n:type:ShaderForge.SFN_Multiply,id:3716,x:31334,y:32058,varname:node_3716,prsc:2|A-2520-T,B-8274-OUT;n:type:ShaderForge.SFN_Slider,id:8274,x:30831,y:31866,ptovrint:False,ptlb:timenois,ptin:_timenois,varname:node_8274,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Add,id:3318,x:31064,y:31924,varname:node_3318,prsc:2|A-9730-T,B-5327-OUT;n:type:ShaderForge.SFN_Power,id:6976,x:31972,y:32392,varname:node_6976,prsc:2|VAL-3844-OUT,EXP-4909-OUT;n:type:ShaderForge.SFN_Slider,id:4909,x:31707,y:32621,ptovrint:False,ptlb:noisepower,ptin:_noisepower,varname:node_4909,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;proporder:8306-3839-2435-3144-7055-572-1485-2965-8274-4909;pass:END;sub:END;*/

Shader "Shader Forge/TestSky1_RainbowCodeColorfull1212" {
    Properties {
        _SkyColor ("Sky Color", Color) = (0.9852941,0,0.5911764,1)
        _HorizonColor ("Horizon Color", Color) = (0.06617647,0.5468207,1,1)
        _SunRadiusB ("Sun Radius B", Range(0, 0.1)) = 0.1
        _SunRadiusA ("Sun Radius A", Range(0, 0.1)) = 0
        _SunIntensity ("Sun Intensity", Float ) = 2
        _node_572 ("node_572", Color) = (0,0.8235294,0.2101419,1)
        _node_1485 ("node_1485", 2D) = "white" {}
        _node_2965 ("node_2965", 2D) = "white" {}
        _timenois ("timenois", Range(0, 1)) = 0
        _noisepower ("noisepower", Range(0, 1)) = 0
    }
    SubShader {
        Tags {
            "PreviewType"="Skybox"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            Stencil {
                Ref 128
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d11 glcore gles gles3 xboxone 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform float4 _SkyColor;
            uniform float4 _HorizonColor;
            uniform float _SunRadiusB;
            uniform float _SunRadiusA;
            uniform float _SunIntensity;
            uniform float4 _node_572;
            uniform sampler2D _node_1485; uniform float4 _node_1485_ST;
            uniform sampler2D _node_2965; uniform float4 _node_2965_ST;
            uniform float _timenois;
            uniform float _noisepower;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                LIGHTING_COORDS(2,3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
////// Emissive:
                float4 node_4469 = _Time + _TimeEditor;
                float4 node_2520 = _Time + _TimeEditor;
                float node_453 = cos((node_2520.g*_timenois));
                float node_716_ang = node_4469.g;
                float node_716_spd = (node_453*0.001);
                float node_716_cos = cos(node_716_spd*node_716_ang);
                float node_716_sin = sin(node_716_spd*node_716_ang);
                float2 node_716_piv = float2(0.5,0.5);
                float2 node_716 = (mul(i.uv0-node_716_piv,float2x2( node_716_cos, -node_716_sin, node_716_sin, node_716_cos))+node_716_piv);
                float4 _node_1485_var = tex2D(_node_1485,TRANSFORM_TEX(node_716, _node_1485));
                float node_5327 = 1.0;
                float node_4683 = 2.0;
                float4 _node_2965_var = tex2D(_node_2965,TRANSFORM_TEX(node_716, _node_2965));
                float4 node_9730 = _Time + _TimeEditor;
                float node_7933 = min(_SunRadiusA,_SunRadiusB);
                float node_1476 = (1.0 - (node_7933*node_7933));
                float node_9367 = max(_SunRadiusA,_SunRadiusB);
                float node_9430 = 1.0;
                float3 emissive = (lerp(lerp(_node_572.rgb,_SkyColor.rgb,pow(((_node_1485_var.r*((node_453+node_5327)/node_4683))*(_node_2965_var.r*((sin(((node_9730.g+node_5327)*_timenois))+node_5327)/node_4683))),_noisepower)),_HorizonColor.rgb,pow((1.0 - max(0,dot(viewDirection,float3(0,-1,0)))),8.0))+(_LightColor0.rgb*pow(saturate((node_9430 + ( (max(0,dot((-1*lightDirection),viewDirection)) - node_1476) * (0.0 - node_9430) ) / ((1.0 - (node_9367*node_9367)) - node_1476))),5.0)*_SunIntensity));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
