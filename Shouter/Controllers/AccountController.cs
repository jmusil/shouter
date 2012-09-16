using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Shouter.Models;
using System.Data.Entity;
using System.Data;


namespace Shouter.Controllers
{
    public class AccountController : Controller
    {
        private ShouterDBContext db = new ShouterDBContext();
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            int uid = 0;
            if (ModelState.IsValid)
            {
                bool found = false;
                foreach (User u in db.Users)
                {

                    if (u.UserEmail == model.UserEmail)
                    {
                        found = true;
                        uid = u.UserID;
                    }
                    if (u.UserID == null)
                    {
                        u.UserID = 10000;
                    }


                }
                foreach (Shout s in db.Shouts)
                {

                    if (s.UserID == null)
                    {
                        s.UserID = 1;
                        db.SaveChanges();
                    }
                    

                }
                if (!found)
                {
                    User u = new User();
                    u.UserEmail = model.UserEmail;
                    db.Users.Add(u);
                    db.SaveChanges();
                    uid = db.Users.OrderByDescending(c => c.UserID).First().UserID;
                        //Find(model.UserEmail).UserID;
                }



                FormsAuthentication.SetAuthCookie(model.UserEmail, false);
                HttpCookie h = new HttpCookie("uid");
                
                h.Value = uid.ToString();
                h.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(h);

                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Shouts");
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            HttpCookie cookie = new HttpCookie("uid");
            cookie.Expires = DateTime.Now.AddDays(-1);
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
