Shader "Custom/OffsetRed"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            Offset 0, 0
            Color(1,0,0,1)
        }
    }
}
