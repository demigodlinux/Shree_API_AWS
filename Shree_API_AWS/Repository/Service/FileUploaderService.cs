using Shree_API_AWS.Context;
using Shree_API_AWS.Models;
using Shree_API_AWS.Repository.Interface;

namespace Shree_API_AWS.Repository.Service
{
    public class FileUploaderService: IFileUploader
    {
        private readonly ShreeDbContext_Postgres _context;

        public FileUploaderService(ShreeDbContext_Postgres context_Postgres)
        {
            _context = context_Postgres;
        }

        public async Task<string> UploadFileAsync(string uploadPath, FetchBlobModel blobModel, IFormFile file, string employeeName)
        {
            try
            {
                string fileName = blobModel.SiteName + " - " + blobModel.SiteLocation + "." + file.FileName.Split(".").Last();
                var filePath = Path.Combine(uploadPath, fileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var saveBlob = new Blobtable()
                {
                    Employeeid = blobModel.EmployeeId,
                    Dateenteredby = employeeName,
                    Dateenteredon = DateTime.Now,
                    Filename = fileName,
                    Filepath = filePath,
                    Servicetype = blobModel.ServiceType,
                    Sitelocation = blobModel.SiteLocation,
                    Sitename = blobModel.SiteName,
                    Typeofupload = "Reports"
                };

                _context.Blobtables.Add(saveBlob);
                _context.SaveChanges();

                return "Success";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error saving file.", ex);
            }
        }

    }
}
