using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using data.service;
using wad.Models;

namespace wad.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMembershipService _membershipService;
        public HomeController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
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
                return Json(new {html = result});
            }

            result = new WebClient().DownloadString(model.Url);
            return Json(new {html = result});
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
    }


}
