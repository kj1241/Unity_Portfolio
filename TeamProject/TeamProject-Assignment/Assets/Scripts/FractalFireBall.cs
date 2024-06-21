using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireBall
{
	//[ExecuteInEditMode()]
	public class FractalFireBall : MonoBehaviour
	{
		private string[] qualitySelect = new[] { "QUALITY_LOW", "QUALITY_MED", "QUALITY_HIGH" };
		bool isUpdate = true;
		float _radius;

		public Texture2D TextureColor; //그라디에이션
		public Texture2D Noise; //노이즈
		public Material Material; //머터리얼
		public float Heat = 1; //열기
		public float Alpha = 1; //알파값
		public float Speed = 1;  // 속도값
		public float Frequency = 1; //빈도값
		public bool Scattering = true; //산란
		public int Octaves = 4; //질량 옥타브
		public int Quality = 2; //퀄리티

		Material rsm;
		float changeHeat = 1;
		float ChangAlpha = 1;
		float changeScroll = 1;
		float changeFreq = 1;
		bool changeScatter = true;
		int changeOctaves = 4;
		int changeQuality = 2;


		// Start is called before the first frame update
		void Start()
		{
			GetComponent<Renderer>().material = Material;
			ShaderProperties();
		}

		// Update is called once per frame
		void Update()
		{
			float minscale = Mathf.Min(transform.lossyScale.x, Mathf.Min(transform.lossyScale.y, transform.lossyScale.z));
			// 변경된 사항이 있으면 업데이트
			if (minscale != _radius)
			{
				_radius = minscale;
				rsm.SetFloat("_Radius", _radius / 2.03f - 1);
			}
			if (changeHeat != Heat)
			{
				changeHeat = Heat;
				rsm.SetFloat("_Heat", Heat);
			}
			if (ChangAlpha != Alpha)
			{
				ChangAlpha = Alpha;
				rsm.SetFloat("_Alpha", Alpha);
			}
			if (changeScroll != Speed)
			{
				changeScroll = Speed;
				rsm.SetFloat("_ScrollSpeed", Speed);
			}
			if (changeFreq != Frequency)
			{
				changeFreq = Frequency;
				rsm.SetFloat("_Frequency", Frequency);
			}
			if (changeScatter != Scattering || changeOctaves != Octaves || changeQuality != Quality)
			{
				changeScatter = Scattering;
				changeOctaves = Octaves;
				changeQuality = Quality;
				SetShaderBox();
			}
		}

		public void ShaderProperties()
		{
			rsm = GetComponent<Renderer>().sharedMaterial;
			rsm = Material;

			rsm.SetTexture("_MainTex", TextureColor);
			rsm.SetTexture("_NoiseTex", Noise);
			rsm.SetFloat("_Heat", Heat);
			rsm.SetFloat("_Alpha", Alpha);
			rsm.SetFloat("_ScrollSpeed", Speed);
			rsm.SetFloat("_Frequency", Frequency);
			SetShaderBox();
		}

		void SetShaderBox()
		{
			string ScatterString;
			if (Scattering)
				ScatterString = "SCATTERING_ON";
			else
				ScatterString = "SCATTERING_OFF";

			List<string> ShaderKeywords = new List<string> { ScatterString, "OCTAVES_" + Octaves, qualitySelect[Quality] };
			GetComponent<Renderer>().sharedMaterial.shaderKeywords = ShaderKeywords.ToArray();
		}
	}
}
