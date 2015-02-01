Shader "Custom/Tangent" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (0.5, 0.5, 0.5, 1.0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200 Cull Off
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			float4 _Color;

			struct Input {
				float4 vertex : POSITION;
				float4 tangent : TANGENT;
				float2 uv : TEXCOORD0;
			};
			struct Inter {
				float4 vertex : POSITION;
				float3 tangent : TANGENT;
				float2 uv : TEXCOORD0;
			};

			Inter vert(Input IN) {
				float3 worldTang = mul(_Object2World, float4(IN.tangent.xyz, 0)).xyz;
				worldTang *= unity_Scale.w;
			
				Inter OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.tangent = worldTang;
				OUT.uv = IN.uv;
				return OUT;
			}
			float4 frag(Inter IN) : COLOR {
				#if defined(TEXTURE)
				return tex2D(_MainTex, IN.uv);
				#elif defined(RAW)
				return float4(IN.tangent, 1);
				#else
				return float4(0.5 * (normalize(IN.tangent) + 1), 1);
				#endif
			}
			ENDCG
		}
	} 
}
