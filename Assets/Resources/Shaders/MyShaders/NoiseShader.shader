Shader "Custom/NoiseShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Noise("Noise (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Denisity("YFogDenisity", Range(0,1)) = 0.0
		_FogColor("YFogColor", Color) = (1, 1, 1, 1)
			_NoiseScale("NoiseScale", Range(0, 128)) = 4
			_NoiseAlpha("NoiseAlpha", Range(0, 1)) = 1.0
			_NoiseDetailScale("NoiseDetailScale", Range(0, 10)) = 4
			_NoiseDetailAlpha("NoiseDetailAlpha", Range(0, 1)) = 1.0
			_VibrationSpeed("VibrationSpeed", Range(0, 0.2)) = 0.01
			_VibrationScale("VibrationScale", Range(0, 1)) = 0.5
			_NoiseBrightness("NoiseBrightness", Range(0, 2)) = 0.5
			_NoiseContrast("NoiseContrast", Range(0.1, 8)) = 1
	}
	SubShader {
		Tags {"RenderType"="Transparent" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Noise;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos; 
			float3 worldNormal;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		half _Denisity;
		fixed4 _FogColor;
		half _NoiseScale;
		half _NoiseAlpha;
		half _NoiseDetailScale;
		half _NoiseDetailAlpha;
		half _VibrationSpeed;
		half _VibrationScale;
		half _NoiseBrightness;
		half _NoiseContrast;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float normX = abs(IN.worldNormal.x);
			float normY = abs(IN.worldNormal.y);
			float normZ = abs(IN.worldNormal.z);
			// Albedo comes from a texture tinted by color
			float denMult = max(0, -IN.worldPos.y);
			float curDen = min(1, _Denisity * denMult);
			fixed4 col;
			float2 pos;
			float yScale = 1;
			pos = float2 ((IN.worldPos.x + IN.worldPos.y * (normX )) / _NoiseScale, (IN.worldPos.z + IN.worldPos.y * (normY + normZ)) / _NoiseScale);
			pos = float2 ((IN.worldPos.x + IN.worldPos.y * yScale) / _NoiseScale, (IN.worldPos.z + IN.worldPos.y * yScale) / _NoiseScale);
			pos = float2 ((IN.worldPos.x) / _NoiseScale, (IN.worldPos.z + IN.worldPos.y * yScale) / _NoiseScale);
			fixed time =  _Time[1] * _VibrationSpeed;
			fixed test;
			test = (tex2D(_Noise, pos + float2 (time, time))).x;
			test += (tex2D(_Noise, pos + float2 (-time + 0.5, -time))).x;
			test += (tex2D(_Noise, pos + float2 (-time + 0.5, time + 0.5))).x;
			test += (tex2D(_Noise, pos + float2 (time, -time + 0.5))).x;
			test *= _VibrationScale;
			fixed4 c;
			fixed4 temp;
			//c = (tex2D(_Noise, pos + float2 (test, test)) * _NoiseAlpha * _NoiseBrightness + (1 - _NoiseAlpha * (1 - _NoiseBrightness))) * _NoiseContrast;
			//c = (tex2D(_Noise, pos + float2 (test, test)) * _NoiseAlpha * (1 + _NoiseBrightness)) + (1 - _NoiseAlpha);
			c = pow(tex2D(_Noise, pos + float2 (test, test)) * _NoiseAlpha * (1 + _NoiseBrightness) + (1 - _NoiseAlpha), _NoiseContrast);
			//c *= tex2D(_Noise, pos * _NoiseDetailScale + float2 (test, test)).x *(_NoiseDetailAlpha) +(1 - _NoiseDetailAlpha * (1 - _NoiseBrightness));;
			c *= pow(tex2D(_Noise, pos + float2 (test, test) * _NoiseDetailScale) * _NoiseDetailAlpha * (1 + _NoiseBrightness) + (1 - _NoiseDetailAlpha), _NoiseContrast);
			/*c -= 0.1;
			c *= 3;*/
			c *= tex2D(_MainTex, IN.uv_MainTex) * _Color * (1 - curDen);
			c += curDen * _FogColor;
			//c = float4(abs (IN.worldNormal.z), 0, 0, 1);
			//c = (0, 0, 0, 0) ;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
