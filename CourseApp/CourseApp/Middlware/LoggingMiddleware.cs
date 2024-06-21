using Microsoft.AspNetCore.Http.Extensions;
using System.Text;

namespace CourseApp.Middlware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            //try
            //{
            //    httpContext.Request.EnableBuffering();
            //    var requestBody = new StreamReader(httpContext.Request.Body);
            //    var content = await requestBody.ReadToEndAsync();
            //    Console.WriteLine($"Request body: {content}");
            //    httpContext.Request.Body.Position = 0;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Exception reading request: {ex.Message}");
            //}
            try
            {
                httpContext.Request.EnableBuffering();
                string requestBody = await new StreamReader(httpContext.Request.Body, Encoding.UTF8).ReadToEndAsync();
                httpContext.Request.Body.Position = 0;

                using (StreamWriter writer = File.AppendText("wwwroot/txt/file.txt"))
                {
                    writer.WriteLine(requestBody);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception reading request: {ex.Message}");
            }

            var originalBodyStream = httpContext.Response.Body; 
            using var responseBody = new MemoryStream();
            httpContext.Response.Body = responseBody;

            //Console.WriteLine($"Method : {httpContext.Request.Method} - Url :{httpContext.Request.GetDisplayUrl()} - Datetime : {DateTime.Now});
            await _next.Invoke(httpContext);
            //Console.WriteLine($"Url : {httpContext.Request.GetDisplayUrl()} - Status : {httpContext.Response.StatusCode} - Datetime : {DateTime.Now}");

            try
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                string responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();
                responseBody.Seek(0, SeekOrigin.Begin);
                Console.WriteLine($"Response body: {responseBodyText}");
                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception reading response: {ex.Message}");
            }
        }

       
    }
}
