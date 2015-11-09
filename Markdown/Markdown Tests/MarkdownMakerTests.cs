using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Markdown;

namespace Markdown_Tests
{
    [TestClass]
    public class MarkdownMakerTests
    {
        [TestMethod]
        public void Should_FindTaggedAreas()
        {
            var tags = new Dictionary<string, MarkdownTag> {{"_", new MarkdownTag("<em>", "</em>")}};
            var markdownMaker = new MarkdownMaker(tags);
            var text = "It_should_change";
            var result = markdownMaker.MarkParagraph(text);
            Assert.AreEqual(result, "It<em>should</em>change");
        }

        [TestMethod]
        public void Should_FindAllTaggedAreas()
        {
            var tags = new Dictionary<string, MarkdownTag>
            {
                { "_", new MarkdownTag("<em>", "</em>") },
                { "__", new MarkdownTag("<strong>", "</strong>")}
            };
            var markdownMaker = new MarkdownMaker(tags);
            var text = "It_should_ __change__";
            var result = markdownMaker.MarkParagraph(text);
            Assert.AreEqual(result, "It<em>should</em> <strong>change</strong>");
        }

        [TestMethod]
        public void Should_FindNestedTaggedAreas()
        {
            var tags = new Dictionary<string, MarkdownTag>
            {
                { "_", new MarkdownTag("<em>", "</em>") },
                { "`", new MarkdownTag("<code>", "</code>")}
            };
            var markdownMaker = new MarkdownMaker(tags);
            var text = "_em tag `and code tag in it`_";
            var result = markdownMaker.MarkParagraph(text);
            Assert.AreEqual(result, "<em>em tag <code>and code tag in it</code></em>");
        }

        [TestMethod]
        public void Shouldnot_FindNestedTaggedAreas_inCodeTag()
        {
            var tags = new Dictionary<string, MarkdownTag>
            {
                { "_", new MarkdownTag("<em>", "</em>") },
                { "`", new MarkdownTag("<code>", "</code>", false, false)}
            };
            var markdownMaker = new MarkdownMaker(tags);
            var text = "`code tag _and em tag in it_`";
            var result = markdownMaker.MarkParagraph(text);
            Assert.AreEqual(result, "<code>code tag _and em tag in it_</code>");
        }

        [TestMethod]
        public void Shouldnot_FindTaggedAreas_nearNumbers()
        {
            var tags = new Dictionary<string, MarkdownTag>
            {
                { "_", new MarkdownTag("<em>", "</em>") },
                { "`", new MarkdownTag("<code>", "</code>", false, false)}
            };
            var markdownMaker = new MarkdownMaker(tags);
            var text = "`123code tag _and em tag in it_`";
            var result = markdownMaker.MarkParagraph(text);
            Assert.AreEqual(result, "`123code tag <em>and em tag in it</em>`");
        }

        [TestMethod]
        public void Shouldnot_FindEscapedTaggedAreas()
        {
            var tags = new Dictionary<string, MarkdownTag>
            {
                { "_", new MarkdownTag("<em>", "</em>") },
                { "`", new MarkdownTag("<code>", "</code>", false, false)}
            };
            var markdownMaker = new MarkdownMaker(tags);
            var text = "It is \\_escaped_";
            var result = markdownMaker.MarkParagraph(text);
            Assert.AreEqual(result, "It is _escaped_");
        }

        [TestMethod]
        public void Shouldnot_MarkUnpairedTags()
        {
            var tags = new Dictionary<string, MarkdownTag>
            {
                { "_", new MarkdownTag("<em>", "</em>") },
                { "__", new MarkdownTag("<strong>", "</strong>")},
                { "`", new MarkdownTag("<code>", "</code>")},

            };
            var markdownMaker = new MarkdownMaker(tags);
            var text = "All tags _there__ are `unpaired";
            var result = markdownMaker.MarkParagraph(text);
            Assert.AreEqual(result, "All tags _there__ are `unpaired");
        }
    }
}
