using CognitiveServices.Explorer.Domain.Profiles;
using System.Collections.Generic;
using System.Linq;

namespace CognitiveServices.Explorer.Application.Profiles.Shared
{
    public static class ProfilesExtensions
    {
        public static void UpdateIsSelected(this List<Profile> profiles, Profile? selectedProfile = null)
        {
            // Change selected state only if the number of selected profiles is not 1.
            var selectedProfiles = profiles.Where(p => p.IsSelected).ToList();
            if (selectedProfiles.Count > 1)
            {
                foreach (var p in selectedProfiles)
                {
                    p.IsSelected = false;
                }

                if (selectedProfile != null)
                {
                    // Assume that we have more then 1 selected
                    // because current profile is marked as selected.
                    selectedProfile.IsSelected = true;
                }
                else
                {
                    // If there is not preferred selected profile, mark first one as selected.
                    var profile = selectedProfiles.FirstOrDefault();
                    profile.IsSelected = true;
                }
            }
            else if (selectedProfiles.Count == 0 && profiles.Count > 0)
            {
                if (selectedProfile != null)
                {
                    // If no profile was selected, select this profile instead.
                    selectedProfile.IsSelected = true;
                }
                else
                {
                    // If there is not preferred selected profile, mark first one as selected.
                    var profile = profiles.FirstOrDefault();
                    profile.IsSelected = true;
                }
            }
        }
    }
}
