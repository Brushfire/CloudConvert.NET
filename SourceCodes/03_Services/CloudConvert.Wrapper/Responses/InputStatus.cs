using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliencube.CloudConvert.Wrapper.Responses
{
    /// <summary>
    /// Represents an input element of a convert status response.
    /// </summary>
    public class InputStatus
    {
        /// <summary>
        /// Gets or sets the input type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the input filename.
        /// </summary>
        /// <value>
        /// The filename.
        /// </value>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the input size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the input name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the input extension.
        /// </summary>
        /// <value>
        /// The extension.
        /// </value>
        public string Ext { get; set; }
    }
}
