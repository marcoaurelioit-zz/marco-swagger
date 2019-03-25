[![Build Status](https://dev.azure.com/marcoaurelioit/marco-swagger/_apis/build/status/marcoaurelioit.marco-swagger)](https://dev.azure.com/marcoaurelioit/marco-swagger/_build/latest?definitionId=1)
[![CodeFactor](https://www.codefactor.io/repository/github/marcoaurelioit/marco-swagger/badge)](https://www.codefactor.io/repository/github/marcoaurelioit/marco-swagger)
[![GitHub release](https://img.shields.io/github/release/marcoaurelioit/marco-swagger.svg)](https://github.com/marcoaurelioit/marco-swagger/releases)

# marco-swagger
Package base Swagger for WebApi AspNetCore.

## Nuget Packages
||Version|Downloads|
|---------------------------|:---:|:---:|
|**Marco.Swagger**|[![NuGet](https://img.shields.io/nuget/v/Marco.Swagger.svg)](https://www.nuget.org/packages/Marco.Swagger/)|![NuGet](https://img.shields.io/nuget/dt/Marco.Swagger.svg)|


To use the package you will need to make the following settings in the WebApi Startup

**Configure Services**

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddSwagger(Configuration);
}
```
**Configure Middlewares**
```c#
public void Configure(IApplicationBuilder app, 
    IApiVersionDescriptionProvider apiVersionDescriptionProvider)
{         
    app.UseMvc();
    app.UseSwaggerUI();
}
````
**Add API Meta data in appsettings.json**

```Json
"ApiMetaData": {
    "Name": "WebApi",
    "Description": "Description WebApi em AspNetCore",
    "DefaultVersion": "1.0",
    "VersionIngDescriptions": []
  }
```
