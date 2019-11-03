using CognitiveServices.Explorer.Application.FaceApi;
using CognitiveServices.Explorer.Domain.Face;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.ViewModels.FaceApi
{
    public class DetectViewModel : BaseFaceApiViewModel
    {
        private HttpRequest _detectRequest;
        private HttpRequest _identifyRequest;

        public DetectViewModel(ICognitiveServicesConfigService csConfigService)
            : base(csConfigService)
        {
            _detectRequest = FaceRequestGenerator.Detect(Array.Empty<byte>());
            _identifyRequest = FaceRequestGenerator.Identify(string.Empty, Array.Empty<string>());

            UpdateRequestList();
        }

        public List<DetectedFaceDto>? Faces { get; private set; }
        public List<IdentityCandidate>? Candidates { get; private set; }
        public List<HttpRequest> Requests { get; } = new List<HttpRequest>();

        public async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        public async Task Detect(byte[] data)
        {
            _detectRequest = FaceRequestGenerator.Detect(data);
            UpdateRequestList();

            Faces = await MakeRequest<List<DetectedFaceDto>>(_detectRequest).ConfigureAwait(false);
        }

        public async Task Identify(string? personGroupId)
        {
            if (string.IsNullOrWhiteSpace(personGroupId))
            {
                Error = "Person Group Id is not set!";
                return;
            }

            _identifyRequest = FaceRequestGenerator.Identify(personGroupId!, Faces.Select(f => f.faceId));
            UpdateRequestList();

            Candidates = await MakeRequest<List<IdentityCandidate>>(_identifyRequest).ConfigureAwait(false);
        }

        private void UpdateRequestList()
        {
            // Both requests are changing every time we do a request and because the reference is changing, we need to rebuild the list.
            Requests.Clear();
            Requests.Add(_detectRequest);
            Requests.Add(_identifyRequest);
        }
    }
}
