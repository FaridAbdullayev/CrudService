using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos;
using Service.Exceptions;
using Service.Services;
using Service.Services.Interfaces;

namespace CourseApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService  _groupservice;
        public GroupsController(IGroupService groupService)
        {
             _groupservice = groupService;
        }

        [HttpPost("")]
        public ActionResult Create(GroupCreateDto createDto)
        {
            return StatusCode(201, new { id =  _groupservice.Create(createDto) });
        }

        [HttpGet("")]
        public ActionResult<List<GroupGetDto>> GetAll(string? search = null, int page = 1, int size = 10)
        {
            return Ok( _groupservice.GetAll(search,page,size));
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var data =  _groupservice.GetById(id);
            return StatusCode(200, data);
        }

        [HttpPut("{id}")]
        public ActionResult Update(GroupUpdateDto groupUpdateDto, int id)
        {
             _groupservice.Update(groupUpdateDto, id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
             _groupservice.Delete(id);
            return NoContent();
        }

        [HttpGet("all")]
        public ActionResult<List<GroupListItemGetDto>> GetAllGroups()
        {
            return Ok(_groupservice.GetAllGroup());
        }
    }
}
