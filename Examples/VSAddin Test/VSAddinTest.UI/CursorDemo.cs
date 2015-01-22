using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxprepAddinAPI;
using TxpAddinLibrary.Handlers;
using TxpAddinLibrary;

namespace VSAddinTest.UI
{
    public partial class CursorDemo : Form
    {
        public CursorDemo()
        {
            InitializeComponent();
        }

        IAppTaxApplicationService FApplication;

        private const int multiplier = 1000000;

        private void button1_Click(object sender, EventArgs e)
        {
            FApplication.StartLongOperation();
            progressBar1.Value = 0;
            for (int i = 0; i < progressBar1.Maximum * multiplier; ++i)
            {
                if (i % multiplier == 0)
                    progressBar1.Value = i / multiplier;
            }
            progressBar1.Value = 0;
            Close();
        }

        private void DoIdle(out bool aProcessed)
        {
            Cursor = Cursors.Default;
            aProcessed = true;
        }

        public static void Display(IAppTaxApplicationService aApplication)
        {
            using (var frm = new CursorDemo())
            {
                frm.FApplication = aApplication;
                frm.ShowDialog();
            }
        }
    }
}
