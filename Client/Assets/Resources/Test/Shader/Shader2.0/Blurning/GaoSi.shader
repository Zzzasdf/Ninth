Shader "Custom/GaoSi"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Amblent("Amblent", float) = 0.001
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
            float _Amblent;

            fixed4 frag (v2f i) : SV_Target
            {
                float tmpUV = i.uv;
                
                fixed4 col = tex2D(_MainTex, tmpUV);
                // just invert the colors
                // col.rgb = 1 - col.rgb;

                fixed4 col2 = tex2D(_MainTex, tmpUV + float2(-_Amblent, 0));
                fixed4 col3 = tex2D(_MainTex, tmpUV + float2(0, -_Amblent));
                fixed4 col4 = tex2D(_MainTex, tmpUV + float2(_Amblent, 0));
                fixed4 col5 = tex2D(_MainTex, tmpUV + float2(0, _Amblent));

                col = (col + col2 + col3 + col4 + col5) / 5.0;
                
                return col;
            }
            ENDCG
        }
    }
}
