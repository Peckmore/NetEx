using System;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using NetEx.Hooks;
using NetEx.Windows.Forms;
using Keys = NetEx.Hooks.Keys;
using KeyEventArgs = NetEx.Hooks.KeyEventArgs;
using MouseEventArgs = NetEx.Hooks.MouseEventArgs;

namespace Tcr.HooksDemo
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
         
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
                sendKeyComboBox.Items.Add(key);

            KeyboardHook.Install();
            MouseHook.Install();
        }

        #region Keyboard Tab

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
        private void HookKeys_KeyDown(KeyEventArgs e)
        {
            if (blockWinKeyCheckBox.Checked && (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin))
            {
                e.SuppressKeyPress = true;
            }

            keyboardLogListBox.Items.Add(String.Format("Key Down:\t{0}", e.KeyCode.ToString()));
            keyboardLogListBox.SelectedIndex = keyboardLogListBox.Items.Count - 1;
        }
        private void HookKeys_KeyUp(KeyEventArgs e)
        {
            keyboardLogListBox.Items.Add(String.Format("Key Up:\t\t{0}", e.KeyCode.ToString()));
            keyboardLogListBox.SelectedIndex = keyboardLogListBox.Items.Count - 1;
            lastKeyPressedPropertyGrid.SelectedObject = e;
        }
        private void sendKeyDownButton_Click(object sender, EventArgs e)
        {
            KeyboardSimulator.KeyDown((Keys)sendKeyComboBox.SelectedItem);
        }
        private void sendKeyPressButton_Click(object sender, EventArgs e)
        {
            KeyboardSimulator.KeyPress((Keys)sendKeyComboBox.SelectedItem);
        }
        private void sendKeyUpButton_Click(object sender, EventArgs e)
        {
            KeyboardSimulator.KeyUp((Keys)sendKeyComboBox.SelectedItem);
        }

        #endregion

        #region Mouse Tab

        private void mouseLogClearButton_Click(object sender, EventArgs e)
        {
            mouseLogListBox.Items.Clear();
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
            mouseLogListBox.Items.Add(String.Format("Move:\tButtons:{0}\tLocation:{1}\tDelta:{2}\tClick:{3}", e.Button.ToString(), e.Location.ToString(), e.Delta.ToString(), e.Clicks.ToString()));
            mouseLogListBox.SelectedIndex = mouseLogListBox.Items.Count - 1;
        }
        private void HookMouse_MouseWheel(MouseEventArgs e)
        {
            mouseLogListBox.Items.Add(String.Format("Wheel:\tButtons:{0}\tLocation:{1}\tDelta:{2}\tClick:{3}", e.Button.ToString(), e.Location.ToString(), e.Delta.ToString(), e.Clicks.ToString()));
            mouseLogListBox.SelectedIndex = mouseLogListBox.Items.Count - 1;
        }
        private void HookMouse_MouseDown(MouseEventArgs e)
        {
            mouseLogListBox.Items.Add(String.Format("Down:\tButtons:{0}\tLocation:{1}\tDelta:{2}\tClick:{3}", e.Button.ToString(), e.Location.ToString(), e.Delta.ToString(), e.Clicks.ToString()));
            mouseLogListBox.SelectedIndex = mouseLogListBox.Items.Count - 1;
        }
        private void HookMouse_MouseUp(MouseEventArgs e)
        {
            mouseLogListBox.Items.Add(String.Format("Up:\tButtons:{0}\tLocation:{1}\tDelta:{2}\tClick:{3}", e.Button.ToString(), e.Location.ToString(), e.Delta.ToString(), e.Clicks.ToString()));
            mouseLogListBox.SelectedIndex = mouseLogListBox.Items.Count - 1;
        }
        private void HookMouse_MouseClick(MouseEventArgs e)
        {
            mouseLogListBox.Items.Add(String.Format("Click:\tButtons:{0}\tLocation:{1}\tDelta:{2}\tClick:{3}", e.Button.ToString(), e.Location.ToString(), e.Delta.ToString(), e.Clicks.ToString()));
            mouseLogListBox.SelectedIndex = mouseLogListBox.Items.Count - 1;
        }
        private void HookMouse_MouseDoubleClick(MouseEventArgs e)
        {
            mouseLogListBox.Items.Add(String.Format("Double Click:\tButtons:{0}\tLocation:{1}\tDelta:{2}\tClick:{3}", e.Button.ToString(), e.Location.ToString(), e.Delta.ToString(), e.Clicks.ToString()));
            mouseLogListBox.SelectedIndex = mouseLogListBox.Items.Count - 1;
        }

        #region Record Mouse

        private List<KeyValuePair<Int32, MouseEventArgs>> _mouseHistory = new List<KeyValuePair<Int32, MouseEventArgs>>();
        private DateTime _mouseHistoryBegin;

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
            foreach (KeyValuePair<Int32, MouseEventArgs> item in _mouseHistory)
            {
                Thread.Sleep(item.Key);
                MouseSimulator.MouseMove(item.Value.Location, MouseCoordinateMapping.PrimaryMonitor);
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
            _mouseHistory.Add(new KeyValuePair<Int32, MouseEventArgs>((DateTime.Now - _mouseHistoryBegin).Milliseconds, e));
            _mouseHistoryBegin = DateTime.Now;
            recordedEventsLabel.Text = String.Format("{0} events recorded", _mouseHistory.Count);
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
            recordedEventsLabel.Text = String.Format("{0} events recorded", _mouseHistory.Count);
        }
        private void playButton_Click(object sender, EventArgs e)
        {
            Play();
        }

        #endregion

        #endregion

        #region Clipboard Tab

        private void hookClipboardCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //if (hookClipboardCheckBox.Checked)
            //{
            //    ClipboardEx.ClipboardChanged += ClipboardEx_ClipboardChanged;
            //}
            //else
            //{
            //    ClipboardEx.ClipboardChanged -= ClipboardEx_ClipboardChanged;
            //}
        }
        //void ClipboardEx_ClipboardChanged(object sender, ClipboardUpdatedEventArgs e)
        //{
        //    clipboardPropertyGrid.SelectedObject = e;
        //}

        #endregion

    }
}
