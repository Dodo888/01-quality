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
        }
    }

    class FileParser
    {
        public static string ReadFile(string path)
        {
            return "";
        }

        public static void writeFile(string path)
        {
        }
    }

    class MarkdownMaker
    {
        private Dictionary<string, Tuple<string>> tagForSymbol;

        public string findMarkdownSymbol()
        {
            var tag = "";
            return tag;
        }

        public string replaceMarkdownSequence(bool isOpening, int sequenceLength, string tag)
        {
            return "";
        }

        public bool hasEscapedCharacter(int sequenceLength)
        {
            return true;
        }

        public bool hasNumbers(int sequenceLength)
        {
            return true;
        }
    }
}
