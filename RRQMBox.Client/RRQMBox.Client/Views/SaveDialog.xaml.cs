using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using RRQMBox.Client.Common;

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
