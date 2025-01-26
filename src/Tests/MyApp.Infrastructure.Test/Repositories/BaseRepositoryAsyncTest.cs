using Xunit;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure.Data;
using MyApp.Infrastructure.Repositories;
using MyApp.Domain.Entities;
using MyApp.Domain.Enums;

namespace MyApp.Infrastructure.Test.Repositories
{
    public class BaseRepositoryAsyncTest
    {
        private readonly MyAppDbContext _myAppDbContext;
        private readonly UnitOfWork _unitOfWork;

        public BaseRepositoryAsyncTest()
        {
            var options = new DbContextOptionsBuilder<MyAppDbContext>().UseInMemoryDatabase(databaseName: "snappedLK").Options;
            _myAppDbContext = new MyAppDbContext(options);
            _unitOfWork = new UnitOfWork(_myAppDbContext);
        }

        [Fact]
        public async void Given_ValidData_When_AddAsync_Then_SuccessfullyInsertData()
        {
            
        }
    }
}