using FluentValidation;
using PdfGenerator.DTOs;
using PdfGenerator.Services;
using PdfGenerator.Validators;

namespace PdfGenerator.Scoped;

public class PageCountValidator : PageCountValidator<PageSizeGenerationParams>
{
  public PageCountValidator(IMeasurementService measurementService) : base()
  {
    RuleFor(x => x.PageSize)
     .NotNull().WithMessage("Pagesize required")
     .Must(pageSize => measurementService.IsValidPageSize(pageSize!))
     .WithMessage("Pagesize must be valid");
  }
}