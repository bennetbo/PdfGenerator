using FluentValidation;
using PdfGenerator.DTOs;

namespace PdfGenerator.Validators;

public abstract class PageCountValidator<ParamType> : AbstractValidator<ParamType>
  where ParamType : IHasPageCount
{
  public PageCountValidator()
  {
    RuleFor(x => x.PageCount)
      .NotNull().WithMessage("PageCount required")
      .GreaterThanOrEqualTo(1).WithMessage("PageCount must be greater or equal to 1")
      .LessThanOrEqualTo(1000).WithMessage("PageCount must be less or equal to 1000");
  }
}