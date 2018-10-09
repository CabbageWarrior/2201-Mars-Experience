// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Glass_shader"
{
	Properties
	{
		[Header(Refraction)]
		_ChromaticAberration("Chromatic Aberration", Range( 0 , 0.3)) = 0.1
		_Refraction("Refraction", Range( 0 , 1)) = 0
		_blending("blending", Range( 0 , 1)) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#pragma target 3.0
		#pragma multi_compile _ALPHAPREMULTIPLY_ON
		#pragma surface surf Standard alpha:fade keepalpha finalcolor:RefractionF noshadow exclude_path:deferred 
		struct Input
		{
			float4 screenPos;
			float3 worldPos;
		};

		uniform sampler2D _GrabTexture;
		uniform float _blending;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float _ChromaticAberration;
		uniform float _Refraction;

		inline float4 Refraction( Input i, SurfaceOutputStandard o, float indexOfRefraction, float chomaticAberration ) {
			float3 worldNormal = o.Normal;
			float4 screenPos = i.screenPos;
			#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
			#else
				float scale = 1.0;
			#endif
			float halfPosW = screenPos.w * 0.5;
			screenPos.y = ( screenPos.y - halfPosW ) * _ProjectionParams.x * scale + halfPosW;
			#if SHADER_API_D3D9 || SHADER_API_D3D11
				screenPos.w += 0.00000000001;
			#endif
			float2 projScreenPos = ( screenPos / screenPos.w ).xy;
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 refractionOffset = ( ( ( ( indexOfRefraction - 1.0 ) * mul( UNITY_MATRIX_V, float4( worldNormal, 0.0 ) ) ) * ( 1.0 / ( screenPos.z + 1.0 ) ) ) * ( 1.0 - dot( worldNormal, worldViewDir ) ) );
			float2 cameraRefraction = float2( refractionOffset.x, -( refractionOffset.y * _ProjectionParams.x ) );
			float4 redAlpha = tex2D( _GrabTexture, ( projScreenPos + cameraRefraction ) );
			float green = tex2D( _GrabTexture, ( projScreenPos + ( cameraRefraction * ( 1.0 - chomaticAberration ) ) ) ).g;
			float blue = tex2D( _GrabTexture, ( projScreenPos + ( cameraRefraction * ( 1.0 + chomaticAberration ) ) ) ).b;
			return float4( redAlpha.r, green, blue, redAlpha.a );
		}

		void RefractionF( Input i, SurfaceOutputStandard o, inout fixed4 color )
		{
			#ifdef UNITY_PASS_FORWARDBASE
			color.rgb = color.rgb + Refraction( i, o, _Refraction, _ChromaticAberration ) * ( 1 - color.a );
			color.a = 1;
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			o.Albedo = float3(0.1,0.1,0.1);
			float4 screenPos8 = i.screenPos;
			#if UNITY_UV_STARTS_AT_TOP
			float scale8 = -1.0;
			#else
			float scale8 = 1.0;
			#endif
			float halfPosW8 = screenPos8.w * 0.5;
			screenPos8.y = ( screenPos8.y - halfPosW8 ) * _ProjectionParams.x* scale8 + halfPosW8;
			screenPos8.w += 0.00000000001;
			screenPos8.xyzw /= screenPos8.w;
			float4 screenColor8 = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD( screenPos8 ) );
			float4 temp_cast_0 = (-0.04).xxxx;
			float4 lerpResult9 = lerp( screenColor8 , temp_cast_0 , _blending);
			o.Emission = lerpResult9.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 0.6758156;
			o.Normal = o.Normal + 0.00001 * i.screenPos * i.worldPos;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13801
1927;90;1266;988;1262.548;369.5341;1.3;True;False
Node;AmplifyShaderEditor.RangedFloatNode;10;-822.9717,84.67657;Float;False;Constant;_white;white;0;0;-0.04;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;11;-844.1224,215.5464;Float;False;Property;_blending;blending;0;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ScreenColorNode;8;-841.5194,-141.2487;Float;False;Global;_GrabScreen0;Grab Screen 0;2;0;Object;-1;False;1;0;FLOAT2;0,0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;15;-453.4049,457.1312;Float;False;Property;_Translucency;Translucency;0;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;12;-464.7332,302.3612;Float;False;Property;_Smoothness;Smoothness;0;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;16;-438.7309,648.4694;Float;False;Constant;_Opacity;Opacity;7;0;0.6758156;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;14;-455.4802,539.1161;Float;False;Property;_Refraction;Refraction;0;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;9;-569.1644,19.90269;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.Vector3Node;4;-273.9412,-117.8356;Float;False;Constant;_Albedo_color;Albedo_color;0;0;0.1,0.1,0.1;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;13;-459.2201,206.2928;Float;False;Property;_Metallic;Metallic;0;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Glass_shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;False;0;False;Transparent;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;8;0
WireConnection;9;1;10;0
WireConnection;9;2;11;0
WireConnection;0;0;4;0
WireConnection;0;2;9;0
WireConnection;0;3;13;0
WireConnection;0;4;12;0
WireConnection;0;8;14;0
WireConnection;0;9;16;0
ASEEND*/
//CHKSM=36C9B20E7D985EF2031AB7031DDB55578FC7E40E