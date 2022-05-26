using RRQMCore.ByteManager;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServiceApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void ShowMsg(string msg)
        {
            this.textBox1.AppendText(msg);
            this.textBox1.AppendText("\r\n");
        }

        TcpService service = new TcpService();

        private void button1_Click(object sender, EventArgs ergs)
        {
            service.Connecting += (client, e) => { };//有客户端正在连接
            service.Connected += (client, e) => { };//有客户端连接
            service.Disconnected += (client, e) => { };//有客户端断开连接
            service.Received += this.Service_Received;

            service.Setup(new RRQMConfig()//载入配置     
                .SetListenIPHosts(new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) })//同时监听两个地址
                .SetMaxCount(10000)
                .SetThreadCount(10))
                .Start();//启动

        }

        private void Service_Received(SocketClient client, ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            this.ShowMsg($"从客户端id={client.ID}，ip={client.IP}，port={client.Port}收到消息：{Encoding.UTF8.GetString(byteBlock.ToArray())}"); 
        }
    }
}
