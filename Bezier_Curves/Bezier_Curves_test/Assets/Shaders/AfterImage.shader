Shader "Custom/AfterImage"
{
    Properties
    {
        //_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "white" {} //�븻�� �߰�
        _NoiseTex("Noise", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"= "Transparent" } // �����ϰ� ó���ϱ����ؼ�
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert Lambert alpha:fade//���İ� �߰�
        #pragma enable_d3d11_debug_symbols //���̷�ƮX12 ���̴� ������� �Ϸ��� ������ PIX���� �����ۿ� ���ϴ� �����������
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap; // �븻��
        sampler2D _NoiseTex; //������ �Լ�

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 viewDir;
        };

        half _Glossiness;
        half _Metallic;
        //fixed4 _Color;
        fixed4 _TrailDir; //���� ���� �̰� ��ũ��Ʈ���� �˵��� ��������
        fixed4 _Trail; //�ϼ��� Ʈ����
        float _Weight;
        float _DotTrail;
      

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        //�̰� ǥ����̴� ���� ���� ���� �޴°�
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap- float2(0.0f, _Time.y))); //�븻 �� �߰� ���� �ִ� ���� �߰�
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex );
            
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            float fRim = dot(IN.viewDir, o.Normal);
            float reusltRim = saturate(pow(1 - fRim * 0.35, 3)); // �������� ���� ���� ������ �ݻ� ����
            float tempAlpha = saturate((c.r + c.g + c.b)*0.5); // �������� ����� ���İ����� �ٲٱ����ؼ�
            o.Alpha = (reusltRim + tempAlpha * 0.95) ;

        }

        //���ؽ� ���̴�
        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);

            _Weight = clamp(dot(v.normal, _TrailDir), 0, 1); //��ֹ��Ϳ� 0~ 90 ���̸� �׸�ŭ ����ġ ��ȯ (clamp ������ �� dot ���� ����) 
            float noise = tex2Dlod(_NoiseTex, v.texcoord).r; //�Ӹ�(������ ��)�� ������ red float �� ����
            _Trail = _TrailDir * _Weight* noise; // ������� ���͸� ����ġ��ŭ ����
            v.vertex.xyz = float3(v.vertex.x + _Trail.x, v.vertex.y + _Trail.y, v.vertex.z + _Trail.z); //�׸�ŭ ���� ���Ϳ� ������
            v.texcoord.y -= _Time.x*2;
            v.texcoord.x = fmod(v.texcoord.x *2 + 0.1, 1);
        }

        ENDCG
    }
    FallBack "Diffuse"
}
