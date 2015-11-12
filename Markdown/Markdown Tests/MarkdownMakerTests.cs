using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Markdown;

namespace Markdown_Tests
{
    [TestClass]
    public class MarkdownMakerTests
    {

        public string DoMarkdown(Dictionary<string, MarkdownTag> tags, string text)
        {
            var markdownMaker = new MarkdownMaker(tags);
            return markdownMaker.MarkParagraph(text);
        }

        [TestMethod]
        public void Should_FindTaggedAreas()
        {
            var tags = new Dictionary<string, MarkdownTag> {{"_", new MarkdownTag("<em>", "</em>")}};
            var text = "It_should_change";
            Assert.AreEqual(DoMarkdown(tags, text), "<p>It<em>should</em>change</p>");
        }

        [TestMethod]
        public void Should_FindAllTaggedAreas()
        {
            var tags = new Dictionary<string, MarkdownTag>
            {
                { "_", new MarkdownTag("<em>", "</em>") },
                { "__", new MarkdownTag("<strong>", "</strong>")}
            };
            var text = "It_should_ __change__";
            Assert.AreEqual(DoMarkdown(tags, text), "<p>It<em>should</em> <strong>change</strong></p>");
        }

        [TestMethod]
        public void Should_FindNestedTaggedAreas()
        {
            var tags = new Dictionary<string, MarkdownTag>
            {
                { "_", new MarkdownTag("<em>", "</em>") },
                { "`", new MarkdownTag("<code>", "</code>")}
            };
            var text = "_em tag `and code tag in it`_";
            Assert.AreEqual(DoMarkdown(tags, text), "<p><em>em tag <code>and code tag in it</code></em></p>");
        }

        [TestMethod]
        public void Shouldnot_FindNestedTaggedAreas_inCodeTag()
        {
            var tags = new Dictionary<string, MarkdownTag>
            {
                { "_", new MarkdownTag("<em>", "</em>") },
                { "`", new MarkdownTag("<code>", "</code>", false, false)}
            };
            var text = "`code tag _and em tag in it_`";
            Assert.AreEqual(DoMarkdown(tags, text), "<p><code>code tag _and em tag in it_</code></p>");
        }

        [TestMethod]
        public void Shouldnot_FindTaggedAreas_nearNumbers()
        {
            var tags = new Dictionary<string, MarkdownTag>
            {
                { "_", new MarkdownTag("<em>", "</em>") },
                { "`", new MarkdownTag("<code>", "</code>", false, false)}
            };
            var text = "`123code tag _and em tag in it_`";
            Assert.AreEqual(DoMarkdown(tags, text), "<p>`123code tag <em>and em tag in it</em>`</p>");
        }

        [TestMethod]
        public void Shouldnot_FindTaggedAreas_inTextWithNumbers()
        {
            var tags = new Dictionary<string, MarkdownTag>
            {
                { "_", new MarkdownTag("<em>", "</em>", true, false)},
                { "`", new MarkdownTag("<code>", "</code>", false, false)}
            };
            var text = "lots`1`of _123_numbers_321_ there";
            Assert.AreEqual(DoMarkdown(tags, text), "<p>lots`1`of _123_numbers_321_ there</p>");
        }

        [TestMethod]
        public void Shouldnot_FindEscapedTaggedAreas()
        {
            var tags = new Dictionary<string, MarkdownTag>
            {
                { "_", new MarkdownTag("<em>", "</em>") },
                { "`", new MarkdownTag("<code>", "</code>", false, false)}
            };
            var text = "It is \\_escaped_";
            Assert.AreEqual(DoMarkdown(tags, text), "<p>It is _escaped_</p>");
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
            var text = "All tags _there__ are `unpaired";
            Assert.AreEqual(DoMarkdown(tags, text), "<p>All tags _there__ are `unpaired</p>");
        }

        [TestMethod]
        public void Should_HandleIntersectedTaggedAreas()
        {
            var tags = new Dictionary<string, MarkdownTag>
            {
                { "_", new MarkdownTag("<em>", "</em>") },
                { "__", new MarkdownTag("<strong>", "</strong>")}

            };
            var text = "There __are _some intersecting__ tags_";
            Assert.IsTrue(new[]
            {
                "<p>There <strong>are _some intersecting</strong> tags_</p>",
                "<p>There __are <em>some intersecting__ tags</em></p>"
            }.Contains(DoMarkdown(tags, text)));
        }

        [TestMethod]
        public void Should_HandleMultipleParagraphs()
        {
            var finalFile = "tests/test1.html";
            if (File.Exists(finalFile))
                File.Delete(finalFile);
            Program.WriteContent("config.txt", "tests/test1.txt", finalFile);
            Assert.AreEqual(File.ReadAllText(finalFile), File.ReadAllText("tests/correcttest1.html"));
        }

        [TestMethod]
        public void Should_HandleSophisticatedFile()
        {
            var finalFile = "tests/test2.html";
            if (File.Exists(finalFile))
                File.Delete(finalFile);
            Program.WriteContent("config.txt", "tests/test2.txt", finalFile);
            Assert.AreEqual(File.ReadAllText(finalFile), File.ReadAllText("tests/correcttest2.html"));
        }
    }
}
