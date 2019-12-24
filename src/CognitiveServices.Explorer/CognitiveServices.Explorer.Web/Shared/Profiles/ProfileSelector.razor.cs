using CognitiveServices.Explorer.Application.Profiles.Commands;
using CognitiveServices.Explorer.Application.Profiles.Queries;
using CognitiveServices.Explorer.Domain.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace CognitiveServices.Explorer.Web.Shared.Profiles
{
    public partial class ProfileSelector
    {
        private Profile _newProfile = new Profile();
        private List<Profile> _profiles = new List<Profile>();
        private string? _selectedProfile;
        private bool _dialogIsOpen;

        [Inject] public GetAllProfilesQueryHandler GetAllProfilesQueryHandler { get; set; }
        [Inject] public SelectProfileCommandHandler SelectProfileCommandHandler { get; set; }
        [Inject] public SaveProfileCommandHandler SaveProfileCommandHandler { get; set; }
        [Inject] public DeleteProfileCommandHandler DeleteProfileCommandHandler { get; set; }

        [Parameter] public Func<Task>? OnUpdated { get; set; }
        [Parameter] public bool AllowManagingProfiles { get; set; } = true;

        public string? SelectedProfile
        {
            get { return _selectedProfile; }
            set
            {
                if (_selectedProfile != value)
                {
                    _selectedProfile = value;
                    SetSelectedProfile();
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await Refresh();
        }

        public async Task SaveProfile()
        {
            try
            {
                Console.WriteLine("Save profile");
                await SaveProfileCommandHandler.Handle(new SaveProfileCommand(_newProfile));
                await Refresh();
            }
            catch
            {
                // TODO: Handle exceptions.
            }

            await UpdateListeners();
            CloseDialog();
        }

        public async Task DeleteProfile()
        {
            try
            {
                await DeleteProfileCommandHandler.Handle(new DeleteProfileCommand(_newProfile.Id));
                await Refresh();
            }
            catch
            {
                // TODO: Handle exceptions.
            }

            await UpdateListeners();
            CloseDialog();
        }

        public void CloseDialog()
        {
            Console.WriteLine("Close dialog");
            _dialogIsOpen = false;
            base.StateHasChanged();
        }

        public async Task SetSelectedProfile()
        {
            var selectedProfileId = _profiles
                .Where(p => p.ProfileName == _selectedProfile)
                .Select(p => p.Id)
                .FirstOrDefault();

            await SelectProfileCommandHandler.Handle(new SelectProfileCommand(selectedProfileId));

            await UpdateListeners();
        }

        public void AddNewProfile()
        {
            _newProfile = new Profile { IsSelected = true };

            _dialogIsOpen = true;
        }

        public void EditProfile()
        {
            _newProfile = _profiles.FirstOrDefault(p => p.IsSelected);
            _dialogIsOpen = true;
        }

        public async Task Refresh()
        {
            _profiles = await GetAllProfilesQueryHandler.Handle(new GetAllProfilesQuery());
            _selectedProfile = _profiles
                .Where(p => p.IsSelected)
                .Select(p => p.ProfileName)
                .FirstOrDefault();
        }

        private async Task UpdateListeners()
        {
            if (OnUpdated != null)
            {
                await OnUpdated();
            }
        }
    }
}
