namespace NameSorter.Repositories.Interfaces
{
    public interface INameRepository
    {
        string[] Get(string path);

        void Save(string[] names);
    }
}
