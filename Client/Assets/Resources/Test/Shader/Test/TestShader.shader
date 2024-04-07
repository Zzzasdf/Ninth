Shader "Custom/Test"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TestInt ("TestInt", Integer) = 1
        _TestFloat("TestFloat", Float) = 1.0
        _TestRange("TestRange", Range(1,5)) = 2
        _TestColor("TestColor", Color) = (1,1,1,1)
        _TestVector("TestVector", Vector) = (1,1,1,1)
        _TestCube("TestCub", Cube) = ""{}
        _Test3D("Texture3D", 3D) = ""{}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
