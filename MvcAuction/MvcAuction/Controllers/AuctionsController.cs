using System;
using System.Collections.Generic;
using System.Linq;
//This reference is needed to use HTTP (ajax)
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcAuction.Models;

namespace MvcAuction.Controllers
{
    public class AuctionsController : Controller
    {
        //
        // GET: /Auctions/
        [AllowAnonymous]
        //Creates an cache with duration of 1 second because this page is updated more often than the homepage
        //A cache with 1 second duration is still enough to take advantage of output caching
        [OutputCache(Duration=1)]
        public ActionResult Index()
        {
            /* Commenting out the hard-coded Auctions, so we can display our database elements
            //creates an array of Auction objects named auctions
            var auctions = new[] {
                new Models.Auction()
                {
                    Title = "Example Auction #1",
                    Description = "This is an example Auction",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddDays(7),
                    StartPrice = 1.00m,
                    CurrentPrice = null,
                },
                new Models.Auction()
                {
                    Title = "Example Auction #2",
                    Description = "This is a second Auction",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddDays(7),
                    StartPrice = 1.00m,
                    CurrentPrice = 30m,
                }
            };
            */

            //Creates a new instance of AuctionsDataContext (if it doesn't already exist)
            var db = new AuctionsDataContext();
            //Uses the toArray to execute a SQL call at this point so we can get the list before passing them to the view
            var auctions = db.Auctions.ToArray();

            return View(auctions);
        }

        //This method saves the Message string to the TempData dictionary and redirects it to the Index method
        //If you call the TempDataDemo method in the url, This message will be sent to the index View page after redirecting
        public ActionResult TempDataDemo()
        {
            TempData["SuccessMessage"] = "The action succeeded!";

            return RedirectToAction("Index");
        }

        //Creating a cache for this controller action actually enables individual, separate caching for each auction object (different ids)
        [OutputCache(Duration = 10)]
        //Passes an id parameter so we can find auction objects by their id using the URL
        public ActionResult Auction(long id)
        {
            /* Commenting out this hard-coded Auction object, so we can use the find method
            //We can call the MvcAuction.model.Auction class without having to reference it using "using MvcAuction.model" in the header
            //This creates an instance of the Auction class and populates some of its properties with values
            var auction = new MvcAuction.Models.Auction()
            {
                Title = "Example Auction",
                Description = "This is an example Auction",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(7),
                StartPrice = 1.00m,
                CurrentPrice = null,
            };
            */

            //To pass our Auction data to the view, we will store it in the ViewData dictionary using the key "Auction", then we will retrieve it in the view using the same key.*/
            //This method will be commented out to instead show how to pass the model directly to the view using the View helper property
            /*
            ViewData["Auction"] = auction;
            return View();
            */

            var db = new AuctionsDataContext();
            var auction = db.Auctions.Find(id);

            //This shows how to use the View helper property to send the model directly to the view
            return View(auction);
        }

        //Creates the Bid action which will be used to add a new bid and update the current price
        [HttpPost]
        //Decorating this action with [ValidateAntiForgeryToken] tells ASP.Net MVC to ensure that every form post to this action includes a valid anti-forgery token value
        [ValidateAntiForgeryToken]
        public ActionResult Bid(Bid bid)
        {
            var db = new AuctionsDataContext();
            var auction = db.Auctions.Find(bid.AuctionId);

            //Checks to see if the auction exists first
            if (auction == null)
            {
                ModelState.AddModelError("AuctionId", "Auction not found!");
            }
            //Checks to see whether the new bid is actually greater than the current price
            else if (auction.CurrentPrice >= bid.Amount)
            {
                ModelState.AddModelError("Amount", "Bid amount must exceed current bid");
            }
            /*If there are no errors (model state is valid), then set bid.username to current user, 
                add bid, update current price, and save changes to db*/
            else
            {
                bid.Username = User.Identity.Name;
                auction.Bids.Add(bid);
                auction.CurrentPrice = bid.Amount;
                db.SaveChanges();
            }

            //Checks to see whether the request is Ajax or not.  If it isn't an Ajax Request, the controller is free to return the redirect action
            //If it is an Ajax request, the controller action should return an http status result that jQuery can analyze to see if the request was successful or not
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Auction", new { id = bid.AuctionId });

            /* This will be commented out because we are going to use the partial view method to update the CurrentPrice
            //Uses httpStatus to check if the request was successful or not and display the appropriate message
            var httpStatus = ModelState.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return new HttpStatusCodeResult(httpStatus);
            */

            //Uses PartialView helper to call the "_CurrentPrice" page after the Ajax Request
            //This will be commented out to instead show how to use the JSON helper method to accomplish a similar function
            /*
                return PartialView("_CurrentPrice", auction);
            */
            
            //Uses JSON helper method to update only the values that we want changed instead of calling a partial view to change the HTML
            //I have added this if statement (not in lab!) to make sure the JSON doesn't update if there is an error in the modelstate
            //If there is an error (auction not found/bid < current price), the Ajax method isn't posted successfully and an error is displayed
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Auction", bid.AuctionId);
            }
            else
            {
                return Json(new
                {
                    CurrentPrice = bid.Amount.ToString("C"),
                    BidCount = auction.BidCount
                });
            }
        }

