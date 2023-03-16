using System;
using System.Windows.Forms;

namespace Glossary_Winform
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainGlossaries());
        }

        public static string LastSelectedList { get; set; }
    }
}
