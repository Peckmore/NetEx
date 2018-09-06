namespace NetExDemo
{
    partial class MainForm
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
            this.buttonCredentialDialog = new System.Windows.Forms.Button();
            this.buttonProgressDialog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonCredentialDialog
            // 
            this.buttonCredentialDialog.Location = new System.Drawing.Point(12, 12);
            this.buttonCredentialDialog.Name = "buttonCredentialDialog";
            this.buttonCredentialDialog.Size = new System.Drawing.Size(108, 23);
            this.buttonCredentialDialog.TabIndex = 0;
            this.buttonCredentialDialog.Text = "Credential Dialog";
            this.buttonCredentialDialog.UseVisualStyleBackColor = true;
            this.buttonCredentialDialog.Click += new System.EventHandler(this.ButtonCredentialDialog_Click);
            // 
            // buttonProgressDialog
            // 
            this.buttonProgressDialog.Location = new System.Drawing.Point(126, 12);
            this.buttonProgressDialog.Name = "buttonProgressDialog";
            this.buttonProgressDialog.Size = new System.Drawing.Size(108, 23);
            this.buttonProgressDialog.TabIndex = 1;
            this.buttonProgressDialog.Text = "Progress Dialog";
            this.buttonProgressDialog.UseVisualStyleBackColor = true;
            this.buttonProgressDialog.Click += new System.EventHandler(this.ButtonProgressDialog_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 46);
            this.Controls.Add(this.buttonProgressDialog);
            this.Controls.Add(this.buttonCredentialDialog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NetEx Demo";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCredentialDialog;
        private System.Windows.Forms.Button buttonProgressDialog;
    }
}