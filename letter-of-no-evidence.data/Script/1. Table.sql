IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [PaymentStatus] (
    [Id] int NOT NULL,
    [Description] nvarchar(20) NOT NULL,
    [Meaning] nvarchar(250) NOT NULL,
    CONSTRAINT [PK_PaymentStatus] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Requests] (
    [Id] int NOT NULL IDENTITY,
    [RequestNumber] nvarchar(20) NOT NULL,
    [SubjectFirstName] nvarchar(50) NOT NULL,
    [SubjectLastName] nvarchar(50) NOT NULL,
    [AlternativeFirstName] nvarchar(50) NULL,
    [AlternativeLastName] nvarchar(50) NULL,
    [DateOfBirth] nvarchar(30) NOT NULL,
    [DateOfDeath] nvarchar(30) NULL,
    [CountryOfBirth] nvarchar(100) NULL,
    [ContactTitle] nvarchar(30) NULL,
    [ContactFirstName] nvarchar(50) NULL,
    [ContactLastName] nvarchar(50) NOT NULL,
    [ContactAddress1] nvarchar(100) NOT NULL,
    [ContactAddress2] nvarchar(100) NULL,
    [ContactCity] nvarchar(100) NOT NULL,
    [ContactCounty] nvarchar(100) NULL,
    [ContactPostCode] nvarchar(30) NOT NULL,
    [ContactCountry] nvarchar(100) NOT NULL,
    [LetterToRequestor] bit NOT NULL,
    [AgentCompanyName] nvarchar(100) NULL,
    [AgentFirstName] nvarchar(50) NULL,
    [AgentLastName] nvarchar(50) NULL,
    [AgentAddress1] nvarchar(100) NULL,
    [AgentAddress2] nvarchar(100) NULL,
    [AgentCity] nvarchar(100) NULL,
    [AgentCounty] nvarchar(100) NULL,
    [AgentPostCode] nvarchar(30) NULL,
    [AgentCountry] nvarchar(100) NULL,
    [ContactEmail] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Requests] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Payments] (
    [Id] int NOT NULL IDENTITY,
    [SessionId] nvarchar(20) NOT NULL,
    [PaymentId] nvarchar(30) NULL,
    [PaymentStatusId] int NOT NULL,
    [ProcessFinished] bit NOT NULL,
    [TransactionDate] datetime2 NOT NULL,
    [Amount] decimal(4,2) NOT NULL,
    [RequestId] int NOT NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Payments_PaymentStatus_PaymentStatusId] FOREIGN KEY ([PaymentStatusId]) REFERENCES [PaymentStatus] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Payments_Requests_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [Requests] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Payments_PaymentStatusId] ON [Payments] ([PaymentStatusId]);
GO

CREATE INDEX [IX_Payments_RequestId] ON [Payments] ([RequestId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230722095357_initial-22-07-2023', N'7.0.9');
GO

COMMIT;
GO

CREATE USER lone_user FOR LOGIN lone_user;  
GO

GRANT SELECT, INSERT, UPDATE ON [dbo].[Requests] TO lone_user
GO

GRANT SELECT, INSERT, UPDATE ON [dbo].[Payments] TO lone_user
GO

GRANT SELECT, INSERT, UPDATE ON [dbo].[PaymentStatus] TO lone_user
GO

GRANT SELECT, INSERT, UPDATE ON [dbo].[__EFMigrationsHistory] TO lone_user
GO