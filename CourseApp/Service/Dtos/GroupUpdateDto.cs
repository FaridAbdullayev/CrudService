using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos
{
    public class GroupUpdateDto
    {
        public string No { get; set; }
        public byte Limit { get; set; }
    
    }
    public class GroupUpdateDtoValidator : AbstractValidator<GroupCreateDto>
    {
        public GroupUpdateDtoValidator()
        {
            RuleFor(x => x.No).NotEmpty().MinimumLength(4).MaximumLength(5);
            RuleFor(x => x.Limit).NotEmpty().WithMessage("Bos Ola Bilmez");
        }
    }
}
