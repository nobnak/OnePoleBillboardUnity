﻿Shader "Custom/FrontFacing" {
	Properties {
		_TangentCube ("Tangent", CUBE) = "" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass {
			CGPROGRAM
			#pragma target 5.0
			#pragma vertex vert
			#pragma fragment frag

			samplerCUBE _TangentCube;

			struct Input {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};
			struct Inter {
				float4 vertex : POSITION;
				float3 color : TEXCOORD0;
			};
			
			Inter vert(Input IN) {
				float3 worldPos = mul(_Object2World, float4(IN.vertex.xyz, 1)).xyz;
				float3 forward = normalize(worldPos.xyz - _WorldSpaceCameraPos.xyz);
				float3 cTangent = texCUBElod(_TangentCube, float4(forward, 0));
				float3 right = normalize(2 * cTangent - 1);
				float3 up = normalize(cross(forward, right));
				worldPos.xyz += IN.uv2.x * right + IN.uv2.y * up;
				
				Inter OUT;
				OUT.vertex = mul(UNITY_MATRIX_VP, float4(worldPos, 1));
				OUT.color = right;
				return OUT;
			}
			
			float4 frag(Inter IN) : COLOR {
				return float4(IN.color, 1);
			}
			ENDCG
		}
	} 
}
