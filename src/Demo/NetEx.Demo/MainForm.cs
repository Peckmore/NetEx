using NetEx.Demo.CredentialDialog;
using NetEx.Demo.Hooks;
using NetEx.Demo.ProgressDialog;
using System;
using System.Windows.Forms;

namespace NetEx.Demo
{
    public partial class MainForm : Form
    {
        #region Construction

        public MainForm()
        {
            InitializeComponent();

#if NET20
            frameworkToolStripStatusLabel.Text = ".Net Framework 2.0";
#elif NET9_0
            frameworkToolStripStatusLabel.Text = ".Net 9.0";
#endif
        }

        #endregion

        #region Methods

        #region Event Handlers

        private void buttonCredentialDialogDemo_Click(object sender, EventArgs e)
        {
            new CredentialDialogForm().Show();
        }
        private void buttonHooks_Click(object sender, EventArgs e)
        {
            new HooksForm().Show();
        }
        private void buttonProgressDialogDemo_Click(object sender, EventArgs e)
        {
            new ProgressDialogForm().Show();
        }

        #endregion

        #endregion
    }
}