using CognitiveServices.Explorer.Application;
using CognitiveServices.Explorer.Application.FaceApi;
using CognitiveServices.Explorer.Domain.Face;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Web.ViewModels.FaceApi
{
    public class DetectViewModel : BaseFaceApiViewModel
    {
        private HttpRequest? _detectRequest;
        private HttpRequest? _identifyRequest;

        public DetectViewModel(ICognitiveServicesConfigService csConfigService) : base(csConfigService)
        {
        }

        public List<DetectedFaceDto>? Faces { get; private set; }
        public List<IdentityCandidate>? Candidates { get; private set; }
        public List<HttpRequest> Requests { get; } = new List<HttpRequest>();

        public async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            _detectRequest = FaceRequestGenerator.Detect(Array.Empty<byte>());
            _identifyRequest = FaceRequestGenerator.Identify(string.Empty, Array.Empty<string>());

            Requests.Add(_detectRequest);
            Requests.Add(_identifyRequest);
        }

        public async Task Detect(byte[] data)
        {
            _detectRequest = FaceRequestGenerator.Detect(data);
            Faces = await MakeRequest<List<DetectedFaceDto>>(_detectRequest).ConfigureAwait(false);
        }

        public async Task Identify(string personGroupId)
        {
            _identifyRequest = FaceRequestGenerator.Identify(personGroupId, Faces.Select(f => f.faceId));
            Candidates = await MakeRequest<List<IdentityCandidate>>(_identifyRequest).ConfigureAwait(false);
        }
    }
}
