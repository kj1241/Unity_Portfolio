Shader "Custom/NightVisionEffectShader" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_VignetteTex ("Vignette Texture", 2D) = "white"{}
		_ScanLineTex ("Scan Line Texture", 2D) = "white"{}
		_NoiseTex ("Noise Texture", 2D) = "white"{}
		_NoiseXSpeed ("Noise X Speed", Float) = 100.0
		_NoiseYSpeed ("Noise Y Speed", Float) = 100.0
		_ScanLineTileAmount ("Scan Line Tile Amount", Float) = 4.0
		_NightVisionColor ("Night Vision Color", Color) = (1,1,1,1)
		_Contrast ("Contrast", Range(0,4)) = 2
		_Brightness ("Brightness", Range(0,2)) = 1
		_RandomValue ("Random Value", Float) = 0
		_distortion ("Distortion", Float) = 0.2
		_scale ("Scale (Zoom)", Float) = 0.8
	}
	
	SubShader 
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform sampler2D _VignetteTex;
			uniform sampler2D _ScanLineTex;
			uniform sampler2D _NoiseTex;
			fixed4 _NightVisionColor;
			fixed _Contrast;
			fixed _ScanLineTileAmount;
			fixed _Brightness;
			fixed _RandomValue;
			fixed _NoiseXSpeed;
			fixed _NoiseYSpeed;
			fixed _distortion;
			fixed _scale;
			
			float2 barrelDistortion(float2 coord) 
			{
				// 랜즈 왜곡 알고리즘 참조: http://www.ssontech.com/content/lensalg.htm
				float2 h = coord.xy - float2(0.5, 0.5);
				float r2 = h.x * h.x + h.y * h.y;
				float f = 1.0 + r2 * (_distortion * sqrt(r2));

				return f * _scale * h + 0.5;
			}
			
			//알파값을 가질수 없음
			fixed4 frag(v2f_img i) : COLOR
			{
				//랜더 텍스처의 uv 색상 가저오기
				half2 distortedUV = barrelDistortion(i.uv); //베럴 왜곡(블록렌즈)
				fixed4 renderTex = tex2D(_MainTex, distortedUV);//메인텍스처 입히기
				fixed4 vignetteTex = tex2D(_VignetteTex, i.uv);//줌 모드 텍스쳐 가져오기
				
				//스캔 라인 노이즈 처리
				half2 scanLinesUV = half2(i.uv.x * _ScanLineTileAmount, i.uv.y * _ScanLineTileAmount);//UV 값에  _ScanLineTileAmount 곱하기
				fixed4 scanLineTex = tex2D(_ScanLineTex, scanLinesUV);// 매칭
			
				
				// _SinTime.x == Sin(t/8),  _SinTime.y == Sin(t/4),  _SinTime.z == Sin(t/2), _SinTime.w == Sin(t)
				//시간 단위의 사인 함수 그래프(시간도 차원임으로)
				half2 noiseUV = half2(i.uv.x + (_RandomValue * _SinTime.z * _NoiseXSpeed), i.uv.y + (_Time.x * _NoiseYSpeed));
				fixed4 noiseTex = tex2D(_NoiseTex, noiseUV); //적용
				
				//아날로그 컬러 TV에 사용되는 색을 사용하기 위에 rgb -> YIQ 변환에서 Y 끌어다 오기
 				fixed lum = dot (fixed3(0.299, 0.587, 0.0114), renderTex.rgb); // rgb -> Y
				lum += _Brightness; //광도 더해주기
				fixed4 finalColor = (lum *2) + _NightVisionColor; //비전색상값 더하기

				//출력
				finalColor = pow(finalColor, _Contrast); //finalColor^ _Contrast
				finalColor *= vignetteTex; //줌 모드 텍스쳐
				finalColor *= scanLineTex * noiseTex; //노이즈
				finalColor.a = finalColor.r;
				
				return finalColor;
			}
	
			ENDCG
		}
	} 
	FallBack off
}
