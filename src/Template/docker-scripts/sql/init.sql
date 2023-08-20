USE [TemplateApp];

CREATE LOGIN app_sa WITH PASSWORD = 'app123';
CREATE USER app_sa FOR LOGIN app_sa;

ALTER ROLE db_datareader ADD MEMBER app_sa;
ALTER ROLE db_datawriter ADD MEMBER app_sa;