Shader "Custom/FrontFacing" {
	Properties {
		_TangentCube ("Tangent", CUBE) = "" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			samplerCUBE _TangentCube;

			struct Input {
				float4 vertex : POSITION;
			};
			struct Inter {
				float4 vertex : POSITION;
				float3 color : TEXCOORD0;
			};
			
			Inter vert(Input IN) {
				float4 worldPos = mul(_Object2World, IN.vertex);
				float3 viewDir = worldPos.xyz - _WorldSpaceCameraPos.xyz;
				float3 t = texCUBElod(_TangentCube, float4(viewDir, 0));
				Inter OUT;
				OUT.vertex = mul(UNITY_MATRIX_VP, worldPos);
				OUT.color = t;
				return OUT;
			}
			
			float4 frag(Inter IN) : COLOR {
				return float4(IN.color, 1);
			}
			ENDCG
		}
	} 
}
