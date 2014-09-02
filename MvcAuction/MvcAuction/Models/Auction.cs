using System;
using System.Collections.Generic;
//This reference is needed to use Collections with the object model
using System.Collections.ObjectModel;
//This reference is needed to use data annotations to validate the property values
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcAuction.Models
{
    public class Auction
    {
        //Required data annotation makes it so the ID property value is needed  
        //This property is needed to add objects to the data context as well
        [Required]
        public long Id { get; set; }

        //Data annotation makes Category required and creates a text type form to take user input
        [Required]
        [DataType(DataType.Text)]
        public string Category { get; set; }

        //Makes Title required, is of type text, and has a minimum of 5 and maximum of 200 characters
        [Required]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 200, MinimumLength = 5)]
        public string Title { get; set; }

        //Sets a multiline text data field for the description property
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        //Sets a ImageURL data field for the imageurl property and sets Display attribute 
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        //Sets DateTime data field for StartTime and EndTime properties
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        //Sets the Datafield to that of type Currency to StarPrice and CurrentPrice
        [DataType(DataType.Currency)]
        [Display(Name = "Starting Price")]
        public decimal StartPrice { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Current Bid Price")]
        public decimal? CurrentPrice { get; set; }

        //Makes this property virtual so Entity Framework can override it when it retrieves this data from the DB
        //Also we use private set because we don't want anyone other than this class and Entity Framework to change this property
        public virtual Collection<Bid> Bids { get; private set; }

        //This property will simply return the number of bids that bid collection contains
        public int BidCount
        {
            get { return Bids.Count; }

        }

        public Auction()
        {
            //We initialize the bids collection in the Auction constructor to make sure it's never null
            Bids = new Collection<Bid>();
        }
    
    }
}