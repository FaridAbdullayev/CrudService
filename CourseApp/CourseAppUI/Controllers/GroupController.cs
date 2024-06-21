using CourseAppUI.Filters;
using CourseAppUI.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UniversityApp.UI.Exceptions;
using UniversityApp.UI.Services;

namespace CourseAppUI.Controllers
{
    [ServiceFilter(typeof(AuthFilter))]
    public class GroupController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly ICrudService _crudService;

        public GroupController(ICrudService crudService)
        {
            _httpClient = new HttpClient();
            _crudService = crudService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                return View(await _crudService.GetAllPaginated<GroupListItemGetResource>("groups", page));
            }
            catch (HttpException e)
            {
                if (e.Status == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("login", "auth");
                }
                else return RedirectToAction("error", "home");
            }
            catch (Exception e)
            {
                return RedirectToAction("error", "home");
            }
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(GroupCreateResource createRequest)
        {
            _httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            if (!ModelState.IsValid) return View();

            var content = new StringContent(JsonSerializer.Serialize(createRequest), Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = await _httpClient.PostAsync("https://localhost:7274/api/Groups", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("index");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("login", "account");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    ErrorResponse errorResponse = JsonSerializer.Deserialize<ErrorResponse>(await response.Content.ReadAsStringAsync(), options);

                    foreach (var item in errorResponse.Errors)
                        ModelState.AddModelError(item.Key, item.Message);

                    return View();
                }
                else
                {
                    TempData["Error"] = "Something went wrong!";
                }
            }

            return View(createRequest);
        }


        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                return View(await _crudService.Get<GroupCreateResource>("groups/" + id));
            }
            catch (HttpException e)
            {
                if (e.Status == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("login", "auth");
                }
                else if (e.Status == System.Net.HttpStatusCode.NotFound)
                    return RedirectToAction("error", "home", new { message = "Group not found" });

                else return RedirectToAction("error", "home");
            }
            catch (Exception e)
            {
                return RedirectToAction("error", "home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GroupCreateResource editRequest, int id)
        {
            _httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            if (!ModelState.IsValid) return View();

            var content = new StringContent(JsonSerializer.Serialize(editRequest), Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = await _httpClient.PutAsync("https://localhost:7274/api/Groups/" + id, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("index");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("login", "account");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    ErrorResponse errorResponse = JsonSerializer.Deserialize<ErrorResponse>(await response.Content.ReadAsStringAsync(), options);

                    foreach (var item in errorResponse.Errors)
                        ModelState.AddModelError(item.Key, item.Message);

                    return View();
                }
                else
                {
                    TempData["Error"] = "Something went wrong!";
                }
            }

            return View(editRequest);
        }

        public async Task<IActionResult> Delete(int id)
        {
            _httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            using (var response = await _httpClient.DeleteAsync("https://localhost:7274/api/Groups/" + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500);
                }
            }
        }
    }
}
