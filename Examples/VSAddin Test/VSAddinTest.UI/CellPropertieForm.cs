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
    public partial class CellPropertieForm : Form
    {
        private IAppTaxCell FCell;
        private IAppDocReturn FReturn;

        public CellPropertieForm()
        {
            InitializeComponent();
        }

        private void CellPropertieForm_Load(object sender, EventArgs e)
        {

        }

        private void FillXLatCombo(ComboBox ACombo, AppLanguage ALanguage, int AColumn, IAppTaxCell ACell, 
            IAppTaxApplicationService AApplication)
        {
            ACombo.Items.Clear();
            int LCount = ACell.GetXLatValuesCount(AppLanguage.lEnglish, AApplication);
            ACombo.Items.Clear();
            try
            {
                for (int i = 0; i < LCount; ++i)
                {
                    ACombo.Items.Add(ACell.GetXLatValue(AApplication, ALanguage, AColumn, i));
                }
                ACombo.SelectedIndex = 0;
            }
            catch
            {
            }
        }

        public static void ShowCellProperties(IAppTaxCell aCell, IAppTaxApplicationService aApplication)
        {
            using (var form = new CellPropertieForm())
            {
                form.FCell = aCell;
                form.FReturn = aApplication.GetCurrentDocReturn();
                form.txtName.Text = aCell.GetCellNameWithGroup();
                form.lblType.Text = aCell.GetCellTypeText();
                form.lblDefaultForm.Text = aCell.GetOwnerTaxData().GetFormName(aCell.GetFormNum());
                string lStringValue = "";
                if (aCell.ConvertToString(ref lStringValue))
                    form.txtValue.Text = lStringValue;
                else
                    form.txtValue.Text = "";
                form.cbHasInput.Checked = aCell.HasInput();
                form.cbHasCalc.Checked = aCell.HasCalc();
                form.cbHasImport.Checked = aCell.IsImported();
                form.cbIsEmpty.Checked = aCell.IsEmpty();
                form.cbIsEstimated.Checked = aCell.IsEstimated();
                form.cbIsTracking.Checked = aCell.IsTracking();
                form.cbHasFormNum.Checked = aCell.HasFormNum();
                form.cbHasInternalOvrd.Checked = aCell.HasInternalOvrd();
                form.cbHasRolledValue.Checked = aCell.HasRolledValue();
                form.cbHasUserOvrd.Checked = aCell.HasUserOvrd(); 
                form.cbIsPositiveOnly.Checked = aCell.IsPositiveOnly();
                form.cbIsNA.Checked = aCell.IsNA(); 
                form.cbIsSourceEstimate.Checked = aCell.IsSourceEstimate();
                form.cbIsProtected.Checked = aCell.IsProtected();
                form.cbHasRolledValue.Checked = aCell.HasRolledValue();
                form.cbIsSelectable.Checked = aCell.IsSelectable();
                form.cbIsRoundOnAssign.Checked = aCell.IsRoundOnAssign();
                form.cbIsDeprecated.Checked = aCell.IsDeprecated();
                form.cbHasTransferredValue.Checked = aCell.HasTransferredValue();
                var LXlat = aCell.GetAssociatedStringTable();
                if (LXlat >= 0)
                {
                    form.lblXlatEnum.Text = LXlat.ToString();
                    form.FillXLatCombo(form.cbXlatEng0, AppLanguage.lEnglish, 0, aCell, aApplication);
                    form.FillXLatCombo(form.cbXlatEng1, AppLanguage.lEnglish, 1, aCell, aApplication);
                    form.FillXLatCombo(form.cbXlatEng2, AppLanguage.lEnglish, 2, aCell, aApplication);
                    form.FillXLatCombo(form.cbXlatFr0, AppLanguage.lFrench, 0, aCell, aApplication);
                    form.FillXLatCombo(form.cbXlatFr1, AppLanguage.lFrench, 1, aCell, aApplication);
                    form.FillXLatCombo(form.cbXlatFr2, AppLanguage.lFrench, 2, aCell, aApplication);
                }
                else
                    form.lblXlatEnum.Text = "None";
                form.lblContentType.Text = aCell.GetCellTypeText();
                form.txtMask.Text = aCell.GetEditControlMask(0);
                string LAliases = "";
                for (int i = 0; i < aCell.GetAliasNamesCount(); ++i)
                {
                    LAliases += aCell.GetAliasNames(i) + "\r\n";
                }
                form.txtAliases.Text = string.Format(
                    "Aliases count {0}\r\nAliases:\r\n{1}",
                    aCell.GetAliasNamesCount(), LAliases
                );
                form.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void CellPropertieForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FCell = null;
            FReturn = null;
        }

        private void cbXlatEng0_SelectedIndexChanged(object sender, EventArgs e)
        {
            int LIndex = (sender as ComboBox).SelectedIndex;
            if (cbXlatEng0 != sender && cbXlatEng0.Items.Count > LIndex) 
                cbXlatEng0.SelectedIndex = LIndex;
            if (cbXlatEng1 != sender && cbXlatEng1.Items.Count > LIndex) 
                cbXlatEng1.SelectedIndex = LIndex;
            if (cbXlatEng2 != sender && cbXlatEng2.Items.Count > LIndex)
                cbXlatEng2.SelectedIndex = LIndex;
            if (cbXlatFr0 != sender && cbXlatFr0.Items.Count > LIndex)
                cbXlatFr0.SelectedIndex = LIndex;
            if (cbXlatFr1 != sender && cbXlatFr0.Items.Count > LIndex)
                cbXlatFr1.SelectedIndex = LIndex;
            if (cbXlatFr2 != sender && cbXlatFr0.Items.Count > LIndex)
                cbXlatFr2.SelectedIndex = LIndex;
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FReturn.SetAliasString(FCell.GetCellNameWithGroup(), txtValue.Text);
        }

    }
}
