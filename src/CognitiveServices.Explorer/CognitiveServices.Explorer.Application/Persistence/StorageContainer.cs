using System.Collections.Generic;

namespace CognitiveServices.Explorer.Application.Persistence
{
    public class StorageContainer<T>
    {
        public int Version { get; set; }

        public List<T> Profiles { get; set; } = new List<T>();
    }
}
