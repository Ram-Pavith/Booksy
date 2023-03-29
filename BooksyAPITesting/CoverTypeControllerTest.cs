/*using BooksyAPI.Models;
using BooksyAPI.Controllers;
using BooksyAPI.Repo.Interfaces;
using BooksyAPI.Repo.Classes;
using BooksyAPI.Services.Interfaces;
using BooksyAPI.Services.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BooksyAPITesting
{
    public class CoverTypeControllerTest
    {
        private Mock<ICoverTypeService<CoverType>> CoverTypeServiceObj;
        private CoverTypesController CoverTypeCtrlr;
        private CoverType c = new CoverType
        {
            Id = 1,
            Name = "Hard Cover"
        };

        private List<CoverType> CoverTypes = new List<CoverType>() {
            new CoverType
            {
                Id = 1,
                Name= "Hard Cover"
            },
            new CoverType
            {
                Id = 2,
                Name= "Soft Cover"
            },
            new CoverType
            {
                Id = 3,
                Name= "E-Book"
            }
        };


        [SetUp]
        public void Setup()
        {
            CoverTypeServiceObj = new Mock<ICoverTypeService<CoverType>>();
            UnitOfWorkRepo = new Mock<IUnitOfWorkRepo>();
            CoverTypeCtrlr = new CoverTypesController(CoverTypeServiceObj.Object);
        }

        [Test]
        public void GetAllCoverTypes_Test()
        {

            CoverTypeServiceObj.Setup(x => x.GetCoverTypes()).Returns(CoverTypes);
            var result = CoverTypeCtrlr.GetCoverTypes();
            Assert.That(result, Is.InstanceOf<Task<ActionResult<IEnumerable<CoverType>>>>());
        }

        [TestCase(119)]
        public void GetGetCoverTypeById_Test(int id)
        {
            CoverTypeServiceObj.Setup(_ => _.GetById(id)).Returns(c);
            Task result = CoverTypeCtrlr.GetCoverType(119);
            Assert.That(result, Is.InstanceOf<Task<ActionResult<CoverType>>>());
            Task<ActionResult<CoverType>> taskResult = (Task<ActionResult<CoverType>>)result;
            System.Diagnostics.Debug.WriteLine(taskResult.Result.Value);
            Assert.That(taskResult.Result.Value, Is.AssignableTo<CoverType>());
        }


    }
}*/