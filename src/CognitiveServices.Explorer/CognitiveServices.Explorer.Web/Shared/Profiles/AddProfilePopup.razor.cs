using CognitiveServices.Explorer.Domain.Profiles;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Web.Shared.Profiles
{
    public partial class AddProfilePopup : ComponentBase
    {
        [Parameter] public bool IsDialogOpen { get; set; }
        [Parameter] public Profile Profile { get; set; }
        [Parameter] public Func<Task> OnSaveProfile { get; set; }
        [Parameter] public Func<Task> OnDeleteProfile { get; set; }
        [Parameter] public Action OnCancel { get; set; }

        public Task SaveProfile() => RunCallback(OnSaveProfile);
        public Task DeleteProfile() => RunCallback(OnDeleteProfile);
        public void Cancel() => OnCancel?.Invoke();

        private Task RunCallback(Func<Task> callback)
        {
            return callback != null ? callback() : Task.CompletedTask;
        }

        public bool IsNewProfile() => Profile.Id == Guid.Empty;
    }
}
