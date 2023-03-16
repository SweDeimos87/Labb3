using System;
using System.Linq;
using System.Windows.Forms;
using Glossary_Library;

namespace Glossary_Winform
{
    public partial class ViewList : Form
    {
        public ViewList()
        {
            InitializeComponent();
        }

        private void ViewList_Load(object sender, EventArgs e)
        {
            lbMain.Items.Clear();
            var list = WordList.GetLists();
            if (list != null && list.Any())
            {
                lbMain.Items.AddRange(list);
            }
        }
    }
}
