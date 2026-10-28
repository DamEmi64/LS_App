using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Base;

/// <summary>
/// Documents the response body shapes returned with HTTP 400 across API actions.
/// </summary>
public sealed class BadRequestResponseOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var logMessage = context.SchemaGenerator.GenerateSchema(typeof(LogMessage), context.SchemaRepository);
        var logMessages = context.SchemaGenerator.GenerateSchema(typeof(LogMessage[]), context.SchemaRepository);
        var validationProblem = context.SchemaGenerator.GenerateSchema(typeof(ValidationProblemDetails), context.SchemaRepository);

        var responseSchema = new OpenApiSchema
        {
            OneOf = [logMessage, logMessages, validationProblem]
        };

        var response = new OpenApiResponse
        {
            Description = "Bad Request",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new() { Schema = responseSchema },
                ["application/problem+json"] = new() { Schema = validationProblem }
            }
        };
        operation.Responses ??= new OpenApiResponses();
        operation.Responses["400"] = response;
    }
}
