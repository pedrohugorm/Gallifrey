using FluentValidation;
using Gallifrey.SharedKernel.Application.Configuration;
using Gallifrey.SharedKernel.Application.Persistence;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Gallifrey.RestApi.Application.Configuration
{
    public class BaseRegistry : Registry
    {
        public BaseRegistry()
        {
            Scan(s =>
            {
                s.WithDefaultConventions();
                s.TheCallingAssembly();
                s.RegisterConcreteTypesAgainstTheFirstInterface();
                s.AddAllTypesOf(typeof (IPersistItem<,>));
                s.AddAllTypesOf(typeof (IRetrieveItemByIdentity<,>));
                s.AddAllTypesOf(typeof (IRetrieveQueryOfItems<>));
                s.AddAllTypesOf(typeof (IRepository<,>));
                s.AddAllTypesOf(typeof (IDatabaseRepository<,>));
                s.AddAllTypesOf(typeof (IIdentity<>));
                s.AddAllTypesOf(typeof (IHandleEntityChanged<>));
                s.AddAllTypesOf(typeof (IHandleEntityChanging<>));
                s.ConnectImplementationsToTypesClosing(typeof (IHandleEntityChanged<>));
                s.ConnectImplementationsToTypesClosing(typeof (IHandleEntityChanging<>));
                s.AddAllTypesOf<IRegisterMapping>();

                s.AddAllTypesOf(typeof (IValidator<>));
                s.AddAllTypesOf(typeof (AbstractValidator<>));
                s.ConnectImplementationsToTypesClosing(typeof (AbstractValidator<>));
                s.ConnectImplementationsToTypesClosing(typeof (IValidator<>));
            });

            For<IPersistenceConfigurationProvider>().UseIfNone<DefaultPersistenceConfiguration>();
        }
    }
}