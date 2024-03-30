Shader "Custom/billboardY"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
         
            float4x4 GetRotationMatrix(float xRadian, float yRadian, float zRadian)
            {
                float sina, cosa;
                sincos(xRadian, sina, cosa);

                float4x4 xMatrix;

    

                xMatrix[0] = float4(1, 0, 0, 0);
                xMatrix[1] = float4(0, cosa, -sina, 0);
                xMatrix[2] = float4(0, sina, cosa, 0);
                xMatrix[3] = float4(0, 0, 0, 1);

                sincos(yRadian, sina, cosa);

                float4x4 yMatrix;


                yMatrix[0] = float4(cosa, 0, sina, 0);
                yMatrix[1] = float4(0, 1, 0, 0);
                yMatrix[2] = float4(-sina, 0, cosa, 0);
                yMatrix[3] = float4(0, 0, 0, 1);

                sincos(zRadian, sina, cosa);

                float4x4 zMatrix;


                zMatrix[0] = float4(cosa, -sina, 0, 0);
                zMatrix[1] = float4(sina, cosa, 0, 0);
                zMatrix[2] = float4(0, 0, 1, 0);
                zMatrix[3] = float4(0, 0, 0, 1);

                return mul(mul(yMatrix, xMatrix), zMatrix);
            }

            v2f vert(appdata v)
            {
                v2f o;

                float4x4 scaleMatrix; // Scale 행렬

                vector sx = vector(unity_ObjectToWorld._m00, unity_ObjectToWorld._m10, unity_ObjectToWorld._m20, 0);
                vector sy = vector(unity_ObjectToWorld._m01, unity_ObjectToWorld._m11, unity_ObjectToWorld._m21, 0);
                vector sz = vector(unity_ObjectToWorld._m02, unity_ObjectToWorld._m12, unity_ObjectToWorld._m22, 0);

                float scaleX = length(sx);
                float scaleY = length(sy);
                float scaleZ = length(sz);

                scaleMatrix[0] = float4(scaleX, 0, 0, 0);
                scaleMatrix[1] = float4(0, scaleY, 0, 0);
                scaleMatrix[2] = float4(0, 0, scaleZ, 0);
                scaleMatrix[3] = float4(0, 0, 0, 1);

                float4 pos = v.vertex;

                float4 worldPos = float4(mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz, 0);
                float4 cameraPos = float4(_WorldSpaceCameraPos.xyz, 0);

                vector cameraDir = cameraPos - worldPos;

                float xAngle = atan2(cameraDir.z, cameraDir.y);
                float yAngle = atan2(cameraDir.z, cameraDir.x);
                float zAngle = atan2(cameraDir.x, cameraDir.y);

                xAngle = -radians(0);
                yAngle = -(radians(270) + yAngle);
                zAngle = 0;

                float4x4 rotationMatrix = GetRotationMatrix(xAngle, yAngle, 0);

                float4x4 moveMatrix;
                moveMatrix[0] = float4(1, 0, 0, unity_ObjectToWorld._m03);
                moveMatrix[1] = float4(0, 1, 0, unity_ObjectToWorld._m13);
                moveMatrix[2] = float4(0, 0, 1, unity_ObjectToWorld._m23);
                moveMatrix[3] = float4(0, 0, 0, 1);

                float4x4 transformMatrix = mul(mul(moveMatrix, rotationMatrix), scaleMatrix);

                pos = mul(transformMatrix, pos);

                o.pos = mul(UNITY_MATRIX_VP, pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
