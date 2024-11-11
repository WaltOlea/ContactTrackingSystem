# Contact Tracking System

This project is using MSSQL - LocalDb for storing contact information (Please make sure to have it installed in your sytem).

When running project, Program.cs is automatically runnning the migration included in the project and seeding with default data for testing purpose

Please log in to the systemn using the following credentials. Once logged system is set to remember password, please logout if needed.

UserName: Admin

Password: Test123!

For testing a duplicate entry please use: 

First Name: Walter	

Laste Name: Olea	

Email: waltolea84@gmail.com	

Phone: 832-806-8200	

Zip Code: 77075

# Plugins Used

* jQuery dataTables for displaying the contacts in a grid

* BootStrap for handling mobile screens

* Font Awesome for adding icons to buttons for UX/UI enhancement

# Troubleshooting Tips (Erros found when first time user with VS 2022): 

* When getting the repo make sure to open the contact page solution file to run. 

* Run debug in IIS Express. 

* If you get NuGet error, please right click on soilution and restore NuGet packages.

* If you get IIS Express error, change the port in the properties launchsettings.json and clean the solution