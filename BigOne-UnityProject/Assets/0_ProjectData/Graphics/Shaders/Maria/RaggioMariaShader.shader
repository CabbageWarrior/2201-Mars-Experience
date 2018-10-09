// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shader_raggio_Maria"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 2
		_MARIA_final_DefaultMaterial_AlbedoTransparency("MARIA_final_DefaultMaterial_AlbedoTransparency", 2D) = "white" {}
		_MARIA_final_DefaultMaterial_Emission("MARIA_final_DefaultMaterial_Emission", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaToMask On
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _MARIA_final_DefaultMaterial_AlbedoTransparency;
		uniform float4 _MARIA_final_DefaultMaterial_AlbedoTransparency_ST;
		uniform sampler2D _MARIA_final_DefaultMaterial_Emission;
		uniform float4 _MARIA_final_DefaultMaterial_Emission_ST;
		uniform float _Cutoff = 2;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_MARIA_final_DefaultMaterial_AlbedoTransparency = i.uv_texcoord * _MARIA_final_DefaultMaterial_AlbedoTransparency_ST.xy + _MARIA_final_DefaultMaterial_AlbedoTransparency_ST.zw;
			float4 tex2DNode1 = tex2D( _MARIA_final_DefaultMaterial_AlbedoTransparency, uv_MARIA_final_DefaultMaterial_AlbedoTransparency );
			o.Albedo = tex2DNode1.rgb;
			float2 uv_MARIA_final_DefaultMaterial_Emission = i.uv_texcoord * _MARIA_final_DefaultMaterial_Emission_ST.xy + _MARIA_final_DefaultMaterial_Emission_ST.zw;
			o.Emission = tex2D( _MARIA_final_DefaultMaterial_Emission, uv_MARIA_final_DefaultMaterial_Emission ).rgb;
			o.Alpha = 1;
			#if UNITY_PASS_SHADOWCASTER
			clip( tex2DNode1.a - _Cutoff );
			#endif
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			AlphaToMask Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD6;
				float4 texcoords01 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.texcoords01 = float4( v.texcoord.xy, v.texcoord1.xy );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord.xy = IN.texcoords01.xy;
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13801
1927;29;1906;1044;1336.119;293.5032;1;True;True
Node;AmplifyShaderEditor.SamplerNode;1;-670.4048,-10.41952;Float;True;Property;_MARIA_final_DefaultMaterial_AlbedoTransparency;MARIA_final_DefaultMaterial_AlbedoTransparency;1;0;Assets/0_ProjectData/Graphics/Textures/MariaTextures/Raggio/MARIA_final_DefaultMaterial_AlbedoTransparency.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-738.0053,336.6805;Float;True;Property;_MARIA_final_DefaultMaterial_Emission;MARIA_final_DefaultMaterial_Emission;2;0;Assets/0_ProjectData/Graphics/Textures/MariaTextures/Raggio/MARIA_final_DefaultMaterial_Emission.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Shader_raggio_Maria;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Off;0;0;False;0;0;Custom;2;True;True;0;False;Transparent;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;True;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;0;0;1;0
WireConnection;0;2;2;0
WireConnection;0;10;1;4
ASEEND*/
//CHKSM=BB7F0B2C860BE9744A5C367F2BDB4E5A24E79002