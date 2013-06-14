using System;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Events
{
    /// <summary>
    /// This provides data for status changed event.
    /// </summary>
    public class StatusChangedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Initialises a new instance of the StatusChangedEventArgs object.
        /// </summary>
        /// <param name="statusMessage">Status message.</param>
        public StatusChangedEventArgs(string statusMessage = null)
        {
            this.StatusMessage = statusMessage;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        public string StatusMessage { get; set; }
        #endregion
    }
}
