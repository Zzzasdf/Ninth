Shader "Custom/TestVertex"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TestColor("TestColor", Color) = (1,0,0,1)
    }
    SubShader
    {
        // Shader1.0 支持
        // 计算顶点的颜色
        // 顶点变换 => 固定好，没法改变
        // 灯光的作用
        
        Pass
        {
            Color[_TestColor]
           
           Material
           {
               // Ambient * Lighting Window's Ambient Intensity setting + (Light Color * Diffuse + Light Color * Specular) + Emission
               Ambient [_TestColor]
               Diffuse [_TestColor]
               Specular [_TestColor]
               Emission [_TestColor]
           }
           Lighting On
           SeparateSpecular On
        }
    }
}
