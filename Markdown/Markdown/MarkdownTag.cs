using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class MarkdownTag
    {
        public string OpeningTag { get; private set; }
        public string ClosingTag { get; private set; }
        public bool IsFormattableInside { get; private set; }
        public bool IsFormattableInNumbers { get; private set; }

        public MarkdownTag()
        {
        }

        public MarkdownTag(string openingTag, string closingTag)
        {
            OpeningTag = openingTag;
            ClosingTag = closingTag;
            IsFormattableInside = true;
            IsFormattableInNumbers = true;
        }

        public MarkdownTag(string openingTag, string closingTag, 
            bool isFormattableInside, bool isFormattableInNumbers)
        {
            OpeningTag = openingTag;
            ClosingTag = closingTag;
            IsFormattableInside = isFormattableInside;
            IsFormattableInNumbers = isFormattableInNumbers;
        }
    }
}
