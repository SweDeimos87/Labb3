using System;
using System.Linq;
using System.Windows.Forms;
using Glossary_Library;

namespace Glossary_Winform
{
    public partial class ViewRemove : Form
    {
        private WordList wordList;

        public ViewRemove()
        {
            InitializeComponent();
        }

        private void ViewRemove_Load(object sender, EventArgs e)
        {
            cmbLists.Items.Clear();
            var list = WordList.GetLists();
            if (list != null && list.Length > 0)
            {
                cmbLists.Items.AddRange(list);
                cmbLists.SelectedIndex = 0;
            }
        }

        private void BindLanguages(string listName)
        {
            cmbLanguage.Items.Clear();
            wordList = WordList.LoadList(listName);
            if (wordList != null && wordList.Languages != null && wordList.Languages.Length > 0)
            {
                cmbLanguage.Items.AddRange(wordList.Languages);
                cmbLanguage.SelectedIndex = 0;
            }
        }

        private void cmbLists_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLists.SelectedItem != null)
            {
                BindLanguages(Convert.ToString(cmbLists.SelectedItem));
            }
        }

        private void btnShowWords_Click(object sender, EventArgs e)
        {
            lbWords.Items.Clear();
            if (cmbLists.SelectedItem != null && cmbLanguage.SelectedItem != null)
            {
                if (wordList != null)
                {
                    wordList.List(cmbLanguage.SelectedIndex, (translations) =>
                    {
                        if (translations != null && translations.Length > 0)
                        {
                            lbWords.Items.AddRange(translations);
                        }
                    });
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (cmbLists.SelectedItem != null && cmbLanguage.SelectedItem != null && lbWords.SelectedItem != null)
            {
                var word = Convert.ToString(lbWords.SelectedItem);

                if (wordList != null)
                {
                    wordList.Remove(cmbLanguage.SelectedIndex, word);
                    wordList.Save();

                    btnShowWords_Click(sender, e);
                }
            }
        }
    }
}
