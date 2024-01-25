# Kancelaria_Webpage
This project has been made in ASP .NET 6. This application already contains webserver, REST API and database (important: in memory). 

<div align="center">
  <table style="width:100%">
    <tr align="center">
      <td>Package name</td>
      <td>Package version</td>
    </tr>
   <tr>
      <td>Microsoft.EntityFrameworkCore</td>
      <td align="center">6.0.14</td>
    </tr>
   <tr>
      <td>Microsoft.EntityFrameworkCore.InMemory</td>
      <td align="center">6.0.14</td>
    </tr>
   <tr>
      <td>Newtonsoft.Json</td>
      <td align="center">13.0.2</td>
    </tr>
   <tr>
      <td>NReco.Logging.File</td>
      <td align="center">1.1.6</td>
    </tr>
   <tr>
      <td>Microsoft.AspNetCore.Authentication.JwtBearer</td>
      <td align="center">6.0.14</td>
    </tr>
  </table>
</div>

# How to run
1. You have to open solution GB_Webpage.sln in IDE MS Visual Stucio Community 2022.
2. I recommend to use config file below to fill up ```appsettings.json```:
``` json
{
   "Logging":{
      "LogLevel":{
         "Default":"Information",
         "Microsoft.AspNetCore":"Information",
         "Microsoft":"Error"
      },
      "File":{
         "Path":"app.log",
         "Append":true,
         "MinLevel":"Information",
         "FileSizeLimitBytes":0,
         "MaxRollingFiles":0
      }
   },
   "AllowedHosts":"*",
   "ApplicationSettings":{
      "JwtSettings":{
         "DaysValidToken":1,
         "SecretSignatureKey":"",
         "Issuer":"https://+:443"
      },
      "DatabaseSettings":{
         "Paths":{
            "MainFolder":"DatabaseFiles",
            "ArticlesFolder":"Articles",
            "RefreshTokenFolder":"RefreshToken",
            "SuspendedUsersFolder":"SuspendedUsers"
         }
      },
      "UsersSettings":{
         "UserCreditionals":{
            "Login":"",
            "Password":"",
            "Salt":""
         },
         "SuspendedUsersSettings":{
            "SuspendDurationDays":1,
            "MaxLoginAttemps":3
         }
      },
      "FormSettings":{
         "EmailSender":"",
         "EmailReceiver":"",
         "EmailSenderKey":""
      },
      "ArticlesSettings":{
         "ArticlesApiUrl":"https://+:443/api/articles"
      }
   },
   "Kestrel":{
      "EndPoints":{
         "Https":{
            "Url":"https://+:443",
            "Certificate":{
               "Path":"../certificate.crt",
               "KeyPath":"../private.key"
            }
         }
      }
   }
}
```
3. Just start appliacion from IDE MS Visual Studio Community 2022.

# REST API
In this project REST is responsible for adding/updating/deleting articles. Endpoints for such actions are secured by JWT. API also gives access to endpoint login to generate JWT for user.

# Database
Application uses database in memory due to small format data. To bypass disappearing data problem, there is a service called ```DatabaseFileService``` that saves data into physical file in JSON format.
