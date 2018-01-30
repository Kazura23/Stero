// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/CustomShader_SingleLayer2Mega"
{
	Properties
	{
		_Diffuse("Diffuse", 2DArray) = "black"
		_MSEO("MSEO", 2DArray) = "black"
		[Header(MegaSplat)]
		_Normal("Normal", 2DArray) = "black"
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.5
		#pragma exclude_renderers d3d9 metal xbox360 psp2 n3ds wiiu 
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float4 vertexColor : COLOR;
			float3 mb_layer3;
			float2 uv_texcoord;
		};

		UNITY_DECLARE_TEX2DARRAY(_Normal);
		UNITY_DECLARE_TEX2DARRAY(_Diffuse);
		UNITY_DECLARE_TEX2DARRAY(_MSEO);


		half3 ComputeMegaSplatWeights(half3 iWeights, half4 tex0, half4 tex1, half4 tex2, half contrast)
		{
			const half epsilon = 1.0f / 1024.0f;
			half3 weights = half3(iWeights.x * (tex0.a + epsilon), iWeights.y * (tex1.a + epsilon),iWeights.z * (tex2.a + epsilon));
			half maxWeight = max(weights.x, max(weights.y, weights.z));
			half transition = contrast * maxWeight;
			half threshold = maxWeight - transition;
			half scale = 1.0f / transition;
			weights = saturate((weights - threshold) * scale);
			half weightScale = 1.0f / (weights.x + weights.y + weights.z);
			weights *= weightScale;
			return weights;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			half choice3 = v.color.a;
			o.mb_layer3 = v.color.xyz * choice3 * 255;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			int im0_3 = round(i.mb_layer3.x / max(i.vertexColor.x, 0.00001));
			int im1_3 = round(i.mb_layer3.y / max(i.vertexColor.y, 0.00001));
			int im2_3 = round(i.mb_layer3.z / max(i.vertexColor.z, 0.00001));
			float2 localUV3 = i.uv_texcoord;
			float3 uv0_3 = float3(localUV3, im0_3);
			float3 uv1_3 = float3(localUV3, im1_3);
			float3 uv2_3 = float3(localUV3, im2_3);
			float contrast_3 = 0.5;
			half4 tex0_3 = UNITY_SAMPLE_TEX2DARRAY(_Diffuse, uv0_3);
			half4 tex1_3 = UNITY_SAMPLE_TEX2DARRAY(_Diffuse, uv1_3);
			half4 tex2_3 = UNITY_SAMPLE_TEX2DARRAY(_Diffuse, uv2_3);
			half3 weights_3 = ComputeMegaSplatWeights( i.mb_layer3,tex0_3,tex1_3,tex2_3,contrast_3);
			half4 resTex_3 = tex0_3 * weights_3.x + tex1_3 * weights_3.y + tex2_3 * weights_3.z;
			float4x4 megadata_3 = float4x4(uv0_3, contrast_3, uv1_3, 1, uv2_3, 1, weights_3, 1);
			float4x4 portData4 = megadata_3;
			float3 uv0_4 = portData4[0].xyz;
			float3 uv1_4 = portData4[1].xyz;
			float3 uv2_4 = portData4[2].xyz;
			float3 weights_4 = portData4[3].xyz;
			float contrast_4 = portData4[0].w;
			half4 tex0_4 = UNITY_SAMPLE_TEX2DARRAY(_Normal, uv0_4);
			half4 tex1_4 = UNITY_SAMPLE_TEX2DARRAY(_Normal, uv1_4);
			half4 tex2_4 = UNITY_SAMPLE_TEX2DARRAY(_Normal, uv2_4);
			half4 resTex_4 = tex0_4 * weights_4.x + tex1_4 * weights_4.y + tex2_4 * weights_4.z;
			float4x4 megadata_4 = float4x4(uv0_4, contrast_4, uv1_4, 1, uv2_4, 1, weights_4, 1);
			o.Normal = UnpackNormal( resTex_4 );
			o.Albedo = resTex_3.rgb;
			float4x4 portData7 = megadata_4;
			float3 uv0_7 = portData7[0].xyz;
			float3 uv1_7 = portData7[1].xyz;
			float3 uv2_7 = portData7[2].xyz;
			float3 weights_7 = portData7[3].xyz;
			float contrast_7 = portData7[0].w;
			half4 tex0_7 = UNITY_SAMPLE_TEX2DARRAY(_MSEO, uv0_7);
			half4 tex1_7 = UNITY_SAMPLE_TEX2DARRAY(_MSEO, uv1_7);
			half4 tex2_7 = UNITY_SAMPLE_TEX2DARRAY(_MSEO, uv2_7);
			half4 resTex_7 = tex0_7 * weights_7.x + tex1_7 * weights_7.y + tex2_7 * weights_7.z;
			float4x4 megadata_7 = float4x4(uv0_7, contrast_7, uv1_7, 1, uv2_7, 1, weights_7, 1);
			o.Metallic = resTex_7.r;
			o.Smoothness = resTex_7.g;
			o.Occlusion = resTex_7.a;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13901
113;13;1242;737;1323.439;396.8911;1.587648;True;False
Node;AmplifyShaderEditor.FunctionNode;14;-615.0142,12.5834;Float;False;MegaSplat;0;;6;dff7efdc6f795ae4aa15c1407b195c7f;0;3;COLOR;FLOAT3;COLOR
Node;AmplifyShaderEditor.BreakToComponentsNode;15;-373.9258,185.021;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;59.6894,-17.87648;Float;False;True;3;Float;ASEMaterialInspector;0;0;Standard;Custom/CustomShader_SingleLayer2Mega;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;False;True;True;True;True;False;True;False;True;True;False;False;False;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;14;0
WireConnection;0;0;14;2
WireConnection;0;1;14;1
WireConnection;0;3;15;0
WireConnection;0;4;15;1
WireConnection;0;5;15;3
ASEEND*/
//CHKSM=FD6F9D47E9F7730A09AA511A90A6CCF0A25A506D