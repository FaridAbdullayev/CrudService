using Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IGroupService
    {
        int Create(GroupCreateDto createDto);
        PaginatedList<GroupGetDto> GetAll(string? search = null, int page = 1, int size = 10);
        GroupGetDto GetById(int id);
        void Update(GroupUpdateDto updateDto,int Id);
        void Delete(int id);
        List<GroupListItemGetDto> GetAllGroup();
    }
}
