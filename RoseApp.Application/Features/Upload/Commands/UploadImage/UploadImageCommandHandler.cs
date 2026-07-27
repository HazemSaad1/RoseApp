using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Contracts.IServices;

namespace RoseApp.Application.Features.Upload.Commands.UploadImage;

public class UploadImageCommandHandler
    : IRequestHandler<UploadImageCommand, ResponseViewModel<string>>
{
    private readonly IImageService _imageService;

    public UploadImageCommandHandler(IImageService imageService)
    {
        _imageService = imageService;
    }

    public async Task<ResponseViewModel<string>> Handle(
        UploadImageCommand request,
        CancellationToken cancellationToken)
    {
        await using var stream = request.File.OpenReadStream();

        var imageUrl = await _imageService.UploadImageAsync(
            stream,
            request.File.FileName);

        return ResponseViewModel<string>.Success(imageUrl, "Image uploaded successfully");
    }
}