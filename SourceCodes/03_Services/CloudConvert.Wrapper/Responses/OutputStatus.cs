using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliencube.CloudConvert.Wrapper.Responses
{
    /// <summary>
    /// Represents the output status of a convert status response.
    /// </summary>
    public class OutputStatus
    {
        public string Filename { get; set; }

        public string Ext { get; set; }

        public string[] Files { get; set; }

        public long Size { get; set; }

        public string Url { get; set; }

        public int Downloads { get; set; }
    }
}
