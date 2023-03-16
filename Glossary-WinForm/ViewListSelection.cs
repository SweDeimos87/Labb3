using System;
using System.Linq;
using System.Windows.Forms;
using Glossary_Library;

namespace Glossary_Winform
{
    public partial class ViewListSelection : Form
    {
        public ViewListSelection()
        {
            InitializeComponent();
        }

        private void ViewListSelection_Load(object sender, EventArgs e)
        {
            cmbLists.Items.Clear();
            var list = WordList.GetLists();
            if (list != null && list.Any())
            {
                cmbLists.Items.AddRange(list.ToArray());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cmbLists.SelectedItem != null)
            {
                Program.LastSelectedList = cmbLists.SelectedItem.ToString();
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
