using System;

namespace NetEx.Windows.Forms
{
    public class ClipboardUpdatedEventArgs : EventArgs
    {

        #region Variables

        private string[] _dataFormats;

        #endregion

        #region Properties

        public string[] DataFormats
        {
            get { return _dataFormats; }
        }

        #endregion

        #region Constructor
        
        public ClipboardUpdatedEventArgs(string[] DataFormats)
        {
            _dataFormats = DataFormats;
        }

        #endregion

    }
}
