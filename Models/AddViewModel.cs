using System.ComponentModel.DataAnnotations;

namespace ContactPage.Models
{
    public class AddViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(5)]
        public string ZipCode { get; set; }
    }




}


