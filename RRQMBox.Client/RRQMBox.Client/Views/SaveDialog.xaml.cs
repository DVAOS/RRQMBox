//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using Microsoft.Win32;
using RRQMBox.Client.Common;
using System.Windows;
using System.Windows.Controls;

namespace RRQMBox.Client.Views
{
    /// <summary>
    /// SaveDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SaveDialog : UserControl
    {
        public SaveDialog()
        {
            InitializeComponent();
        }

        public DialogResult DialogResult
        {
            get { return (DialogResult)GetValue(DialogResultProperty); }
            set { SetValue(DialogResultProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DialogResult.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.Register("DialogResult", typeof(DialogResult), typeof(SaveDialog), new PropertyMetadata(null, OnResultChanged));

        private static void OnResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SaveDialog saveDialog = (SaveDialog)d;
            if (saveDialog.DialogResult != null)
            {
                saveDialog.Visibility = saveDialog.DialogResult.Visibility;
                saveDialog.pathBox.Text = saveDialog.DialogResult.Path;
            }
        }

        private void CorrugatedButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DialogResult != null)
            {
                this.Visibility = Visibility.Hidden;
                this.DialogResult.WaitHandle.Set();
            }
        }

        private void pathBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.DialogResult.Path = ((TextBox)sender).Text;
        }

        private void CorrugatedButton_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();

            fileDialog.Filter = "所有文件|*.*";
            fileDialog.FileName = this.DialogResult.Path;
            fileDialog.ShowDialog();

            if (fileDialog.FileName != null && fileDialog.FileName.Length > 0)
            {
                this.pathBox.Text = fileDialog.FileName;
            }
        }
    }
}