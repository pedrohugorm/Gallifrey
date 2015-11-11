using System;
using System.Data.Entity;
using System.Diagnostics;
using FunctionalTest.Domain;
using Gallifrey.Persistence.Application.Persistence;
using Gallifrey.Persistence.Application.Strategy;
using Gallifrey.SharedKernel.Application.Ioc;
using Gallifrey.SharedKernel.Application.Persistence;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Gallifrey.SharedKernel.Application.Persistence.Strategy;
using Gallifrey.SharedKernel.Application.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using StructureMap.Graph;

namespace FunctionalTest
{
    public class FakeDbContext : DbContext
    {
        public DbSet<TestModel> Tests { set; get; }
    }

    public class DefaultTestIocConfiguration : IConfigureContainer
    {
        public Action<ConfigurationExpression> Configure()
        {
            return x =>
            {
                x.Scan(s =>
                {
                    s.TheCallingAssembly();
                    s.RegisterConcreteTypesAgainstTheFirstInterface();
                    s.AddAllTypesOf(typeof (IValidationStrategy<>));
                    s.AddAllTypesOf(typeof (IPersistItem<,>));
                    s.AddAllTypesOf(typeof (IRetrieveItemByIdentity<,>));
                    s.AddAllTypesOf(typeof (IRetrieveQueryOfItems<>));
                    s.AddAllTypesOf(typeof (IRepository<,>));
                    s.AddAllTypesOf(typeof (IDatabaseRepository<,>));
                    s.AddAllTypesOf(typeof (IIdentity<>));
                    s.AddAllTypesOf(typeof (DbSet<>));
                });

                x.For<DbContext>().Use<FakeDbContext>();

                x.For(typeof(IRepository<,>)).Use(typeof(DatabaseRepository<,>));

                x.For<IPersistenceConfigurationProvider>().Use<DefaultPersistenceConfiguration>();

                x.For(typeof (IHandleModelFilterStrategy<>)).Use(typeof (NullHandleModelFilterStrategy<>));
                x.For(typeof (IAddItemStrategy<,>)).Use(typeof (DefaultAddItemStrategy<,>));
                x.For(typeof (IUpdateItemStrategy<,>)).Use(typeof (DefaultUpdateItemStrategy<,>));
                x.For(typeof (IRemoveItemStrategy<,>)).Use(typeof (DefaultRemoveItemStrategy<,>));
            };
        }
    }

    [TestClass]
    public class IocContainerTest
    {
        [TestMethod]
        public void ShouldLoadIocContainerAndGetType()
        {
            var container = new Container();
            container.Configure(new DefaultTestIocConfiguration().Configure());

            var model = new TestModel();

            var instanceRepository = container.GetInstance<IRepository<TestModel, Guid>>();
            instanceRepository.InsertOrUpdate(model);
            instanceRepository.Save();

            Debug.WriteLine(model.Id);

            Assert.IsNotNull(model.Id);
        }
    }
}
