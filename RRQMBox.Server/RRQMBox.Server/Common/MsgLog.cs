using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RRQMCore.Log;

namespace RRQMBox.Server.Common
{
    public class MsgLog : ILog
    {
        public MsgLog(Action<string> action)
        {
            this.action = action;
        }

        private Action<string> action;
        public void Debug(LogType logType, object source, string message, string stackTrace)
        {
            this.action.Invoke($"类型：{logType},消息：{message}");
        }

        public void Debug(LogType logType, object source, string message)
        {
            this.Debug(logType,source,message,null);
        }
    }
}
