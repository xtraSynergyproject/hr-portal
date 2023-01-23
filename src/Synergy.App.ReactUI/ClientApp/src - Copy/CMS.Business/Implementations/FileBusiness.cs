﻿using AutoMapper;
using CMS.Common;
using CMS.Common.Utilities;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class FileBusiness : BusinessBase<FileViewModel, File>, IFileBusiness
    {
        //private ITeamUserBusiness _teamuserBusiness;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo;
        public FileBusiness(IRepositoryBase<FileViewModel, File> repo, IMapper autoMapper,
             IRepositoryQueryBase<IdNameViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<FileViewModel>> Create(FileViewModel viewModel)
        {
            if (viewModel.Id.IsNullOrEmpty())
            {
                viewModel.Id = Guid.NewGuid().ToString();
            }

            if (viewModel.ContentByte.IsNotNull())
            {
                viewModel.MongoFileId = await _repo.UploadMongoFile(viewModel.FileName, viewModel.ContentByte);
               // viewModel.SnapshotMongoId = await _repo.UploadMongoFile(viewModel.FileName, GetSnapshot(viewModel));
            }

            var result = await base.Create(viewModel);
            //if (result.IsSuccess && (viewModel.FileExtension == ".pptx" || viewModel.FileExtension == ".doc" || viewModel.FileExtension == ".docx"))
            //{
            //    Hangfire.BackgroundJob.Enqueue<HangfireScheduler>(x => x.ConvertSingleFileToPdf(viewModel.Id));
            //}
            return CommandResult<FileViewModel>.Instance(viewModel, result.IsSuccess, result.Messages);
        }

        private byte[] GetSnapshot(FileViewModel viewModel)
        {
            try
            {
                var tempPath = @"D:\ERPDocuments\SynergyTempFiles\";
                var snapshot = new byte[0];
                if (viewModel.ContentByte == null && viewModel.ContentByte.Length == 0)
                {
                    return snapshot;
                }
                var dirPath = string.Concat(tempPath, viewModel.Id, "\\");
                if (!System.IO.Directory.Exists(dirPath))
                {
                    System.IO.Directory.CreateDirectory(dirPath);
                }
                var destinationFolder = string.Concat(dirPath, "ThumbNail");
                if (!System.IO.Directory.Exists(destinationFolder))
                {
                    System.IO.Directory.CreateDirectory(destinationFolder);
                }
                var destinationFile = string.Concat(destinationFolder.Trim('\\'), "\\", viewModel.Id, ".jpg");
                var dir = new System.IO.DirectoryInfo(dirPath);
                foreach (var item in dir.GetFiles())
                {
                    item.Delete();
                }
                var sourceFile = string.Concat(dirPath, viewModel.Id, ".", viewModel.FileExtension.Replace(".", ""));

                System.IO.File.WriteAllBytes(sourceFile, viewModel.ContentByte);

                if (this.Is2JpegSupportable(viewModel.FileExtension.Replace(".", "")))
                {
                    var args = string.Concat("/c 2jpeg.exe -src \"", sourceFile, "\" -dst \"", destinationFolder, "\" -options alerts:no pages:\"1\" overwrite:yes -oper Resize size:\"240 320\"");
                    Helper.RunCommand(args, 30000);

                    byte[] bytes2 = null;
                    var dir3 = new System.IO.DirectoryInfo(destinationFolder);
                    var file2 = dir3.GetFiles().FirstOrDefault();
                    if (file2 != null)
                    {
                        bytes2 = System.IO.File.ReadAllBytes(file2.FullName);
                        snapshot = bytes2;
                        System.IO.File.Delete(file2.FullName);
                        viewModel.IsFileViewableFormat = true;
                    }
                    else
                    {
                        viewModel.IsFileViewableFormat = false;
                    }
                    if (System.IO.Directory.Exists(dirPath))
                    {
                        System.IO.Directory.Delete(dirPath, true);
                    }
                    return snapshot;

                }
            }
            //    else if (Helper.IsVideoFile(viewModel.FileExtension) || Helper.IsAudioFile(viewModel.FileExtension))
            //    {
            //        var shellFile = ShellFile.FromFilePath(sourceFile);
            //        var shellThumb = shellFile.Thumbnail.LargeBitmap;
            //        shellThumb.Save(destinationFile, System.Drawing.Imaging.ImageFormat.Jpeg);
            //        viewModel.IsFileViewableFormat = true;
            //    }
            //    byte[] bytes = null;
            //    var dir2 = new DirectoryInfo(destinationFolder);
            //    var file = dir2.GetFiles().FirstOrDefault();
            //    if (file != null)
            //    {
            //        bytes = File.ReadAllBytes(file.FullName);
            //        snapshot = Convert.ToBase64String(bytes);
            //        File.Delete(file.FullName);
            //    }
            //    viewModel.SourceFile = sourceFile;
            //    viewModel.DestinationFolder = destinationFolder;
            //    viewModel.TempDirectory = dir;
            //    return snapshot;
            //}
            catch (Exception ex)
            {
                return new byte[0];
            }

            return new byte[0];
        }

        private bool Is2JpegSupportable(string ext)
        {
            ext = string.Concat(",", ext.ToLower(), ",");
            return ",jpeg,jpg,tiff,tif,gif,png,bmp,pdf,xps,ico,wbm,psd,psp,html,htm,txt,rtf,doc,docx,xls,xlsx,xlsm,ppt,pptx,pps,ppsx,vsd,vsdx,cdr,".Contains(ext);
        }

        public async override Task<CommandResult<FileViewModel>> Edit(FileViewModel model)
        {

            var data = _autoMapper.Map<FileViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<FileViewModel>.Instance(model, false, validateName.Messages);
            }
            if (model.ContentByte.IsNotNull())
            {
                model.MongoFileId = await _repo.UploadMongoFile(model.FileName, model.ContentByte);
                await _repo.DeleteMongoFile(model.MongoPreviewFileId);
                model.MongoPreviewFileId = null;
            }
            model.VersionNo = model.VersionNo + 1;
            var result = await base.Edit(model);
            //if (result.IsSuccess && (model.FileExtension==".pptx" || model.FileExtension == ".doc" || model.FileExtension == ".docx"))
            //{
            //    Hangfire.BackgroundJob.Enqueue<HangfireScheduler>(x => x.ConvertSingleFileToPdf(model.Id));
            //}
            // var pagecol = await _teamuserBusiness.GetList(x => x.TeamId == model.Id);
            // var existingIds = pagecol.Select(x => x.UserId);
            // var newIds = model.UserIds;
            //var ToDelete = existingIds.Except(newIds).ToList();
            //var ToAdd = newIds.Except(existingIds).ToList();
            // Add
            //foreach (var id in model.UserIds)
            //{

            //    var team = new TeamUserViewModel();
            //    if (model.TeamOwnerId == id)
            //    { team.IsTeamOwner = true; }
            //    else { team.IsTeamOwner = false; }
            //    team.TeamId = result.Item.Id;
            //    team.UserId = id;
            //    await _teamuserBusiness.Edit(team);


            //}

            // Delete
            //foreach (var id in ToDelete)
            //{
            //    var role = await _teamuserBusiness.GetSingle(x => x.TeamId == model.Id && x.UserId == id);
            //    await _teamuserBusiness.Delete(role.Id);
            //}





            return CommandResult<FileViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        private async Task<CommandResult<FileViewModel>> IsNameExists(FileViewModel model)
        {

            // var errorList = new Dictionary<string, string>();
            //if (model.Name.IsNullOrEmpty())
            //{
            //    errorList.Add("Name", "Name is required.");
            //}
            // else
            //{
            //    var name = await _repo.GetSingle(x => x.Name.ToLower() == model.Name.ToLower() && x.Id != model.Id);
            //    if (name != null)
            //    {
            //        errorList.Add("Name", "Name already exist.");
            //    }
            //}
            //if (!model.Code.IsNullOrEmpty())


            //{
            //    var name = await _repo.GetSingle(x => x.Code.ToLower() == model.Code.ToLower() && x.Id != model.Id);
            //    if (name != null)
            //    {
            //        errorList.Add("Code", "Code already exist.");
            //    }
            //}

            //if (errorList.Count > 0)
            //{
            //    return CommandResult<FileViewModel>.Instance(model, false, errorList);
            //}

            return CommandResult<FileViewModel>.Instance();
        }
        public async Task<byte[]> DownloadMongoFileByte(string mongoFileId)
        {
            try
            {
                return await _repo.DownloadMongoFile(mongoFileId);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<bool> UploadMongoFileByte(FileViewModel model,Byte[] bytes)
        {
            model.MongoPreviewFileId = await _repo.UploadMongoFile(model.FileName, bytes);
            var result = await base.Edit(model);
            if (result.IsSuccess)
            {
                return true;
            }
            return false;
        }
        public async Task<byte[]> GetFileByte(string fileId)
        {
            try
            {
                var doc = await this.GetSingleById(fileId);
                if (doc != null)
                {
                    if (doc.MongoFileId.IsNotNullAndNotEmpty())
                    {
                        return await DownloadMongoFileByte(doc.MongoFileId);
                    }

                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;

        }
        public async Task<byte[]> GetFilePreviewByte(string fileId)
        {
            try
            {
                var doc = await this.GetSingleById(fileId);
                if (doc != null)
                {
                    
                    if (doc.MongoFileId.IsNotNullAndNotEmpty())
                    {
                        if (doc.MongoPreviewFileId.IsNotNullAndNotEmpty())
                        {
                            return await DownloadMongoFileByte(doc.MongoPreviewFileId);
                        }
                        return await DownloadMongoFileByte(doc.MongoFileId);
                    }

                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;

        }
        public async Task<byte[]> GetSnapFileByte(string fileId)
        {
            try
            {
                var doc = await this.GetSingleById(fileId);
                if (doc != null)
                {
                    if (doc.MongoFileId.IsNotNullAndNotEmpty())
                    {
                        return await DownloadMongoFileByte(doc.SnapshotMongoId);
                    }

                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;

        }

        public async Task<FileViewModel> GetFile(string fileId)
        {
            try
            {
                var doc = await this.GetSingleById(fileId);
                if (doc != null)
                {
                    if (doc.MongoFileId.IsNotNullAndNotEmpty())
                    {
                        var byt = await DownloadMongoFileByte(doc.MongoFileId);
                        doc.ContentByte = byt;
                    }

                }
                return doc;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<List<FileViewModel>> GetFileList()
        {
            try
            {
                var query = $@"Select * from  public.""File""
                    where ""IsDeleted""=false and ""MongoPreviewFileId"" is null and ""FileExtension"" in ('.pptx','.doc','.docx') Order by ""LastUpdatedDate"" Desc limit 20";
                var list = await _queryRepo.ExecuteQueryList<FileViewModel>(query, null);                           
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<FileViewModel> GetFileByName(string fileName)
        {
            var model = await GetSingle(x => x.FileName.ToLower() == fileName.ToLower() && x.IsDeleted == false);
            return model;
        }

        public async Task<FileViewModel> CopyFile(string id, string appendname = "")
        {
            var file = new FileViewModel();
            var orginal = await GetSingleById(id);
            if (orginal != null)
            {
                orginal.Id = null;
                orginal.DataAction = DataActionEnum.Create;
                if (orginal.MongoFileId != null)
                {
                    orginal.ContentByte = await DownloadMongoFileByte(orginal.MongoFileId);
                }
                else
                {
                    orginal.ContentBase64 = orginal.ContentBase64;

                }
                var newfile = await Create(orginal);
                file.Id = newfile.Item.Id;
                file.FileName = appendname + newfile.Item.FileName;
            }
            return file;
        }

        public async Task<List<IdNameViewModel>> GetFileLogsDetails(string fileId)
        {
            var query = $@"Select ""Id"",""VersionNo"" as Name,""IsLatest"" as Code from log.""FileLog"" where ""RecordId""='{fileId}' and ""IsVersionLatest""=true";
            var data = await _queryRepo.ExecuteQueryList(query,null);
            return data;
        }
        public async Task<List<FileViewModel>> GetFileLogsDetailsById(string Id)
        {
            var query = $@"Select l.*,l.""RecordId"" as Id,l.""VersionNo"" as VersionNo,f.""FileName"" as FileName,
l.""FileExtension"" as FileExtension
from log.""FileLog"" as l
join  public.""File"" as f on f.""Id""=l.""RecordId"" and f.""IsDeleted""=false

where l.""Id""='{Id}'  ";
            var data = await _queryRepo.ExecuteQueryList<FileViewModel>(query, null);
            return data;
        }
        public async Task<FileViewModel> GetFileLogsDetailsByFileIdAndVersion(string FileId,long versionNo)
        {
            var query = $@"Select l.*,l.""RecordId"" as Id,l.""VersionNo"" as VersionNo,f.""FileName"" as FileName,
l.""FileExtension"" as FileExtension
from log.""FileLog"" as l
join  public.""File"" as f on f.""Id""=l.""RecordId"" and f.""IsDeleted""=false

where l.""RecordId""='{FileId}' and l.""VersionNo""={versionNo} and ""IsVersionLatest""=true";
            var data = await _queryRepo.ExecuteQuerySingle<FileViewModel>(query, null);
            return data;
        }
    }
}
