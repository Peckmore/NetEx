using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

namespace NetExDemo.CredentialDialog
{
    public partial class CredentialDialogForm : Form
    {
        #region Construction

        public CredentialDialogForm()
        {
            InitializeComponent();
            credentialDialogPropertyGrid.SelectedObject = credentialDialog;
        }

        #endregion

        #region Methods

        [SuppressMessage("ReSharper", "LocalizableElement")]
        private void PromptButton_Click(object sender, EventArgs e)
        {
            if (credentialDialog.ShowDialog() == DialogResult.OK)
#if DEBUG
                MessageBox.Show("Domain:\t" + credentialDialog.Domain + "\nUser:\t" + credentialDialog.Username + "\nPassword:\t" + credentialDialog.PasswordString);
#else
                MessageBox.Show("Domain:\t" + credentialDialog.Domain + "\nUser:\t" + credentialDialog.Username + "\nPassword is a SecureString and cannot be displayed without converting to a String.");
#endif

            // Refresh the property grid
            credentialDialogPropertyGrid.Refresh();
        }

        #endregion
    }
}