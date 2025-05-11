namespace NetEx.Demo
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
            this.buttonCredentialDialogDemo = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.frameworkToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonProgressDialogDemo = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCredentialDialogDemo
            // 
            this.buttonCredentialDialogDemo.Location = new System.Drawing.Point(12, 12);
            this.buttonCredentialDialogDemo.Name = "buttonCredentialDialogDemo";
            this.buttonCredentialDialogDemo.Size = new System.Drawing.Size(109, 42);
            this.buttonCredentialDialogDemo.TabIndex = 0;
            this.buttonCredentialDialogDemo.Text = "Credential Dialog";
            this.buttonCredentialDialogDemo.UseVisualStyleBackColor = true;
            this.buttonCredentialDialogDemo.Click += new System.EventHandler(this.buttonCredentialDialogDemo_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.frameworkToolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 70);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(360, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // frameworkToolStripStatusLabel
            // 
            this.frameworkToolStripStatusLabel.Name = "frameworkToolStripStatusLabel";
            this.frameworkToolStripStatusLabel.Size = new System.Drawing.Size(170, 17);
            this.frameworkToolStripStatusLabel.Text = "frameworkToolStripStatusLabel";
            // 
            // buttonProgressDialogDemo
            // 
            this.buttonProgressDialogDemo.Location = new System.Drawing.Point(127, 12);
            this.buttonProgressDialogDemo.Name = "buttonProgressDialogDemo";
            this.buttonProgressDialogDemo.Size = new System.Drawing.Size(109, 42);
            this.buttonProgressDialogDemo.TabIndex = 7;
            this.buttonProgressDialogDemo.Text = "Progress Dialog";
            this.buttonProgressDialogDemo.UseVisualStyleBackColor = true;
            this.buttonProgressDialogDemo.Click += new System.EventHandler(this.buttonProgressDialogDemo_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(242, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 42);
            this.button1.TabIndex = 8;
            this.button1.Text = "Hooks";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonHooks_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 92);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonProgressDialogDemo);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.buttonCredentialDialogDemo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NetEx Demo";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCredentialDialogDemo;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel frameworkToolStripStatusLabel;
        private System.Windows.Forms.Button buttonProgressDialogDemo;
        private System.Windows.Forms.Button button1;
    }
}