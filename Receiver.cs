using EleCho.GoCqHttpSdk;
using EleCho.GoCqHttpSdk.Message;
using EleCho.GoCqHttpSdk.Post;
using MiraiGoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiGoC
{
    class Receiver : CqPostPlugin
    {
        public override async Task OnGroupMessageAsync(CqGroupMessagePostContext context)
        {
            if (context.Session is not ICqActionSession actionSession)
                return;

            string GroupID = context.GroupId.ToString();
            string SenderID = context.Sender.UserId.ToString();
            string SenderName = context.Sender.Nickname;
            string Message = context.RawMessage;

            //var _msg = "["+GroupID+"] "+SenderName+"("+SenderID+") > "+Message;

            Program.Logger.LogInfo($"群消息/{GroupID}: {Message}");
        }
    }
}
