using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CheaperThanAi.Shared.Requests;
using CheaperThanAi.Shared.dto;
using System.Linq;
using Xunit;

namespace CheaperThanAi.Tests;

public class SupportRequestTests
{
    [Fact]
    public void Validation_Fails_When_Required_Fields_Missing()
    {
        var req = new SupportRequest();
        var context = new ValidationContext(req);
        var results = new List<ValidationResult>();

        var valid = Validator.TryValidateObject(req, context, results, validateAllProperties: true);

        Assert.False(valid);
        Assert.True(results.Count >= 3);
    }

    [Fact]
    public void Validation_Succeeds_With_Valid_Data()
    {
        var req = new SupportRequest
        {
            Name = "Test User",
            Email = "test@example.com",
            Message = "This is a test message."
        };

        var context = new ValidationContext(req);
        var results = new List<ValidationResult>();

        var valid = Validator.TryValidateObject(req, context, results, validateAllProperties: true);

        Assert.True(valid);
        Assert.Empty(results);
    }

    [Fact]
    public void Validation_Fails_With_Invalid_Email()
    {
        var req = new SupportRequest
        {
            Name = "Test User",
            Email = "not-an-email",
            Message = "Hello"
        };

        var context = new ValidationContext(req);
        var results = new List<ValidationResult>();

        var valid = Validator.TryValidateObject(req, context, results, validateAllProperties: true);

        Assert.False(valid);
        Assert.Contains(results, r => r.MemberNames != null && r.MemberNames.Contains("Email"));
    }

    [Fact]
    public void TicketSearchResult_Defaults()
    {
        var tsr = new TicketSearchResult();

        Assert.Equal(0d, tsr.Score);
        Assert.Equal(PriorityLevel.Low, tsr.PriorityLevel);
    }

    [Fact]
    public void PriorityLevel_Contains_Expected_Names()
    {
        var names = System.Enum.GetNames(typeof(PriorityLevel)).ToList();

        Assert.Contains("Low", names);
        Assert.Contains("Medium", names);
        Assert.Contains("High", names);
    }
}
