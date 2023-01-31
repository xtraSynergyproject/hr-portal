using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IFileBusiness : IBusinessBase<FileViewModel, File>
    {
        Task<byte[]> DownloadMongoFileByte(string mongoFileId);
        Task<bool> UploadMongoFileByte(FileViewModel model, Byte[] bytes);
        Task<byte[]> GetFileByte(string fileId);
        Task<byte[]> GetFilePreviewByte(string fileId);
        Task<byte[]> GetSnapFileByte(string fileId);
        Task<FileViewModel> GetFile(string fileId);
        Task<List<FileViewModel>> GetFileList();
        Task<FileViewModel> GetFileByName(string fileName);
        Task<FileViewModel> CopyFile(string id, string appendname = "");
        Task<List<IdNameViewModel>> GetFileLogsDetails(string fileId);
        Task<List<FileViewModel>> GetFileLogsDetailsById(string Id);
        Task<FileViewModel> GetFileLogsDetailsByFileIdAndVersion(string FileId, long versionNo);
    }
}
