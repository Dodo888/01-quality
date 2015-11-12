using System.IO;
using System.Linq;

namespace Markdown
{
    public class Program
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
            WriteContent(configFile, originalFile, finalFile);
            FileParser.WriteLineToFile(finalFile, header[1]);
        }

        public static void WriteContent(string configFile, string originalFile, string finalFile)
        {
            var markdownMaker = new MarkdownMaker(configFile);
            foreach (var result in
                FileParser.ReadParagraphFromFile(originalFile)
                          .Select(paragraph => markdownMaker.MarkParagraph(paragraph)))
            {
                FileParser.WriteLineToFile(finalFile, result);
            }
        }
    }
}
