using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MarkdownMaker
    {
        private readonly Dictionary<string, MarkdownTag> tagForSymbol;

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

        public string MarkParagraph(string text)
        {
            var tags = FindMarkdownTags(text);
            text = FindTaggedAreas(text, tags, 0);
            return RemoveEscapes(text);
        }

        public string RemoveEscapes(string text)
        {
            var result = "";
            for (var i=0; i<text.Length; i++)
            {
                if (text[i] == '\\')
                {
                    if (i == text.Length - 1) break;
                    i++;
                    result += text[i];
                }
                else
                    result += text[i];
            }
            return result;
        }

        public bool IsInAnotherTag(KeyValuePair<int, string> item, int tagIndex)
        {
            return ((item.Key <= tagIndex) && (item.Key + item.Value.Length > tagIndex));
        }

        public bool IsEscaped(int tagIndex, string text)
        {
            return ((tagIndex != 0) && (text[tagIndex-1] == '\\'));
        }

        public bool IsInNumbers(int tagIndex, string text)
        {
            var numbers = "0123456789";
            return ((tagIndex != 0) && (numbers.Contains(text[tagIndex - 1])) ||
                ((tagIndex != text.Length - 1) && (numbers.Contains(text[tagIndex + 1]))));
        }

        public Dictionary<int, string> FindMarkdownTags(string text)
        {
            var tagPositions = new Dictionary<int, string>();
            foreach (var tag in tagForSymbol.Keys.OrderByDescending(x => x.Length))
            {
                var count = 0;
                while (text.IndexOf(tag, count, StringComparison.Ordinal) != -1)
                {
                    var tagIndex = text.IndexOf(tag, count, StringComparison.Ordinal);
                    if (!IsEscaped(tagIndex, text) &&
                        (tagForSymbol[tag].IsFormattableInNumbers || !IsInNumbers(tagIndex, text)) &&
                        tagPositions.All(item => !IsInAnotherTag(item, tagIndex) &&
                                                 !IsInAnotherTag(item, tagIndex + tag.Length - 1)))
                    {
                        tagPositions.Add(tagIndex, tag);
                    }
                    count = tagIndex + tag.Length;
                }
            }
            return tagPositions;
        }

        public Tuple<int, int> FindTagPair(Dictionary<int, string> tagPositions, int offset, int textLength)
        {
            var openingTagIndex = 0;
            var closingTagIndex = 0;
            foreach (var tagIndex in tagPositions.Where(item => item.Key >= offset).OrderBy(item => item.Key))
            {
                openingTagIndex = tagIndex.Key;
                var nextTags = tagPositions
                    .Where(item => (item.Value == tagPositions[openingTagIndex]) &&
                        (item.Key > openingTagIndex) && (item.Key < textLength + offset))
                    .OrderBy(item => item.Key).Select(item => item.Key);
                if (nextTags.Any())
                {
                    closingTagIndex = nextTags.ToArray()[0];
                    break;
                }
            }
            return new Tuple<int, int>(openingTagIndex, closingTagIndex);
        }

        public string FindTaggedAreas(string text, Dictionary<int, string> tagPositions, int offset)
        {
            var tagIndexes = FindTagPair(tagPositions, offset, text.Length);
            var openingTagIndex = tagIndexes.Item1;
            var closingTagIndex = tagIndexes.Item2;
            if (closingTagIndex == 0)
                return text;

            var textBeforeTaggedArea = text.Substring(0, openingTagIndex - offset);
            var tag = tagForSymbol[tagPositions[openingTagIndex]];
            var textInsideTaggedArea = text.Substring(openingTagIndex + tagPositions[openingTagIndex].Length - offset,
                closingTagIndex - tagPositions[openingTagIndex].Length - openingTagIndex);
            var textAfterTaggedArea =
                FindTaggedAreas(text.Substring(closingTagIndex + tagPositions[closingTagIndex].Length - offset),
                    tagPositions, closingTagIndex + tagPositions[closingTagIndex].Length);

            return textBeforeTaggedArea + tag.OpeningTag +
                    (tag.IsFormattableInside
                        ? FindTaggedAreas(textInsideTaggedArea, tagPositions,
                            openingTagIndex + tagPositions[openingTagIndex].Length)
                        : textInsideTaggedArea) +
                    tag.ClosingTag + textAfterTaggedArea;
        }
    }
}
