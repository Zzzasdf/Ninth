Shader "Custom/Front"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Tags{"Queue" = "Overlay"}
        
        Pass
        {
            Color (1,0,0,1)
        }
    }
}
