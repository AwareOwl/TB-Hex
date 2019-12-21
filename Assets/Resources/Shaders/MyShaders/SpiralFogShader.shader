Shader "Custom/SpiralFogShader"
{
	Properties
	{
		_Tint("TintColor", Color) = (1, 1, 1, 1)
		_MainTex ("Texture", 2D) = "white" {}
		_MainTexScale("MainTexScale", Range(0,64)) = 1
		_DetailTex("DetailTexture", 2D) = "white" {}
		_DetailTexScale("DetailTexScale", Range(0,64)) = 1
			_CentreX("Centre X", Float) = 0
			_CentreZ("Centre Z", Float) = 0
		_TransparencyRange ("TransparencyRange", Range(0.000000001,32)) = 1
		_FadingTransparencyRange("FadingTransparencyLength", Range(0,32)) = 1
		_Spiral("Spiral", Range(0,100)) = 1
			_RotatingSpeed1("RotatingSpeed1", Range(0,4)) = 1
			_RotatingSpeed2("RotatingSpeed2", Range(0,4)) = 1
			_RotatingSpeed3("RotatingSpeed3", Range(0,4)) = 1
	}
	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 worldVertex : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			float4 _Tint;
			sampler2D _MainTex;
			half _MainTexScale;
			sampler2D _DetailTex;
			half _DetailTexScale;
			half _CentreX;
			half _CentreZ;
			half _TransparencyRange;
			half _FadingTransparencyRange;
			float4 _MainTex_ST;
			half _Spiral;
			half _RotatingSpeed1;
			half _RotatingSpeed2;
			half _RotatingSpeed3;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldVertex = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1)).xyz;
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				const float PI = 3.14159;
				fixed time = _Time[1];
				// sample the texture
				float WVX = i.worldVertex.x - _CentreX; // pozycja X pixela
				float WVZ = i.worldVertex.z - _CentreZ; // pozycja Z pixela 
				float distance = sqrt (WVX * WVX + WVZ * WVZ); // odległość pixela od centrum
				float atan22 = atan2(WVZ, WVX); // kąt piksela od osi X względem centrum
				float temp = atan22 + sqrt(distance);
				float angle;

				float2 texPos1 = i.worldVertex.xz;
				angle = temp - time * _RotatingSpeed1;
				texPos1.x = distance * cos(angle);
				texPos1.y = distance * sin(angle);
				texPos1 /= _MainTexScale;

				float2 texPos2 = i.worldVertex.xz;
				angle = temp - time * _RotatingSpeed2;
				texPos2.x = distance * cos(angle);
				texPos2.y = distance * sin(angle);
				texPos2 /= _MainTexScale;

				float2 texPos3 = i.worldVertex.xz;
				angle = temp - time * _RotatingSpeed3;
				texPos3.x = distance * cos(angle);
				texPos3.y = distance * sin(angle);
				texPos3 /= _MainTexScale;

				fixed4 col = (1, 1, 1, 1);
				col *= tex2D(_MainTex, texPos1) + tex2D(_MainTex, texPos2) + tex2D(_MainTex, texPos2);
				//col /= 3;
				col *= tex2D(_DetailTex, texPos1 * _DetailTexScale) + tex2D(_DetailTex, texPos2 * _DetailTexScale) + tex2D(_DetailTex, texPos3 *_DetailTexScale);
				col /= 3;
				//col += 0.1;
				//col /= 2;
				col.a = clamp (col.r, 0, 1);
				//col.a = 1;
				col.a *= clamp((distance / _TransparencyRange - 1) * _TransparencyRange / _FadingTransparencyRange, 0, 1);
				col.xyz *= _Tint;
				//col = float4 (0, 0, 0, 1);
				//col.r = cos(distance);// / PI;
				//col.r = tex2D(_MainTex, i.worldVertex.xz / _MainTexScale).r;
				//col.r += atan22;
				//col.b -= atan22;
				return col;
			}
			ENDCG
		}
	}
}
