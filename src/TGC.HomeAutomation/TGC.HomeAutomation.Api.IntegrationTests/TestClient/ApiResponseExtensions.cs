using System.Net;
using TGC.HomeAutomation.API.IntegrationTests.Generated;

namespace TGC.HomeAutomation.Api.IntegrationTests.TestClient;

internal static class ApiResponseExtensions
{
	public static Task<SwaggerResponse<T>> AssertOk<T>(this Task<SwaggerResponse<T>> request)
		=> AssertSuccess(request, HttpStatusCode.OK);

	public static Task<SwaggerResponse> AssertOk(this Task<SwaggerResponse> request)
		=> AssertSuccess(request, HttpStatusCode.OK);

	public static Task<SwaggerResponse> AssertNoContent(this Task<SwaggerResponse> request)
		=> AssertSuccess(request, HttpStatusCode.NoContent);

	public static Task<ApiException> AssertUnauthorized(this Task<SwaggerResponse> request)
		=> AssertError(request, HttpStatusCode.Unauthorized);

	public static Task<ApiException> AssertForbidden(this Task<SwaggerResponse> request)
		=> AssertError(request, HttpStatusCode.Forbidden);

	public static Task<ApiException> AssertNotFound(this Task<SwaggerResponse> request)
		=> AssertError(request, HttpStatusCode.NotFound);

	public static Task<ApiException> AssertUnauthorized<T>(this Task<SwaggerResponse<T>> request)
		=> AssertError(request, HttpStatusCode.Unauthorized);

	public static Task<ApiException> AssertNotFound<T>(this Task<SwaggerResponse<T>> request)
		=> AssertError(request, HttpStatusCode.NotFound);

	public static Task<ApiException> AssertBadRequest(this Task<SwaggerResponse> request)
		=> AssertError(request, HttpStatusCode.BadRequest);

	public static Task<ApiException> AssertBadRequest<T>(this Task<SwaggerResponse<T>> request)
		=> AssertError(request, HttpStatusCode.BadRequest);

	public static Task<ApiException> AssertForbidden<T>(this Task<SwaggerResponse<T>> request)
		=> AssertError(request, HttpStatusCode.Forbidden);

	public static async Task<ApiException> AssertError<T>(this Task<SwaggerResponse<T>> request, HttpStatusCode httpStatusCode)
	{
		try
		{
			var result = await request;
			Assert.Fail($"HTTP status code was {result.StatusCode}");
		}
		catch (ApiException ex)
		{
			Assert.Equal((int)httpStatusCode, ex.StatusCode);
			return ex;
		}

		Assert.Fail("No ApiException was thrown");
		return null;
	}

	public static async Task<ApiException> AssertError(this Task<SwaggerResponse> request, HttpStatusCode httpStatusCode)
	{
		try
		{
			var result = await request;
			Assert.Fail($"HTTP status code was {result.StatusCode}");
		}
		catch (ApiException ex)
		{
			Assert.Equal((int)httpStatusCode, ex.StatusCode);
			return ex;
		}

		Assert.Fail("No ApiException was thrown");
		return null;
	}
	public static async Task<SwaggerResponse> AssertSuccess(this Task<SwaggerResponse> request, HttpStatusCode httpStatusCode)
	{
		var result = await request;
		Assert.Equal((int)httpStatusCode, result.StatusCode);
		return result;
	}
	public static async Task<SwaggerResponse<T>> AssertSuccess<T>(this Task<SwaggerResponse<T>> request, HttpStatusCode httpStatusCode)
	{
		var result = await request;
		Assert.Equal((int)httpStatusCode, result.StatusCode);
		return result;
	}
}
