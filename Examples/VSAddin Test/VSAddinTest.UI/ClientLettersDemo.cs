using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxprepAddinAPI;

namespace VSAddinTest.UI
{
    public partial class ClientLettersDemo : Form
    {
        private IAppClientLetterManager FLettersManager;
        private IAppUFL FUfl;
        private IAppDocReturn FReturn;

        public ClientLettersDemo(IAppClientLetterManager aLettersManager, IAppDocReturn aReturn, IAppUFL aUfl)
        {
            FLettersManager = aLettersManager;
            FReturn = aReturn;
            FUfl = aUfl;
            InitializeComponent();
            lstMain.Items.Clear();
        }

        private void LoadLetters(bool aOnlySignable)
        {
            lstMain.Items.Clear();
            for (int LLetterIndex = 0; LLetterIndex < FLettersManager.Count; ++LLetterIndex)
            {
                var LLetter = FLettersManager.ClientLetters[LLetterIndex];
                var lAddinSupported = (LLetter.AddInsBitField & 1) != 0; 
                var lApplicable = FReturn.IsApplicable((uint) FUfl.GetFormIndexByName(LLetter.FormNAme));

                if (aOnlySignable && !lAddinSupported)
                    continue;

                var lListNode = new ListViewItem();
                lListNode.Text = lApplicable ? "X" : "";
                lstMain.Items.Add(lListNode);

                var lListSubNode = new ListViewItem.ListViewSubItem();
                lListSubNode.Text = LLetter.FormNAme;
                lListNode.SubItems.Add(lListSubNode);

                lListSubNode = new ListViewItem.ListViewSubItem();
                lListSubNode.Text = lAddinSupported ? "Yes" : "No";
                lListNode.SubItems.Add(lListSubNode);

                lListSubNode = new ListViewItem.ListViewSubItem();
                lListSubNode.Text = LLetter.FilePath;
                lListNode.SubItems.Add(lListSubNode);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadLetters(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadLetters(true);
        }

        public static void ShowDemo(IAppClientLetterManager aLettersManager, IAppDocReturn aReturn, IAppUFL aUfl)
        {
            using (var LDialog = new ClientLettersDemo(aLettersManager, aReturn, aUfl))
            {
                LDialog.ShowDialog();
            }
        }
    }
}
