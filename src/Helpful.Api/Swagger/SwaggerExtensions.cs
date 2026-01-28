namespace Helpful.Api.Swagger;

using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection" />.
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Подключение Swagger.
    /// - позволяет работать с IFormFile в Swagger.
    /// </summary>
    /// <param name="serviceCollection"> Коллекция сервисов. </param>
    public static void ParametrizeSwagger(this IServiceCollection serviceCollection)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        serviceCollection.AddEndpointsApiExplorer();

        const string version = "v1";
        var executingAssembly = Assembly.GetCallingAssembly();

        serviceCollection.AddSwaggerGen(swaggerGenOptions =>
        {
            var openApiInfo = new OpenApiInfo { Title = executingAssembly.GetName().Name, Version = version };
            swaggerGenOptions.SwaggerDoc(version, openApiInfo);

            swaggerGenOptions.OperationFilter<SwaggerFileOperationFilter>();

            var assemblyDirectory = Path.GetDirectoryName(executingAssembly.Location);
            var assemblyXml = $"{executingAssembly.GetName().Name}.xml";
            swaggerGenOptions.IncludeXmlComments($"{assemblyDirectory}/{assemblyXml}"); 
            
            swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                In = ParameterLocation.Header, 
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey 
            });
            swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { 
                    new OpenApiSecurityScheme 
                    { 
                        Reference = new OpenApiReference 
                        { 
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer" 
                        } 
                    },
                    new string[] { } 
                } 
            });
        });
    }

    public static void ParametrizeSwagger(
        this IApplicationBuilder webApplication,
        IWebHostEnvironment webHostEnvironment)
    {
        // Configure the HTTP request pipeline.
        if (!webHostEnvironment.IsDevelopment())
            return;

        webApplication.UseSwagger();
        webApplication.UseSwaggerUI(swaggerUiOptions =>
        {
            swaggerUiOptions.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
            swaggerUiOptions.RoutePrefix = string.Empty;
        });
    }
}