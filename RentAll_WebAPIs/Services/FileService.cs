namespace RentAll_WebAPIs.Services
{
    public class FileService
    {
        private static readonly Dictionary<string, string> MimeTypes = new()
        {
            { ".jpg",  "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png",  "image/png"  },
            { ".webp", "image/webp" }
        };

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var mime = MimeTypes.GetValueOrDefault(ext, "image/jpeg");

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var base64 = Convert.ToBase64String(ms.ToArray());

            return $"data:{mime};base64,{base64}";
        }
        public void DeleteFile(string imageUrl) { }
    }
}