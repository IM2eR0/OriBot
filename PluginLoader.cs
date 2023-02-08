using EleCho.GoCqHttpSdk;
using MiraiGo_C.Plugins;
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
        static char g = Program.g;
        public static void LoadPlugins(CqWsSession _s)
        {
            if (!Directory.Exists($".{g}plugins"))
            {
                Util.Print("插件目录不存在，创建中...", Util.PrintType.WARNING);
                Directory.CreateDirectory($".{g}plugins");
            }
            else
            {
                DirectoryInfo dir = new DirectoryInfo($".{g}plugins");
                var fs = dir.GetFiles();

                foreach ( FileInfo f in fs )
                {
                    if(f.Extension == ".dll")
                    {
                        try
                        {
                            Util.Print(f.Name, Util.PrintType.LP);

                            // 利用反射来加载 C# DLL
                            var _a = Assembly.LoadFile(f.FullName);
                            Type[] types = _a.GetTypes();
                            
                            for(int i = 0; i < types.Length; i++)
                            {
                                if (types[i].Name == "PluginLoadEnter")
                                {
                                    try
                                    {
                                        object _obj = Activator.CreateInstance(types[i]);
                                        MethodInfo mf = types[i].GetMethod("Load");
                                        // 入口：Load
                                        mf.Invoke(_obj, new CqWsSession[] { _s });
                                    }catch(Exception ex)
                                    {
                                        Util.Print(ex.Message, Util.PrintType.ERROR);
                                    }
                                };
                            };
                        }
                        catch (Exception ex)
                        {
                            Util.Print(ex.Message, Util.PrintType.ERROR);
                        }
                    }
                }
            }
        }
    }
}
