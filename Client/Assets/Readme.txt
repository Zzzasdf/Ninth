1、热更方案: Hybirdclr

2、DI插件：VContainer

3、轻量级异步插件：UniTask

4、全局配置
	1 => 已启用 Nullable，C#8.0 有但默认不开启，C#10 默认开启。文件中使用 #nullable enable 开启，同步上下文
		配置文件：Asset/../Directory.Build.props
		eg! Unity项目的 .csproj 在每次启动 Unity 时，都会重新生成一个，
			所以无法将 <Nullable>enable<Nullable> 配置在项目的 .csproj 里，
			所以采用新建一个文件 Directory.Build.props

5、LitJson
   eg!! 该类的所有的字段一定要给它设置 Set 权限, 2024/2/25 警钟敲烂！！！！！！！！！！！！！！！！！！

6、TextMeshPro
    HanZiZhiMeiFangSongGBK
    Default: Static
    Fallback: Dynamic
    
7、Shader
    渲染管线：顶点着色器 -> 光栅化 -> 片段着色器 -> alpha 测试 -> 模板测试 -> 深度测试 -> Blend -> GBuffer -> front buffer -> frame buffer -> 显示
    Shader 1.0 适用所有显卡
    Shader 2.0 可以实现编程
   1、顶点着色器
        1、计算顶点颜色
        2、计算灯光设置
            Ambient * Lighting Window's Ambient Intensity setting + (Light Color * Diffuse + Light Color * Specular) + Emission
   2、片段着色器
        1、纹理采样
     
   
   
   
>> TODO
Thread
// UI
// protobuf
// ..
Task
UniTask
IOC/DI zenject
Regular
show ScriptableObject in Inspector
Attribute

// 音乐配置
// => 短音效
//   =>使用 PCM 或 ADPCM (可容噪音，如脚步声 / 爆炸 / 武器)
// => 长音效
//	 => 使用 Vorbis, 质量无需100%
// => 不常出现的音乐 / 音效
// 	 => 使用 CompressedInMemory, 或 Streaming (节省内存)
