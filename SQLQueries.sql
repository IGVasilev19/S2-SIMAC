CREATE TABLE Permission
(
    PermissionId int IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL
);

CREATE TABLE [Role]
(
    RoleId int IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL
);


CREATE TABLE RolePermission
(
    RolePermissionId int IDENTITY(1,1) PRIMARY KEY,
    RoleId int NOT NULL FOREIGN KEY REFERENCES [Role](RoleId),
    PermissionId int NOT NULL FOREIGN KEY REFERENCES Permission(PermissionId),
);

CREATE TABLE Organization
(
    OrganizationId int IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL
);

CREATE TABLE Account
(
    AccountId int IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL,
    Email NVARCHAR(50) NOT NULL UNIQUE,
    [Password] NVARCHAR(255) NOT NULL,
    OrganizationId int NULL FOREIGN KEY REFERENCES Organization(OrganizationId),
    RoleId int NOT NULL FOREIGN KEY REFERENCES [Role](RoleId)
);

CREATE TABLE [Status]
(
    StatusId int IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL
);

CREATE TABLE Device
(
    DeviceId int IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL,
    [Location] NVARCHAR(50) NOT NULL,
    StatusId int NOT NULL FOREIGN KEY REFERENCES [Status](StatusId),
    OrganizationId int NOT NULL FOREIGN KEY REFERENCES Organization(OrganizationId)
);

CREATE TABLE [Notification]
(
    NotificationId int IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Content VARCHAR(5000) NOT NULL,
    Important BIT NOT NULL DEFAULT 0,
    [Date] DATETIME NOT NULL,
    PermissionId int NULL FOREIGN KEY REFERENCES Permission(PermissionId),
    DeviceId int NULL FOREIGN KEY REFERENCES Device(DeviceId),
    OrganizationId int NULL FOREIGN KEY REFERENCES Organization(OrganizationId)
)
CREATE TABLE UserReadNotification
(
    UserReadNotificationId int IDENTITY(1,1) PRIMARY KEY,
    AccountId int NOT NULL FOREIGN KEY REFERENCES Account(AccountId),
    NotificationId int NOT NULL FOREIGN KEY REFERENCES [Notification](NotificationId),
); 

-- Disable all constraints temporarily (use with caution)
EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all"

ALTER TABLE [Role]
ADD OrganizationId INT NULL;


ALTER TABLE [Role]
ADD CONSTRAINT FK_Role_Organization
FOREIGN KEY (OrganizationId) REFERENCES Organization(OrganizationId);

EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"

SELECT * FROM Account

insert into [Role] ([Name], OrganizationId) values ('Damaliscus lunatus', 1);
insert into [Role] ([Name], OrganizationId) values ('Macropus rufus', 1);
insert into [Role] ([Name], OrganizationId) values ('Eumetopias jubatus', 1);
insert into [Role] ([Name], OrganizationId) values ('Streptopelia senegalensis', 1);
insert into [Role] ([Name], OrganizationId) values ('Marmota caligata', 1);
insert into [Role] ([Name], OrganizationId) values ('unavailable', 1);
insert into [Role] ([Name], OrganizationId) values ('Uraeginthus angolensis', 1);
insert into [Role] ([Name], OrganizationId) values ('Nannopterum harrisi', 1);
insert into [Role] ([Name], OrganizationId) values ('Eumetopias jubatus', 1);
insert into [Role] ([Name], OrganizationId) values ('Spizaetus coronatus', 1);
insert into [Role] ([Name], OrganizationId) values ('Cacatua tenuirostris', 1);
insert into [Role] ([Name], OrganizationId) values ('Carduelis uropygialis', 1);
insert into [Role] ([Name], OrganizationId) values ('unavailable', 1);
insert into [Role] ([Name], OrganizationId) values ('Speotyte cuniculata', 1);
insert into [Role] ([Name], OrganizationId) values ('Cordylus giganteus', 1);
insert into [Role] ([Name], OrganizationId) values ('Echimys chrysurus', 1);
insert into [Role] ([Name], OrganizationId) values ('Panthera leo persica', 1);
insert into [Role] ([Name], OrganizationId) values ('Phalacrocorax niger', 1);
insert into [Role] ([Name], OrganizationId) values ('Plocepasser mahali', 1);
insert into [Role] ([Name], OrganizationId) values ('Chordeiles minor', 1);
insert into [Role] ([Name], OrganizationId) values ('Cebus albifrons', 1);
insert into [Role] ([Name], OrganizationId) values ('Oreamnos americanus', 1);
insert into [Role] ([Name], OrganizationId) values ('Callipepla gambelii', 1);
insert into [Role] ([Name], OrganizationId) values ('Charadrius tricollaris', 1);
insert into [Role] ([Name], OrganizationId) values ('Chauna torquata', 1);
insert into [Role] ([Name], OrganizationId) values ('Corvus albicollis', 1);
insert into [Role] ([Name], OrganizationId) values ('Spilogale gracilis', 1);

SELECT * FROM [Role]
