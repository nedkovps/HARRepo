using FluentValidation;
using HARRepo.FileManager.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HARRepo.FileManager.API.Validators
{
    public class FileShareValidator : AbstractValidator<FileShareModel>
    {
        public FileShareValidator()
        {
            RuleFor(x => x.FileId)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithMessage("File is required.");

            RuleFor(x => x.UserEmail)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithMessage("User email is reuired.")
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                .WithMessage("User email is not valid.");
        }
    }
}
