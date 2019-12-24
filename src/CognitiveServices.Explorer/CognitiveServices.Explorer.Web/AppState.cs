using System;

namespace CognitiveServices.Explorer.Web
{
    public class AppState
    {
        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