        //This httpget statement is need to differentiate between the two create functions
        //This action is of type HttpGet because it presents the form view
        [HttpGet]
        public ActionResult Create()
        {
            //This is the list of categories that are to be used for the Category property in the view
            var categoryList = new SelectList(new[] { "Automotive", "Electronics", "Games", "Home" });

            //Sets the categoryList to ViewBag so that we can send it to the view
            ViewBag.CategoryList = categoryList;
            return View();
        }

        //This HttpPost is used for this create function because it processes form post requests
        [HttpPost]
        //Authorize here is used to make sure that only authorized users (user with username "Rolando") are allowed to post auctions
        [Authorize(Users="Rolando")]
        //Calls the strongly-typed view named "Create.cshtml so the user can add new auctions"
        //Adds the auction model as the action parameter so we can bind the model with this action
        //As you can see, we added a Bind statement that EXCLUDES editing the Current Price property
        public ActionResult Create([Bind(Exclude="CurrentPrice")]Models.Auction auction)
        {
            /*Now that we have created validation using Data Annotations, these validation statements will be commented out    
            //This IF statement makes sure a user enters the required title and throws an error if they don't
            if (string.IsNullOrWhiteSpace(auction.Title))
            {
                //Adds a model error to the Model state.  The parameters are the property name key and the error message 
                ModelState.AddModelError("Title", "Title is required!");
            }
            //This IF statement makes sure the title is between 5 and 200 characters long
            else if (auction.Title.Length < 5 || auction.Title.Length > 200)
            {
                ModelState.AddModelError("Title", "Title must be between 5 and 20 characters long!");
            }
            */

            //If the modelstate is valid (no errors), redirect the action to "Index"
            if (ModelState.IsValid)
            {
                //Creates a new instance of AuctionsDataContext, adds new object, and saves changes
                var db = new AuctionsDataContext();
                db.Auctions.Add(auction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //calls the Create function again if the user's input has errors!
            return Create();
        }

        //We are using httppost here to associate this controller action with the form in the partial view "SearchBar"
        [HttpPost]
        //This controller action is for displaying a list of auctions which have a title that contains the string passed in the SearchBar form
        //The parameter name "value" must match the name of the parameter passed to the @html.textbox method in the form
        public ActionResult SearchResults(string value)
        {
            //Creates a new instance of AuctionsDataContext for storing the results of the following query
            var db = new AuctionsDataContext();
            //Queries the variable db to find Auctions which have titles which contain the contents of the string "value" posted in the form
            var results = db.Auctions.Where(x => x.Title.Contains(value));
            //Uses the view helper method to send this variable to the "SearchResults" view for displaying
            return View(results);
        }

        //This controller action is for displaying a list of auctions with a category matching the respective category link in the CategoryNavigation bar
        public ActionResult CategoryResults(string category)
        {
            //Creates a new instance of AuctionsDataContext for storing the results of the following query
            var db = new AuctionsDataContext();
            //Queries the variable db to find Auctions which have categories that match the "category" string passed into the controller action
            var results = db.Auctions.Where(x => x.Category.Contains(category));
            //Uses the view helper method to send this variable to the "CategoryResults" view for displaying
            return View(results);
        }

        // ActionResult which is used to call a partial view for the sidebar which outputs a list of each category along with a link to the respective "CategoryResults" view 
        public ActionResult SidebarCategories(string categories)
        {
            //Creates a new instace of AuctionsDataContext for storing the results of the following query
            var db = new AuctionsDataContext();
            //Queries the variable db to find the a list of distinct categories
            var categoryList = db.Auctions.Select(x => x.Category).Distinct();
            //Casts the categoryList to an Array and stores this in ViewBag.CategoryList so we can use it in the partial view
            ViewBag.CategoryList = categoryList.ToArray();
            //Calls the partial view "SidebarCategories"
            return PartialView();
        }
    }
}
