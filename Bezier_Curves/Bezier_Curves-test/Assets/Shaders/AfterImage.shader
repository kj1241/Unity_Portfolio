Shader "Custom/AfterImage"
{
    Properties
    {
        //_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "white" {} //노말맵 추가
        _NoiseTex("Noise", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"= "Transparent" } // 투명하게 처리하기위해서
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert Lambert alpha:fade//알파값 추가
        #pragma enable_d3d11_debug_symbols //다이렉트X12 셰이더 디버깅을 하려면 무조건 PIX에서 디버깅밖에 못하니 선언해줘야함
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap; // 노말맵
        sampler2D _NoiseTex; //노이즈 함수

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 viewDir;
        };

        half _Glossiness;
        half _Metallic;
        //fixed4 _Color;
        fixed4 _TrailDir; //자취 방향 이건 스크립트에서 궤도를 받을꺼임
        fixed4 _Trail; //완성된 트레일
        float _Weight;
        float _DotTrail;
      

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        //이게 표면셰이더 빛의 값을 영향 받는것
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap- float2(0.0f, _Time.y))); //노말 맵 추가 열기 주는 느낌 추가
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex );
            
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            float fRim = dot(IN.viewDir, o.Normal);
            float reusltRim = saturate(pow(1 - fRim * 0.35, 3)); // 투명함을 위해 위해 프리넬 반사 구현
            float tempAlpha = saturate((c.r + c.g + c.b)*0.5); // 검정색의 배경을 알파값으로 바꾸기위해서
            o.Alpha = (reusltRim + tempAlpha * 0.95) ;

        }

        //버텍스 셰이더
        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);

            _Weight = clamp(dot(v.normal, _TrailDir), 0, 1); //노멀백터와 0~ 90 차이면 그만큼 가중치 반환 (clamp 사이의 값 dot 백터 내적) 
            float noise = tex2Dlod(_NoiseTex, v.texcoord).r; //밉맵(노이즈 맵)을 얻은후 red float 값 추출
            _Trail = _TrailDir * _Weight* noise; // 자취방향 백터를 가중치만큼 곱함
            v.vertex.xyz = float3(v.vertex.x + _Trail.x, v.vertex.y + _Trail.y, v.vertex.z + _Trail.z); //그만큼 본래 백터에 더해줌
            v.texcoord.y -= _Time.x*2;
            v.texcoord.x = fmod(v.texcoord.x *2 + 0.1, 1);
        }

        ENDCG
    }
    FallBack "Diffuse"
}
