namespace Aliencube.CloudConvert.Wrapper.Responses
{
    /// <summary>
    /// This represents the convert response entity.
    /// </summary>
    public class ConvertResponse : BaseResponse
    {
        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the process/status URL.
        /// </summary>
        /// <value>
        /// The status URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the process identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the conversion percentage.
        /// </summary>
        /// <value>
        /// The percent.
        /// </value>
        public string Percent { get; set; }

        /// <summary>
        /// Gets or sets the conversion message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the conversion step.
        /// </summary>
        /// <value>
        /// The step.
        /// </value>
        public ConversionStep Step { get; set; }

        /// <summary>
        /// Gets or sets the conversion start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public long StartTime { get; set; }

        /// <summary>
        /// Gets or sets the conversion end time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public long? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the conversion expiration time.
        /// </summary>
        /// <value>
        /// The expiration time.
        /// </value>
        public long Expire { get; set; }

        /// <summary>
        /// Gets or sets the input status.
        /// </summary>
        /// <value>
        /// The input status.
        /// </value>
        public InputStatus Input { get; set; }

        /// <summary>
        /// Gets or sets the converter status.
        /// </summary>
        /// <value>
        /// The converter status.
        /// </value>
        public ConverterStatus Converter { get; set; }

        /// <summary>
        /// Gets or sets the output status.
        /// </summary>
        /// <value>
        /// The output status.
        /// </value>
        public OutputStatus Output { get; set; }
    }
}