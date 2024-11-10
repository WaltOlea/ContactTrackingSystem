using System.ComponentModel.DataAnnotations;

namespace ContactPage.Models.Database
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }

    }
}
