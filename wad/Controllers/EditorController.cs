using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace wad.Controllers
{
    public class EditorController : Controller
    {
        //
        // GET: /Editor/
        public ActionResult Index()
        {
            ViewBag.Classes = "editor";

            return View();
        }

        [ValidateInput(false)]
        public ActionResult GetRdf(String text)
        {
            var cl = new HttpClient {BaseAddress = new Uri("http://www.w3.org/")};
            var htmltext = Uri.EscapeDataString(text);
            var response = cl.GetAsync("2012/pyRdfa/extract?format=json&text="+htmltext).Result;
            var rdf = response.Content.ReadAsStringAsync().Result;

            StringBuilder sb = new StringBuilder();
            string[] parts = rdf.Split(new char[] { ' ', '\n', '\t', '\r', '\f', '\v', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            int size = parts.Length;
            for (int i = 0; i < size; i++)
                sb.AppendFormat("{0} ", parts[i]);

            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}
