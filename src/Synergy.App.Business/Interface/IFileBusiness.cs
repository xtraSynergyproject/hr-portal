using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IFileBusiness : IBusinessBase<FileViewModel, DataModel.File>
    {
        Task<byte[]> DownloadMongoFileByte(string mongoFileId);
        Task<bool> UploadMongoFileByte(FileViewModel model, Byte[] bytes, bool autoCommit = true);
        Task<bool> UploadMongoSnapshotFile(FileViewModel model, Byte[] bytes, bool autoCommit = true);
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
        Task<Tuple<string, string, MemoryStream>> GenerateBarCodeFile(string data, QRCodeDataTypeEnum dataType = QRCodeDataTypeEnum.Text, QRCodeTypeEnum codeType = QRCodeTypeEnum.QR_CODE, ReferenceTypeEnum? referenceType = null, string referenceId = null, bool isPopup = false);
    }
}
