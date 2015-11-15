using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using FunctionalTest.Domain;
using Gallifrey.RestApi.Application.Configuration;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Gallifrey.SharedKernel.Application.Persistence.Strategy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace FunctionalTest
{
    public class TestModelAddStrategy : IAddItemStrategy<TestModel, Guid>
    {
        public void AddItem(DbSet<TestModel> dbSet, TestModel model)
        {
            
        }
    }

    public class HandleTestModelFilterStrategy : IHandleModelFilterStrategy<TestModel>
    {
        public IQueryable<TestModel> HandleFilter(IQueryable<TestModel> queryable)
        {
            return queryable;
        }
    }

    public class FakeDbContext : DbContext
    {
        public DbSet<TestModel> Tests { set; get; }
    }

    public class TestConfiguration : DefaultConfiguration
    {
        public TestConfiguration(IContainer container) : base(container)
        {
            SetDatabaseContext<FakeDbContext>();
        }
    }

    public class TestConfigurationFromBase : BaseConfiguration
    {
        public TestConfigurationFromBase(IContainer container) : base(container)
        {
            SetDatabaseContext<FakeDbContext>();
        }
    }

    [TestClass]
    public class IocContainerTest
    {
        [TestMethod]
        public void ShouldGenerateNewGuid()
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

        [TestMethod]
        public void ShouldTriggerHandlers()
        {
            var changed = new Mock<IHandleEntityChanged<TestModel>>();
            var changing = new Mock<IHandleEntityChanging<TestModel>>();

            var container = new Container();
            container.Configure(x => x.For<IHandleEntityChanged<TestModel>>().Use(changed.Object));
            container.Configure(x => x.For<IHandleEntityChanging<TestModel>>().Use(changing.Object));

            var configuration = new TestConfiguration(container);

            var model = new TestModel();

            var instanceRepository = container.GetInstance<IRepository<TestModel, Guid>>();
            instanceRepository.InsertOrUpdate(model);
            instanceRepository.Save();

            changing.Verify(r => r.OnEntityChanging(It.IsAny<DbEntityEntry<TestModel>>()));
            changed.Verify(r => r.OnEntityChanged(It.IsAny<DbEntityEntry<TestModel>>()));
        }

        [TestMethod]
        public void ShouldLoadRepositoryWithStrategies()
        {
            var container = new Container();
            var configuration = new TestConfigurationFromBase(container)
                .UsingDefaultDatabaseRepository()
                .UsingDefaultCrudStrategies();

            var instance = container.GetInstance<IRepository<TestModel, Guid>>();

            Debug.Write(instance);
        }

        [TestMethod]
        public void ShouldAssertStructureMapConfiguration()
        {
            var container = new Container();
            var configuration = new TestConfiguration(container);

            container.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void ShouldUseFilterHandlerProperly()
        {
            var container = new Container();
            var configuration = new TestConfiguration(container);

            configuration.SetFilterStrategy<HandleTestModelFilterStrategy, TestModel>();

            container.AssertConfigurationIsValid();
            container.GetInstance<IHandleModelFilterStrategy<TestModel>>();
        }
    }
}
