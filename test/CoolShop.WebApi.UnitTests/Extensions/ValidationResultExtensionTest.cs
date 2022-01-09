using System.Collections.Generic;
using CoolShop.WebApi.Extensions;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoolShop.WebApi.UnitTests.Extensions;

[TestClass]
public class ValidationResultExtensionTest
{
    [TestMethod]
    public void GetValidationErrors_WithoutErrors_CreatesEmptyDictionary()
    {
        var validationResult = new ValidationResult();

        var result = validationResult.GetValidationErrors();

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetValidationErrors_WithoutNullResult_CreatesEmptyDictionary()
    {
        ValidationResult? validationResult = null;

        var result = validationResult.GetValidationErrors();

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetValidationErrors_WithErrors_ReturnsFilledDictionary()
    {
        var validationResult = new ValidationResult(new List<ValidationFailure>()
        {
            new("Name", "Name cannot be null"),
            new("Name", "Name cannot be emmpty"),
            new("Uri", "Uri cannot be null")
        });

        var result = validationResult.GetValidationErrors();

        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count);
    }
}
