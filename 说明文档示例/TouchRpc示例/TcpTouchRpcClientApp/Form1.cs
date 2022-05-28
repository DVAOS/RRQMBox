using RRQMProxy;
using RRQMSocket;
using RRQMSocket.RPC.TouchRpc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TouchRpcClientApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TcpTouchRpcClient client = new TcpTouchRpcClient();
            client.Setup(new RRQMConfig()
                .SetRemoteIPHost("127.0.0.1:7789")
                .SetVerifyToken("TouchRpc"));
            client.Connect();

            //直接调用时，第一个参数为服务名+方法名（必须全小写）
            //第二个参数为调用配置参数，可设置调用超时时间，取消调用等功能。
            //后续参数为调用参数。
            bool result = client.Invoke<bool>("myrpcserver/login", InvokeOption.WaitInvoke, textBox1.Text,textBox2.Text);
            MessageBox.Show(result.ToString());

            client.SafeDisposeWithNull();//client是长连接，可以复用，但在此处使用短连接。
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TcpTouchRpcClient client = new TcpTouchRpcClient();
            client.Setup(new RRQMConfig()
                .SetRemoteIPHost("127.0.0.1:7789")
                .SetVerifyToken("TouchRpc"));
            client.Connect();

            MyRpcServer myRpcServer = new MyRpcServer(client);//MyRpcServer类是由代码工具生成的类。

            //代理调用时，基本和本地调用一样。只是会多一个调用配置参数。
            bool result = myRpcServer.Login(textBox1.Text, textBox2.Text, InvokeOption.WaitInvoke);
            MessageBox.Show(result.ToString());

            client.SafeDisposeWithNull();//client是长连接，可以复用，但在此处使用短连接。
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TcpTouchRpcClient client = new TcpTouchRpcClient();
            client.Setup(new RRQMConfig()
                .SetRemoteIPHost("127.0.0.1:7789")
                .SetVerifyToken("TouchRpc"));
            client.Connect();

            //扩展调用时，首先要保证本地已有代理文件，然后调用和和本地调用一样。只是会多一个调用配置参数。
            bool result = client.Login(textBox1.Text, textBox2.Text, InvokeOption.WaitInvoke);
            MessageBox.Show(result.ToString());

            client.SafeDisposeWithNull();//client是长连接，可以复用，但在此处使用短连接。
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UdpTouchRpc client = new UdpTouchRpc();
            client.Setup(new RRQMConfig()
                .SetBindIPHost(7794)
                .SetRemoteIPHost("127.0.0.1:7791"));//设置目标地址。
            client.Start();

            bool result = client.Invoke<bool>("myrpcserver/login", InvokeOption.WaitInvoke, textBox1.Text, textBox2.Text);
            MessageBox.Show(result.ToString());

            client.SafeDisposeWithNull();//client可以复用，但在此处直接释放。
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UdpTouchRpc client = new UdpTouchRpc();
            client.Setup(new RRQMConfig()
                .SetBindIPHost(7794)
                .SetRemoteIPHost("127.0.0.1:7791"));//设置目标地址。
            client.Start();

            MyRpcServer myRpcServer = new MyRpcServer(client);//MyRpcServer类是由代码工具生成的类。

            //代理调用时，基本和本地调用一样。只是会多一个调用配置参数。
            bool result = myRpcServer.Login(textBox1.Text, textBox2.Text, InvokeOption.WaitInvoke);
            MessageBox.Show(result.ToString());

            client.SafeDisposeWithNull();//client可以复用，但在此处直接释放。
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UdpTouchRpc client = new UdpTouchRpc();
            client.Setup(new RRQMConfig()
                .SetBindIPHost(7794)
                .SetRemoteIPHost("127.0.0.1:7791"));//设置目标地址。
            client.Start();

            //扩展调用时，首先要保证本地已有代理文件，然后调用和和本地调用一样。只是会多一个调用配置参数。
            bool result = client.Login(textBox1.Text, textBox2.Text, InvokeOption.WaitInvoke);
            MessageBox.Show(result.ToString());

            client.SafeDisposeWithNull();//client可以复用，但在此处直接释放。
        }

        private void button7_Click(object sender, EventArgs e)
        {
            HttpTouchRpcClient client = new HttpTouchRpcClient();
            client.Setup(new RRQMConfig()
               .SetRemoteIPHost("127.0.0.1:7790")
               .SetVerifyToken("TouchRpc"));
            client.Connect();
            //直接调用时，第一个参数为服务名+方法名（必须全小写）
            //第二个参数为调用配置参数，可设置调用超时时间，取消调用等功能。
            //后续参数为调用参数。
            bool result = client.Invoke<bool>("myrpcserver/login", InvokeOption.WaitInvoke, textBox1.Text, textBox2.Text);
            MessageBox.Show(result.ToString());

            client.SafeDisposeWithNull();//client是长连接，可以复用，但在此处使用短连接。
        }

        private void button9_Click(object sender, EventArgs e)
        {
            HttpTouchRpcClient client = new HttpTouchRpcClient();
            client.Setup(new RRQMConfig()
                .SetRemoteIPHost("127.0.0.1:7790")
                .SetVerifyToken("TouchRpc"));
            client.Connect();
            MyRpcServer myRpcServer = new MyRpcServer(client);//MyRpcServer类是由代码工具生成的类。

            //代理调用时，基本和本地调用一样。只是会多一个调用配置参数。
            bool result = myRpcServer.Login(textBox1.Text, textBox2.Text, InvokeOption.WaitInvoke);
            MessageBox.Show(result.ToString());

            client.SafeDisposeWithNull();//client是长连接，可以复用，但在此处使用短连接。
        }

        private void button8_Click(object sender, EventArgs e)
        {
            HttpTouchRpcClient client = new HttpTouchRpcClient();
            client.Setup(new RRQMConfig()
               .SetRemoteIPHost("127.0.0.1:7790")
               .SetVerifyToken("TouchRpc"));
            client.Connect();
            //扩展调用时，首先要保证本地已有代理文件，然后调用和和本地调用一样。只是会多一个调用配置参数。
            bool result = client.Login(textBox1.Text, textBox2.Text, InvokeOption.WaitInvoke);
            MessageBox.Show(result.ToString());

            client.SafeDisposeWithNull();//client是长连接，可以复用，但在此处使用短连接。
        }
    }
}
