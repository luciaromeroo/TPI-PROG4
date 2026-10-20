using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Web.OpenApi;

// Agrega al documento OpenAPI el esquema "Bearer" (JWT). Gracias a esto,
// Swagger UI muestra el botón "Authorize" para pegar el token del login
// y lo manda en el header Authorization de cada request.
internal sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            ["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                In = ParameterLocation.Header,
                BearerFormat = "JWT",
                Description = "Pegá acá el token que devuelve POST /api/authentication/login"
            }
        };

        foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations!))
        {
            operation.Value.Security ??= [];
            operation.Value.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        }

        return Task.CompletedTask;
    }
}
