using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Events
{
    public class StatusProgress
    {
        public StatusProgress(string filename = null)
        {
            this.Filename = filename;
        }

        public string Filename { get; set; }
        public long CurrentState { get; set; }
        public long MaximumState { get; set; }
        public int PercentageState { get; set; }
    }
}
