using ContactPage.Models.Database;

namespace ContactPage.Models
{
    public static class DbInitializer
    {
        public static void SeedData(ContactDbContext dbContext)
        {
            #region Contacts Default Data

            if (!dbContext.Contacts.Any())
            {
                dbContext.AddRange(
                    [
                    new Contact { FirstName="Walter",LastName="Olea",EmailAddress="waltolea84@gmail.com",PhoneNumber="832-806-8200",ZipCode="77075"},
                    new Contact { FirstName="Jose",LastName="Lee",EmailAddress="lee84@gmail.com",PhoneNumber="832-806-8200",ZipCode="77015"},
                    new Contact { FirstName="Victor",LastName="Cruz",EmailAddress="waltolea@yahoo.com",PhoneNumber="832-806-8200",ZipCode="77020"},
                    new Contact { FirstName="John",LastName="Smith",EmailAddress="wal14@hotmail.com",PhoneNumber="832-806-8200",ZipCode="33111"},
                    new Contact { FirstName="Luke",LastName="Cage",EmailAddress="cage111@gmail.com",PhoneNumber="832-806-8200",ZipCode="33010"},
                    new Contact { FirstName="Walter",LastName="Cage",EmailAddress="waltolea84@gmail.com",PhoneNumber="832-806-8200",ZipCode="77075"},
                    new Contact { FirstName="Jose",LastName="Olea",EmailAddress="waltolea84@gmail.com",PhoneNumber="832-806-8200",ZipCode="77015"},
                    new Contact { FirstName="Victor",LastName="Cruz",EmailAddress="waltolea84@gmail.com",PhoneNumber="832-806-8200",ZipCode="33075"}


                    ]
                );
            }

            #endregion Contacts Default Data

            #region User Default Data

            if (!dbContext.Users.Any())
            {
                dbContext.Add( 
                    new User
                    {
                        UserName = "Admin",
                        Password = "Test123!"
                    }
                );
            }

            #endregion

            dbContext.SaveChanges();
        }
    }
}