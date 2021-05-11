//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  源代码仓库：https://gitee.com/RRQM_Home
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using System;
using System.Timers;
using System.Windows;

namespace Demo.ServiceGUI.Views
{
    /// <summary>
    /// MessageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageWindow : Window
    {
        public MessageWindow(string mes)
        {
            InitializeComponent();
            this.MesBox.Text = mes;
            timer = new Timer(10);
            timer.Elapsed += Timer_Elapsed;
        }

        private Timer timer;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Enabled = true;
        }

        private int i;

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                MainWindow.Window.Dispatcher.Invoke(new Action(() =>
                {
                    if (i > 100)
                    {
                        this.Top -= 6;
                    }

                    i++;
                    this.Opacity -= 0.005;
                    if (i > 150)
                    {
                        this.Close();
                    }
                }));
            }
            catch (Exception)
            {
            }
        }
    }
}