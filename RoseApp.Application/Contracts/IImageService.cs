using System;
using System.Collections.Generic;
using System.Text;

namespace RoseApp.Application.Contracts
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(Stream imageStream, string fileName);
        Task DeleteImageAsync(string publicId);
    }
}
