//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  API首页：https://www.yuque.com/eo2w71/rrqm
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMCore;
using RRQMSocket;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Windows.Forms;

namespace EERPCServiceDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();
            this.Load += this.Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.tcpRpcParser = new TcpRpcParser();
            this.tcpRpcParser.Connected += this.TcpRpcParser_Connected;
            this.tcpRpcParser.Disconnected += this.TcpRpcParser_Disconnected;

            var config = new RRQMConfig();
            config.SetListenIPHosts(new RRQMSocket.IPHost[] { new RRQMSocket.IPHost(7789) })
                .SetClearInterval(-1);

            this.tcpRpcParser.Setup(config).Start();
            this.ShowMsg("服务器已启动");
        }

        private void TcpRpcParser_Disconnected(RpcSocketClient client, RRQMSocket.MesEventArgs e)
        {
            lock (this)
            {
                this.listBox2.Items.Remove(client.ID);
            }
            
        }

        private void TcpRpcParser_Connected(RpcSocketClient client, RRQMEventArgs e)
        {
            this.listBox2.Items.Add(client.ID);
        }

        public void ShowMsg(string msg)
        {
            this.Invoke((Action)(delegate () { this.textBox1.AppendText(msg + "\r\n"); }));
        }

        TcpRpcParser tcpRpcParser;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                AccessType accessType = AccessType.Owner;
                if (this.checkBox1.Checked)
                {
                    accessType = accessType | AccessType.Owner;
                }
                if (this.checkBox2.Checked)
                {
                    accessType = accessType | AccessType.Service;
                }
                if (this.checkBox3.Checked)
                {
                    accessType = accessType | AccessType.Everyone;
                }
                if (this.checkBox4.Checked)
                {
                    this.tcpRpcParser.PublishEvent(this.textBox2.Text, accessType);
                    this.ShowMsg("发布成功");
                }
                else if (this.listBox2.SelectedItem is string id)
                {
                    if (this.tcpRpcParser.TryGetSocketClient(id, out RpcSocketClient socketClient))
                    {
                        socketClient.PublishEvent(this.textBox2.Text, accessType);
                        this.ShowMsg("发布成功");
                    }
                    else
                    {
                        this.ShowMsg("没有找到对应客户端");
                    }
                }
                else
                {
                    this.ShowMsg("请选择一个客户端ID");
                }
            }
            catch (Exception ex)
            {
                this.ShowMsg(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] events = this.tcpRpcParser.GetAllEvents();

            this.listBox1.Items.Clear();
            this.listBox1.Items.AddRange(events);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.checkBox4.Checked)
                {
                    this.tcpRpcParser.SubscribeEvent<string>(this.textBox3.Text, this.SubscribeEvent);
                    this.ShowMsg("订阅成功");
                }
                else if (this.listBox2.SelectedItem is string id)
                {
                    if (this.tcpRpcParser.TryGetSocketClient(id, out RpcSocketClient socketClient))
                    {
                        socketClient.SubscribeEvent<string>(this.textBox3.Text, this.SubscribeEvent);
                        this.ShowMsg("订阅成功");
                    }
                    else
                    {
                        this.ShowMsg("没有找到对应客户端");
                    }
                }
                else
                {
                    this.ShowMsg("请选择一个客户端ID");
                }
            }
            catch (Exception ex)
            {
                this.ShowMsg(ex.Message);
            }

        }

        private void SubscribeEvent(EventSender eventSender, string arg)
        {
            this.ShowMsg($"从{eventSender.RaiseSourceType}收到通知事件{eventSender.EventName}，信息：{arg}");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBox1.SelectedItem is string eventName)
                {
                    if (this.checkBox4.Checked)
                    {
                        this.tcpRpcParser.RaiseEvent(eventName, this.textBox4.Text);
                        this.ShowMsg("触发成功");
                    }
                    else if (this.listBox2.SelectedItem is string id)
                    {
                        if (this.tcpRpcParser.TryGetSocketClient(id, out RpcSocketClient socketClient))
                        {
                            socketClient.RaiseEvent(eventName, this.textBox4.Text);
                            this.ShowMsg("触发成功");
                        }
                        else
                        {
                            this.ShowMsg("没有找到对应客户端");
                        }
                    }
                    else
                    {
                        this.ShowMsg("请选择一个客户端ID");
                    }
                }
                else
                {
                    this.ShowMsg("请先选择事件");
                }
            }
            catch (Exception ex)
            {
                this.ShowMsg(ex.Message);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.checkBox4.Checked)
                {
                    this.tcpRpcParser.UnsubscribeEvent(this.textBox3.Text);
                    this.ShowMsg("取消订阅成功");
                }
                else if (this.listBox2.SelectedItem is string id)
                {
                    if (this.tcpRpcParser.TryGetSocketClient(id, out RpcSocketClient socketClient))
                    {
                        socketClient.UnsubscribeEvent(this.textBox3.Text);
                        this.ShowMsg("取消订阅成功");
                    }
                    else
                    {
                        this.ShowMsg("没有找到对应客户端");
                    }
                }
                else
                {
                    this.ShowMsg("请选择一个客户端ID");
                }
            }
            catch (Exception ex)
            {
                this.ShowMsg(ex.Message);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.checkBox4.Checked)
                {
                    this.tcpRpcParser.UnpublishEvent(this.textBox2.Text);
                    this.ShowMsg("取消发布成功");
                }
                else if (this.listBox2.SelectedItem is string id)
                {
                    if (this.tcpRpcParser.TryGetSocketClient(id, out RpcSocketClient socketClient))
                    {
                        socketClient.UnpublishEvent(this.textBox2.Text);
                        this.ShowMsg("取消发布成功");
                    }
                    else
                    {
                        this.ShowMsg("没有找到对应客户端");
                    }
                }
                else
                {
                    this.ShowMsg("请选择一个客户端ID");
                }
            }
            catch (Exception ex)
            {
                this.ShowMsg(ex.Message);
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.textBox1.Clear();
        }
    }
}
