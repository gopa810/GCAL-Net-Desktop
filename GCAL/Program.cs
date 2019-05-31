using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GCAL.Base;

namespace GCAL
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
            GCGlobal.LoadInstanceData();
            Application.Run(new FrameMain());
            GCGlobal.SaveInstanceData();
        }
    }
}
