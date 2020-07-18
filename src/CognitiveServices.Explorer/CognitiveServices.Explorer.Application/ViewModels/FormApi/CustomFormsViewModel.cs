using CognitiveServices.Explorer.Application.Commands;
using CognitiveServices.Explorer.Application.Forms;
using CognitiveServices.Explorer.Application.Profiles.Queries;
using CognitiveServices.Explorer.Domain.Forms;
using CognitiveServices.Explorer.Domain.Profiles;
using Flurl;
using Flurl.Http;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.ViewModels.FormApi
{
    public class CustomFormsViewModel
    {
        private readonly IMediator _mediator;
        private HttpRequest _getModels;
        private HttpRequest _startFormAnalyze;
        private HttpRequest _getFormResults;

        public CustomFormsViewModel(IMediator mediator)
        {
            _mediator = mediator;
            _getModels = CustomFormRequestGenerator.GetModels();
            _startFormAnalyze = CustomFormRequestGenerator.StartAnalyzeForm(string.Empty, new byte[0], "image/jpeg");
            _getFormResults = CustomFormRequestGenerator.GetResultFromForm(string.Empty);

            Requests.Add(_getModels);
            Requests.Add(_startFormAnalyze);
            Requests.Add(_getFormResults);
        }

        public List<HttpRequest> Requests { get; } = new List<HttpRequest>();
        public CognitiveServiceConfig? FormApiConfig { get; private set; } = null;
        public bool IsFormApiAvailable { get; set; }

        public GetModelsDto? Models { get; set; }
        public string[] SupportedContentTypes { get; set; } = new[] { "image/jpeg", "image/png", "image/tiff", "application/pdf" };

        public string? ModelId { get; set; }
        public string ContentType { get; set; } = "image/jpeg";

        public string? Error { get; set; }
        public FormDto? FormResult { get; set; }
        public string FormResultText { get; set; }

        public virtual async Task OnInitializedAsync()
        {
            Console.WriteLine($"Loading configs...");
            await LoadLatestConfig().ConfigureAwait(false);

            Console.WriteLine($"Attempt loading? {IsFormApiAvailable}");
            if (IsFormApiAvailable)
            {
                Console.WriteLine("Loading");
                var json = await _mediator.Send(new ExecuteCognitiveServicesCommand(_getModels, FormApiConfig!), default);
                Console.WriteLine("JSON: " + json);
                Models = JsonSerializer.Deserialize<GetModelsDto>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                Console.WriteLine("Models: " + Models?.modelList?.Length);
            }
        }

        public async Task LoadLatestConfig()
        {
            var profile = await _mediator.Send(new GetCurrentProfileQuery());
            FormApiConfig = profile?.FormApiConfig;

            IsFormApiAvailable = FormApiConfig?.IsConfigured() == true;
            Console.WriteLine($"{FormApiConfig?.BaseUrl} => {IsFormApiAvailable}");
        }

        public async Task Analyze(string imageUrl)
        {
            _startFormAnalyze = CustomFormRequestGenerator.StartAnalyzeForm(ModelId!, imageUrl);

            await Analyze();
        }

        public async Task Analyze(byte[] data)
        {
            _startFormAnalyze = CustomFormRequestGenerator.StartAnalyzeForm(ModelId!, data, "image/jpeg");

            await Analyze();
        }

        private async Task Analyze()
        {
            FormResultText = string.Empty;

            _getFormResults = CustomFormRequestGenerator.GetResultFromForm(string.Empty);

            UpdateRequestList();

            // TODO: Quick hack because we need body as well as headers in response unlike other Cognitive Services responses.
            var byteArrayContent = new ByteArrayContent(_startFormAnalyze.BinaryContent);
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(_startFormAnalyze.ContentType);

            var responseAnalyze = await new Url(FormApiConfig.BaseUrl)
                .AppendPathSegment(_startFormAnalyze.RelativePath)
                .WithHeader(_startFormAnalyze.TokenHeaderName, FormApiConfig.Token)
                .PostAsync(byteArrayContent);

            if (responseAnalyze.StatusCode == 202)
            {
                Console.WriteLine($"Headers: {responseAnalyze.Headers.Count}:");
                foreach (var header in responseAnalyze.Headers)
                {
                    Console.WriteLine($"\t{header.Key}: {header.Value}");
                }

                // Operation Location is based on specs cased with uppercases as in the where clause and in browser you'll see this casing.
                // However, either browser or Blazor lowercase it. Just to be sure, we are getting the header in case-insensitive way.
                string operationLocation = responseAnalyze.Headers
                    .Where(h => h.Key.Equals("Operation-Location", StringComparison.CurrentCultureIgnoreCase))
                    .Select(h => h.Value)
                    .FirstOrDefault();

                if (string.IsNullOrWhiteSpace(operationLocation))
                {
                    Error = "Unable to get Operation-Location from header.";
                    return;
                }

                _getFormResults = CustomFormRequestGenerator.GetResultFromForm(operationLocation);
                var getFormResultRequest = new Url(_getFormResults.AbsoluteUrl)
                    .WithHeader(_getFormResults.TokenHeaderName, FormApiConfig.Token);

                FormDto? formResult = null;
                int tries = 10;
                while (tries-- > 0)
                {
                    var response = await getFormResultRequest.GetAsync();

                    try
                    {
                        formResult = await response.GetJsonAsync<FormDto>();
                        if (formResult?.status == "failed" || response.StatusCode != 200)
                        {
                            Error = formResult?.error?.message;
                            Console.WriteLine("Unable to analyze the form. Error: " + Error);
                            break;
                        }
                        else if (formResult?.status == "succeeded")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Trying again in 10 seconds. Status: " + formResult?.status);
                            await Task.Delay(10000);
                        }
                    }
                    catch (Exception ex)
                    {
                        Error = ex.Message;
                        Console.WriteLine("Unable to analyze the form with exceptions. Error: " + Error);
                        FormResultText = "Unable to analyze the form with exceptions. Error: " + Error;
                    }
                }

                if (formResult?.analyzeResult != null)
                {
                    Console.WriteLine("Yay!");
                    FormResultText = "Form results:\n";

                    foreach (var forms in formResult.analyzeResult.documentResults)
                    {
                        foreach (var item in forms.fields)
                        {
                            string textLine = $"{item.Key}: {item.Value?.text} ({Math.Round(item.Value?.confidence ?? 0, 2) * 100d}%)";
                            FormResultText += textLine + "\n";
                            Console.WriteLine(textLine);
                        }
                    }
                }
            }
        }

        private void UpdateRequestList()
        {
            // Both requests are changing every time we do a request and because the reference is changing, we need to rebuild the list.
            Requests.Clear();
            Requests.Add(_getModels);
            Requests.Add(_startFormAnalyze);
            Requests.Add(_getFormResults);
        }
    }
}
