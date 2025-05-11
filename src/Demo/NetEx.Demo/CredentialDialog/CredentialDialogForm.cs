using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

namespace NetEx.Demo.CredentialDialog
{
    public partial class CredentialDialogForm : Form
    {
        #region Construction

        [SuppressMessage("ReSharper", "LocalizableElement")]
        public CredentialDialogForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        [SuppressMessage("ReSharper", "LocalizableElement")]
        private void showDialogButton_Click(object sender, EventArgs e)
        {
            if (credentialDialog.ShowDialog() == DialogResult.OK)
            {
#if DEBUG
                MessageBox.Show("Domain:\n"
                                + credentialDialog.Domain
                                + "\n\nUser:\n"
                                + credentialDialog.Username
                                + "\n\nPassword:\n"
                                + credentialDialog.PasswordString);
#else
                MessageBox.Show("Domain:\n"
                                + credentialDialog.Domain
                                + "\n\nUser:\n"
                                + credentialDialog.Username
                                + "\n\nPassword is a SecureString and cannot be displayed without converting to a String.");
#endif
            }

            // Refresh the property grid
            credentialDialogPropertyGrid.Refresh();
        }

        #endregion
    }
}