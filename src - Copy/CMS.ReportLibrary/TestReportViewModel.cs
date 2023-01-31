using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.ReportLibrary
{
    [System.ComponentModel.DataObject]
    public class TestReportViewModel
    {
        public TestReportViewModel()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
        public List<TestReportViewModel> GetHeader(string param)
        {
            var data = new List<TestReportViewModel>();
            data.Add(new TestReportViewModel { Id=01, Name="Test01",Code="Code01"});
            data.Add(new TestReportViewModel { Id=02, Name="Test02",Code="Code02"});
            data.Add(new TestReportViewModel { Id=03, Name="Test03",Code="Code03"});
            data.Add(new TestReportViewModel { Id=04, Name="Test04",Code="Code04"});
            data.Add(new TestReportViewModel { Id=05, Name="Test05",Code="Code05"});
            return data;
        }
        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
        public List<TestReportViewModel> GetDetail(string param2)
        {
            var data = new List<TestReportViewModel>();
            data.Add(new TestReportViewModel { Id = 01, Name = "Test01", Code = "Code01" });
            data.Add(new TestReportViewModel { Id = 02, Name = "Test02", Code = "Code02" });
            data.Add(new TestReportViewModel { Id = 03, Name = "Test03", Code = "Code03" });
            data.Add(new TestReportViewModel { Id = 04, Name = "Test04", Code = "Code04" });
            data.Add(new TestReportViewModel { Id = 05, Name = "Test05", Code = "Code05" });
            return data;
        }
    }
}
