namespace NetEx.Demo.Hooks
{
    partial class HooksForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HooksForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.keyboardTabPage = new System.Windows.Forms.TabPage();
            this.hookKeyboardCheckBox = new System.Windows.Forms.CheckBox();
            this.keyboardHookControlsPanel = new System.Windows.Forms.Panel();
            this.sendKeyComboBox = new System.Windows.Forms.ComboBox();
            this.sendKeyPressButton = new System.Windows.Forms.Button();
            this.sendKeyUpButton = new System.Windows.Forms.Button();
            this.sendKeyDownButton = new System.Windows.Forms.Button();
            this.keyUpHookCheckBox = new System.Windows.Forms.CheckBox();
            this.keyDownHookCheckBox = new System.Windows.Forms.CheckBox();
            this.lastKeyPressedPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.keyboardLogListBox = new System.Windows.Forms.ListBox();
            this.blockWinKeyCheckBox = new System.Windows.Forms.CheckBox();
            this.keyboardLogClearButton = new System.Windows.Forms.Button();
            this.mouseTabPage = new System.Windows.Forms.TabPage();
            this.hookMouseCheckBox = new System.Windows.Forms.CheckBox();
            this.mouseHookControlsPanel = new System.Windows.Forms.Panel();
            this.hookMouseMoveCheckBox = new System.Windows.Forms.CheckBox();
            this.playButton = new System.Windows.Forms.Button();
            this.mouseLogClearButton = new System.Windows.Forms.Button();
            this.hookMouseDoubleClickCheckBox = new System.Windows.Forms.CheckBox();
            this.mouseLogListBox = new System.Windows.Forms.ListBox();
            this.hookMouseUpCheckBox = new System.Windows.Forms.CheckBox();
            this.hookMouseDownCheckBox = new System.Windows.Forms.CheckBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.hookMouseWheelCheckBox = new System.Windows.Forms.CheckBox();
            this.hookMouseClickCheckBox = new System.Windows.Forms.CheckBox();
            this.recordButton = new System.Windows.Forms.Button();
            this.recordedEventsLabel = new System.Windows.Forms.Label();
            this.clipboardTabPage = new System.Windows.Forms.TabPage();
            this.clipboardPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.hookClipboardCheckBox = new System.Windows.Forms.CheckBox();
            this.tabControl.SuspendLayout();
            this.keyboardTabPage.SuspendLayout();
            this.keyboardHookControlsPanel.SuspendLayout();
            this.mouseTabPage.SuspendLayout();
            this.mouseHookControlsPanel.SuspendLayout();
            this.clipboardTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.keyboardTabPage);
            this.tabControl.Controls.Add(this.mouseTabPage);
            this.tabControl.Controls.Add(this.clipboardTabPage);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(518, 454);
            this.tabControl.TabIndex = 5;
            // 
            // keyboardTabPage
            // 
            this.keyboardTabPage.Controls.Add(this.hookKeyboardCheckBox);
            this.keyboardTabPage.Controls.Add(this.keyboardHookControlsPanel);
            this.keyboardTabPage.Location = new System.Drawing.Point(4, 22);
            this.keyboardTabPage.Name = "keyboardTabPage";
            this.keyboardTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.keyboardTabPage.Size = new System.Drawing.Size(510, 428);
            this.keyboardTabPage.TabIndex = 0;
            this.keyboardTabPage.Text = "Keyboard";
            this.keyboardTabPage.UseVisualStyleBackColor = true;
            // 
            // hookKeyboardCheckBox
            // 
            this.hookKeyboardCheckBox.AutoSize = true;
            this.hookKeyboardCheckBox.Location = new System.Drawing.Point(6, 10);
            this.hookKeyboardCheckBox.Name = "hookKeyboardCheckBox";
            this.hookKeyboardCheckBox.Size = new System.Drawing.Size(100, 17);
            this.hookKeyboardCheckBox.TabIndex = 21;
            this.hookKeyboardCheckBox.Text = "Hook Keyboard";
            this.hookKeyboardCheckBox.UseVisualStyleBackColor = true;
            this.hookKeyboardCheckBox.CheckedChanged += new System.EventHandler(this.hookKeyboardCheckBox_CheckedChanged);
            // 
            // keyboardHookControlsPanel
            // 
            this.keyboardHookControlsPanel.Controls.Add(this.sendKeyComboBox);
            this.keyboardHookControlsPanel.Controls.Add(this.sendKeyPressButton);
            this.keyboardHookControlsPanel.Controls.Add(this.sendKeyUpButton);
            this.keyboardHookControlsPanel.Controls.Add(this.sendKeyDownButton);
            this.keyboardHookControlsPanel.Controls.Add(this.keyUpHookCheckBox);
            this.keyboardHookControlsPanel.Controls.Add(this.keyDownHookCheckBox);
            this.keyboardHookControlsPanel.Controls.Add(this.lastKeyPressedPropertyGrid);
            this.keyboardHookControlsPanel.Controls.Add(this.keyboardLogListBox);
            this.keyboardHookControlsPanel.Controls.Add(this.blockWinKeyCheckBox);
            this.keyboardHookControlsPanel.Controls.Add(this.keyboardLogClearButton);
            this.keyboardHookControlsPanel.Enabled = false;
            this.keyboardHookControlsPanel.Location = new System.Drawing.Point(0, 30);
            this.keyboardHookControlsPanel.Name = "keyboardHookControlsPanel";
            this.keyboardHookControlsPanel.Size = new System.Drawing.Size(510, 398);
            this.keyboardHookControlsPanel.TabIndex = 20;
            // 
            // sendKeyComboBox
            // 
            this.sendKeyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sendKeyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sendKeyComboBox.FormattingEnabled = true;
            this.sendKeyComboBox.Location = new System.Drawing.Point(327, 369);
            this.sendKeyComboBox.Name = "sendKeyComboBox";
            this.sendKeyComboBox.Size = new System.Drawing.Size(177, 21);
            this.sendKeyComboBox.TabIndex = 27;
            // 
            // sendKeyPressButton
            // 
            this.sendKeyPressButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sendKeyPressButton.Location = new System.Drawing.Point(219, 369);
            this.sendKeyPressButton.Name = "sendKeyPressButton";
            this.sendKeyPressButton.Size = new System.Drawing.Size(102, 23);
            this.sendKeyPressButton.TabIndex = 24;
            this.sendKeyPressButton.Text = "Send Key Press";
            this.sendKeyPressButton.UseVisualStyleBackColor = true;
            this.sendKeyPressButton.Click += new System.EventHandler(this.sendKeyPressButton_Click);
            // 
            // sendKeyUpButton
            // 
            this.sendKeyUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sendKeyUpButton.Location = new System.Drawing.Point(111, 369);
            this.sendKeyUpButton.Name = "sendKeyUpButton";
            this.sendKeyUpButton.Size = new System.Drawing.Size(102, 23);
            this.sendKeyUpButton.TabIndex = 25;
            this.sendKeyUpButton.Text = "Send Key Up";
            this.sendKeyUpButton.UseVisualStyleBackColor = true;
            this.sendKeyUpButton.Click += new System.EventHandler(this.sendKeyUpButton_Click);
            // 
            // sendKeyDownButton
            // 
            this.sendKeyDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sendKeyDownButton.Location = new System.Drawing.Point(3, 369);
            this.sendKeyDownButton.Name = "sendKeyDownButton";
            this.sendKeyDownButton.Size = new System.Drawing.Size(102, 23);
            this.sendKeyDownButton.TabIndex = 26;
            this.sendKeyDownButton.Text = "Send Key Down";
            this.sendKeyDownButton.UseVisualStyleBackColor = true;
            this.sendKeyDownButton.Click += new System.EventHandler(this.sendKeyDownButton_Click);
            // 
            // keyUpHookCheckBox
            // 
            this.keyUpHookCheckBox.AutoSize = true;
            this.keyUpHookCheckBox.Location = new System.Drawing.Point(6, 26);
            this.keyUpHookCheckBox.Name = "keyUpHookCheckBox";
            this.keyUpHookCheckBox.Size = new System.Drawing.Size(121, 17);
            this.keyUpHookCheckBox.TabIndex = 23;
            this.keyUpHookCheckBox.Text = "Hook Key Up Event";
            this.keyUpHookCheckBox.UseVisualStyleBackColor = true;
            this.keyUpHookCheckBox.CheckedChanged += new System.EventHandler(this.keyUpHookCheckBox_CheckedChanged);
            // 
            // keyDownHookCheckBox
            // 
            this.keyDownHookCheckBox.AutoSize = true;
            this.keyDownHookCheckBox.Location = new System.Drawing.Point(6, 3);
            this.keyDownHookCheckBox.Name = "keyDownHookCheckBox";
            this.keyDownHookCheckBox.Size = new System.Drawing.Size(135, 17);
            this.keyDownHookCheckBox.TabIndex = 22;
            this.keyDownHookCheckBox.Text = "Hook Key Down Event";
            this.keyDownHookCheckBox.UseVisualStyleBackColor = true;
            this.keyDownHookCheckBox.CheckedChanged += new System.EventHandler(this.keyDownHookCheckBox_CheckedChanged);
            // 
            // lastKeyPressedPropertyGrid
            // 
            this.lastKeyPressedPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lastKeyPressedPropertyGrid.Enabled = false;
            this.lastKeyPressedPropertyGrid.Location = new System.Drawing.Point(6, 72);
            this.lastKeyPressedPropertyGrid.Name = "lastKeyPressedPropertyGrid";
            this.lastKeyPressedPropertyGrid.Size = new System.Drawing.Size(243, 291);
            this.lastKeyPressedPropertyGrid.TabIndex = 18;
            // 
            // keyboardLogListBox
            // 
            this.keyboardLogListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.keyboardLogListBox.FormattingEnabled = true;
            this.keyboardLogListBox.Location = new System.Drawing.Point(255, 34);
            this.keyboardLogListBox.Name = "keyboardLogListBox";
            this.keyboardLogListBox.Size = new System.Drawing.Size(249, 329);
            this.keyboardLogListBox.TabIndex = 21;
            // 
            // blockWinKeyCheckBox
            // 
            this.blockWinKeyCheckBox.AutoSize = true;
            this.blockWinKeyCheckBox.Enabled = false;
            this.blockWinKeyCheckBox.Location = new System.Drawing.Point(6, 49);
            this.blockWinKeyCheckBox.Name = "blockWinKeyCheckBox";
            this.blockWinKeyCheckBox.Size = new System.Drawing.Size(138, 17);
            this.blockWinKeyCheckBox.TabIndex = 20;
            this.blockWinKeyCheckBox.Text = "Block the Windows key";
            this.blockWinKeyCheckBox.UseVisualStyleBackColor = true;
            // 
            // keyboardLogClearButton
            // 
            this.keyboardLogClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.keyboardLogClearButton.Location = new System.Drawing.Point(429, 5);
            this.keyboardLogClearButton.Name = "keyboardLogClearButton";
            this.keyboardLogClearButton.Size = new System.Drawing.Size(75, 23);
            this.keyboardLogClearButton.TabIndex = 19;
            this.keyboardLogClearButton.Text = "Clear";
            this.keyboardLogClearButton.UseVisualStyleBackColor = true;
            this.keyboardLogClearButton.Click += new System.EventHandler(this.keyboardLogClearButton_Click);
            // 
            // mouseTabPage
            // 
            this.mouseTabPage.Controls.Add(this.hookMouseCheckBox);
            this.mouseTabPage.Controls.Add(this.mouseHookControlsPanel);
            this.mouseTabPage.Location = new System.Drawing.Point(4, 22);
            this.mouseTabPage.Name = "mouseTabPage";
            this.mouseTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.mouseTabPage.Size = new System.Drawing.Size(510, 428);
            this.mouseTabPage.TabIndex = 1;
            this.mouseTabPage.Text = "Mouse";
            this.mouseTabPage.UseVisualStyleBackColor = true;
            // 
            // hookMouseCheckBox
            // 
            this.hookMouseCheckBox.AutoSize = true;
            this.hookMouseCheckBox.Location = new System.Drawing.Point(6, 10);
            this.hookMouseCheckBox.Name = "hookMouseCheckBox";
            this.hookMouseCheckBox.Size = new System.Drawing.Size(87, 17);
            this.hookMouseCheckBox.TabIndex = 22;
            this.hookMouseCheckBox.Text = "Hook Mouse";
            this.hookMouseCheckBox.UseVisualStyleBackColor = true;
            this.hookMouseCheckBox.CheckedChanged += new System.EventHandler(this.hookMouseCheckBox_CheckedChanged);
            // 
            // mouseHookControlsPanel
            // 
            this.mouseHookControlsPanel.Controls.Add(this.hookMouseMoveCheckBox);
            this.mouseHookControlsPanel.Controls.Add(this.playButton);
            this.mouseHookControlsPanel.Controls.Add(this.mouseLogClearButton);
            this.mouseHookControlsPanel.Controls.Add(this.hookMouseDoubleClickCheckBox);
            this.mouseHookControlsPanel.Controls.Add(this.mouseLogListBox);
            this.mouseHookControlsPanel.Controls.Add(this.hookMouseUpCheckBox);
            this.mouseHookControlsPanel.Controls.Add(this.hookMouseDownCheckBox);
            this.mouseHookControlsPanel.Controls.Add(this.clearButton);
            this.mouseHookControlsPanel.Controls.Add(this.stopButton);
            this.mouseHookControlsPanel.Controls.Add(this.hookMouseWheelCheckBox);
            this.mouseHookControlsPanel.Controls.Add(this.hookMouseClickCheckBox);
            this.mouseHookControlsPanel.Controls.Add(this.recordButton);
            this.mouseHookControlsPanel.Controls.Add(this.recordedEventsLabel);
            this.mouseHookControlsPanel.Enabled = false;
            this.mouseHookControlsPanel.Location = new System.Drawing.Point(0, 30);
            this.mouseHookControlsPanel.Name = "mouseHookControlsPanel";
            this.mouseHookControlsPanel.Size = new System.Drawing.Size(510, 398);
            this.mouseHookControlsPanel.TabIndex = 16;
            // 
            // hookMouseMoveCheckBox
            // 
            this.hookMouseMoveCheckBox.AutoSize = true;
            this.hookMouseMoveCheckBox.Location = new System.Drawing.Point(6, 3);
            this.hookMouseMoveCheckBox.Name = "hookMouseMoveCheckBox";
            this.hookMouseMoveCheckBox.Size = new System.Drawing.Size(148, 17);
            this.hookMouseMoveCheckBox.TabIndex = 15;
            this.hookMouseMoveCheckBox.Text = "Hook Mouse Move Event";
            this.hookMouseMoveCheckBox.UseVisualStyleBackColor = true;
            this.hookMouseMoveCheckBox.CheckedChanged += new System.EventHandler(this.hookMouseMoveCheckBox_CheckedChanged);
            // 
            // playButton
            // 
            this.playButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.playButton.Enabled = false;
            this.playButton.Location = new System.Drawing.Point(165, 369);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(75, 23);
            this.playButton.TabIndex = 4;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // mouseLogClearButton
            // 
            this.mouseLogClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mouseLogClearButton.Location = new System.Drawing.Point(426, 43);
            this.mouseLogClearButton.Name = "mouseLogClearButton";
            this.mouseLogClearButton.Size = new System.Drawing.Size(75, 23);
            this.mouseLogClearButton.TabIndex = 13;
            this.mouseLogClearButton.Text = "Clear";
            this.mouseLogClearButton.UseVisualStyleBackColor = true;
            this.mouseLogClearButton.Click += new System.EventHandler(this.mouseLogClearButton_Click);
            // 
            // hookMouseDoubleClickCheckBox
            // 
            this.hookMouseDoubleClickCheckBox.AutoSize = true;
            this.hookMouseDoubleClickCheckBox.Location = new System.Drawing.Point(213, 49);
            this.hookMouseDoubleClickCheckBox.Name = "hookMouseDoubleClickCheckBox";
            this.hookMouseDoubleClickCheckBox.Size = new System.Drawing.Size(181, 17);
            this.hookMouseDoubleClickCheckBox.TabIndex = 15;
            this.hookMouseDoubleClickCheckBox.Text = "Hook Mouse Double Click Event";
            this.hookMouseDoubleClickCheckBox.UseVisualStyleBackColor = true;
            this.hookMouseDoubleClickCheckBox.CheckedChanged += new System.EventHandler(this.hookMouseDoubleClickCheckBox_CheckedChanged);
            // 
            // mouseLogListBox
            // 
            this.mouseLogListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mouseLogListBox.FormattingEnabled = true;
            this.mouseLogListBox.Location = new System.Drawing.Point(6, 72);
            this.mouseLogListBox.Name = "mouseLogListBox";
            this.mouseLogListBox.Size = new System.Drawing.Size(498, 290);
            this.mouseLogListBox.TabIndex = 14;
            // 
            // hookMouseUpCheckBox
            // 
            this.hookMouseUpCheckBox.AutoSize = true;
            this.hookMouseUpCheckBox.Location = new System.Drawing.Point(213, 26);
            this.hookMouseUpCheckBox.Name = "hookMouseUpCheckBox";
            this.hookMouseUpCheckBox.Size = new System.Drawing.Size(135, 17);
            this.hookMouseUpCheckBox.TabIndex = 15;
            this.hookMouseUpCheckBox.Text = "Hook Mouse Up Event";
            this.hookMouseUpCheckBox.UseVisualStyleBackColor = true;
            this.hookMouseUpCheckBox.CheckedChanged += new System.EventHandler(this.hookMouseUpCheckBox_CheckedChanged);
            // 
            // hookMouseDownCheckBox
            // 
            this.hookMouseDownCheckBox.AutoSize = true;
            this.hookMouseDownCheckBox.Location = new System.Drawing.Point(6, 26);
            this.hookMouseDownCheckBox.Name = "hookMouseDownCheckBox";
            this.hookMouseDownCheckBox.Size = new System.Drawing.Size(149, 17);
            this.hookMouseDownCheckBox.TabIndex = 15;
            this.hookMouseDownCheckBox.Text = "Hook Mouse Down Event";
            this.hookMouseDownCheckBox.UseVisualStyleBackColor = true;
            this.hookMouseDownCheckBox.CheckedChanged += new System.EventHandler(this.hookMouseDownCheckBox_CheckedChanged);
            // 
            // clearButton
            // 
            this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearButton.Location = new System.Drawing.Point(246, 369);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 3;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(84, 369);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // hookMouseWheelCheckBox
            // 
            this.hookMouseWheelCheckBox.AutoSize = true;
            this.hookMouseWheelCheckBox.Location = new System.Drawing.Point(213, 3);
            this.hookMouseWheelCheckBox.Name = "hookMouseWheelCheckBox";
            this.hookMouseWheelCheckBox.Size = new System.Drawing.Size(152, 17);
            this.hookMouseWheelCheckBox.TabIndex = 15;
            this.hookMouseWheelCheckBox.Text = "Hook Mouse Wheel Event";
            this.hookMouseWheelCheckBox.UseVisualStyleBackColor = true;
            this.hookMouseWheelCheckBox.CheckedChanged += new System.EventHandler(this.hookMouseWheelCheckBox_CheckedChanged);
            // 
            // hookMouseClickCheckBox
            // 
            this.hookMouseClickCheckBox.AutoSize = true;
            this.hookMouseClickCheckBox.Location = new System.Drawing.Point(6, 49);
            this.hookMouseClickCheckBox.Name = "hookMouseClickCheckBox";
            this.hookMouseClickCheckBox.Size = new System.Drawing.Size(144, 17);
            this.hookMouseClickCheckBox.TabIndex = 15;
            this.hookMouseClickCheckBox.Text = "Hook Mouse Click Event";
            this.hookMouseClickCheckBox.UseVisualStyleBackColor = true;
            this.hookMouseClickCheckBox.CheckedChanged += new System.EventHandler(this.hookMouseClickCheckBox_CheckedChanged);
            // 
            // recordButton
            // 
            this.recordButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.recordButton.Location = new System.Drawing.Point(3, 369);
            this.recordButton.Name = "recordButton";
            this.recordButton.Size = new System.Drawing.Size(75, 23);
            this.recordButton.TabIndex = 0;
            this.recordButton.Text = "Record";
            this.recordButton.UseVisualStyleBackColor = true;
            this.recordButton.Click += new System.EventHandler(this.recordButton_Click);
            // 
            // recordedEventsLabel
            // 
            this.recordedEventsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.recordedEventsLabel.AutoSize = true;
            this.recordedEventsLabel.Location = new System.Drawing.Point(327, 374);
            this.recordedEventsLabel.Name = "recordedEventsLabel";
            this.recordedEventsLabel.Size = new System.Drawing.Size(93, 13);
            this.recordedEventsLabel.TabIndex = 2;
            this.recordedEventsLabel.Text = "0 recorded events";
            // 
            // clipboardTabPage
            // 
            this.clipboardTabPage.Controls.Add(this.clipboardPropertyGrid);
            this.clipboardTabPage.Controls.Add(this.hookClipboardCheckBox);
            this.clipboardTabPage.Location = new System.Drawing.Point(4, 22);
            this.clipboardTabPage.Name = "clipboardTabPage";
            this.clipboardTabPage.Size = new System.Drawing.Size(510, 428);
            this.clipboardTabPage.TabIndex = 2;
            this.clipboardTabPage.Text = "Clipboard";
            this.clipboardTabPage.UseVisualStyleBackColor = true;
            // 
            // clipboardPropertyGrid
            // 
            this.clipboardPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clipboardPropertyGrid.Enabled = false;
            this.clipboardPropertyGrid.Location = new System.Drawing.Point(6, 33);
            this.clipboardPropertyGrid.Name = "clipboardPropertyGrid";
            this.clipboardPropertyGrid.Size = new System.Drawing.Size(498, 388);
            this.clipboardPropertyGrid.TabIndex = 1;
            // 
            // hookClipboardCheckBox
            // 
            this.hookClipboardCheckBox.AutoSize = true;
            this.hookClipboardCheckBox.Location = new System.Drawing.Point(6, 10);
            this.hookClipboardCheckBox.Name = "hookClipboardCheckBox";
            this.hookClipboardCheckBox.Size = new System.Drawing.Size(99, 17);
            this.hookClipboardCheckBox.TabIndex = 0;
            this.hookClipboardCheckBox.Text = "Hook Clipboard";
            this.hookClipboardCheckBox.UseVisualStyleBackColor = true;
            this.hookClipboardCheckBox.CheckedChanged += new System.EventHandler(this.hookClipboardCheckBox_CheckedChanged);
            // 
            // HooksForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 478);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HooksForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hooks Demo";
            this.tabControl.ResumeLayout(false);
            this.keyboardTabPage.ResumeLayout(false);
            this.keyboardTabPage.PerformLayout();
            this.keyboardHookControlsPanel.ResumeLayout(false);
            this.keyboardHookControlsPanel.PerformLayout();
            this.mouseTabPage.ResumeLayout(false);
            this.mouseTabPage.PerformLayout();
            this.mouseHookControlsPanel.ResumeLayout(false);
            this.mouseHookControlsPanel.PerformLayout();
            this.clipboardTabPage.ResumeLayout(false);
            this.clipboardTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage keyboardTabPage;
        private System.Windows.Forms.TabPage mouseTabPage;
        private System.Windows.Forms.TabPage clipboardTabPage;
        private System.Windows.Forms.Button mouseLogClearButton;
        private System.Windows.Forms.ListBox mouseLogListBox;
        private System.Windows.Forms.CheckBox hookMouseDoubleClickCheckBox;
        private System.Windows.Forms.CheckBox hookMouseUpCheckBox;
        private System.Windows.Forms.CheckBox hookMouseWheelCheckBox;
        private System.Windows.Forms.CheckBox hookMouseClickCheckBox;
        private System.Windows.Forms.CheckBox hookMouseDownCheckBox;
        private System.Windows.Forms.CheckBox hookMouseMoveCheckBox;
        private System.Windows.Forms.Button recordButton;
        private System.Windows.Forms.Label recordedEventsLabel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.PropertyGrid clipboardPropertyGrid;
        private System.Windows.Forms.CheckBox hookClipboardCheckBox;
        private System.Windows.Forms.CheckBox hookKeyboardCheckBox;
        private System.Windows.Forms.Panel keyboardHookControlsPanel;
        private System.Windows.Forms.ComboBox sendKeyComboBox;
        private System.Windows.Forms.Button sendKeyPressButton;
        private System.Windows.Forms.Button sendKeyUpButton;
        private System.Windows.Forms.Button sendKeyDownButton;
        private System.Windows.Forms.CheckBox keyUpHookCheckBox;
        private System.Windows.Forms.CheckBox keyDownHookCheckBox;
        private System.Windows.Forms.PropertyGrid lastKeyPressedPropertyGrid;
        private System.Windows.Forms.ListBox keyboardLogListBox;
        private System.Windows.Forms.CheckBox blockWinKeyCheckBox;
        private System.Windows.Forms.Button keyboardLogClearButton;
        private System.Windows.Forms.CheckBox hookMouseCheckBox;
        private System.Windows.Forms.Panel mouseHookControlsPanel;
    }
}

