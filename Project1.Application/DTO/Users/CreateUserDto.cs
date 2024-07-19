using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Application.DTO.Users
{
    public class CreateUserDto
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public string Organizer { get; set; }
    }

    public class CreateBrandDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateBrandDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Location).NotEmpty().NotNull();
        }
    }
}
