using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Threading;
using System.Web.Mvc;
using Shouter.Models;
using System.Text.RegularExpressions;

namespace Shouter.Controllers
{ 
    public class ShoutsController : Controller
    {
        private ShouterDBContext db = new ShouterDBContext();
        

        public PartialViewResult AddShout(string q)
        {
            string t = Regex.Replace(q, @"<(.|\n)*?>", string.Empty);
            int i = 0;
            Shout s = new Shout();
            s.ShoutText = t;
            s.ShoutCreationTime = DateTime.Now;

            HttpCookie cookie = Request.Cookies.Get("uid");
            
            i = Int32.Parse(cookie.Value);

            User u = new User();
            //u.UserID = i;

            foreach (User v in db.Users)
           {

                if (v.UserID == i)
                {
                    u = v;
                }
            }

            s.User = u;

            db.Shouts.Add(s);
            db.SaveChanges();
            return Shouts();
            
        }

        public PartialViewResult Shouts()
        {
            var shout = db.Shouts.OrderByDescending(c => c.ShoutCreationTime).ToList();
            return PartialView("_Shouts", shout);
            
        }

        //
        // GET: /Shouts/
        public ViewResult Index()
        {
            return View(db.Shouts.OrderByDescending(c => c.ShoutCreationTime).ToList());
        }

        //
        // GET: /Shouts/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Shouts/Create

        [HttpPost]
        public ActionResult Create(Shout shout)
        {
            if (ModelState.IsValid)
            {
                db.Shouts.Add(shout);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(shout);
        }
        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}