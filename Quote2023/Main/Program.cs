﻿using System;
using System.Threading;
using System.Windows.Forms;

namespace Main
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

            Application.ThreadException += Application_ThreadException;

            Application.Run(new UI.frmMDI());

        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            if (e.Exception is System.Data.ConstraintException)
            {
                Thread thread = (Thread)sender;
                System.Data.ConstraintException e1 = (System.Data.ConstraintException)e.Exception;
                MessageBox.Show(e.Exception.ToString());
                int t = 0;
            }
            else
            {
                MessageBox.Show(e.Exception.ToString());
                Application.Exit();
            }
        }
    }
}