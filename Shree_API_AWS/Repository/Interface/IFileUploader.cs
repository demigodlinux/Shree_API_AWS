using Shree_API_AWS.Models;

namespace Shree_API_AWS.Repository.Interface
{
    public interface IFileUploader
    {
        /// <summary>
        /// Handles file upload and returns the file path.
        /// </summary>
        /// <param name="path">the path of the file.</param>
        /// <param name="blob">The blob file with data to be uploaded.</param>
        /// <param name="employeeName">The employee Name for the file uploaded.</param>
        /// <param name="file">The file data uploaded.</param>
        /// <returns>The full file path of the uploaded file.</returns>
        Task<string> UploadFileAsync(string path, FetchBlobModel blob, IFormFile file, string employeeName);
    }
}
