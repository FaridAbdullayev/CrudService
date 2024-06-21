using AutoMapper;
using Core.Entities;
using Data;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Pustok.Helpers;
using Service.Dtos;
using Service.Exceptions;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Service.Exceptions.ResetException;

namespace Service.Services
{
    public class StudentService : IStudentService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStudentRepository _studentRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public StudentService(IWebHostEnvironment webHostEnvironment, IStudentRepository studentRepository,IGroupRepository groupRepository,IMapper mapper)
        {
            _webHostEnvironment = webHostEnvironment;
            _studentRepository = studentRepository;
            _groupRepository = groupRepository;
            _mapper = mapper;
        }
        public int Create(StudentCreateDto createDto)
        {
            Group group = _groupRepository.Get(x => x.Id == createDto.GroupId && !x.IsDeleted);

            if (group == null)
                throw new RestException(StatusCodes.Status404NotFound, "GroupId", "Group not found by given GroupId");

            //if (group.Limit <= group.Students.Count)
            //    throw new RestException(StatusCodes.Status400BadRequest, "Group is full");

            if (_studentRepository.Exists(x => x.Email.ToUpper() == createDto.Email.ToUpper() && !x.IsDeleted))
                throw new RestException(StatusCodes.Status400BadRequest, "Email", "Student already exists by given Email");

            Student student = new()
            {
                FullName = createDto.FullName,
                Email = createDto.Email,
                BirthDay = createDto.BirthDate,
                GroupId = createDto.GroupId,
            };

            if (createDto.FormFile != null)
                student.Image = FileManager.Save(createDto.FormFile, _webHostEnvironment.WebRootPath, "image");

            _studentRepository.Add(student);
            _studentRepository.Save();
            return student.Id;

        }


        public void Delete(int id)
        {
            var data = _studentRepository.Get(x=>x.Id == id && !x.IsDeleted);

            if (data == null)
                throw new RestException(StatusCodes.Status404NotFound, "Not found id");
            data.IsDeleted = true;
            _studentRepository.Save();
        }

        public PaginatedList<StudentGetDto> GetAll(int page = 1, int size = 10)
        {
            var query = _studentRepository.GetAll(x => !x.IsDeleted);

            PaginatedList<Student> students = PaginatedList<Student>.Create(query, page, size);

            return new PaginatedList<StudentGetDto>(_mapper.Map<List<StudentGetDto>>(students.Items), students.TotalPages, students.PageIndex, students.PageSize);
        }

        public StudentGetDto GetById(int id)
        {
            Student entity = _studentRepository.Get(x => x.Id == id && !x.IsDeleted);

            if (entity == null) throw new RestException(StatusCodes.Status404NotFound, "Student not found by given id");

            return _mapper.Map<StudentGetDto>(entity);

        }

        public void Update(StudentUpdateDto updateDto, int Id)
        {
            Group group = _groupRepository.Get(x => x.Id == updateDto.GroupId && !x.IsDeleted);

            if (group == null)
            {
                throw new RestException(StatusCodes.Status404NotFound, "GroupId", "not found group");
            }


            var entity = _studentRepository.Get(x=>x.Id ==Id && !x.IsDeleted);
            if (entity == null)
            {
                throw new RestException(StatusCodes.Status404NotFound, "Id", "not found student");
            }
                //throw new NotFoundException();
            if (entity.Email != updateDto.Email && _studentRepository.Exists(x => x.Email == updateDto.Email && !x.IsDeleted))
            {
                throw new RestException(StatusCodes.Status400BadRequest, "error");
            }
            entity.FullName = updateDto.FullName;
            entity.Email = updateDto.Email;
            entity.BirthDay = updateDto.BirthDate;
            entity.GroupId = updateDto.GroupId;


            _studentRepository.Save();
        }
    }
}
