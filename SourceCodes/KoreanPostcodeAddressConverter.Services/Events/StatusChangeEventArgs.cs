using System;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Events
{
    /// <summary>
    /// This provides data for status change event.
    /// </summary>
    public class StatusChangeEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Initialises a new instance of the StatusChangeEventArge object.
        /// </summary>
        /// <param name="statusMessage">Status message.</param>
        public StatusChangeEventArgs(string statusMessage = null)
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
