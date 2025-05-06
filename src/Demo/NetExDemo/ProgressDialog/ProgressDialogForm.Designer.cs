namespace NetExDemo.ProgressDialog
{
    partial class ProgressDialogForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressDialogForm));
            this.showDialogButton = new System.Windows.Forms.Button();
            this.showButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.progressDialog = new NetEx.Windows.Forms.ProgressDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.frameworkToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // showDialogButton
            // 
            this.showDialogButton.Location = new System.Drawing.Point(12, 12);
            this.showDialogButton.Name = "showDialogButton";
            this.showDialogButton.Size = new System.Drawing.Size(75, 23);
            this.showDialogButton.TabIndex = 0;
            this.showDialogButton.Text = "Show Dialog";
            this.showDialogButton.UseVisualStyleBackColor = true;
            this.showDialogButton.Click += new System.EventHandler(this.ShowDialogButton_Click);
            // 
            // showButton
            // 
            this.showButton.Location = new System.Drawing.Point(93, 12);
            this.showButton.Name = "showButton";
            this.showButton.Size = new System.Drawing.Size(75, 23);
            this.showButton.TabIndex = 2;
            this.showButton.Text = "Show";
            this.showButton.UseVisualStyleBackColor = true;
            this.showButton.Click += new System.EventHandler(this.ShowButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Enabled = false;
            this.closeButton.Location = new System.Drawing.Point(174, 12);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(12, 41);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.SelectedObject = this.progressDialog;
            this.propertyGrid.Size = new System.Drawing.Size(314, 446);
            this.propertyGrid.TabIndex = 3;
            // 
            // progressDialog
            // 
            this.progressDialog.Title = "Progress Dialog";
            this.progressDialog.Canceled += new System.EventHandler(this.ProgressDialog_Canceled);
            this.progressDialog.Closed += new System.EventHandler(this.ProgressDialog_Closed);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.frameworkToolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 501);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(338, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // frameworkToolStripStatusLabel
            // 
            this.frameworkToolStripStatusLabel.Name = "frameworkToolStripStatusLabel";
            this.frameworkToolStripStatusLabel.Size = new System.Drawing.Size(171, 17);
            this.frameworkToolStripStatusLabel.Text = "frameworkToolStripStatusLabel";
            // 
            // ProgressDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 523);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.showButton);
            this.Controls.Add(this.showDialogButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProgressDialogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Progress Dialog Demo";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button showDialogButton;
        private System.Windows.Forms.Button showButton;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button closeButton;
        private NetEx.Windows.Forms.ProgressDialog progressDialog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel frameworkToolStripStatusLabel;
    }
}

