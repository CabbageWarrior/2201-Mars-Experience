// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MARIAShaders/ColorChanger_Shader"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Color("Color", Range( 0 , 1)) = 0
		_TextureSample0("TextureSample0", 2D) = "white" {}
		_TextureSample1("TextureSample1", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Color;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			o.Albedo = tex2D( _TextureSample0, uv_TextureSample0 ).rgb;
			float4 lerpResult4 = lerp( float4(0,0.241,0,0) , float4(0,0,0.2392157,0) , _Color);
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float4 tex2DNode16 = tex2D( _TextureSample1, uv_TextureSample1 );
			float4 blendOpSrc20 = lerpResult4;
			float4 blendOpDest20 = tex2DNode16;
			o.Emission = ( saturate( (( blendOpSrc20 > 0.5 ) ? ( blendOpDest20 / ( ( 1.0 - blendOpSrc20 ) * 2.0 ) ) : ( 1.0 - ( ( ( 1.0 - blendOpDest20 ) * 0.5 ) / blendOpSrc20 ) ) ) )).rgb;
			o.Alpha = 1;
			clip( tex2DNode16.r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13801
1927;29;1906;1004;696.7687;-64.31753;1.057284;True;True
Node;AmplifyShaderEditor.RangedFloatNode;7;-114.519,454.693;Float;False;Property;_Color;Color;0;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;3;-350.9969,352.608;Float;False;Constant;_Color1;Color 1;0;0;0,0,0.2392157,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;2;-350.326,184.2301;Float;False;Constant;_Color0;Color 0;1;0;0,0.241,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;4;228.9464,334.2218;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;16;7.685109,549.3234;Float;True;Property;_TextureSample1;TextureSample1;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.BlendOpsNode;20;417.1027,428.87;Float;True;VividLight;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;11;287.0124,77.65789;Float;True;Property;_TextureSample0;TextureSample0;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;740.3644,251.5293;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;MARIAShaders/ColorChanger_Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;False;TransparentCutout;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;2;0
WireConnection;4;1;3;0
WireConnection;4;2;7;0
WireConnection;20;0;4;0
WireConnection;20;1;16;0
WireConnection;0;0;11;0
WireConnection;0;2;20;0
WireConnection;0;10;16;0
ASEEND*/
//CHKSM=02A838D2751D91BD0324356DB9A54FE87B4553A5