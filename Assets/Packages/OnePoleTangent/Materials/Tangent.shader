Shader "Custom/Tangent" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200 Cull Off
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;

			struct Input {
				float4 vertex : POSITION;
				float4 tangent : TANGENT;
				float2 uv : TEXCOORD0;
			};
			struct Inter {
				float4 vertex : POSITION;
				float4 tangent : TANGENT;
				float2 uv : TEXCOORD0;
			};

			Inter vert(Input IN) {
				Inter OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.tangent = IN.tangent;
				OUT.uv = IN.uv;
				return OUT;
			}
			float4 frag(Inter IN) : COLOR {
				#if TEXTURE
				float4 c = tex2D(_MainTex, IN.uv);
				return c;
				#endif
				
				float4 t = IN.tangent;
				return float4(0.5 * (t.xyz + 1), 1);
			}
			ENDCG
		}
	} 
}
