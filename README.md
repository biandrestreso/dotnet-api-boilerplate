
# .Net Core 6.0 API Boilerplate

This is a fully functional Asp.net core 6.0 Web API Boilerplate, which I set up for personal use to make life a lot easier when you want to set up a project.

Feel free to contribute.

**NOTE:** Remember to configure appsettings.json variables and JWT Token settings. Some basic examples are already set up for you to use.




## Tech Stack

**API:** C#, ASP.NET Core 6, EFCore 6, NLog, Swagger, AutoMapper


## Features

- Logging to local text files using NLog
- Repository Folder Structure
- Repository Wrapper for easy DB calls
- JWT Token issuing and validation with expiry and claims


## Installation

Clone the repo and open it in your favorite IDE. Build/run and if you run into any issues make sure to check if all required NuGet packages are installed, it should install automatically, but that depends on IDE used and user settings.

```bash
  dotnet restore
  or
  nuget restore <projectPath>
```
    
## Environment Variables

To run this project, you will need to add the following environment variables to your appsettings.json file. The default connection is used for EFCore and the Key is nested inside the Jwt Json and is used to generate JWT Tokens.

`DefaultConnection: "Server=[server];Database=[DB];Trusted_Connection=True;"`

`Key: "RandomKey"`


## Acknowledgements

 - [Code Maze ASP.NET Core Series](https://code-maze.com/net-core-series/)


## License


[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)
