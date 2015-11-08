using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class MarkdownMaker
    {
        private Dictionary<string, MarkdownTag> tagForSymbol;

        public MarkdownMaker(Dictionary<string, MarkdownTag> tags)
        {
            tagForSymbol = tags;
        }

        public MarkdownMaker(string configFile)
        {
            tagForSymbol = new Dictionary<string, MarkdownTag>();
            foreach (var line in FileParser.ReadLineFromFile(configFile))
            {
                var tagProperties = line.Split();
                var markdownTag = new MarkdownTag(tagProperties[1], tagProperties[2],
                    tagProperties[3] == "formatting_inside",
                    tagProperties[4] == "in_numbers");
                tagForSymbol.Add(tagProperties[0], markdownTag);
            }
        }

        public string FindTaggedArea(string text)
        {
            var minIndex = text.Length;
            var closingIndexForMin = 0;
            var minIndexTag = "";
            foreach (var markdownSequence in tagForSymbol.Keys)
            {
                var openingTagIndex = text.IndexOf(markdownSequence, StringComparison.Ordinal);
                if (openingTagIndex != -1)
                {
                    var closingTagIndex = text.Substring(openingTagIndex + markdownSequence.Length)
                        .IndexOf(markdownSequence, StringComparison.Ordinal);
                    if (closingTagIndex != -1 && (minIndex > openingTagIndex ||
                                                  (minIndex == openingTagIndex &&
                                                   minIndexTag.Length < markdownSequence.Length)))
                    {
                        minIndex = openingTagIndex;
                        minIndexTag = markdownSequence;
                        closingIndexForMin = closingTagIndex + openingTagIndex +1;
                    }
                }
            }
            if (closingIndexForMin == 0)
                return text;
            return text.Substring(0, minIndex) + tagForSymbol[minIndexTag].OpeningTag +
                FindTaggedArea(text.Substring(minIndex + minIndexTag.Length, 
                closingIndexForMin - minIndex - minIndexTag.Length)) +
                tagForSymbol[minIndexTag].ClosingTag +
                FindTaggedArea(text.Substring(closingIndexForMin + minIndexTag.Length));
        }

        public bool CheckForNumbers(int sequenceLength)
        {
            return true;
        }
    }
}
