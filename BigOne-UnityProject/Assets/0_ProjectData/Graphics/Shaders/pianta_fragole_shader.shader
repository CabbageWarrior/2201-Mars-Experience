// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shader Plants"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_prova_texturing_fragola_02Default_AlbedoTransparency("prova_texturing_fragola_02 - Default_AlbedoTransparency", 2D) = "white" {}
		_prova_texturing_fragola_02Default_MetallicSmoothness("prova_texturing_fragola_02 - Default_MetallicSmoothness", 2D) = "white" {}
		_prova_texturing_fragola_02Default_Normal("prova_texturing_fragola_02 - Default_Normal", 2D) = "bump" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _prova_texturing_fragola_02Default_Normal;
		uniform float4 _prova_texturing_fragola_02Default_Normal_ST;
		uniform sampler2D _prova_texturing_fragola_02Default_AlbedoTransparency;
		uniform float4 _prova_texturing_fragola_02Default_AlbedoTransparency_ST;
		uniform sampler2D _prova_texturing_fragola_02Default_MetallicSmoothness;
		uniform float4 _prova_texturing_fragola_02Default_MetallicSmoothness_ST;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_prova_texturing_fragola_02Default_Normal = i.uv_texcoord * _prova_texturing_fragola_02Default_Normal_ST.xy + _prova_texturing_fragola_02Default_Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _prova_texturing_fragola_02Default_Normal, uv_prova_texturing_fragola_02Default_Normal ) );
			float2 uv_prova_texturing_fragola_02Default_AlbedoTransparency = i.uv_texcoord * _prova_texturing_fragola_02Default_AlbedoTransparency_ST.xy + _prova_texturing_fragola_02Default_AlbedoTransparency_ST.zw;
			float4 tex2DNode1 = tex2D( _prova_texturing_fragola_02Default_AlbedoTransparency, uv_prova_texturing_fragola_02Default_AlbedoTransparency );
			o.Albedo = tex2DNode1.rgb;
			float2 uv_prova_texturing_fragola_02Default_MetallicSmoothness = i.uv_texcoord * _prova_texturing_fragola_02Default_MetallicSmoothness_ST.xy + _prova_texturing_fragola_02Default_MetallicSmoothness_ST.zw;
			float4 tex2DNode2 = tex2D( _prova_texturing_fragola_02Default_MetallicSmoothness, uv_prova_texturing_fragola_02Default_MetallicSmoothness );
			o.Metallic = tex2DNode2.r;
			o.Smoothness = tex2DNode2.a;
			o.Alpha = 1;
			clip( tex2DNode1.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13801
381;92;1148;552;1198.967;254.9418;1.780743;True;True
Node;AmplifyShaderEditor.SamplerNode;1;-715.001,-216.8296;Float;True;Property;_prova_texturing_fragola_02Default_AlbedoTransparency;prova_texturing_fragola_02 - Default_AlbedoTransparency;0;0;Assets/0_ProjectData/Graphics/Models/FBXs/prova_texturing_fragola_02 - Default_AlbedoTransparency.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-778.3149,294.6802;Float;True;Property;_prova_texturing_fragola_02Default_MetallicSmoothness;prova_texturing_fragola_02 - Default_MetallicSmoothness;1;0;Assets/0_ProjectData/Graphics/Models/FBXs/prova_texturing_fragola_02 - Default_MetallicSmoothness.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;3;-706.671,49.75492;Float;True;Property;_prova_texturing_fragola_02Default_Normal;prova_texturing_fragola_02 - Default_Normal;2;0;Assets/0_ProjectData/Graphics/Models/FBXs/prova_texturing_fragola_02 - Default_Normal.png;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Shader Plants;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;0;False;0;0;Masked;0.5;True;True;0;False;TransparentCutout;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;0;0;1;0
WireConnection;0;1;3;0
WireConnection;0;3;2;0
WireConnection;0;4;2;4
WireConnection;0;10;1;4
ASEEND*/
//CHKSM=2B89F440B0B9ABE1627842ED7D5A30B45D3BD50C