﻿Shader "Custom/Grab/GrabGravityShader"
{
	Properties
	{
		_BlackSize("BlackSize", Range(0,0.999)) = 0.1
		_GravityPower("Gravity Power", Float) = 0
	}

	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }

		Cull Back
		ZWrite On
		ZTest LEqual
		ColorMask RGB

		GrabPass { "_GrabPassTexture" }

		Pass {

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				half4 vertex                : POSITION;
				half4 texcoord              : TEXCOORD0;
			};

			struct v2f {
				half4 vertex                : SV_POSITION;
				half2 uv                    : TEXCOORD0;
				half4 renderRect               : TEXCOORD2;
			};

			sampler2D _GrabPassTexture;
			half _BlackSize;
			half _GravityPower;

			v2f vert(appdata v)
			{
				v2f o = (v2f)0;
				o.uv = v.texcoord.xy;
				float scaleX = length(float3(unity_ObjectToWorld[0].x, unity_ObjectToWorld[1].x, unity_ObjectToWorld[2].x));
				half4 originPos = mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1));
				o.vertex = mul(UNITY_MATRIX_P, originPos + float4(o.uv - 0.5, 0, 0)*scaleX);

				o.renderRect = half4(	ComputeGrabScreenPos(mul(UNITY_MATRIX_P, originPos - float4(0.5, -0.5, 0, 0)*scaleX)).xy,
										ComputeGrabScreenPos(mul(UNITY_MATRIX_P, originPos + float4(0.5, -0.5, 0, 0)*scaleX)).xy)/ o.vertex.w;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				half r = length(i.uv * 2 - 1);
				half t = -atan2(i.uv.y*2-1,i.uv.x*2-1);


				half a = -1 / (_BlackSize-1);
				half b = 1 - a;

				half r2 = min(max(a*r+b,0),r);
				half t2 = t;


				half2 pos = (i.renderRect.zw + i.renderRect.xy) / 2 + half2(cos(t2), sin(t2)) * r2 * (i.renderRect.zw - i.renderRect.xy)*0.5;


				return  fixed4(tex2D(_GrabPassTexture, pos).xyz*ceil(min(r2,1)),1);
			}
			ENDCG
		}
	}
}
