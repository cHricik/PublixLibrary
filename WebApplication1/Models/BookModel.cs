using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class BookModel
    {
       
        [StringLength(maximumLength: 128, ErrorMessage = "The length of the Publisher should be 128 characters")]
        public String Publisher { get; set; }

        [StringLength(maximumLength: 128, ErrorMessage = "The length of the Author should be 128 characters")]
        [Required]
        public String Author { get; set; }
        
        [StringLength(maximumLength: 128, MinimumLength = 5, ErrorMessage = "The length of the title should be between 5 and 128 characters.")]
        [Required]
        public String Title { get; set; }
       
        [Range(minimum: 0.25, maximum: 250, ErrorMessage = "Please enter a cost between $0.25 and $250.00")]
        [Required]
        public Decimal Cost { get; set; }

        public int Id { get; set; }

        public System.Web.Mvc.SelectList AvailablePublishers { get; set; }
    }
}