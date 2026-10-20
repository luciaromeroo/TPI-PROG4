using System.Net;
using System.Text.Json;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Web.Middlewares;

// PIEZA COMPARTIDA: traduce las excepciones de dominio a respuestas HTTP para todo el backend.
// NotFoundException -> 404, AppValidationException -> 400,
// InvalidCredentialsException -> 401, NotAllowedException -> 403, cualquier otra -> 500.
public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            await WriteProblem(context, HttpStatusCode.NotFound, "NotFound", ex.Message);
        }
        catch (AppValidationException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            await WriteProblem(context, HttpStatusCode.BadRequest, "ValidationError", ex.Message);
        }
        catch (InvalidCredentialsException ex)
        {
            _logger.LogWarning(ex.Message);
            await WriteProblem(context, HttpStatusCode.Unauthorized, "Unauthorized", ex.Message);
        }
        catch (NotAllowedException ex)
        {
            _logger.LogWarning(ex.Message);
            await WriteProblem(context, HttpStatusCode.Forbidden, "Forbidden", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await WriteProblem(context, HttpStatusCode.InternalServerError, "ServerError", "Ocurrió un error interno en el servidor.");
        }
    }

    private static async Task WriteProblem(HttpContext context, HttpStatusCode statusCode, string title, string detail)
    {
        var problem = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
