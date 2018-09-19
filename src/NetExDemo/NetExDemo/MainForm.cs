using NetExDemo.CredentialDialog;
using NetExDemo.ProgressDialog;
using System;
using System.Windows.Forms;

namespace NetExDemo
{
    public partial class MainForm : Form
    {
        #region Construction

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void ButtonCredentialDialog_Click(object sender, EventArgs e)
        {
            using (var f = new CredentialDialogForm())
                f.ShowDialog();
        }
        private void ButtonProgressDialog_Click(object sender, EventArgs e)
        {
            using (var f = new ProgressDialogForm())
                f.ShowDialog();
        }

        #endregion
    }
}