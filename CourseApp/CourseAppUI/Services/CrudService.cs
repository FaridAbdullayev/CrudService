using CourseAppUI.Resources;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using UniversityApp.UI.Exceptions;

namespace UniversityApp.UI.Services
{
    public class CrudService : ICrudService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpClient _client;
        private const string baseUrl = "https://localhost:7274/api/";
        public CrudService(IHttpContextAccessor httpContextAccessor)
        {
            _client = new HttpClient();
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<PaginatedResponseResource<TResponse>> GetAllPaginated<TResponse>(string path, int page)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, _httpContextAccessor.HttpContext.Request.Cookies["token"]);

            using(var response = await _client.GetAsync(baseUrl+path + "?page=" + page))
            {
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<PaginatedResponseResource<TResponse>>(await response.Content.ReadAsStringAsync(),options);
                }
                else throw new HttpException(response.StatusCode);
            }
        }

        public async Task<TResponse> Get<TResponse>(string path)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, _httpContextAccessor.HttpContext.Request.Cookies["token"]);

            using (var response = await _client.GetAsync(baseUrl + path))
            {
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<TResponse>(await response.Content.ReadAsStringAsync(), options);
                }
                else { throw new HttpException(response.StatusCode);}
            }
        }

        
    }
}
