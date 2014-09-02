using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//This reference is needed to use the methods used in the SwitchView controller action
using System.Web.WebPages;
using MvcAuction.Models;

namespace MvcAuction.Controllers
{
    //AllowAnonymous allows anyone to view all the controller actions specified in the Home controller
    [AllowAnonymous]
    public class HomeController : Controller
    {
        //This statement causes the Index page to be cached for a duration of 3 seconds
        [OutputCache(Duration=3)]
        public ActionResult Index()
        {
            ViewBag.Message = "This page was rendered at " + DateTime.Now;

            return View();
        }

        //Because this navigation bar hardly updatesand we don't want to make a ton of DB calls, we will cache it every hour
        [OutputCache(Duration = 3600)]
        //This action will access the database, find the categories of the Auctions in the database and displays them in the navigation bar
        public ActionResult CategoryNavigation()
        {
            var db = new AuctionsDataContext();
            //Selects distincts categories to display in the navigation bar
            var categories = db.Auctions.Select(x => x.Category).Distinct();
            ViewBag.Categories = categories.ToArray();

            return PartialView();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //This controller action is used to send mobile users to the desktop version of the website (redirects them to page they are currently on)
        public ActionResult SwitchView(string returnUrl, bool mobile = false)
        {
            //First statement clears any overriden browser settings we might have done previously
            HttpContext.ClearOverriddenBrowser();
            //This statement sets the Overridden Browswer setting to mobile or desktop depending on what the user is requesting
            HttpContext.SetOverriddenBrowser(
                mobile ? BrowserOverride.Mobile : BrowserOverride.Desktop);

            return Redirect(returnUrl);
        }
    }
}
