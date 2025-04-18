using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows.Forms;

namespace NetEx.WinForms.ProgressDialogDemo
{
    public partial class ProgressDialogForm : Form
    {
        #region Fields

        private bool _cancelThread;
        private Thread? _t;

        #endregion

        #region Constructor

        [SuppressMessage("ReSharper", "LocalizableElement")]
        public ProgressDialogForm()
        {
            InitializeComponent();

            progressDialog.Animation = AnimationResource.CopyFile;

#if NET20
            frameworkToolStripStatusLabel.Text = ".Net Framework 2.0";
#elif NET8_0
            frameworkToolStripStatusLabel.Text = ".Net 8.0";
#endif
        }

        #endregion

        #region Methods

        #region Event Handlers

        private void CloseButton_Click(object sender, EventArgs e)
        {
            // Close the progress dialog.
            progressDialog.Close();
        }
        private void ProgressDialog_Canceled(object sender, EventArgs e)
        {
            // Stop worker thread.
            StopThread();
        }
        private void ProgressDialog_Closed(object sender, EventArgs e)
        {
            // Stop worker thread.
            StopThread();

            // Unlock the form buttons.
            UnlockButtons();
        }
        private void ShowButton_Click(object sender, EventArgs e)
        {
            // Reset the dialog value to 0.
            progressDialog.Value = 0;

            // Lock the form buttons.
            LockButtons();

            // Start worker thread.
            StartThread();

            // Show the dialog before we start our worker thread
            // to prove that this is shown in a non-modal fashion
            // and is not a blocking call.
            progressDialog.Show(this);
        }
        private void ShowDialogButton_Click(object sender, EventArgs e)
        {
            // Reset the dialog value to 0.
            progressDialog.Value = 0;

            // Lock the form buttons.
            LockButtons();

            // Start worker thread.
            StartThread();

            // Show the dialog box after we start our worker thread
            // because calling ShowDialog is a blocking call.
            // The message box shows the dialog result of the ProgressDialog
            // (whether it completed or was canceled).
            MessageBox.Show(progressDialog.ShowDialog(this).ToString());

            // Stop worker thread.
            StopThread();

            // Unlock the form buttons.
            UnlockButtons();
        }

        #endregion

        #region Private

        private void LockButtons()
        {
            closeButton.Enabled = true;
            showButton.Enabled = false;
            showDialogButton.Enabled = false;
        }
        private void StartThread()
        {
            // Set our cancel flag to false.
            _cancelThread = false;

            // Start worker thread.
            _t = new Thread(ThreadMethod);
            _t.Start();
        }
        private void StopThread()
        {
            _cancelThread = true;

            if (_t != null)
            {
                _t.Join(500);

                if (_t.IsAlive)
                {
                    throw new Exception("Failed to end thread.");
                }
            }
        }
        [SuppressMessage("ReSharper", "LocalizableElement")]
        private void ThreadMethod()
        {
            while (!_cancelThread && progressDialog.Value < progressDialog.Maximum)
            {
                progressDialog.Value += 1;
                progressDialog.SetLine(1, "Line #1: step " + progressDialog.Value);
                progressDialog.SetLine(2, "Line #2: step " + progressDialog.Value);
                progressDialog.SetLine(3, "Line #3: step " + progressDialog.Value);
                progressDialog.Title = "Title: step " + progressDialog.Value;
                Thread.Sleep(100);
            }
        }
        private void UnlockButtons()
        {
            Invoke(new ThreadStart(delegate
            {
                closeButton.Enabled = false;
                showButton.Enabled = true;
                showDialogButton.Enabled = true;

                // Refresh the property grid
                propertyGrid.Refresh();
            }));
        }

        #endregion

        #endregion
    }
}