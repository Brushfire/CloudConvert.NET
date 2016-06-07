using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliencube.CloudConvert.Wrapper.Responses
{
    /// <summary>
    /// Represents steps that a conversion process can pass through.
    /// </summary>
    public enum ConversionStep
    {
        /// <summary>
        /// Input file is downloaded, e.g. from a URL or S3 storage.
        /// </summary>
        Input,
        /// <summary>
        /// Conversion has to wait for some reason.  Happens only in very special cases.
        /// </summary>
        Wait,
        /// <summary>
        /// The actual conversion takes place.
        /// </summary>
        Convert,
        /// <summary>
        /// The output file is uploaded, e.g. to S3, Google Drive or Dropbox.
        /// </summary>
        Output,
        /// <summary>
        /// The errorSomething went wrong.  Message property contains an error description.
        /// </summary>
        Error,
        /// <summary>
        /// We are done!
        /// </summary>
        Finished
    }
}
