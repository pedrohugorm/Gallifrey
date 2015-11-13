using System;
using System.Data.Entity;
using System.Diagnostics;
using FunctionalTest.Domain;
using Gallifrey.RestApi.Application.Configuration;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace FunctionalTest
{
    public class FakeDbContext : DbContext
    {
        public DbSet<TestModel> Tests { set; get; }
    }

    public class TestConfiguration : BaseConfiguration
    {
        public TestConfiguration(IContainer container) : base(container)
        {
            SetDatabaseContext<FakeDbContext>();
        }
    }

    [TestClass]
    public class IocContainerTest
    {
        [TestMethod]
        public void ShouldLoadIocContainerAndGetType()
        {
            var container = new Container();
            var configuration = new TestConfiguration(container);

            var model = new TestModel();

            var instanceRepository = container.GetInstance<IRepository<TestModel, Guid>>();
            instanceRepository.InsertOrUpdate(model);
            instanceRepository.Save();

            Debug.WriteLine(model.Id);

            Assert.IsNotNull(model.Id);
        }
    }
}
