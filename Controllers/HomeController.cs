using ContactPage.Models;
using ContactPage.Models.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ContactPage.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ContactDbContext _contactDbContext;

        public HomeController(ILogger<HomeController> logger, ContactDbContext contactDbContext)
        {
            _logger = logger;
            _contactDbContext = contactDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ContactViewModel contactViewModel = new ContactViewModel();

            if (HttpContext.Session.Keys.Any())
            {
                // Extract search data from Session
                var firstName = HttpContext.Session.GetString("FirstName");
                var lastName = HttpContext.Session.GetString("LastName");
                var emailAddress = HttpContext.Session.GetString("EmailAddress");
                var phoneNumber = HttpContext.Session.GetString("PhoneNumber");
                var zipCode = HttpContext.Session.GetString("ZipCode");

                // Get list of all contacts then filter by search data
                var allContacts = _contactDbContext.Contacts.ToList();

                if (!string.IsNullOrWhiteSpace(firstName))
                {
                    allContacts = allContacts.Where(f => string.Equals(f.FirstName, firstName, StringComparison.CurrentCultureIgnoreCase)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(lastName))
                {
                    allContacts = allContacts.Where(f => string.Equals(f.LastName, lastName, StringComparison.CurrentCultureIgnoreCase)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(emailAddress))
                {
                    allContacts = allContacts.Where(f => string.Equals(f.EmailAddress, emailAddress, StringComparison.CurrentCultureIgnoreCase)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(phoneNumber))
                {
                    allContacts = allContacts.Where(f => string.Equals(f.PhoneNumber, phoneNumber, StringComparison.CurrentCultureIgnoreCase)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(zipCode))
                {
                    allContacts = allContacts.Where(f => string.Equals(f.ZipCode, zipCode, StringComparison.CurrentCultureIgnoreCase)).ToList();
                }

                // Add search result list to view model to disaplay
                foreach (var contact in allContacts.OrderByDescending(a => a.FirstName))
                {
                    contactViewModel.ContactViews.Add(new ContactView
                    {
                        FirstName = contact.FirstName ?? string.Empty,
                        LastName = contact.LastName ?? string.Empty,
                        EmailAddress = contact.EmailAddress ?? string.Empty,
                        PhoneNumber = contact.PhoneNumber ?? string.Empty,
                        ZipCode = contact.ZipCode ?? string.Empty
                    });
                }

                // Reset the session for next search
                // And display all contacts if user selects list of contacts
                HttpContext.Session.Clear();
            }
            else // If no search entry in Session display complete list
            {
                // Create Model with list of Contacts
                if (_contactDbContext.Contacts.Count() > 0)
                {
                    foreach (var contact in _contactDbContext.Contacts.OrderByDescending(a => a.FirstName))
                    {
                        contactViewModel.ContactViews.Add(new ContactView
                        {
                            FirstName = contact.FirstName ?? string.Empty,
                            LastName = contact.LastName ?? string.Empty,
                            EmailAddress = contact.EmailAddress ?? string.Empty,
                            PhoneNumber = contact.PhoneNumber ?? string.Empty,
                            ZipCode = contact.ZipCode ?? string.Empty
                        });
                    }
                }
            }

            return View(contactViewModel);
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(SearchViewModel model)
        {
            try
            {
                // Flag to identify atleast one search field entered
                bool atLeastOne = false;

                // Add search value to session to filter in the contact list
                if (!string.IsNullOrWhiteSpace(model.FirstName))
                {
                    HttpContext.Session.SetString("FirstName", model.FirstName);
                    atLeastOne = true;
                }

                if (!string.IsNullOrWhiteSpace(model.LastName))
                {
                    HttpContext.Session.SetString("LastName", model.LastName);
                    atLeastOne = true;
                }

                if (!string.IsNullOrWhiteSpace(model.EmailAddress))
                {
                    HttpContext.Session.SetString("EmailAddress", model.EmailAddress);
                    atLeastOne = true;
                }

                if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
                {
                    HttpContext.Session.SetString("PhoneNumber", model.PhoneNumber);
                    atLeastOne = true;
                }

                if (!string.IsNullOrWhiteSpace(model.ZipCode))
                {
                    HttpContext.Session.SetString("ZipCode", model.ZipCode);
                    atLeastOne = true;
                }

                // If no entry entered return error
                if (!atLeastOne)
                {
                    ModelState.AddModelError("Error", "Please enter at least one search item");
                    return View(model);
                }
            }
            catch
            {
                ModelState.AddModelError("Error", "There was a problem");
                return View(model);
            }

            return RedirectToAction("Contact", "Home");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Get a list of all contacts from the db
                    var allContacts = _contactDbContext.Contacts.ToList();

                    // Check for duplicates in the contact db
                    var duplicate = allContacts.Find(f => string.Equals(f.FirstName, model.FirstName, StringComparison.CurrentCultureIgnoreCase)
                        && string.Equals(f.LastName, model.LastName, StringComparison.CurrentCultureIgnoreCase)
                        && string.Equals(f.PhoneNumber, model.PhoneNumber, StringComparison.CurrentCultureIgnoreCase)
                        && string.Equals(f.EmailAddress, model.EmailAddress, StringComparison.CurrentCultureIgnoreCase)
                        && string.Equals(f.ZipCode, model.ZipCode, StringComparison.CurrentCultureIgnoreCase));

                    if (duplicate != null)
                    {
                        ModelState.AddModelError("Error", model.FirstName + " " + model.LastName + " contact already exist");
                        return View(model);
                    }
                    else
                    {
                        _contactDbContext.Add(new Contact
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            PhoneNumber = model.PhoneNumber,
                            EmailAddress = model.EmailAddress,
                            ZipCode = model.ZipCode
                        });

                        await _contactDbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch
            {
                ModelState.AddModelError("Error", "There was a problem");
                return View(model);
            }

            return RedirectToAction("Contact", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}