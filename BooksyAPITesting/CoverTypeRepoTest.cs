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

namespace BooksyAPITesting
{
    internal class CoverTypeRepoTest
    {
        private List<CoverType> CoverTypes = new List<CoverType>();
        IQueryable<CoverType> CoverTypeData;
        Mock<DbSet<CoverType>> mockSet;
        Mock<ApplicationDbContext> mockAPIContext;
        CoverTypeRepo CoverTypeRepo;


        [SetUp]
        public void Setup()
        {
            CoverTypes = new List<CoverType>() {
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
            CoverTypeData = CoverTypes.AsQueryable();
            mockSet = new Mock<DbSet<CoverType>>();
            mockSet.As<IQueryable<CoverType>>().Setup(m => m.Provider).Returns(CoverTypeData.Provider);
            mockSet.As<IQueryable<CoverType>>().Setup(m => m.Expression).Returns(CoverTypeData.Expression);
            mockSet.As<IQueryable<CoverType>>().Setup(m => m.ElementType).Returns(CoverTypeData.ElementType);
            mockSet.As<IQueryable<CoverType>>().Setup(m => m.GetEnumerator()).Returns(CoverTypeData.GetEnumerator());
            var p = new DbContextOptions<ApplicationDbContext>();
            mockAPIContext = new Mock<ApplicationDbContext>(p);
            mockAPIContext.Setup(x => x.CoverTypes).Returns(mockSet.Object);
            CoverTypeRepo = new CoverTypeRepo(mockAPIContext.Object);

        }


        [Test]
        public async Task GetById_Test()
        {
            var Taskresult = CoverTypeRepo.GetById(1);
            var result = await Taskresult;
            string title = result.Name;
            Assert.AreEqual("Hard Cover", title);
        }


    }

}

