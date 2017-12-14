// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/CharacterShader"
{
	Properties
	{
		_Normal("Normal", 2D) = "white" {}
		_GMA("GMA", 2D) = "white" {}
		_Diff("Diff", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		CGPROGRAM
		#pragma target 3.5
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:forward 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Diff;
		uniform float4 _Diff_ST;
		uniform sampler2D _GMA;
		uniform float4 _GMA_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_Diff = i.uv_texcoord * _Diff_ST.xy + _Diff_ST.zw;
			o.Albedo = tex2D( _Diff, uv_Diff ).rgb;
			float2 uv_GMA = i.uv_texcoord * _GMA_ST.xy + _GMA_ST.zw;
			float4 tex2DNode2 = tex2D( _GMA, uv_GMA );
			o.Metallic = tex2DNode2.g;
			o.Smoothness = tex2DNode2.r;
			o.Occlusion = tex2DNode2.a;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13901
77;120;1242;737;1225.442;647.7726;1.792549;True;False
Node;AmplifyShaderEditor.SamplerNode;1;-365,20.5;Float;True;Property;_Diff;Diff;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-390.1135,218.0288;Float;True;Property;_GMA;GMA;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;3;-370.3954,422.3793;Float;True;Property;_Normal;Normal;0;0;None;True;0;True;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;214,4;Float;False;True;3;Float;ASEMaterialInspector;0;0;Standard;Custom/CharacterShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;DeferredOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;0;0;1;0
WireConnection;0;1;3;0
WireConnection;0;3;2;2
WireConnection;0;4;2;1
WireConnection;0;5;2;4
ASEEND*/
//CHKSM=56BEE68F622A5B3EA4ED38308E0304748D101C52