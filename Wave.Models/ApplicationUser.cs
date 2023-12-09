using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wave.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [DisplayName("Street address")]
        public string? StreetAdress { get; set; }

        public string? Country { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        [DisplayName("Postal code")]
        public string? PostalCode { get; set; }

        [PersonalData]
        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        [MaxLength(15, ErrorMessage = "Incorrect phone number")]
        public override string PhoneNumber
        {
            get { return base.PhoneNumber; }
            set { base.PhoneNumber = value; }
        }

        [NotMapped]
        public string Role { get; set; }
    }
}
