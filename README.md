# RoundTable
**Overview**

RoundTable is a .NET 9 solution designed to demonstrate the workflow between a web API and a back-office content management system (CMS). This solution comprises the following projects:

- **UmbracoCMS**: Umbraco is a free and open-source .NET content management system. For more information, visit [Umbraco on GitHub](https://github.com/umbraco/Umbraco-CMS).

- **UmbracoBridge**: This is a .NET Web API that provides three anonymous endpoints. It serves as a bridge by calling the corresponding Umbraco endpoints while utilizing managed credentials for authentication. 

Together, these components illustrate the integration and interaction between a web API and a CMS within the .NET ecosystem.

---

# UmbracoCMS Setup Guide

This guide provides step-by-step instructions to set up and configure UmbracoCMS with .NET Core.

## Prerequisites
- .NET Core SDK installed on your system.
- A terminal or command prompt.
- A browser to access the application.

---

## Steps to Set Up UmbracoCMS

### Step 1: Navigate to the UmbracoCMS Project Folder
1. Open a terminal or command prompt.
2. Navigate to the folder where your UmbracoCMS project is located:
   ```bash
   cd path/to/UmbracoCMS
Replace path/to/UmbracoCMS with the actual path to your project folder.

### Step 2: Build and Run the UmbracoCMS Project
1. Build the project:
   ```bash
   dotnet build
Ensure the build completes successfully.

2. Run the project:
   ```bash
   dotnet run
This will start the UmbracoCMS application.

Look for the output in the terminal. You should see something like:
```plain text
[17:17:54 INF] Now listening on: https://localhost:44338
[17:17:54 INF] Now listening on: http://localhost:39749
```
Note the HTTP and HTTPS URLs provided.   

### Step 3: Open the Application in a Browser
1. Open your preferred web browser.
2. Navigate to the URL provided in the terminal output (e.g., http://localhost:39749).
3. You should see the Umbraco setup wizard.

### Step 4: Complete the Setup Wizard
1. Generate the Admin Account:
Follow the on-screen instructions to create an admin account.

2. Provide a username, password, and email address.
Save these credentials securely.

3. Consent for telemetry data:
Decide level of consent telemetry (minimal, basic, detailed).

4. Database Configuration:
Select SQLite as the database type when prompted.
Wait for the setup process to complete. Once finished, you will be redirected to the Umbraco login page.

### Step 5: Log in to the Umbraco Backoffice
Use the admin account credentials you created earlier to log in.
Once logged in, you will be taken to the Umbraco backoffice dashboard.

### Step 6: Create an API User Account
1. Navigate to the Users tab in the backoffice dashboard.
2. Click on the Create button to generate a new API user account.
3. Provide the necessary details for the API user (e.g., username and password).
Assign appropriate permissions to the API user.

### Step 7: Create client credential for API user
1. Go to client credential section and click the Add button
2. Generate the "ClientId" and "ClientSecret"
For more information use the [official documentation](https://docs.umbraco.com/umbraco-cms/fundamentals/data/users/api-users) for the API user

### Step 8: Test UmbracoCMS Managment API
1. Open your preferred web browser.
2. Navigate to https://localhost:<PORT>/umbraco/swagger/index.html?urls.primaryName=Umbraco+Management+API (e.g., https://localhost:44338/umbraco/swagger/index.html?urls.primaryName=Umbraco+Management+API).

---

# UmbracoBridge Setup Guide

### Step 9: Use the generated client credentials to authenticate API requests
1. Navigate to UmbracoBridge proyect 
   ```bash
   cd path/to/UmbracoBridge
2. Edit appsettings.json file
```json text
  "UmbracoCMSSettings": {
    "BaseUrl": "<PROTOCOL>://localhost:<PORT>/umbraco/management/api/v1/",
    "GrantType": "client_credentials",
    "ClientId": "<umbraco-back-office-client-id>",
    "ClientSecret": "<umbraco-back-office-client-secret>"
  }
```
3. Replace "<PROTOCOL>" and "<PORT>" for BaseUrl
4. Replace ClientId and SecretClient values with the proper client credentials
   e.g:
```json text
  "UmbracoCMSSettings": {
    "BaseUrl": "https://localhost:<PORT>/umbraco/management/api/v1/",
    "GrantType": "client_credentials",
    "ClientId": "umbraco-back-office-some-client-id",
    "ClientSecret": "p4ssW0rd53cret!!!"
  }
```

### Step 10: Build and Run the UmbracoBridge Project
1. Build the project:
   ```bash
   dotnet build
Ensure the build completes successfully.

2. Run the project:
   ```bash
   dotnet run

Look for the output in the terminal. You should see something like:
```plain text
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5298
```
Note the URL provided.

### Step 11: Test UmbracoBridge API
1. Open your preferred web browser.
2. Navigate to http://localhost:5298/health to test healthy connection between UmbracoBridge API and UnmbracoCMS.

You should see something like:
```json text
{
  "status": "Healthy",
  "totalDuration": "00:00:00.2763719",
  "entries": {
    "umbraco-cms-check": {
      "data": {
        "Total": 6,
        "Items": [
          {
            "name": "Configuration"
          },
          {
            "name": "Data Integrity"
          },
          {
            "name": "Live Environment"
          },
          {
            "name": "Permissions"
          },
          {
            "name": "Security"
          },
          {
            "name": "Services"
          }
        ]
      },
      "description": "Healthy",
      "duration": "00:00:00.2712149",
      "status": "Healthy",
      "tags": []
    }
  }
}
```
---

# UmbracoBridge Project
### OpenApi & Documentation
Replace the PORT with the actual from listening URL
### Swagger
```plain text
http://localhost:<PORT>/swagger/index.html
```
### Scalar
```plain text
http://localhost:<PORT>/scalar/
```
### Redocs
```plain text
http://localhost:<PORT>/api-docs/index.html
```
