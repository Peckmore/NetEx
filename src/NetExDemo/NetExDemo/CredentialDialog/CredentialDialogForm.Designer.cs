using System.Windows.Forms;

namespace NetExDemo.CredentialDialog
{
    partial class CredentialDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Security.SecureString secureString1 = new System.Security.SecureString();
            this.promptButton = new System.Windows.Forms.Button();
            this.credentialDialogPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.credentialDialog = new System.Windows.Forms.CredentialDialog();
            this.SuspendLayout();
            // 
            // promptButton
            // 
            this.promptButton.Location = new System.Drawing.Point(12, 12);
            this.promptButton.Name = "promptButton";
            this.promptButton.Size = new System.Drawing.Size(75, 23);
            this.promptButton.TabIndex = 2;
            this.promptButton.Text = "Prompt";
            this.promptButton.UseVisualStyleBackColor = true;
            this.promptButton.Click += new System.EventHandler(this.PromptButton_Click);
            // 
            // credentialDialogPropertyGrid
            // 
            this.credentialDialogPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.credentialDialogPropertyGrid.Location = new System.Drawing.Point(12, 41);
            this.credentialDialogPropertyGrid.Name = "credentialDialogPropertyGrid";
            this.credentialDialogPropertyGrid.Size = new System.Drawing.Size(314, 470);
            this.credentialDialogPropertyGrid.TabIndex = 4;
            // 
            // credentialDialog
            // 
            this.credentialDialog.AutoUpgradeEnabled = true;
            this.credentialDialog.Banner = null;
            this.credentialDialog.CredentialFilter = System.Windows.Forms.CredentialDialogCredentialFilter.AllCredentials;
            this.credentialDialog.Domain = "";
            this.credentialDialog.ErrorCode = 0;
            this.credentialDialog.IncorrectPasswordPrompt = false;
            this.credentialDialog.Message = "";
            this.credentialDialog.Password = secureString1;
            this.credentialDialog.SaveChecked = false;
            this.credentialDialog.ShowSave = true;
            this.credentialDialog.Title = "";
            this.credentialDialog.Username = "credentialDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 523);
            this.Controls.Add(this.credentialDialogPropertyGrid);
            this.Controls.Add(this.promptButton);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CredentialDialog Demo";
            this.ResumeLayout(false);

        }

        #endregion

        private Button promptButton;
        private PropertyGrid credentialDialogPropertyGrid;
        private System.Windows.Forms.CredentialDialog credentialDialog;
    }
}

