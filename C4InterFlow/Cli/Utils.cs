﻿using C4InterFlow.Automation;
using C4InterFlow.Elements;
using C4InterFlow.Elements.Interfaces;
using System.Reflection;
using System.Runtime.Loader;

namespace C4InterFlow.Cli
{
    public class Utils
    {
        public static IEnumerable<string> ResolveWildcardStructures(IEnumerable<string> structures)
        {
            return ArchitectureAsCodeReaderContext.Strategy.ResolveWildcardStructures(structures);
        }

        public static IEnumerable<Interface> GetAllInterfaces()
        {
            return ArchitectureAsCodeReaderContext.Strategy.GetAllInterfaces();
        }

        public static IEnumerable<string> ReadLines(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Could not read lines from a file. The file {filePath} does not exist.");
                yield break;
            }

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public static void WriteLines(List<string> items, string filePath, bool append = false)
        {
            string directoryPath = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, append))
                {
                    foreach (string item in items)
                    {
                        writer.WriteLine(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not write lines to a file. An error occurred: {ex.Message}");
            }
        }
    }
}
