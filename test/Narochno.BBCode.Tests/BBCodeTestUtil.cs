using Narochno.BBCode.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Narochno.BBCode.Tests
{
    public static class BBCodeTestUtil
    {
        public static SequenceNode CreateRootNode(BBTag[] allowedTags)
        {
            var node = new SequenceNode();
            AddSubnodes(allowedTags, node);
            return node;
        }
        static SyntaxTreeNode CreateNode(BBTag[] allowedTags, bool allowText)
        {
                var tag = allowedTags.First();
                var node = new TagNode(tag);

                if (tag.Attributes != null)
                {
                    var selectedIds = new List<string>();
                    foreach (var attr in tag.Attributes)
                    {
                        if (!selectedIds.Contains(attr.ID))
                        {
                            var val = "hello";
                            node.AttributeValues[attr] = val;
                            selectedIds.Add(attr.ID);
                        }
                    }
                }
                return node;
        }
        static void AddSubnodes(BBTag[] allowedTags, SyntaxTreeNode node)
        {
            int count = 3;
            bool lastWasText = false;
            for (int i = 0; i < count; i++)
            {
                var subNode = CreateNode(allowedTags, !lastWasText);
                lastWasText = subNode is TextNode;
                node.SubNodes.Add(subNode);
            }
        }

        public static BBCodeParser GetParserForTest(ErrorMode errorMode, bool includePlaceholder, BBTagClosingStyle listItemBBTagClosingStyle, bool enableIterationElementBehavior)
        {
            var tags = new List<BBTag>
            {
                new BBTag("url2", "<a href=\"${href}\">", "</a>", new BBAttribute("href", "", GetUrl2Href), new BBAttribute("href", "href", GetUrl2Href))
            };

            tags.AddRange(BBCodeParser.DefaultTags);

            // Some tests need to customise the list item tag
            var listItem = tags.Single(x => x.Name == "*");
            tags.Remove(listItem);
            tags.Add(new BBTag(listItem.Name, listItem.OpenTagTemplate, listItem.CloseTagTemplate, true, listItemBBTagClosingStyle, listItem.ContentTransformer, enableIterationElementBehavior));

            if (includePlaceholder)
            {
                tags.Add(new BBTag("placeholder", "${name}", "", false, BBTagClosingStyle.LeafElementWithoutContent, null, new BBAttribute("name", "", name => "xxx" + name.AttributeValue + "yyy")));
            }

            return new BBCodeParser(errorMode, null, tags);
        }
        static string GetUrl2Href(IAttributeRenderingContext attributeRenderingContext)
        {
            if (!string.IsNullOrEmpty(attributeRenderingContext.AttributeValue)) return attributeRenderingContext.AttributeValue;

            var content = attributeRenderingContext.GetAttributeValueByID(BBTag.ContentPlaceholderName);
            if (!string.IsNullOrEmpty(content) && content.StartsWith("http:")) return content;

            return null;
        }

        public static BBCodeParser GetSimpleParserForTest(ErrorMode errorMode)
        {
            return new BBCodeParser(errorMode, null, new[]
                {
                    new BBTag("x", "${content}${x}", "${y}", true, true, new BBAttribute("x", "x"), new BBAttribute("y", "y", x => x.AttributeValue)), 
                });
        }

        public static string SimpleBBEncodeForTest(string bbCode, ErrorMode errorMode)
        {
            return GetSimpleParserForTest(errorMode).ToHtml(bbCode);
        }

        public static bool IsValid(string bbCode, ErrorMode errorMode)
        {
            try
            {
                BBCodeParserTest.BBEncodeForTest(bbCode, errorMode);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static SequenceNode GetAnyTree()
        {
            var parser = GetParserForTest(ErrorMode.Strict, true, BBTagClosingStyle.AutoCloseElement, false);
            return CreateRootNode(parser.Tags.ToArray());
        }
    }
}