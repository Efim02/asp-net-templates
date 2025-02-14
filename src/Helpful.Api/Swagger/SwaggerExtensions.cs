namespace Helpful.Api.Swagger;

using System.Reflection;

using Microsoft.AspNetCore.Builder;
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
        serviceCollection.AddSwaggerGen();

        const string version = "v1";
        var executingAssembly = Assembly.GetCallingAssembly();

        serviceCollection.AddSwaggerGen(swaggerGenOptions =>
        {
            swaggerGenOptions.SwaggerDoc(version,
                new OpenApiInfo { Title = executingAssembly.GetName().Name, Version = version });

            swaggerGenOptions.OperationFilter<SwaggerFileOperationFilter>();

            var assemblyDirectory = Path.GetDirectoryName(executingAssembly.Location);
            var assemblyXml = $"{executingAssembly.GetName().Name}.xml";
            swaggerGenOptions.IncludeXmlComments($"{assemblyDirectory}/{assemblyXml}");
        });
    }

    public static void ParametrizeSwagger(this WebApplication webApplication)
    {
        // Configure the HTTP request pipeline.
        if (!webApplication.Environment.IsDevelopment())
            return;

        webApplication.UseSwagger();
        webApplication.UseSwaggerUI(swaggerUiOptions =>
        {
            swaggerUiOptions.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
            swaggerUiOptions.RoutePrefix = string.Empty;
        });
    }   
}