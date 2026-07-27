using FluentValidation;

namespace RoseApp.Application.Features.Upload.Commands.UploadImage;

public class UploadImageCommandValidator
    : AbstractValidator<UploadImageCommand>
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

    public UploadImageCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required");

        RuleFor(x => x.File.Length)
            .LessThanOrEqualTo(MaxFileSize)
            .WithMessage("File size must not exceed 5MB");

        RuleFor(x => x.File.FileName)
            .Must(fileName =>
            {
                var ext = Path.GetExtension(fileName).ToLower();
                return _allowedExtensions.Contains(ext);
            })
            .WithMessage("Only .jpg, .jpeg, .png, .webp files are allowed");
    }
}