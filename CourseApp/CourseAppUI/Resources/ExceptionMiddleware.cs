using UniversityApp.UI.Exceptions;

namespace CourseAppUI.Resources
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _delegate;

        public ExceptionMiddleware(RequestDelegate requestDelegate)
        {
            _delegate = requestDelegate;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _delegate(httpContext);
            }
            catch(HttpException e) when (e.Status == System.Net.HttpStatusCode.Unauthorized) 
            {
                httpContext.Response.Redirect("/auth/login");
            }
            catch(Exception e)
            {
                httpContext.Response.Redirect("/home/error");
            }
        }
    }
}
