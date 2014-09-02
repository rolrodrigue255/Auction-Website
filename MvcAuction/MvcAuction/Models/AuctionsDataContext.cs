using System;
using System.Collections.Generic;
//This reference is needed to use the Entity Framework
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcAuction.Models
{
    public class AuctionsDataContext : DbContext
    {
        //Adds auction model to our data context
        public DbSet<Auction> Auctions { get; set; }

        //Statement that Drops and Recreates the Database if the Model class is changed after initial Database creation
        //The contents of the Database will be DROPPED if you enable this method by changing the model properties!!
        static AuctionsDataContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AuctionsDataContext>());
        }
    }
}