using AutoMapper;
using SixLabors.ImageSharp.PixelFormats;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;

namespace Synergy.App.Business
{
    public class QRCodeBusiness : BusinessBase<QRCodeDataViewModel, QRCodeData>, IQRCodeBusiness
    {
        IFileBusiness _fileBusiness;
        IServiceProvider _sp;
        public QRCodeBusiness(IRepositoryBase<QRCodeDataViewModel, QRCodeData> repo, IMapper autoMapper,
            IFileBusiness fileBusiness
            , IServiceProvider sp) : base(repo, autoMapper)
        {
            _fileBusiness = fileBusiness;
            _sp = sp;
        }
        public async Task<Tuple<string, string, MemoryStream>> GenerateBarCode(string data, QRCodeDataTypeEnum dataType = QRCodeDataTypeEnum.Url, QRCodeTypeEnum codeType = QRCodeTypeEnum.QR_CODE, ReferenceTypeEnum? referenceType = null, string referenceId = null, bool isPopup = false)
        {
            var width = 250;
            var height = 250;
            var margin = 0;
            var barcodeFormat = (BarcodeFormat)codeType;
            if (barcodeFormat != BarcodeFormat.QR_CODE)
            {
                height = 100;
            }
            var barcodeWriter = new ZXing.ImageSharp.BarcodeWriter<Rgba32>
            {
                Format = barcodeFormat,
                Options = new EncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = margin
                }
            };
            var _configuration = _sp.GetService<Microsoft.Extensions.Configuration.IConfiguration>();
            var qrCodeId = Guid.NewGuid().ToString();
            var qrCodeUrl = @$"{ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration)}home/loadqrcode?id={qrCodeId}";
            using (var image = barcodeWriter.Write(qrCodeUrl))
            {
                var format = SixLabors.ImageSharp.Formats.Jpeg.JpegFormat.Instance;
                var ms = new MemoryStream();
                // image.Save(ms, format);
                var bytes = ms.ToArray();
                var file = await _fileBusiness.Create(new FileViewModel
                {
                    ContentByte = bytes,
                    ContentType = "image/jpeg",
                    ContentLength = bytes.Length,
                    FileName = "barcode.jpg",
                    FileExtension = ".jpg"
                });


                var qrCode = await _repo.Create<QRCodeDataViewModel, QRCodeData>(new QRCodeDataViewModel
                {
                    QRCodeDataType = dataType,
                    Data = data,
                    QRCodeType = codeType,
                    QrCodeUrl = qrCodeUrl,
                    ReferenceType = referenceType,
                    ReferenceTypeId = referenceId,
                    IsPopup = isPopup,
                    Id = qrCodeId
                });
                return new Tuple<string, string, MemoryStream>(file.Item.Id, qrCode.Id, ms);
            }
        }
        //public async Task<IActionResult> ReadQrCode(IList<IFormFile> file)
        //{
        //    try
        //    {
        //        var ms = new MemoryStream();
        //        file[0].OpenReadStream().CopyTo(ms);
        //        var bitMap = (System.DrawingCore.Bitmap)System.DrawingCore.Bitmap.FromStream(ms);
        //        var reader = new ZXing.ZKWeb.BarcodeReader();
        //        return Convert.ToString(reader.Decode(bitMap));

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }
}
