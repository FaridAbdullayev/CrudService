using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos
{
    public class StudentUpdateDto
    {
        public int GroupId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class StudentUpdateDtoValidator : AbstractValidator<StudentCreateDto>
    {
        public StudentUpdateDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().MaximumLength(35).MinimumLength(5);

            RuleFor(x => x.Email).EmailAddress().NotEmpty().MaximumLength(100).MinimumLength(5);

            RuleFor(x => x.BirthDate.Date)
                .LessThanOrEqualTo(DateTime.Now.AddYears(-15).Date)
                    .WithMessage("Student's age must be greator or equal than 15");

            
        }
    }
}
