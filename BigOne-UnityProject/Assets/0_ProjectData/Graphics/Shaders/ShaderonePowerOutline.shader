// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BigOne/ShaderonePowerOutline"
{
	Properties
	{
		_OutlineColor("Outline Color (A thickness)", Color) = (0,0,0,0.1)
		_Outline("Outline width", Range(0.0, 10.0)) = .005
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" }

		Pass //Outline pass
		{
			Name "Outline"

			Cull Front
			ZTest Always

			CGPROGRAM

#pragma vertex vert 
#pragma fragment frag

			float4  _OutlineColor;
			float  _Outline;

			struct AppData
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct VertexToFragment
			{
				float4 pos : SV_POSITION;
			};

			// vertex shader 
			VertexToFragment vert(AppData input)
			{
				VertexToFragment output;

				input.vertex.xyz += input.normal * _OutlineColor.a * _Outline;

				output.pos = UnityObjectToClipPos(input.vertex);

				return output;
			}

			// fragment shader
			float4 frag(VertexToFragment input) : COLOR
			{
				return _OutlineColor;
			}

			ENDCG // here ends the part in Cg 
		}
	}
}