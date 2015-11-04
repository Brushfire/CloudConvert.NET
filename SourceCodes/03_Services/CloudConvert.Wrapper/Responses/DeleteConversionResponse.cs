namespace Aliencube.CloudConvert.Wrapper.Responses
{
    /// <summary>
    /// Represents a response from a delete conversion request.
    /// </summary>
    public class DeleteConvertResponse : BaseResponse
    {
        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
    }
}