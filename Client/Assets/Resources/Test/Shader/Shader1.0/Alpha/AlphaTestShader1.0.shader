Shader "C/AlphustomaTest1.0"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AlphaCut("Alpha cut", float) = 0.5
    }
    SubShader
    {
        AlphaTest Greater[_AlphaCut]
        
        Pass
        {
            SetTexture[_MainTex]
            {
                Combine Texture
            }
        }
    }
}
