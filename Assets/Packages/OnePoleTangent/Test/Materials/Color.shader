Shader "Custom/Color" {
	Properties {
		_Color ("Color", Color) = (0.5, 0.5, 0.5, 1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4 _Color;

			struct Input {
				float4 vertex : POSITION;
			};
			struct Inter {
				float4 vertex : POSITION;
			};

			Inter vert(Input IN) {
				Inter OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				return OUT;
			}
			float4 frag(Inter IN) : COLOR {
				return _Color;
			}
			ENDCG
		}
	} 
}
