using Marco.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerServiceCollectionExtensions
    {
        private static readonly string xmlCommentsFilePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, $"{PlatformServices.Default.Application.ApplicationName}.xml");

        public static IServiceCollection AddMarcoSwagger(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            var apiMetaData = configuration.GetSection(nameof(ApiMetaData)).TryGet<ApiMetaData>();

            if (apiMetaData is null)
                throw new ArgumentNullException(nameof(apiMetaData));

            if (string.IsNullOrWhiteSpace(xmlCommentsFilePath))
                throw new ArgumentNullException(nameof(xmlCommentsFilePath));

            services.AddVersionedApiExplorer(
               options =>
               {
                   options.GroupNameFormat = "'v'VVV";
                   options.SubstituteApiVersionInUrl = true;
               });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = ApiVersion.Parse(apiMetaData.DefaultVersion);
            });

            services.AddSwaggerGen(
                    options =>
                    {
                        options.AddSecurityDefinition(
                           "Bearer",
                           new ApiKeyScheme()
                           {
                               In = "header",
                               Description = "Please enter your JWT with the prefix Bearer into the field below.",
                               Name = "Authorization",
                               Type = "apiKey"
                           });

                        options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                            {
                                { "Bearer", Enumerable.Empty<string>() }
                            });

                        var apiVersionDescriptionProvider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Where(a => !a.IsDeprecated)
                        .OrderBy(a => a.ApiVersion.MajorVersion).ThenBy(a => a.ApiVersion.MinorVersion))
                            options.SwaggerDoc(description.GroupName, CreateSwaggerInfoForApiVersion(description, apiMetaData));

                        options.DescribeAllEnumsAsStrings();
                        options.SchemaFilter<SwaggerExcludeFilter>();
                        options.OperationFilter<SwaggerDefaultValues>();
                        options.IncludeXmlComments(xmlCommentsFilePath);
                    });

            return services;
        }

        private static Info CreateSwaggerInfoForApiVersion(ApiVersionDescription description, ApiMetaData apiMetaData)
        {
            var info = new Info()
            {
                Title = apiMetaData.Name,
                Description = apiMetaData.Description,
                Version = description.ApiVersion.ToString()
            };

            if (info.Version.Equals("1.0"))
                info.Description += "<br>Initial version.";
            else
            {
                var versionDescription = apiMetaData.VersionIngDescriptions?[info.Version];
                if (!string.IsNullOrWhiteSpace(versionDescription))
                    info.Description += $"<br>{versionDescription}";
            }

            if (description.IsDeprecated)
                info.Description += "<br><br><span style=\"color: #ff0000;font-weight: bold;\">This version is already deprecated.</span>";

            return info;
        }
    }
}