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

2. To create the LONE database on your local machine

	I.   Open the Nuget package manager console on Visual Studio 2022 Professional
		 ![Package manager console](https://github.com/nationalarchives/ds-letter-of-no-evidence/assets/40386980/deafb43c-9b61-4856-97f6-5ebd46447e25)
	
	II.  If you don't have migration script then run (required only if you made any changes to the data model)
		  `Add-Migration <any name for the script Eg: Initial-17-07-2023>` 
	
	III. To create the database from migration script run (make sure letter-of-no-evidence.api is the start-up project and override the DB connection string in letter-of-no-evidence.api -> Program.cs file with your local)
		  `update-database`
	
3. In Visual Studio right click on the sln and select Properties. Set the solution to be multiple project startup and select letter-of-no-evidence.api and letter-of-no-evidence.web as startup projects in that order
![MultipleProjectStartup](https://github.com/nationalarchives/ds-letter-of-no-evidence/assets/40386980/a005d193-118a-4bfa-b72c-28630075996b)

4. The solution should be ready to run. In VS click the Start button to run. The API will open in a browser showing the swagger page and the Web app will open in another browser window. Note the solution by default runs under IIS Express.

### Using AWS RDS SQL Services for distributed caching in ASP.NET Core Web Applications
1. Open a terminal on your local development machine and install the sql-cache command by executing the following command:
   
   ```dotnet tool install --global dotnet-sql-cache```

2. Use the following command to create the caching schema, replacing [ConnectionString] with the Cache database’s connection string:
   
   ```dotnet sql-cache create "[ConnectionString]" dbo CacheData```

3. Make sure the sql user have the following permission
   
```
   GRANT SELECT, INSERT, UPDATE, DELETE ON [dbo].[CacheData] TO <lone_user>
   GO
```

### Entity relationship diagram

![Letter of no evidence system](https://github.com/nationalarchives/ds-letter-of-no-evidence/assets/40386980/b7c929d8-94d6-45fa-baac-d838f30cdd7c)
