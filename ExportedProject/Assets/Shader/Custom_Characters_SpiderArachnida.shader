Shader "Custom/Characters/SpiderArachnida" {
	Properties {
		_MainTex ("Diffuse", 2D) = "white" {}
		_TintColor ("Color", Vector) = (1,1,1,1)
		_DissolveRimColor ("Dissolve rim Color", Vector) = (0.5,0.5,0.5,1)
		_DissolveTexture ("Dissolve Mask", 2D) = "white" {}
		_DissolveDuration ("Dissolve duration", Float) = 2
		_EmissivieTexture ("Emissive", 2D) = "black" {}
		_EmisColor ("Emisive color", Vector) = (1,1,1,1)
		_EmissiveIntensity ("Emissive Intensity", Range(0, 10)) = 1
		_BloodTexture ("BloodUV2", 2D) = "black" {}
		_BloodColor ("Blood color", Vector) = (0.6,0,0.05,1)
		_ShadowClampMultiplier ("Shadow clamp contrast", Range(1, 2)) = 1.25
		_ShadowClampAdd ("Shadow clamp brightness", Range(0, 1)) = 0.2
		_LambertWrap ("Lambert Wrap", Range(0, 1)) = 0.5
		_UV3Emission ("UV3 Emission", 2D) = "black" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixVP;
			float4 _MainTex_ST;

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Vertex_Stage_Output
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.uv = (input.uv.xy * _MainTex_ST.xy) + _MainTex_ST.zw;
				output.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, input.pos));
				return output;
			}

			Texture2D<float4> _MainTex;
			SamplerState sampler_MainTex;

			struct Fragment_Stage_Input
			{
				float2 uv : TEXCOORD0;
			};

			float4 frag(Fragment_Stage_Input input) : SV_TARGET
			{
				return _MainTex.Sample(sampler_MainTex, input.uv.xy);
			}

			ENDHLSL
		}
	}
}