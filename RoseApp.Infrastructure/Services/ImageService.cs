using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using RoseApp.Application.Contracts;
using RoseApp.Application.Contracts.IServices;

namespace RoseApp.Infrastructure.Services;

public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;

    public ImageService(IConfiguration configuration)
    {
        var cloudName = configuration["CloudinarySettings:CloudName"]!;
        var apiKey = configuration["CloudinarySettings:ApiKey"]!;
        var apiSecret = configuration["CloudinarySettings:ApiSecret"]!;

        var account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account);
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, imageStream),
            Folder = "rose-app",
            Transformation = new Transformation()
                .Width(800)
                .Height(800)
                .Crop("limit")
                .Quality("auto")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error is not null)
            throw new Exception(uploadResult.Error.Message);

        return uploadResult.SecureUrl.ToString();
    }

    public async Task DeleteImageAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        await _cloudinary.DestroyAsync(deleteParams);
    }
}