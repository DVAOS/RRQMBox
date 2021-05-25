using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RRQMBox.Client.Common
{
    public class DialogResult
    {
        public Visibility Visibility { get; set; }
        public string Path { get; set; }
        public EventWaitHandle WaitHandle { get; set; }
    }
}
