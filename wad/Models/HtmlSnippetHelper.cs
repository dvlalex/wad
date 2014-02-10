using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using HtmlAgilityPack;
using data.entity;

namespace wad.Models
{
    public class HtmlSnippetHelper
    {
        #region SplitHtml
        public static Dictionary<int, string> SplitHTML(string html)
        {
            html = html.Replace("doctype", "DOCTYPE");
            Dictionary<int, string> htmlPart = new Dictionary<int, string>();
            HtmlDocument htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(html);
            ParseHTMLDoc(htmlPart, htmldoc.DocumentNode);
            html = htmldoc.DocumentNode.OuterHtml;
            //foreach (var iterator in htmlPart)
            //{
            //    html = html.Replace(iterator.Value, 
            //        string.Format("<div contentid=\"{0}\"></div>", iterator.Key));
            //}

            htmlPart.Add(0, html);

            return htmlPart;
        }

        public static Dictionary<int, string> ParseHTMLDoc(Dictionary<int, string> content, HtmlNode elem)
        {
            if (hasRDFaAttribute(elem) && !content.Any(m => m.Value.Contains(elem.OuterHtml)))
            {
                content.Add(content.Count() == 0 ? 1 : content.Max(k => k.Key) + 1, elem.OuterHtml);
            }
            else
            {
                foreach (object child in elem.ChildNodes)
                {
                    if (child.GetType() == typeof(HtmlNode))
                    {
                        content = ParseHTMLDoc(content, (HtmlNode)child);
                    }
                }
            }

            return content;
        }
        public static bool hasRDFaAttribute(HtmlNode el)
        {
            string[] attr = 
            {
                "vocab", "itemscope" 
            };
            foreach (var a in attr)
            {
                if (el.Attributes.Contains(a))
                    return true;

            }
            return false;
        }
        #endregion

        #region JoinHtml

        public static HtmlToSend JoinHTML(List<HtmlSnippet> snippets)
        {
            var mainHTML = snippets.Where(k => k.DivId == 0).Single();
            var listOfSnippets = new List<string>();
            foreach (var s in snippets.Where(k => k.DivId != 0))
            {
                HtmlNode node = HtmlNode.CreateNode(s.HtmlCode);
                node.Attributes.Add("data-snippetId", s.Id.ToString());
                listOfSnippets.Add(s.Id.ToString());

                mainHTML.HtmlCode = mainHTML.HtmlCode.Replace(string.Format("<div contentid=\"{0}\"></div>", s.Id), node.OuterHtml);
            }
            return new HtmlToSend { content = mainHTML.HtmlCode, listOfSnippets = listOfSnippets };
        }

        #endregion 
    }
    public class HtmlToSend
    {
        public string content;
        public List<string> listOfSnippets;
    }

    public class HtmlReceived
    {
        public string data;
        public int pageid;
    }
    
}