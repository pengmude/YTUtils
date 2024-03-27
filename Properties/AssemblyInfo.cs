using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 有关程序集的一般信息由以下
// 控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("YTUtils工具集")]
[assembly: AssemblyDescription("这是常用的工具类库，包含Json配置文件读写模块，日志重定向到文件模块，松下PLC串口通讯模块，图片压缩模块等")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("东莞云田视觉科技有限公司")]
[assembly: AssemblyProduct("YTUtils")]
[assembly: AssemblyCopyright("Copyright ©  2024")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 会使此程序集中的类型
//对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型
//请将此类型的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("21d5316b-cc6b-4137-8f38-0f15b1589da7")]

// 程序集的版本信息由下列四个值组成: 
//
//      主版本
//      次版本
//      生成号
//      修订号
//
//可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值
//通过使用 "*"，如下所示:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.2.0.0")]
[assembly: AssemblyFileVersion("1.2.0.0")]
// 日志配置文件路径
[assembly: log4net.Config.XmlConfigurator(Watch = true, ConfigFile = @"Config\log4net.config")]
