// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/GridLineMat"{
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
	}
		SubShader{
			Lighting Off
			Cull Off
			ZWrite Off
			ZTest always
			Fog { Mode Off }

			Tags { "Queue" = "Transparent+1" }
			Pass {
				Blend SrcAlpha OneMinusSrcAlpha
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#include "UnityCG.cginc"

					float4 _Color;

					struct v2f {
						float4 pos : SV_POSITION;
						float4 col : COLOR;
					};

					v2f vert(appdata_full vInput) {
						v2f OUT;
						OUT.pos = UnityObjectToClipPos(vInput.vertex);
						OUT.col = vInput.color;
						return OUT;
					}

					half4 frag(v2f fInput) : COLOR {
						return _Color * fInput.col;
					}
				ENDCG
			}
	}
		FallBack "Diffuse"
}