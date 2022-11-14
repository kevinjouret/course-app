using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace CourseAPI.Utils
{
    public class UploadFile
    {
        public static bool TestImage(IFormFile file)
        {
            if (file != null)
            {
                // Catch the last 3 file name char.
                var extension = file.FileName[^3..].ToUpper();
                // Return true if...
                return extension == "JPG" || extension == "PNG" || extension == "GIF";
            }
            return false;
        }

        public static string WriteFile(IFormFile file)
        {
            string fileUrl = string.Empty;

            try
            {
                var extension = file.FileName[^3..].ToUpper();
                var uniqueName = Guid.NewGuid();
                // Create entire file path
                var filePath = Path.Combine("wwwroot" + "\\images", uniqueName + "." + extension);

                // Create file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                fileUrl = filePath;
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return fileUrl;
        }

        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);               
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
