Shader "Custom/SetTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlendTex ("Texture", 2D) = "white" {}
        _ConsColor("Color", Color) = (1,0,0,1)
    }
    SubShader
    {
        Pass
        {
//            Color (1,0,0,1)
//            SetTexture[_MainTex]
//            {
//                Combine Primary * Texture
//            }
            
//            SetTexture[_MainTex]
//            {
//                Combine Texture
//            }
//            
//            SetTexture[_BlendTex]
//            {
//                Combine Previous * Texture
//            }
            
//            SetTexture[_BlendTex]
//            {
//                Combine Previous Lerp(Previous) Texture
//            }
            
            SetTexture[_BlendTex]
            {
                ConstantColor[_ConsColor]
                Combine Texture * Constant
            }
        }
    }
}
