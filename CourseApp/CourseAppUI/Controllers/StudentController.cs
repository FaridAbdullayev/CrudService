using CourseAppUI.Filters;
using CourseAppUI.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using UniversityApp.UI.Services;

namespace CourseAppUI.Controllers
{
    [ServiceFilter(typeof(AuthFilter))]
    public class StudentController : Controller
    {
        private HttpClient _client;
        private readonly ICrudService _crudService;

        public StudentController(ICrudService crudService)
        {
            _client = new HttpClient();
            _crudService = crudService;
        }
        public async Task<IActionResult> Index(int page = 1, int size = 4)
        {
            return View(await _crudService.GetAllPaginated<StudentListItemGetResponse>("students", page));
        }
        public async Task<IActionResult> Create()
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            ViewBag.Groups = await getGroups();

            if (ViewBag.Groups == null) return RedirectToAction("error", "home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentCreateResource createRequest)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]); MultipartFormDataContent content = new MultipartFormDataContent();

            var fileContent = new StreamContent(createRequest.FormFile.OpenReadStream());
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(createRequest.FormFile.ContentType);

            content.Add(new StringContent(createRequest.FullName), "FullName");
            content.Add(new StringContent(createRequest.GroupId.ToString()), "GroupId");
            content.Add(new StringContent(createRequest.Email), "Email");
            content.Add(new StringContent(createRequest.BirthDay.ToLongDateString()), "BirthDay");
            //content.Add(new StringContent(JsonSerializer.Serialize(createRequest),System.Text.Encoding.UTF8,"application/json"),"json");
            content.Add(fileContent, "FormFile", createRequest.FormFile.FileName);


            using (var response = await _client.PostAsync("https://localhost:7274/api/students", content))
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
                    ViewBag.Groups = await getGroups();

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
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            using (var response = await _client.GetAsync("https://localhost:7274/api/Students/" + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Groups = await getGroups();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    StudentEditResource request = JsonSerializer.Deserialize<StudentEditResource>(await response.Content.ReadAsStringAsync(), options);
                    return View(request);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("login", "account");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    TempData["Error"] = "Student not found";
                else
                    TempData["Error"] = "Something went wrong!";
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(StudentEditResource studentEdit,int id)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            if (!ModelState.IsValid) return View();
            var content = new StringContent(JsonSerializer.Serialize(studentEdit), Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = await _client.PutAsync("https://localhost:7274/api/Students/" + id, content))
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

            return View(studentEdit);
        }




        private async Task<List<GroupListItemGetResponse>> getGroups()
        {
            using (var response = await _client.GetAsync("https://localhost:7274/api/Groups/all"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var data = JsonSerializer.Deserialize<List<GroupListItemGetResponse>>(await response.Content.ReadAsStringAsync(), options);

                    return data;
                }
            }
            return null;
        }











        public async Task<IActionResult> Delete(int id)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, Request.Cookies["token"]);

            using (var response = await _client.DeleteAsync("https://localhost:7274/api/Students/" + id))
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
