# Letter of no evidence (LONE)

## Prerequisite

Download and install:

- Visual Studio 2022 Professional
- .NET 6.0
- Docker Desktop
- SQL Server Express version at least
- Azure Data Studio (for Mac users)


### Windows setup instructions
1. Open the Visual Studio 2022 Professional as "administrator" and clone the ds-letter-of-no-evidence repository to the local folder
![Package manager console](https://github.com/nationalarchives/ds-letter-of-no-evidence/assets/40386980/deafb43c-9b61-4856-97f6-5ebd46447e25)

2. To create the LONE database on your local machine

	I. Open the Nuget package manager console on Visual Studio 2022 Professional
	
	II. Run the following commands 
		If you don't have migration script then run `Add-Migration <any name for the script Eg: Initial-17-07-2023>` 
	
		To create the database from migration script run `update-database`
	
3. In Visual Studio right click on the sln and select Properties. Set the solution to be multiple project startup and select letter-of-no-evidence.api and letter-of-no-evidence.web as startup projects in that order
![MultipleProjectStartup](https://github.com/nationalarchives/ds-letter-of-no-evidence/assets/40386980/a005d193-118a-4bfa-b72c-28630075996b)

4. The solution should be ready to run. In VS click the Start button to run. The API will open in a browser showing the swagger page and the Web app will open in another browser window. Note the solution by default runs under IIS Express.


### Entity relationship diagram

![Letter of no evidence system](https://github.com/nationalarchives/ds-letter-of-no-evidence/assets/40386980/11141264-fbaf-4ddd-87ac-c24cc40d0296)