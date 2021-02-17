using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HARRepo.FileManager.API.Validators
{
    public static class Extensions
    {
        public static Dictionary<string, string> ToErrorDictionary(this ValidationResult validationResult)
        {
            return validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(x => string.IsNullOrEmpty(x.Key) ? "form" :
                    $"{char.ToLower(x.Key[0])}{x.Key.Substring(1)}",
                    x => x.Select(y => y.ErrorMessage)
                        .Aggregate((i, j) => $"{i}, {j}"));
        }
    }
}
