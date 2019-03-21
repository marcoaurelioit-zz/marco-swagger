using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMarcoSwagger(this IApplicationBuilder applicationBuilder,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (applicationBuilder == null)
                throw new ArgumentNullException(nameof(applicationBuilder));

            if (apiVersionDescriptionProvider == null)
                throw new ArgumentNullException(nameof(apiVersionDescriptionProvider));

            applicationBuilder.UseApiVersioning();
            applicationBuilder.UseSwagger();
            applicationBuilder.UseSwaggerUI(
                options =>
                {
                    options.DefaultModelsExpandDepth(-1);
                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                });

            return applicationBuilder;
        }
    }
}