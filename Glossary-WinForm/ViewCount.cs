using System;
using System.Linq;
using System.Windows.Forms;
using Glossary_Library;

namespace Glossary_Winform
{
    public partial class ViewCount : Form
    {
        public ViewCount()
        {
            InitializeComponent();
        }

        private void ViewCount_Load(object sender, EventArgs e)
        {
            LoadLists();
        }

        private void LoadLists()
        {
            cmbLists.Items.Clear();
            var list = WordList.GetLists();
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    cmbLists.Items.Add(item);
                }

                cmbLists.SelectedIndex = 0;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmbLists_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLists.SelectedItem != null)
            {
                var listName = cmbLists.SelectedItem.ToString();

                var wordList = WordList.LoadList(listName);

                if (wordList != null)
                {
                    lbCount.Text = $"Words Count: {wordList.Count()}";
                }
            }
        }
    }
}

