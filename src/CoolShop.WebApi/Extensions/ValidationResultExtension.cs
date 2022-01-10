using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace CoolShop.WebApi.Extensions;

/// <summary>
/// Various validation extensions
/// </summary>
public static class ValidationResultExtension
{
    /// <summary>
    /// Get validation errors from <see cref="ValidationResult"/> into a dictionary
    /// </summary>
    /// <param name="validationResult"></param>
    /// <returns></returns>
    public static Dictionary<string, string[]> GetValidationErrors(this ValidationResult validationResult)
    {
        if (validationResult is null)
        {
            return new();
        }

        return validationResult
            .Errors
            .GroupBy(x => x.ErrorMessage, x => x.PropertyName)
            .ToDictionary(x => x.Key, group => group.ToArray());
    }
}
