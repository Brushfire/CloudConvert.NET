using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Aliencube.CloudConvert.Wrapper.Exceptions;
using Aliencube.CloudConvert.Wrapper.Extensions;
using Aliencube.CloudConvert.Wrapper.Interfaces;
using Aliencube.CloudConvert.Wrapper.Options;
using Aliencube.CloudConvert.Wrapper.Requests;
using Aliencube.CloudConvert.Wrapper.Responses;
using AutoMapper;
using Newtonsoft.Json;

namespace Aliencube.CloudConvert.Wrapper
{
    /// <summary>
    /// This represents the converter wrapper entity.
    /// </summary>
    public class ConverterWrapper : IConverterWrapper
    {
        private readonly IConverterSettings _settings;
        private readonly IMapper _mapper;

        private bool _disposed;

        /// <summary>
        /// Initialises a new instance of the <c>ConverterWrapper</c> class.
        /// </summary>
        /// <param name="settings"></param>
        public ConverterWrapper(IConverterSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this._settings = settings;
            MapperConfiguration mapperConfig = this.InitializeMapper();
            mapperConfig.AssertConfigurationIsValid();
            this._mapper = mapperConfig.CreateMapper();
        }

        /// <summary>
        /// Initializes the mapper definitions.
        /// </summary>
        private MapperConfiguration InitializeMapper()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<InputParameters, ConvertRequest>()
                    .ForMember(d => d.InputMethod, o => o.MapFrom(s => s.InputMethod.ToLower()))
                    .ForMember(d => d.Filepath, o => o.MapFrom(s => s.Filepath))
                    .ForMember(d => d.Filename, o => o.MapFrom(s => s.Filename))
                    .ForMember(d => d.Tag, o => o.MapFrom(s => s.Tag))
                    .ForMember(d => d.OutputFormat, o => o.Ignore())
                    .ForMember(d => d.ConverterOptions, o => o.Ignore())
                    .ForMember(d => d.PresetId, o => o.Ignore())
                    .ForMember(d => d.Timeout, o => o.Ignore())
                    .ForMember(d => d.Email, o => o.Ignore())
                    .ForMember(d => d.OutputStorage, o => o.Ignore())
                    .ForMember(d => d.CallbackUrl, o => o.Ignore())
                    .ForMember(d => d.Wait, o => o.Ignore())
                    .ForMember(d => d.DownloadMethod, o => o.Ignore())
                    .ForMember(d => d.SaveToServer, o => o.Ignore());
                config.CreateMap<OutputParameters, ConvertRequest>()
                    .ForMember(d => d.InputMethod, o => o.Ignore())
                    .ForMember(d => d.Filepath, o => o.Ignore())
                    .ForMember(d => d.Filename, o => o.Ignore())
                    .ForMember(d => d.Tag, o => o.Ignore())
                    .ForMember(d => d.OutputFormat, o => o.Ignore())
                    .ForMember(d => d.ConverterOptions, o => o.Ignore())
                    .ForMember(d => d.PresetId, o => o.Ignore())
                    .ForMember(d => d.Timeout, o => o.Ignore())
                    .ForMember(d => d.Email, o => o.MapFrom(s => s.Email ? s.Email : (bool?)null))
                    .ForMember(d => d.OutputStorage, o => o.MapFrom(s => s.OutputStorage != OutputStorage.None ? s.OutputStorage.ToLower() : null))
                    .ForMember(d => d.CallbackUrl, o => o.MapFrom(s => s.CallbackUrl))
                    .ForMember(d => d.Wait, o => o.MapFrom(s => s.Wait ? s.Wait : (bool?)null))
                    .ForMember(d => d.DownloadMethod, o => o.MapFrom<object>(s => s.DownloadMethod == DownloadMethod.Inline ? (object)"inline" : s.DownloadMethod == DownloadMethod.True))
                    .ForMember(d => d.SaveToServer, o => o.MapFrom(s => s.SaveToServer ? s.SaveToServer : (bool?)null));
                config.CreateMap<ConversionParameters, ConvertRequest>()
                    .ForMember(d => d.InputMethod, o => o.Ignore())
                    .ForMember(d => d.Filepath, o => o.Ignore())
                    .ForMember(d => d.Filename, o => o.Ignore())
                    .ForMember(d => d.Tag, o => o.Ignore())
                    .ForMember(d => d.OutputFormat, o => o.MapFrom(s => s.OutputFormat))
                    .ForMember(d => d.ConverterOptions, o => o.MapFrom(s => s.ConverterOptions))
                    .ForMember(d => d.PresetId, o => o.MapFrom(s => s.PresetId))
                    .ForMember(d => d.Timeout, o => o.MapFrom(s => s.Timeout > 0 ? s.Timeout : (int?)null))
                    .ForMember(d => d.Email, o => o.Ignore())
                    .ForMember(d => d.OutputStorage, o => o.Ignore())
                    .ForMember(d => d.CallbackUrl, o => o.Ignore())
                    .ForMember(d => d.Wait, o => o.Ignore())
                    .ForMember(d => d.DownloadMethod, o => o.Ignore())
                    .ForMember(d => d.SaveToServer, o => o.Ignore()); ;
            });
        }

        /// <summary>
        /// Converts the requested file to a designated format.
        /// </summary>
        /// <param name="input"><c>InputParameters</c> object.</param>
        /// <param name="output"><c>OutputParameters</c> object.</param>
        /// <param name="conversion"><c>ConversionParameters</c> object.</param>
        /// <returns>Returns the conversion response.</returns>
        public async Task<ConvertResponse> ConvertAsync(InputParameters input, OutputParameters output, ConversionParameters conversion)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            if (conversion == null)
            {
                throw new ArgumentNullException("conversion");
            }

            var processResponse = await this.GetProcessResponseAsync(input.InputFormat, conversion.OutputFormat);
            var convertRequest = this.GetConvertRequest(input, output, conversion);
            var convertResponse = await this.ConvertAsync(convertRequest, String.Format("https:{0}", processResponse.Url));
            return convertResponse;
        }

        /// <summary>
        /// Gets the <c>ProcessResponse</c> to be consumed in subsequent requests.
        /// </summary>
        /// <param name="inputFormat">Input file format.</param>
        /// <param name="outputFormat">Output file format.</param>
        /// <returns>Returns the <c>ProcessResponse</c> object to be consumed in subsequent requests.</returns>
        public async Task<ProcessResponse> GetProcessResponseAsync(string inputFormat, string outputFormat)
        {
            if (String.IsNullOrWhiteSpace(inputFormat))
            {
                throw new ArgumentNullException("inputFormat");
            }

            if (String.IsNullOrWhiteSpace(outputFormat))
            {
                throw new ArgumentNullException("outputFormat");
            }

            var request = this.GetProcessRequest(inputFormat, outputFormat);

            ProcessResponse deserialised;
            using (var client = new HttpClient())
            {
                var apiKey = this._settings.Basic.ApiKey.Value;
                var processUrl = this._settings.Basic.ProcessUrl;

                if (this._settings.Basic.UseHeader)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                }
                else
                {
                    request.ApiKey = apiKey;
                }

                var serialised = this.Serialise(request);
                using (var content = new StringContent(serialised, Encoding.UTF8, "application/json"))
                using (var response = await client.PostAsync(processUrl, content))
                {
                    var result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var error = this.Deserialise<ErrorResponse>(result);
                        throw new ErrorResponseException(error);
                    }
                    deserialised = this.Deserialise<ProcessResponse>(result);
                }
            }

            return deserialised;
        }

        /// <summary>
        /// Gets the <c>ProcessRequest</c> object.
        /// </summary>
        /// <param name="inputFormat">Input file format.</param>
        /// <param name="outputFormat">Output file format.</param>
        /// <returns>Returns the <c>ProcessRequest</c> object.</returns>
        private ProcessRequest GetProcessRequest(string inputFormat, string outputFormat)
        {
            if (String.IsNullOrWhiteSpace(inputFormat))
            {
                throw new ArgumentNullException("inputFormat");
            }

            if (String.IsNullOrWhiteSpace(outputFormat))
            {
                throw new ArgumentNullException("outputFormat");
            }

            var request = new ProcessRequest()
                          {
                              InputFormat = inputFormat,
                              OutputFormat = outputFormat
                          };
            return request;
        }

        /// <summary>
        /// Gets the <c>ConvertRequest</c> object.
        /// </summary>
        /// <param name="input"><c>InputParameters</c> object.</param>
        /// <param name="output"><c>OutputParameters</c> object.</param>
        /// <param name="conversion"><c>ConversionParameters</c> object.</param>
        /// <returns>Returns the <c>ConvertRequest</c> object.</returns>
        public ConvertRequest GetConvertRequest(InputParameters input, OutputParameters output, ConversionParameters conversion)
        {
            var request = this._mapper.Map<ConvertRequest>(input);
            this._mapper.Map(output, request);
            this._mapper.Map(conversion, request);
            return request;
        }

        /// <summary>
        /// Converts the requested file to a designated format.
        /// </summary>
        /// <param name="request"><c>ConvertRequest</c> object.</param>
        /// <param name="convertUrl">Process URL to convert.</param>
        /// <returns></returns>
        public async Task<ConvertResponse> ConvertAsync(ConvertRequest request, string convertUrl)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (String.IsNullOrWhiteSpace(convertUrl))
            {
                throw new ArgumentNullException("convertUrl");
            }

            var serialised = this.Serialise(request);
            ConvertResponse deserialised;

            using (var client = new HttpClient())
            using (var content = new StringContent(serialised, Encoding.UTF8, "application/json"))
            using (var response = await client.PostAsync(convertUrl, content))
            {
                var result = await response.Content.ReadAsStringAsync();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var error = this.Deserialise<ErrorResponse>(result);
                    throw new ErrorResponseException(error);
                }

                deserialised = this.Deserialise<ConvertResponse>(result);
                deserialised.Code = (int)response.StatusCode;
            }

            return deserialised;
        }

        /// <summary>
        /// Asynchronously gets the status of the conversion.
        /// </summary>
        /// <param name="statusUrl">The status URL.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">statusUrl</exception>
        /// <exception cref="ErrorResponseException"></exception>
        public async Task<ConversionStatusResponse> GetConversionStatusAsync(string statusUrl)
        {
            if (string.IsNullOrEmpty(statusUrl))
            {
                throw new ArgumentNullException("statusUrl");
            }

            ConversionStatusResponse deserialised;
            using (var client = new HttpClient())
            {                
                using (var response = await client.GetAsync(statusUrl))
                {
                    var result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var error = this.Deserialise<ErrorResponse>(result);
                        throw new ErrorResponseException(error);
                    }

                    deserialised = this.Deserialise<ConversionStatusResponse>(result);
                    deserialised.Code = (int)response.StatusCode;
                }
            }

            return deserialised;
        }

        /// <summary>
        /// Asynchronously delete the conversion at the specified URL.
        /// </summary>
        /// <param name="deleteUrl">The conversion delete URL.</param>
        /// <returns></returns>
        public async Task<DeleteConvertResponse> DeleteConversionAsync(string deleteUrl)
        {
            if (string.IsNullOrEmpty(deleteUrl))
            {
                throw new ArgumentNullException("deleteUrl");
            }

            DeleteConvertResponse deserialised;
            using (var client = new HttpClient())
            {
                using (var response = await client.DeleteAsync(deleteUrl))
                {
                    var result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var error = this.Deserialise<ErrorResponse>(result);
                        throw new ErrorResponseException(error);
                    }

                    deserialised = this.Deserialise<DeleteConvertResponse>(result);
                    deserialised.Code = (int)response.StatusCode;
                }
            }

            return deserialised;
        }
        
        /// <summary>
        /// Serialises the request object in JSON format.
        /// </summary>
        /// <typeparam name="TRequest">Request object type.</typeparam>
        /// <param name="request">Request object.</param>
        /// <returns>Returns the JSON-serialised string.</returns>
        public string Serialise<TRequest>(TRequest request) where TRequest : BaseRequest
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var settings = new JsonSerializerSettings()
                           {
                               ContractResolver = new LowercaseContractResolver()
                           };
            var serialised = JsonConvert.SerializeObject(request, Formatting.None, settings);
            return serialised;
        }

        /// <summary>
        /// Deserialises the JSON serialised string into a designated type.
        /// </summary>
        /// <typeparam name="TResponse">Response object type.</typeparam>
        /// <param name="value">JSON serialised string.</param>
        /// <returns>Returns a deserialised object.</returns>
        public TResponse Deserialise<TResponse>(string value) where TResponse : BaseResponse
        {
            var deserialised = JsonConvert.DeserializeObject<TResponse>(value);
            return deserialised;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this._disposed)
            {
                return;
            }

            this._disposed = true;
        }
    }
}