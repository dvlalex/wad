using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using data.service;
using data.entity;
using wad.Models;
using HtmlAgilityPack;

namespace wad.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMembershipService _membershipService;
        private readonly IHtmlSnippetService _htmlSnippetService;
        private readonly IHtmlItemService _htmlItemService;
        public HomeController(IMembershipService membershipService, IHtmlSnippetService htmlSnippetService, IHtmlItemService htmlItemService)
        {
            _membershipService = membershipService;
            _htmlSnippetService = htmlSnippetService;
            _htmlItemService = htmlItemService;
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        //
        // POST: /Home/ProcessForm/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessForm(HomeModel model, HttpPostedFileBase fileUpload)
        {
            //check for user and retrieve session id
            var userId = _membershipService.GetUserSessionId(GetLoggedUser());

            //model always valid
            string result;
            if (fileUpload != null)
            {
                result = System.Text.Encoding.UTF8.GetString(
                    new BinaryReader(fileUpload.InputStream).ReadBytes((int) fileUpload.InputStream.Length));
            }
            else 
            {
                result = new WebClient().DownloadString(model.Url);
            }
            //aici start
            Dictionary<int, string> splittedHtml = HtmlSnippetHelper.SplitHTML(result);

            var item = new HtmlItem()
                        {
                            User = _membershipService.GetUserSession(GetLoggedUser()),
                            Snippets = new List<HtmlSnippet>()
                        };

            _htmlItemService.CreateHtmlItem(item);
            var mainhtml = splittedHtml[0];
            foreach (var html in splittedHtml.Where(p=>p.Key!=0) )
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
            //aici ednd
            var sn = new HtmlSnippet()
            {
                HtmlCode = mainhtml,
                DivId = 0
            };
            _htmlSnippetService.CreateHtmlSnippet(sn);
            item.Snippets.Add(sn);
            _htmlItemService.UpdateHtmlItem(item);

            splittedHtml[0] = mainhtml;
            HtmlToSend joinedHtml = HtmlSnippetHelper.JoinHTML(item.Snippets);

            //return redirect item.id

            #region ToBeMoved
            HtmlReceived  received = new HtmlReceived {
                data = joinedHtml.content,
                pageid = item.Id
            };
            string main;
            var res = GetAllSnippets(joinedHtml.content);
            
            var listOfIds = new List<string>();
            main = res.Last();
            foreach (var x in res.Where(s => s != res.Last()))
            {
                HtmlNode hnode = HtmlNode.CreateNode(x);
                string id = hnode.Attributes["data-snippetid"].Value;
                listOfIds.Add(id);
                hnode.Attributes.Remove("data-snippetid");
                var snipp = _htmlSnippetService.RetrieveHtmlSnippet(int.Parse(id));
                snipp.HtmlCode = hnode.OuterHtml;
                _htmlSnippetService.UpdateHtmlSnippet(snipp);
                main = main.Replace(x, string.Format("<div contentid=\"{0}\"></div>", id));

            }
            HtmlItem hitem = _htmlItemService.RetrieveHtmlItem(received.pageid);
            foreach (var sni in hitem.Snippets.Where(i => i.DivId != 0))
            {
                if (!listOfIds.Contains(sni.Id.ToString()))
                    _htmlSnippetService.DeleteHtmlSnippet(sni.Id);
            }
            #endregion
            // aici il aplici pe main
            return Json(new { content = joinedHtml.content, listofsnippets = joinedHtml.listOfSnippets});
        }

        [Authorize]
        public ActionResult Logout()
        {
            _membershipService.Logout();
            return RedirectToAction("Index", "Home");
        }

        #region Helpers
        private string GetLoggedUser()
        {
            if (!_membershipService.IsAuthenticated)
            {
                var userHash = _membershipService.CreateUserAndLogin();
                if (userHash != null)
                {
                    Response.Cookies[0].Expires = DateTime.Now.AddYears(1);
                    return userHash;
                }
            }
            return User.Identity.Name;
        }
        #endregion Helpers

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
