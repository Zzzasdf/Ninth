Shader "Custom/Back"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        
        Tags{"Queue" = "Transparent"}
        
        Pass
        {
            Color (0,1,0,1)
        }
    }
}
