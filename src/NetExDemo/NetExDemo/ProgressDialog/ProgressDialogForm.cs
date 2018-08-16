using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows.Forms;

namespace NetExDemo.ProgressDialog
{
    public partial class ProgressDialogForm : Form
    {
        #region Fields

        private Thread _t;

        #endregion

        #region Constructor

        public ProgressDialogForm()
        {
            InitializeComponent();

            progressDialog.Animation = AnimationResource.CopyFile;
            progressDialog.ProgressBarStyle = ProgressDialogProgressBarStyle.Continuous;
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
            // Terminate the worker thread.
            _t.Abort();
        }
        private void ProgressDialog_Closed(object sender, EventArgs e)
        {
            // Unlock the form buttons.
            UnlockButtons();
        }
        private void ProgressDialog_Completed(object sender, EventArgs e)
        {
            // Terminate the worker thread.
            _t.Abort();
        }
        private void ShowButton_Click(object sender, EventArgs e)
        {
            // Reset the dialog value to 0.
            progressDialog.Value = 0;

            // Lock the form buttons.
            LockButtons();

            // Show the dialog before we start our worker thread
            // to prove that this is shown in a non-modal fashion
            // and is not a blocking call.
            progressDialog.Show(this);

            // Start worker thread.
            _t = new Thread(ThreadMethod);
            _t.Start();
        }
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private void ShowDialogButton_Click(object sender, EventArgs e)
        {
            // Reset the dialog value to 0.
            progressDialog.Value = 0;

            // Lock the form buttons.
            LockButtons();

            // Start worker thread.
            _t = new Thread(ThreadMethod);
            _t.Start();

            // Show the dialog box after we start our worker thread
            // because calling ShowDialog is a blocking call.
            // The messagebox shows the dialog result of the ProgressDialog
            // (whether it completed or was canceled).
            MessageBox.Show(progressDialog.ShowDialog(this).ToString());

            // Terminate the worker thread.
            _t.Abort();

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
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters")]
        [SuppressMessage("ReSharper", "LocalizableElement")]
        private void ThreadMethod()
        {
            while (progressDialog.Value < progressDialog.Maximum)
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
            }));
        }

        #endregion

        #endregion
    }
}