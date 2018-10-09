// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BigOne/ShaderonePowerOverlap"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_LightRamp("Light ramp", 2D) = "white" {}
		_SpecularRamp("Specular ramp", 2D) = "black" {}
	}

	SubShader
	{
		Tags{ "Queue" = "Overlay" }
		
		Name "Hidden"

		ZWrite Off
		ZTest Always

		CGPROGRAM

#pragma surface surf CustomLighting fullforwardshadows noambient
#pragma	vertex vert

		sampler2D _MainTex;

		sampler2D _LightRamp;
		sampler2D _SpecularRamp;

		struct Input
		{
			float2 uv_MainTex;
		};

		struct CustomSurfaceOutput
		{
			//Standard surface shader fields ----- 
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			fixed Alpha;
			//------------------------------------
		};

		void vert(inout appdata_full v, out Input output)
		{
			UNITY_INITIALIZE_OUTPUT(Input, output);
		}

		half4 LightingCustomLighting(CustomSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
		{
			half diffuseComponent = dot(s.Normal, lightDir) * 0.5 + 0.5;

			half3 reflectedLightDir = normalize(reflect(-lightDir, s.Normal));
			half specularComponent = dot(reflectedLightDir, viewDir) * 0.5 + 0.5;
			half3 specularColor = tex2D(_SpecularRamp, half2(specularComponent, 0.0)) * _LightColor0.rgb;

			fixed3 lightColor = tex2D(_LightRamp, half2(diffuseComponent, 0.0)).rgb * _LightColor0.rgb;

			half4 color;
			color.rgb = (s.Albedo * lightColor + specularColor) * atten;
			color.rgb += s.Emission;

			color.a = s.Alpha;

			return color;
		}

		void surf(Input input, inout CustomSurfaceOutput output)
		{
			fixed4 mainColor = tex2D(_MainTex, input.uv_MainTex);

			output.Albedo = mainColor.rgb;

			output.Alpha = mainColor.a;
		}

		ENDCG
	}
}