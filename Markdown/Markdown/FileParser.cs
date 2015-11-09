using System.Collections.Generic;
using System.IO;

namespace Markdown
{
    public class FileParser
    {
        public static IEnumerable<string> ReadParagraphFromFile(string path)
        {
            var file = new StreamReader(path);
            string paragraph = "";
            string line;
            while ((line = file.ReadLine()) != null)
            {
                if (line == "")
                {
                    yield return paragraph;
                    paragraph = "";
                }
                paragraph += line + "\r\n";    
            }
            yield return paragraph;
            file.Close();
        }

        public static IEnumerable<string> ReadLineFromFile(string path)
        {
            var file = new StreamReader(path);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                yield return line;
            }
            file.Close();
        }

        public static void WriteLineToFile(string path, string line)
        {
            using (var file = new StreamWriter(path, true))
            {
                file.WriteLine(line);
            }
        }
    }
}
