
CREATE DATABASE [letter-of-no-evidence]
GO

CREATE LOGIN lone_user   
    WITH PASSWORD = '';  
GO  

CREATE USER lone_user FOR LOGIN lone_user;  
GO

GRANT SELECT, INSERT, UPDATE ON [dbo].[Requests] TO lone_user
GO

GRANT SELECT, INSERT, UPDATE ON [dbo].[Payments] TO lone_user
GO