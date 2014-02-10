using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HtmlAgilityPack;
using data.entity;
using data.service;
using wad.Models;

namespace wad.Controllers
{
    public class EditorController : Controller
    {
        private readonly IHtmlSnippetService _htmlSnippetService;
        private readonly IHtmlItemService _htmlItemService;
        public EditorController(IHtmlItemService htmlItemService, IHtmlSnippetService htmlSnippetService)
        {
            _htmlItemService = htmlItemService;
            _htmlSnippetService = htmlSnippetService;
        }

        //
        // GET: /Editor/
        public ActionResult Index(int id = 0)
        {
            ViewBag.Classes = "editor";

            if (id != 0)
            {
                var item = _htmlItemService.RetrieveHtmlItem(id);
                var joinedHtml = HtmlSnippetHelper.JoinHTML(item.Snippets);

                ViewBag.HtmlContent = joinedHtml.content;
                ViewBag.HtmlId = item.Id;
            }
            else
                ViewBag.HtmlContent = "";

            return View();
        }

        [ValidateInput(false)]
        public ActionResult GetRdfFor(int id, string text = "")
        {
            if(id != 0)
            {
                var item = _htmlItemService.RetrieveHtmlItem(id);
                var joinedHtml = HtmlSnippetHelper.JoinHTML(item.Snippets);

                StringBuilder sb = new StringBuilder();
                string[] parts = joinedHtml.content.Split(new char[] { ' ', '\n', '\t', '\r', '\f', '\v', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                int size = parts.Length;
                for (int i = 0; i < size; i++)
                    sb.AppendFormat("{0} ", parts[i]);
                joinedHtml.content = sb.ToString();

                if(text != "")
                {
                    return GetRdf(text, (TypeModel)item.Type);
                }

                return GetRdf(joinedHtml.content, (TypeModel)item.Type);
            }

            return Json(new {}, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public ActionResult SaveItem(int id, string text)
        {
            if(id != 0)
            {
                HtmlReceived received = new HtmlReceived
                {
                    data = text,
                    pageid = id
                };

                string main;
                var res = GetAllSnippets(text);

                var listOfIds = new List<string>();
                main = res.Last();
                foreach (var x in res.Where(s => s != res.Last()))
                {
                    HtmlNode hnode = HtmlNode.CreateNode(x);
                    string idd = hnode.Attributes["data-snippetid"].Value;
                    listOfIds.Add(idd);
                    hnode.Attributes.Remove("data-snippetid");
                    var snipp = _htmlSnippetService.RetrieveHtmlSnippet(int.Parse(idd));
                    snipp.HtmlCode = hnode.OuterHtml;
                    _htmlSnippetService.UpdateHtmlSnippet(snipp);
                    main = main.Replace(x, string.Format("<div contentid=\"{0}\"></div>", idd));

                }
                HtmlItem hitem = _htmlItemService.RetrieveHtmlItem(received.pageid);
                foreach (var sni in hitem.Snippets.Where(i => i.DivId != 0))
                {
                    if (!listOfIds.Contains(sni.Id.ToString()))
                        _htmlSnippetService.DeleteHtmlSnippet(sni.Id);
                }

                Dictionary<int, string> splittedHtml = HtmlSnippetHelper.SplitHTML(main);

                var item = _htmlItemService.RetrieveHtmlItem(id);

                var mainhtml = splittedHtml[0];
                foreach (var html in splittedHtml.Where(p => p.Key != 0))
                {
                    var snippet = new HtmlSnippet()
                    {
                        HtmlCode = html.Value,
                        DivId = html.Key
                    };
                    _htmlSnippetService.CreateHtmlSnippet(snippet);
                    item.Snippets.Add(snippet);
                    mainhtml = mainhtml.Replace(html.Value,
                        string.Format("<div contentid=\"{0}\"></div>", snippet.Id));
                    _htmlItemService.UpdateHtmlItem(item);
                }

                HtmlSnippet sn = hitem.Snippets.First(i => i.DivId == 0);
                item.Snippets.Add(sn);
                _htmlItemService.UpdateHtmlItem(item);

                return Json(new { message = "saved" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { message = "notSaved" }, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        private ActionResult GetRdf(String text, TypeModel type)
        {
            var cl = new HttpClient {BaseAddress = new Uri("http://www.w3.org/")};
            var htmltext = Uri.EscapeDataString(Server.HtmlDecode(text));
            HttpResponseMessage response;
            if(type == TypeModel.Rdfa)
            {
                response = cl.GetAsync("2012/pyRdfa/extract?format=json&text=" + htmltext).Result;
            }
            else
            {
                response = cl.GetAsync("2012/pyMicrodata/extract?format=json&text="+htmltext).Result;
            }

            var rdf = response.Content.ReadAsStringAsync().Result;

            StringBuilder sb = new StringBuilder();
            string[] parts = rdf.Split(new char[] { ' ', '\n', '\t', '\r', '\f', '\v', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            int size = parts.Length;
            for (int i = 0; i < size; i++)
                sb.AppendFormat("{0} ", parts[i]);

            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        private List<string> GetAllSnippets(string html)
        {
            List<string> htmlPart = new List<string>();
            HtmlDocument htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(html);
            ParseHTMLDoc(htmlPart, htmldoc.DocumentNode);
            html = htmldoc.DocumentNode.OuterHtml;
            htmlPart.Add(html);
            return htmlPart;
        }

        public static List<string> ParseHTMLDoc(List<string> content, HtmlNode elem)
        {
            if (elem.Attributes.Contains("data-snippetid") && !content.Any(m => m.Contains(elem.OuterHtml)))
            {
                content.Add(elem.OuterHtml);
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
    }
}
