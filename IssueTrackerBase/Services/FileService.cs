using IssueTrackerBase.Services.Interfaces;

namespace IssueTrackerBase.Services
{
    public class FileService : IFileService
    {
        private readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };

        public string ContentType(IFormFile file)
        {
            return file?.ContentType;
        }

        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            using MemoryStream memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var byteFile = memoryStream.ToArray();

            return byteFile;
        }

        public string ConvertToByteArrayToFile(byte[] fileData, string contentType)
        {
            string fileBase64Data = Convert.ToBase64String(fileData);

            return string.Format($"data:{contentType};base64,{fileBase64Data}");

        }

        public string DecodeImage(byte[] data, string type)
        {
            if(data is null || type is null)
            {
                return null;
            }
            else
            {
                return $"data:{type};base64,{Convert.ToBase64String(data)}";
            }
        }

        public async Task<byte[]> EncodeImageAsync(IFormFile image)
        {
            if(image is null)
            {
                return null;
            }
            return await ConvertFileToByteArrayAsync(image);
        }

        public async Task<byte[]> EncodeImageAsync(string image)
        {
            var imagePath = $"{Directory.GetCurrentDirectory}/wwwroot/img/{image}";
            return await File.ReadAllBytesAsync(imagePath);
        }

        public string FormatFileSize(long bytes)
        {
            int counter = 0;
            decimal number = bytes;
            while(Math.Round(number /1024) >= 1)
            {
                number /= 1024;
                counter++;
            }

            return string.Format("{0:n1}{1}", number, suffixes[counter]);
        }

        public string GetFileIcon(string file)
        {
            string ext = Path.GetExtension(file).Replace(".", "");
            return $"/img/png/{ext}.png";
        }

        public bool IsValidType(IFormFile file)
        {
            var type = ContentType(file);
            type = type.Split("/")[1];

            var typeList = new List<string>
            {
                "png",
                "jpg",
                "jpeg",
                "bmp",
                "tiff",
                "gif"
            };

            var isValid = typeList.Contains(type);

            return isValid;
        }
    }
}
