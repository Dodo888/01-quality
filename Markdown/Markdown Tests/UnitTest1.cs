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
            string text = "It_should_change";
            var result = markdownMaker.FindTaggedArea(text);
            Assert.AreEqual(result, "It<em>should</em>change");
        }
    }
}
