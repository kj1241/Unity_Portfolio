Shader "Custom/mWater3"
{
    Properties
    {
       _MainTex("tex",2D) = "white" {}
       _CUBE("Cubemap",cube) = ""{}
       _BumpTex("bumptex",2D) = "bump"{}
     
       _SpecColor("Spec", color) = (1,1,1,1)

    }
        SubShader
    {
     Tags { "RenderType" = "Opaque" }
        LOD 200
        GrabPass{}

      
       CGPROGRAM
       #pragma surface surf BlinnPhong vertex:vert
       #pragma target 3.0


        sampler2D _MainTex;
        sampler2D _BumpTex;
        sampler2D _BumpTex2;
       // sampler2D _GrabTex;  //솔찍히 이거 왜 밑에 굴절안나는지 모르겠음
        sampler2D _GrabTexture;
  
        samplerCUBE _CUBE;

        float4 _SepcColor;

        void vert(inout appdata_full v)
        {
            v.vertex.z += cos(abs(v.texcoord.x * 2 - 1) * 10 + _Time.y) * 0.03;
            //파도간격 ,파도속도,파도높이
            v.vertex.y += sin((abs(v.texcoord.x * 2 - 1) * 3) + sin(_Time.y * 1)) * 0.1;
        }


        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpTex;
            float2 uv_BumpTex2;
            float3 worldRefl;
            float4 screenPos;
            float3 viewDir;
            INTERNAL_DATA
        };



        void surf(Input IN, inout SurfaceOutput o)
        {
     
           // fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            float3 normal1 = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex +float2( _Time.y * 0.05,0.0f)));
            float3 normal2 = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex +float2( _Time.y * 0.02,0.0f)));

            o.Normal = (normal1 + normal2) * 0.5;
            o.Normal *= float3(0.5, 0.5, 1);



            float4 reflection = texCUBE(_CUBE, WorldReflectionVector(IN, o.Normal));
            
       

            float rim = dot(o.Normal, IN.viewDir);
            float rim1 = saturate(pow(1 - rim, 20));
            float rim2 = saturate(pow(1 - rim, 2));


            float4  noise = tex2D(_MainTex, IN.uv_MainTex + _Time.x);//굴절 고처야되는데..
            float3 scrPos = IN.screenPos.xyz / (IN.screenPos.w + 0.0000001);
            float3 grab = tex2D(_GrabTexture, scrPos.xy + o.Normal.xy*0.03)*0.5;
                
            o.Emission = lerp(grab, reflection, rim2) ;
            o.Gloss = 1;
            o.Specular = 1;
            o.Alpha = 1;
            o.Emission = lerp(grab, reflection, rim2) + (rim1 * _LightColor0);
           
        }
        
        float4 Lightingwater(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
        {
          
            //rim = pow(1 - rim, 3);
            float3 H = normalize(lightDir + viewDir);
            float spec = saturate(dot(s.Normal, H));
            spec = pow(spec, 1050) * 10;

            float4 final;
            final.rgb = spec * _LightColor0;
            final.a = s.Alpha + spec;

            return final;
        }
        



        ENDCG
    }
    FallBack "Diffuse"
}
