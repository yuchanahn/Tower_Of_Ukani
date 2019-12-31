Shader "Custom/Offset" {
	Properties{
	   _Color("Color (RGBA)", Color) = (1,1,1,1)
	   [NoScaleOffset] _MainTex("Texture (RGBA)", 2D) = "white" {}
		_Offset("Offset", Vector) = (1, 1, 0, 0)
	}

		SubShader
		{
			Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" "PreviewType" = "Plane" }

			//subshaders are used for compatibility. If the first subshader isn't compatible, it'll attempt to use the one below it.
			Pass
			{

				Blend SrcAlpha OneMinusSrcAlpha
				ZWrite Off
				ZTest LEqual
				Lighting Off
				Cull Off
				CGPROGRAM
			//begin CG block

			#pragma vertex vert
			//we will use a vertex function, named "vert". vert_img is defined in UnityCG.cginc

			#pragma fragment frag
			//we will use a fragment function, named "frag"

			#include "UnityCG.cginc"
			//use a CGInclude file defining several useful functions, including our vertex function

			//declare our external properties
			uniform fixed4 _Color;
			uniform sampler2D _MainTex;
			uniform half2 _Offset;

			//declare input and output structs for vertex and fragment functions

			struct appdata
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord + _Offset;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// : SV_Target semantic marks the return value as the color of the fragment.

				fixed4 textureCol = tex2D(_MainTex, i.uv);
			//sample from the current texture
			return _Color * textureCol; //return tinted texture color
		}
	ENDCG
}
		}
			Fallback "Diffuse" //If all of our subshaders aren't compatible, use subshaders from a different shader file
}