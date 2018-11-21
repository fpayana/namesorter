using NameSorter.Common;
using NameSorter.Services.Interfaces;
using System;

namespace NameSorter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // bind all interfaces to its implementations
                ObjectFactory.BindAll();

                if (args == null || args.Length != 1)
                {
                    // args must be 1, if not display usage and error message
                    Console.WriteLine("USAGE:");
                    Console.WriteLine("name-sorter filepath");
                    Console.WriteLine();

                    if (args == null || args.Length == 0)
                    {
                        Console.WriteLine("ERROR: filepath is missing");
                    }
                    else
                    {
                        Console.WriteLine("ERROR: more than 1 parameters detected");
                    }
                }
                else
                {
                    // create instance of INameService
                    INameService nameService = ObjectFactory.CreateInstance<INameService>();

                    // args[0] contains path of the file that we will sort and save
                    string[] sortedNames = nameService.SortAndSave(args[0]);

                    if (sortedNames != null && sortedNames.Length > 0)
                    {
                        // go through the sorted names and display to screen
                        foreach (var sortedName in sortedNames)
                        {
                            Console.WriteLine(sortedName);
                        }
                    }
                    else
                    {
                        // empty files detected
                        Console.WriteLine("File contents cannot be empty");
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
