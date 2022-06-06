using FluentValidation.Results;

namespace PdfGenerator.Scoped;
public static class FluentValidationExtension
{
  public static Dictionary<string, string[]> ToErrorMessageDict(this List<ValidationFailure> validationFailures)
    => validationFailures.GroupBy(x => x.PropertyName).ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());
}
