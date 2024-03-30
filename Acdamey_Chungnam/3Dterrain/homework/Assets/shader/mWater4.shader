Shader "Custom/water4"
{
    Properties
    {
        _CubeMap("CubeMap", cube) = "" {}
        _MainTex("MainTex", 2D) = "white" {}
        _BumpMap("Water Bump", 2D) = "bump" {}
       

        _SpecColor("Spec", color) = (1,1,1,1)
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque"}
            LOD 200

            GrabPass{}

            CGPROGRAM
            #pragma surface surf BlinnPhong vertex:vert
            #pragma target 3.0

            samplerCUBE _CubeMap;
            sampler2D _BumpMap;
            sampler2D _MainTex;
            sampler2D _GrabTexture;

            float4 _SepcColor;

            struct Input
            {
                float2 uv_MainTex;
                float2 uv_BumpMap;
                float3 worldRefl;
                float3 viewDir;
                float4 screenPos;

                INTERNAL_DATA
            };

            void vert(inout appdata_full v)
            {
                v.vertex.z += cos(abs(v.texcoord.x * 2 - 1) * 10 + _Time.y) * 0.03;
                v.vertex.y += sin((abs(v.texcoord.x * 2 - 1) * 3) + sin(_Time.y * 1)) * 0.1;
            }

            void surf(Input IN, inout SurfaceOutput o)
            {
                // Normal
                float3 fNormal1 = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap + float2(_Time.y * 0.05, 0.0f)));
                float3 fNormal2 = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap + float2(_Time.y * 0.02, 0.0f)));
                o.Normal = (fNormal1 + fNormal2) / 2;

                // reflection
                float3 fRefl = texCUBE(_CubeMap, WorldReflectionVector(IN, o.Normal));

                // rim
                float fRim = dot(o.Normal, IN.viewDir);
                fRim = saturate(pow(1 - fRim, 2));

                //grab
                float4 fNoise = tex2D(_MainTex, IN.uv_MainTex + _Time.x);
                float3 scrPos = IN.screenPos.xyz / (IN.screenPos.w );
                float3 fGrab = tex2D(_GrabTexture, scrPos.xy + fNoise.r * 0.05);

                o.Gloss = 1;
                o.Specular = 1;
                o.Alpha = 1;

                o.Emission = lerp(fGrab, fRefl, fRim);
            }
            ENDCG
        }
            FallBack "Diffuse"

}
