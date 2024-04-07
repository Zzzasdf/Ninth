Shader "Custom/OffsetGreen"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            Offset 0, 1
            Color(0,1,0,1)
        }
    }
}
