using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using TaxprepAddinAPI;

namespace VSAddinTest.UI
{
    public partial class UFLDemo : Form
    {
        private IAppUFL fUfl;
        private IAppDocReturn FReturn;

        public UFLDemo(IAppUFL aUfl, IAppDocReturn aReturn)
        {
            fUfl = aUfl;
            FReturn = aReturn;
            InitializeComponent();
            lstMain.Items.Clear();
            cbLanguage.SelectedIndex = 0;
        }

        private void DisplayUFL(bool aOnlySignable)
        {
            lstMain.Items.Clear();
            for (int lNodeIndex = 0; lNodeIndex < fUfl.Count; ++lNodeIndex)
            {
                var lNode = fUfl.Form[lNodeIndex];
                var lAddinSupported = lNode.IsAddinSupported(1);
                var lApplicable = FReturn.IsApplicable((uint) lNode.formNumber);

                if (aOnlySignable && !lAddinSupported)
                    continue;

                var lListNode = new ListViewItem();
                lListNode.Text = lApplicable ? "X" : "";
                lListNode.Tag = lNode;
                lstMain.Items.Add(lListNode);

                var lListSubNode = new ListViewItem.ListViewSubItem();
                lListSubNode.Text = lNode.FormNAme;
                lListNode.SubItems.Add(lListSubNode);

                lListSubNode = new ListViewItem.ListViewSubItem();
                lListSubNode.Text = lAddinSupported ? "Yes" : "No";
                lListNode.SubItems.Add(lListSubNode);

                lListSubNode = new ListViewItem.ListViewSubItem();
                lListSubNode.Text = lNode.LongDesc;
                lListNode.SubItems.Add(lListSubNode);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DisplayUFL(false);
        }

        public static void ShowUFL(IAppUFL aUFL, IAppDocReturn aReturn)
        {
            using (var lDalog = new UFLDemo(aUFL, aReturn))
            {
                lDalog.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DisplayUFL(true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lstMain.FocusedItem == null)
            {
                MessageBox.Show("Please select the form", "Print form fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            IAppUFLNode lNode = (IAppUFLNode)lstMain.FocusedItem.Tag;
            AppLanguage lLanguage = cbLanguage.SelectedIndex == 0 ? AppLanguage.lEnglish : AppLanguage.lFrench;
            string LFileName;
            if (lNode.Print(FReturn, lLanguage, out LFileName))
            {
                if (MessageBox.Show(string.Format("Form {0} has been printed succesfully!\n\nThe file name is:\n{1}\nWould you like to open file?",
                    lNode.FormNAme, LFileName), "Print form successfull", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo(LFileName) { UseShellExecute = true });
                }
            }
            else
            {
                MessageBox.Show("Could not print the file!", "Print form fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
