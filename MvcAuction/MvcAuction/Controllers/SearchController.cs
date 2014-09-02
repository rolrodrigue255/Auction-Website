using System;
using System.Collections.Generic;
using System.Linq;
//This reference is needed to create an asynchronous controller
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcAuction.Controllers
{
    public class SearchController : AsyncController
    {
        //This statement creates an asynchronous task that takes in a keyword to use to search the Auctions database
        //To use this action, we need to set the URL to "~/Search/Auctions?Keyword= " followed by the search terms (title of auction)
        public async Task<ActionResult> Auctions (string keyword)
        {
            //await keyword forces it to wait for the task to complete then continues executing the method when it does
            var auctions = await Task.Run<IEnumerable<Models.Auction>>(
                () =>
                {
                    var db = new Models.AuctionsDataContext();
                    //Uses the Contains method to find any title that contains the keywords that are passed as parameters to the search controller
                    return db.Auctions.Where(x => x.Title.Contains(keyword)).ToArray();

                });

            //After finding matching titles, the JSON is returned to the browser
            return Json(auctions, JsonRequestBehavior.AllowGet);
        }
    }
}
