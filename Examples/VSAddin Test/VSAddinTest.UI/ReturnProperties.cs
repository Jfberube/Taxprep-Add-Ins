using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxprepAddinAPI;

namespace VSAddinTest
{
    public partial class ReturnProperties : Form
    {
        public ReturnProperties()
        {
            InitializeComponent();
        }

        private void ReturnProperties_Load(object sender, EventArgs e)
        {

        }

        IAppStatusProperties FStatus = null;

        public static void ShowReturnproperties(IAppDocReturn AReturn)
        {
            var LStatus = AReturn.GetProperties();
            using (var form = new ReturnProperties())
            {
                form.txtStatus.Text = "";
                form.FStatus = LStatus;
                form.txtStatus.Items.Clear();
                for (int i = 0; i < LStatus.Count; ++i)
                { 
                    string LName;
                    LStatus.GetKeyAt(i, out LName);
                    form.txtStatus.Items.Add(string.Format("{0} = {1}\r\n", LName, LStatus.AsString(LName, "")));
                }
                form.txtReturnStatusEng.Text = LStatus.GetReturnStatus(AppLanguage.lEnglish);
                form.txtReturnStatusFr.Text = LStatus.GetReturnStatus(AppLanguage.lFrench);
                form.ShowDialog();
                form.FStatus = null;
            }
            LStatus = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                txtStatusValueEn.Text = "";
                txtStatusValueFr.Text = "";
                if (txtStatus.SelectedIndex < 0)
                    return;
                string LName;
                FStatus.GetKeyAt(txtStatus.SelectedIndex, out LName);
                txtStatusValueEn.Text = FStatus.GetStatusInLanguage(LName, AppLanguage.lEnglish, txtStatusEnumName.Text);
                txtStatusValueFr.Text = FStatus.GetStatusInLanguage(LName, AppLanguage.lFrench, txtStatusEnumName.Text);
            }
            catch
            {
                MessageBox.Show("Could not get the status value");
            }
        }

    }
}
