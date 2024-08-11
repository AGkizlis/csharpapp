namespace CSharpApp.Api.Configuration
{
	public static class HttpRequestExceptionExtensions
	{
		public static IResult ConvertToResult(this HttpRequestException exception)
		{
			/*
			 * This method should be enough for most cases. However, that depends on
			 * business requirements. So, depending on who makes the endpoint calls
			 * and what level of detail is expected on the result in case of failure,
			 * this method might need adjustments. It is even possible that each endpoint
			 * will need different handling and this method should be removed.
			 */
			return exception.StatusCode switch
			{
				System.Net.HttpStatusCode.NotFound => Results.NotFound(),
				System.Net.HttpStatusCode.ServiceUnavailable => Results.StatusCode(StatusCodes.Status503ServiceUnavailable),
				_ => Results.NoContent(),
			};
		}
	}
}
