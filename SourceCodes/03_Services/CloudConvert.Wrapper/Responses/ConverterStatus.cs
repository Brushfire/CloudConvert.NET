using System.Collections.Generic;

namespace Aliencube.CloudConvert.Wrapper.Responses
{
    /// <summary>
    /// Represents a converter element of a status response.
    /// </summary>
    public class ConverterStatus
    {
        /// <summary>
        /// Gets or sets the converter format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the converter type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the converter options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public Dictionary<string, string> Options { get; set; }

        /// <summary>
        /// Gets or sets the converter duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public decimal Duration { get; set; }
    }
}
