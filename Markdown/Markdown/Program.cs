using System;
using System.IO;
using System.Linq;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            string configFile = args[0];
            string originalFile = args[1];
            string headerFile = "header.txt";
            var header = FileParser.ReadParagraphFromFile(headerFile).ToList();
            string finalFile = originalFile.Split('.')[0] + ".html";
            if (File.Exists(finalFile))
                File.Delete(finalFile);
            FileParser.WriteLineToFile(finalFile, header[0]);
            var markdownMaker = new MarkdownMaker(configFile);
            foreach (var result in 
                FileParser.ReadParagraphFromFile(originalFile)
                          .Select(line => markdownMaker.MarkParagraph(line)))
            {
                FileParser.WriteLineToFile(finalFile, "<p>" + result + "</p>");
            }
            FileParser.WriteLineToFile(finalFile, header[1]);
            Console.WriteLine(finalFile);
        }
    }
}
