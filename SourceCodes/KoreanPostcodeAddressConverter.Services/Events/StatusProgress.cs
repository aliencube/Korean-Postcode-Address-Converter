using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Events
{
    /// <summary>
    /// This represents the status progress entity.
    /// </summary>
    public class StatusProgress
    {
        /// <summary>
        /// Initialises a new instance of the StatusProgress object.
        /// </summary>
        /// <param name="filename">Filename.</param>
        public StatusProgress(string filename = null)
        {
            this.Filename = filename;
        }

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the current state.
        /// </summary>
        public long CurrentState { get; set; }

        /// <summary>
        /// Gets or sets the maximun state.
        /// </summary>
        public long MaximumState { get; set; }

        /// <summary>
        /// Gets or sets the current state as percentage value.
        /// </summary>
        public int PercentageState { get; set; }
    }
}
