# Introduction
This is the official open source repository for the [Learning Hub](https://learninghub.nhs.uk/) user profile and authentication service.

This repository is used to provide user authentication for the main [Learning Hub repository](https://github.com/TechnologyEnhancedLearning/LearningHub.Nhs.WebUI).

The Learning Hub is the national digital learning platform providing easy access to a wide range of educational resources and support for the health and care workforce and educators.

The Learning Hub is provided and supported the Technology Enhanced Learning platforms team at [NHS England](https://www.england.nhs.uk/).

# Getting Started

## Required installs
- [Visual Studio Professional 2022](https://visualstudio.microsoft.com/downloads/) or other suitable An IDE that supports the Microsoft Tech Stack
  - Make sure you have the [NPM Task Runner](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.NPMTaskRunner) extension
- SQL Server 2019
- [SQL Server Management Studio 18](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)
- [Git](https://git-scm.com/download)
- [Node](https://nodejs.org/en/download/) install specific/required version using NVM - see below.
- [SASS](https://www.sass-lang.com/install) for the command line
    - Specifically, follow the "Install Anywhere (Standalone)" guide. Simply download and extract the files somewhere, and point PATH at the dart-sass folder. This should allow you to use the "sass" command.
    - You don't want to install it via Yarn, as those are JavaScript versions that perform significantly worse.
- [.Net 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Azure storage emulator](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Azure storage explorer](https://azure.microsoft.com/en-gb/features/storage-explorer/#overview)
- [Node version manager (nvm)](https://github.com/coreybutler/nvm-windows/releases) - use this to install and use Node version 16.13.0 and NPM version 8.1.0 to work with this repository.

# Getting the code

Clone the repository from [GitHub](https://github.com/TechnologyEnhancedLearning/LearningHub.Nhs.UserApi):

```bash
git clone git@github.com:TechnologyEnhancedLearning/LearningHub.Nhs.UserApi.git
```

You should now be able to open the solution in your IDE by finding and opening the `LearningHub.Nhs.UserApi.sln` file.

# Build the solution

## Restore the databases

You will need copies of the development databases to run the Learning Hub code. To obtain these, use the details below to become a Learning Hub contributor.

### Restore the ELFH databases
The ELFH database is currently the user store for Learning Hub learners.

Use SSMS to restore the backup of ELFH DB to your local SQL server (using the DB names in the backup).

### Import the Learning Hub data-tier application
Use SSMS "Import Data Tier Application" option to restore the LH database. 

Simplify the database name to "learninghub" or similar to keep connection strings simple.

### Create a SQL Server Login to use in Connection Strings
Because the Learning Hub apps will run in IIS, we can't rely on integrated security for the DB connection (because IIS doesn't use your login) a SQL server login will be needed instead.

Ensure that a SQL server login is created that has db owner access to both the ELFH and Learning Hub databases and use the credentials for this login in connection strings.

## Configure IIS hosting

### Create a self-signed certificate

In Windows 10/11 open Powershell with Administrator privileges and enter the following command:

```console
New-SelfSignedCertificate  -DnsName "*.dev.local", "dev.local", "localhost", "*.e-lfh.org.uk", "*.e-lfhtech.org.uk"  -CertStoreLocation cert:\LocalMachine\My  -FriendlyName "Dev Cert *.dev.local, dev.local, localhost, *.e-lfh.org.uk, *.e-lfhtech.org.uk"  -NotAfter (Get-Date).AddYears(15)
```

### Trust the certificate
The new certificate is not part of any chain of trust and is thus not considered trustworthy by any browsers. To change that, copy the certificate to the certificate store for Trusted Root CAs on your machine. In Windows:

1. Open the **Microsoft Management Console** as Administrator (Go to Start, type `mmc` and run as administrator)
3. From the **File** menu, choose **Add/Remove Snap-In**
4. Choose **Certificates** in left column and then **Add**
5. Choose **Computer Account**  from the **Select Computer** dialogue and then **Next**
6. Ensure **Local Computer** is selected and choose **Finish** to close the **Select Computer** dialogue
7. Choose **OK** to close the **Add or Remove Snap-ins** dialogue
8. In the left column choose **Certificates (Local Computer) / Personal / Certificates**
9. Find the newly created cert (in Windows 10 the column **Friendly name** may help)
10. Select the certificate and hit **Ctrl + C** to copy it to clipboard
11. In the left column choose **Certificates (Local Computer) / Trusted Root CAs / Certificates**
12. Hit **Ctrl-V** to paste your certificate to this store
13. The certificate should appear in the list of **Trusted Root Authorities** and is now considered trustworthy.

### Use in ISS
Run Internet Information Services (IIS) Manager as Administator (Go to Start and type `iis` and run as administrator).

Create individual websites in IIS Manager. The sites should differ by name but not by port number.

- LearningHubWebClient : "https://lh-web.dev.local/"
- LearningHubUserAPIUrl : "https://lh-userapi.dev.local/api/"
- LearningHubApiUrl : "https://lh-api.dev.local/api/"
- LearningHubAdminUrl : https://lh-admin.dev.local/
- LearningHub Auth URL: https://lh-auth.dev.local/

For each site edit the bindings: 
1. Choose **https** 
2. Check the hostname matches the pattern in the certificate (your cert is only valid for `*.dev.local`) 
3. Choose the new certificate 
4. Choose **OK**.

**NOTE:** IIS is a Windows feature. If it is not already installed it can be switched on using the “Turn Windows features on or off“ dialogue.

### Add to hosts
Edit your **hosts** file (C:\Windows\System32\drivers\etc\hosts) and add:
```
127.0.0.1  lh-web.dev.local
127.0.0.1  lh-userapi.dev.local
127.0.0.1  lh-api.dev.local
127.0.0.1  lh-admin.dev.local
127.0.0.1  lh-auth.dev.local
```
## Configure App Settings

Add appsettings.Development.json files to the following projects:
- LearningHub.Nhs.Auth
- LearningHub.Nhs.UserApi

### Modify settings

Go through each of the app settings files and ensure connection strings are correct.

Look for references in each of the App Settings files to the URLs of other parts of the service and update them to the URLs specified above.

If you are an official contributor (see below) working appsettings.Development.json will be provided by the service team.

## Configure Local IIS profile

Create a Local IIS launch profile for the following projects:
- LearningHub.Nhs.Auth
- LearningHub.Nhs.UserApi

1. From the launch drop down choose **Debug Properties**
2. Create a **New** profile
3. Choose **IIS** from the drop down and name the profile **IIS Local**
4. Add the environment name to **Environment variables**: `ASPNETCORE_ENVIRONMENT=Development`
5. Check **Launch browser** if appropriate (Auth)
6. Set the **App URL**. Suggested URLs as follows:
   - LearningHub.Nhs.Auth = https://lh-auth.dev.local
   - LearningHub.Nhs.UserAPI = https://lh-userapi.dev.local
7. Tick the checkbox for **Enable Anonymous Authentication**.

## Add NuGet Package Source
The Learning Hub solutions have some external custom package dependencies. To obtain access to the package source, use the details below to become a Learning Hub contributor.

Open the solution in Visual Studio and add a new custom Nuget package source:

Go to: **Tools > NuGet Package Manager > Manage NuGet Packages for Solution > Cog** icon

Click the **+** button to create a new package source with name `LHPackages` and source: https://pkgs.dev.azure.com/e-LfH/_packaging/LearningHubFeed/nuget/v3/index.json

## Rebuild and run 

1. Rebuild the solution
2. Set **LearningHub.Nhs.WebUI** as the startup Project
3. Start debugging.


# Contribute
If you are interested in contributing to the Learning Hub, please contact [support@learninghub.nhs.uk](mailto:support@learninghub.nhs.uk)
