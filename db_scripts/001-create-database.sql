CREATE DATABASE MiniPdf;
GO

USE MiniPdf;

CREATE TABLE AppUser
(
    id NVARCHAR PRIMARY KEY not null,
    name NVARCHAR(500) not null,
    email NVARCHAR(200) not null,
    remainingCompressions INTEGER
);
GO
