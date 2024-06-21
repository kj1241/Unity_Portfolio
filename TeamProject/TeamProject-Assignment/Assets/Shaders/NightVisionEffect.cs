using UnityEngine;
using System.Collections;

//
[ExecuteInEditMode]
public class NightVisionEffect : MonoBehaviour 
{
	#region Variables
	public Shader nightVisionShader;
	
	public float contrast = 2.0f;
	public float brightness = 1.0f;
	public Color nightVisionColor = Color.white;
	
	public Texture2D vignetteTexture;
	
	public Texture2D scanLineTexture;
	public float scanLineTileAmount = 4.0f;
	
	public Texture2D nightVisionNoise;
	public float noiseXSpeed = 100.0f;
	public float noiseYSpeed = 100.0f;
	
	public float distortion = 0.2f;
	public float scale = 0.8f;
	
	private float randomValue = 0.0f;
	private Material curMaterial;
	#endregion
	
	#region Properties
	Material material
	{
		get
		{
			if(curMaterial == null)
			{
				curMaterial = new Material(nightVisionShader);
				curMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return curMaterial;
		}
	}
	#endregion
	
	void Start()
	{
		//예전버전 이제는 후처리 이펙트는 항상 ture임
		//if(!SystemInfo.supportsImageEffects) 
		//{
		//	enabled = false;
		//	return;
		//}
		
		if(!nightVisionShader || !nightVisionShader.isSupported)
		{
		
			enabled = false; //GetComponent<NightVisionEffect>().enabled = false;
		}

		contrast = Mathf.Clamp(contrast, 0f, 4f);
		brightness = Mathf.Clamp(brightness, 0f, 2f);
		randomValue = Random.Range(-1f, 1f);
		distortion = Mathf.Clamp(distortion, -1f, 1f);
		scale = Mathf.Clamp(scale, 0f, 3f);

	}
	
	//포스트 프로세싱 작성
	void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		//쉐이더 변수들 넣어주기
		if(nightVisionShader != null)
		{	
			material.SetFloat("_Contrast", contrast);
			material.SetFloat("_Brightness", brightness);
			material.SetColor("_NightVisionColor", nightVisionColor);
			material.SetFloat("_RandomValue", randomValue);
			material.SetFloat("_distortion", distortion);
			material.SetFloat("_scale",scale);
			
			if(vignetteTexture)
			{
				material.SetTexture("_VignetteTex", vignetteTexture);
			}
			
			if(scanLineTexture)
			{
				material.SetTexture("_ScanLineTex", scanLineTexture);
				material.SetFloat("_ScanLineTileAmount", scanLineTileAmount);
			}
			
			if(nightVisionNoise)
			{
				material.SetTexture("_NoiseTex", nightVisionNoise);
				material.SetFloat("_NoiseXSpeed", noiseXSpeed);
				material.SetFloat("_NoiseYSpeed", noiseYSpeed);
			}
			//카메라 렌더링할 이미지 교체
			Graphics.Blit(sourceTexture, destTexture, material);
		}
		else
		{
			//원래대로
			Graphics.Blit(sourceTexture, destTexture);
		}
	}
	
	void Update()
	{
		
	}
	
	void OnDisable()
	{
		if(curMaterial)
		{
			//바로 삭제하면 렌더링 파이프라인도중 에러날수 있음 따라서 시간 지나고 삭제
			DestroyImmediate(curMaterial);
		}
	}
}
