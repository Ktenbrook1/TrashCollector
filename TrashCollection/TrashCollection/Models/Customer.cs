using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollection.Models
{
    public class Customer
    {  
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Zip Code")]
        public int ZipCode { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        [Display(Name="Day of pickup")]
        public Days Day { get; set; }
        [Required]
        [Display(Name = "Optional extra pickup date")]
        public string RequestOfExtraPickup { get; set; }
        [Required]
        [Display(Name = "Start date")]
        public string StartDate { get; set; }
        [Required]
        [Display(Name = "End date")]
        public string EndDate { get; set; }
        public double Balance { get; set; }

        [ForeignKey("IdentityUser")]
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }
    public enum Days
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday
    }
}
