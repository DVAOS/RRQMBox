using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMBox.Server
{
   public static class Msg
    {
        public static Action<string> msgShow;

        public static void ShowMsg(string msg)
        {
            msgShow.Invoke(msg);
        }
    }
}
