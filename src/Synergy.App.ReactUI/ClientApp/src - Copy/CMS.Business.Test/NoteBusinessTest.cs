using NUnit.Framework;
using System;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using System.Threading.Tasks;

namespace CMS.Business.Test
{
    [TestFixture()]
    public class NoteBusinessTest
    {
        IServiceProvider _sp;

        [SetUp]
        public void Setup()
        {
            _sp = TestHelper.Setup("");
        }

        [Test]
        [TestCase(null)]
        public void GetVersionDetails_NoExceptionTest(string noteId)
        {
            var nb = _sp.GetService<INoteBusiness>();
            Assert.DoesNotThrowAsync(() => nb.GetVersionDetails(noteId), "Exception thrown");
        }
        [Test]
        [TestCase(null)]
        [TestCase("45bba746-3309-49b7-9c03-b5793369d73c")]
        public async Task GetVersionDetails_ValidateResultCount(string noteId)
        {
            var nb = _sp.GetService<INoteBusiness>();
            var result = await nb.GetVersionDetails(noteId);
            Assert.That(result.Count, Is.LessThanOrEqualTo(1));
        }
        //[Test]

        //[TestCase("")]
        //[TestCase("45bba746-3309-49b7-9c03-b5793369d73c")]
        //public void GetNoteBookDetails_NoExceptionTest(string noteId)
        //{
        //    var nb = _sp.GetService<INoteBusiness>();
        //    Assert.DoesNotThrowAsync(() => nb.GetNoteBookDetails(noteId), "Exception thrown");
        //}
        //[Test]
        //[TestCase("45bba746-3309-49b7-9c03-b5793369d73c")]
        //public async Task GetNoteBookDetails_ValidateResultCount(string noteId)
        //{
        //    var nb = _sp.GetService<INoteBusiness>();
        //    var result = await nb.GetNoteBookDetails(noteId);
        //    Assert.That(result.Count, Is.LessThanOrEqualTo(1));
        //}
        [TearDown]
        public void Cleanup()
        {
            //Any clean up
        }
    }
}