using EleCho.GoCqHttpSdk;
using MiraiGo_C;
using System.Xml;

namespace MiraiGoC
{
    class Program
    {
        static public char g = Path.DirectorySeparatorChar;
        static public object o = new object();
        static void Main() => new Program().MainAsync();
        
        private async void MainAsync()
        {
            Console.WriteLine("   ____       _ ____        __ ");
            Console.WriteLine("  / __ \\_____(_) __ )____  / /_");
            Console.WriteLine(" / / / / ___/ / __  / __ \\/ __/");
            Console.WriteLine("/ /_/ / /  / / /_/ / /_/ / /_  ");
            Console.WriteLine("\\____/_/  /_/_____/\\____/\\__/  \n");
            Console.WriteLine("Version: 1.0\n");

            try
            {
                //XmlDocument读取xml文件
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("settings.xml");
                //获取xml根节点
                XmlNode xmlRoot = xmlDoc.DocumentElement;
                //根据节点顺序逐步读取
                //读取第一个name节点

                string a = xmlRoot.SelectSingleNode("HOST").InnerText;
                string q = xmlRoot.SelectSingleNode("PORT").InnerText;

                string _ws = $"ws://{a}:{q}";

                Util.Print("正在连接至 "+_ws);

                CqWsSession session = new CqWsSession(new CqWsSessionOptions()
                {
                    BaseUri = new Uri(_ws),
                    UseApiEndPoint = true,
                    UseEventEndPoint = true,
                });

                session.StartAsync();
                Util.Print("Bot启动完成！");

                Util.Print("正在加载插件...");
                PluginLoader.LoadPlugins(session);

                Util.Print("正在启动信息处理器...");
                session.UsePlugin(new Receiver());

                Util.Print("准备就绪");
                while (true)
                {
                    string cmd = Console.ReadLine();
                    string[] cargs = cmd.Split(" ");
                    lock (o)
                    {
                        if (cargs[0] == "help")
                        {
                            Util.Print("114514!");
                        }
                        else if (cargs[0] == "exit")
                        {
                            Util.Print("感谢使用！");
                            break;
                        }
                        else
                            Util.Print("无效的命令. 要获取帮助请输入help.", Util.PrintType.INFO, ConsoleColor.Red);
                    }
                }
            }
            catch(Exception ex)
            {
                Util.Print("连接至Bot时失败："+ex.Message, Util.PrintType.ERROR);
                Console.WriteLine("按下 回车键 退出程序...");
                Console.ReadLine();
            }
        }
    }
}