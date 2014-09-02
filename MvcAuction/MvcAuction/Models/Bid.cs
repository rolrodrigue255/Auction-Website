using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcAuction.Models
{
    public class Bid
    {
        public long Id { get; internal set; }

        public long AuctionId { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        public string Username { get; set; }

        [Range(1, double.MaxValue)]
        public decimal Amount { get; set; }

        public Bid()
        {
            Timestamp = DateTime.Now;
        }

    }
}