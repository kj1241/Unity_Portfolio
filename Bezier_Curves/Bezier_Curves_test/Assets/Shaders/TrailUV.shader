Shader "Custom/TrailUV"
{
    Properties
    {
        
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _MainTexAbsFade("MainTex Abs Fade", Range(0, 1)) = 0
        _MainTexAbsFadePow("MainTex Abs Fade Pow", Float) = 1
        _MainTexAPow("MainTex AlphaGamma", Float) = 1
        _MainTexMultiplier("Main Texture Multiplier", Float) = 1
        _SoftAlpha("Soft Alpha", Range(0, 1)) = 1
        _TintTex("Tint Texture (RGB)", 2D) = "white" {}
        _MainTrailSpeedU("Main Traill U Speed", Float) = 10
        _MainTrailSpeedV("Main Trail V Speed", Float) = 0
        _MainTrailDistantce("Main Trail Distantce" , Float) =0.2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"  "IgnoreProjector" = "True" }
        Blend SrcAlpha OneMinusSrcAlpha //알파체널 있게 만드는거 맞고
        ZWrite Off
        
        Pass //지오 메트릭스가 한번만 렌더링
        {
        CGPROGRAM
            
            //#pragma surface surf Standard fullforwardshadows //표면쉐이더 쓸꺼아니니깐
            #pragma vertex vert //버텍스 쉐이더
            #pragma fragment frag //프레그먼트
            //#pragma enable_d3d11_debug_symbols
            #define TRANSFORM_TEX(tex,name) (tex.xy * name##_ST.xy + name##_ST.zw) // unity cg에 상응하는것은 없어서 정의 hlsl 문법인데 튜토리얼에서는 어떻게 정의 되있는지 모르겠음
            #pragma target 3.0


            //directx에서는 분명 구조체 선언할때 256의 배수에 따라서 페딩값 마춰줘야 최적화 되는데 유니티는 모르겠음.
            //시간날때 최적화 부분 봐야할듯
            struct appdata //속성
            {
                float4 vertex : POSITION; //위치
                float2 uv : TEXCOORD0;  //uv
                half4 color : COLOR;    //색
            };

            struct v2f //변화
            {
                float4 vertex : SV_POSITION; //위치
                float2 uv : TEXCOORD0; //uv
                float2 uv1 : TEXCOORD1; //저장할 uv
                half4 color : COLOR;  //색
            };

            sampler2D _MainTex;
            sampler2D _TintTex;

            float4 _MainTex_ST;
            half _MainTexVFade;
            half _MainTexVFadePow;
            half _MainTexAPow;
            half _MainTexMultiplier;
            half _SoftAlpha;
            half _MainTrailSpeedU;
            half _MainTrailSpeedV;
            half _MainTrailDistantce;

            //버텍스 쉐이더->레스터라이즈
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // 버텍스 값 넣어주고
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);  //uv 
                o.uv.x -= frac(_Time.x * _MainTrailSpeedU)+ _MainTrailDistantce; //알파값 더해야함
                o.uv.y -= frac(_Time.x * _MainTrailSpeedV);
                o.uv1 = v.uv;
                o.color = v.color;
                return o;
            }

            //픽셀쉐이더 비트 색 넣기 전
            //일딴 택스쳐가 png 인데 알파채널 열려있음에도 불구하고 검정색으로 색칠되어서 검정인것 그러면 main 텍스쳐의 알파채널을 우선적으로 정의 해줘야됨
            half4 frag(v2f IN) : SV_Target //중정밀도로 사용할꺼니깐
            {
                half4 mainTex = tex2D(_MainTex, IN.uv);

                half absFade = 1 - abs(IN.uv1.y - 0.5) * 2; // 절대값 v 자 그래프를 만듬
                absFade = pow(abs(absFade), _MainTexVFadePow); // v의 계수를 조정하여 n차로 둥글게하거나 아니면 더욱 조절
                absFade = lerp(1, absFade, _MainTexVFade); //
                mainTex.rgb *= absFade; //세로축 움직임
                mainTex.rgb = pow(abs(mainTex.rgb), _MainTexAPow) * _MainTexMultiplier;
                
                half tempAlphaTex = IN.color.r; //넘어온 기본 택스처 알파값을 저장하기 위해서
                
                // 버택스에서 넘어온 택스쳐의 알파채널이 1임으로 알파채널부터
                mainTex.a = (mainTex.r + mainTex.g + mainTex.b) * 0.333; // 흰색 검정 밖에 없음으로 r=g=b=a 근데 안믿기 때문에 다더해서 1/3
                mainTex.a = pow(abs(mainTex.a), _MainTexAPow); // 거듭제곱을 해주는 이유 투명한 택스쳐를 어려장 더하여서 품질 올림
                half AlphaFactor = saturate((mainTex.a - (1 - tempAlphaTex)) / _SoftAlpha);  //품질 올린 알파 +버택스 알파 산술평군   saturate 0~1 사이 클램프값 반환 
                half alpha = mainTex.a * tempAlphaTex; //품질 올린 택스쳐에 버택스에서 넘어온 알파값을 곱해줌 기하평균 내려
                half alphaMix = lerp(alpha, AlphaFactor, tempAlphaTex); //선형보간 해줌
                
                half4 tintCol = tex2D(_TintTex, half2(alphaMix, 0.5)); //그라데이션 있는택스쳐 가저오기 
                
                half4 col;

                col.rgb = lerp(tintCol.rgb * mainTex.rgb, tintCol.rgb, IN.color.a); //색 선형보간
                col.rgb *= IN.color.rgb; //
                col.a = alphaMix;

                return col;
            }
            ENDCG
        }
    }
}
