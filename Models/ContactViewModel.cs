namespace ContactPage.Models
{

    public class ContactViewModel
    {
        public List<ContactView> ContactViews = new List<ContactView>();
    }

    public class ContactView
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string ZipCode { get; set; }
    }




}


