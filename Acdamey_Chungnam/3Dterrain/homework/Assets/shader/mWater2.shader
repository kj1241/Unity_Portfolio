Shader "Custom/mWater2"
{
    Properties
    {
        _MainTex("tex",2D) = "white" {}
       _CUBE("Cubemap",CUBE) = ""{}
       _BumpTex("bumptex",2D) = "white"{}
    }
        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 200

        CGPROGRAM
   #pragma surface surf BlinnPhong vertex:vert

        #pragma target 3.0

       
        sampler2D _MainTex;
        sampler2D _BumpTex;
        samplerCUBE _CUBE;

        void vert(inout appdata_full v)
        {
            v.vertex.z += cos(abs(v.texcoord.x*2-1)*10+_Time.y)*0.03;
            //파도간격 ,파도속도,파도높이
        }


        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpTex;
            float3 worldRefl;
            INTERNAL_DATA
        };

  

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            float3 normal1 = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex + _Time.y * 0.05));
            float3 normal2 = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex + _Time.y * 0.02));

            o.Normal = (normal1 + normal2) * 0.5;
            o.Normal *= float3(0.5, 0.5, 1);


            float4 reflection = texCUBE(_CUBE, WorldReflectionVector(IN, o.Normal));
            o.Emission = reflection*1.05;
            o.Alpha = 1;
        }

        float4 Lightingwater(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
        {
            float rim = saturate(dot(s.Normal, viewDir));
            float rim1 = pow(1 - rim, 20);
            float rim2 = pow(1 - rim, 2);
            float3 H = normalize(lightDir + viewDir);
            float spec = saturate(dot(s.Normal, H));
            spec = pow(spec, 1050) * 10;
            //rim = pow(1 - rim, 3);

            float4 final = saturate(rim1+spec) * _LightColor0;

            return float4(final.rgb, saturate(rim2 + spec));
        }



        ENDCG
    }
    FallBack "Diffuse"
}
