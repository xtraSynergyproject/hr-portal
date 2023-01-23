using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;
using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;
using UglyToad.PdfPig.DocumentLayoutAnalysis.ReadingOrderDetector;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis;
using System.Linq;

namespace Synergy.App.Business
{
    public class OCRMappingBusiness : BusinessBase<OCRMappingViewModel, OCRMapping>, IOCRMappingBusiness
    {
        IFileBusiness _fileBusiness;
        public OCRMappingBusiness(IRepositoryBase<OCRMappingViewModel, OCRMapping> repo, IMapper autoMapper, IFileBusiness fileBusiness)
            : base(repo, autoMapper)
        {
            _fileBusiness = fileBusiness;
        }
        public async Task<OCRMappingViewModel> GetExistingOCRMapping(OCRMappingViewModel model)
        {
            var existingrecord = await _repo.GetSingle(x => x.FieldName.ToLower() == model.FieldName.ToLower()
            && x.TemplateId == model.TemplateId );
            return existingrecord;
            
        }       
        public async override Task<CommandResult<OCRMappingViewModel>> Create(OCRMappingViewModel model, bool autoCommit = true)
        {
            
            var result = await base.Create(model,autoCommit);           

            return CommandResult<OCRMappingViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async override Task<CommandResult<OCRMappingViewModel>> Edit(OCRMappingViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model,autoCommit);
            return CommandResult<OCRMappingViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<Dictionary<string, object>> GetExtractedData(string fileId,List<OCRMappingViewModel> ocrmodel,Dictionary<string,object> rowdata)
        {
            var bytes = await _fileBusiness.GetFilePreviewByte(fileId);
            //string filePath = "C:\\bill.pdf";
            using (var document = PdfDocument.Open(bytes))
            {
                foreach (var item in ocrmodel)
                {
                    if (rowdata.ContainsKey(item.FieldName) && item.Cordinate1.IsNotNullAndNotEmpty() && item.Cordinate2.IsNotNullAndNotEmpty() && item.Cordinate3.IsNotNullAndNotEmpty())
                    {
                        int pagenum = Convert.ToInt32(item.Cordinate3);
                        float x = Convert.ToSingle(item.Cordinate1);
                        float y = Convert.ToSingle(item.Cordinate2);                         
                        foreach (var page in document.GetPages().Where(x => x.Number == pagenum))
                        {
                            // 0. Preprocessing
                            var letters = page.Letters; // no preprocessing

                            // 1. Extract words
                            var wordExtractor = NearestNeighbourWordExtractor.Instance;
                            var wordExtractorOptions = new NearestNeighbourWordExtractor.NearestNeighbourWordExtractorOptions()
                            {
                                Filter = (pivot, candidate) =>
                                {
                                    // check if white space (default implementation of 'Filter')
                                    if (string.IsNullOrWhiteSpace(candidate.Value))
                                    {
                                        // pivot and candidate letters cannot belong to the same word 
                                        // if candidate letter is null or white space.
                                        // ('FilterPivot' already checks if the pivot is null or white space by default)
                                        return false;
                                    }

                                    // check for height difference
                                    var maxHeight = Math.Max(pivot.PointSize, candidate.PointSize);
                                    var minHeight = Math.Min(pivot.PointSize, candidate.PointSize);
                                    if (minHeight != 0 && maxHeight / minHeight > 2.0)
                                    {
                                        // pivot and candidate letters cannot belong to the same word 
                                        // if one letter is more than twice the size of the other.
                                        return false;
                                    }

                                    // check for colour difference
                                    var pivotRgb = pivot.Color.ToRGBValues();
                                    var candidateRgb = candidate.Color.ToRGBValues();
                                    if (!pivotRgb.Equals(candidateRgb))
                                    {
                                        // pivot and candidate letters cannot belong to the same word 
                                        // if they don't have the same colour.
                                        return false;
                                    }

                                    return true;
                                }
                            };

                            var words = wordExtractor.GetWords(letters, wordExtractorOptions);
                            var query = new System.Drawing.PointF { X = x, Y = Convert.ToSingle(page.Height - y) };
                            // 2. Segment page
                            var pageSegmenter = DocstrumBoundingBoxes.Instance;
                            var pageSegmenterOptions = new DocstrumBoundingBoxes.DocstrumBoundingBoxesOptions()
                            {

                            };

                            var textBlocks = pageSegmenter.GetBlocks(words, pageSegmenterOptions);

                            // 3. Postprocessing
                            var readingOrder = UnsupervisedReadingOrderDetector.Instance;
                            var orderedTextBlocks = readingOrder.Get(textBlocks);
                            var block = GetClosestTextBlockPoint(orderedTextBlocks.ToList(), query);
                            //// 4. Extract text
                            if (block.TextLines.Count > 1)
                            {
                                var line = GetClosestTextLinePoint(block.TextLines.ToList(), query);
                                rowdata[item.FieldName]= line.Text;
                            }
                            else
                            {
                                //var word = GetClosestWordPoint(block.TextLines[0].Words.Where(x => x.Text != " ").ToList(), query);                        
                                //return Json(word.Text);
                                rowdata[item.FieldName] = block.Text;
                            }
                            

                        }
                    }
                }
                return rowdata;
            }            

        }
        private static Word GetClosestWordPoint(List<Word> points, System.Drawing.PointF query)
        {
            return points.OrderBy(x => WordDistance(query, x)).First();
        }

        private static double WordDistance(System.Drawing.PointF pt1, Word pt2)
        {
            return Math.Sqrt((pt2.BoundingBox.Top - pt1.Y) * (pt2.BoundingBox.Top - pt1.Y) + (pt2.BoundingBox.Left - pt1.X) * (pt2.BoundingBox.Left - pt1.X));

        }
        private static TextBlock GetClosestTextBlockPoint(List<TextBlock> points, System.Drawing.PointF query)
        {
            return points.OrderBy(x => TextBlockDistance(query, x)).First();
        }

        private static double TextBlockDistance(System.Drawing.PointF pt1, TextBlock pt2)
        {
            return Math.Sqrt((pt2.BoundingBox.Top - pt1.Y) * (pt2.BoundingBox.Top - pt1.Y) + (pt2.BoundingBox.Left - pt1.X) * (pt2.BoundingBox.Left - pt1.X));

        }
        private static TextLine GetClosestTextLinePoint(List<TextLine> points, System.Drawing.PointF query)
        {
            return points.OrderBy(x => TextLineDistance(query, x)).First();
        }
        private static double TextLineDistance(System.Drawing.PointF pt1, TextLine pt2)
        {
            return Math.Sqrt((pt2.BoundingBox.Top - pt1.Y) * (pt2.BoundingBox.Top - pt1.Y) + (pt2.BoundingBox.Left - pt1.X) * (pt2.BoundingBox.Left - pt1.X));

        }
    }
}
