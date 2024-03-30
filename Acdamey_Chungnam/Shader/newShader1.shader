Shader "Custom/newShader1"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100
		Pass
		{
			Material
			{
				Diffuse(1,1,1,1)
				Ambient(1,1,1,1)
			}
			Lighting On
			SetTexture[_MainTex] {
				constantColor(1,1,1,1)
				Combine texture * primary DOUBLE, texture * constant
			}
		}
	}
    FallBack "Diffuse"
}
