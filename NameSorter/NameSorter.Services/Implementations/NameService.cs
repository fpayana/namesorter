using NameSorter.Engines.Interfaces;
using NameSorter.Repositories.Interfaces;
using NameSorter.Services.Interfaces;

namespace NameSorter.Services.Implementations
{
    public class NameService : INameService
    {
        private readonly INameRepository _nameRepository;
        private readonly INameEngine _nameEngine;

        public NameService(INameRepository nameRepository, INameEngine nameEngine)
        {
            _nameRepository = nameRepository;
            _nameEngine = nameEngine;
        }

        public string[] SortAndSave(string path)
        {
            // read contents of file path and sort the names
            string[] sortedNames = _nameEngine.Sort(path);

            // save the sorted names into save file path
            _nameRepository.Save(sortedNames);

            return sortedNames;
        }
    }
}
