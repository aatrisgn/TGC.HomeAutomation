using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TGC.HomeAutomation.API;

public class ApiExceptionHandler : IExceptionHandler
{
	private readonly ILogger<ApiExceptionHandler> logger;
	public ApiExceptionHandler(ILogger<ApiExceptionHandler> logger)
	{
		this.logger = logger;
	}

	public ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		logger.LogError("Unhandled exception: {Message}", exception.Message);
		httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
		httpContext.Response.ContentType = "application/json";
		var problemDetails = new ProblemDetails
		{
			Status = StatusCodes.Status500InternalServerError,
			Title = "An unexpected error occurred.",
			Detail = exception.Message
		};
		var json = JsonSerializer.Serialize(problemDetails);
		return new ValueTask<bool>(httpContext.Response.WriteAsync(json, cancellationToken).ContinueWith(_ => true, cancellationToken));
	}
}
