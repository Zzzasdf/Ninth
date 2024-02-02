1、热更方案: Hybirdclr

2、ioc框架插件：zenject

3、轻量级异步插件：UniTask

4、全局配置
	1 => 已启用Nullable，C#8.0有但默认不开启，C#10默认开启。文件中使用 #nullable enable 开启，同步上下文
		配置文件：Asset/../Directory.Build.props
		eg! Unity项目的.csproj在每次启动Unity时，都会重新生成一个，
			所以无法将<Nullable>enable<Nullable>配置在项目的.csproj里，
			所以采用新建一个文件Directory.Build.props

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
