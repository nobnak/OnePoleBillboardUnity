Shader "Custom/TangentPicker" {
	Properties {
		_TangentCube ("Tangent", CUBE) = "" {}
		_Color ("Color", Color) = (0.5, 0.5, 0.5, 1.0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200 Cull Off
		
		Pass {
			CGPROGRAM
			#pragma target 5.0
			#pragma vertex vert
			#pragma fragment frag

			samplerCUBE _TangentCube;
			float4 _Color;

			struct Input {
				float4 vertex : POSITION;
			};
			struct Inter {
				float4 vertex : POSITION;
				float4 forward : TEXCOORD0;
			};
			
			Inter vert(Input IN) {
				float3 worldPos = mul(_Object2World, float4(IN.vertex.xyz, 1)).xyz;
				float3 forward = normalize(worldPos.xyz - _WorldSpaceCameraPos.xyz);

				Inter OUT;
				OUT.vertex = mul(UNITY_MATRIX_VP, float4(worldPos, 1));
				OUT.forward = float4(forward, 0);
				return OUT;
			}
			
			float4 frag(Inter IN) : COLOR {
				float3 cTangent = texCUBElod(_TangentCube, IN.forward);
				float3 right = normalize(2 * cTangent - 1);				
				return float4(right * _Color.rgb * _Color.a, 1.0);
			}
			ENDCG
		}
	} 
}
