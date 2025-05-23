// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Effects/Explosions/Particles/Alpha Blended" {
Properties {
	[HDR]_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("Particle Texture", 2D) = "white" {}
	_InvFade ("Soft Particles Factor", Float) = 0.5
}

SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Pass {
			Fog { Mode Off}
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _TintColor;

			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD4;
				#endif

			};

			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				#endif
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}

			sampler2D _CameraDepthTexture;
			float _InvFade;

			half4 frag (v2f i) : COLOR
			{
				#ifdef SOFTPARTICLES_ON
				if(_InvFade > 0.0001)	{
					float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
					float partZ = i.projPos.z;
					float fade = saturate (_InvFade * (sceneZ-partZ));
					i.color.a *= fade;
				}
				#endif

				half4 col = 2.0f * i.color * tex2D(_MainTex, i.texcoord);
				_TintColor.rgb *= _TintColor.rgb * 2;
				half gray = dot(col.rgb, 0.3);
				col.rgb *= gray * _TintColor.rgb;
				col.rgb += (1-IsGammaSpace()) * 3 * col.rgb;
				col.a = saturate(col.a * _TintColor.a);
				return col;
			}
			ENDCG
		}
	}
}
