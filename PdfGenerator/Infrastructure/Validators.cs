using FluentValidation;
using FluentValidation.Results;
using PdfGenerator.DTOs;
using PdfGenerator.Services;
using QuestPDF.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace PdfGenerator.Validators;
interface ICustomValidator
{
  CustomValidationResult Validate<T>(T model);
}
interface ICustomValidator<ResultType>
{
  ResultingValidationResult<ResultType> Validate<T>(T model);
}

class AttributeValidator<ResultType> : ICustomValidator
{
  public CustomValidationResult Validate<T>(T model)
  {
    var result = new CustomValidationResult();
    var properties = typeof(T).GetProperties();
    foreach (var property in properties)
    {
      var customAttributes = property.GetCustomAttributes(typeof(ValidationAttribute), true);
      foreach (var attribute in customAttributes)
      {
        if (attribute is ValidationAttribute validationAttribute)
        {
          var propertyValue = property.CanRead ? property.GetValue(model) : null;
          var isValid = validationAttribute.IsValid(propertyValue);

          if (!isValid)
          {
            if (result.ErrorMessages.ContainsKey(property.Name))
            {
              var errors = result.ErrorMessages[property.Name].ToList();
              errors.Add(validationAttribute.FormatErrorMessage(property.Name));
              result.ErrorMessages[property.Name] = errors.ToArray();
            }
            else
            {
              result.ErrorMessages.Add(property.Name, new string[] { validationAttribute.FormatErrorMessage(property.Name) });
            }
          }
        }
      }
    }

    return result;
  }
}

class CustomValidationResult : FluentValidation.Results.ValidationResult
{
  public Dictionary<string, string[]> ErrorMessages => Errors.ToErrorMessageDict();
}

class ResultingValidationResult<ResultType> : CustomValidationResult
{
  public ResultType? Result { get; init; }
}

class PageCountValidator<ParamType> : AbstractValidator<ParamType>
  where ParamType : IHasPageCount
{
  public PageCountValidator()
  {
    RuleFor(x => x.PageCount)
      .NotNull().WithMessage("PageCount required")
      .GreaterThanOrEqualTo(1).WithMessage("PageCount must be greater or equal to 1")
      .LessThanOrEqualTo(1000).WithMessage("PageCount must be greater or equal to 1000");
  }
}

class PagesizeValidator : PageCountValidator<PageSizeGenerationParams>
{
  public PagesizeValidator(IMeasurementService measurementService) : base()
  {
    RuleFor(x => x.PageSize)
     .NotNull().WithMessage("Pagesize required")
     .Must(pageSize => measurementService.IsValidPageSize(pageSize!))
     .WithMessage("Pagesize must be valid");
  }
}

class ExplicitParamsValidator : PageCountValidator<ExplicitGenerationParams>
{
  public ExplicitParamsValidator(IMeasurementService measurementService) : base()
  {
    RuleFor(x => x.Width)
     .NotNull().WithMessage("Width required")
     .Must(width => measurementService.IsValidWidth(width ?? 0))
     .WithMessage("Width must be valid");

    RuleFor(x => x.Height)
     .NotNull().WithMessage("Height required")
     .Must(height => measurementService.IsValidWidth(height ?? 0))
     .WithMessage("Height must be valid");
  }
}

static class FluentValidationExtensions
{
  public static Dictionary<string, string[]> ToErrorMessageDict(this List<ValidationFailure> validationFailures)
    => validationFailures.GroupBy(x => x.PropertyName).ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());
}