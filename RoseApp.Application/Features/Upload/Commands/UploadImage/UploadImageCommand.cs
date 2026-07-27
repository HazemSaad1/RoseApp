using MediatR;
using Microsoft.AspNetCore.Http;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Upload.Commands.UploadImage;

public record UploadImageCommand(IFormFile File)
    : IRequest<ResponseViewModel<string>>;