using FluentValidation;
using PdfGenerator.DTOs;
using PdfGenerator.Validators;

namespace PdfGenerator.Scoped;

public class DocumentGenerationExplicitParamsValidator : PageCountValidator<ExplicitGenerationParams>
{
  public DocumentGenerationExplicitParamsValidator(IMeasurementService measurementService) : base()
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
