using EleCho.GoCqHttpSdk;
using EleCho.GoCqHttpSdk.Message;
using NullLib.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiraiGo_C
{
    /// <summary>
    /// 管理指令
    /// </summary>
    internal class ManageCommands
    {
        public ManageCommands(CqWsSession session)
        {
            Session = session;
        }

        public CqWsSession Session { get; }

        [Command]
        public void Help()
        {
            Console.WriteLine($"OriBot v.{Assembly.GetExecutingAssembly().GetName().Version}");
            Console.WriteLine("114514!");

            Console.WriteLine(
            """
            OriBot 命令手册
            
              退出:
                Exit

              发送群消息:
                SendGroupMessage {群号} {消息内容}

              发送私聊消息;
                SendPrivateMessage {群号} {消息内容}
            """);
        }

        [Command]
        public void Exit()
        {
            Console.WriteLine("感谢使用!");
            Environment.Exit(0);
        }

        [Command]
        public void SendGroupMessage(long groupId, CqMessage message)
        {
            Session.SendGroupMessageAsync(groupId, message).Wait();
        }

        [Command]
        public void SendPrivateMessage(long friendId, CqMessage message)
        {
            Session.SendPrivateMessageAsync(friendId, message);
        }
    }
}
