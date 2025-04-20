using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

namespace NetExDemo.CredentialDialog
{
    public partial class CredentialDialogForm : Form
    {
        #region Construction

        [SuppressMessage("ReSharper", "LocalizableElement")]
        public CredentialDialogForm()
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

        [SuppressMessage("ReSharper", "LocalizableElement")]
        private void showDialogButton_Click(object sender, EventArgs e)
        {
            if (credentialDialog.ShowDialog() == DialogResult.OK)
            {
#if DEBUG
                MessageBox.Show("Domain:\t" + credentialDialog.Domain + "\nUser:\t" + credentialDialog.Username + "\nPassword:\t" + credentialDialog.PasswordString);
#else
                MessageBox.Show("Domain:\t" + credentialDialog.Domain + "\nUser:\t" + credentialDialog.Username + "\nPassword is a SecureString and cannot be displayed without converting to a String.");
#endif
            }

            // Refresh the property grid
            credentialDialogPropertyGrid.Refresh();
        }

        #endregion
    }
}