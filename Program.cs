using EleCho.GoCqHttpSdk;
using MiraiGo_C;
using MiraiGoC;
using NullLib.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Xml;

namespace MiraiGoC
{
    class Program
    {
        static Program()
        {
            // 启用 ANSI 转义序列
            Util.EnableAnsiSequence();
        }

        static string AppConfigPath = "appconfig.json";
        static char g = Path.DirectorySeparatorChar;
        static object o = new object();

        public readonly static SimpleLogger Logger = new SimpleLogger();

        private static bool LoadConfig([NotNullWhen(true)] out AppConfig? config)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            if (!File.Exists(AppConfigPath))
            {
                Logger.LogError("未找到配置文件, 已创建. 请编辑后重启");
                using Stream wfs = File.Create(AppConfigPath);
                JsonSerializer.Serialize(wfs, new AppConfig(), options);

                config = null;
                return false;
            }

            using Stream rfs = File.OpenRead(AppConfigPath);
            config = JsonSerializer.Deserialize<AppConfig>(rfs, options);

            if (config == null)
            {
                Logger.LogError("配置文件解析失败, 请检查配置文件");
                return false;
            }

            return true;
        }

        static async Task Main()
        {
            Console.WriteLine("   ____       _ ____        __ ");
            Console.WriteLine("  / __ \\_____(_) __ )____  / /_");
            Console.WriteLine(" / / / / ___/ / __  / __ \\/ __/");
            Console.WriteLine("/ /_/ / /  / / /_/ / /_/ / /_  ");
            Console.WriteLine("\\____/_/  /_/_____/\\____/\\__/  \n");
            Console.WriteLine("Version: 1.0\n");

            try
            {
                if (!LoadConfig(out AppConfig? config))
                    return;

                string _ws
                    = $"ws://{config.Host}:{config.Port}";

                Logger.LogInfo($"正在连接至 {_ws}");

                CqWsSession session = new CqWsSession(new CqWsSessionOptions()
                {
                    BaseUri = new Uri(_ws),
                    UseApiEndPoint = true,
                    UseEventEndPoint = true,
                });

                await session.StartAsync();
                Logger.LogInfo("Bot启动完成！");

                Logger.LogInfo("正在加载插件...");
                PluginLoader.LoadPlugins(session);

                Logger.LogInfo("正在启动信息处理器...");
                session.UsePlugin(new Receiver());

                Logger.LogInfo("准备就绪");

                CommandObject<ManageCommands> cmdobj = new CommandObject<ManageCommands>();
                while (true)
                {
                    string? stdinput = Console.ReadLine();
                    if (stdinput == null)                        // 当没有可用的输入时, 停止程序
                        return;

                    // 执行指令 (解析是自动的)
                    if (!cmdobj.TryExecuteCommand(stdinput, out _))
                    {
                        Logger.LogError("无效的命令. 要获取帮助请输入help.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"连接至Bot时失败： {ex.Message}");
                Console.WriteLine("按下 回车键 退出程序...");

                // 只有按下回车键才会退出, 且按别的键不会在屏幕中显示
                while (Console.ReadKey(true).Key != ConsoleKey.Enter)
                {
                    // pass
                }
            }
        }
    }
}