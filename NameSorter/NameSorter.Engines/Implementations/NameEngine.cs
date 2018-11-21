using NameSorter.Engines.Interfaces;
using NameSorter.Entities;
using NameSorter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NameSorter.Engines.Implementations
{
    public class NameEngine : INameEngine
    {
        private INameRepository _nameRepository;
        public NameEngine(INameRepository nameRepository)
        {
            _nameRepository = nameRepository;
        }

        public string[] Sort(string path)
        {            
            // get the contents of the file path
            string[] lines = _nameRepository.Get(path);

            // ensure file contents not empty
            if (lines == null || lines.Length == 0)
            {
                throw new Exception("File contents cannot be empty");
            }

            // go through each line, convert to Name, and add to list of names
            List<Name> names = new List<Name>();
            foreach (var line in lines)
            {
                // get Name object from text line
                Name name = GetName(line);
                if(name != null)
                {
                    // add to collection if not null
                    names.Add(name);
                }
            }

            // sort by last name then given name
            var sortedNames = names.OrderBy(e => e.LastName).ThenBy(e => e.GivenName);

            // return sorted names as string[] by joining given name and last name
            return (from sortedName in sortedNames
                    select string.Format("{0} {1}", sortedName.GivenName, sortedName.LastName)).ToArray();

        }

        public Name GetName(string text)
        {
            // return null if empty
            if(string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            // split text by whitespace into parts of name
            string[] nameParts = text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            // name must contains 1 last name, and between 1-3 given names
            if(nameParts.Length < 2)
            {
                throw new Exception(string.Format("A name must have at least 1 given name and a last name: {0}", text));
            }
            else if(nameParts.Length > 4)
            {
                throw new Exception(string.Format("A name may have up to 3 given names and a last name: {0}", text));
            }

            Name name = new Name();

            // the last part is last name
            name.LastName = nameParts.Last();

            // all parts except last one are given name parts
            var givenNameParts = nameParts.Take(nameParts.Length - 1);

            // join all given name parts and separate using whitespace to get given name
            name.GivenName = string.Join(" ", givenNameParts);

            return name;
        }
    }
}
