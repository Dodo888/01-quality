namespace Markdown
{
    public class TagPair
    {
        public int OpeningTagIndex { get; private set; }
        public int ClosingTagIndex { get; private set; }

        public TagPair(int openingTagIndex, int closingTagIndex)
        {
            OpeningTagIndex = openingTagIndex;
            ClosingTagIndex = closingTagIndex;
        }
    }

}
