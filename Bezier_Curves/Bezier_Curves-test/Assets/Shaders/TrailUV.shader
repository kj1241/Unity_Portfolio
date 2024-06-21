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
        Blend SrcAlpha OneMinusSrcAlpha //����ü�� �ְ� ����°� �°�
        ZWrite Off
        
        Pass //���� ��Ʈ������ �ѹ��� ������
        {
        CGPROGRAM
            
            //#pragma surface surf Standard fullforwardshadows //ǥ�齦�̴� �����ƴϴϱ�
            #pragma vertex vert //���ؽ� ���̴�
            #pragma fragment frag //�����׸�Ʈ
            //#pragma enable_d3d11_debug_symbols
            #define TRANSFORM_TEX(tex,name) (tex.xy * name##_ST.xy + name##_ST.zw) // unity cg�� �����ϴ°��� ��� ���� hlsl �����ε� Ʃ�丮�󿡼��� ��� ���� ���ִ��� �𸣰���
            #pragma target 3.0


            //directx������ �и� ����ü �����Ҷ� 256�� ����� ���� ����� ������� ����ȭ �Ǵµ� ����Ƽ�� �𸣰���.
            //�ð����� ����ȭ �κ� �����ҵ�
            struct appdata //�Ӽ�
            {
                float4 vertex : POSITION; //��ġ
                float2 uv : TEXCOORD0;  //uv
                half4 color : COLOR;    //��
            };

            struct v2f //��ȭ
            {
                float4 vertex : SV_POSITION; //��ġ
                float2 uv : TEXCOORD0; //uv
                float2 uv1 : TEXCOORD1; //������ uv
                half4 color : COLOR;  //��
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

            //���ؽ� ���̴�->�����Ͷ�����
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // ���ؽ� �� �־��ְ�
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);  //uv 
                o.uv.x -= frac(_Time.x * _MainTrailSpeedU)+ _MainTrailDistantce; //���İ� ���ؾ���
                o.uv.y -= frac(_Time.x * _MainTrailSpeedV);
                o.uv1 = v.uv;
                o.color = v.color;
                return o;
            }

            //�ȼ����̴� ��Ʈ �� �ֱ� ��
            //�ϵ� �ý��İ� png �ε� ����ä�� ������������ �ұ��ϰ� ���������� ��ĥ�Ǿ �����ΰ� �׷��� main �ؽ����� ����ä���� �켱������ ���� ����ߵ�
            half4 frag(v2f IN) : SV_Target //�����е��� ����Ҳ��ϱ�
            {
                half4 mainTex = tex2D(_MainTex, IN.uv);

                half absFade = 1 - abs(IN.uv1.y - 0.5) * 2; // ���밪 v �� �׷����� ����
                absFade = pow(abs(absFade), _MainTexVFadePow); // v�� ����� �����Ͽ� n���� �ձ۰��ϰų� �ƴϸ� ���� ����
                absFade = lerp(1, absFade, _MainTexVFade); //
                mainTex.rgb *= absFade; //������ ������
                mainTex.rgb = pow(abs(mainTex.rgb), _MainTexAPow) * _MainTexMultiplier;
                
                half tempAlphaTex = IN.color.r; //�Ѿ�� �⺻ �ý�ó ���İ��� �����ϱ� ���ؼ�
                
                // ���ý����� �Ѿ�� �ý����� ����ä���� 1������ ����ä�κ���
                mainTex.a = (mainTex.r + mainTex.g + mainTex.b) * 0.333; // ��� ���� �ۿ� �������� r=g=b=a �ٵ� �ȹϱ� ������ �ٴ��ؼ� 1/3
                mainTex.a = pow(abs(mainTex.a), _MainTexAPow); // �ŵ������� ���ִ� ���� ������ �ý��ĸ� ����� ���Ͽ��� ǰ�� �ø�
                half AlphaFactor = saturate((mainTex.a - (1 - tempAlphaTex)) / _SoftAlpha);  //ǰ�� �ø� ���� +���ý� ���� �����   saturate 0~1 ���� Ŭ������ ��ȯ 
                half alpha = mainTex.a * tempAlphaTex; //ǰ�� �ø� �ý��Ŀ� ���ý����� �Ѿ�� ���İ��� ������ ������� ����
                half alphaMix = lerp(alpha, AlphaFactor, tempAlphaTex); //�������� ����
                
                half4 tintCol = tex2D(_TintTex, half2(alphaMix, 0.5)); //�׶��̼� �ִ��ý��� �������� 
                
                half4 col;

                col.rgb = lerp(tintCol.rgb * mainTex.rgb, tintCol.rgb, IN.color.a); //�� ��������
                col.rgb *= IN.color.rgb; //
                col.a = alphaMix;

                return col;
            }
            ENDCG
        }
    }
}
