// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Glass_shader_dirty"
{
	Properties
	{
		[Header(Refraction)]
		_Smoothness("Smoothness", Range( 0 , 1)) = 0.7354432
		_Refraction("Refraction", Range( 0 , 1)) = 1
		_ChromaticAberration("Chromatic Aberration", Range( 0 , 0.3)) = 0.1
		_blending("blending", Range( 0 , 1)) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0.6687419
		_images1("images (1)", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
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
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 screenPos;
			float3 worldPos;
		};

		uniform sampler2D _images1;
		uniform float4 _images1_ST;
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
			float2 uv_images1 = i.uv_texcoord * _images1_ST.xy + _images1_ST.zw;
			float4 temp_output_18_0 = (tex2D( _images1, uv_images1 )*0.5 + 0.0);
			float4 lerpResult26 = lerp( temp_output_18_0 , i.vertexColor , float4( 0,0,0,0 ));
			float4 lerpResult17 = lerp( float4( float3(0.1,0.1,0.1) , 0.0 ) , lerpResult26 , i.vertexColor.r);
			o.Albedo = lerpResult17.rgb;
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
			float4 temp_cast_2 = (-0.04).xxxx;
			float4 lerpResult9 = lerp( screenColor8 , temp_cast_2 , _blending);
			o.Emission = lerpResult9.rgb;
			float temp_output_13_0 = _Metallic;
			o.Metallic = temp_output_13_0;
			float temp_output_12_0 = _Smoothness;
			o.Smoothness = temp_output_12_0;
			o.Alpha = 0.6758156;
			o.Normal = o.Normal + 0.00001 * i.screenPos * i.worldPos;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13801
1927;29;1906;1044;2302.147;403.8351;1.416198;True;True
Node;AmplifyShaderEditor.RangedFloatNode;25;-1949.946,-485.0839;Float;False;Constant;_Float0;Float 0;7;0;0.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;20;-2428.41,-141.7182;Float;True;Property;_images1;images (1);6;0;Assets/0_ProjectData/Graphics/Shaders/images (1).jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ScaleAndOffsetNode;18;-1646.016,-405.7751;Float;False;3;0;COLOR;0.0;False;1;FLOAT;1.0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.VertexColorNode;24;-1825.579,81.55946;Float;False;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;26;-1172.215,-283.3408;Float;False;3;0;COLOR;0.0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.ScreenColorNode;8;-841.5194,-141.2487;Float;False;Global;_GrabScreen0;Grab Screen 0;2;0;Object;-1;False;1;0;FLOAT2;0,0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;10;-822.9717,84.67657;Float;False;Constant;_white;white;0;0;-0.04;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.Vector3Node;4;-928.1522,-525.5175;Float;False;Constant;_Albedo_color;Albedo_color;0;0;0.1,0.1,0.1;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;11;-844.1224,215.5464;Float;False;Property;_blending;blending;0;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;14;-1020.763,758.5092;Float;False;Property;_Refraction;Refraction;0;0;1;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;13;-1042.291,423.7092;Float;False;Property;_Metallic;Metallic;0;0;0.6687419;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;17;-673.4382,-307.8901;Float;False;3;0;COLOR;0.0;False;1;COLOR;0,0,0;False;2;FLOAT;0.0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;16;-1004.013,867.8625;Float;False;Constant;_Opacity;Opacity;7;0;0.6758156;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;9;-569.1644,19.90269;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;15;-1018.687,676.5242;Float;False;Property;_Translucency;Translucency;0;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;23;-564.4829,485.6823;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;12;-1043.781,517.6247;Float;False;Property;_Smoothness;Smoothness;0;0;0.7354432;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;22;-572.0649,309.5345;Float;False;3;0;COLOR;0.0;False;1;COLOR;0.0;False;2;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Glass_shader_dirty;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;False;0;False;Transparent;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;18;0;20;0
WireConnection;18;1;25;0
WireConnection;26;0;18;0
WireConnection;26;1;24;0
WireConnection;17;0;4;0
WireConnection;17;1;26;0
WireConnection;17;2;24;1
WireConnection;9;0;8;0
WireConnection;9;1;10;0
WireConnection;9;2;11;0
WireConnection;23;0;12;0
WireConnection;23;1;18;0
WireConnection;23;2;24;1
WireConnection;22;0;13;0
WireConnection;22;1;18;0
WireConnection;22;2;24;1
WireConnection;0;0;17;0
WireConnection;0;2;9;0
WireConnection;0;3;13;0
WireConnection;0;4;12;0
WireConnection;0;8;14;0
WireConnection;0;9;16;0
ASEEND*/
//CHKSM=EFAA7DF60F1827A353726B6D58DEF4C04E86FD6F