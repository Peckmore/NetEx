using NetEx.Hooks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using KeyEventArgs = NetEx.Hooks.KeyEventArgs;
using Keys = NetEx.Hooks.Keys;
using MouseEventArgs = NetEx.Hooks.MouseEventArgs;

namespace NetEx.Demo.Hooks
{
    public partial class HooksForm : Form
    {
        #region Fields

        private List<KeyValuePair<int, MouseEventArgs>> _mouseHistory = new List<KeyValuePair<int, MouseEventArgs>>();
        private DateTime _mouseHistoryBegin;

        #endregion

        #region Construction

        public HooksForm()
        {
            InitializeComponent();

            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                sendKeyComboBox.Items.Add(key);
            }

            ClipboardHook.ClipboardUpdated += ClipboardHook_ClipboardUpdated;
        }

        #endregion

        #region Methods

        #region Protected

        protected override void OnClosed(EventArgs e)
        {
            ClipboardHook.TryUninstall();
            KeyboardHook.TryUninstall();
            MouseHook.TryUninstall();

            ClipboardHook.ClipboardUpdated -= ClipboardHook_ClipboardUpdated;

            base.OnClosed(e);
        }

        #endregion

        #region Clipboard

        private void ClipboardHook_ClipboardUpdated(ClipboardUpdatedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(ClipboardHook_ClipboardUpdated, e);
            }
            else
            {
                var obj = Clipboard.GetDataObject();
                clipboardPropertyGrid.SelectedObject = Clipboard.GetDataObject()?.GetFormats();
            }
        }
        private void hookClipboardCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (hookClipboardCheckBox.Checked)
            {
                if (ClipboardHook.TryInstall())
                {
                    clipboardPropertyGrid.Enabled = true;
                }
                else
                { 
                    MessageBox.Show("Clipboard Hook failed to install.");
                    hookClipboardCheckBox.Checked = false;
                }
            }
            else
            {
                ClipboardHook.TryUninstall();
                clipboardPropertyGrid.SelectedObject = null;
                clipboardPropertyGrid.Enabled = false;
            }
        }

        #endregion

        #region Keyboard

        private void hookKeyboardCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (hookKeyboardCheckBox.Checked)
            {
                if (KeyboardHook.TryInstall())
                {
                    keyboardHookControlsPanel.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Keyboard Hook failed to install.");
                    hookKeyboardCheckBox.Checked = false;
                }
            }
            else
            {
                KeyboardHook.TryUninstall();
                lastKeyPressedPropertyGrid.SelectedObject = null;
                keyboardHookControlsPanel.Enabled = false;
            }
        }
        private void HookKeys_KeyDown(KeyEventArgs e)
        {
            if (blockWinKeyCheckBox.Checked && (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin))
            {
                e.SuppressKeyPress = true;
            }

            keyboardLogListBox.Items.Add(string.Format("Key Down:\t{0}", e.KeyCode.ToString()));
            keyboardLogListBox.SelectedIndex = keyboardLogListBox.Items.Count - 1;
        }
        private void HookKeys_KeyUp(KeyEventArgs e)
        {
            keyboardLogListBox.Items.Add(string.Format("Key Up:\t\t{0}", e.KeyCode.ToString()));
            keyboardLogListBox.SelectedIndex = keyboardLogListBox.Items.Count - 1;
            lastKeyPressedPropertyGrid.SelectedObject = e;
        }
        private void keyboardLogClearButton_Click(object sender, EventArgs e)
        {
            keyboardLogListBox.Items.Clear();
        }
        private void keyDownHookCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (keyDownHookCheckBox.Checked)
            {
                KeyboardHook.KeyDown += HookKeys_KeyDown;
            }
            else
            {
                KeyboardHook.KeyDown -= HookKeys_KeyDown;
            }

            blockWinKeyCheckBox.Enabled = keyDownHookCheckBox.Checked;
        }
        private void keyUpHookCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (keyUpHookCheckBox.Checked)
            {
                KeyboardHook.KeyUp += HookKeys_KeyUp;
            }
            else
            {
                KeyboardHook.KeyUp -= HookKeys_KeyUp;
            }

            lastKeyPressedPropertyGrid.Enabled = keyUpHookCheckBox.Checked;
        }
        private void sendKeyDownButton_Click(object sender, EventArgs e)
        {
            if (sendKeyComboBox.SelectedItem != null)
            {
                KeyboardSimulator.KeyDown((Keys)sendKeyComboBox.SelectedItem);
            }
        }
        private void sendKeyPressButton_Click(object sender, EventArgs e)
        {
            if (sendKeyComboBox.SelectedItem != null)
            {
                KeyboardSimulator.KeyPress((Keys)sendKeyComboBox.SelectedItem);
            }
        }
        private void sendKeyUpButton_Click(object sender, EventArgs e)
        {
            if (sendKeyComboBox.SelectedItem != null)
            {
                KeyboardSimulator.KeyUp((Keys)sendKeyComboBox.SelectedItem);
            }
        }

        #endregion

        #region Mouse

        private void hookMouseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (hookMouseCheckBox.Checked)
            {
                if (MouseHook.TryInstall())
                {
                    mouseHookControlsPanel.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Mouse Hook failed to install.");
                    hookMouseCheckBox.Checked = false;
                }
            }
            else
            {
                MouseHook.TryUninstall();
                lastKeyPressedPropertyGrid.SelectedObject = null;
                mouseHookControlsPanel.Enabled = false;
            }
        }
        private void hookMouseMoveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (hookMouseMoveCheckBox.Checked)
            {
                MouseHook.MouseMove += HookMouse_MouseMove;
            }
            else
            {
                MouseHook.MouseMove -= HookMouse_MouseMove;
            }
        }
        private void hookMouseWheelCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (hookMouseWheelCheckBox.Checked)
            {
                MouseHook.MouseWheel += HookMouse_MouseWheel;
            }
            else
            {
                MouseHook.MouseWheel -= HookMouse_MouseWheel;
            }
        }
        private void hookMouseDownCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (hookMouseDownCheckBox.Checked)
            {
                MouseHook.MouseDown += HookMouse_MouseDown;
            }
            else
            {
                MouseHook.MouseDown -= HookMouse_MouseDown;
            }
        }
        private void hookMouseUpCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (hookMouseUpCheckBox.Checked)
            {
                MouseHook.MouseUp += HookMouse_MouseUp;
            }
            else
            {
                MouseHook.MouseUp -= HookMouse_MouseUp;
            }
        }
        private void hookMouseClickCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (hookMouseClickCheckBox.Checked)
            {
                MouseHook.MouseClick += HookMouse_MouseClick;
            }
            else
            {
                MouseHook.MouseClick -= HookMouse_MouseClick;
            }
        }
        private void hookMouseDoubleClickCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (hookMouseDoubleClickCheckBox.Checked)
            {
                MouseHook.MouseDoubleClick += HookMouse_MouseDoubleClick;
            }
            else
            {
                MouseHook.MouseDoubleClick -= HookMouse_MouseDoubleClick;
            }
        }
        private void HookMouse_MouseMove(MouseEventArgs e)
        {
            mouseLogListBox.Items.Add(string.Format("Move:\tButtons:{0}\tLocation:{1}\tDelta:{2}\tClick:{3}", e.Button.ToString(), e.Location.ToString(), e.Delta.ToString(), e.Clicks.ToString()));
            mouseLogListBox.SelectedIndex = mouseLogListBox.Items.Count - 1;
        }
        private void HookMouse_MouseWheel(MouseEventArgs e)
        {
            mouseLogListBox.Items.Add(string.Format("Wheel:\tButtons:{0}\tLocation:{1}\tDelta:{2}\tClick:{3}", e.Button.ToString(), e.Location.ToString(), e.Delta.ToString(), e.Clicks.ToString()));
            mouseLogListBox.SelectedIndex = mouseLogListBox.Items.Count - 1;
        }
        private void HookMouse_MouseDown(MouseEventArgs e)
        {
            mouseLogListBox.Items.Add(string.Format("Down:\tButtons:{0}\tLocation:{1}\tDelta:{2}\tClick:{3}", e.Button.ToString(), e.Location.ToString(), e.Delta.ToString(), e.Clicks.ToString()));
            mouseLogListBox.SelectedIndex = mouseLogListBox.Items.Count - 1;
        }
        private void HookMouse_MouseUp(MouseEventArgs e)
        {
            mouseLogListBox.Items.Add(string.Format("Up:\tButtons:{0}\tLocation:{1}\tDelta:{2}\tClick:{3}", e.Button.ToString(), e.Location.ToString(), e.Delta.ToString(), e.Clicks.ToString()));
            mouseLogListBox.SelectedIndex = mouseLogListBox.Items.Count - 1;
        }
        private void HookMouse_MouseClick(MouseEventArgs e)
        {
            mouseLogListBox.Items.Add(string.Format("Click:\tButtons:{0}\tLocation:{1}\tDelta:{2}\tClick:{3}", e.Button.ToString(), e.Location.ToString(), e.Delta.ToString(), e.Clicks.ToString()));
            mouseLogListBox.SelectedIndex = mouseLogListBox.Items.Count - 1;
        }
        private void HookMouse_MouseDoubleClick(MouseEventArgs e)
        {
            mouseLogListBox.Items.Add(string.Format("Double Click:\tButtons:{0}\tLocation:{1}\tDelta:{2}\tClick:{3}", e.Button.ToString(), e.Location.ToString(), e.Delta.ToString(), e.Clicks.ToString()));
            mouseLogListBox.SelectedIndex = mouseLogListBox.Items.Count - 1;
        }
        private void mouseLogClearButton_Click(object sender, EventArgs e)
        {
            mouseLogListBox.Items.Clear();
        }

        #region Record Mouse

        private void Start()
        {
            hookMouseMoveCheckBox.Checked = false;
            hookMouseWheelCheckBox.Checked = false;
            hookMouseDownCheckBox.Checked = false;
            hookMouseUpCheckBox.Checked = false;
            hookMouseClickCheckBox.Checked = false;
            hookMouseDoubleClickCheckBox.Checked = false;
            hookMouseMoveCheckBox.Enabled = false;
            hookMouseWheelCheckBox.Enabled = false;
            hookMouseDownCheckBox.Enabled = false;
            hookMouseUpCheckBox.Enabled = false;
            hookMouseClickCheckBox.Enabled = false;
            hookMouseDoubleClickCheckBox.Enabled = false;
            _mouseHistoryBegin = DateTime.Now;
            MouseHook.MouseMove +=RecordMouse;
            recordButton.Enabled = false;
            playButton.Enabled = false;
            stopButton.Enabled = true;
        }
        private void Stop()
        {
            hookMouseMoveCheckBox.Enabled = true;
            hookMouseWheelCheckBox.Enabled = true;
            hookMouseDownCheckBox.Enabled = true;
            hookMouseUpCheckBox.Enabled = true;
            hookMouseClickCheckBox.Enabled = true;
            hookMouseDoubleClickCheckBox.Enabled = true;
            MouseHook.MouseMove -= RecordMouse;
            recordButton.Enabled = true;
            playButton.Enabled = true;
            stopButton.Enabled = false;
        }
        private void Play()
        {
            MouseHook.Uninstall();
            hookMouseMoveCheckBox.Checked = false;
            hookMouseWheelCheckBox.Checked = false;
            hookMouseDownCheckBox.Checked = false;
            hookMouseUpCheckBox.Checked = false;
            hookMouseClickCheckBox.Checked = false;
            hookMouseDoubleClickCheckBox.Checked = false;
            hookMouseMoveCheckBox.Enabled = false;
            hookMouseWheelCheckBox.Enabled = false;
            hookMouseDownCheckBox.Enabled = false;
            hookMouseUpCheckBox.Enabled = false;
            hookMouseClickCheckBox.Enabled = false;
            hookMouseDoubleClickCheckBox.Enabled = false;
            playButton.Enabled = false;
            recordButton.Enabled = false;
            stopButton.Enabled = false;
            clearButton.Enabled = false;
            foreach (KeyValuePair<int, MouseEventArgs> item in _mouseHistory)
            {
                Thread.Sleep(item.Key);
                MouseSimulator.MouseMove(item.Value.Location, MouseCoordinateMapping.Absolute);
            }
            playButton.Enabled = true;
            recordButton.Enabled = true;
            stopButton.Enabled = false;
            clearButton.Enabled = true;
            hookMouseMoveCheckBox.Enabled = true;
            hookMouseWheelCheckBox.Enabled = true;
            hookMouseDownCheckBox.Enabled = true;
            hookMouseUpCheckBox.Enabled = true;
            hookMouseClickCheckBox.Enabled = true;
            hookMouseDoubleClickCheckBox.Enabled = true;
            MouseHook.Install();
        }
        private void RecordMouse(MouseEventArgs e)
        {
            _mouseHistory.Add(new KeyValuePair<int, MouseEventArgs>((DateTime.Now - _mouseHistoryBegin).Milliseconds, e));
            _mouseHistoryBegin = DateTime.Now;
            recordedEventsLabel.Text = string.Format("{0} events recorded", _mouseHistory.Count);
            if (_mouseHistory.Count > 10000)
            {
                Stop();
            }
        }
        private void recordButton_Click(object sender, EventArgs e)
        {
            Start();
        }
        private void stopButton_Click(object sender, EventArgs e)
        {
            Stop();
        }
        private void clearButton_Click(object sender, EventArgs e)
        {
            _mouseHistory.Clear();
            recordedEventsLabel.Text = string.Format("{0} events recorded", _mouseHistory.Count);
        }
        private void playButton_Click(object sender, EventArgs e)
        {
            Play();
        }

        #endregion

        #endregion

        #endregion
    }
}