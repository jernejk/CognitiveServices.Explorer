using System;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Web
{
    public class AppState
    {
        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
