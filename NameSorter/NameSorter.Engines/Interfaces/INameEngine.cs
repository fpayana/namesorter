using NameSorter.Entities;

namespace NameSorter.Engines.Interfaces
{
    public interface INameEngine
    {
        string[] Sort(string path);

        Name GetName(string text);
    }
}
