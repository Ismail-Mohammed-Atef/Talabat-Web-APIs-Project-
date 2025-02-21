
namespace Talabat.APIS.Errors
{
	public class ApiResponse
	{
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse(int statusCode,string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(StatusCode);
        }

		private string? GetDefaultMessageForStatusCode(int statuscode)
		{
            return statuscode switch
            {
                400 => "a bad request happened",
                401 => "you are not authorized",
                404 => "resource not found",
                500 => "server error",
                _ => null
            };
		}
	}
}
