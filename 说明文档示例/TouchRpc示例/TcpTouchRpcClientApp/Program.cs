using RRQMCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TouchRpcClientApp
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Enterprise.ForTest();
            }
            catch (Exception)
            {
                Console.WriteLine($"HttpTouchRpcServiceΪ��ҵ�湦�ܣ�������Ҫ������ҵ�湦�ܡ�");
            }

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
