using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            string configFile = args[0];
            string originalFile = args[1];
            string finalFile = originalFile.Split('.')[0] + ".html";
            var markdownMaker = new MarkdownMaker(configFile);
            foreach (var line in FileParser.ReadLineFromFile(originalFile))
            {
                var result = markdownMaker.FindTaggedArea(line);
                Console.WriteLine(result);
            }
            Console.WriteLine(finalFile);
        }
    }
}
