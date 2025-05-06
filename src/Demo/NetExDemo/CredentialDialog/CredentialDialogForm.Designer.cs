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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CredentialDialogForm));
            this.showDialogButton = new System.Windows.Forms.Button();
            this.credentialDialogPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.credentialDialog = new NetEx.Windows.Forms.CredentialDialog();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.frameworkToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // showDialogButton
            // 
            this.showDialogButton.Location = new System.Drawing.Point(12, 12);
            this.showDialogButton.Name = "showDialogButton";
            this.showDialogButton.Size = new System.Drawing.Size(75, 23);
            this.showDialogButton.TabIndex = 2;
            this.showDialogButton.Text = "Show Dialog";
            this.showDialogButton.UseVisualStyleBackColor = true;
            this.showDialogButton.Click += new System.EventHandler(this.showDialogButton_Click);
            // 
            // credentialDialogPropertyGrid
            // 
            this.credentialDialogPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.credentialDialogPropertyGrid.Location = new System.Drawing.Point(12, 41);
            this.credentialDialogPropertyGrid.Name = "credentialDialogPropertyGrid";
            this.credentialDialogPropertyGrid.SelectedObject = this.credentialDialog;
            this.credentialDialogPropertyGrid.Size = new System.Drawing.Size(314, 446);
            this.credentialDialogPropertyGrid.TabIndex = 4;
            // 
            // credentialDialog
            // 
            this.credentialDialog.Title = "Credential Dialog";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.frameworkToolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 501);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(338, 22);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // frameworkToolStripStatusLabel
            // 
            this.frameworkToolStripStatusLabel.Name = "frameworkToolStripStatusLabel";
            this.frameworkToolStripStatusLabel.Size = new System.Drawing.Size(171, 17);
            this.frameworkToolStripStatusLabel.Text = "frameworkToolStripStatusLabel";
            // 
            // CredentialDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 523);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.credentialDialogPropertyGrid);
            this.Controls.Add(this.showDialogButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CredentialDialogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Credential Dialog Demo";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button showDialogButton;
        private PropertyGrid credentialDialogPropertyGrid;
        private NetEx.Windows.Forms.CredentialDialog credentialDialog;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel frameworkToolStripStatusLabel;
    }
}