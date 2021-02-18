using FluentValidation;
using HARRepo.FileManager.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HARRepo.FileManager.API.Validators
{
    public class FileUploadValidator : AbstractValidator<FileUploadModel>
    {
        public FileUploadValidator()
        {
            RuleFor(x => x.DirectoryId)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .Matches(@"^(.+\.har$")
                .WithMessage("File is not with the correct extension.");

            RuleFor(x => x.Content)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty();
        }
    }
}
