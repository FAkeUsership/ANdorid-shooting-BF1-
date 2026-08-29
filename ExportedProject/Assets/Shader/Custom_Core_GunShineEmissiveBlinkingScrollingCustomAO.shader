Shader "Custom/Core/GunShineEmissiveBlinkingScrollingCustomAO" {
	Properties {
		[HideInInspector] NoScene ("Should this material be in scene?", Float) = 1
		_MainTex ("Main Texture", 2D) = "white" {}
		_TintColor ("Color", Vector) = (1,1,1,1)
		[KeywordEnum(On, Off)] _Specular ("Enable specular", Float) = 0
		_SpecularColor ("Specular Color", Vector) = (0.5,0.5,0.5,1)
		_Shininess ("Shininess", Range(0.01, 1)) = 0.078125
		_SpecStrength ("Specular Strength", Range(0.01, 10)) = 2
		_OverallShininess ("OverallShininess", Range(0, 1)) = 0.5
		_DirtBias ("Dirt", Range(0, 1)) = 0.5
		_DirtColor ("DirtColor", Vector) = (0,0,0,1)
		[KeywordEnum(On, Off)] _Emission ("Emissive", Float) = 0
		_EmissivieTexture ("Emissive Texture", 2D) = "white" {}
		_EmisColor ("Emisive color", Vector) = (1,1,1,1)
		_EmisIntesity ("Emissive intensity", Float) = 0
		_EmisScroll ("Emissive Scroll Texture", 2D) = "white" {}
		_EmisScrollX ("Emissive ScrollX", Float) = 1
		_EmisScrollY ("Emissive ScrollY", Float) = 1
		[KeywordEnum(Off,On)] _UseGlobalCubemap ("Use global cubemap", Float) = 0
		[KeywordEnum(On, Off)] _Cubemap ("Cubemap", Float) = 0
		_SpecCubeTex ("SpecCube", Cube) = "black" {}
		_SpecCubeTexIntensity ("CubemapIntensity", Range(0, 2)) = 1
		[KeywordEnum(On, Off)] _AO ("AO", Float) = 0
		_AOTexture ("Ambient Occlusion Texture", 2D) = "white" {}
		_AOIntensity ("AO Intensity", Range(0, 5)) = 1
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