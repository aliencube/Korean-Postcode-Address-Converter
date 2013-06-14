using System;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Events
{
    /// <summary>
    /// This provides data for exception thrown event.
    /// </summary>
    public class ExceptionThrownEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Initialises a new instance of the ExceptionThrownEventArgs object.
        /// </summary>
        /// <param name="ex">Exception thrown.</param>
        public ExceptionThrownEventArgs(Exception ex)
        {
            this.Exception = ex;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the exception thrown.
        /// </summary>
        public Exception Exception { get; set; }
        #endregion
    }
}
