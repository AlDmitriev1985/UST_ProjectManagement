using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UST_ProjectManagement
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            foreach (string s in args)
            {
                MainForm.request += s;
            }


            Application.Run(new MainForm());

        }

        static string ProcessInput(string s)
        {
            // TODO Verify and validate the input 
            // string as appropriate for your application.
            return s;
        }
    }
}
