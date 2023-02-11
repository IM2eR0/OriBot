using EleCho.GoCqHttpSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MiraiGoC
{
    class PluginLoader
    {
        public static void LoadPlugins(CqWsSession _s)
        {
            if (!Directory.Exists($"plugins"))
            {
                Program.Logger.LogWarning("插件目录不存在, 创建中...");
                Directory.CreateDirectory($"plugins");
            }


            DirectoryInfo dir = new DirectoryInfo($"plugins");
            var fs = dir.GetFiles();

            foreach (FileInfo f in fs)
            {
                // 如果不是动态链接库, 则跳过这个文件
                if (!f.Extension.Equals(".dll", StringComparison.OrdinalIgnoreCase))
                    continue;

                try
                {
                    Program.Logger.LogInfo($"加载插件: {f.Name}");

                    // 利用反射来加载 C# DLL
                    var pluginAsm = Assembly.LoadFile(f.FullName);
                    Type[] types = pluginAsm.GetTypes();

                    foreach (Type type in types)
                    {
                        if (!typeof(CqPostPlugin).IsAssignableFrom(type) || !type.Name.Equals("PluginLoadEnter"))
                            continue;

                        try
                        {
                            object _obj = Activator.CreateInstance(type) ?? throw new Exception("无法创建类型实例");
                            MethodInfo mf = type.GetMethod("Load") ?? throw new Exception("无法获取插件加载方法");


                            // 入口：Load
                            mf.Invoke(_obj, new CqWsSession[] { _s });
                        }
                        catch (Exception ex)
                        {
                            Program.Logger.LogError(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.Logger.LogError(ex.Message);
                }
            }
        }
    }
}
