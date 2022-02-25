namespace IssueTrackerBase.Services.Interfaces
{
    public interface IFileService
    {
        Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);

        string ConvertToByteArrayToFile(byte[] fileData, string contentType);

        string GetFileIcon(string file);

        string FormatFileSize(long bytes);

        string DecodeImage(byte[] data, string type);

        bool IsValidType(IFormFile file);

        string ContentType(IFormFile file);

        Task<byte[]> EncodeImageAsync(IFormFile image);

        Task<byte[]> EncodeImageAsync(string image);
    }
}
