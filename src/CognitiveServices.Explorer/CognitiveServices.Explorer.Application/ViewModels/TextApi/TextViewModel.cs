using CognitiveServices.Explorer.Application.Commands;
using CognitiveServices.Explorer.Application.Curl;
using CognitiveServices.Explorer.Application.Profiles.Queries;
using CognitiveServices.Explorer.Application.Text;
using CognitiveServices.Explorer.Domain.Face;
using CognitiveServices.Explorer.Domain.Profiles;
using Flurl.Http;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.ViewModels.TextApi
{
    public class TextViewModel
    {
        private readonly IMediator _mediator;
        private HttpRequest _sentimentAnalysis;
        private HttpRequest _keyPhrases;
        private HttpRequest _entities;
        private HttpRequest _detectLanguage;
        private HttpRequest _entityLinking;
        private HttpRequest _entityRecognitionPii;

        public TextViewModel(IMediator mediator)
        {
            _mediator = mediator;
            _sentimentAnalysis = TextRequestGenerator.Sentiment(Text, Language, TextApiVersion);
            _keyPhrases = TextRequestGenerator.KeyPhrases(Text, Language, TextApiVersion);
            _entities = TextRequestGenerator.Entities(Text, Language, TextApiVersion);
            _detectLanguage = TextRequestGenerator.DetectLanguage(Text, Language, TextApiVersion);
            _entityLinking = TextRequestGenerator.EntityLinking(Text, Language);
            _entityRecognitionPii = TextRequestGenerator.EntityRecognitionPii(Text, Language);

            Requests.Add(_sentimentAnalysis);
            Requests.Add(_keyPhrases);
            Requests.Add(_entities);
            Requests.Add(_detectLanguage);
        }

        public List<HttpRequest> Requests { get; } = new List<HttpRequest>();
        public string Text { get; set; } = "Cognitive Services and Blazor are awesome technologies! Blazor needs a bit more polishing. Despite that, I love it! 😁";
        public string Language { get; set; } = TextRequestGenerator.DefaultLanguage;

        public string? SentimentJson { get; set; } = string.Empty;
        public string? KeyPhraseJson { get; set; } = string.Empty;
        public string? EntitiesJson { get; set; } = string.Empty;
        public string? DetectLanguageJson { get; set; } = string.Empty;
        public string? EntityLinkingJson { get; set; } = string.Empty;
        public string? EntityRecognitionPiiJson { get; set; } = string.Empty;
        public string? Error { get; set; } = string.Empty;
        public CognitiveServiceConfig? TextApiConfig { get; private set; } = null;
        public bool IsTextApiAvailable { get; set; }
        public string TextApiVersion { get; set; } = TextRequestGenerator.StableVersion;
        public bool IsStableApi => TextApiVersion == TextRequestGenerator.StableVersion;
        public bool IsPreviewApi => TextApiVersion == TextRequestGenerator.PreviewVersion;

        public virtual async Task OnInitializedAsync()
        {
            await LoadLatestConfig().ConfigureAwait(false);
        }

        public async Task LoadLatestConfig()
        {
            var profile = await _mediator.Send(new GetCurrentProfileQuery());
            TextApiConfig = profile?.TextApiConfig;

            IsTextApiAvailable = TextApiConfig?.IsConfigured() == true;
            Console.WriteLine($"{TextApiConfig?.BaseUrl} => {IsTextApiAvailable}");
        }

        public async Task SentimentAnalysis()
        {
            _sentimentAnalysis = TextRequestGenerator.Sentiment(Text, Language, TextApiVersion);
            SentimentJson = await MakeRequest<string>(_sentimentAnalysis).ConfigureAwait(false);
        }

        public async Task KeyPhrasesAnalysis()
        {
            _keyPhrases = TextRequestGenerator.KeyPhrases(Text, Language, TextApiVersion);
            KeyPhraseJson = await MakeRequest<string>(_keyPhrases).ConfigureAwait(false);
        }

        public async Task EntitiesAnalysis()
        {
            _entities = TextRequestGenerator.Entities(Text, Language, TextApiVersion);
            EntitiesJson = await MakeRequest<string>(_entities).ConfigureAwait(false);
        }

        public async Task DetectLanguage()
        {
            _detectLanguage = TextRequestGenerator.DetectLanguage(Text, "", TextApiVersion);
            DetectLanguageJson = await MakeRequest<string>(_detectLanguage).ConfigureAwait(false);
        }

        public async Task EntityLinking()
        {
            _entityLinking = TextRequestGenerator.EntityLinking(Text, "");
            EntityLinkingJson = await MakeRequest<string>(_entityLinking).ConfigureAwait(false);
        }

        public async Task EntityRecognitionPii()
        {
            _entityRecognitionPii = TextRequestGenerator.EntityRecognitionPii(Text, "");
            EntityRecognitionPiiJson = await MakeRequest<string>(_entityRecognitionPii).ConfigureAwait(false);
        }

        protected async Task<string?> MakeRequest<T>(HttpRequest? request)
            where T : class
        {
            Error = string.Empty;
            if (request == null)
            {
                Error = "Request is not set!";
                return default!;
            }

            if (TextApiConfig == null)
            {
                Error = "Text API configuration is not set\n";
                return default!;
            }

            try
            {
                await LoadLatestConfig().ConfigureAwait(false);

                string json = await _mediator.Send(new ExecuteCognitiveServicesCommand(request, TextApiConfig)).ConfigureAwait(false) ?? string.Empty;
                return json;
                //return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                //{
                //    PropertyNameCaseInsensitive = true
                //});
            }
            catch (FlurlHttpException fe)
            {
                try
                {
                    ErrorDto e = await fe.Call.Response.GetJsonAsync<ErrorDto>();
                    if (e?.Error != null)
                    {
                        Error = $"Text API error code {e.Error.Code}: \n{e.Error.Message}";
                    }
                }
                catch
                {
                }

                if (string.IsNullOrWhiteSpace(Error))
                {
                    Error = fe.Message;
                }

                return default;
            }
            catch (Exception e)
            {
                Error = e.ToString();
                return default!;
            }
        }
    }
}
