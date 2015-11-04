using System;
using System.Collections.Generic;
using Aliencube.CloudConvert.Wrapper;
using Aliencube.CloudConvert.Wrapper.DataFormats;
using Aliencube.CloudConvert.Wrapper.Exceptions;
using Aliencube.CloudConvert.Wrapper.Extensions;
using Aliencube.CloudConvert.Wrapper.Interfaces;
using Aliencube.CloudConvert.Wrapper.Options;
using Aliencube.CloudConvert.Wrapper.Responses;
using FluentAssertions;
using NUnit.Framework;
using System.Threading;

namespace Aliencube.CloudConvert.Tests
{
    [TestFixture]
    public class ConverterWrapperTest
    {
        private IConverterSettings _settings;
        private IFormats _formats;
        private IConverterWrapper _wrapper;
        private InputParameters _input;
        private OutputParameters _output;
        private ConversionParameters _conversion;

        [SetUp]
        public void Init()
        {
            // MAKE SURE before you run this test, you MUST change the API key to yours; otherwise the test fails.
            this._settings = ConverterSettings.CreateInstance();
            this._formats = new Formats();
            this._wrapper = new ConverterWrapper(this._settings);

            this.SetConversionParametersToDefaults();
        }

        [TearDown]
        public void Cleanup()
        {
            if (this._wrapper != null)
            {
                this._wrapper.Dispose();
            }

            if (this._formats != null)
            {
                this._formats.Dispose();
            }

            if (this._settings != null)
            {
                this._settings.Dispose();
            }
        }

        [Test]
        public async void GetProcessId_GivenApiKey_ReturnResult()
        {
            try
            {
                var response = await this._wrapper.GetProcessResponseAsync(this._formats.Document.Md, this._formats.Document.Docx);
                response.Id.Should().NotBeNullOrEmpty();
            }
            catch (Exception ex)
            {
                var error = ex as ErrorResponseException;
                error.Should().NotBeNull();

                var response = error.Error;
                response.Should().BeOfType<ErrorResponse>();
                response.Code.Should().NotBe(200);
            }
        }

        [Test]
        public void GetConvertRequest_GivenParameters_ReturnConvertRequest()
        {
            var conversion = new ConversionParameters()
                             {
                                 OutputFormat = this._formats.Document.Docx,
                                 ConverterOptions = new MarkdownConverterOptions()
                                                    {
                                                        InputMarkdownSyntax = MarkdownSyntaxType.Auto
                                                    },
                             };
            var request = this._wrapper.GetConvertRequest(this._input, this._output, conversion);
            request.InputMethod.Should().Be(InputMethod.Download.ToLower());
            request.OutputStorage.Should().Be(OutputStorage.OneDrive.ToLower());

            var serialised1 = this._wrapper.Serialise(request);
            serialised1.Should().Contain("input_markdown_syntax");

            conversion = new ConversionParameters()
                         {
                             OutputFormat = this._formats.Document.Docx,
                             ConverterOptions = new Dictionary<string, object>()
                                                {
                                                    { "input_markdown_syntax", MarkdownSyntaxType.Auto },
                                                },
                         };
            request = this._wrapper.GetConvertRequest(this._input, this._output, conversion);
            request.InputMethod.Should().Be(InputMethod.Download.ToLower());
            request.OutputStorage.Should().Be(OutputStorage.OneDrive.ToLower());

            var serialised2 = this._wrapper.Serialise(request);
            serialised2.Should().Contain("input_markdown_syntax");

            serialised1.Should().BeEquivalentTo(serialised2);
        }

        [Test]
        public async void GetConverted_GivenParameters_ReturnConverted()
        {
            try
            {
                var response = await this._wrapper.ConvertAsync(this._input, this._output, this._conversion);
                response.Code.Should().Be(200);
                response.Url.Should().NotBeNullOrWhiteSpace();
            }
            catch (Exception ex)
            {
                var error = ex as ErrorResponseException;
                error.Should().NotBeNull();

                var response = error.Error;
                response.Should().BeOfType<ErrorResponse>();
                response.Code.Should().NotBe(200);
            }
        }

        [Test]
        public async void GetConversionStatus_GivenParameters_ReturnStatus()
        {
            try
            {
                ConvertResponse convertResponse = await this._wrapper.ConvertAsync(this._input, this._output, this._conversion);
                convertResponse.Code.Should().Be(200);
                convertResponse.Url.Should().NotBeNullOrWhiteSpace();

                Thread.Sleep(2000);

                ConversionStatusResponse statusResponse = await this._wrapper.GetConversionStatusAsync("https:" + convertResponse.Url);
                statusResponse.Code.Should().Be(200);
            }
            catch (Exception ex)
            {
                var error = ex as ErrorResponseException;
                error.Should().NotBeNull();

                var response = error.Error;
                response.Should().BeOfType<ErrorResponse>();
                response.Code.Should().NotBe(200);
            }
        }

        [Test]
        public async void DeleteConversion_GivenParameters_ReturnDelete()
        {
            try
            {
                ConvertResponse convertResponse = await this._wrapper.ConvertAsync(this._input, this._output, this._conversion);
                convertResponse.Code.Should().Be(200);
                convertResponse.Url.Should().NotBeNullOrWhiteSpace();

                Thread.Sleep(2000);

                DeleteConvertResponse deleteResponse = await this._wrapper.DeleteConversionAsync("https:" + convertResponse.Url);
                deleteResponse.Code.Should().Be(200);
            }
            catch (Exception ex)
            {
                var error = ex as ErrorResponseException;
                error.Should().NotBeNull();

                var response = error.Error;
                response.Should().BeOfType<ErrorResponse>();
                response.Code.Should().NotBe(200);
            }
        }

        /// <summary>
        /// Sets the conversion parameters to initial test defaults.
        /// </summary>
        private void SetConversionParametersToDefaults()
        {
            this._input = new InputParameters()
            {
                InputFormat = this._formats.Document.Md,
                InputMethod = InputMethod.Download,
                Filepath = "https://raw.githubusercontent.com/aliencube/CloudConvert.NET/dev/README.md",
                Filename = "README.md",
            };
            this._output = new OutputParameters()
            {
                DownloadMethod = DownloadMethod.False,
                OutputStorage = OutputStorage.OneDrive,
            };
            this._conversion = new ConversionParameters()
            {
                OutputFormat = this._formats.Document.Docx,
                ConverterOptions = new MarkdownConverterOptions()
                {
                    InputMarkdownSyntax = MarkdownSyntaxType.Auto
                },
            };
        }
    }
}