using Microsoft.EntityFrameworkCore;
using Moq;
using BooksyAPI.Repo.Interfaces;
using BooksyAPI.Repo.Classes;
using BooksyAPI.Services.Interfaces;
using BooksyAPI.Services.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksyAPI.Models;
using BooksyAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace BooksyAPITesting
{
    internal class CoverTypeServiceTest
    {
        private Mock<ICoverTypeRepo<CoverType>> CoverTypeRepoObj;

        private CoverTypeService CoverTypeServ;
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
        public async Task Setup()
        {
            IEnumerable<CoverType> coverTypeEnumerable = CoverTypes.AsEnumerable();
            Task<IEnumerable<CoverType>> covertypes = Task.FromResult(coverTypeEnumerable);
            CoverTypeRepoObj = new Mock<ICoverTypeRepo<CoverType>>();
            CoverTypeRepoObj.Setup(x => x.GetCoverTypes()).Returns(covertypes);
            CoverTypeServ = new CoverTypeService(CoverTypeRepoObj.Object);
        }



        [Test]
        public async Task GetCoverTypes_Test()
        {
            var Taskresult = CoverTypeServ.GetCoverTypes();
            var x = await Taskresult??CoverTypes;
            var result = (List<CoverType>)x;
            // To print something to debug window
            //System.Diagnostics.Debug.WriteLine(result);
            Assert.AreEqual(3, result.Count);
            Assert.That(Taskresult, Is.InstanceOf<Task<IEnumerable<CoverType>>>());
        }


    }

}

